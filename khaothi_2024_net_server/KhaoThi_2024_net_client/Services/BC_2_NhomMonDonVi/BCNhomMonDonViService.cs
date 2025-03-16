using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Models.Shares;

namespace KhaoThi_2024_net_client.Services.BC_2_NhomMonDonVi
{
    public class BCNhomMonDonViService : IBCNhomMonDonViService
    {
        public Task<int> CreateAsync(KhaoThi_2_THPT_NhomMon_DonViModel nhomMon)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<KhaoThi_2_THPT_NhomMon_DonViModel>> GetByMaTruongAsync(string maTruong)
        {
            throw new NotImplementedException();
        }

        public Task<PaginatedResult<KhaoThi_2_THPT_NhomMon_DonViModel>> GetPaginatedAsync(int page, int pageSize, string? searchTerm = null, string? maTruong = null, int? minSoLuong = null)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsNhomMonExistAsync(string maTruong, string monLuaChon1, string monLuaChon2)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<QuanModel>> LoadDanhSachQuan()
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(KhaoThi_2_THPT_NhomMon_DonViModel nhomMon)
        {
            throw new NotImplementedException();
        }
    }
}
