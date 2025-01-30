namespace khaothi_2024_net_server.Features.UserManagement.DTOs
{
    public class KhaoThiUser
    {
        public int ID { get; set; }
        public string TenDangNhap { get; set; } = string.Empty;
        public string MatKhau { get; set; } = string.Empty;
        public string HoTen { get; set; } = string.Empty;
        public DateTime? NgaySinh { get; set; }  // Đảm bảo là nullable DateTime
        public string MaDonVi { get; set; } = string.Empty;
        public string TenDonVi { get; set; } = string.Empty;
        public DateTime NgayTao { get; set; }
        public string MaChucVu { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string SDT { get; set; } = string.Empty;
        public string SoTaiKhoan { get; set; } = string.Empty;
        public string TenNganHang { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;
        public string MaSoThue { get; set; } = string.Empty;
        public bool Active { get; set; } = true;
        // Thêm 2 properties mới
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
