using khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT.DTOs;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT.Interfaces
{
    public interface IBCThongTinConThiService
    {
        /// <summary>
        /// Thêm mới thông tin con thí sinh vào cơ sở dữ liệu.
        /// </summary>
        /// <param name="ThongTin">Đối tượng chứa thông tin con thí sinh cần thêm mới.</param>
        /// <returns>Trả về true nếu thêm mới thành công, ngược lại trả về false.</returns>
        Task<bool> InsertAsync(KhaoThi_5_THPT_ThongTin_ConThi ThongTin);

        /// <summary>
        /// Cập nhật thông tin con thí sinh trong cơ sở dữ liệu.
        /// </summary>
        /// <param name="ThongTin">Đối tượng chứa thông tin con thí sinh cần cập nhật.</param>
        /// <returns>Trả về true nếu cập nhật thành công, ngược lại trả về false.</returns>
        Task<bool> UpdateAsync(KhaoThi_5_THPT_ThongTin_ConThi ThongTin);

        /// <summary>
        /// Xóa thông tin con thí sinh khỏi cơ sở dữ liệu dựa trên Mã trường, CCCD và Mã định danh của con.
        /// </summary>
        /// <param name="MaTruong">Mã trường.</param>
        /// <param name="CCCD">CCCD.</param>
        /// <param name="MaDinhDanhCuaCon">Mã định danh của con thí sinh.</param>
        /// <returns>Trả về true nếu xóa thành công, ngược lại trả về false.</returns>
        Task<bool> DeleteAsync(string MaTruong, string CCCD, string MaDinhDanhCuaCon);

        /// <summary>
        /// Lấy danh sách thông tin con thí sinh phân trang.
        /// </summary>
        /// <param name="page">Số trang.</param>
        /// <param name="pageSize">Kích thước trang.</param>
        /// <param name="searchTerm">Từ khóa tìm kiếm (tùy chọn).</param>
        /// <returns>Trả về một tuple chứa danh sách các mục và tổng số mục.</returns>
        Task<(IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? searchTerm = null);

        /// <summary>
        /// Lấy thông tin con thí sinh theo Mã trường, CCCD và Mã định danh của con.
        /// </summary>
        /// <param name="MaTruong">Mã trường.</param>
        /// <param name="CCCD">CCCD.</param>
        /// <param name="MaDinhDanhCuaCon">Mã định danh của con thí sinh.</param>
        /// <returns>Trả về đối tượng chứa thông tin con thí sinh nếu tìm thấy, ngược lại trả về null.</returns>
        Task<KhaoThi_5_THPT_ThongTin_ConThi?> GetByKeysAsync(string MaTruong, string CCCD, string MaDinhDanhCuaCon);

        /// <summary>
        /// Lấy toàn bộ thông tin con thí sinh.
        /// </summary>
        /// <returns>Trả về danh sách chứa toàn bộ thông tin con thí sinh.</returns>
        Task<IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi>> GetAllAsync();

        /// <summary>
        /// Kiểm tra xem thông tin con thí sinh đã tồn tại trong cơ sở dữ liệu hay chưa.
        /// </summary>
        /// <param name="MaTruong">Mã trường.</param>
        /// <param name="CCCD">CCCD.</param>
        /// <param name="MaDinhDanhCuaCon">Mã định danh của con thí sinh.</param>
        /// <returns>Trả về true nếu thông tin con thí sinh đã tồn tại, ngược lại trả về false.</returns>
        Task<bool> ExistsAsync(string MaTruong, string CCCD, string MaDinhDanhCuaCon);
    }
}
