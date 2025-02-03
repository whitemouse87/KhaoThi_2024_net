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

        public AuthService(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
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

                if (!claims.ContainsKey("id") || !claims.ContainsKey("tenDangNhap"))
                    throw new InvalidOperationException("Token does not contain valid user information");

                return new UserInfo
                {
                    Id = int.Parse(claims["id"]),
                    TenDangNhap = claims["tenDangNhap"],
                    HoTen = claims.GetValueOrDefault("hoTen", string.Empty),
                    MaDonVi = claims.GetValueOrDefault("maDonVi", string.Empty),
                    MaChucVu = claims.GetValueOrDefault("maChucVu", string.Empty)
                };
            }
            catch (Exception ex)
            {
              
                await Logger.Error($"[Error] Failed to parse token:"+ ex.Message);
                throw new InvalidOperationException("Invalid token", ex);
            }
        }


        public async Task Logout()
        {
            try
            {
                await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
                await Logger.Info($"[Info] Token removed from local storage");
               
            }
            catch (Exception ex)
            {
                await Logger.Error($"[Error] Failed to logout:" + ex.Message);
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
                        { "id", userInfo.Id.ToString() },
                        { "tenDangNhap", userInfo.TenDangNhap },
                        { "hoTen", userInfo.HoTen ?? string.Empty },
                        { "maDonVi", userInfo.MaDonVi ?? string.Empty },
                        { "maChucVu", userInfo.MaChucVu ?? string.Empty }
                    }
                };
            }
            catch (Exception ex)
            {
               
                await Logger.Error($"[Error] Token validation failed:" + ex.Message);
                return new ValidateTokenResponse { IsValid = false };
            }
        }

        private async Task<LoginResponse> HandleSuccessfulLogin(HttpResponseMessage response)
        {
            var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
            if (!string.IsNullOrEmpty(loginResponse?.Token))
            {
                await _localStorage.SetItemAsync(AUTH_TOKEN_KEY, loginResponse.Token);
                return new LoginResponse { Success = true, Token = loginResponse.Token };
            }

            return new LoginResponse
            {
                Success = false,
                ErrorMessage = "Invalid token received from server"
            };
        }

        private async Task<LoginResponse> HandleTooManyRequestsError(HttpResponseMessage response)
        {
            var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return new LoginResponse
            {
                Success = false,
                ErrorMessage = "Too many requests. Please try again later.",
                ErrorType = errorContent?.ErrorType ?? "UnknownError"
            };
        }

        private async Task<LoginResponse> HandleUnsuccessfulResponse(HttpResponseMessage response)
        {
            var errorContent = await response.Content.ReadFromJsonAsync<ErrorResponse>();
            return new LoginResponse
            {
                Success = false,
                ErrorMessage = "Login failed",
                ErrorType = errorContent?.ErrorType ?? "UnknownError"
            };
        }

        private LoginResponse HandleLoginException(Exception ex)
        {
            return new LoginResponse
            {
                Success = false,
                ErrorMessage = "Connection error",
                ErrorType = "ConnectionError"
            };
        }

        private Dictionary<string, string> ParseJwtClaims(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            return jwtToken.Claims.ToDictionary(
                c => c.Type switch
                {
                    "nameid" => "id",
                    "unique_name" => "tenDangNhap",
                    "MaDonVi" => "maDonVi",
                    "MaChucVu" => "maChucVu",
                    "HoTen" => "hoTen",
                    _ => c.Type.ToLower()
                },
                c => c.Value
            );
        }
        public async Task<int?> GetUserIdFromToken()
        {
            var token = await _localStorage.GetItemAsync<string>(AUTH_TOKEN_KEY);
            if (string.IsNullOrWhiteSpace(token)) return null;

            try
            {
                var claims = ParseJwtClaims(token);
                return claims.ContainsKey("id") ? int.Parse(claims["id"]) : null;
            }
            catch (Exception ex)
            {
                await Logger.Error($"[Error] Lỗi lấy dữ liệu UserId from token: {ex.Message}");
                return null;
            }
        }
    }
}
