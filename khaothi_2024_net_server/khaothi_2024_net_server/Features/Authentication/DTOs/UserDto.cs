namespace khaothi_2024_net_server.Features.Authentication.DTOs
{
    public class UserDto
    {
        public int ID { get; set; }
        public string? TenDangNhap { get; set; }
        public string? HoTen { get; set; }
        public string? MaDonVi { get; set; }
        public string? MaChucVu { get; set; }
        public string? Email { get; set; }
    }
}
