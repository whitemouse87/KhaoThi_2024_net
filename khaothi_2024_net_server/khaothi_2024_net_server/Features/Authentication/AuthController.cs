using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.Authentication.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
[ApiController]
[Route("api/[controller]")]
[ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorResponse))]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IKhaoThiUserRepository _userRepository;
    private readonly ILogger<AuthController> _logger;
    private readonly IDistributedCache _cache;

    public AuthController(
        IAuthService authService,
            IKhaoThiUserRepository userRepository,
            ILogger<AuthController> logger,
            IDistributedCache cache)
    {
        _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }
    private int? GetUserIdFromClaims()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return claim != null && int.TryParse(claim.Value, out int userId) ? userId : null;
    }
    [HttpPost("login")]
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(ErrorResponse))]
    [ProducesResponseType(StatusCodes.Status429TooManyRequests, Type = typeof(ErrorResponse))]
    public async Task<ActionResult<LoginResponse>> Login([FromBody][Required] LoginRequest request)
    {
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        _logger.LogInformation("Login attempt for user: {Username} from IP: {IpAddress}",
            request.TenDangNhap, ipAddress);

        try
        {
            var response = await _authService.LoginAsync(request);
            if (!response.Success)
            {
                if (response.ErrorType == LoginErrorType.TooManyAttempts)
                {
                    return StatusCode(StatusCodes.Status429TooManyRequests, new ErrorResponse
                    {
                        Message = response.ErrorMessage,
                        ErrorType = response.ErrorType.ToString()
                    });
                }

                return Unauthorized(new ErrorResponse
                {
                    Message = response.ErrorMessage,
                    ErrorType = response.ErrorType.ToString()
                });
            }

            _logger.LogInformation("User {Username} logged in successfully from IP: {IpAddress}",
                request.TenDangNhap, ipAddress);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for user {Username} from IP {IpAddress}",
                request.TenDangNhap, ipAddress);
            return StatusCode(500, new ErrorResponse { Message = "Đã xảy ra lỗi trong quá trình đăng nhập" });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = GetUserIdFromClaims();
        var jti = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();

        if (!userId.HasValue)
        {
            _logger.LogWarning("Logout attempt with invalid user ID from IP: {IpAddress}", ipAddress);
            return Unauthorized(new ErrorResponse { Message = "Không tìm thấy thông tin người dùng" });
        }

        try
        {
            var result = await _authService.LogoutAsync(userId.Value);
            if (result)
            {
                // Lưu Token vào Blacklist (Redis)
                if (!string.IsNullOrEmpty(jti))
                {
                    await _cache.SetStringAsync($"blacklist_{jti}", "revoked", new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Token bị vô hiệu trong 30 phút
                    });
                }

                _logger.LogInformation("User {UserId} logged out successfully", userId);
                return Ok(new ApiResponse { Success = true, Message = "Đăng xuất thành công" });
            }

            return BadRequest(new ApiResponse { Success = false, Message = "Đăng xuất thất bại" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Logout failed for user {UserId}", userId);
            return StatusCode(500, new ErrorResponse { Message = "Đã xảy ra lỗi trong quá trình đăng xuất" });
        }
    }

    [Authorize]
    [HttpGet("validate")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
 
    public IActionResult ValidateToken()
    {
        try
        {
            var claims = User.Claims.ToDictionary(
                c => c.Type switch
                {
                    ClaimTypes.NameIdentifier => "id",
                    ClaimTypes.Name => "tenDangNhap",
                    "MaDonVi" => "maDonVi",
                    "MaChucVu" => "maChucVu",
                    _ => c.Type
                },
                c => c.Value
            );

            var jti = User.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.Value;
            if (!string.IsNullOrEmpty(jti))
            {
                var isRevoked = _cache.GetString($"blacklist_{jti}");
                if (!string.IsNullOrEmpty(isRevoked))
                {
                    return Unauthorized(new ErrorResponse { Message = "Token đã bị thu hồi" });
                }
            }

            return Ok(new { isValid = true, user = claims });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token validation failed");
            return StatusCode(500, new ErrorResponse { Message = "Lỗi xác thực token" });
        }
    }
}
public class ErrorResponse
{
    public string Message { get; set; }
    public string ErrorType { get; set; }
}

public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
}