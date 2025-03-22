using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Models.Shares;

namespace KhaoThi_2024_net_client.Services.BC_4_LanhDaoDonVi
{
    public interface IBCLanhDaoDonViService
    {
        /// <summary>
        /// Lấy thông tin lãnh đạo theo mã trường và CCCD
        /// </summary>
        /// <param name="maTruong">Mã trường cần tìm</param>
        /// <param name="cccd">CCCD của lãnh đạo</param>
        /// <returns>Thông tin lãnh đạo, null nếu không tìm thấy</returns>
        Task<KhaoThi_4_THPT_ThongTin_LanhDaoModel?> GetByMaTruongAndCCCDAsync(string maTruong, string cccd);

        /// <summary>
        /// Lấy danh sách lãnh đạo theo mã trường
        /// </summary>
        /// <param name="maTruong">Mã trường cần tìm</param>
        /// <returns>Danh sách lãnh đạo của trường</returns>
        Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetByMaTruongAsync(string maTruong);

        /// <summary>
        /// Lấy toàn bộ danh sách lãnh đạo
        /// </summary>
        /// <returns>Danh sách tất cả lãnh đạo</returns>
        Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetAllAsync();

        /// <summary>
        /// Lấy danh sách lãnh đạo có phân trang và tìm kiếm
        /// </summary>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Số lượng item trên một trang</param>
        /// <param name="searchTerm">Từ khóa tìm kiếm (tùy chọn)</param>
        /// <param name="maTruong">Mã trường để lọc (tùy chọn)</param>
        /// <param name="namSinh">Năm sinh để lọc (tùy chọn)</param>
        /// <param name="cccd">CCCD để lọc (tùy chọn)</param>
        /// <returns>Kết quả phân trang gồm danh sách lãnh đạo và thông tin phân trang</returns>
        Task<PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null,
            int? namSinh = null,
            string? cccd = null);

        /// <summary>
        /// Tạo mới thông tin lãnh đạo
        /// </summary>
        /// <param name="lanhDao">Thông tin lãnh đạo cần tạo</param>
        /// <returns>true nếu tạo thành công, false nếu thất bại</returns>
        Task<bool> CreateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao);

        /// <summary>
        /// Cập nhật thông tin lãnh đạo
        /// </summary>
        /// <param name="lanhDao">Thông tin lãnh đạo cần cập nhật</param>
        /// <returns>true nếu cập nhật thành công, false nếu thất bại</returns>
        Task<bool> UpdateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao);

        /// <summary>
        /// Xóa thông tin lãnh đạo
        /// </summary>
        /// <param name="maTruong">Mã trường của lãnh đạo cần xóa</param>
        /// <param name="cccd">CCCD của lãnh đạo cần xóa</param>
        /// <returns>true nếu xóa thành công, false nếu thất bại</returns>
        Task<bool> DeleteAsync(string maTruong, string cccd);

        /// <summary>
        /// Kiểm tra thông tin lãnh đạo đã tồn tại chưa
        /// </summary>
        /// <param name="maTruong">Mã trường</param>
        /// <param name="cccd">CCCD của lãnh đạo</param>
        /// <returns>true nếu thông tin lãnh đạo đã tồn tại, false nếu chưa tồn tại</returns>
        Task<bool> ExistsAsync(string maTruong, string cccd);

        /// <summary>
        /// Lấy danh sách quận
        /// </summary>
        /// <returns>Danh sách quận</returns>
        Task<IEnumerable<QuanModel>> LoadDanhSachQuan_LanhDaoDonVi();
    }
}
