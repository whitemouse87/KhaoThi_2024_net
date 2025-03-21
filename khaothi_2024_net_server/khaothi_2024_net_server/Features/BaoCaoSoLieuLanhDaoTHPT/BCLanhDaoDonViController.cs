using khaothi_2024_net_server.Core.Models.Common;
using khaothi_2024_net_server.Features.BaoCaoSoLieuLanhDaoTHPT.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuLanhDaoTHPT.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuLanhDaoTHPT
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // Yêu cầu Bearer token cho tất cả endpoints
    public class BCLanhDaoDonViController : ControllerBase
    {
        private readonly IBCLanhDaoDonViService _lanhDaoService;
        private readonly ILogger<BCLanhDaoDonViController> _logger;

        public BCLanhDaoDonViController(
            IBCLanhDaoDonViService lanhDaoService,
            ILogger<BCLanhDaoDonViController> logger)
        {
            _lanhDaoService = lanhDaoService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách lãnh đạo có phân trang và tìm kiếm
        /// </summary>
        [HttpGet("all-phantrang-lanhdao")]
        [ProducesResponseType(typeof(PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? maTruong = null,
            [FromQuery] int? namSinh = null,
            [FromQuery] string? cccd = null)
        {
            try
            {
                // Xác thực tham số đầu vào
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100; // Giới hạn kích thước trang tối đa

                // Gọi service để lấy dữ liệu
                var (items, totalCount) = await _lanhDaoService.GetPaginatedAsync(
                    page, pageSize, searchTerm, maTruong, namSinh, cccd);

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
                _logger.LogError(ex, "Lỗi khi lấy danh sách lãnh đạo phân trang. SearchTerm: {SearchTerm}, Page: {Page}, PageSize: {PageSize}",
                    searchTerm, page, pageSize);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Lấy tất cả lãnh đạo
        /// </summary>
        [HttpGet("all-lanhdao")]
        [ProducesResponseType(typeof(IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var lanhDaos = await _lanhDaoService.GetAllAsync();
                return Ok(lanhDaos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tất cả danh sách lãnh đạo");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Lấy danh sách lãnh đạo theo mã trường
        /// </summary>
        [HttpGet("ldtruong/{maTruong}")]
        [ProducesResponseType(typeof(IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByMaTruong(string maTruong)
        {
            try
            {
                var lanhDaos = await _lanhDaoService.GetByMaTruongAsync(maTruong);
                if (lanhDaos == null || !lanhDaos.Any())
                {
                    return NotFound(new { message = $"Không tìm thấy lãnh đạo nào cho trường có mã: {maTruong}" });
                }

                return Ok(lanhDaos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách lãnh đạo theo mã trường: {MaTruong}", maTruong);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Lấy thông tin lãnh đạo theo mã trường và CCCD
        /// </summary>
        [HttpGet("ldtruong/{maTruong}/cccd/{cccd}")]
        [ProducesResponseType(typeof(KhaoThi_4_THPT_ThongTin_LanhDaoModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByMaTruongAndCCCD(string maTruong, string cccd)
        {
            try
            {
                var lanhDao = await _lanhDaoService.GetByMaTruongAndCCCDAsync(maTruong, cccd);
                if (lanhDao == null)
                {
                    return NotFound(new { message = $"Không tìm thấy lãnh đạo với mã trường: {maTruong} và CCCD: {cccd}" });
                }

                return Ok(lanhDao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin lãnh đạo. MaTruong: {MaTruong}, CCCD: {CCCD}", maTruong, cccd);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Tạo mới thông tin lãnh đạo
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(KhaoThi_4_THPT_ThongTin_LanhDaoModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create([FromBody] KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool result = await _lanhDaoService.CreateAsync(lanhDao);
                if (!result)
                {
                    return BadRequest(new { message = "Không thể tạo thông tin lãnh đạo. Thông tin này có thể đã tồn tại." });
                }

                return CreatedAtAction(nameof(GetByMaTruongAndCCCD),
                    new { maTruong = lanhDao.MaTruong, cccd = lanhDao.CCCD }, lanhDao);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo mới thông tin lãnh đạo");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Cập nhật thông tin lãnh đạo
        /// </summary>
        [HttpPut("ldtruong/{maTruong}/cccd/{cccd}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(string maTruong, string cccd, [FromBody] KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao)
        {
            try
            {
                if (maTruong != lanhDao.MaTruong || cccd != lanhDao.CCCD)
                {
                    return BadRequest(new { message = "Mã trường hoặc CCCD không khớp" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _lanhDaoService.UpdateAsync(lanhDao);
                if (!result)
                {
                    return NotFound(new { message = $"Không tìm thấy lãnh đạo với mã trường: {maTruong} và CCCD: {cccd}" });
                }

                return Ok(new { message = "Cập nhật thông tin lãnh đạo thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thông tin lãnh đạo. MaTruong: {MaTruong}, CCCD: {CCCD}", maTruong, cccd);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Xóa thông tin lãnh đạo
        /// </summary>
        [HttpDelete("ldtruong/{maTruong}/cccd/{cccd}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(string maTruong, string cccd)
        {
            try
            {
                var result = await _lanhDaoService.DeleteAsync(maTruong, cccd);
                if (!result)
                {
                    return NotFound(new { message = $"Không tìm thấy lãnh đạo với mã trường: {maTruong} và CCCD: {cccd}" });
                }

                return Ok(new { message = "Xóa thông tin lãnh đạo thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa thông tin lãnh đạo. MaTruong: {MaTruong}, CCCD: {CCCD}", maTruong, cccd);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Kiểm tra thông tin lãnh đạo đã tồn tại chưa
        /// </summary>
        [HttpGet("check-exist-lanhdao")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CheckExist(
            [FromQuery] string maTruong,
            [FromQuery] string cccd)
        {
            try
            {
                if (string.IsNullOrEmpty(maTruong) || string.IsNullOrEmpty(cccd))
                {
                    return BadRequest(new { message = "Cần cung cấp đầy đủ mã trường và CCCD" });
                }

                bool exists = await _lanhDaoService.ExistsAsync(maTruong, cccd);
                return Ok(new { exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kiểm tra thông tin lãnh đạo tồn tại. MaTruong: {MaTruong}, CCCD: {CCCD}",
                    maTruong, cccd);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }
    }
}