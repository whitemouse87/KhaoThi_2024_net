using Blazored.LocalStorage;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Models.Shares;
using KhaoThi_2024_net_client.Models.Users;
using KhaoThi_2024_net_client.Services.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace KhaoThi_2024_net_client.Services.BC_1_ThongTinDonVi
{
    public class BCThongTinDonViService : BaseService, IBCThongTinDonViService
    {
        // private const string API_ENDPOINT = "BCThongTinDonVi";
        // private readonly JsonSerializerOptions _jsonOptions;
        // public BCThongTinDonViService(
        //IHttpClientFactory httpClientFactory,
        //ILocalStorageService localStorage,
        //ILoggingService logger)
        //: base(httpClientFactory, localStorage, logger)
        // {
        //     _jsonOptions = new JsonSerializerOptions
        //     {
        //         PropertyNameCaseInsensitive = true
        //     };
        // }
        private const string API_ENDPOINT = "BCThongTinDonVi";
        private readonly JsonSerializerOptions _jsonOptions;
        public BCThongTinDonViService(
       IHttpClientFactory httpClientFactory,
       ILocalStorageService localStorage,
       ILoggingService logger)
       : base(httpClientFactory, localStorage, logger)
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }
        public async Task<KhaoThi_1_THPT_ThongTin_DonViModel?> GetByMaTruongAsync(string MaTruong)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.GetAsync($"{API_ENDPOINT}/{MaTruong}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<KhaoThi_1_THPT_ThongTin_DonViModel>(_jsonOptions);
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }

                await HandleErrorResponse(response);
                return null;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Lỗi khi lấy thông tin đơn vị với mã trường: {MaTruong}", ex, nameof(BCThongTinDonViService));
                throw;
            }
        }
        public async Task<PaginatedResult<KhaoThi_1_THPT_ThongTin_DonViModel>> GetPaginatedAsync(int page, int pageSize, string? searchTerm)
        {
            try
            {
                var token = await GetToken();

                await AddAuthenticationHeader();
                var url = $"{API_ENDPOINT}/all-phantrang?page={page}&pageSize={pageSize}";
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    url += $"&searchTerm={Uri.EscapeDataString(searchTerm)}";
                }

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PaginatedResult<KhaoThi_1_THPT_ThongTin_DonViModel>>(_jsonOptions);
                    if (result != null)
                    {
                        return result;
                    }
                }

                await HandleErrorResponse(response);
                throw new Exception("Không thể lấy danh sách đơn vị");
            }
            catch (HttpRequestException ex)
            {
                await _logger.LogErrorAsync($"Lỗi HTTP khi lấy danh sách người dùng đơn vị phân trang: {ex.Message}", ex, nameof(BCThongTinDonViService));
                throw;
            }
            catch (Exception ex)
            {

                await _logger.LogErrorAsync($"Lỗi không xác định khi lấy danh sách đơn vị phân trang: {ex.Message}", ex, nameof(BCThongTinDonViService));
                throw;
            }

        }


        public async Task<bool> UpdateAsync(KhaoThi_1_THPT_ThongTin_DonViModel DonVi)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.PutAsJsonAsync($"{API_ENDPOINT}/{DonVi.MaTruong}", DonVi, _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogInfoAsync($"Cập nhật đơn vị thành công: {DonVi.MaTruong}", nameof(BCThongTinDonViService));
                    return true;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }

                await HandleErrorResponse(response);
                return false;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Lỗi khi cập nhật đơn vị: {DonVi.MaTruong}", ex, nameof(BCThongTinDonViService));
                throw;
            }
        }
        public async Task<IEnumerable<QuanModel>> LoadDanhSachQuan()
        {
            try
            {
                var response = await _httpClient.GetAsync($"/quan");

                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadFromJsonAsync<IEnumerable<QuanModel>>();
                return content ?? Array.Empty<QuanModel>();


            }
            catch (HttpRequestException ex)
            {
                await _logger.LogErrorAsync("Lỗi HTTP khi lấy danh sách quận: {Message}", ex, nameof(BCThongTinDonViService));
                throw;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Lỗi không xác định khi lấy danh sách quận {Message}", ex, nameof(BCThongTinDonViService));
                throw;
            }
        }
    }
}
