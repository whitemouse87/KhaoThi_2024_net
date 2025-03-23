using khaothi_2024_net_server.Features.BaoCaoSoLieuTruongDiemTHPT.DTOs;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuTruongDiemTHPT.Interfaces
{
    public interface IBCThongTinTruongDiemService
    {
        Task<bool> UpdateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel LanhDaoDiemThi);
        Task<(IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel> Items, int TotalCount)> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null
        );
    }
}
