using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT.DTOs;

namespace khaothi_2024_net_server.Core.Interfaces
{
    public interface IBCThongTinDonViService
    {
        Task<bool> UpdateAsync(KhaoThi_1_THPT_ThongTin_DonViModel ThongTin);
        Task<(IEnumerable<KhaoThi_1_THPT_ThongTin_DonViModel> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? searchTerm = null);
        Task<KhaoThi_1_THPT_ThongTin_DonViModel> GetByMaTruongAsync(string MaTruong);
    }
}
