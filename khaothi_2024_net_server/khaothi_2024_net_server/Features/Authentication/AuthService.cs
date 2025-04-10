using Azure.Core;
using khaothi_2024_net_server.Core.Exceptions;
using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.Authentication.DTOs;
using khaothi_2024_net_server.Features.UserManagement.DTOs;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace khaothi_2024_net_server.Features.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IKhaoThiUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthService> _logger;
        private readonly IDistributedCache _cache;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(
            IKhaoThiUserRepository userRepository,
            IConfiguration configuration,
            ILogger<AuthService> logger,
            IDistributedCache cache,
            IHttpContextAccessor httpContextAccessor,
            IPasswordHasher passwordHasher
            )
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _logger = logger;
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _passwordHasher = passwordHasher;
        }
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                if (!await CheckLoginAttempts(request.TenDangNhap))
                {
                    return new LoginResponse
                    {
                        Success = false,
                        ErrorType = LoginErrorType.TooManyAttempts,
                        ErrorMessage = "Quá nhiều lần đăng nhập thất bại. Vui lòng thử lại sau."
                    };
                }

                var user = await _userRepository.GetByUsernameAsync(request.TenDangNhap);
                if (user == null)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        ErrorType = LoginErrorType.InvalidCredentials,
                        ErrorMessage = "Tên đăng nhập không tồn tại"
                    };
                }
                //_logger.LogError(_passwordHasher.HashPassword(request.MatKhau));
                // Verify password (assuming password is hashed)
                if (!_passwordHasher.VerifyPassword(request.MatKhau, user.MatKhau))
                {


                    return new LoginResponse
                    {
                        Success = false,
                        ErrorType = LoginErrorType.InvalidCredentials,
                        //ErrorMessage = "Mật khẩu không đúng:" + request.MatKhau.ToString() + "-" + user.MatKhau
                        ErrorMessage = "Mật khẩu không đúng"
                    };
                }

                if (!user.Active)
                {
                    return new LoginResponse
                    {
                        Success = false,
                        ErrorType = LoginErrorType.AccountInactive,
                        ErrorMessage = "Tài khoản không hoạt động"
                    };
                }

                // Kiểm soát các phiên đồng thời
                //if (!await CheckConcurrentSessionsAsync(user.ID.ToString(), 3)) // Giới hạn 3 phiên đồng thời
                //{
                //    return new LoginResponse
                //    {
                //        Success = false,
                //        ErrorType = LoginErrorType.TooManyAttempts,
                //        ErrorMessage = "Quá nhiều phiên hoạt động. Vui lòng đăng xuất khỏi các thiết bị khác."
                //    };
                //}

                var token = GenerateJwtToken(user);
                var refreshToken = GenerateRefreshToken();

                // Lưu refresh token vào database nếu cần
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await _userRepository.UpdateAsync(user);

                // Lưu thông tin phiên
                await StoreUserSessionAsync(user.ID.ToString(), token);

                // Reset login attempts on successful login
                await ResetLoginAttempts(request.TenDangNhap);

                return new LoginResponse
                {
                    Success = true,
                    Token = token,
                    RefreshToken = refreshToken,
                    User = new UserDto
                    {
                        ID = user.ID,
                        TenDangNhap = user.TenDangNhap,
                        HoTen = user.HoTen,
                        MaDonVi = user.MaDonVi,
                        MaChucVu = user.MaChucVu
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for user {Username}", request.TenDangNhap);
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Đã xảy ra lỗi trong quá trình đăng nhập: " + ex.Message
                };
            }
        }

        private string GenerateJwtToken(KhaoThiUser user)
        {
            var secretKey = _configuration["Jwt:SecretKey"];
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("JWT Secret Key is not configured.");
            }

            // Thêm device info vào claims
            var deviceInfo = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();
            var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.ID.ToString()),
            new Claim(ClaimTypes.Name, user.TenDangNhap),
            new Claim("MaDonVi", user.MaDonVi),
            new Claim("MaChucVu", user.MaChucVu),

            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("DeviceInfo", deviceInfo ?? "unknown"),
            new Claim("IpAddress", ipAddress ?? "unknown")
        }),
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
                NotBefore = DateTime.UtcNow // Thêm thời điểm bắt đầu có hiệu lực
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public async Task<bool> LogoutAsync(int userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    // 1. Xử lý refresh token
                    // Thay vì gán null, gán chuỗi rỗng cho RefreshToken
                    user.RefreshToken = string.Empty;
                    user.RefreshTokenExpiryTime = DateTime.UtcNow; // Đặt thời gian hết hạn về hiện tại
                    await _userRepository.UpdateAsync(user);

                    // 2. Xử lý token hiện tại
                    var currentToken = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
                        .ToString().Replace("Bearer ", "");

                    if (!string.IsNullOrEmpty(currentToken))
                    {
                        // 3. Thêm token vào blacklist
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var jwtToken = tokenHandler.ReadJwtToken(currentToken);
                        var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;

                        if (!string.IsNullOrEmpty(jti))
                        {
                            // Lưu token vào blacklist với thời gian hết hạn
                            await _cache.SetStringAsync(
                                $"blacklist_{jti}",
                                "revoked",
                                new DistributedCacheEntryOptions
                                {
                                    AbsoluteExpiration = jwtToken.ValidTo
                                });

                            // 4. Xóa phiên hiện tại
                            await RemoveUserSessionAsync(userId.ToString(), currentToken);
                        }
                    }

                    // Log thông tin đăng xuất thành công
                    _logger.LogInformation("User {UserId} logged out successfully", userId);
                    return true;
                }

                _logger.LogWarning("Logout attempted for non-existent user {UserId}", userId);
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout failed for user {UserId}", userId);
                return false;
            }
        }

        public async Task<LoginResponse> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userRepository.GetByRefreshTokenAsync(refreshToken);

            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                throw new UnauthorizedException("Invalid refresh token");
            }

            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userRepository.UpdateAsync(user);

            return new LoginResponse
            {
                Token = newAccessToken,
                RefreshToken = newRefreshToken,
                User = new UserDto
                {
                    ID = user.ID,
                    TenDangNhap = user.TenDangNhap,
                    HoTen = user.HoTen,
                    MaDonVi = user.MaDonVi,
                    MaChucVu = user.MaChucVu
                }
            };
        }

        private async Task<bool> CheckLoginAttempts(string username)
        {
            var cacheKey = $"login_attempts_{username}";
            var attemptsString = await _cache.GetStringAsync(cacheKey);
            int attempts = !string.IsNullOrEmpty(attemptsString) ? int.Parse(attemptsString) : 0;

            // Thêm kiểm tra IP để ngăn chặn tấn công từ một địa chỉ IP
            var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();
            var ipCacheKey = $"login_attempts_ip_{ipAddress}";
            var ipAttemptsString = await _cache.GetStringAsync(ipCacheKey);
            int ipAttempts = !string.IsNullOrEmpty(ipAttemptsString) ? int.Parse(ipAttemptsString) : 0;

            if (attempts >= 5 || ipAttempts >= 10)
            {
                _logger.LogWarning("Too many login attempts for user {Username} from IP {IpAddress}",
                    username, ipAddress);
                return false;
            }

            // Cập nhật cả username và IP attempts
            await Task.WhenAll(
                _cache.SetStringAsync(
                    cacheKey,
                    (attempts + 1).ToString(),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    }
                ),
                _cache.SetStringAsync(
                    ipCacheKey,
                    (ipAttempts + 1).ToString(),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                    }
                )
            );

            return true;
        }

        private async Task ResetLoginAttempts(string username)
        {
            await _cache.RemoveAsync($"login_attempts_{username}");
        }

        private async Task StoreUserSessionAsync(string userId, string token)
        {
            var cacheKey = $"user_sessions_{userId}";
            var sessions = await _cache.GetStringAsync(cacheKey);
            var sessionList = string.IsNullOrEmpty(sessions) ? new List<string>() : JsonConvert.DeserializeObject<List<string>>(sessions);

            sessionList.Add(token);
            await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(sessionList), new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7) // Thời gian hết hạn của phiên
            });
        }

        private async Task RemoveUserSessionAsync(string userId, string token)
        {
            var cacheKey = $"user_sessions_{userId}";
            var sessions = await _cache.GetStringAsync(cacheKey);
            if (!string.IsNullOrEmpty(sessions))
            {
                var sessionList = JsonConvert.DeserializeObject<List<string>>(sessions);
                sessionList.Remove(token);
                await _cache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(sessionList), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
                });
            }
        }

        public async Task<bool> LogoutAllDevicesAsync(int userId)
        {
            try
            {
                // 1. Xóa tất cả sessions
                var cacheKey = $"user_sessions_{userId}";
                var sessions = await _cache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(sessions))
                {
                    var sessionList = JsonConvert.DeserializeObject<List<string>>(sessions);
                    foreach (var token in sessionList)
                    {
                        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                        var jti = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
                        if (!string.IsNullOrEmpty(jti))
                        {
                            await _cache.SetStringAsync(
                                $"blacklist_{jti}",
                                "revoked",
                                new DistributedCacheEntryOptions
                                {
                                    AbsoluteExpiration = jwtToken.ValidTo
                                });
                        }
                    }
                    await _cache.RemoveAsync(cacheKey);
                }

                // 2. Reset refresh token
                var user = await _userRepository.GetByIdAsync(userId);
                if (user != null)
                {
                    user.RefreshToken = null;
                    user.RefreshTokenExpiryTime = null;
                    await _userRepository.UpdateAsync(user);
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Logout all devices failed for user {UserId}", userId);
                return false;
            }
        }

        private async Task<bool> CheckConcurrentSessionsAsync(string userId, int maxSessions)
        {
            var cacheKey = $"user_sessions_{userId}";
            var sessions = await _cache.GetStringAsync(cacheKey);
            var sessionList = string.IsNullOrEmpty(sessions)
                ? new List<string>()
                : JsonConvert.DeserializeObject<List<string>>(sessions);

            // Xóa các session hết hạn trước khi kiểm tra
            var tokenHandler = new JwtSecurityTokenHandler();
            sessionList = sessionList.Where(token =>
            {
                try
                {
                    var jwt = tokenHandler.ReadJwtToken(token);
                    return jwt.ValidTo > DateTime.UtcNow;
                }
                catch
                {
                    return false;
                }
            }).ToList();

            if (sessionList.Count >= maxSessions)
            {
                _logger.LogWarning("Max concurrent sessions reached for user {UserId}", userId);
                return false;
            }

            // Cập nhật danh sách session đã làm sạch
            await _cache.SetStringAsync(
                cacheKey,
                JsonConvert.SerializeObject(sessionList),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7)
                }
            );

            return true;
        }
    }
}
