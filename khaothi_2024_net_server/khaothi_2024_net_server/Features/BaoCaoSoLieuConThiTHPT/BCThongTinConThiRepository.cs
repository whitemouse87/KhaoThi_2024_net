using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT.Interfaces;
using khaothi_2024_net_server.Features.Logging.DTOs;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT
{
    public class BCThongTinConThiRepository : IBCThongTinConThiRepository
    {
        private readonly IDataAccessLayer _dataAccess;
        private readonly ILogger<BCThongTinConThiRepository> _logger;
        private const string TableName = "KhaoThi_2_THPT_ThongTin_ConThi"; // Tên bảng trong cơ sở dữ liệu

        public BCThongTinConThiRepository(IDataAccessLayer dataAccess, ILogger<BCThongTinConThiRepository> logger)
        {
            _dataAccess = dataAccess;
            _logger = logger;
        }

        public async Task<bool> InsertAsync(KhaoThi_5_THPT_ThongTin_ConThi ThongTin)
        {
            try
            {
                const string sql = $@"
                    INSERT INTO {TableName} (MaTruong, CCCD, HoTen, ChucVuDonVi, MaDinhDanhCuaCon, HoTenCon, ChucVuGiaDinh, TenTruongDangHoc, SDTDiDong, KyThiThamDu)
                    VALUES (@MaTruong, @CCCD, @HoTen, @ChucVuDonVi, @MaDinhDanhCuaCon, @HoTenCon, @ChucVuGiaDinh, @TenTruongDangHoc, @SDTDiDong, @KyThiThamDu)";

                var parameters = new
                {
                    MaTruong = ThongTin.MaTruong,
                    CCCD = ThongTin.CCCD,
                    HoTen = ThongTin.HoTen,
                    ChucVuDonVi = ThongTin.ChucVuDonVi,
                    MaDinhDanhCuaCon = ThongTin.MaDinhDanhCuaCon,
                    HoTenCon = ThongTin.HoTenCon,
                    ChucVuGiaDinh = ThongTin.ChucVuGiaDinh,
                    TenTruongDangHoc = ThongTin.TenTruongDangHoc,
                    SDTDiDong = ThongTin.SDTDiDong,
                    KyThiThamDu = ThongTin.KyThiThamDu
                };

                int rowsAffected = await _dataAccess.ExecuteAsync(sql, parameters);
                return rowsAffected > 0; // Trả về true nếu có ít nhất một hàng bị ảnh hưởng
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm dữ liệu vào bảng {TableName}", TableName);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(KhaoThi_5_THPT_ThongTin_ConThi ThongTin)
        {
            try
            {
                const string sql = $@"
                    UPDATE {TableName}
                    SET HoTen = @HoTen,
                        ChucVuDonVi = @ChucVuDonVi,
                        HoTenCon = @HoTenCon,
                        ChucVuGiaDinh = @ChucVuGiaDinh,
                        TenTruongDangHoc = @TenTruongDangHoc,
                        SDTDiDong = @SDTDiDong,
                        KyThiThamDu = @KyThiThamDu
                    WHERE MaTruong = @MaTruong AND CCCD = @CCCD AND MaDinhDanhCuaCon = @MaDinhDanhCuaCon";

                var parameters = new
                {
                    MaTruong = ThongTin.MaTruong,
                    CCCD = ThongTin.CCCD,
                    HoTen = ThongTin.HoTen,
                    ChucVuDonVi = ThongTin.ChucVuDonVi,
                    MaDinhDanhCuaCon = ThongTin.MaDinhDanhCuaCon,
                    HoTenCon = ThongTin.HoTenCon,
                    ChucVuGiaDinh = ThongTin.ChucVuGiaDinh,
                    TenTruongDangHoc = ThongTin.TenTruongDangHoc,
                    SDTDiDong = ThongTin.SDTDiDong,
                    KyThiThamDu = ThongTin.KyThiThamDu
                };

                int rowsAffected = await _dataAccess.ExecuteAsync(sql, parameters);
                return rowsAffected > 0; // Trả về true nếu có ít nhất một hàng bị ảnh hưởng
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật dữ liệu trong bảng {TableName}", TableName);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string MaTruong, string CCCD, string MaDinhDanhCuaCon)
        {
            try
            {
                const string sql = $@"
                    DELETE FROM {TableName}
                    WHERE MaTruong = @MaTruong AND CCCD = @CCCD AND MaDinhDanhCuaCon = @MaDinhDanhCuaCon";

                var parameters = new
                {
                    MaTruong = MaTruong,
                    CCCD = CCCD,
                    MaDinhDanhCuaCon = MaDinhDanhCuaCon
                };

                int rowsAffected = await _dataAccess.ExecuteAsync(sql, parameters);
                return rowsAffected > 0; // Trả về true nếu có ít nhất một hàng bị ảnh hưởng
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa dữ liệu khỏi bảng {TableName}", TableName);
                return false;
            }
        }

        public async Task<(IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi> Items, int TotalCount)> GetPaginatedAsync(int page, int pageSize, string? searchTerm = null)
        {
            try
            {
                var sql = $@"SELECT * FROM {TableName} WHERE 1=1"; // Lấy tất cả các bản ghi từ bảng
                var parameters = new List<object>();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    sql += " AND (HoTen LIKE @Search OR HoTenCon LIKE @Search)"; // Thêm điều kiện tìm kiếm nếu có
                    parameters.Add(new { Name = "@Search", Value = $"%{searchTerm}%" });
                }

                return await _dataAccess.QueryPaginatedAsync<KhaoThi_5_THPT_ThongTin_ConThi>(sql, page, pageSize, parameters.ToArray());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy dữ liệu phân trang từ bảng {TableName}", TableName);
                return (new List<KhaoThi_5_THPT_ThongTin_ConThi>(), 0); // Trả về danh sách rỗng và tổng số là 0 nếu có lỗi
            }
        }

        public async Task<KhaoThi_5_THPT_ThongTin_ConThi?> GetByKeysAsync(string MaTruong, string CCCD, string MaDinhDanhCuaCon)
        {
            try
            {
                const string sql = $@"
                    SELECT * FROM {TableName}
                    WHERE MaTruong = @MaTruong AND CCCD = @CCCD AND MaDinhDanhCuaCon = @MaDinhDanhCuaCon";

                var parameters = new
                {
                    MaTruong = MaTruong,
                    CCCD = CCCD,
                    MaDinhDanhCuaCon = MaDinhDanhCuaCon
                };

                return await _dataAccess.QueryFirstOrDefaultAsync<KhaoThi_5_THPT_ThongTin_ConThi>(sql, parameters);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy dữ liệu theo khóa từ bảng {TableName}", TableName);
                return null; // Trả về null nếu có lỗi hoặc không tìm thấy bản ghi
            }
        }

        public async Task<IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi>> GetAllAsync()
        {
            try
            {
                const string sql = $"SELECT * FROM {TableName}";
                return await _dataAccess.QueryAsync<KhaoThi_5_THPT_ThongTin_ConThi>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tất cả dữ liệu từ bảng {TableName}", TableName);
                return new List<KhaoThi_5_THPT_ThongTin_ConThi>(); // Trả về danh sách rỗng nếu có lỗi
            }
        }

        public async Task<bool> ExistsAsync(string MaTruong, string CCCD, string MaDinhDanhCuaCon)
        {
            try
            {
                const string sql = $@"
                    SELECT COUNT(1)
                    FROM {TableName}
                    WHERE MaTruong = @MaTruong AND CCCD = @CCCD AND MaDinhDanhCuaCon = @MaDinhDanhCuaCon";

                var parameters = new
                {
                    MaTruong = MaTruong,
                    CCCD = CCCD,
                    MaDinhDanhCuaCon = MaDinhDanhCuaCon
                };

                int count = await _dataAccess.ExecuteScalarAsync<int>(sql, parameters);
                return count > 0; // Trả về true nếu có ít nhất một bản ghi thỏa mãn điều kiện
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kiểm tra sự tồn tại của thông tin con thí sinh trong bảng {TableName}", TableName);
                return false; // Trả về false nếu có lỗi
            }
        }
    }
}