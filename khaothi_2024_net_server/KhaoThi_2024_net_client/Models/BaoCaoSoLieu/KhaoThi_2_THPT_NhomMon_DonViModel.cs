namespace KhaoThi_2024_net_client.Models.BaoCaoSoLieu
{
    public class KhaoThi_2_THPT_NhomMon_DonViModel
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
