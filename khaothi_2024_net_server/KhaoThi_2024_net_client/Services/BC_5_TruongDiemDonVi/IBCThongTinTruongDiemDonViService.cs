using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;

namespace KhaoThi_2024_net_client.Services.BC_5_TruongDiemDonVi
{
    public interface IBCThongTinTruongDiemDonViService
    {
        /// <summary>
        /// Lấy danh sách thông tin trường điểm có phân trang và tìm kiếm
        /// </summary>
        /// <param name="page">Trang hiện tại</param>
        /// <param name="pageSize">Số lượng item trên một trang</param>
        /// <param name="searchTerm">Từ khóa tìm kiếm (tùy chọn)</param>
        /// <param name="maTruong">Mã trường để lọc (tùy chọn)</param>
        /// <returns>Kết quả phân trang gồm danh sách thông tin trường điểm và thông tin phân trang</returns>
        Task<PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null);

        /// <summary>
        /// Cập nhật thông tin trường điểm
        /// </summary>
        /// <param name="truongDiem">Thông tin trường điểm cần cập nhật</param>
        /// <returns>true nếu cập nhật thành công, false nếu thất bại</returns>
        Task<bool> UpdateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel truongDiem);
    }
}
