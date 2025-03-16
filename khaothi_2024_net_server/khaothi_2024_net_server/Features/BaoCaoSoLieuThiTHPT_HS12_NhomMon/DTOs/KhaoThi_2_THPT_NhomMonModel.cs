namespace khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT_HS12_NhomMon.DTOs
{
    public class KhaoThi_2_THPT_NhomMonModel
    {
        public int ID { get; set; }
        public string? MaTruong { get; set; } // Mã trường, có thể null (Unchecked)
        public int TenNhom { get; set; } = 0; // Tên nhóm, kiểu int (Checked)
        public string MonLuaChon_1 { get; set; } = string.Empty; // Môn lựa chọn 1, không null (Unchecked)
        public string MonLuaChon_2 { get; set; } = string.Empty; // Môn lựa chọn 2, không null (Unchecked)
        public int SoLuong { get; set; } = 0; // Số lượng (Checked)
        public bool Lock { get; set; } = false; // Trạng thái khóa (Checked)
    }
}
