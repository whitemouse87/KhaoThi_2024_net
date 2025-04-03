//using Blazored.LocalStorage;
//using Microsoft.AspNetCore.Components;
//using System.Net;
//using System.Net.Http.Headers;

//namespace KhaoThi_2024_net_client.Services.Auth
//{
//    public class AuthInterceptor : DelegatingHandler
//    {
//        private readonly IAuthService _authService;
//        private readonly ILocalStorageService _localStorage;
//        private readonly NavigationManager _navigationManager;
//        private readonly ILogger<AuthInterceptor> _logger;

//        public AuthInterceptor(
//            IAuthService authService,
//            ILocalStorageService localStorage,
//            NavigationManager navigationManager,
//            ILogger<AuthInterceptor> logger)
//        {
//            _authService = authService;
//            _localStorage = localStorage;
//            _navigationManager = navigationManager;
//            _logger = logger;
//        }

//        protected override async Task<HttpResponseMessage> SendAsync(
//            HttpRequestMessage request, CancellationToken cancellationToken)
//        {
//            var response = await base.SendAsync(request, cancellationToken);

//            if (response.StatusCode == HttpStatusCode.Unauthorized)
//            {
//                try
//                {
//                    var refreshResult = await _authService.RefreshToken();
//                    if (refreshResult.Success)
//                    {
//                        request.Headers.Authorization = new AuthenticationHeaderValue(
//                            "Bearer", refreshResult.Token);
//                        return await base.SendAsync(request, cancellationToken);
//                    }
//                }
//                catch (Exception ex)
//                {
//                    _logger.LogError(ex, "Token refresh failed");
//                }

//                await _authService.Logout();
//                _navigationManager.NavigateTo("/login", true);
//            }

//            return response;
//        }
//    }
//}
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
            // Thêm token vào request trước khi gửi
            await AddTokenToRequest(request);

            // Đảm bảo header Content-Type cho POST/PUT requests
            if ((request.Method == HttpMethod.Post || request.Method == HttpMethod.Put) &&
                request.Content != null &&
                request.Content.Headers.ContentType == null)
            {
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            }

            var response = await base.SendAsync(request, cancellationToken);

            // Xử lý lỗi Unauthorized
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                try
                {
                    _logger.LogInformation("Unauthorized response detected. Attempting to refresh token.");
                    var refreshResult = await _authService.RefreshToken();
                    if (refreshResult.Success)
                    {
                        _logger.LogInformation("Token refreshed successfully. Retrying the request.");
                        // Tạo request mới với token mới
                        var newRequest = await CloneHttpRequestMessageAsync(request);
                        newRequest.Headers.Authorization = new AuthenticationHeaderValue(
                            "Bearer", refreshResult.Token);
                        return await base.SendAsync(newRequest, cancellationToken);
                    }
                    else
                    {
                        _logger.LogWarning("Token refresh failed. Redirecting to login.");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Token refresh failed with exception");
                }

                await _authService.Logout();
                _navigationManager.NavigateTo("/login", true);
            }

            return response;
        }

        private async Task AddTokenToRequest(HttpRequestMessage request)
        {
            try
            {
                var token = await _localStorage.GetItemAsync<string>("authToken");
                if (!string.IsNullOrEmpty(token))
                {
                    _logger.LogDebug("Adding authentication token to request");
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                }
                else
                {
                    _logger.LogDebug("No authentication token found in storage");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving token from storage");
            }
        }

        private async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage request)
        {
            var newRequest = new HttpRequestMessage(request.Method, request.RequestUri);

            // Sao chép headers
            foreach (var header in request.Headers)
            {
                newRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            // Sao chép properties
            foreach (var property in request.Properties)
            {
                newRequest.Properties.Add(property);
            }

            // Sao chép content nếu có
            if (request.Content != null)
            {
                var contentBytes = await request.Content.ReadAsByteArrayAsync();
                var newContent = new ByteArrayContent(contentBytes);

                // Sao chép content headers
                foreach (var header in request.Content.Headers)
                {
                    newContent.Headers.TryAddWithoutValidation(header.Key, header.Value);
                }

                newRequest.Content = newContent;
            }

            return newRequest;
        }
    }
}