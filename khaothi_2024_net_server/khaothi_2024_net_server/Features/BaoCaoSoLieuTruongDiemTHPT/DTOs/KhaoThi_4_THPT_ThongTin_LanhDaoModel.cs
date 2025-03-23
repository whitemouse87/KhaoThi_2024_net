namespace khaothi_2024_net_server.Features.BaoCaoSoLieuTruongDiemTHPT.DTOs
{
    public class KhaoThi_4_THPT_ThongTin_LanhDaoModel
    {
        public string MaTruong { get; set; } = string.Empty; // PrimaryKey, Unchecked

        public string CCCD { get; set; } = string.Empty; // PrimaryKey, Unchecked

        public string HoTen { get; set; } = string.Empty; // Checked

        public int NamSinh { get; set; } = 0; // Checked

        public string ChucVuDonVi { get; set; } = string.Empty; // Checked

        public string DiaChiNha { get; set; } = string.Empty; // Checked

        public int QuanNha { get; set; } = 0; // Checked

        public string SDTDiDong { get; set; } = string.Empty; // Unchecked

        public string Email { get; set; } = string.Empty; // Checked

        public bool CoiThiTS10 { get; set; } = false; // Checked

        public string ChucVuCoiThiTS10 { get; set; } = string.Empty; // Checked

        public string LyDoKhongThamGiaTS10 { get; set; } = string.Empty; // Checked

        public bool CoiThiTHPT { get; set; } = false; // Checked

        public string ChucVuCoiThiTHPT { get; set; } = string.Empty; // Checked

        public string LyDoKhongThamGiaTHPT { get; set; } = string.Empty; // Checked

        public string CumChuyenMon { get; set; } = string.Empty; // Checked

        public string ChucVuCumChuyenMon { get; set; } = string.Empty; // Checked
    }
}
