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
        public record LoginAction(LoginRequest Request);
        public record LoginSuccessAction(string Token, UserInfo User);
        public record LoginFailureAction(string? Error);
        public record LogoutAction();
    }
}
//}
