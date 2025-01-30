using System.Text.Json.Serialization;

namespace KhaoThi_2024_net_client.Models.Auth
{
    public class ValidateTokenResponse
    {
        [JsonPropertyName("isValid")]
        public bool IsValid { get; set; }

        [JsonPropertyName("user")]
        public Dictionary<string, string> UserClaims { get; set; }
    }
}
