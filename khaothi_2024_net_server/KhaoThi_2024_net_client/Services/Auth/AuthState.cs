using Fluxor;
using KhaoThi_2024_net_client.Models.Auth;

namespace KhaoThi_2024_net_client.Services.Auth
{
    [FeatureState]
    public class AuthState
    {
        // Properties
        public bool IsAuthenticated { get; }
        public bool IsLoading { get; }
        public UserInfo? CurrentUser { get; }
        public string? Token { get; }
        public string? Error { get; }

        private AuthState() { } // Required for Fluxor

        public AuthState(bool isAuthenticated, bool isLoading, UserInfo? user, string? token, string? error)
        {
            IsAuthenticated = isAuthenticated;
            IsLoading = isLoading;
            CurrentUser = user;
            Token = token;
            Error = error;
        }
    }
}