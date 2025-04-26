using Blazored.LocalStorage;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Services.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace KhaoThi_2024_net_client.Services.BC_5_TruongDiemDonVi
{
    public class BCThongTinTruongDiemService : BaseService, IBCThongTinTruongDiemDonViService
    {
        private const string API_ENDPOINT = "BCThongTinTruongDiem";
        private readonly JsonSerializerOptions _jsonOptions;

        public BCThongTinTruongDiemService(
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

        public async Task<PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null)
        {
            try
            {
                await AddAuthenticationHeader();
                var url = $"{API_ENDPOINT}/all-phantrang-truongdiem?page={page}&pageSize={pageSize}";

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    url += $"&searchTerm={Uri.EscapeDataString(searchTerm)}";
                }

                if (!string.IsNullOrEmpty(maTruong))
                {
                    url += $"&maTruong={Uri.EscapeDataString(maTruong)}";
                }

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>>(_jsonOptions);
                    if (result != null)
                    {
                        return result;
                    }
                }

                await HandleErrorResponse(response);
                throw new Exception("Không thể lấy danh sách thông tin lãnh đạo điểm thi");
            }
            catch (HttpRequestException ex)
            {
                await _logger.LogErrorAsync($"Lỗi HTTP khi lấy danh sách thông tin danh sách lãnh đạo điểm thi, phân trang: {ex.Message}", ex, nameof(BCThongTinTruongDiemService));
                throw;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Lỗi không xác định khi lấy danh sách thông tin lãnh đạo điểm thi, phân trang: {ex.Message}", ex, nameof(BCThongTinTruongDiemService));
                throw;
            }
        }

        public async Task<bool> UpdateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel truongDiem)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.PutAsJsonAsync(
                    $"{API_ENDPOINT}/truongdiemtruong/{Uri.EscapeDataString(truongDiem.MaTruong)}/cccd/{Uri.EscapeDataString(truongDiem.CCCD)}",
                    truongDiem,
                    _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogInfoAsync($"Cập nhật thông tin lãnh đạo điểm thi thành công: MaTruong: {truongDiem.MaTruong}, CCCD: {truongDiem.CCCD}", nameof(BCThongTinTruongDiemService));
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
                await _logger.LogErrorAsync($"Lỗi khi cập nhật thông tin lãnh đạo điểm thi. MaTruong: {truongDiem.MaTruong}, CCCD: {truongDiem.CCCD}",
                    ex, nameof(BCThongTinTruongDiemService));
                throw;
            }
        }
        public async Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetByMaTruongAsync(string maTruong)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.GetAsync($"{API_ENDPOINT}/truongdiem/{Uri.EscapeDataString(maTruong)}");

                if (response.IsSuccessStatusCode)
                {
                    var lanhDaos = await response.Content.ReadFromJsonAsync<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>>(_jsonOptions);
                    return lanhDaos ?? Enumerable.Empty<KhaoThi_4_THPT_ThongTin_LanhDaoModel>();
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return Enumerable.Empty<KhaoThi_4_THPT_ThongTin_LanhDaoModel>();
                }

                await HandleErrorResponse(response);
                return Enumerable.Empty<KhaoThi_4_THPT_ThongTin_LanhDaoModel>();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Lỗi khi lấy danh sách lãnh đạo theo mã trường: {maTruong}", ex, nameof(BCThongTinTruongDiemService));
                throw;
            }
        }
    }
}
