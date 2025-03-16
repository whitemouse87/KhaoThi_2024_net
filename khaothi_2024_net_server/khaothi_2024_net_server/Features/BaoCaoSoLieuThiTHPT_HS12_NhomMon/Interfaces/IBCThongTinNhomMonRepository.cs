using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT_HS12_NhomMon.DTOs;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT_HS12_NhomMon.Interfaces
{
    public interface IBCThongTinNhomMonRepository
    {
        Task<bool> UpdateAsync(KhaoThi_2_THPT_NhomMonModel NhomMon);
        Task<bool> DeleteAsync(int id);
        Task<int> CreateAsync(KhaoThi_2_THPT_NhomMonModel NhomMon);
        Task<(IEnumerable<KhaoThi_2_THPT_NhomMonModel> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? searchTerm = null, string? maTruong = null, int? minSoLuong = null);

        Task<IEnumerable<KhaoThi_2_THPT_NhomMonModel>> GetByMaDonViAsync(string MaTruong);
        Task<bool> IsNhomMonExistAsync(string MaTruong, string MonLuaChon1, string MonLuaChon2);
    }
}
