using khaothi_2024_net_server.Features.BaoCaoSoLieuLanhDaoTHPT.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuLanhDaoTHPT.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuLanhDaoTHPT
{
    public class BCLanhDaoDonViService : IBCLanhDaoDonViService
    {
        private readonly IBCLanhDaoDonViRepository _lanhDaoRepository;
        private readonly ILogger<BCLanhDaoDonViService> _logger;
        private readonly IMemoryCache _cache;
        public BCLanhDaoDonViService(IBCLanhDaoDonViRepository lanhDaoRepository, ILogger<BCLanhDaoDonViService> logger)
        {
            _lanhDaoRepository = lanhDaoRepository;
            _logger = logger;
        }

        public async Task<KhaoThi_4_THPT_ThongTin_LanhDaoModel?> GetByMaTruongAndCCCDAsync(string maTruong, string cccd)
        {
            try
            {
                return await _lanhDaoRepository.GetByMaTruongAndCCCDAsync(maTruong, cccd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin lãnh đạo theo mã trường và CCCD: {MaTruong}, {CCCD}", maTruong, cccd);
                return null;
            }
        }

        public async Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetByMaTruongAsync(string maTruong)
        {
            try
            {
                return await _lanhDaoRepository.GetByMaTruongAsync(maTruong);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin lãnh đạo theo mã trường: {MaTruong}", maTruong);
                return new List<KhaoThi_4_THPT_ThongTin_LanhDaoModel>();
            }
        }

        public async Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetAllAsync()
        {
            try
            {
                string cacheKey = "AllLanhDao";
                if (!_cache.TryGetValue(cacheKey, out IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel> result))
                {
                    result = await _lanhDaoRepository.GetAllAsync();
                    _cache.Set(cacheKey, result, TimeSpan.FromMinutes(10));
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tất cả thông tin lãnh đạo");
                return new List<KhaoThi_4_THPT_ThongTin_LanhDaoModel>();
            }
        }

        public async Task<bool> CreateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao)
        {
            try
            {
                // Validation trước khi tạo
                if (string.IsNullOrEmpty(lanhDao.MaTruong) || string.IsNullOrEmpty(lanhDao.CCCD))
                {
                    _logger.LogError("Mã trường và CCCD không được để trống khi tạo thông tin lãnh đạo.");
                    return false;
                }

                if (await _lanhDaoRepository.ExistsAsync(lanhDao.MaTruong, lanhDao.CCCD))
                {
                    _logger.LogError("Thông tin lãnh đạo với Mã trường {MaTruong} và CCCD {CCCD} đã tồn tại.", lanhDao.MaTruong, lanhDao.CCCD);
                    return false; // Hoặc throw exception nếu muốn
                }

                return await _lanhDaoRepository.CreateAsync(lanhDao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo thông tin lãnh đạo: {MaTruong}, {CCCD}", lanhDao.MaTruong, lanhDao.CCCD);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao)
        {
            try
            {
                // Validation trước khi cập nhật
                if (string.IsNullOrEmpty(lanhDao.MaTruong) || string.IsNullOrEmpty(lanhDao.CCCD))
                {
                    _logger.LogError("Mã trường và CCCD không được để trống khi cập nhật thông tin lãnh đạo.");
                    return false;
                }

                if (!await _lanhDaoRepository.ExistsAsync(lanhDao.MaTruong, lanhDao.CCCD))
                {
                    _logger.LogError("Không tìm thấy thông tin lãnh đạo với Mã trường {MaTruong} và CCCD {CCCD}.", lanhDao.MaTruong, lanhDao.CCCD);
                    return false;
                }

                return await _lanhDaoRepository.UpdateAsync(lanhDao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thông tin lãnh đạo: {MaTruong}, {CCCD}", lanhDao.MaTruong, lanhDao.CCCD);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string maTruong, string cccd)
        {
            try
            {
                return await _lanhDaoRepository.DeleteAsync(maTruong, cccd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa thông tin lãnh đạo: {MaTruong}, {CCCD}", maTruong, cccd);
                return false;
            }
        }

        public async Task<bool> ExistsAsync(string maTruong, string cccd)
        {
            try
            {
                return await _lanhDaoRepository.ExistsAsync(maTruong, cccd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kiểm tra sự tồn tại của thông tin lãnh đạo: {MaTruong}, {CCCD}", maTruong, cccd);
                return false;
            }
        }

        public async Task<(IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel> Items, int TotalCount)> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null,
            int? namSinh = null,
            string? cccd = null
        )
        {
            try
            {
                return await _lanhDaoRepository.GetPaginatedAsync(page, pageSize, searchTerm, maTruong, namSinh, cccd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách lãnh đạo phân trang và tìm kiếm");
                return (new List<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(), 0);
            }
        }
    }

}
