using khaothi_2024_net_server.Features.Authentication.DTOs;

namespace khaothi_2024_net_server.Core.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
        Task<bool> LogoutAsync(int userId);
        Task<LoginResponse> RefreshTokenAsync(string refreshToken);
        Task<bool> LogoutAllDevicesAsync(int userId); // Add this method to the interface

        string GenerateRefreshToken();
    }
}
