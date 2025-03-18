using khaothi_2024_net_server.Features.BaoCaoSoLieuLanhDaoTHPT.DTOs;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuLanhDaoTHPT.Interfaces
{
    public interface IBCLanhDaoDonViService
    {
        Task<KhaoThi_4_THPT_ThongTin_LanhDaoModel?> GetByMaTruongAndCCCDAsync(string maTruong, string cccd);
        Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetByMaTruongAsync(string maTruong);
        Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetAllAsync();
        Task<bool> CreateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao);
        Task<bool> UpdateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao);
        Task<bool> DeleteAsync(string maTruong, string cccd);
        Task<bool> ExistsAsync(string maTruong, string cccd);

        Task<(IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel> Items, int TotalCount)> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null,
            int? namSinh = null,
            string? cccd = null
        );
    }
}
