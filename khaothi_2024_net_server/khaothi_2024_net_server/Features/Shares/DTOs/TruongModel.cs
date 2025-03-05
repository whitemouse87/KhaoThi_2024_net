namespace khaothi_2024_net_server.Features.Shares.DTOs
{
    public class TruongModel
    {
        public string MaTruong { get; set; } = string.Empty;
        public string TenTruong { get; set; } = string.Empty;
        public int MaQuan { get; set; }
        public string TenQuan { get; set; } = string.Empty;
        public string CapHoc { get; set; } = string.Empty;
        public string MaTruongNew { get; set; } = string.Empty;
        public bool CoiThi { get; set; }
        public bool ChamThi { get; set; }
        public bool ChucVuVanPhong { get; set; }



    }
}
