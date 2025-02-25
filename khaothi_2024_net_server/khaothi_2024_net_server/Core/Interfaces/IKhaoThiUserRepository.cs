using khaothi_2024_net_server.Features.UserManagement.DTOs;
namespace khaothi_2024_net_server.Core.Interfaces
{
    public interface IKhaoThiUserRepository
    {
        Task<KhaoThiUser> GetByIdAsync(int id);
        Task<KhaoThiUser> GetByUsernameAsync(string username);
        Task<IEnumerable<KhaoThiUser>> GetAllAsync();
        Task<(IEnumerable<KhaoThiUser> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? searchTerm = null);
        Task<int> CreateAsync(KhaoThiUser user);
        Task<bool> UpdateAsync(KhaoThiUser user);
        Task<bool> DeleteAsync(int id);
        Task<bool> IsUsernameExistAsync(string username);
        Task<IEnumerable<KhaoThiUser>> GetByMaDonViAsync(string maDonVi);
        Task<KhaoThiUser> GetByRefreshTokenAsync(string refreshToken);
        Task BulkInsertUsers(IEnumerable<KhaoThiUser> users);
        Task BulkUpdateUsers(IEnumerable<KhaoThiUser> users);
        Task<bool> UpdatePasswordAsync(int userId, string newHashedPassword);
        Task<string> GetPasswordHashAsync(int userId);
    }
}
