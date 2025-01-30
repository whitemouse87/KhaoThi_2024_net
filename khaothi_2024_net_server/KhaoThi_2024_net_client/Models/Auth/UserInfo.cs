using System.Text.Json.Serialization;

namespace KhaoThi_2024_net_client.Models.Auth
{
    public class UserInfo
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("tenDangNhap")]
        public string TenDangNhap { get; set; }

        [JsonPropertyName("hoTen")]
        public string HoTen { get; set; }

        [JsonPropertyName("maDonVi")]
        public string MaDonVi { get; set; }

        [JsonPropertyName("maChucVu")]
        public string MaChucVu { get; set; }

        [JsonPropertyName("email")]
        public string? Email { get; set; } // Nullable cho trường hợp email null
    }
}
