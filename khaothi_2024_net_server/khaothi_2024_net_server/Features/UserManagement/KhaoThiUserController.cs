using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Core.Models;
using khaothi_2024_net_server.Core.Models.Common;
using khaothi_2024_net_server.Features.UserManagement.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace khaothi_2024_net_server.Features.UserManagement
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]  // Thêm attribute này để yêu cầu Bearer token cho tất cả endpoints
    public class KhaoThiUserController : ControllerBase
    {
        private readonly IKhaoThiUserService _userService;
        private readonly ILogger<KhaoThiUserController> _logger;

        public KhaoThiUserController(
            IKhaoThiUserService userService,
            ILogger<KhaoThiUserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Lấy thông tin người dùng theo ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(KhaoThiUser), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var user = await _userService.GetByIdAsync(id);
                if (user == null)
                {
                    return NotFound(new { message = $"Không tìm thấy người dùng với ID: {id}" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy thông tin người dùng ID: {id}");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }
        /// <summary>
        /// Lấy danh sách tất cả người dùng
        /// </summary>
        /// <returns>Danh sách người dùng</returns>
        [HttpGet("all")]
        [Authorize(Roles = "Admin")] // Chỉ Admin mới có quyền xem tất cả
        [ProducesResponseType(typeof(IEnumerable<KhaoThiUser>), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var users = await _userService.GetAllAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi truy xuất tất cả người dùng");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }
        /// <summary>
        /// Lấy danh sách người dùng có phân trang và tìm kiếm
        /// </summary>
        [HttpGet("all-phantrang")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(PaginatedResult<KhaoThiUser>), (int)HttpStatusCode.OK)]
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
                var (items, totalCount) = await _userService.GetPaginatedAsync(page, pageSize, searchTerm);

                // Tạo kết quả phân trang sử dụng constructor của PaginatedResult
                var result = new PaginatedResult<KhaoThiUser>(
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
                _logger.LogError(ex, "Lỗi khi lấy danh sách người dùng phân trang. SearchTerm: {SearchTerm}, Page: {Page}, PageSize: {PageSize}",
                    searchTerm, page, pageSize);
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }


        /// <summary>
        /// Tạo mới người dùng
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(KhaoThiUser), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Create([FromBody] KhaoThiUser user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdUser = await _userService.CreateAsync(user);
                return CreatedAtAction(nameof(GetById), new { id = createdUser.ID }, createdUser);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo mới người dùng");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Cập nhật thông tin người dùng
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Update(int id, [FromBody] KhaoThiUser user)
        {
            try
            {
                if (id != user.ID)
                {
                    return BadRequest(new { message = "ID không khớp" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var result = await _userService.UpdateAsync(user);
                if (!result)
                {
                    return NotFound(new { message = $"Không tìm thấy người dùng với ID: {id}" });
                }

                return Ok(new { message = "Cập nhật thành công" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật người dùng ID: {id}");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Xóa người dùng
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _userService.DeleteAsync(id);
                if (!result)
                {
                    return NotFound(new { message = $"Không tìm thấy người dùng với ID: {id}" });
                }

                return Ok(new { message = "Xóa thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi xóa người dùng ID: {id}");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Lấy danh sách người dùng theo mã đơn vị
        /// </summary>
        [HttpGet("by-donvi/{maDonVi}")]
        [ProducesResponseType(typeof(IEnumerable<KhaoThiUser>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetByMaDonVi(string maDonVi)
        {
            try
            {
                var users = await _userService.GetByMaDonViAsync(maDonVi);
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy danh sách người dùng theo mã đơn vị: {maDonVi}");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Thêm nhiều người dùng cùng lúc
        /// </summary>
        [HttpPost("bulk-insert")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> BulkInsert([FromBody] IEnumerable<KhaoThiUser> users)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _userService.BulkInsertUsersAsync(users);
                return Ok(new { message = "Thêm dữ liệu hàng loạt thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm nhiều người dùng");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Cập nhật nhiều người dùng cùng lúc
        /// </summary>
        [HttpPut("bulk-update")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> BulkUpdate([FromBody] IEnumerable<KhaoThiUser> users)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _userService.BulkUpdateUsersAsync(users);
                return Ok(new { message = "Cập nhật dữ liệu hàng loạt thành công" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật nhiều người dùng");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }

        /// <summary>
        /// Kiểm tra tên đăng nhập đã tồn tại
        /// </summary>
        [HttpGet("check-username/{username}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CheckUsername(string username)
        {
            try
            {
                var exists = await _userService.IsUsernameExistAsync(username);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi kiểm tra tên đăng nhập: {username}");
                return StatusCode(500, new { message = "Đã xảy ra lỗi khi xử lý yêu cầu" });
            }
        }


        [HttpPost("change-password")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new ApiResponse
                    {
                        Success = false,
                        Message = "Dữ liệu không hợp lệ",
                        Errors = ModelState.Values
                            .SelectMany(v => v.Errors)
                            .Select(e => e.ErrorMessage)
                            .ToList()
                    });
                }

                // Lấy ID từ request hoặc token
                int userId;
                if (request.UserId.HasValue)
                {
                    // Nếu có ID trong request, kiểm tra quyền admin
                    if (!User.IsInRole("Admin"))
                    {
                        return Forbid();
                    }
                    userId = request.UserId.Value;
                }
                else
                {
                    // Nếu không có ID, lấy từ token
                    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out userId))
                    {
                        return Unauthorized(new ApiResponse
                        {
                            Success = false,
                            Message = "Không thể xác thực người dùng"
                        });
                    }
                }

                var result = await _userService.ChangePasswordAsync(userId, request);

                if (result)
                {
                    return Ok(new ApiResponse
                    {
                        Success = true,
                        Message = "Đổi mật khẩu thành công"
                    });
                }

                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Không thể thay đổi mật khẩu"
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi không xác định khi đổi mật khẩu");
                return StatusCode(500, new ApiResponse
                {
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xử lý yêu cầu"
                });
            }
        }
    }


}

