//using KhaoThi_2024_net_client.Models.Auth;

//namespace KhaoThi_2024_net_client.Services.Auth
//{
//    public class AuthActions
//    {
//        public class LoginAction { public LoginRequest? Request { get; set; } }
//        public class LoginSuccessAction { public string? Token { get; set; } public UserInfo? User { get; set; } }
//        public class LoginFailureAction { public string? Error { get; set; } }
//        public class LogoutAction { }
//    }
//}

using KhaoThi_2024_net_client.Models.Auth;

namespace KhaoThi_2024_net_client.Services.Auth
{
    public static class AuthActions
    {
        // Login actions - Các actions liên quan đến đăng nhập
        public record LoginAction(LoginRequest Request);
        public record LoginSuccessAction(string Token, UserInfo User);
        public record LoginFailureAction(string ErrorMessage);

        // Logout actions - Các actions liên quan đến đăng xuất
        public record LogoutAction();  // Action khởi tạo quá trình đăng xuất
        public record LogoutSuccessAction();  // Action khi đăng xuất thành công
        public record LogoutFailureAction(string ErrorMessage);  // Action khi đăng xuất thất bại

        // Utils actions - Các actions tiện ích
        public record SetLoadingAction(bool IsLoading);  // Action điều khiển trạng thái loading
        public record ClearErrorAction();  // Action xóa thông báo lỗi
        public record ShowNotificationAction(string Message, string Type);  // Action hiển thị thông báo
        
        // Refresh token actions - Các actions liên quan đến làm mới token
        public record RefreshTokenAction();  // Action khởi tạo quá trình refresh token
        public record RefreshTokenSuccessAction(string Token, UserInfo User);  // Action khi refresh thành công
        public record RefreshTokenFailureAction(string ErrorMessage);  // Action khi refresh thất bại
    }
}