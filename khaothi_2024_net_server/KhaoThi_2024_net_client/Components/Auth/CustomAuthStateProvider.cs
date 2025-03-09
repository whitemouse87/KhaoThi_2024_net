
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
    public class CustomAuthStateProvider : AuthenticationStateProvider, IDisposable
    {
        private readonly ILocalStorageService _localStorage;
        private readonly IAuthService _authService;
        private readonly IUserService _khaoThiUserService;
        private readonly IMapper _mapper;
        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private Task<AuthenticationState>? _authenticationStateTask;
        private UserInfo? _cachedUserInfo;
        private DateTime _userInfoCacheTime;
        private const int USER_INFO_CACHE_MINUTES = 5;
        private const string AUTH_TOKEN_KEY = "authToken";
        private const string REFRESH_TOKEN_KEY = "refreshToken";

        public CustomAuthStateProvider(
            ILocalStorageService localStorage,
            IAuthService authService,
            IUserService khaoThiUserService,
            IMapper mapper)
        {
            _localStorage = localStorage;
            _authService = authService;
            _khaoThiUserService = khaoThiUserService;
            _mapper = mapper;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (_authenticationStateTask != null)
            {
                return await _authenticationStateTask;
            }

            try
            {
                await _semaphore.WaitAsync();

                if (_authenticationStateTask != null)
                {
                    return await _authenticationStateTask;
                }

                _authenticationStateTask = GetAuthenticationStateInternalAsync();
                return await _authenticationStateTask;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        private async Task<AuthenticationState> GetAuthenticationStateInternalAsync()
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>(AUTH_TOKEN_KEY);
                if (string.IsNullOrEmpty(token))
                {
                    return CreateAnonymousState();
                }

                if (!await ValidateCurrentToken(token))
                {
                    return CreateAnonymousState();
                }

                var userInfo = await GetCachedUserInfo(token);
                if (userInfo == null)
                {
                    return CreateAnonymousState();
                }

                var claims = BuildUserClaims(userInfo);
                var identity = new ClaimsIdentity(claims, "jwt");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
            catch (Exception ex)
            {
                await Logger.Error("Lỗi trong GetAuthenticationStateInternalAsync", ex, GetType().Name);
                return CreateAnonymousState();
            }
            finally
            {
                _authenticationStateTask = null;
            }
        }

        private async Task<bool> ValidateCurrentToken(string token)
        {
            if (_authService.IsTokenExpired(token))
            {
                var refreshToken = await _localStorage.GetItemAsync<string>(REFRESH_TOKEN_KEY);
                if (!string.IsNullOrEmpty(refreshToken))
                {
                    var refreshResult = await _authService.RefreshToken();
                    if (refreshResult.Success && refreshResult.Token != null)
                    {
                        await _localStorage.SetItemAsync(AUTH_TOKEN_KEY, refreshResult.Token);
                        await _localStorage.SetItemAsync(REFRESH_TOKEN_KEY, refreshResult.RefreshToken);
                        return true;
                    }
                }
                await HandleTokenExpired();
                return false;
            }

            var validateResult = await _authService.ValidateToken();
            if (!validateResult.IsValid)
            {
                await HandleTokenExpired();
                return false;
            }

            return true;
        }

        private async Task<UserInfo?> GetCachedUserInfo(string token)
        {
            if (_cachedUserInfo != null && DateTime.Now.Subtract(_userInfoCacheTime).TotalMinutes < USER_INFO_CACHE_MINUTES)
            {
                return _cachedUserInfo;
            }

            var userInfo = await GetLatestUserInfo(token);
            if (userInfo != null)
            {
                _cachedUserInfo = userInfo;
                _userInfoCacheTime = DateTime.Now;
            }

            return userInfo;
        }

        private async Task<UserInfo?> GetLatestUserInfo(string token)
        {
            try
            {
                var userInfoFromToken = await _authService.GetUserInfo(token);
                if (userInfoFromToken?.ID == 0)
                {
                    await Logger.Warning("UserInfoFromToken không hợp lệ");
                    return null;
                }

                var khaoThiUser = await _khaoThiUserService.GetByIdAsync(userInfoFromToken.ID);
                if (khaoThiUser == null)
                {
                    await Logger.Warning($"Không tìm thấy KhaoThiUser với ID: {userInfoFromToken.ID}");
                    return null;
                }

                return MapKhaoThiUserToUserInfo(khaoThiUser);
            }
            catch (Exception ex)
            {
                await Logger.Error("Lỗi trong GetLatestUserInfo", ex, GetType().Name);
                return null;
            }
        }

        private UserInfo MapKhaoThiUserToUserInfo(KhaoThiUserModel khaoThiUser)
        {
            try
            {
                return _mapper.Map<UserInfo>(khaoThiUser);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi mapping user: {ex.Message}", ex);
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

                _authenticationStateTask = null;
                _cachedUserInfo = null;

                var authState = await GetAuthenticationStateAsync();
                NotifyAuthenticationStateChanged(Task.FromResult(authState));

                await Logger.Info("Đăng nhập thành công");
            }
            catch (Exception ex)
            {
                await Logger.Error("Đăng nhập thất bại", ex, GetType().Name);
                await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
                await _localStorage.RemoveItemAsync(REFRESH_TOKEN_KEY);
                throw;
            }
        }

        public async Task MarkUserAsLoggedOut()
        {
            try
            {
                await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
                await _localStorage.RemoveItemAsync(REFRESH_TOKEN_KEY);

                _authenticationStateTask = null;
                _cachedUserInfo = null;

                var anonymousState = CreateAnonymousState();
                NotifyAuthenticationStateChanged(Task.FromResult(anonymousState));

                await Logger.Info("Đăng xuất thành công");
            }
            catch (Exception ex)
            {
                await Logger.Error("Đăng xuất thất bại", ex, GetType().Name);
                throw;
            }
        }

        private List<Claim> BuildUserClaims(UserInfo userInfo)
        {
            var claims = new List<Claim>();

            if (userInfo.ID != 0)
                claims.Add(new Claim(ClaimTypes.NameIdentifier, userInfo.ID.ToString()));

            if (!string.IsNullOrEmpty(userInfo.TenDangNhap))
                claims.Add(new Claim(ClaimTypes.Name, userInfo.TenDangNhap));

            if (!string.IsNullOrEmpty(userInfo.HoTen))
                claims.Add(new Claim("HoTen", userInfo.HoTen));

            if (!string.IsNullOrEmpty(userInfo.MaDonVi))
                claims.Add(new Claim("MaDonVi", userInfo.MaDonVi));

            if (!string.IsNullOrEmpty(userInfo.TenDonVi))
                claims.Add(new Claim("TenDonVi", userInfo.TenDonVi));

            if (!string.IsNullOrEmpty(userInfo.MaChucVu))
            {
                claims.Add(new Claim("MaChucVu", userInfo.MaChucVu));
                if (userInfo.MaChucVu == "01")
                {
                    claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                }
            }

            if (!string.IsNullOrEmpty(userInfo.Email))
                claims.Add(new Claim(ClaimTypes.Email, userInfo.Email));

            return claims;
        }

        private async Task HandleTokenExpired()
        {
            try
            {
                await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
                await _localStorage.RemoveItemAsync(REFRESH_TOKEN_KEY);

                _authenticationStateTask = null;
                _cachedUserInfo = null;

                var anonymousState = CreateAnonymousState();
                NotifyAuthenticationStateChanged(Task.FromResult(anonymousState));

                await Logger.Warning("Token hết hạn");
            }
            catch (Exception ex)
            {
                await Logger.Error("Lỗi khi xử lý token hết hạn", ex, GetType().Name);
            }
        }

        private static AuthenticationState CreateAnonymousState()
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        public void Dispose()
        {
            _semaphore.Dispose();
        }
    }
}