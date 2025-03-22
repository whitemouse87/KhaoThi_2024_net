using Blazored.LocalStorage;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Models.Shares;
using KhaoThi_2024_net_client.Services.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace KhaoThi_2024_net_client.Services.BC_4_LanhDaoDonVi
{
    public class BCLanhDaoDonViService : BaseService, IBCLanhDaoDonViService
    {
        private const string API_ENDPOINT = "BCLanhDaoDonVi";
        private readonly JsonSerializerOptions _jsonOptions;

        public BCLanhDaoDonViService(
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

        public async Task<KhaoThi_4_THPT_ThongTin_LanhDaoModel?> GetByMaTruongAndCCCDAsync(string maTruong, string cccd)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.GetAsync($"{API_ENDPOINT}/ldtruong/{Uri.EscapeDataString(maTruong)}/cccd/{Uri.EscapeDataString(cccd)}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(_jsonOptions);
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
                await _logger.LogErrorAsync($"Lỗi khi lấy thông tin lãnh đạo. MaTruong: {maTruong}, CCCD: {cccd}", ex, nameof(BCLanhDaoDonViService));
                throw;
            }
        }

        public async Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetByMaTruongAsync(string maTruong)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.GetAsync($"{API_ENDPOINT}/ldtruong/{Uri.EscapeDataString(maTruong)}");

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
                await _logger.LogErrorAsync($"Lỗi khi lấy danh sách lãnh đạo theo mã trường: {maTruong}", ex, nameof(BCLanhDaoDonViService));
                throw;
            }
        }

        public async Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetAllAsync()
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.GetAsync($"{API_ENDPOINT}/all-lanhdao");

                if (response.IsSuccessStatusCode)
                {
                    var lanhDaos = await response.Content.ReadFromJsonAsync<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>>(_jsonOptions);
                    return lanhDaos ?? Enumerable.Empty<KhaoThi_4_THPT_ThongTin_LanhDaoModel>();
                }

                await HandleErrorResponse(response);
                return Enumerable.Empty<KhaoThi_4_THPT_ThongTin_LanhDaoModel>();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Lỗi khi lấy tất cả danh sách lãnh đạo", ex, nameof(BCLanhDaoDonViService));
                throw;
            }
        }

        public async Task<PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null,
            int? namSinh = null,
            string? cccd = null)
        {
            try
            {
                var token = await GetToken();

                await AddAuthenticationHeader();
                var url = $"{API_ENDPOINT}/all-phantrang-lanhdao?page={page}&pageSize={pageSize}";

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    url += $"&searchTerm={Uri.EscapeDataString(searchTerm)}";
                }

                if (!string.IsNullOrEmpty(maTruong))
                {
                    url += $"&maTruong={Uri.EscapeDataString(maTruong)}";
                }

                if (namSinh.HasValue)
                {
                    url += $"&namSinh={namSinh.Value}";
                }

                if (!string.IsNullOrEmpty(cccd))
                {
                    url += $"&cccd={Uri.EscapeDataString(cccd)}";
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
                throw new Exception("Không thể lấy danh sách lãnh đạo");
            }
            catch (HttpRequestException ex)
            {
                await _logger.LogErrorAsync($"Lỗi HTTP khi lấy danh sách lãnh đạo phân trang: {ex.Message}", ex, nameof(BCLanhDaoDonViService));
                throw;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Lỗi không xác định khi lấy danh sách lãnh đạo phân trang: {ex.Message}", ex, nameof(BCLanhDaoDonViService));
                throw;
            }
        }

        public async Task<bool> CreateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.PostAsJsonAsync($"{API_ENDPOINT}", lanhDao, _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogInfoAsync($"Tạo mới thông tin lãnh đạo thành công: {lanhDao.HoTen}, {lanhDao.CCCD}", nameof(BCLanhDaoDonViService));
                    return true;
                }

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await _logger.LogErrorAsync($"Lỗi khi tạo thông tin lãnh đạo: {errorContent}", null, nameof(BCLanhDaoDonViService));
                    return false;
                }

                await HandleErrorResponse(response);
                return false;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Lỗi khi tạo mới thông tin lãnh đạo", ex, nameof(BCLanhDaoDonViService));
                throw;
            }
        }

        public async Task<bool> UpdateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.PutAsJsonAsync(
                    $"{API_ENDPOINT}/ldtruong/{Uri.EscapeDataString(lanhDao.MaTruong)}/cccd/{Uri.EscapeDataString(lanhDao.CCCD)}",
                    lanhDao,
                    _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogInfoAsync($"Cập nhật thông tin lãnh đạo thành công: {lanhDao.HoTen}, {lanhDao.CCCD}", nameof(BCLanhDaoDonViService));
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
                await _logger.LogErrorAsync($"Lỗi khi cập nhật thông tin lãnh đạo. MaTruong: {lanhDao.MaTruong}, CCCD: {lanhDao.CCCD}",
                    ex, nameof(BCLanhDaoDonViService));
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string maTruong, string cccd)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.DeleteAsync($"{API_ENDPOINT}/ldtruong/{Uri.EscapeDataString(maTruong)}/cccd/{Uri.EscapeDataString(cccd)}");

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogInfoAsync($"Xóa thông tin lãnh đạo thành công. MaTruong: {maTruong}, CCCD: {cccd}", nameof(BCLanhDaoDonViService));
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
                await _logger.LogErrorAsync($"Lỗi khi xóa thông tin lãnh đạo. MaTruong: {maTruong}, CCCD: {cccd}", ex, nameof(BCLanhDaoDonViService));
                throw;
            }
        }

        public async Task<bool> ExistsAsync(string maTruong, string cccd)
        {
            try
            {
                await AddAuthenticationHeader();
                var url = $"{API_ENDPOINT}/check-exist-lanhdao?maTruong={Uri.EscapeDataString(maTruong)}" +
                          $"&cccd={Uri.EscapeDataString(cccd)}";

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
                await _logger.LogErrorAsync($"Lỗi khi kiểm tra tồn tại thông tin lãnh đạo. MaTruong: {maTruong}, CCCD: {cccd}",
                    ex, nameof(BCLanhDaoDonViService));
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
                await _logger.LogErrorAsync("Lỗi HTTP khi lấy danh sách quận: {Message}", ex, nameof(BCLanhDaoDonViService));
                throw;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Lỗi không xác định khi lấy danh sách quận {Message}", ex, nameof(BCLanhDaoDonViService));
                throw;
            }
        }
        public async Task<IEnumerable<QuanModel>> LoadDanhSachQuan_LanhDaoDonVi()
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
                await _logger.LogErrorAsync("Lỗi HTTP khi lấy danh sách quận: {Message}", ex, nameof(BCLanhDaoDonViService));
                throw;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Lỗi không xác định khi lấy danh sách quận {Message}", ex, nameof(BCLanhDaoDonViService));
                throw;
            }
        }
    }
}