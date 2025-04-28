namespace khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT.DTOs
{
    public class KhaoThi_1_THPT_ThongTin_DonViModel
    {
        public string? MaTruong { get; set; } = null;
        public string? TenTruong { get; set; } = null;
        public string? EmailNhanThongBao { get; set; } = null;
        public int QuanDangKyDuThi { get; set; } = 1;
        public string LoaiHinhDaoTao { get; set; } = null;
        public string? MaTruongBo { get; set; } = null;
        public string? DiaChiTruong { get; set; } = null;
        public string? SDTTruong { get; set; } = null;
        public string? SDTPhongHoiDong { get; set; } = null;
        public string? HoTenNhapLieu { get; set; } = null;
        public string? ChucVuNhapLieu { get; set; } = null;
        public string? SDTDiDongNhapLieu { get; set; } = null;
        public string? EmailNhapLieu { get; set; } = null;
        public int TongHieuTruong { get; set; } = 0;
        public int TongPhoHieuTruong { get; set; } = 0;
        public int TongGiaoVien { get; set; } = 0;
        public int TongNhanVien { get; set; } = 0;
        public int TongGiaoVien_ToTruong { get; set; } = 0;
        public int TongSoPhongToiDa { get; set; } = 0;
        public int TongGiaoVien_CoiThi { get; set; } = 0;
        public int TongSoPhongCoiThi { get; set; } = 0;
        public int Tong_HS_12 { get; set; } = 0;
        public int Tong_HS_KhuyetTat_Nhe { get; set; } = 0;
        public int Tong_HS_KhuyetTat_Nang { get; set; } = 0;
        public int Tong_HS_KhiemThi { get; set; } = 0;
        public int Tong_HS_CanHoTroDacBiet { get; set; } = 0;
        public string? NoiDung_HoTro_HS { get; set; } = null;
        public bool Active { get; set; } = false;
        public bool Lock { get; set; } = false;
        public bool KhoaTaiKhoan { get; set; } = false;
    }
}
