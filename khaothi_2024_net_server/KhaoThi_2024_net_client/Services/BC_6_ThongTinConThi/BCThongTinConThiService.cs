using Blazored.LocalStorage;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Services.Logging;
using System.Net.Http.Json;
using System.Text.Json;
namespace KhaoThi_2024_net_client.Services.BC_6_ThongTinConThi
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

        public async Task<KhaoThi_5_THPT_ThongTin_ConThi?> GetByKeysAsync(string maTruong, string cccd, string maDinhDanhCuaCon)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.GetAsync(
                    $"{API_ENDPOINT}/{Uri.EscapeDataString(maTruong)}/{Uri.EscapeDataString(cccd)}/{Uri.EscapeDataString(maDinhDanhCuaCon)}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<KhaoThi_5_THPT_ThongTin_ConThi>(_jsonOptions);
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
                await _logger.LogErrorAsync(
                    $"Lỗi khi lấy thông tin con thí sinh. MaTruong: {maTruong}, CCCD: {cccd}, MaDinhDanhCuaCon: {maDinhDanhCuaCon}",
                    ex, nameof(BCThongTinConThiService));
                throw;
            }
        }

        public async Task<IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi>> GetByMaTruongAndCCCDAsync(string maTruong, string cccd)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.GetAsync(
                    $"{API_ENDPOINT}/by-truong-cccd/{Uri.EscapeDataString(maTruong)}/{Uri.EscapeDataString(cccd)}");

                if (response.IsSuccessStatusCode)
                {
                    var conThiList = await response.Content.ReadFromJsonAsync<IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi>>(_jsonOptions);
                    return conThiList ?? Enumerable.Empty<KhaoThi_5_THPT_ThongTin_ConThi>();
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return Enumerable.Empty<KhaoThi_5_THPT_ThongTin_ConThi>();
                }

                await HandleErrorResponse(response);
                return Enumerable.Empty<KhaoThi_5_THPT_ThongTin_ConThi>();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync(
                    $"Lỗi khi lấy danh sách con thí sinh theo mã trường và CCCD. MaTruong: {maTruong}, CCCD: {cccd}",
                    ex, nameof(BCThongTinConThiService));
                throw;
            }
        }

        public async Task<IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi>> GetAllAsync()
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.GetAsync($"{API_ENDPOINT}/all-conthi");

                if (response.IsSuccessStatusCode)
                {
                    var conThiList = await response.Content.ReadFromJsonAsync<IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi>>(_jsonOptions);
                    return conThiList ?? Enumerable.Empty<KhaoThi_5_THPT_ThongTin_ConThi>();
                }

                await HandleErrorResponse(response);
                return Enumerable.Empty<KhaoThi_5_THPT_ThongTin_ConThi>();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Lỗi khi lấy tất cả danh sách con thí sinh", ex, nameof(BCThongTinConThiService));
                throw;
            }
        }

        public async Task<PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThi>> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null)
        {
            try
            {
                await AddAuthenticationHeader();
                var url = $"{API_ENDPOINT}/all-phantrang-conthi?page={page}&pageSize={pageSize}";

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    url += $"&searchTerm={Uri.EscapeDataString(searchTerm)}";
                }

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThi>>(_jsonOptions);
                    if (result != null)
                    {
                        return result;
                    }
                }

                await HandleErrorResponse(response);
                throw new Exception("Không thể lấy danh sách con thí sinh phân trang");
            }
            catch (HttpRequestException ex)
            {
                await _logger.LogErrorAsync($"Lỗi HTTP khi lấy danh sách con thí sinh phân trang: {ex.Message}", ex, nameof(BCThongTinConThiService));
                throw;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Lỗi không xác định khi lấy danh sách con thí sinh phân trang: {ex.Message}", ex, nameof(BCThongTinConThiService));
                throw;
            }
        }

        public async Task<bool> InsertAsync(KhaoThi_5_THPT_ThongTin_ConThi conThi)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.PostAsJsonAsync($"{API_ENDPOINT}", conThi, _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogInfoAsync($"Tạo mới thông tin con thí sinh thành công: {conThi.HoTenCon}, MaDinhDanh: {conThi.MaDinhDanhCuaCon}", nameof(BCThongTinConThiService));
                    return true;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await _logger.LogErrorAsync($"Lỗi khi tạo thông tin con thí sinh: {errorContent}", null, nameof(BCThongTinConThiService));
                    return false;
                }

                await HandleErrorResponse(response);
                return false;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Lỗi khi tạo mới thông tin con thí sinh", ex, nameof(BCThongTinConThiService));
                throw;
            }
        }

        public async Task<bool> UpdateAsync(KhaoThi_5_THPT_ThongTin_ConThi conThi)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.PutAsJsonAsync(
                    $"{API_ENDPOINT}/{Uri.EscapeDataString(conThi.MaTruong)}/{Uri.EscapeDataString(conThi.CCCD)}/{Uri.EscapeDataString(conThi.MaDinhDanhCuaCon)}",
                    conThi,
                    _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogInfoAsync($"Cập nhật thông tin con thí sinh thành công: {conThi.HoTenCon}, MaDinhDanh: {conThi.MaDinhDanhCuaCon}", nameof(BCThongTinConThiService));
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
                await _logger.LogErrorAsync(
                    $"Lỗi khi cập nhật thông tin con thí sinh. MaTruong: {conThi.MaTruong}, CCCD: {conThi.CCCD}, MaDinhDanhCuaCon: {conThi.MaDinhDanhCuaCon}",
                    ex, nameof(BCThongTinConThiService));
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string maTruong, string cccd, string maDinhDanhCuaCon)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.DeleteAsync(
                    $"{API_ENDPOINT}/{Uri.EscapeDataString(maTruong)}/{Uri.EscapeDataString(cccd)}/{Uri.EscapeDataString(maDinhDanhCuaCon)}");

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogInfoAsync($"Xóa thông tin con thí sinh thành công. MaTruong: {maTruong}, CCCD: {cccd}, MaDinhDanhCuaCon: {maDinhDanhCuaCon}", nameof(BCThongTinConThiService));
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
                await _logger.LogErrorAsync(
                    $"Lỗi khi xóa thông tin con thí sinh. MaTruong: {maTruong}, CCCD: {cccd}, MaDinhDanhCuaCon: {maDinhDanhCuaCon}",
                    ex, nameof(BCThongTinConThiService));
                throw;
            }
        }

        public async Task<bool> ExistsAsync(string maTruong, string cccd, string maDinhDanhCuaCon)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.GetAsync(
                    $"{API_ENDPOINT}/exists/{Uri.EscapeDataString(maTruong)}/{Uri.EscapeDataString(cccd)}/{Uri.EscapeDataString(maDinhDanhCuaCon)}");

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
                await _logger.LogErrorAsync(
                    $"Lỗi khi kiểm tra tồn tại thông tin con thí sinh. MaTruong: {maTruong}, CCCD: {cccd}, MaDinhDanhCuaCon: {maDinhDanhCuaCon}",
                    ex, nameof(BCThongTinConThiService));
                throw;
            }
        }
    }
}
