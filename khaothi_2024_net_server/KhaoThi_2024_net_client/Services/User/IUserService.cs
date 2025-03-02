using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.Shares;
using KhaoThi_2024_net_client.Models.Users;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace KhaoThi_2024_net_client.Services.User
{

    // Services/User/IUserService.cs
    public interface IUserService
    {
        /// <summary>
        /// Lấy danh sách người dùng có phân trang và tìm kiếm
        /// </summary>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Số lượng item trên một trang</param>
        /// <param name="searchTerm">Từ khóa tìm kiếm</param>
        /// <returns>Tuple chứa danh sách user và tổng số lượng</returns>
        Task<PaginatedResult<KhaoThiUserModel>> GetPaginatedAsync(int page, int pageSize, string? searchTerm = null);
        Task<IEnumerable<KhaoThiUserModel>> GetAllAsync();
        /// <summary>
        /// Lấy thông tin người dùng theo ID
        /// </summary>
        /// <param name="id">ID của người dùng</param>
        Task<KhaoThiUserModel?> GetByIdAsync(int id);

        /// <summary>
        /// Tạo mới người dùng
        /// </summary>
        /// <param name="user">Thông tin người dùng cần tạo</param>
        Task<KhaoThiUserModel> CreateAsync(KhaoThiUserModel user);

        /// <summary>
        /// Cập nhật thông tin người dùng
        /// </summary>
        /// <param name="user">Thông tin người dùng cần cập nhật</param>
        /// <returns>true nếu cập nhật thành công, false nếu không tìm thấy user</returns>
        Task<bool> UpdateAsync(KhaoThiUserModel user);

        /// <summary>
        /// Xóa người dùng theo ID
        /// </summary>
        /// <param name="id">ID của người dùng cần xóa</param>
        /// <returns>true nếu xóa thành công, false nếu không tìm thấy user</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Lấy danh sách người dùng theo mã đơn vị
        /// </summary>
        /// <param name="maDonVi">Mã đơn vị cần tìm</param>
        Task<IEnumerable<KhaoThiUserModel>> GetByMaDonViAsync(string maDonVi);

        /// <summary>
        /// Thêm nhiều người dùng cùng lúc
        /// </summary>
        /// <param name="users">Danh sách người dùng cần thêm</param>
        Task BulkInsertUsersAsync(IEnumerable<KhaoThiUserModel> users);

        /// <summary>
        /// Cập nhật nhiều người dùng cùng lúc
        /// </summary>
        /// <param name="users">Danh sách người dùng cần cập nhật</param>
        Task BulkUpdateUsersAsync(IEnumerable<KhaoThiUserModel> users);

        /// <summary>
        /// Kiểm tra tên đăng nhập đã tồn tại
        /// </summary>
        /// <param name="username">Tên đăng nhập cần kiểm tra</param>
        /// <returns>true nếu tên đăng nhập đã tồn tại</returns>
        Task<bool> IsUsernameExistAsync(string username);
        Task<bool> ChangePassword(int id, ChangePasswordModel model);
        Task<bool> ChangeActive(int id, bool active);
        Task<IEnumerable<NganHangModel>> LoadDanhSachNganHang();
    }

}
