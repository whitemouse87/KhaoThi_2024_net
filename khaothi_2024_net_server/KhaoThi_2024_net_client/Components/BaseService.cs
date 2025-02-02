using Blazored.LocalStorage;
using KhaoThi_2024_net_client.Services.Logging;
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
        private const string AUTH_TOKEN_KEY = "authToken";
    

        protected BaseService(HttpClient httpClient, ILocalStorageService localStorage, ILoggingService logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _localStorage = localStorage ?? throw new ArgumentNullException(nameof(localStorage));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Lấy token xác thực từ localStorage
        /// </summary>
        protected async Task<string?> GetToken()
        {
            return await _localStorage.GetItemAsync<string>(AUTH_TOKEN_KEY);
        }

        /// <summary>
        /// Thêm token xác thực vào header của request
        /// </summary>
        protected async Task AddAuthenticationHeader()
        {
            var token = await GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        /// <summary>
        /// Xử lý response lỗi từ API
        /// </summary>
        protected async Task HandleErrorResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var errorMessage = $"API Error: {response.StatusCode} - {content}";

            // Ghi log lỗi vào hệ thống
            await _logger.LogErrorAsync(errorMessage, null, "BaseService");

            throw new HttpRequestException(errorMessage, null, response.StatusCode);
        }
    }
}