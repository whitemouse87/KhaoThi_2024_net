using System.ComponentModel.DataAnnotations;

namespace khaothi_2024_net_server.Features.Authentication.DTOs
{
    public class LoginRequest
    {
        [Required]
        [MinLength(3)]
        public string TenDangNhap { get; set; } = string.Empty;
        [Required]
        [MinLength(6)]
        public string MatKhau { get; set; } = string.Empty;
    }
}
