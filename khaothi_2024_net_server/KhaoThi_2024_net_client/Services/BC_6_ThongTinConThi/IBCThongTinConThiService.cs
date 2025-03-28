using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;

namespace khaothi_2024_net_client.Services.BC_5_ThongTinConThi
{
    public interface IBCThongTinConThiService
    {
        /// <summary>
        /// Lấy danh sách con thi có phân trang và tìm kiếm
        /// </summary>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Số lượng item trên một trang</param>
        /// <param name="searchTerm">Từ khóa tìm kiếm</param>
        /// <param name="maTruong">Mã trường để lọc (tùy chọn)</param>
        /// <param name="kyThiThamDu">Kỳ thi tham dự để lọc (tùy chọn)</param>
        /// <returns>Kết quả phân trang gồm danh sách con thi và thông tin phân trang</returns>
        Task<PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThiModel>> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null,
            string? kyThiThamDu = null);

        /// <summary>
        /// Lấy danh sách con thi theo mã trường
        /// </summary>
        /// <param name="maTruong">Mã trường cần tìm</param>
        /// <returns>Danh sách con thi của trường</returns>
        Task<IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel>> GetByMaTruongAsync(string maTruong);

        /// <summary>
        /// Lấy thông tin chi tiết của một con thi
        /// </summary>
        /// <param name="maTruong">Mã trường</param>
        /// <param name="cccd">CCCD của phụ huynh</param>
        /// <param name="maDinhDanhCuaCon">Mã định danh của con</param>
        /// <returns>Thông tin chi tiết của con thi</returns>
        Task<KhaoThi_5_THPT_ThongTin_ConThiModel?> GetThongTinCaNhan(string maTruong, string cccd, string maDinhDanhCuaCon);

        /// <summary>
        /// Tạo mới thông tin con thi
        /// </summary>
        /// <param name="conThi">Thông tin con thi cần tạo</param>
        /// <returns>ID của con thi mới được tạo, -1 nếu có lỗi</returns>
        Task<bool> CreateAsync(KhaoThi_5_THPT_ThongTin_ConThiModel conThi);

        /// <summary>
        /// Cập nhật thông tin con thi
        /// </summary>
        /// <param name="conThi">Thông tin con thi cần cập nhật</param>
        /// <returns>true nếu cập nhật thành công, false nếu thất bại</returns>
        Task<bool> UpdateAsync(KhaoThi_5_THPT_ThongTin_ConThiModel conThi);

        /// <summary>
        /// Xóa thông tin con thi
        /// </summary>
        /// <param name="maTruong">Mã trường</param>
        /// <param name="cccd">CCCD của phụ huynh</param>
        /// <param name="maDinhDanhCuaCon">Mã định danh của con</param>
        /// <returns>true nếu xóa thành công, false nếu thất bại</returns>
        Task<bool> DeleteAsync(string maTruong, string cccd, string maDinhDanhCuaCon);

        /// <summary>
        /// Kiểm tra thông tin con thi đã tồn tại chưa
        /// </summary>
        /// <param name="maTruong">Mã trường</param>
        /// <param name="cccd">CCCD của phụ huynh</param>
        /// <param name="maDinhDanhCuaCon">Mã định danh của con</param>
        /// <returns>true nếu con thi đã tồn tại, false nếu chưa tồn tại</returns>
        Task<bool> IsConThiExistAsync(string maTruong, string cccd, string maDinhDanhCuaCon);
    }
}