using System.Text.Json.Serialization;

namespace KhaoThi_2024_net_client.Models.Auth
{
    public class UserInfo
    {



        public int ID { get; set; }


        public string TenDangNhap { get; set; } = string.Empty;


        public string HoTen { get; set; } = string.Empty;


        public string MaDonVi { get; set; } = string.Empty;
        public string TenDonVi { get; set; } = string.Empty;
        public string MaChucVu { get; set; } = string.Empty;


        public string Email { get; set; } = string.Empty;
    }
}
