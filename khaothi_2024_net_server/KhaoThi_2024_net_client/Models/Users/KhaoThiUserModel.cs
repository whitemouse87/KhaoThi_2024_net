namespace KhaoThi_2024_net_client.Models.Users
{
    public class KhaoThiUserModel
    {

        public int ID { get; set; }

        // Thông tin đăng nhập cơ bản
        public string TenDangNhap { get; set; } = string.Empty;
        // Không lấy MatKhau vì đây là thông tin nhạy cảm

        // Thông tin cá nhân
        public string HoTen { get; set; } = string.Empty;
        public DateTime? NgaySinh { get; set; }
        public string Email { get; set; } = string.Empty;
        public string SDT { get; set; } = string.Empty;
        public string DiaChi { get; set; } = string.Empty;

        // Thông tin đơn vị
        public string MaDonVi { get; set; } = string.Empty;
        public string TenDonVi { get; set; } = string.Empty;
        public string MaChucVu { get; set; } = string.Empty;

        // Thông tin thanh toán
        public string SoTaiKhoan { get; set; } = string.Empty;
        public string TenNganHang { get; set; } = string.Empty;
        public string MaSoThue { get; set; } = string.Empty;

        // Trạng thái
        public bool Active { get; set; } = true;
        public DateTime NgayTao { get; set; }

        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}
