using khaothi_2024_net_server.Features.BaoCaoSoLieuTruongDiemTHPT.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuTruongDiemTHPT.Interfaces;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuTruongDiemTHPT
{
    /// <summary>
    /// Service xử lý nghiệp vụ thông tin trường điểm thi
    /// </summary>
    public class BCThongTinTruongDiemService : IBCThongTinTruongDiemService
    {
        private readonly IBCThongTinTruongDiemRepository _truongDiemRepository;
        private readonly ILogger<BCThongTinTruongDiemService> _logger;

        /// <summary>
        /// Constructor với Dependency Injection
        /// </summary>
        public BCThongTinTruongDiemService(
            IBCThongTinTruongDiemRepository truongDiemRepository,
            ILogger<BCThongTinTruongDiemService> logger)
        {
            _truongDiemRepository = truongDiemRepository;
            _logger = logger;
        }

        /// <summary>
        /// Cập nhật thông tin trường điểm thi
        /// </summary>
        public async Task<bool> UpdateAsync(KhaoThi_5_THPT_ThongTin_TruongDiemModel lanhDaoDiemThi)
        {
            try
            {
                // Kiểm tra dữ liệu đầu vào
                if (lanhDaoDiemThi == null)
                {
                    _logger.LogWarning("Không thể cập nhật thông tin trường điểm thi với dữ liệu null");
                    return false;
                }

                if (string.IsNullOrEmpty(lanhDaoDiemThi.MaTruong) || string.IsNullOrEmpty(lanhDaoDiemThi.CCCD))
                {
                    _logger.LogWarning("Không thể cập nhật thông tin trường điểm thi khi thiếu mã trường hoặc CCCD");
                    return false;
                }

                // Thực hiện cập nhật
                return await _truongDiemRepository.UpdateAsync(lanhDaoDiemThi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thông tin trường điểm thi. MaTruong: {MaTruong}, CCCD: {CCCD}, HoTen: {HoTen}",
                    lanhDaoDiemThi?.MaTruong, lanhDaoDiemThi?.CCCD, lanhDaoDiemThi?.HoTen);
                return false;
            }
        }

        /// <summary>
        /// Lấy danh sách trường điểm thi có phân trang và tìm kiếm
        /// </summary>
        public async Task<(IEnumerable<KhaoThi_5_THPT_ThongTin_TruongDiemModel> Items, int TotalCount)> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null)
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
                return await _truongDiemRepository.GetPaginatedAsync(page, pageSize, searchTerm, maTruong);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách trường điểm thi có phân trang. Page: {Page}, PageSize: {PageSize}, SearchTerm: {SearchTerm}, MaTruong: {MaTruong}",
                    page, pageSize, searchTerm, maTruong);
                return (new List<KhaoThi_5_THPT_ThongTin_TruongDiemModel>(), 0);
            }
        }
    }
}