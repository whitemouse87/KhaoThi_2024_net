using System.ComponentModel.DataAnnotations;

namespace khaothi_2024_net_server.Features.UserManagement.DTOs
{
    public class ChangePasswordRequest
    {
        public int? UserId { get; set; } // Optional, nếu không có sẽ lấy từ token

        [Required(ErrorMessage = "Mật khẩu hiện tại không được để trống")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        [MinLength(8, ErrorMessage = "Mật khẩu mới phải có ít nhất 8 ký tự")]
        public string NewPassword { get; set; }

        [Compare("NewPassword", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; }

    }
}
