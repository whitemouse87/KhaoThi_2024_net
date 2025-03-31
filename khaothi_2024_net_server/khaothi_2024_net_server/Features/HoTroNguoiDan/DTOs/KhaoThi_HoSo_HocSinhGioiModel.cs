namespace khaothi_2024_net_server.Features.HoTroNguoiDan.DTOs
{
    public class KhaoThi_HoSo_HocSinhGioiModel
    {
        public int ID { get; set; } // PrimaryKey, Unchecked

        public string Dan_HoTen { get; set; } = string.Empty; // Checked

        public string Dan_VaiTro { get; set; } = string.Empty; // Checked

        public string Dan_LoaiCauHoi { get; set; } = string.Empty; // Checked

        public string Dan_SoDiDong { get; set; } = string.Empty; // Checked

        public string Dan_Email { get; set; } = string.Empty; // Checked

        public string Dan_MaDinhDanhHocSinh { get; set; } = string.Empty; // Checked

        public string Dan_HoVaTenHocSinh { get; set; } = string.Empty; // Checked

        public DateTime Dan_NTNS { get; set; } // Checked

        public string Dan_TenTruong { get; set; } = string.Empty; // Checked

        public string Dan_CapHoc { get; set; } = string.Empty; // Checked

        public string Dan_MaQuan { get; set; } = string.Empty; // Checked

        public string Dan_NoiDungCanHoTro { get; set; } = string.Empty; // Checked

        public string Truong_LienHe { get; set; } = string.Empty; // Checked

        public string Truong_HoVaTenDaHoTro { get; set; } = string.Empty; // Checked

        public string Truong_TenTruongDaHoTro { get; set; } = string.Empty; // Checked

        public string Truong_ChatLuongHoTro { get; set; } = string.Empty; // Checked

        public int Truong_DanhGia { get; set; } // Checked

        public string So_CanBoXuLy { get; set; } = string.Empty; // Checked

        public string So_CauTraLoi { get; set; } = string.Empty; // Checked

        public bool So_XuLy { get; set; } = false; // Checked
    }
}
