using khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT.DTOs;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT.Interfaces
{
    public interface IBCThongTinConThiService
    {
        Task<bool> UpdateAsync(KhaoThi_5_THPT_ThongTin_ConThiModel conThi);
        Task<bool> DeleteAsync(string maTruong, string cccd, string maDinhDanhCuaCon);
        Task<bool> CreateAsync(KhaoThi_5_THPT_ThongTin_ConThiModel conThi);
        Task<(IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel> Items, int TotalCount)> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null,
            string? kyThiThamDu = null
        );

        Task<IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel>> GetByMaTruongAsync(string maTruong);
        Task<KhaoThi_5_THPT_ThongTin_ConThiModel?> GetThongTinCaNhan(string maTruong, string cccd, string maDinhDanhCuaCon);
        Task<bool> IsConThiExistAsync(string maTruong, string cccd, string maDinhDanhCuaCon);
    }
}
