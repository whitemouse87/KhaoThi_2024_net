using Blazored.LocalStorage;
using KhaoThi_2024_net_client.Services.Logging;
using System.Net;
using System.Net.Http.Headers;

namespace KhaoThi_2024_net_client.Components
{
    /// <summary>
    /// Lớp cơ sở cho các services trong ứng dụng
    /// </summary>
    public abstract class BaseService
    {
        protected readonly HttpClient _httpClient;
        protected readonly ILocalStorageService _localStorage;
        protected readonly ILoggingService _logger;
        protected readonly IHttpClientFactory _httpClientFactory;
        private const string AUTH_TOKEN_KEY = "authToken";
        private const string REFRESH_TOKEN_KEY = "refreshToken";

        protected BaseService(
            IHttpClientFactory httpClientFactory,
            ILocalStorageService localStorage,
            ILoggingService logger)
        {
            _httpClientFactory = httpClientFactory;
            _localStorage = localStorage;
            _logger = logger;
            _httpClient = _httpClientFactory.CreateClient("API"); // Sử dụng named client với AuthInterceptor
        }

        protected async Task<string?> GetToken()
        {
            return await _localStorage.GetItemAsync<string>(AUTH_TOKEN_KEY);
        }

        protected async Task<string?> GetRefreshToken()
        {
            return await _localStorage.GetItemAsync<string>(REFRESH_TOKEN_KEY);
        }

        protected async Task AddAuthenticationHeader()
        {
            var token = await GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        protected async Task HandleErrorResponse(HttpResponseMessage response)
        {
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                var errorMessage = $"API Error: {response.StatusCode} - {content}";

                // Log với thông tin chi tiết hơn
                await _logger.LogErrorAsync(errorMessage, null,
                    $"BaseService - {response.RequestMessage?.Method} {response.RequestMessage?.RequestUri}");

                // Xử lý riêng cho 401 Unauthorized 
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    await HandleTokenExpired();
                }

                throw new HttpRequestException(errorMessage, null, response.StatusCode);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Failed to handle error response", ex, "BaseService");
                throw;
            }
        }

        protected async Task HandleTokenExpired()
        {
            await _localStorage.RemoveItemAsync(AUTH_TOKEN_KEY);
            await _localStorage.RemoveItemAsync(REFRESH_TOKEN_KEY);
            await _logger.LogWarningAsync("Token expired, cleared from storage", "BaseService");
        }
    }
}