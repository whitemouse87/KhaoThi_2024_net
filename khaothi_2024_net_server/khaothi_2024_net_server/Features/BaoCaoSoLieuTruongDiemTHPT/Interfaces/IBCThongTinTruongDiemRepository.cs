using khaothi_2024_net_server.Features.BaoCaoSoLieuTruongDiemTHPT.DTOs;
using System.Security.Claims;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuTruongDiemTHPT.Interfaces
{
    public interface IBCThongTinTruongDiemRepository
    {
        Task<bool> UpdateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel LanhDaoDiemThi);
        Task<(IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel> Items, int TotalCount)> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null
        ); // Thêm dấu ngoặc đơn đóng tại đây
        Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetByMaTruongAsync(string maTruong);
    }
}