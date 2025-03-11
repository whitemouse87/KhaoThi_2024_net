using Azure.Core;
using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT.DTOs;
using khaothi_2024_net_server.Features.Logging.DTOs;
using khaothi_2024_net_server.Features.UserManagement.DTOs;
using khaothi_2024_net_server.Infrastructure.Repositories;
using System.Net;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT
{
    public class BCThongTinDonViRepository : IBCThongTinDonViRepository
    {
        private readonly IDataAccessLayer _dataAccess;
        private readonly ILogger<BCThongTinDonViRepository> _logger;
        private const string TableName = "KhaoThi_1_THPT_ThongTin_DonVi";
        public BCThongTinDonViRepository(IDataAccessLayer dataAccess, ILogger<BCThongTinDonViRepository> logger)
        {
            _dataAccess = dataAccess;
            _logger = logger;
        }
        public async Task<KhaoThiUser> GetByIdAsync(int id)
        {
            const string sql = @"SELECT * FROM KhaoThi_1_THPT_ThongTin_DonVi WHERE ID = @Id";
            return await _dataAccess.QueryFirstOrDefaultAsync<KhaoThiUser>(sql, "@Id", id);
        }

        public async Task<bool> UpdateAsync(KhaoThi_1_THPT_ThongTin_DonViModel ThongTin)
        {
            try
            {
                const string sql = @"
            UPDATE KhaoThi_1_THPT_ThongTin_DonVi 
            SET EmailNhanThongBao = @EmailNhanThongBao,
            QuanDangKyDuThi = @QuanDangKyDuThi,
            LoaiHinhDaoTao = @LoaiHinhDaoTao,
            MaTruongBo = @MaTruongBo,
            DiaChiTruong = @DiaChiTruong,
            SDTTruong = @SDTTruong,
            SDTPhongHoiDong = @SDTPhongHoiDong,
            HoTenNhapLieu = @HoTenNhapLieu,
            ChucVuNhapLieu = @ChucVuNhapLieu,
            SDTDiDongNhapLieu = @SDTDiDongNhapLieu,
            EmailNhapLieu = @EmailNhapLieu,
            TongHieuTruong = @TongHieuTruong,
            TongPhoHieuTruong = @TongPhoHieuTruong,
            TongGiaoVien = @TongGiaoVien,
            TongNhanVien = @TongNhanVien,
            TongGiaoVien_ToTruong = @TongGiaoVien_ToTruong,
            TongSoPhongToiDa = @TongSoPhongToiDa,
            TongGiaoVien_CoiThi = @TongGiaoVien_CoiThi,
            TongSoPhongCoiThi = @TongSoPhongCoiThi,
            Tong_HS_12 = @Tong_HS_12,
            Tong_HS_KhuyetTat_Nhe = @Tong_HS_KhuyetTat_Nhe,
            Tong_HS_KhuyetTat_Nang = @Tong_HS_KhuyetTat_Nang,
            Tong_HS_KhiemThi = @Tong_HS_KhiemThi,
            Tong_HS_CanHoTroDacBiet = @Tong_HS_CanHoTroDacBiet,
            NoiDung_HoTro_HS = @NoiDung_HoTro_HS,
            Active=@Active
        WHERE  MaTruong = @MaTruong";

                // Xử lý các giá trị null
                var parameters = new
                {
                    MaTruong = ThongTin.MaTruong ?? string.Empty,
                    EmailNhanThongBao = ThongTin.EmailNhanThongBao ?? string.Empty,
                    QuanDangKyDuThi = ThongTin.QuanDangKyDuThi,
                    LoaiHinhDaoTao = ThongTin.LoaiHinhDaoTao ?? string.Empty,
                    MaTruongBo = ThongTin.MaTruongBo ?? string.Empty,
                    DiaChiTruong = ThongTin.DiaChiTruong ?? string.Empty,
                    SDTTruong = ThongTin.SDTTruong ?? string.Empty,
                    SDTPhongHoiDong = ThongTin.SDTPhongHoiDong ?? string.Empty,
                    HoTenNhapLieu = ThongTin.HoTenNhapLieu ?? string.Empty,
                    ChucVuNhapLieu = ThongTin.ChucVuNhapLieu ?? string.Empty,
                    SDTDiDongNhapLieu = ThongTin.SDTDiDongNhapLieu ?? string.Empty,
                    EmailNhapLieu = ThongTin.EmailNhapLieu ?? string.Empty,
                    TongHieuTruong = ThongTin.TongHieuTruong,
                    TongPhoHieuTruong = ThongTin.TongPhoHieuTruong,
                    TongGiaoVien = ThongTin.TongGiaoVien,
                    TongNhanVien = ThongTin.TongNhanVien,
                    TongGiaoVien_ToTruong = ThongTin.TongGiaoVien_ToTruong,
                    TongSoPhongToiDa = ThongTin.TongSoPhongToiDa,
                    TongGiaoVien_CoiThi = ThongTin.TongGiaoVien_CoiThi,
                    TongSoPhongCoiThi = ThongTin.TongSoPhongCoiThi,
                    Tong_HS_12 = ThongTin.Tong_HS_12,
                    Tong_HS_KhuyetTat_Nhe = ThongTin.Tong_HS_KhuyetTat_Nhe,
                    Tong_HS_KhuyetTat_Nang = ThongTin.Tong_HS_KhuyetTat_Nang,
                    Tong_HS_KhiemThi = ThongTin.Tong_HS_KhiemThi,
                    Tong_HS_CanHoTroDacBiet = ThongTin.Tong_HS_CanHoTroDacBiet,
                    NoiDung_HoTro_HS = ThongTin.NoiDung_HoTro_HS ?? string.Empty,
                    Active = ThongTin.Active

                };

                var result = await _dataAccess.ExecuteAsync(sql,
                    "@EmailNhanThongBao", parameters.EmailNhanThongBao.Trim(),
                    "@QuanDangKyDuThi", parameters.QuanDangKyDuThi,
                    "@LoaiHinhDaoTao", parameters.LoaiHinhDaoTao,
                    "@MaTruongBo", parameters.MaTruongBo.Trim().ToUpper(),
                    "@DiaChiTruong", parameters.DiaChiTruong.Trim(),
                    "@SDTTruong", parameters.SDTTruong.Trim(),
                    "@SDTPhongHoiDong", parameters.SDTPhongHoiDong.Trim(),
                    "@HoTenNhapLieu", parameters.HoTenNhapLieu.Trim().ToUpper(),
                    "@ChucVuNhapLieu", parameters.ChucVuNhapLieu.Trim(),
                    "@SDTDiDongNhapLieu", parameters.SDTDiDongNhapLieu.Trim(),
                    "@EmailNhapLieu", parameters.EmailNhapLieu.Trim(),
                    "@TongHieuTruong", parameters.TongHieuTruong,
                    "@TongPhoHieuTruong", parameters.TongPhoHieuTruong,
                    "@TongGiaoVien", parameters.TongGiaoVien,
                    "@TongNhanVien", parameters.TongNhanVien,
                    "@TongGiaoVien_ToTruong", parameters.TongGiaoVien_ToTruong,
                    "@TongSoPhongToiDa", parameters.TongSoPhongToiDa,
                    "@TongGiaoVien_CoiThi", parameters.TongGiaoVien_CoiThi,
                    "@TongSoPhongCoiThi", parameters.TongSoPhongCoiThi,
                    "@Tong_HS_12", parameters.Tong_HS_12,
                    "@Tong_HS_KhuyetTat_Nhe", parameters.Tong_HS_KhuyetTat_Nhe,
                    "@Tong_HS_KhuyetTat_Nang", parameters.Tong_HS_KhuyetTat_Nang,
                    "@Tong_HS_KhiemThi", parameters.Tong_HS_KhiemThi,
                    "@Tong_HS_CanHoTroDacBiet", parameters.Tong_HS_CanHoTroDacBiet,
                    "@NoiDung_HoTro_HS", parameters.NoiDung_HoTro_HS.Trim(),
                    "@Active", parameters.Active,
                    "@MaTruong", parameters.MaTruong.Trim().ToUpper()
                );

                return result > 0;
            }
            catch(Exception ex)
            {
                var logEntry = new LogEntry
                {
                    Timestamp = DateTime.Now,
                    Level = "Error",
                    Component = nameof(BCThongTinDonViRepository),
                    Message = "Lỗi khi cập nhật dữ liệu",
                    Exception = ex.ToString()
                };

                _logger.LogError(ex, message: $"[{logEntry.Timestamp}] {logEntry.Level} - {logEntry.Component}: {logEntry.Message}. Exception: {logEntry.Exception}");

                return false;
            }
         
        }
    }
}
