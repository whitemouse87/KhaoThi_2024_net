using Blazored.LocalStorage;
using Fluxor;
using KhaoThi_2024_net_client.Services.Auth;
using Microsoft.AspNetCore.Components;
using KhaoThi_2024_net_client.Services.Logging;
using static KhaoThi_2024_net_client.Services.Auth.AuthActions;
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
            IState<AuthState> authState)
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
                dispatcher.Dispatch(new SetLoadingAction(true));
                var response = await _authService.Login(action.Request);

                if (response.Success && response.Token != null)
                {
                    await _localStorage.SetItemAsync(AUTH_TOKEN_KEY, response.Token);
                    var userInfo = await _authService.GetUserInfo(response.Token);
                    dispatcher.Dispatch(new LoginSuccessAction(response.Token, userInfo));
                    await Logger.Info($"[CLIENT] user đăng nhập thành công: {userInfo.TenDangNhap}");
                    dispatcher.Dispatch(new ShowNotificationAction("Đăng nhập thành công", "success"));
                    _navigationManager.NavigateTo("/dashboard", forceLoad: true);
                }
                else
                {
                    await Logger.Warning($"[CLIENT] user đăng nhập thất bại: {response.ErrorMessage}");
                    dispatcher.Dispatch(new LoginFailureAction(response.ErrorMessage));
                    dispatcher.Dispatch(new ShowNotificationAction("Đăng nhập thất bại", "error"));
                }
            }
            catch (Exception ex)
            {
                await Logger.Error($"[CLIENT] Lỗi trong Effects: {ex}");
                dispatcher.Dispatch(new LoginFailureAction("Lỗi hệ thống"));
                dispatcher.Dispatch(new ShowNotificationAction("Lỗi hệ thống", "error"));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleLogoutAction(LogoutAction action, IDispatcher dispatcher)
        {
            var currentUser = _authState.Value.CurrentUser;
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                await Logger.Info("[CLIENT] Bắt đầu xử lý đăng xuất");

                // Chỉ cần gọi Logout() từ AuthService
                await _authService.Logout();

                dispatcher.Dispatch(new LogoutSuccessAction());
                dispatcher.Dispatch(new ShowNotificationAction("Đăng xuất thành công", "success"));

                if (currentUser != null)
                {
                    await Logger.Info($"[CLIENT] Người dùng {currentUser.TenDangNhap} đã đăng xuất thành công");
                }

                _navigationManager.NavigateTo("/login", forceLoad: true);
            }
            catch (Exception ex)
            {
                string errorMessage = currentUser != null
                    ? $"Lỗi đăng xuất cho người dùng {currentUser.TenDangNhap}"
                    : "Lỗi trong quá trình đăng xuất";

                await Logger.Error(errorMessage, ex);
                dispatcher.Dispatch(new LogoutFailureAction(ex.Message));
                dispatcher.Dispatch(new ShowNotificationAction("Đăng xuất thất bại", "error"));

                try
                {
                    // Cố gắng cleanup một lần nữa thông qua service
                    await _authService.Logout();
                }
                catch
                {
                    // Bỏ qua lỗi trong cleanup
                }
                _navigationManager.NavigateTo("/login", forceLoad: true);
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }
        //[EffectMethod]
        //public async Task HandleRefreshTokenAction(RefreshTokenAction action, IDispatcher dispatcher)
        //{
        //    try
        //    {
        //        dispatcher.Dispatch(new SetLoadingAction(true));
        //        var result = await _authService.RefreshToken();

        //        if (result.Success)
        //        {
        //            var userInfo = await _authService.GetUserInfo(result.Token);
        //            dispatcher.Dispatch(new RefreshTokenSuccessAction(result.Token, userInfo));
        //        }
        //        else
        //        {
        //            dispatcher.Dispatch(new RefreshTokenFailureAction(result.ErrorMessage));
        //            dispatcher.Dispatch(new LogoutAction());
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        dispatcher.Dispatch(new RefreshTokenFailureAction(ex.Message));
        //        dispatcher.Dispatch(new LogoutAction());
        //    }
        //    finally
        //    {
        //        dispatcher.Dispatch(new SetLoadingAction(false));
        //    }
        //}
      
        [EffectMethod]
        public async Task HandleRefreshTokenAction(RefreshTokenAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                await Logger.Info("[CLIENT] Bắt đầu quy trình refresh token");

                var response = await _authService.RefreshToken();

                if (response.Success && response.Token != null)
                {
                    await _localStorage.SetItemAsync(AUTH_TOKEN_KEY, response.Token);
                    var userInfo = await _authService.GetUserInfo(response.Token);

                    dispatcher.Dispatch(new RefreshTokenSuccessAction(response.Token, userInfo));
                    await Logger.Info($"[CLIENT] Refresh token thành công cho user: {userInfo.TenDangNhap}");
                    dispatcher.Dispatch(new ShowNotificationAction("Làm mới phiên thành công", "success"));
                }
                else
                {
                    await Logger.Warning($"[CLIENT] Refresh token thất bại: {response.ErrorMessage}");
                    dispatcher.Dispatch(new RefreshTokenFailureAction(response.ErrorMessage));
                    dispatcher.Dispatch(new ShowNotificationAction("Phiên làm việc hết hạn", "error"));

                    // Chuyển về trang login khi refresh thất bại
                    await _authService.Logout();
                    _navigationManager.NavigateTo("/login", forceLoad: true);
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "Lỗi trong quá trình làm mới phiên";
                await Logger.Error(errorMessage, ex);
                dispatcher.Dispatch(new RefreshTokenFailureAction(ex.Message));
                dispatcher.Dispatch(new ShowNotificationAction(errorMessage, "error"));

                // Đảm bảo logout và chuyển về login trong trường hợp lỗi
                await _authService.Logout();
                _navigationManager.NavigateTo("/login", forceLoad: true);
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }
    }
}