using khaothi_2024_net_server.Features.BaoCaoSoLieuLanhDaoTHPT.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuLanhDaoTHPT.Interfaces;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuLanhDaoTHPT
{
    /// <summary>
    /// Service xử lý nghiệp vụ thông tin lãnh đạo đơn vị
    /// </summary>
    public class BCLanhDaoDonViService : IBCLanhDaoDonViService
    {
        private readonly IBCLanhDaoDonViRepository _lanhDaoRepository;
        private readonly ILogger<BCLanhDaoDonViService> _logger;

        /// <summary>
        /// Constructor với Dependency Injection
        /// </summary>
        public BCLanhDaoDonViService(
            IBCLanhDaoDonViRepository lanhDaoRepository,
            ILogger<BCLanhDaoDonViService> logger)
        {
            _lanhDaoRepository = lanhDaoRepository;
            _logger = logger;
        }

        /// <summary>
        /// Lấy thông tin lãnh đạo theo mã trường và CCCD
        /// </summary>
        public async Task<KhaoThi_4_THPT_ThongTin_LanhDaoModel?> GetByMaTruongAndCCCDAsync(string maTruong, string cccd)
        {
            try
            {
                // Kiểm tra đầu vào
                if (string.IsNullOrEmpty(maTruong) || string.IsNullOrEmpty(cccd))
                {
                    _logger.LogWarning("Không thể lấy thông tin lãnh đạo khi thiếu mã trường hoặc CCCD");
                    return null;
                }

                // Gọi repository để lấy dữ liệu
                return await _lanhDaoRepository.GetByMaTruongAndCCCDAsync(maTruong, cccd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin lãnh đạo. MaTruong: {MaTruong}, CCCD: {CCCD}", maTruong, cccd);
                return null;
            }
        }

        /// <summary>
        /// Lấy danh sách lãnh đạo theo mã trường
        /// </summary>
        public async Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetByMaTruongAsync(string maTruong)
        {
            try
            {
                if (string.IsNullOrEmpty(maTruong))
                {
                    _logger.LogWarning("Không thể lấy danh sách lãnh đạo khi mã trường trống");
                    return new List<KhaoThi_4_THPT_ThongTin_LanhDaoModel>();
                }

                return await _lanhDaoRepository.GetByMaTruongAsync(maTruong);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách lãnh đạo theo mã trường: {MaTruong}", maTruong);
                return new List<KhaoThi_4_THPT_ThongTin_LanhDaoModel>();
            }
        }

        /// <summary>
        /// Lấy toàn bộ danh sách lãnh đạo
        /// </summary>
        public async Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetAllAsync()
        {
            try
            {
                return await _lanhDaoRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tất cả danh sách lãnh đạo");
                return new List<KhaoThi_4_THPT_ThongTin_LanhDaoModel>();
            }
        }

        /// <summary>
        /// Tạo mới thông tin lãnh đạo
        /// </summary>
        public async Task<bool> CreateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (lanhDao == null)
                {
                    _logger.LogError("Không thể tạo thông tin lãnh đạo với dữ liệu null");
                    return false;
                }

                if (string.IsNullOrEmpty(lanhDao.MaTruong) || string.IsNullOrEmpty(lanhDao.CCCD))
                {
                    _logger.LogError("Không thể tạo thông tin lãnh đạo khi thiếu mã trường hoặc CCCD");
                    return false;
                }

                // Kiểm tra trùng lặp
                bool exists = await _lanhDaoRepository.ExistsAsync(lanhDao.MaTruong, lanhDao.CCCD);
                if (exists)
                {
                    _logger.LogError("Thông tin lãnh đạo đã tồn tại. MaTruong: {MaTruong}, CCCD: {CCCD}",
                        lanhDao.MaTruong, lanhDao.CCCD);
                    return false;
                }

                // Thực hiện tạo mới
                return await _lanhDaoRepository.CreateAsync(lanhDao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo thông tin lãnh đạo. MaTruong: {MaTruong}, CCCD: {CCCD}, HoTen: {HoTen}",
                    lanhDao?.MaTruong, lanhDao?.CCCD, lanhDao?.HoTen);
                return false;
            }
        }

        /// <summary>
        /// Cập nhật thông tin lãnh đạo
        /// </summary>
        public async Task<bool> UpdateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (lanhDao == null)
                {
                    _logger.LogWarning("Không thể cập nhật thông tin lãnh đạo với dữ liệu null");
                    return false;
                }

                if (string.IsNullOrEmpty(lanhDao.MaTruong) || string.IsNullOrEmpty(lanhDao.CCCD))
                {
                    _logger.LogWarning("Không thể cập nhật thông tin lãnh đạo khi thiếu mã trường hoặc CCCD");
                    return false;
                }

                // Kiểm tra sự tồn tại của bản ghi
                bool exists = await _lanhDaoRepository.ExistsAsync(lanhDao.MaTruong, lanhDao.CCCD);
                if (!exists)
                {
                    _logger.LogWarning("Không thể cập nhật thông tin lãnh đạo không tồn tại. MaTruong: {MaTruong}, CCCD: {CCCD}",
                        lanhDao.MaTruong, lanhDao.CCCD);
                    return false;
                }

                // Thực hiện cập nhật
                return await _lanhDaoRepository.UpdateAsync(lanhDao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thông tin lãnh đạo. MaTruong: {MaTruong}, CCCD: {CCCD}, HoTen: {HoTen}",
                    lanhDao?.MaTruong, lanhDao?.CCCD, lanhDao?.HoTen);
                return false;
            }
        }

        /// <summary>
        /// Xóa thông tin lãnh đạo
        /// </summary>
        public async Task<bool> DeleteAsync(string maTruong, string cccd)
        {
            try
            {
                // Kiểm tra đầu vào
                if (string.IsNullOrEmpty(maTruong) || string.IsNullOrEmpty(cccd))
                {
                    _logger.LogWarning("Không thể xóa thông tin lãnh đạo khi thiếu mã trường hoặc CCCD");
                    return false;
                }

                // Kiểm tra sự tồn tại
                bool exists = await _lanhDaoRepository.ExistsAsync(maTruong, cccd);
                if (!exists)
                {
                    _logger.LogWarning("Không thể xóa thông tin lãnh đạo không tồn tại. MaTruong: {MaTruong}, CCCD: {CCCD}",
                        maTruong, cccd);
                    return false;
                }

                // Thực hiện xóa
                return await _lanhDaoRepository.DeleteAsync(maTruong, cccd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa thông tin lãnh đạo. MaTruong: {MaTruong}, CCCD: {CCCD}",
                    maTruong, cccd);
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra sự tồn tại của thông tin lãnh đạo
        /// </summary>
        public async Task<bool> ExistsAsync(string maTruong, string cccd)
        {
            try
            {
                if (string.IsNullOrEmpty(maTruong) || string.IsNullOrEmpty(cccd))
                {
                    _logger.LogWarning("Thiếu thông tin khi kiểm tra sự tồn tại của lãnh đạo. MaTruong: {MaTruong}, CCCD: {CCCD}",
                        maTruong, cccd);
                    return false;
                }

                return await _lanhDaoRepository.ExistsAsync(maTruong, cccd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kiểm tra sự tồn tại của lãnh đạo. MaTruong: {MaTruong}, CCCD: {CCCD}",
                    maTruong, cccd);
                return false;
            }
        }

        /// <summary>
        /// Lấy danh sách lãnh đạo có phân trang và tìm kiếm
        /// </summary>
        public async Task<(IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel> Items, int TotalCount)> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null,
            int? namSinh = null,
            string? cccd = null)
        {
            try
            {
                // Xác thực các tham số đầu vào
                if (page < 1)
                {
                    page = 1;
                }

                if (pageSize < 1)
                {
                    pageSize = 10;
                }

                if (pageSize > 100)
                {
                    pageSize = 100; // Giới hạn kích thước trang tối đa
                }

                // Thực hiện truy vấn với phân trang
                return await _lanhDaoRepository.GetPaginatedAsync(page, pageSize, searchTerm, maTruong, namSinh, cccd);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách lãnh đạo có phân trang. Page: {Page}, PageSize: {PageSize}, SearchTerm: {SearchTerm}, MaTruong: {MaTruong}",
                    page, pageSize, searchTerm, maTruong);
                return (new List<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(), 0);
            }
        }
    }
}