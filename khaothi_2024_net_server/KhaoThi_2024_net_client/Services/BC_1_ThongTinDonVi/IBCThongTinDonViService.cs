using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Models.Users;

namespace KhaoThi_2024_net_client.Services.BC_1_ThongTinDonVi
{
    public interface IBCThongTinDonViService
    {
        Task<bool> UpdateAsync(KhaoThi_1_THPT_ThongTin_DonViModel ThongTin);


        Task<KhaoThi_1_THPT_ThongTin_DonViModel?> GetByMaTruongAsync(string MaTruong);
        Task<PaginatedResult<KhaoThi_1_THPT_ThongTin_DonViModel>> GetPaginatedAsync(int page, int pageSize, string? searchTerm = null);
    }
}
