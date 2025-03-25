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
    [Authorize]
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
        /// Lấy danh sách thông tin con thí sinh phân trang.
        /// </summary>
        [HttpGet("all-phantrang-conthi")]
        [ProducesResponseType(typeof(PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThi>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null)
        {
            try
            {
                // Validate tham số đầu vào
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100;

                var (items, totalCount) = await _conThiService.GetPaginatedAsync(page, pageSize, searchTerm);

                var result = new PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThi>(
                    items: items,
                    count: totalCount,
                    page: page,
                    pageSize: pageSize);

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách thông tin con thí sinh phân trang (page: {page}, pageSize: {pageSize}, searchTerm: {searchTerm}).", page, pageSize, searchTerm);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        /// <summary>
        /// Lấy tất cả thông tin con thí sinh.
        /// </summary>
        [HttpGet("all-conthi")]
        [ProducesResponseType(typeof(IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var conThiList = await _conThiService.GetAllAsync();
                return Ok(conThiList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tất cả thông tin con thí sinh.");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        /// <summary>
        /// Lấy thông tin con thí sinh theo Mã trường, CCCD và Mã định danh của con.
        /// </summary>
        [HttpGet("{maTruong}/{cccd}/{maDinhDanhCuaCon}")]
        [ProducesResponseType(typeof(KhaoThi_5_THPT_ThongTin_ConThi), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByKeys(string maTruong, string cccd, string maDinhDanhCuaCon)
        {
            try
            {
                var conThi = await _conThiService.GetByKeysAsync(maTruong, cccd, maDinhDanhCuaCon);
                if (conThi == null)
                {
                    return NotFound(new { message = $"Không tìm thấy thông tin con thí sinh với (MaTruong: {maTruong}, CCCD: {cccd}, MaDinhDanhCuaCon: {maDinhDanhCuaCon})." });
                }

                return Ok(conThi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin con thí sinh theo khóa (MaTruong: {maTruong}, CCCD: {cccd}, MaDinhDanhCuaCon: {maDinhDanhCuaCon}).", maTruong, cccd, maDinhDanhCuaCon);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        /// <summary>
        /// Thêm mới thông tin con thí sinh.
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(KhaoThi_5_THPT_ThongTin_ConThi), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create([FromBody] KhaoThi_5_THPT_ThongTin_ConThi conThi)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool result = await _conThiService.InsertAsync(conThi);
                if (!result)
                {
                    return BadRequest(new { message = "Không thể tạo thông tin con thí sinh.  Có thể do trùng khóa hoặc dữ liệu không hợp lệ." });
                }

                return CreatedAtAction(nameof(GetByKeys),
                    new { maTruong = conThi.MaTruong, cccd = conThi.CCCD, maDinhDanhCuaCon = conThi.MaDinhDanhCuaCon }, conThi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm thông tin con thí sinh (MaTruong: {maTruong}, CCCD: {cccd}, MaDinhDanhCuaCon: {maDinhDanhCuaCon}).", conThi.MaTruong, conThi.CCCD, conThi.MaDinhDanhCuaCon);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        /// <summary>
        /// Cập nhật thông tin con thí sinh.
        /// </summary>
        [HttpPut("{maTruong}/{cccd}/{maDinhDanhCuaCon}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(string maTruong, string cccd, string maDinhDanhCuaCon, [FromBody] KhaoThi_5_THPT_ThongTin_ConThi conThi)
        {
            try
            {
                if (maTruong != conThi.MaTruong || cccd != conThi.CCCD || maDinhDanhCuaCon != conThi.MaDinhDanhCuaCon)
                {
                    return BadRequest(new { message = "Các khóa không khớp giữa URL và body." });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool result = await _conThiService.UpdateAsync(conThi);
                if (!result)
                {
                    return NotFound(new { message = $"Không tìm thấy thông tin con thí sinh để cập nhật (MaTruong: {maTruong}, CCCD: {cccd}, MaDinhDanhCuaCon: {maDinhDanhCuaCon})." });
                }

                return Ok(new { message = "Cập nhật thông tin con thí sinh thành công." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thông tin con thí sinh (MaTruong: {maTruong}, CCCD: {cccd}, MaDinhDanhCuaCon: {maDinhDanhCuaCon}).", maTruong, cccd, maDinhDanhCuaCon);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        /// <summary>
        /// Xóa thông tin con thí sinh.
        /// </summary>
        [HttpDelete("{maTruong}/{cccd}/{maDinhDanhCuaCon}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(string maTruong, string cccd, string maDinhDanhCuaCon)
        {
            try
            {
                bool result = await _conThiService.DeleteAsync(maTruong, cccd, maDinhDanhCuaCon);
                if (!result)
                {
                    return NotFound(new { message = $"Không tìm thấy thông tin con thí sinh để xóa (MaTruong: {maTruong}, CCCD: {cccd}, MaDinhDanhCuaCon: {maDinhDanhCuaCon})." });
                }

                return Ok(new { message = "Xóa thông tin con thí sinh thành công." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa thông tin con thí sinh (MaTruong: {maTruong}, CCCD: {cccd}, MaDinhDanhCuaCon: {maDinhDanhCuaCon}).", maTruong, cccd, maDinhDanhCuaCon);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }

        /// <summary>
        /// Kiểm tra xem thông tin con thí sinh đã tồn tại chưa.
        /// </summary>
        [HttpGet("exists/{maTruong}/{cccd}/{maDinhDanhCuaCon}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Exists(string maTruong, string cccd, string maDinhDanhCuaCon)
        {
            try
            {
                bool exists = await _conThiService.ExistsAsync(maTruong, cccd, maDinhDanhCuaCon);
                return Ok(new { exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kiểm tra sự tồn tại của thông tin con thí sinh (MaTruong: {maTruong}, CCCD: {cccd}, MaDinhDanhCuaCon: {maDinhDanhCuaCon}).", maTruong, cccd, maDinhDanhCuaCon);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu." });
            }
        }
    }
}