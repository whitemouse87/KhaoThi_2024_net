using khaothi_2024_net_server.Core.Models.Common;
using khaothi_2024_net_server.Features.BaoCaoSoLieuTruongDiemTHPT.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuTruongDiemTHPT.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuTruongDiemTHPT
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // Yêu cầu Bearer token cho tất cả endpoints
    public class BCThongTinTruongDiemController : ControllerBase
    {
        private readonly IBCThongTinTruongDiemService _truongDiemService;
        private readonly ILogger<BCThongTinTruongDiemController> _logger;

        public BCThongTinTruongDiemController(
            IBCThongTinTruongDiemService truongDiemService,
            ILogger<BCThongTinTruongDiemController> logger)
        {
            _truongDiemService = truongDiemService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách thông tin trường điểm thi có phân trang và tìm kiếm
        /// </summary>
        [HttpGet("all-phantrang-truongdiem")]
        [ProducesResponseType(typeof(PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? maTruong = null)
        {
            try
            {
                // Validate input parameters
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100; // Giới hạn kích thước trang tối đa

                // Gọi service để lấy dữ liệu
                var (items, totalCount) = await _truongDiemService.GetPaginatedAsync(
                    page, pageSize, searchTerm, maTruong);

                // Tạo kết quả phân trang
                var result = new PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(
                    items: items,
                    count: totalCount,
                    page: page,
                    pageSize: pageSize
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách thông tin trường điểm thi phân trang. SearchTerm: {SearchTerm}, Page: {Page}, PageSize: {PageSize}, MaTruong: {MaTruong}",
                    searchTerm, page, pageSize, maTruong);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Cập nhật thông tin trường điểm thi
        /// </summary>
        [HttpPut("truongdiemtruong/{maTruong}/cccd/{cccd}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(string maTruong, string cccd, [FromBody] KhaoThi_4_THPT_ThongTin_LanhDaoModel truongDiem)
        {
            try
            {
                if (maTruong != truongDiem.MaTruong || cccd != truongDiem.CCCD)
                {
                    return BadRequest(new { message = "Mã trường hoặc CCCD không khớp" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _truongDiemService.UpdateAsync(truongDiem);
                if (!result)
                {
                    return NotFound(new { message = $"Không tìm thấy thông tin trường điểm thi với mã trường: {maTruong} và CCCD: {cccd}" });
                }

                return Ok(new { message = "Cập nhật thông tin trường điểm thi thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thông tin trường điểm thi. MaTruong: {MaTruong}, CCCD: {CCCD}", maTruong, cccd);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }
    }
}