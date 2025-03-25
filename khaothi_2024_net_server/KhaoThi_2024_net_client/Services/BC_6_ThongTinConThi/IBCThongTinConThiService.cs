using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;

namespace KhaoThi_2024_net_client.Services.BC_6_ThongTinConThi
{
    public interface IBCThongTinConThiService
    {
        /// <summary>
        /// Lấy thông tin con thí sinh theo mã trường, CCCD và mã định danh của con
        /// </summary>
        /// <param name="maTruong">Mã trường</param>
        /// <param name="cccd">CCCD của phụ huynh</param>
        /// <param name="maDinhDanhCuaCon">Mã định danh của con</param>
        /// <returns>Thông tin con thí sinh, null nếu không tìm thấy</returns>
        Task<KhaoThi_5_THPT_ThongTin_ConThi?> GetByKeysAsync(string maTruong, string cccd, string maDinhDanhCuaCon);

        /// <summary>
        /// Lấy danh sách con thí sinh theo mã trường và CCCD
        /// </summary>
        /// <param name="maTruong">Mã trường</param>
        /// <param name="cccd">CCCD của phụ huynh</param>
        /// <returns>Danh sách con thí sinh của phụ huynh</returns>
        Task<IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi>> GetByMaTruongAndCCCDAsync(string maTruong, string cccd);

        /// <summary>
        /// Lấy toàn bộ danh sách con thí sinh
        /// </summary>
        /// <returns>Danh sách tất cả con thí sinh</returns>
        Task<IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi>> GetAllAsync();

        /// <summary>
        /// Lấy danh sách con thí sinh có phân trang và tìm kiếm
        /// </summary>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Số lượng item trên một trang</param>
        /// <param name="searchTerm">Từ khóa tìm kiếm (tùy chọn)</param>
        /// <returns>Kết quả phân trang gồm danh sách con thí sinh và thông tin phân trang</returns>
        Task<PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThi>> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null);

        /// <summary>
        /// Thêm mới thông tin con thí sinh
        /// </summary>
        /// <param name="conThi">Thông tin con thí sinh cần tạo</param>
        /// <returns>true nếu tạo thành công, false nếu thất bại</returns>
        Task<bool> InsertAsync(KhaoThi_5_THPT_ThongTin_ConThi conThi);

        /// <summary>
        /// Cập nhật thông tin con thí sinh
        /// </summary>
        /// <param name="conThi">Thông tin con thí sinh cần cập nhật</param>
        /// <returns>true nếu cập nhật thành công, false nếu thất bại</returns>
        Task<bool> UpdateAsync(KhaoThi_5_THPT_ThongTin_ConThi conThi);

        /// <summary>
        /// Xóa thông tin con thí sinh
        /// </summary>
        /// <param name="maTruong">Mã trường</param>
        /// <param name="cccd">CCCD của phụ huynh</param>
        /// <param name="maDinhDanhCuaCon">Mã định danh của con</param>
        /// <returns>true nếu xóa thành công, false nếu thất bại</returns>
        Task<bool> DeleteAsync(string maTruong, string cccd, string maDinhDanhCuaCon);

        /// <summary>
        /// Kiểm tra thông tin con thí sinh đã tồn tại chưa
        /// </summary>
        /// <param name="maTruong">Mã trường</param>
        /// <param name="cccd">CCCD của phụ huynh</param>
        /// <param name="maDinhDanhCuaCon">Mã định danh của con</param>
        /// <returns>true nếu thông tin con thí sinh đã tồn tại, false nếu chưa tồn tại</returns>
        Task<bool> ExistsAsync(string maTruong, string cccd, string maDinhDanhCuaCon);
    }
}
