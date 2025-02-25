using System.ComponentModel.DataAnnotations;

namespace KhaoThi_2024_net_client.Models.Users
{
    public class ChangePasswordModel
    {
        //public string OldPassword { get; set; } = string.Empty;
        //public string NewPassword { get; set; } = string.Empty;
        //public string ConfirmPassword { get; set; } = string.Empty;

        public int? UserId { get; set; } // Optional, nếu không có sẽ lấy từ token

        [Required(ErrorMessage = "Mật khẩu hiện tại không được để trống")]
        public string? CurrentPassword { get; set; }

        [Required(ErrorMessage = "Mật khẩu mới không được để trống")]
        [MinLength(8, ErrorMessage = "Mật khẩu mới phải có ít nhất 8 ký tự")]
        public string? NewPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu mới")]
        [Compare(nameof(NewPassword), ErrorMessage = "Xác nhận mật khẩu không khớp")]
        public string? ConfirmPassword { get; set; }




    }
}
