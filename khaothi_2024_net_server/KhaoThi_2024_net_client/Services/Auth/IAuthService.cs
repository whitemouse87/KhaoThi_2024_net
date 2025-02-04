using KhaoThi_2024_net_client.Models.Auth;
namespace KhaoThi_2024_net_client.Services.Auth
{
    public interface IAuthService
    {
        Task<LoginResponse> Login(LoginRequest request);
        Task<UserInfo> GetUserInfo(string token);
        Task Logout();
        Task<ValidateTokenResponse> ValidateToken();
        Task<int?> GetUserIdFromToken();
        Task<LoginResponse> RefreshToken(); // Thêm method này
        bool IsTokenExpired(string token);

    }
}
