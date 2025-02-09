using Blazored.LocalStorage;
using KhaoThi_2024_net_client.Models;
using KhaoThi_2024_net_client.Models.Auth;
using KhaoThi_2024_net_client.Services.Logging;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;

namespace KhaoThi_2024_net_client.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private const string AUTH_TOKEN_KEY = "authToken";
        private const string REFRESH_TOKEN_KEY = "refreshToken";
        public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
        public  bool IsTokenExpired(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                return jwtToken.ValidTo < DateTime.UtcNow;
            }
            catch (Exception ex)
            {
                Logger.Error($"[Error] Lỗi kiểm tra token hết hạn: {ex.Message}").Wait();
                return true;
            }
        }
        public async Task<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                //Console.WriteLine($"Gửi request tới: {_httpClient.BaseAddress}auth/login");
                var response = await _httpClient.PostAsJsonAsync("auth/login", request);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    return new LoginResponse
                    {
                        Success = false,
                        ErrorMessage = error?.Message ?? "Lỗi đăng nhập"
                    };
                }

                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                return loginResponse ?? new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Không thể đọc phản hồi từ server"
                };
            }
            catch (Exception ex)
            {
                await Logger.Error("Something went wrong", ex);
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Lỗi kết nối đến server"
                };
            }
        }

        public async Task<UserInfo> GetUserInfo(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentException("Token cannot be null or empty", nameof(token));

            try
            {
                var claims = ParseJwtClaims(token);

                if (!claims.ContainsKey("ID") || !claims.ContainsKey("TenDangNhap"))
                {
                    await Logger.Error("Token does not contain valid user information",null, this.GetType().Name);
                    throw new InvalidOperationException("Token does not contain valid user information");
                    
                }
                   

                return new UserInfo
                {
                    ID = int.Parse(claims["ID"]),
                    TenDangNhap = claims["TenDangNhap"],
                    HoTen = claims.GetValueOrDefault("HoTen", string.Empty),
                    MaDonVi = claims.GetValueOrDefault("MaDonVi", string.Empty),
                    MaChucVu = claims.GetValueOrDefault("MaChucVu", string.Empty)
                };
            }
            catch (Exception ex)
            {
              
                await Logger.Error($"[Error] Failed to parse token:"+ ex.Message,ex, this.GetType().Name);
                throw new InvalidOperationException("Invalid token", ex);
            }
        }


        public async Task Logout()
        {
            try
            {
                await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
                await _localStorage.RemoveItemAsync(REFRESH_TOKEN_KEY);
                await Logger.Info($"[Info] Tokens removed from local storage");

            }
            catch (Exception ex)
            {
               
                await Logger.Error($"[Error] Failed to logout:" + ex.Message, ex, this.GetType().Name);
                throw new InvalidOperationException("Logout failed", ex);
            }
        }

        public async Task<ValidateTokenResponse> ValidateToken()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>(AUTH_TOKEN_KEY);

                if (string.IsNullOrWhiteSpace(token))
                    return new ValidateTokenResponse { IsValid = false };

                var userInfo = await GetUserInfo(token);

                return new ValidateTokenResponse
                {
                    IsValid = true,
                    UserClaims = new Dictionary<string, string>
                    {
                        { "ID", userInfo.ID.ToString() },
                        { "TenDangNhap", userInfo.TenDangNhap },
                        { "HoTen", userInfo.HoTen ?? string.Empty },
                        { "MaDonVi", userInfo.MaDonVi ?? string.Empty },
                        { "MaChucVu", userInfo.MaChucVu ?? string.Empty }
                    }
                };
            }
            catch (Exception ex)
            {
               
                await Logger.Error($"[Error] Token validation failed:" + ex.Message,ex,this.GetType().Name);
                return new ValidateTokenResponse { IsValid = false };
            }
        }

      

        private Dictionary<string, string> ParseJwtClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims.ToDictionary(
                c => c.Type switch
                {
                    "nameid" => "ID",
                    "unique_name" => "TenDangNhap",
                    "MaDonVi" => "MaDonVi",
                    "MaChucVu" => "MaChucVu",
                    "HoTen" => "HoTen",
                    _ => c.Type.ToLower()
                },
                c => c.Value
            );
        }
        public async Task<int?> GetUserIdFromToken()
        {
            //try
            //{
            //    var token = await _localStorage.GetItemAsync<string>(AUTH_TOKEN_KEY);
            //    if (string.IsNullOrWhiteSpace(token)) return null;

            //    // Kiểm tra token hết hạn
            //    if (IsTokenExpired(token))
            //    {
            //        // Thử refresh token
            //        var refreshResult = await RefreshToken();
            //        if (!refreshResult.Success)
            //        {
            //            await Logout();
            //            return null;
            //        }
            //        token = refreshResult.Token;
            //    }

            //    var claims = ParseJwtClaims(token);
            //    return claims.ContainsKey("ID") ? int.Parse(claims["id"]) : null;
            //}
            //catch (Exception ex)
            //{
            //    await Logger.Error($"[Error] Lỗi lấy UserId từ token: {ex.Message}",ex,this.GetType().Name);
            //    return null;
            //}
            try
            {
                var token = await _localStorage.GetItemAsync<string>(AUTH_TOKEN_KEY);
                Console.WriteLine($"Token exists: {!string.IsNullOrEmpty(token)}"); // Debug log

                if (string.IsNullOrWhiteSpace(token))
                {
                    Console.WriteLine("Token is empty"); // Debug log
                    return null;
                }

                // Kiểm tra token hết hạn
                if (IsTokenExpired(token))
                {
                    Console.WriteLine("Token is expired, trying to refresh"); // Debug log
                    var refreshResult = await RefreshToken();
                    if (!refreshResult.Success)
                    {
                        Console.WriteLine("Token refresh failed"); // Debug log
                        await Logout();
                        return null;
                    }
                    token = refreshResult.Token;
                }

                var claims = ParseJwtClaims(token);
                Console.WriteLine($"Claims found: {string.Join(", ", claims.Keys)}"); // Debug log

                // Thay đổi từ "id" thành "ID" để match với ParseJwtClaims
                if (claims.ContainsKey("ID"))
                {
                    var userId = int.Parse(claims["ID"]);
                    Console.WriteLine($"Found userId: {userId}"); // Debug log
                    return userId;
                }

                Console.WriteLine("ID claim not found in token"); // Debug log
                return null;
            }
            catch (Exception ex)
            {
                await Logger.Error($"[Error] Lỗi lấy UserId từ token: {ex.Message}", ex, nameof(AuthService));
                Console.WriteLine($"Error in GetUserIdFromToken: {ex.Message}"); // Debug log
                return null;
            }
        }
        public async Task<LoginResponse> RefreshToken()
        {
            try
            {
                var refreshToken = await _localStorage.GetItemAsync<string>(REFRESH_TOKEN_KEY);
                if (string.IsNullOrEmpty(refreshToken))
                {
                    return new LoginResponse
                    {
                        Success = false,
                        ErrorMessage = "Không tìm thấy refresh token"
                    };
                }

                var response = await _httpClient.PostAsJsonAsync("auth/refresh-token",
                    new { RefreshToken = refreshToken });

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    if (result?.Success == true)
                    {
                        await _localStorage.SetItemAsync(AUTH_TOKEN_KEY, result.Token);
                        await _localStorage.SetItemAsync(REFRESH_TOKEN_KEY, result.RefreshToken);
                        return result;
                    }
                }

                // Nếu refresh thất bại, xóa tokens
                await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
                await _localStorage.RemoveItemAsync(REFRESH_TOKEN_KEY);
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Không thể làm mới token"
                };
            }
            catch (Exception ex)
            {
                await Logger.Error($"[Error] Lỗi refresh token: {ex.Message}");
                return new LoginResponse
                {
                    Success = false,
                    ErrorMessage = "Lỗi khi làm mới token"
                };
            }
        }
    }
}
