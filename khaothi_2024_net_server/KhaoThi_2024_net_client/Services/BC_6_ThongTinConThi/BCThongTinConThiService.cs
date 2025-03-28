using Blazored.LocalStorage;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Services.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace khaothi_2024_net_client.Services.BC_5_ThongTinConThi
{
    public class BCThongTinConThiService : BaseService, IBCThongTinConThiService
    {
        private const string API_ENDPOINT = "BCThongTinConThi";
        private readonly JsonSerializerOptions _jsonOptions;

        public BCThongTinConThiService(
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

        public async Task<bool> CreateAsync(KhaoThi_5_THPT_ThongTin_ConThiModel conThi)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.PostAsJsonAsync($"{API_ENDPOINT}", conThi, _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogInfoAsync($"Tạo mới thông tin con thi thành công: {conThi.MaDinhDanhCuaCon}", nameof(BCThongTinConThiService));
                    return true;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await _logger.LogErrorAsync($"Lỗi khi tạo thông tin con thi: {errorContent}", null, nameof(BCThongTinConThiService));
                    return false;
                }

                await HandleErrorResponse(response);
                return false;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Lỗi khi tạo mới thông tin con thi", ex, nameof(BCThongTinConThiService));
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string maTruong, string cccd, string maDinhDanhCuaCon)
        {
            try
            {
                await AddAuthenticationHeader();
                var url = $"{API_ENDPOINT}?maTruong={Uri.EscapeDataString(maTruong)}" +
                          $"&cccd={Uri.EscapeDataString(cccd)}" +
                          $"&maDinhDanhCuaCon={Uri.EscapeDataString(maDinhDanhCuaCon)}";

                var response = await _httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogInfoAsync($"Xóa thông tin con thi thành công: {maTruong}, {cccd}, {maDinhDanhCuaCon}",
                        nameof(BCThongTinConThiService));
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
                await _logger.LogErrorAsync($"Lỗi khi xóa thông tin con thi: {maTruong}, {cccd}, {maDinhDanhCuaCon}",
                    ex, nameof(BCThongTinConThiService));
                throw;
            }
        }

        public async Task<IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel>> GetByMaTruongAsync(string maTruong)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.GetAsync($"{API_ENDPOINT}/truong/{Uri.EscapeDataString(maTruong)}");

                if (response.IsSuccessStatusCode)
                {
                    var conThiList = await response.Content.ReadFromJsonAsync<IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel>>(_jsonOptions);
                    return conThiList ?? Enumerable.Empty<KhaoThi_5_THPT_ThongTin_ConThiModel>();
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return Enumerable.Empty<KhaoThi_5_THPT_ThongTin_ConThiModel>();
                }

                await HandleErrorResponse(response);
                return Enumerable.Empty<KhaoThi_5_THPT_ThongTin_ConThiModel>();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Lỗi khi lấy danh sách con thi theo mã trường: {maTruong}",
                    ex, nameof(BCThongTinConThiService));
                throw;
            }
        }

        public async Task<PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThiModel>> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null,
            string? kyThiThamDu = null)
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

                if (!string.IsNullOrEmpty(maTruong))
                {
                    url += $"&maTruong={Uri.EscapeDataString(maTruong)}";
                }

                if (!string.IsNullOrEmpty(kyThiThamDu))
                {
                    url += $"&kyThiThamDu={Uri.EscapeDataString(kyThiThamDu)}";
                }

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThiModel>>(_jsonOptions);
                    if (result != null)
                    {
                        return result;
                    }
                }

                await HandleErrorResponse(response);
                throw new Exception("Không thể lấy danh sách con thi");
            }
            catch (HttpRequestException ex)
            {
                await _logger.LogErrorAsync($"Lỗi HTTP khi lấy danh sách con thi phân trang: {ex.Message}",
                    ex, nameof(BCThongTinConThiService));
                throw;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Lỗi không xác định khi lấy danh sách con thi phân trang: {ex.Message}",
                    ex, nameof(BCThongTinConThiService));
                throw;
            }
        }

        public async Task<KhaoThi_5_THPT_ThongTin_ConThiModel?> GetThongTinCaNhan(
            string maTruong, string cccd, string maDinhDanhCuaCon)
        {
            try
            {
                await AddAuthenticationHeader();
                var url = $"{API_ENDPOINT}/detailconthi?maTruong={Uri.EscapeDataString(maTruong)}" +
                          $"&cccd={Uri.EscapeDataString(cccd)}" +
                          $"&maDinhDanhCuaCon={Uri.EscapeDataString(maDinhDanhCuaCon)}";

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<KhaoThi_5_THPT_ThongTin_ConThiModel>(_jsonOptions);
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
                await _logger.LogErrorAsync($"Lỗi khi lấy thông tin chi tiết con thi: {maTruong}, {cccd}, {maDinhDanhCuaCon}",
                    ex, nameof(BCThongTinConThiService));
                throw;
            }
        }

        public async Task<bool> IsConThiExistAsync(string maTruong, string cccd, string maDinhDanhCuaCon)
        {
            try
            {
                await AddAuthenticationHeader();
                var url = $"{API_ENDPOINT}/check-exist-conthi?maTruong={Uri.EscapeDataString(maTruong)}" +
                          $"&cccd={Uri.EscapeDataString(cccd)}" +
                          $"&maDinhDanhCuaCon={Uri.EscapeDataString(maDinhDanhCuaCon)}";

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<dynamic>();
                    return result?.exists ?? false;
                }

                await HandleErrorResponse(response);
                return false;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Lỗi khi kiểm tra tồn tại con thi: {maTruong}, {cccd}, {maDinhDanhCuaCon}",
                    ex, nameof(BCThongTinConThiService));
                throw;
            }
        }

        public async Task<bool> UpdateAsync(KhaoThi_5_THPT_ThongTin_ConThiModel conThi)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.PutAsJsonAsync($"{API_ENDPOINT}", conThi, _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogInfoAsync($"Cập nhật thông tin con thi thành công: {conThi.MaTruong}, {conThi.CCCD}, {conThi.MaDinhDanhCuaCon}",
                        nameof(BCThongTinConThiService));
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
                await _logger.LogErrorAsync($"Lỗi khi cập nhật thông tin con thi: {conThi.MaTruong}, {conThi.CCCD}, {conThi.MaDinhDanhCuaCon}",
                    ex, nameof(BCThongTinConThiService));
                throw;
            }
        }
    }
}