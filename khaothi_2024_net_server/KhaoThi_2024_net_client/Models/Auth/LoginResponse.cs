using System.Text.Json.Serialization;

namespace KhaoThi_2024_net_client.Models.Auth
{
    public class LoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("user")]
        public UserInfo User { get; set; }

        [JsonPropertyName("errorType")]
        public string ErrorType { get; set; }

        [JsonPropertyName("errorMessage")]
        public string ErrorMessage { get; set; }

        [JsonPropertyName("success")]
        public bool Success { get; set; }
    }

}
