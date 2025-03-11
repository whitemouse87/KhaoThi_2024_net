using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Core.Models.Common;
using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT.Interfaces;
using khaothi_2024_net_server.Features.UserManagement;
using khaothi_2024_net_server.Features.UserManagement.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // Thêm attribute này để yêu cầu Bearer token cho tất cả endpoints
    public class BCThongTinDonViController : ControllerBase
    {
        private readonly IBCThongTinDonViService _TTDonViService;
        private readonly ILogger<BCThongTinDonViController> _logger;
        public BCThongTinDonViController(
           IBCThongTinDonViService TTDonViService,
           ILogger<BCThongTinDonViController> logger)
        {
            _TTDonViService = TTDonViService;
            _logger = logger;
        }
        /// <summary>
        /// Lấy thông tin đơn vị theo mã trường
        /// </summary>
        [HttpGet("{MaTruong}")]
        [ProducesResponseType(typeof(KhaoThi_1_THPT_ThongTin_DonViModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetByMaTruong(string MaTruong)
        {
            try
            {
                var DonVi = await _TTDonViService.GetByMaTruongAsync(MaTruong);
                if (DonVi == null)
                {
                    return NotFound(new { message = $"Không tìm thấy đơn vị với MaTruong: {MaTruong}" });
                }

                return Ok(DonVi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy thông tin đơn vị với mã trường: {MaTruong}");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }
        [HttpGet("all-phantrang")]
        [Authorize(Roles = "01")]
        [ProducesResponseType(typeof(PaginatedResult<KhaoThi_1_THPT_ThongTin_DonViModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null)
        {
            try
            {
                // Validate input parameters
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100; // Giới hạn kích thước trang tối đa

                // Gọi service để lấy dữ liệu
                var (items, totalCount) = await _TTDonViService.GetPaginatedAsync(page, pageSize, searchTerm);

                // Tạo kết quả phân trang sử dụng constructor của PaginatedResult
                var result = new PaginatedResult<KhaoThi_1_THPT_ThongTin_DonViModel>(
                    items: items,
                    count: totalCount,
                    page: page,
                    pageSize: pageSize
                );

                // Trả về kết quả
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy danh sách đơn vị phân trang. SearchTerm: {searchTerm}, Page: {page}, PageSize: {pageSize}",
                    searchTerm, page, pageSize);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }
        /// <summary>
        /// Cập nhật thông tin người dùng
        /// </summary>
        [HttpPut("{MaTruong}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update(string MaTruong, [FromBody] KhaoThi_1_THPT_ThongTin_DonViModel DonVi)
        {
            try
            {
                if (MaTruong.ToUpper().Trim() != DonVi.MaTruong.Trim().ToUpper())
                {
                    return BadRequest(new { message = "MaTruong không khớp" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _TTDonViService.UpdateAsync(DonVi);
                if (!result)
                {
                    return NotFound(new { message = $"Không tìm thấy đơn vị với mã trường: {MaTruong}" });
                }

                return Ok(new { message = "Cập nhật thành công" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật mã trường: {MaTruong}");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }
    }

}
