using Blazored.LocalStorage;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.Users;
using KhaoThi_2024_net_client.Services.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace KhaoThi_2024_net_client.Services.User
{
    public class KhaoThiUserService : BaseService, IUserService
    {
        private const string API_ENDPOINT = "KhaoThiUser";
        private readonly JsonSerializerOptions _jsonOptions;
        public KhaoThiUserService(
           HttpClient httpClient,
           ILocalStorageService localStorage,
           ILoggingService logger) : base(httpClient, localStorage, logger)
        {
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }
        public async Task<PaginatedResult<KhaoThiUserModel>> GetPaginatedAsync(int page, int pageSize, string? searchTerm)
        {
            try
            {
                await AddAuthenticationHeader();
                var url = $"{API_ENDPOINT}?page={page}&pageSize={pageSize}";
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    url += $"&searchTerm={Uri.EscapeDataString(searchTerm)}";
                }

                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PaginatedResult<KhaoThiUserModel>>(_jsonOptions);
                    if (result != null)
                    {
                        return result;
                    }
                }

                await HandleErrorResponse(response);
                throw new Exception("Không thể lấy danh sách người dùng");
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Lỗi khi lấy danh sách người dùng phân trang", ex, nameof(KhaoThiUserService));
                throw;
            }
        }
        public async Task<KhaoThiUserModel?> GetByIdAsync(int id)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.GetAsync($"{API_ENDPOINT}/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<KhaoThiUserModel>(_jsonOptions);
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
                await _logger.LogErrorAsync($"Lỗi khi lấy thông tin người dùng ID: {id}", ex, nameof(KhaoThiUserService));
                throw;
            }
        }
        public async Task<KhaoThiUserModel> CreateAsync(KhaoThiUserModel user)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.PostAsJsonAsync(API_ENDPOINT, user, _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    var createdUser = await response.Content.ReadFromJsonAsync<KhaoThiUserModel>(_jsonOptions);
                    if (createdUser != null)
                    {
                        await _logger.LogInfoAsync($"Tạo mới người dùng thành công: {createdUser.ID}", nameof(KhaoThiUserService));
                        return createdUser;
                    }
                }

                await HandleErrorResponse(response);
                throw new Exception("Không thể tạo người dùng mới");
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Lỗi khi tạo mới người dùng", ex, nameof(KhaoThiUserService));
                throw;
            }
        }
        public async Task<bool> UpdateAsync(KhaoThiUserModel user)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.PutAsJsonAsync($"{API_ENDPOINT}/{user.ID}", user, _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogInfoAsync($"Cập nhật người dùng thành công: {user.ID}", nameof(KhaoThiUserService));
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
                await _logger.LogErrorAsync($"Lỗi khi cập nhật người dùng ID: {user.ID}", ex, nameof(KhaoThiUserService));
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
                    await _logger.LogInfoAsync($"Xóa người dùng thành công: {id}", nameof(KhaoThiUserService));
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
                await _logger.LogErrorAsync($"Lỗi khi xóa người dùng ID: {id}", ex, nameof(KhaoThiUserService));
                throw;
            }
        }
        public async Task<IEnumerable<KhaoThiUserModel>> GetByMaDonViAsync(string maDonVi)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.GetAsync($"{API_ENDPOINT}/by-donvi/{Uri.EscapeDataString(maDonVi)}");

                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadFromJsonAsync<IEnumerable<KhaoThiUserModel>>(_jsonOptions);
                    return users ?? Enumerable.Empty<KhaoThiUserModel>();
                }

                await HandleErrorResponse(response);
                return Enumerable.Empty<KhaoThiUserModel>();
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Lỗi khi lấy danh sách người dùng theo mã đơn vị: {maDonVi}", ex, nameof(KhaoThiUserService));
                throw;
            }
        }
        public async Task BulkInsertUsersAsync(IEnumerable<KhaoThiUserModel> users)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.PostAsJsonAsync($"{API_ENDPOINT}/bulk-insert", users, _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogInfoAsync($"Thêm {users.Count()} người dùng thành công", nameof(KhaoThiUserService));
                    return;
                }

                await HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Lỗi khi thêm nhiều người dùng", ex, nameof(KhaoThiUserService));
                throw;
            }
        }
        public async Task BulkUpdateUsersAsync(IEnumerable<KhaoThiUserModel> users)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.PutAsJsonAsync($"{API_ENDPOINT}/bulk-update", users, _jsonOptions);

                if (response.IsSuccessStatusCode)
                {
                    await _logger.LogInfoAsync($"Cập nhật {users.Count()} người dùng thành công", nameof(KhaoThiUserService));
                    return;
                }

                await HandleErrorResponse(response);
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync("Lỗi khi cập nhật nhiều người dùng", ex, nameof(KhaoThiUserService));
                throw;
            }
        }
        public async Task<bool> IsUsernameExistAsync(string username)
        {
            try
            {
                await AddAuthenticationHeader();
                var response = await _httpClient.GetAsync($"{API_ENDPOINT}/check-username/{Uri.EscapeDataString(username)}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<bool>();
                }

                await HandleErrorResponse(response);
                return false;
            }
            catch (Exception ex)
            {
                await _logger.LogErrorAsync($"Lỗi khi kiểm tra tên đăng nhập: {username}", ex, nameof(KhaoThiUserService));
                throw;
            }
        }

    }
}
