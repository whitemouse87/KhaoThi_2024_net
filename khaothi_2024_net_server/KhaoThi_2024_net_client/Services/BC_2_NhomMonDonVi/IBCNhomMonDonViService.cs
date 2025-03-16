using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Models.Shares;

namespace KhaoThi_2024_net_client.Services.BC_2_NhomMonDonVi
{
    public interface IBCNhomMonDonViService
    {
        /// <summary>
        /// Lấy danh sách nhóm môn theo mã trường
        /// </summary>
        /// <param name="maTruong">Mã trường cần tìm</param>
        /// <returns>Danh sách nhóm môn của trường</returns>
        Task<IEnumerable<KhaoThi_2_THPT_NhomMon_DonViModel>> GetByMaTruongAsync(string maTruong);

        /// <summary>
        /// Lấy danh sách nhóm môn có phân trang và tìm kiếm
        /// </summary>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Số lượng item trên một trang</param>
        /// <param name="searchTerm">Từ khóa tìm kiếm</param>
        /// <param name="maTruong">Mã trường để lọc (tùy chọn)</param>
        /// <param name="minSoLuong">Số lượng tối thiểu để lọc (tùy chọn)</param>
        /// <returns>Kết quả phân trang gồm danh sách nhóm môn và thông tin phân trang</returns>
        Task<PaginatedResult<KhaoThi_2_THPT_NhomMon_DonViModel>> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null,
            int? minSoLuong = null);

        /// <summary>
        /// Tạo mới nhóm môn
        /// </summary>
        /// <param name="nhomMon">Thông tin nhóm môn cần tạo</param>
        /// <returns>ID của nhóm môn mới được tạo, -1 nếu có lỗi</returns>
        Task<int> CreateAsync(KhaoThi_2_THPT_NhomMon_DonViModel nhomMon);

        /// <summary>
        /// Cập nhật số lượng cho nhóm môn
        /// </summary>
        /// <param name="nhomMon">Thông tin nhóm môn cần cập nhật</param>
        /// <returns>true nếu cập nhật thành công, false nếu thất bại</returns>
        Task<bool> UpdateAsync(KhaoThi_2_THPT_NhomMon_DonViModel nhomMon);

        /// <summary>
        /// Xóa nhóm môn
        /// </summary>
        /// <param name="id">ID của nhóm môn cần xóa</param>
        /// <returns>true nếu xóa thành công, false nếu thất bại</returns>
        Task<bool> DeleteAsync(int id);

        /// <summary>
        /// Kiểm tra nhóm môn đã tồn tại chưa
        /// </summary>
        /// <param name="maTruong">Mã trường</param>
        /// <param name="monLuaChon1">Môn lựa chọn 1</param>
        /// <param name="monLuaChon2">Môn lựa chọn 2</param>
        /// <returns>true nếu nhóm môn đã tồn tại, false nếu chưa tồn tại</returns>
        Task<bool> IsNhomMonExistAsync(string maTruong, string monLuaChon1, string monLuaChon2);

        /// <summary>
        /// Lấy danh sách môn học để tạo tổ hợp
        /// </summary>
        /// <returns>Danh sách môn học</returns>
        Task<IEnumerable<QuanModel>> LoadDanhSachQuan();
    }
}
