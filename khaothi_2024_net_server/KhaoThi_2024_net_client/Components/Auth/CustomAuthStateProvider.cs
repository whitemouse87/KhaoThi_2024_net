//using Blazored.LocalStorage;
//using KhaoThi_2024_net_client.Services.Auth;
//using KhaoThi_2024_net_client.Services.Logging;
//using Microsoft.AspNetCore.Components.Authorization;
//using System.Security.Claims;

//namespace KhaoThi_2024_net_client.Components.Auth
//{
//    public class CustomAuthStateProvider : AuthenticationStateProvider
//    {
//        private readonly ILocalStorageService _localStorage;
//        private readonly IAuthService _authService;
//        private readonly ILoggingService _loggingService;
//        private const string AUTH_TOKEN_KEY = "authToken";

//        public CustomAuthStateProvider(
//            ILocalStorageService localStorage,
//            IAuthService authService,
//            ILoggingService loggingService)
//        {
//            _localStorage = localStorage;
//            _authService = authService;
//            _loggingService = loggingService;
//        }

//        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//        {
//            try
//            {
//                var token = await _localStorage.GetItemAsync<string>(AUTH_TOKEN_KEY);
//                if (string.IsNullOrEmpty(token))
//                {
//                    await Logger.Info("No token found - returning anonymous state");
//                    return CreateAnonymousState();
//                }

//                // Luôn validate token và lấy thông tin mới từ server
//                var validateResult = await _authService.ValidateToken();
//                if (!validateResult.IsValid)
//                {
//                    await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
//                    await Logger.Warning("Invalid token - clearing authentication");
//                    await HandleTokenExpired();
//                    return CreateAnonymousState();
//                }

//                // Lấy thông tin user mới nhất từ server
//                var userInfo = await _authService.GetUserInfo(token);

//                var claims = new List<Claim>
//                {
//                    new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
//                    new Claim(ClaimTypes.Name, userInfo.TenDangNhap),
//                    new Claim("hoTen", userInfo.HoTen ?? string.Empty),
//                    new Claim("maDonVi", userInfo.MaDonVi ?? string.Empty),
//                    new Claim("maChucVu", userInfo.MaChucVu ?? string.Empty)
//                };

//                var identity = new ClaimsIdentity(claims, "jwt");
//                var user = new ClaimsPrincipal(identity);

//                await Logger.Info($"Refreshed auth state for user: {userInfo.TenDangNhap} with role: {userInfo.MaChucVu}");

//                return new AuthenticationState(user);
//            }
//            catch (Exception ex)
//            {
//                await Logger.Error("Error in GetAuthenticationStateAsync", ex);
//                return CreateAnonymousState();
//            }
//        }

//        public async Task MarkUserAsAuthenticated(string token)
//        {
//            try
//            {
//                // Lưu token trước
//                await _localStorage.SetItemAsync(AUTH_TOKEN_KEY, token);

//                var userInfo = await _authService.GetUserInfo(token);
//                var claims = new List<Claim>
//                {
//                    new Claim("id", userInfo.Id.ToString()),
//                    new Claim("tenDangNhap", userInfo.TenDangNhap),
//                    new Claim("hoTen", userInfo.HoTen ?? string.Empty),
//                    new Claim("maDonVi", userInfo.MaDonVi ?? string.Empty),
//                    new Claim("maChucVu", userInfo.MaChucVu ?? string.Empty)
//                };
//                // Thêm role dựa trên mã chức vụ
//                if (userInfo.MaChucVu == Constants.Roles.Admin)
//                {
//                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
//                }

//                var identity = new ClaimsIdentity(claims, "jwt");
//                var user = new ClaimsPrincipal(identity);
//                var authState = new AuthenticationState(user);

//                await Logger.Info($"User marked as authenticated: {userInfo.TenDangNhap} - {userInfo.HoTen}");

//                NotifyAuthenticationStateChanged(Task.FromResult(authState));
//            }
//            catch (Exception ex)
//            {
//                await Logger.Error($"Error in MarkUserAsAuthenticated", ex);
//                await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
//                throw;
//            }
//        }

//        public async Task MarkUserAsLoggedOut()
//        {
//            try
//            {
//                var token = await _localStorage.GetItemAsync<string>(AUTH_TOKEN_KEY);
//                if (!string.IsNullOrEmpty(token))
//                {
//                    var userInfo = await _authService.GetUserInfo(token);
//                    await Logger.Info($"User logged out: {userInfo.TenDangNhap}");
//                }

//                await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
//                var authState = CreateAnonymousState();
//                NotifyAuthenticationStateChanged(Task.FromResult(authState));
//            }
//            catch (Exception ex)
//            {
//                await Logger.Error("Error in MarkUserAsLoggedOut", ex);
//                throw;
//            }
//        }

//        private AuthenticationState CreateAnonymousState()
//        {
//            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
//        }

//        private async Task HandleTokenExpired()
//        {
//            try
//            {
//                var token = await _localStorage.GetItemAsync<string>(AUTH_TOKEN_KEY);
//                if (!string.IsNullOrEmpty(token))
//                {
//                    var userInfo = await _authService.GetUserInfo(token);
//                    await Logger.Warning($"Token expired for user: {userInfo.TenDangNhap}");
//                }

//                await MarkUserAsLoggedOut();
//                // Có thể thêm thông báo cho người dùng thông qua service
//            }
//            catch (Exception ex)
//            {
//                await Logger.Error("Error handling token expiration", ex);
//            }
//        }
//    }
//}

using Blazored.LocalStorage;
using KhaoThi_2024_net_client.Models.Auth;
using KhaoThi_2024_net_client.Services.Auth;
using KhaoThi_2024_net_client.Services.Logging;
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

        public CustomAuthStateProvider(
            ILocalStorageService localStorage,
            IAuthService authService,
            ILoggingService loggingService)
        {
            _localStorage = localStorage;
            _authService = authService;
            _loggingService = loggingService;
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

                var validateResult = await _authService.ValidateToken();
                if (!validateResult.IsValid)
                {
                    await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
                    await HandleTokenExpired();
                    _lastAuthState = CreateAnonymousState();
                    return _lastAuthState;
                }

                var userInfo = await _authService.GetUserInfo(token);
                var claims = BuildUserClaims(userInfo);
                var identity = new ClaimsIdentity(claims, "jwt");
                var user = new ClaimsPrincipal(identity);
                var newAuthState = new AuthenticationState(user);

                // Chỉ notify và log khi có thay đổi thực sự
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

        public async Task MarkUserAsAuthenticated(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentNullException(nameof(token));
            }

            try
            {
                await _localStorage.SetItemAsync(AUTH_TOKEN_KEY, token);
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
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userInfo.Id.ToString()),
                new Claim(ClaimTypes.Name, userInfo.TenDangNhap),
                new Claim("hoTen", userInfo.HoTen ?? string.Empty),
                new Claim("maDonVi", userInfo.MaDonVi ?? string.Empty),
                new Claim("maChucVu", userInfo.MaChucVu ?? string.Empty)
            };

            // Thêm role cho admin
            if (userInfo.MaChucVu == "01")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
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