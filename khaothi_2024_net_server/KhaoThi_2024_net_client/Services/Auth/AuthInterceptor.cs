using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Headers;

namespace KhaoThi_2024_net_client.Services.Auth
{
    public class AuthInterceptor : DelegatingHandler
    {
        private readonly IAuthService _authService;
        private readonly ILocalStorageService _localStorage;
        private readonly NavigationManager _navigationManager;
        private readonly ILogger<AuthInterceptor> _logger;

        public AuthInterceptor(
            IAuthService authService,
            ILocalStorageService localStorage,
            NavigationManager navigationManager,
            ILogger<AuthInterceptor> logger)
        {
            _authService = authService;
            _localStorage = localStorage;
            _navigationManager = navigationManager;
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = await base.SendAsync(request, cancellationToken);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                try
                {
                    var refreshResult = await _authService.RefreshToken();
                    if (refreshResult.Success)
                    {
                        request.Headers.Authorization = new AuthenticationHeaderValue(
                            "Bearer", refreshResult.Token);
                        return await base.SendAsync(request, cancellationToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Token refresh failed");
                }

                await _authService.Logout();
                _navigationManager.NavigateTo("/login", true);
            }

            return response;
        }
    }
}