using khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT.Interfaces;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT
{
    public class BCThongTinConThiService : IBCThongTinConThiService
    {
        private readonly IBCThongTinConThiRepository _conThiRepository;
        private readonly ILogger<BCThongTinConThiService> _logger;

        /// <summary>
        /// Constructor với Dependency Injection
        /// </summary>
        public BCThongTinConThiService(
            IBCThongTinConThiRepository conThiRepository,
            ILogger<BCThongTinConThiService> logger)
        {
            _conThiRepository = conThiRepository;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(KhaoThi_5_THPT_ThongTin_ConThiModel conThi)
        {

            //try
            //{
            //    if (conThi == null)
            //    {
            //        _logger.LogError("Không thể tạo thông tin con thi với dữ liệu null");
            //        return -1;
            //    }

            //    if (string.IsNullOrEmpty(conThi.MaTruong) || string.IsNullOrEmpty(conThi.CCCD) ||
            //        string.IsNullOrEmpty(conThi.MaDinhDanhCuaCon))
            //    {
            //        _logger.LogError("Không thể tạo thông tin con thi khi thiếu mã trường, CCCD hoặc mã định danh con");
            //        return -1;
            //    }

            //    // Kiểm tra trùng lặp
            //    bool exists = await _conThiRepository.IsConThiExistAsync(
            //        conThi.MaTruong,
            //        conThi.CCCD,
            //        conThi.MaDinhDanhCuaCon);

            //    if (exists)
            //    {
            //        _logger.LogError("Thông tin con thi đã tồn tại: HoTen: {HoTen}, HoTenCon: {HoTenCon}, MaTruong: {MaTruong}",
            //            conThi.HoTen, conThi.HoTenCon, conThi.MaTruong);
            //        return -1;
            //    }

            //    return await _conThiRepository.CreateAsync(conThi);
            //}
            //catch (Exception ex)
            //{
            //    _logger.LogError(ex, "Lỗi khi tạo thông tin con thi: HoTen: {HoTen}, HoTenCon: {HoTenCon}",
            //        conThi?.HoTen, conThi?.HoTenCon);
            //    return -1;
            //}
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (conThi == null)
                {
                    _logger.LogError("Không thể tạo thông tin con thi với dữ liệu null");
                    return false;
                }

                if (string.IsNullOrEmpty(conThi.MaTruong) || string.IsNullOrEmpty(conThi.CCCD) || string.IsNullOrEmpty(conThi.MaDinhDanhCuaCon))
                {
                    _logger.LogError("Không thể tạo thông tin con thi khi thiếu mã trường, CCCD hoặc mã định danh con");
                    return false;
                }

                // Kiểm tra trùng lặp
                bool exists = await _conThiRepository.IsConThiExistAsync(
                   conThi.MaTruong,
                    conThi.CCCD,
                    conThi.MaDinhDanhCuaCon);
                if (exists)
                {
                    _logger.LogError("Thông tin con thi đã tồn tại: HoTen: {HoTen}, HoTenCon: {HoTenCon}, MaTruong: {MaTruong}",
                       conThi.HoTen, conThi.HoTenCon, conThi.MaTruong);
                    return false;
                }

                // Thực hiện tạo mới
                return await _conThiRepository.CreateAsync(conThi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo thông tin con thi: HoTen: {HoTen}, HoTenCon: {HoTenCon}",
                     conThi?.HoTen, conThi?.HoTenCon);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string maTruong, string cccd, string maDinhDanhCuaCon)
        {
            try
            {
                if (string.IsNullOrEmpty(maTruong) || string.IsNullOrEmpty(cccd) ||
                    string.IsNullOrEmpty(maDinhDanhCuaCon))
                {
                    _logger.LogWarning("Thiếu thông tin khi xóa con thi. MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}",
                        maTruong, cccd, maDinhDanhCuaCon);
                    return false;
                }

                return await _conThiRepository.DeleteAsync(maTruong, cccd, maDinhDanhCuaCon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa thông tin con thi. MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}",
                    maTruong, cccd, maDinhDanhCuaCon);
                return false;
            }
        }

        public async Task<KhaoThi_5_THPT_ThongTin_ConThiModel?> GetThongTinCaNhan(string maTruong, string cccd, string maDinhDanhCuaCon)
        {
            try
            {
                if (string.IsNullOrEmpty(maTruong) || string.IsNullOrEmpty(cccd) ||
                    string.IsNullOrEmpty(maDinhDanhCuaCon))
                {
                    _logger.LogWarning("Thiếu thông tin khi lấy chi tiết con thi. MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}",
                        maTruong, cccd, maDinhDanhCuaCon);
                    return null;
                }

                return await _conThiRepository.GetThongTinCaNhan(maTruong, cccd, maDinhDanhCuaCon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết con thi. MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}",
                    maTruong, cccd, maDinhDanhCuaCon);
                return null;
            }
        }

        public async Task<IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel>> GetByMaTruongAsync(string maTruong)
        {
            try
            {
                if (string.IsNullOrEmpty(maTruong))
                {
                    _logger.LogWarning("Không thể lấy danh sách con thi khi mã trường trống");
                    return new List<KhaoThi_5_THPT_ThongTin_ConThiModel>();
                }

                return await _conThiRepository.GetByMaTruongAsync(maTruong);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách con thi theo mã trường: {MaTruong}", maTruong);
                return new List<KhaoThi_5_THPT_ThongTin_ConThiModel>();
            }
        }

        public async Task<(IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel> Items, int TotalCount)> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null,
            string? kyThiThamDu = null)
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

                if (pageSize > 100)
                {
                    pageSize = 100; // Giới hạn kích thước trang tối đa
                }

                return await _conThiRepository.GetPaginatedAsync(page, pageSize, searchTerm, maTruong, kyThiThamDu);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách con thi có phân trang. Page: {Page}, PageSize: {PageSize}, SearchTerm: {SearchTerm}, MaTruong: {MaTruong}, KyThiThamDu: {KyThiThamDu}",
                    page, pageSize, searchTerm, maTruong, kyThiThamDu);
                return (new List<KhaoThi_5_THPT_ThongTin_ConThiModel>(), 0);
            }
        }

        public async Task<bool> IsConThiExistAsync(string maTruong, string cccd, string maDinhDanhCuaCon)
        {
            try
            {
                if (string.IsNullOrEmpty(maTruong) || string.IsNullOrEmpty(cccd) ||
                    string.IsNullOrEmpty(maDinhDanhCuaCon))
                {
                    _logger.LogWarning("Thiếu thông tin khi kiểm tra con thi tồn tại. MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}",
                        maTruong, cccd, maDinhDanhCuaCon);
                    return false;
                }

                return await _conThiRepository.IsConThiExistAsync(maTruong, cccd, maDinhDanhCuaCon);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kiểm tra con thi tồn tại. MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}",
                    maTruong, cccd, maDinhDanhCuaCon);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(KhaoThi_5_THPT_ThongTin_ConThiModel conThi)
        {
            try
            {
                if (conThi == null)
                {
                    _logger.LogWarning("Không thể cập nhật thông tin con thi với dữ liệu null");
                    return false;
                }

                if (string.IsNullOrEmpty(conThi.MaTruong) || string.IsNullOrEmpty(conThi.CCCD) ||
                    string.IsNullOrEmpty(conThi.MaDinhDanhCuaCon))
                {
                    _logger.LogWarning("Không thể cập nhật thông tin con thi khi thiếu mã trường, CCCD hoặc mã định danh con");
                    return false;
                }

                return await _conThiRepository.UpdateAsync(conThi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thông tin con thi. MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}, HoTen: {HoTen}, HoTenCon: {HoTenCon}",
                    conThi?.MaTruong, conThi?.CCCD, conThi?.MaDinhDanhCuaCon, conThi?.HoTen, conThi?.HoTenCon);
                return false;
            }
        }
    }
}