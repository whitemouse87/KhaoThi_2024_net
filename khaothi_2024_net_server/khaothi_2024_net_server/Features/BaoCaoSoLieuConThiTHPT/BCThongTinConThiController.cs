using khaothi_2024_net_server.Core.Models.Common;
using khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // Yêu cầu Bearer token cho tất cả endpoints
    public class BCThongTinConThiController : ControllerBase
    {
        private readonly IBCThongTinConThiService _conThiService;
        private readonly ILogger<BCThongTinConThiController> _logger;

        public BCThongTinConThiController(
            IBCThongTinConThiService conThiService,
            ILogger<BCThongTinConThiController> logger)
        {
            _conThiService = conThiService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách thông tin con thi có phân trang và tìm kiếm
        /// </summary>
        [HttpGet("all-phantrang")]
        [ProducesResponseType(typeof(PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThiModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? maTruong = null,
            [FromQuery] string? kyThiThamDu = null)
        {
            try
            {
                // Validate input parameters
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100; // Giới hạn kích thước trang tối đa

                // Gọi service để lấy dữ liệu
                var (items, totalCount) = await _conThiService.GetPaginatedAsync(
                    page, pageSize, searchTerm, maTruong, kyThiThamDu);

                // Tạo kết quả phân trang
                var result = new PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThiModel>(
                    items: items,
                    count: totalCount,
                    page: page,
                    pageSize: pageSize
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách con thi phân trang. SearchTerm: {SearchTerm}, Page: {Page}, PageSize: {PageSize}, MaTruong: {MaTruong}, KyThiThamDu: {KyThiThamDu}",
                    searchTerm, page, pageSize, maTruong, kyThiThamDu);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Lấy danh sách con thi theo mã trường
        /// </summary>
        [HttpGet("truong/{maTruong}")]
        [ProducesResponseType(typeof(IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByMaDonVi(string maTruong)
        {
            try
            {
                var conThiList = await _conThiService.GetByMaTruongAsync(maTruong);
                if (conThiList == null || !conThiList.Any())
                {
                    return NotFound(new { message = $"Không tìm thấy thông tin con thi nào cho trường có mã: {maTruong}" });
                }

                return Ok(conThiList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách con thi theo mã trường: {MaTruong}", maTruong);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Lấy thông tin chi tiết của một con thi
        /// </summary>
        [HttpGet("detailconthi")]
        [ProducesResponseType(typeof(KhaoThi_5_THPT_ThongTin_ConThiModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetThongTinCaNhan(
            [FromQuery] string maTruong,
            [FromQuery] string cccd,
            [FromQuery] string maDinhDanhCuaCon)
        {
            try
            {
                if (string.IsNullOrEmpty(maTruong) || string.IsNullOrEmpty(cccd) || string.IsNullOrEmpty(maDinhDanhCuaCon))
                {
                    return BadRequest(new { message = "Cần cung cấp đầy đủ mã trường, CCCD và mã định danh của con" });
                }

                var conThi = await _conThiService.GetThongTinCaNhan(maTruong, cccd, maDinhDanhCuaCon);
                if (conThi == null)
                {
                    return NotFound(new { message = $"Không tìm thấy thông tin con thi với mã trường: {maTruong}, CCCD: {cccd}, mã định danh: {maDinhDanhCuaCon}" });
                }

                return Ok(conThi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin chi tiết con thi. MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}",
                    maTruong, cccd, maDinhDanhCuaCon);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Tạo mới thông tin con thi
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(KhaoThi_5_THPT_ThongTin_ConThiModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create([FromBody] KhaoThi_5_THPT_ThongTin_ConThiModel conThi)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Kiểm tra thông tin đầu vào cần thiết
                if (string.IsNullOrEmpty(conThi.MaTruong) || string.IsNullOrEmpty(conThi.CCCD) ||
                    string.IsNullOrEmpty(conThi.MaDinhDanhCuaCon))
                {
                    return BadRequest(new { message = "Cần cung cấp đầy đủ mã trường, CCCD và mã định danh của con" });
                }

                bool result = await _conThiService.CreateAsync(conThi);
                if (!result)
                {
                    return BadRequest(new { message = "Không thể tạo thông tin con thi. Thông tin này có thể đã tồn tại." });
                }

                // Trả về đường dẫn đến resource mới
                return CreatedAtAction(nameof(GetThongTinCaNhan), new
                {
                    maTruong = conThi.MaTruong,
                    cccd = conThi.CCCD,
                    maDinhDanhCuaCon = conThi.MaDinhDanhCuaCon
                }, conThi);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo mới thông tin con thi");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Cập nhật thông tin con thi
        /// </summary>
        [HttpPut]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update([FromBody] KhaoThi_5_THPT_ThongTin_ConThiModel conThi)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Kiểm tra thông tin đầu vào cần thiết
                if (string.IsNullOrEmpty(conThi.MaTruong) || string.IsNullOrEmpty(conThi.CCCD) ||
                    string.IsNullOrEmpty(conThi.MaDinhDanhCuaCon))
                {
                    return BadRequest(new { message = "Cần cung cấp đầy đủ mã trường, CCCD và mã định danh của con" });
                }

                // Kiểm tra xem record đã tồn tại chưa
                bool exists = await _conThiService.IsConThiExistAsync(conThi.MaTruong, conThi.CCCD, conThi.MaDinhDanhCuaCon);
                if (!exists)
                {
                    return NotFound(new { message = $"Không tìm thấy thông tin con thi với mã trường: {conThi.MaTruong}, CCCD: {conThi.CCCD}, mã định danh: {conThi.MaDinhDanhCuaCon}" });
                }

                var result = await _conThiService.UpdateAsync(conThi);
                if (!result)
                {
                    return StatusCode(500, new { message = "Cập nhật thông tin con thi không thành công" });
                }

                return Ok(new { message = "Cập nhật thông tin con thi thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thông tin con thi. MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}",
                    conThi?.MaTruong, conThi?.CCCD, conThi?.MaDinhDanhCuaCon);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Xóa thông tin con thi
        /// </summary>
        [HttpDelete]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(
            [FromQuery] string maTruong,
            [FromQuery] string cccd,
            [FromQuery] string maDinhDanhCuaCon)
        {
            try
            {
                if (string.IsNullOrEmpty(maTruong) || string.IsNullOrEmpty(cccd) || string.IsNullOrEmpty(maDinhDanhCuaCon))
                {
                    return BadRequest(new { message = "Cần cung cấp đầy đủ mã trường, CCCD và mã định danh của con" });
                }

                var result = await _conThiService.DeleteAsync(maTruong, cccd, maDinhDanhCuaCon);
                if (!result)
                {
                    return NotFound(new { message = $"Không tìm thấy thông tin con thi với mã trường: {maTruong}, CCCD: {cccd}, mã định danh: {maDinhDanhCuaCon}" });
                }

                return Ok(new { message = "Xóa thông tin con thi thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa thông tin con thi. MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}",
                    maTruong, cccd, maDinhDanhCuaCon);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Kiểm tra thông tin con thi đã tồn tại chưa
        /// </summary>
        [HttpGet("check-exist-conthi")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CheckExist(
            [FromQuery] string maTruong,
            [FromQuery] string cccd,
            [FromQuery] string maDinhDanhCuaCon)
        {
            try
            {
                if (string.IsNullOrEmpty(maTruong) || string.IsNullOrEmpty(cccd) || string.IsNullOrEmpty(maDinhDanhCuaCon))
                {
                    return BadRequest(new { message = "Cần cung cấp đầy đủ mã trường, CCCD và mã định danh của con" });
                }

                bool exists = await _conThiService.IsConThiExistAsync(maTruong, cccd, maDinhDanhCuaCon);
                return Ok(new { exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kiểm tra thông tin con thi tồn tại. MaTruong: {MaTruong}, CCCD: {CCCD}, MaDinhDanhCuaCon: {MaDinhDanhCuaCon}",
                    maTruong, cccd, maDinhDanhCuaCon);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }
    }
}