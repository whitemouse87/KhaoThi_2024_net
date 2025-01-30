using System.Text.Json.Serialization;

namespace KhaoThi_2024_net_client.Models
{
    public class ErrorResponse
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("errorType")]
        public string? ErrorType { get; set; }
    }
}
