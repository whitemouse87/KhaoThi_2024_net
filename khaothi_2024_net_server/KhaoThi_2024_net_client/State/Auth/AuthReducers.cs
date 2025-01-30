using Fluxor;
using KhaoThi_2024_net_client.Services.Auth;
using KhaoThi_2024_net_client.Services.Logging;
using static KhaoThi_2024_net_client.Services.Auth.AuthActions;

namespace KhaoThi_2024_net_client.State.Auth
{
    public static class AuthReducers
    {
        [ReducerMethod]
        public static AuthState ReduceLoginAction(AuthState state, LoginAction action)
        {
            //Console.WriteLine("[Info] Starting login process...");
            return new AuthState(
                isAuthenticated: false,
                isLoading: true,
                user: null,
                token: null,
                error: null
            );
        }

        [ReducerMethod]
        public static AuthState ReduceLoginSuccess(AuthState state, LoginSuccessAction action)
        {
            if (action.User == null || string.IsNullOrEmpty(action.Token))
            {
                //Console.WriteLine("[Error] Invalid login response: missing user or token");
                return new AuthState(
                    isAuthenticated: false,
                    isLoading: false,
                    user: null,
                    token: null,
                    error: "Invalid login response from server"
                );
            }

            //await Logger.Info($"[Success] User {action.User.TenDangNhap} logged in successfully");
            return new AuthState(
                isAuthenticated: true,
                isLoading: false,
                user: action.User,
                token: action.Token,
                error: null
            );
        }

        [ReducerMethod]
        public static AuthState ReduceLoginFailure(AuthState state, LoginFailureAction action)
        {
            var errorMessage = string.IsNullOrEmpty(action.Error)
                ? "Unknown error occurred"
                : action.Error;

            //Console.WriteLine($"[Error] Login failed: {errorMessage}");
            return new AuthState(
                isAuthenticated: false,
                isLoading: false,
                user: null,
                token: null,
                error: errorMessage
            );
        }

        [ReducerMethod]
        public static AuthState ReduceLogout(AuthState state, LogoutAction action)
        {
            //Console.WriteLine("[Info] User logged out");
            return new AuthState(
                isAuthenticated: false,
                isLoading: false,
                user: null,
                token: null,
                error: null
            );
        }
    }
}