using Blazored.LocalStorage;
using Fluxor;
using KhaoThi_2024_net_client.Services.Auth;
using Microsoft.AspNetCore.Components;
using KhaoThi_2024_net_client.Services.Logging;
using static KhaoThi_2024_net_client.Services.Auth.AuthActions;
using Microsoft.AspNetCore.Components.Authorization;
using KhaoThi_2024_net_client.Models.Auth;

namespace KhaoThi_2024_net_client.State.Auth
{
    public class AuthEffects
    {


        private readonly IAuthService _authService;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;
        private const string AUTH_TOKEN_KEY = "authToken";
        private readonly IState<AuthState> _authState;
        public AuthEffects(
            IAuthService authService,
            ILocalStorageService localStorage,
            NavigationManager navigationManager,
              IState<AuthState> authState
            )
        {
            _authService = authService;
            _localStorage = localStorage;
            _navigationManager = navigationManager;
            _authState = authState;
        }

        [EffectMethod]
        public async Task HandleLoginAction(LoginAction action, IDispatcher dispatcher)
        {
            try
            {
                //Console.WriteLine($"Xử lý đăng nhập cho user: {action.Request.TenDangNhap}");
               
                var response = await _authService.Login(action.Request);

                if (response.Success && response.Token != null)
                {
                    await _localStorage.SetItemAsync(AUTH_TOKEN_KEY, response.Token);
                    var userInfo = await _authService.GetUserInfo(response.Token);
                    dispatcher.Dispatch(new LoginSuccessAction(response.Token, userInfo));
                    await Logger.Info($"[CLIENT] user đăng nhập thành công: {userInfo.TenDangNhap}");
                    _navigationManager.NavigateTo("/dashboard", forceLoad: true);
                  
                }
                else
                {
                    await Logger.Warning($"[CLIENT] user đăng nhập thất bại:{response.ErrorMessage}");
                    dispatcher.Dispatch(new LoginFailureAction(response.ErrorMessage));
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"Lỗi trong Effects: {ex}");
                await Logger.Error($"[CLIENT] Lỗi trong Effects: {ex}");
                dispatcher.Dispatch(new LoginFailureAction("Lỗi hệ thống"));
            }
        }


        [EffectMethod]
        public async Task HandleLogoutAction(LogoutAction action, IDispatcher dispatcher)
        {
            var currentUser = _authState.Value.CurrentUser;
            //Console.WriteLine("[Info] Processing logout.");
            await Logger.Info("Processing logout");
            try
            {
                // Remove token from local storage
                await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
                //Console.WriteLine("[Info] Token removed from local storage.");
                await Logger.Info($"[CLIENT] Token removed from local storage.");
                // Call logout service if necessary
                await _authService.Logout();
                if (currentUser != null)
                {
                    await Logger.Info($"[CLIENT] User {currentUser.TenDangNhap} đã logout thành công");
                }

                // Dispatch logout action to reset state
                dispatcher.Dispatch(new LoginSuccessAction(null, null));

                // Navigate back to login page
                //Console.WriteLine("[Info] Navigating to login page.");

                _navigationManager.NavigateTo("/login", forceLoad: true);
            }
            catch (Exception ex)
            {
                string errorMessage = currentUser != null
                      ? $"Lỗi logout cho user {currentUser.TenDangNhap}"
                      : "Lỗi trong quá trình logout";
                //Console.WriteLine($"[Error] Exception during logout: {ex.Message}");
                await Logger.Error(errorMessage, ex);
                dispatcher.Dispatch(new LoginFailureAction("An error occurred during logout."));
            }
        }
    }
}