using Fluxor;
using KhaoThi_2024_net_client.Models.Auth;
using KhaoThi_2024_net_client.Services.Auth;
using static KhaoThi_2024_net_client.Services.Auth.AuthActions;

namespace KhaoThi_2024_net_client.State.Auth
{
    public static class AuthReducers
    {
        /// <summary>
        /// Reducer xử lý action đăng nhập
        /// </summary>
        [ReducerMethod]
        public static AuthState ReduceLoginAction(AuthState state, LoginAction action) =>
            new AuthState(
                isAuthenticated: false,
                isLoading: true,
                user: null,
                token: null,
                error: null
            );

        /// <summary>
        /// Reducer xử lý đăng nhập thành công
        /// </summary>
        [ReducerMethod]
        public static AuthState ReduceLoginSuccessAction(AuthState state, LoginSuccessAction action) =>
            new AuthState(
                isAuthenticated: true,
                isLoading: false,
                user: action.User,
                token: action.Token,
                error: null
            );

        /// <summary>
        /// Reducer xử lý đăng nhập thất bại
        /// </summary>
        [ReducerMethod]
        public static AuthState ReduceLoginFailureAction(AuthState state, LoginFailureAction action) =>
            new AuthState(
                isAuthenticated: false,
                isLoading: false,
                user: null,
                token: null,
                error: action.ErrorMessage
            );

        /// <summary>
        /// Reducer xử lý bắt đầu đăng xuất
        /// </summary>
        [ReducerMethod]
        public static AuthState ReduceLogoutAction(AuthState state, LogoutAction action) =>
            new AuthState(
                isAuthenticated: state.IsAuthenticated,
                isLoading: true,
                user: state.CurrentUser,
                token: state.Token,
                error: null
            );

        /// <summary>
        /// Reducer xử lý đăng xuất thành công
        /// </summary>
        [ReducerMethod]
        public static AuthState ReduceLogoutSuccessAction(AuthState state, LogoutSuccessAction action) =>
            new AuthState(
                isAuthenticated: false,
                isLoading: false,
                user: null,
                token: null,
                error: null
            );

        /// <summary>
        /// Reducer xử lý đăng xuất thất bại
        /// </summary>
        [ReducerMethod]
        public static AuthState ReduceLogoutFailureAction(AuthState state, LogoutFailureAction action) =>
            new AuthState(
                isAuthenticated: state.IsAuthenticated,
                isLoading: false,
                user: state.CurrentUser,
                token: state.Token,
                error: action.ErrorMessage
            );

        /// <summary>
        /// Reducer xử lý set loading state
        /// </summary>
        [ReducerMethod]
        public static AuthState ReduceSetLoadingAction(AuthState state, SetLoadingAction action) =>
            new AuthState(
                isAuthenticated: state.IsAuthenticated,
                isLoading: action.IsLoading,
                user: state.CurrentUser,
                token: state.Token,
                error: state.Error
            );

        /// <summary>
        /// Reducer xử lý xóa error message
        /// </summary>
        [ReducerMethod]
        public static AuthState ReduceClearErrorAction(AuthState state, ClearErrorAction action) =>
            new AuthState(
                isAuthenticated: state.IsAuthenticated,
                isLoading: state.IsLoading,
                user: state.CurrentUser,
                token: state.Token,
                error: null
            );
        // Trong AuthReducers.cs
        /// <summary>
        /// Reducer xử lý refresh token thành công
        /// </summary>
        [ReducerMethod]
        public static AuthState ReduceRefreshTokenSuccessAction(AuthState state, RefreshTokenSuccessAction action) =>
            new AuthState(
                isAuthenticated: true,
                isLoading: false,
                user: action.User,
                token: action.Token,
                error: null
            );

        /// <summary>
        /// Reducer xử lý refresh token thất bại
        /// </summary>
        [ReducerMethod]
        public static AuthState ReduceRefreshTokenFailureAction(AuthState state, RefreshTokenFailureAction action) =>
            new AuthState(
                isAuthenticated: false,
                isLoading: false,
                user: null,
                token: null,
                error: action.ErrorMessage
            );
    }
}