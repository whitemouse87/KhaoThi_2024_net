using AutoMapper;
using Blazored.LocalStorage;
using KhaoThi_2024_net_client.Models.Auth;
using KhaoThi_2024_net_client.Models.Users;
using KhaoThi_2024_net_client.Services.Auth;
using KhaoThi_2024_net_client.Services.Logging;
using KhaoThi_2024_net_client.Services.User;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace KhaoThi_2024_net_client.Components.Auth
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IAuthService _authService;
        private readonly ILoggingService _loggingService;
        private AuthenticationState? _lastAuthState;
        private const string AUTH_TOKEN_KEY = "authToken";
        private const string REFRESH_TOKEN_KEY = "refreshToken";  // Thêm dòng này
        private readonly IUserService _khaoThiUserService;
        private readonly IMapper _mapper;
        public CustomAuthStateProvider(
            ILocalStorageService localStorage,
            IAuthService authService,
            ILoggingService loggingService,
            IUserService khaoThiUserService,
            IMapper mapper
            )
        {
            _localStorage = localStorage;
            _authService = authService;
            _loggingService = loggingService;
            _khaoThiUserService = khaoThiUserService;
            _mapper = mapper;

        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>(AUTH_TOKEN_KEY);
                if (string.IsNullOrEmpty(token))
                {
                    _lastAuthState = CreateAnonymousState();
                    return _lastAuthState;
                }

                if (_authService.IsTokenExpired(token))
                {
                    var refreshToken = await _localStorage.GetItemAsync<string>(REFRESH_TOKEN_KEY);
                    if (!string.IsNullOrEmpty(refreshToken))
                    {
                        var refreshResult = await _authService.RefreshToken();
                        if (refreshResult.Success && refreshResult.Token != null)
                        {
                            token = refreshResult.Token;

                            await _localStorage.SetItemAsync(AUTH_TOKEN_KEY, token);
                            await _localStorage.SetItemAsync(REFRESH_TOKEN_KEY, refreshResult.RefreshToken);
                        }
                        else
                        {
                            // Refresh token thất bại. Xử lý lỗi ở đây.
                            await HandleTokenExpired(); // Xóa token và refresh token cũ
                            await Logger.Error($"Refresh token failed: {refreshResult?.ErrorMessage}"); // Ghi log lỗi (nếu có)
                            return CreateAnonymousState();
                        }
                    }
                    else
                    {
                      //  await HandleTokenExpired();
                        return CreateAnonymousState();
                    }
                }

                var validateResult = await _authService.ValidateToken();
                if (!validateResult.IsValid)
                {
                    await HandleTokenExpired();
                    return CreateAnonymousState();
                }

                // Lấy thông tin người dùng mới nhất
                var userInfo = await GetLatestUserInfo(token); // Hàm mới
                if (userInfo == null)
                {
                    await HandleTokenExpired(); // Xử lý nếu userInfo là null
                    _lastAuthState = CreateAnonymousState(); // Cập nhật _lastAuthState trước khi trả về
                    return _lastAuthState; // Trả về trạng thái ẩn danh
                }


                var claims = BuildUserClaims(userInfo);
                // Log claims để debug
                foreach (var claim in claims)
                {
                    await Logger.Info($"Claim - Type: {claim.Type}, Value: {claim.Value}");
                }
                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);
                var newAuthState = new AuthenticationState(user);

                if (ShouldUpdateAuthState(_lastAuthState, newAuthState))
                {
                    await Logger.Info($"Auth state updated - User: {userInfo.TenDangNhap}, Role: {userInfo.MaChucVu}");
                    _lastAuthState = newAuthState;
                    NotifyAuthenticationStateChanged(Task.FromResult(newAuthState));
                }

                return newAuthState;
            }
            catch (Exception ex)
            {
                await Logger.Error("Error in GetAuthenticationStateAsync", ex);
                _lastAuthState = CreateAnonymousState();
                return _lastAuthState;
            }
        }
        private UserInfo MapKhaoThiUserToUserInfo(KhaoThiUserModel khaoThiUser)
        {
            try
            {
                var userInfo = _mapper.Map<UserInfo>(khaoThiUser);
                // Log để debug
                Console.WriteLine($"Mapping result - HoTen: {userInfo.HoTen}, TenDangNhap: {userInfo.TenDangNhap}");
                return userInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Mapping error: {ex.Message}");
                throw;
            }
        }
        private async Task<UserInfo?> GetLatestUserInfo(string token)
        {
            try
            {
                var userInfoFromToken = await _authService.GetUserInfo(token);
                await Logger.Info($"UserInfoFromToken: {System.Text.Json.JsonSerializer.Serialize(userInfoFromToken)}");

                if (userInfoFromToken != null)
                {
                    var khaoThiUser = await _khaoThiUserService.GetByIdAsync(userInfoFromToken.Id);
                    await Logger.Info($"KhaoThiUser: {System.Text.Json.JsonSerializer.Serialize(khaoThiUser)}");

                    if (khaoThiUser != null)
                    {
                        var userInfo = MapKhaoThiUserToUserInfo(khaoThiUser);
                        await Logger.Info($"Mapped UserInfo: {System.Text.Json.JsonSerializer.Serialize(userInfo)}");
                        return userInfo;
                    }
                    await Logger.Warning($"Không tìm thấy KhaoThiUser với ID: {userInfoFromToken.Id}");
                }
                await Logger.Warning("UserInfoFromToken is null");
                return null;
            }
            catch (Exception ex)
            {
                await Logger.Error("Lỗi trong GetLatestUserInfo", ex);
                return null;
            }
        }


        public async Task MarkUserAsAuthenticated(string token, string refreshToken)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentNullException(nameof(token));

            try
            {
                await _localStorage.SetItemAsync(AUTH_TOKEN_KEY, token);
                await _localStorage.SetItemAsync(REFRESH_TOKEN_KEY, refreshToken);
                var userInfo = await _authService.GetUserInfo(token);

                var claims = BuildUserClaims(userInfo);
                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);
                var authState = new AuthenticationState(user);

                _lastAuthState = authState;
                NotifyAuthenticationStateChanged(Task.FromResult(authState));

                await Logger.Info($"User authenticated successfully - {userInfo.TenDangNhap}");
            }
            catch (Exception ex)
            {
                await Logger.Error($"Authentication failed", ex);
                await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
                await _localStorage.RemoveItemAsync(REFRESH_TOKEN_KEY);
                throw;
            }
        }

        public async Task MarkUserAsLoggedOut()
        {
            try
            {
                var currentUser = string.Empty;
                var token = await _localStorage.GetItemAsync<string>(AUTH_TOKEN_KEY);
                if (!string.IsNullOrEmpty(token))
                {
                    var userInfo = await _authService.GetUserInfo(token);
                    currentUser = userInfo.TenDangNhap;
                }

                await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
                await _localStorage.RemoveItemAsync(REFRESH_TOKEN_KEY);
                var anonymousState = CreateAnonymousState();
                _lastAuthState = anonymousState;
                NotifyAuthenticationStateChanged(Task.FromResult(anonymousState));

                if (!string.IsNullOrEmpty(currentUser))
                {
                    await Logger.Info($"User logged out: {currentUser}");
                }
            }
            catch (Exception ex)
            {
                await Logger.Error("Logout failed", ex);
                throw;
            }
        }
        private List<Claim> BuildUserClaims(UserInfo userInfo)
        {
            var claims = new List<Claim>();

            if (userInfo.Id != 0)
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()));

            if (!string.IsNullOrEmpty(userInfo.TenDangNhap))
                claims.Add(new Claim(ClaimTypes.Name, userInfo.TenDangNhap));

            if (!string.IsNullOrEmpty(userInfo.HoTen))
            {
                claims.Add(new Claim("hoTen", userInfo.HoTen));
                // Log để debug
                Console.WriteLine($"Adding hoTen claim: {userInfo.HoTen}");
            }

            if (!string.IsNullOrEmpty(userInfo.MaDonVi))
                claims.Add(new Claim("maDonVi", userInfo.MaDonVi));

            if (!string.IsNullOrEmpty(userInfo.TenDonVi))
                claims.Add(new Claim("tenDonVi", userInfo.TenDonVi));

            if (!string.IsNullOrEmpty(userInfo.MaChucVu))
            {
                claims.Add(new Claim("maChucVu", userInfo.MaChucVu));
                if (userInfo.MaChucVu == "01")
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }
            }

            if (!string.IsNullOrEmpty(userInfo.Email))
                claims.Add(new Claim(ClaimTypes.Email, userInfo.Email));

            // Log tất cả claims để debug
            Console.WriteLine("All claims:");
            foreach (var claim in claims)
            {
                Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
            }

            return claims;
        }

        private bool ShouldUpdateAuthState(AuthenticationState? lastState, AuthenticationState newState)
        {
            if (lastState == null) return true;

            var lastUser = lastState.User;
            var newUser = newState.User;

            return lastUser.FindFirst(ClaimTypes.Name)?.Value != newUser.FindFirst(ClaimTypes.Name)?.Value ||
                   lastUser.FindFirst("maChucVu")?.Value != newUser.FindFirst("maChucVu")?.Value;
        }

        private AuthenticationState CreateAnonymousState()
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        private async Task HandleTokenExpired()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>(AUTH_TOKEN_KEY);
                if (!string.IsNullOrEmpty(token))
                {
                    var userInfo = await _authService.GetUserInfo(token);
                    await Logger.Warning($"Token expired for user: {userInfo.TenDangNhap}");
                }

                await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
                await _localStorage.RemoveItemAsync(REFRESH_TOKEN_KEY);
                var anonymousState = CreateAnonymousState();
                _lastAuthState = anonymousState;
                NotifyAuthenticationStateChanged(Task.FromResult(anonymousState));
            }
            catch (Exception ex)
            {
                await Logger.Error("Error handling token expiration", ex);
            }
        }
    }
}