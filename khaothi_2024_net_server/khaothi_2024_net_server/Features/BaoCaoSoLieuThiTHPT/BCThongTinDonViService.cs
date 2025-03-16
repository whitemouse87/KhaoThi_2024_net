using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT.Interfaces;
using khaothi_2024_net_server.Features.UserManagement.DTOs;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT
{
    public class BCThongTinDonViService : IBCThongTinDonViService
    {
        private readonly IBCThongTinDonViRepository _TTDonViRepository;
        private readonly ILogger<BCThongTinDonViService> _logger;
        /// <summary>
        /// Constructor với Dependency Injection
        /// </summary>
        public BCThongTinDonViService(
            IBCThongTinDonViRepository TTDonViRepository,
            ILogger<BCThongTinDonViService> logger)
        {
            _TTDonViRepository = TTDonViRepository;
            _logger = logger;

        }
        public async Task<KhaoThi_1_THPT_ThongTin_DonViModel?> GetByMaTruongAsync(string MaTruong)
        {
            if (MaTruong == null)
            {
                _logger.LogError($"[Warning] Mã trường không hợp lệ: {MaTruong}");
                throw new ArgumentException("Nhập lại mã trường của Sở (6 ký tự)", nameof(MaTruong));
            }

            try
            {
                _logger.LogInformation($"🔍 Đang tìm đơn vị với mã trường: {MaTruong}");

                var DonVi = await _TTDonViRepository.GetByMaTruongAsync(MaTruong);

                if (DonVi is null)
                {
                    _logger.LogError($"❌ Không tìm thấy đơn vị với mã trường {MaTruong}");
                    return null; // Trả về null thay vì throw
                }

                _logger.LogInformation($"✅ Tìm thấy đơn vị với mã trường {MaTruong}: {DonVi.TenTruong.ToUpper().Trim()}");
                return DonVi;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"🔥 Lỗi khi truy vấn đơn vị với mã trường {MaTruong}");
                throw;
            }
        }
        public async Task<(IEnumerable<KhaoThi_1_THPT_ThongTin_DonViModel> Items, int TotalCount)> GetPaginatedAsync(
          int page,
          int pageSize,
          string? searchTerm = null)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100; // Giới hạn kích thước trang tối đa

                return await _TTDonViRepository.GetPaginatedAsync(page, pageSize, searchTerm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách đơn vị phân trang");
                throw;
            }
        }
        private void ValidateTruongData(KhaoThi_1_THPT_ThongTin_DonViModel DonVi)
        {
            if (DonVi == null)
            {
                throw new ArgumentNullException(nameof(DonVi));
            }

            if (string.IsNullOrWhiteSpace(DonVi.TenTruong))
            {
                throw new ArgumentException("Tên trường không được để trống");
            }

            if (string.IsNullOrWhiteSpace(DonVi.MaTruong))
            {
                throw new ArgumentException("Mã trường không được để trống");
            }


            // Có thể thêm các validation khác tùy theo yêu cầu nghiệp vụ
        }
        public async Task<bool> UpdateAsync(KhaoThi_1_THPT_ThongTin_DonViModel DonVi)
        {
            try
            {
                // Validate dữ liệu đầu vào
                ValidateTruongData(DonVi);

                var existingDonVi = await GetByMaTruongAsync(DonVi.MaTruong);
                if (existingDonVi == null)
                {
                    return false;
                }


                return await _TTDonViRepository.UpdateAsync(DonVi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật đơn vị trường: {DonVi.TenTruong}");
                throw;
            }
        }
    }
}
