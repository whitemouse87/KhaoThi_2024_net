using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT_HS12_NhomMon.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT_HS12_NhomMon.Interfaces;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT_HS12_NhomMon
{
    public class BCThongTinNhomMonService : IBCThongTinNhomMonService
    {
        private readonly IBCThongTinNhomMonRepository _TTDonViNhomMonRepository;
        private readonly ILogger<BCThongTinNhomMonService> _logger;
        /// <summary>
        /// Constructor với Dependency Injection
        /// </summary>
        public BCThongTinNhomMonService(
            IBCThongTinNhomMonRepository TTDonViNhomMonRepository,
            ILogger<BCThongTinNhomMonService> logger)
        {
            _TTDonViNhomMonRepository = TTDonViNhomMonRepository;
            _logger = logger;

        }
        public async Task<int> CreateAsync(KhaoThi_2_THPT_NhomMonModel nhomMon)
        {
            try
            {
                if (nhomMon == null)
                {
                    _logger.LogError("Không thể tạo nhóm môn với dữ liệu null");
                    return -1;
                }

                if (string.IsNullOrEmpty(nhomMon.MaTruong))
                {
                    _logger.LogError("Không thể tạo nhóm môn khi mã trường không được cung cấp");
                    return -1;
                }

                // Kiểm tra trùng lặp
                bool exists = await _TTDonViNhomMonRepository.IsNhomMonExistAsync(
                    nhomMon.MaTruong,
                    nhomMon.MonLuaChon_1,
                    nhomMon.MonLuaChon_2);

                if (exists)
                {
                    _logger.LogError("Nhóm môn đã tồn tại: {TenNhom}, {MonLuaChon1}, {MonLuaChon2}, MaTruong: {MaTruong}",
                        nhomMon.TenNhom, nhomMon.MonLuaChon_1, nhomMon.MonLuaChon_2, nhomMon.MaTruong);
                    return -1;
                }

                return await _TTDonViNhomMonRepository.CreateAsync(nhomMon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo nhóm môn: {TenNhom}, {MonLuaChon1}, {MonLuaChon2}",
                    nhomMon?.TenNhom, nhomMon?.MonLuaChon_1, nhomMon?.MonLuaChon_2);
                return -1;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("ID không hợp lệ khi xóa nhóm môn: {Id}", id);
                    return false;
                }

                return await _TTDonViNhomMonRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa nhóm môn có ID: {Id}", id);
                return false;
            }
        }

        public async Task<IEnumerable<KhaoThi_2_THPT_NhomMonModel>> GetByMaDonViAsync(string maTruong)
        {
            try
            {
                if (string.IsNullOrEmpty(maTruong))
                {
                    _logger.LogWarning("Không thể lấy nhóm môn khi mã trường trống");
                    return new List<KhaoThi_2_THPT_NhomMonModel>();
                }

                return await _TTDonViNhomMonRepository.GetByMaDonViAsync(maTruong);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách nhóm môn theo mã trường: {MaTruong}", maTruong);
                return new List<KhaoThi_2_THPT_NhomMonModel>();
            }
        }

        public async Task<(IEnumerable<KhaoThi_2_THPT_NhomMonModel> Items, int TotalCount)> GetPaginatedAsync(
             int page,
             int pageSize,
             string? searchTerm = null,
             string? maTruong = null,
             int? minSoLuong = null)
        {
            try
            {
                if (page < 1)
                {
                    page = 1;
                }

                if (pageSize < 1)
                {
                    pageSize = 10;
                }

                return await _TTDonViNhomMonRepository.GetPaginatedAsync(page, pageSize, searchTerm, maTruong, minSoLuong);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách nhóm môn có phân trang. Page: {Page}, PageSize: {PageSize}, SearchTerm: {SearchTerm}, MaTruong: {MaTruong}",
                    page, pageSize, searchTerm, maTruong);
                return (new List<KhaoThi_2_THPT_NhomMonModel>(), 0);
            }
        }

        public async Task<bool> IsNhomMonExistAsync(string maTruong, string monLuaChon1, string monLuaChon2)
        {
            try
            {
                if (string.IsNullOrEmpty(maTruong) || string.IsNullOrEmpty(monLuaChon1) || string.IsNullOrEmpty(monLuaChon2))
                {
                    _logger.LogWarning("Thiếu thông tin khi kiểm tra nhóm môn tồn tại. MaTruong: {MaTruong}, MonLuaChon1: {MonLuaChon1}, MonLuaChon2: {MonLuaChon2}",
                        maTruong, monLuaChon1, monLuaChon2);
                    return false;
                }

                return await _TTDonViNhomMonRepository.IsNhomMonExistAsync(maTruong, monLuaChon1, monLuaChon2);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kiểm tra nhóm môn tồn tại. MaTruong: {MaTruong}, MonLuaChon1: {MonLuaChon1}, MonLuaChon2: {MonLuaChon2}",
                    maTruong, monLuaChon1, monLuaChon2);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(KhaoThi_2_THPT_NhomMonModel nhomMon)
        {
            try
            {
                if (nhomMon == null)
                {
                    _logger.LogWarning("Không thể cập nhật nhóm môn với dữ liệu null");
                    return false;
                }

                if (nhomMon.ID <= 0)
                {
                    _logger.LogWarning("ID không hợp lệ khi cập nhật nhóm môn: {Id}", nhomMon.ID);
                    return false;
                }

                // Lưu ý: Theo repository, chỉ cho phép cập nhật trường SoLuong
                return await _TTDonViNhomMonRepository.UpdateAsync(nhomMon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật nhóm môn. ID: {Id}, SoLuong: {SoLuong}",
                    nhomMon?.ID, nhomMon?.SoLuong);
                return false;
            }
        }
    }
}
