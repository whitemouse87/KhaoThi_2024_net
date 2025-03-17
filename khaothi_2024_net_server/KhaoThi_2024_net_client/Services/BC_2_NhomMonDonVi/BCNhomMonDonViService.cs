using Blazored.LocalStorage;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Models.Shares;
using KhaoThi_2024_net_client.Services.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace KhaoThi_2024_net_client.Services.BC_2_NhomMonDonVi
{
    public class BCNhomMonDonViService : BaseService, IBCNhomMonDonViService
    {
        private const string API_ENDPOINT = "BCThongTinNhomMon";
        private readonly JsonSerializerOptions _jsonOptions;

        public BCNhomMonDonViService(
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
        public async Task<int> CreateAsync(KhaoThi_2_THPT_NhomMon_DonViModel nhomMon)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.PostAsJsonAsync($"{API_ENDPOINT}", nhomMon, _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    var createdNhomMon = await response.Content.ReadFromJsonAsync<KhaoThi_2_THPT_NhomMon_DonViModel>(_jsonOptions);
                    if (createdNhomMon != null)
                    {
                        await _logger.LogInfoAsync($"Tạo mới nhóm môn thành công: {createdNhomMon.ID}", nameof(BCNhomMonDonViService));
                        return createdNhomMon.ID;
                    }
                }

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    await _logger.LogErrorAsync($"Lỗi khi tạo nhóm môn: {errorContent}", null, nameof(BCNhomMonDonViService));
                    return -1;
                }

                await HandleErrorResponse(response);
                return -1;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Lỗi khi tạo mới nhóm môn", ex, nameof(BCNhomMonDonViService));
                throw;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.DeleteAsync($"{API_ENDPOINT}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogInfoAsync($"Xóa nhóm môn thành công: {id}", nameof(BCNhomMonDonViService));
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
                await _logger.LogErrorAsync($"Lỗi khi xóa nhóm môn ID: {id}", ex, nameof(BCNhomMonDonViService));
                throw;
            }
        }

        public async Task<IEnumerable<KhaoThi_2_THPT_NhomMon_DonViModel>> GetByMaTruongAsync(string maTruong)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.GetAsync($"{API_ENDPOINT}/truong/{Uri.EscapeDataString(maTruong)}");

                if (response.IsSuccessStatusCode)
                {
                    var nhomMons = await response.Content.ReadFromJsonAsync<IEnumerable<KhaoThi_2_THPT_NhomMon_DonViModel>>(_jsonOptions);
                    return nhomMons ?? Enumerable.Empty<KhaoThi_2_THPT_NhomMon_DonViModel>();
                }

                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return Enumerable.Empty<KhaoThi_2_THPT_NhomMon_DonViModel>();
                }

                await HandleErrorResponse(response);
                return Enumerable.Empty<KhaoThi_2_THPT_NhomMon_DonViModel>();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Lỗi khi lấy danh sách nhóm môn theo mã trường: {maTruong}", ex, nameof(BCNhomMonDonViService));
                throw;
            }
        }

        public async Task<PaginatedResult<KhaoThi_2_THPT_NhomMon_DonViModel>> GetPaginatedAsync(
          int page,
          int pageSize,
          string? searchTerm = null,
          string? maTruong = null,
          int? minSoLuong = null)
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

                if (minSoLuong.HasValue)
                {
                    url += $"&minSoLuong={minSoLuong.Value}";
                }

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PaginatedResult<KhaoThi_2_THPT_NhomMon_DonViModel>>(_jsonOptions);
                    if (result != null)
                    {
                        return result;
                    }
                }

                await HandleErrorResponse(response);
                throw new Exception("Không thể lấy danh sách nhóm môn");
            }
            catch (HttpRequestException ex)
            {
                await _logger.LogErrorAsync($"Lỗi HTTP khi lấy danh sách nhóm môn phân trang: {ex.Message}", ex, nameof(BCNhomMonDonViService));
                throw;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Lỗi không xác định khi lấy danh sách nhóm môn phân trang: {ex.Message}", ex, nameof(BCNhomMonDonViService));
                throw;
            }
        }

        public async Task<bool> IsNhomMonExistAsync(string maTruong, string monLuaChon1, string monLuaChon2)
        {
            try
            {
                await AddAuthenticationHeader();
                var url = $"{API_ENDPOINT}/check-exist?maTruong={Uri.EscapeDataString(maTruong)}" +
                          $"&monLuaChon1={Uri.EscapeDataString(monLuaChon1)}" +
                          $"&monLuaChon2={Uri.EscapeDataString(monLuaChon2)}";

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
                await _logger.LogErrorAsync($"Lỗi khi kiểm tra tồn tại nhóm môn: {maTruong}, {monLuaChon1}, {monLuaChon2}", ex, nameof(BCNhomMonDonViService));
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
                await _logger.LogErrorAsync("Lỗi HTTP khi lấy danh sách quận: {Message}", ex, nameof(BCNhomMonDonViService));
                throw;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Lỗi không xác định khi lấy danh sách quận {Message}", ex, nameof(BCNhomMonDonViService));
                throw;
            }
        }

        public async Task<bool> UpdateAsync(KhaoThi_2_THPT_NhomMon_DonViModel nhomMon)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.PutAsJsonAsync($"{API_ENDPOINT}/{nhomMon.ID}", nhomMon, _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogInfoAsync($"Cập nhật nhóm môn thành công: {nhomMon.ID}", nameof(BCNhomMonDonViService));
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
                await _logger.LogErrorAsync($"Lỗi khi cập nhật nhóm môn ID: {nhomMon.ID}", ex, nameof(BCNhomMonDonViService));
                throw;
            }
        }
    }
}
