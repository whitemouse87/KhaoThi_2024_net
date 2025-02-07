using Fluxor;
using KhaoThi_2024_net_client.Models.Auth;

namespace KhaoThi_2024_net_client.Services.Auth
{
    [FeatureState]
    public class AuthState
    {
        public bool IsAuthenticated { get; init; }
        public bool IsLoading { get; init; }
        public UserInfo? CurrentUser { get; init; }
        public string? Token { get; init; }
        public string? RefreshToken { get; init; } // Added RefreshToken
        public string? Error { get; init; }

        public static AuthState InitialState => new AuthState(false, false, null, null, null, null);

        private AuthState() { }

        public AuthState(
            bool isAuthenticated,
            bool isLoading,
            UserInfo? user,
            string? token,
            string? refreshToken, // Added RefreshToken to constructor
            string? error)
        {
            IsAuthenticated = isAuthenticated;
            IsLoading = isLoading;
            CurrentUser = user;
            Token = token;
            RefreshToken = refreshToken; // Initialize RefreshToken
            Error = error;
        }
    }
}