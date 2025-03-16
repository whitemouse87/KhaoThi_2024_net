using khaothi_2024_net_server.Core.Models.Common;
using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT_HS12_NhomMon.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT_HS12_NhomMon.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT_HS12_NhomMon
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // Yêu cầu Bearer token cho tất cả endpoints
    public class BCThongTinNhomMonController : ControllerBase
    {
        private readonly IBCThongTinNhomMonService _nhomMonService;
        private readonly ILogger<BCThongTinNhomMonController> _logger;

        public BCThongTinNhomMonController(
            IBCThongTinNhomMonService nhomMonService,
            ILogger<BCThongTinNhomMonController> logger)
        {
            _nhomMonService = nhomMonService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy danh sách nhóm môn có phân trang và tìm kiếm
        /// </summary>
        [HttpGet("all-phantrang")]
        [ProducesResponseType(typeof(PaginatedResult<KhaoThi_2_THPT_NhomMonModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetPaginated(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? searchTerm = null,
            [FromQuery] string? maTruong = null,
            [FromQuery] int? minSoLuong = null)
        {
            try
            {
                // Validate input parameters
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100; // Giới hạn kích thước trang tối đa

                // Gọi service để lấy dữ liệu
                var (items, totalCount) = await _nhomMonService.GetPaginatedAsync(page, pageSize, searchTerm, maTruong, minSoLuong);

                // Tạo kết quả phân trang
                var result = new PaginatedResult<KhaoThi_2_THPT_NhomMonModel>(
                    items: items,
                    count: totalCount,
                    page: page,
                    pageSize: pageSize
                );

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách nhóm môn phân trang. SearchTerm: {SearchTerm}, Page: {Page}, PageSize: {PageSize}",
                    searchTerm, page, pageSize);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Lấy danh sách nhóm môn theo mã trường
        /// </summary>
        [HttpGet("truong/{maTruong}")]
        [ProducesResponseType(typeof(IEnumerable<KhaoThi_2_THPT_NhomMonModel>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetByMaDonVi(string maTruong)
        {
            try
            {
                var nhomMons = await _nhomMonService.GetByMaDonViAsync(maTruong);
                if (nhomMons == null || !nhomMons.Any())
                {
                    return NotFound(new { message = $"Không tìm thấy nhóm môn nào cho trường có mã: {maTruong}" });
                }

                return Ok(nhomMons);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách nhóm môn theo mã trường: {MaTruong}", maTruong);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Tạo mới nhóm môn
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(KhaoThi_2_THPT_NhomMonModel), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Create([FromBody] KhaoThi_2_THPT_NhomMonModel nhomMon)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                int id = await _nhomMonService.CreateAsync(nhomMon);
                if (id <= 0)
                {
                    return BadRequest(new { message = "Không thể tạo nhóm môn. Nhóm môn này có thể đã tồn tại." });
                }

                nhomMon.ID = id;
                return CreatedAtAction(nameof(GetByMaDonVi), new { maTruong = nhomMon.MaTruong }, nhomMon);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo mới nhóm môn");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Cập nhật số lượng cho nhóm môn
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Update(int id, [FromBody] KhaoThi_2_THPT_NhomMonModel nhomMon)
        {
            try
            {
                if (id != nhomMon.ID)
                {
                    return BadRequest(new { message = "ID không khớp" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _nhomMonService.UpdateAsync(nhomMon);
                if (!result)
                {
                    return NotFound(new { message = $"Không tìm thấy nhóm môn với ID: {id}" });
                }

                return Ok(new { message = "Cập nhật số lượng thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật nhóm môn ID: {Id}", id);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Xóa nhóm môn
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _nhomMonService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new { message = $"Không tìm thấy nhóm môn với ID: {id}" });
                }

                return Ok(new { message = "Xóa nhóm môn thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa nhóm môn ID: {Id}", id);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Kiểm tra nhóm môn đã tồn tại chưa
        /// </summary>
        [HttpGet("check-exist")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> CheckExist(
            [FromQuery] string maTruong,
            [FromQuery] string monLuaChon1,
            [FromQuery] string monLuaChon2)
        {
            try
            {
                if (string.IsNullOrEmpty(maTruong) || string.IsNullOrEmpty(monLuaChon1) || string.IsNullOrEmpty(monLuaChon2))
                {
                    return BadRequest(new { message = "Cần cung cấp đầy đủ mã trường và các môn lựa chọn" });
                }

                bool exists = await _nhomMonService.IsNhomMonExistAsync(maTruong, monLuaChon1, monLuaChon2);
                return Ok(new { exists });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kiểm tra nhóm môn tồn tại. MaTruong: {MaTruong}, MonLuaChon1: {MonLuaChon1}, MonLuaChon2: {MonLuaChon2}",
                    maTruong, monLuaChon1, monLuaChon2);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }
    }
}
