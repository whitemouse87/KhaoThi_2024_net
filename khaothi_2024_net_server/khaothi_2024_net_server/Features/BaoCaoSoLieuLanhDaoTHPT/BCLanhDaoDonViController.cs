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
    [Authorize]
    public class BCLanhDaoDonViController : ControllerBase
    {
        private readonly IBCLanhDaoDonViService _lanhDaoService;
        private readonly ILogger<BCLanhDaoDonViController> _logger;

        public BCLanhDaoDonViController(IBCLanhDaoDonViService lanhDaoService, ILogger<BCLanhDaoDonViController> logger)
        {
            _lanhDaoService = lanhDaoService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy thông tin lãnh đạo theo mã trường và CCCD
        /// </summary>
        [HttpGet("{maTruong}/{cccd}")]
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
                    return NotFound(new { message = $"Không tìm thấy lãnh đạo với mã trường {maTruong} và CCCD {cccd}" });
                }

                return Ok(lanhDao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin lãnh đạo theo mã trường và CCCD: {MaTruong}, {CCCD}", maTruong, cccd);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Lấy danh sách lãnh đạo theo mã trường
        /// </summary>
        [HttpGet("truong/{maTruong}")]
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
                    return NotFound(new { message = $"Không tìm thấy lãnh đạo nào với mã trường {maTruong}" });
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
        /// Lấy danh sách tất cả lãnh đạo (Cần xem xét cẩn thận trước khi sử dụng)
        /// </summary>
        [HttpGet("all")]
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
                _logger.LogError(ex, "Lỗi khi lấy tất cả thông tin lãnh đạo");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Tạo mới thông tin lãnh đạo
        /// </summary>
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create([FromBody] KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var created = await _lanhDaoService.CreateAsync(lanhDao);
                if (!created)
                {
                    return Conflict(new { message = $"Thông tin lãnh đạo với mã trường {lanhDao.MaTruong} và CCCD {lanhDao.CCCD} đã tồn tại hoặc dữ liệu không hợp lệ." });
                }

                return CreatedAtAction(nameof(GetByMaTruongAndCCCD), new { maTruong = lanhDao.MaTruong, cccd = lanhDao.CCCD }, lanhDao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo mới thông tin lãnh đạo: {MaTruong}, {CCCD}", lanhDao.MaTruong, lanhDao.CCCD);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Cập nhật thông tin lãnh đạo
        /// </summary>
        [HttpPut("{maTruong}/{cccd}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(string maTruong, string cccd, [FromBody] KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (maTruong != lanhDao.MaTruong || cccd != lanhDao.CCCD)
                {
                    return BadRequest(new { message = "Mã trường và CCCD trong URL không khớp với dữ liệu trong body" });
                }

                var updated = await _lanhDaoService.UpdateAsync(lanhDao);
                if (!updated)
                {
                    return NotFound(new { message = $"Không tìm thấy thông tin lãnh đạo với mã trường {maTruong} và CCCD {cccd}" });
                }

                return NoContent(); // Trả về 204 No Content khi cập nhật thành công
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thông tin lãnh đạo: {MaTruong}, {CCCD}", maTruong, cccd);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Xóa thông tin lãnh đạo
        /// </summary>
        [HttpDelete("{maTruong}/{cccd}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(string maTruong, string cccd)
        {
            try
            {
                var deleted = await _lanhDaoService.DeleteAsync(maTruong, cccd);
                if (!deleted)
                {
                    return NotFound(new { message = $"Không tìm thấy thông tin lãnh đạo với mã trường {maTruong} và CCCD {cccd}" });
                }

                return NoContent(); // Trả về 204 No Content khi xóa thành công
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa thông tin lãnh đạo: {MaTruong}, {CCCD}", maTruong, cccd);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Lấy danh sách lãnh đạo có phân trang và tìm kiếm
        /// </summary>
        [HttpGet("all-phantrang")]
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
                // Validate input parameters (optional)
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100; // Giới hạn kích thước trang tối đa

                var (items, totalCount) = await _lanhDaoService.GetPaginatedAsync(page, pageSize, searchTerm, maTruong, namSinh, cccd);

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
                _logger.LogError(ex, "Lỗi khi lấy danh sách lãnh đạo phân trang. SearchTerm: {SearchTerm}, Page: {Page}, PageSize: {PageSize}, MaTruong: {MaTruong}, NamSinh: {NamSinh}, CCCD: {CCCD}",
                    searchTerm, page, pageSize, maTruong, namSinh, cccd);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Kiểm tra sự tồn tại của thông tin lãnh đạo
        /// </summary>
        [HttpGet("exists/{maTruong}/{cccd}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Exists(string maTruong, string cccd)
        {
            try
            {
                var exists = await _lanhDaoService.ExistsAsync(maTruong, cccd);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kiểm tra sự tồn tại của thông tin lãnh đạo: {MaTruong}, {CCCD}", maTruong, cccd);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }
    }
}
