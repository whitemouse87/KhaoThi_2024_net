using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.BaoCaoSoLieuLanhDaoTHPT.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuLanhDaoTHPT.Interfaces;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuLanhDaoTHPT
{
    public class BCLanhDaoDonViRepository : IBCLanhDaoDonViRepository
    {
        private readonly IDataAccessLayer _dataAccess;
        private readonly ILogger<BCLanhDaoDonViRepository> _logger;
        private const string TableName = "KhaoThi_4_THPT_ThongTin_LanhDao";

        public BCLanhDaoDonViRepository(IDataAccessLayer dataAccess, ILogger<BCLanhDaoDonViRepository> logger)
        {
            _dataAccess = dataAccess;
            _logger = logger;
        }

        public async Task<KhaoThi_4_THPT_ThongTin_LanhDaoModel?> GetByMaTruongAndCCCDAsync(string maTruong, string cccd)
        {
            try
            {
                const string sql = @"SELECT * FROM KhaoThi_4_THPT_ThongTin_LanhDao WHERE MaTruong = @MaTruong AND CCCD = @CCCD";
                return await _dataAccess.QueryFirstOrDefaultAsync<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(sql, new { MaTruong = maTruong, CCCD = cccd });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin lãnh đạo theo mã trường và CCCD: {MaTruong}, {CCCD}", maTruong, cccd);
                return null;
            }
        }

        public async Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetByMaTruongAsync(string maTruong)
        {
            try
            {
                const string sql = @"SELECT * FROM KhaoThi_4_THPT_ThongTin_LanhDao WHERE MaTruong = @MaTruong";
                return await _dataAccess.QueryAsync<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(sql, new { MaTruong = maTruong });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin lãnh đạo theo mã trường: {MaTruong}", maTruong);
                return new List<KhaoThi_4_THPT_ThongTin_LanhDaoModel>();
            }
        }

        public async Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetAllAsync()
        {
            try
            {
                const string sql = @"SELECT * FROM KhaoThi_4_THPT_ThongTin_LanhDao";
                return await _dataAccess.QueryAsync<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(sql);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tất cả thông tin lãnh đạo");
                return new List<KhaoThi_4_THPT_ThongTin_LanhDaoModel>();
            }
        }

        public async Task<bool> CreateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao)
        {
            try
            {
                const string sql = @"
                    INSERT INTO KhaoThi_4_THPT_ThongTin_LanhDao (MaTruong, CCCD, HoTen, NamSinh, ChucVuDonVi, DiaChiNha, QuanNha, SDTDiDong, Email, CoiThiTS10, ChucVuCoiThiTS10, LyDoKhongThamGiaTS10, CoiThiTHPT, ChucVuCoiThiTHPT, LyDoKhongThamGiaTHPT, CumChuyenMon, ChucVuCumChuyenMon)
                    VALUES (@MaTruong, @CCCD, @HoTen, @NamSinh, @ChucVuDonVi, @DiaChiNha, @QuanNha, @SDTDiDong, @Email, @CoiThiTS10, @ChucVuCoiThiTS10, @LyDoKhongThamGiaTS10, @CoiThiTHPT, @ChucVuCoiThiTHPT, @LyDoKhongThamGiaTHPT, @CumChuyenMon, @ChucVuCumChuyenMon)";

                int rowsAffected = await _dataAccess.ExecuteAsync(sql, lanhDao);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo thông tin lãnh đạo: {MaTruong}, {CCCD}", lanhDao.MaTruong, lanhDao.CCCD);
                return false;
            }
        }

        public async Task<bool> UpdateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao)
        {
            try
            {
                const string sql = @"
                    UPDATE KhaoThi_4_THPT_ThongTin_LanhDao 
                    SET HoTen = @HoTen, NamSinh = @NamSinh, ChucVuDonVi = @ChucVuDonVi, DiaChiNha = @DiaChiNha, QuanNha = @QuanNha, 
                        SDTDiDong = @SDTDiDong, Email = @Email, CoiThiTS10 = @CoiThiTS10, ChucVuCoiThiTS10 = @ChucVuCoiThiTS10, 
                        LyDoKhongThamGiaTS10 = @LyDoKhongThamGiaTS10, CoiThiTHPT = @CoiThiTHPT, ChucVuCoiThiTHPT = @ChucVuCoiThiTHPT, 
                        LyDoKhongThamGiaTHPT = @LyDoKhongThamGiaTHPT, CumChuyenMon = @CumChuyenMon, ChucVuCumChuyenMon = @ChucVuCumChuyenMon
                    WHERE MaTruong = @MaTruong AND CCCD = @CCCD";

                int rowsAffected = await _dataAccess.ExecuteAsync(sql, lanhDao);
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật thông tin lãnh đạo: {MaTruong}, {CCCD}", lanhDao.MaTruong, lanhDao.CCCD);
                return false;
            }
        }

        public async Task<bool> DeleteAsync(string maTruong, string cccd)
        {
            try
            {
                const string sql = @"DELETE FROM KhaoThi_4_THPT_ThongTin_LanhDao WHERE MaTruong = @MaTruong AND CCCD = @CCCD";
                int rowsAffected = await _dataAccess.ExecuteAsync(sql, new { MaTruong = maTruong, CCCD = cccd });
                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa thông tin lãnh đạo: {MaTruong}, {CCCD}", maTruong, cccd);
                return false;
            }
        }

        public async Task<bool> ExistsAsync(string maTruong, string cccd)
        {
            try
            {
                const string sql = @"SELECT COUNT(1) FROM KhaoThi_4_THPT_ThongTin_LanhDao WHERE MaTruong = @MaTruong AND CCCD = @CCCD";
                int count = await _dataAccess.ExecuteScalarAsync<int>(sql, new { MaTruong = maTruong, CCCD = cccd });
                return count > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi kiểm tra sự tồn tại của thông tin lãnh đạo: {MaTruong}, {CCCD}", maTruong, cccd);
                return false;
            }
        }

        public async Task<(IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel> Items, int TotalCount)> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null,
            int? namSinh = null,
            string? cccd = null
        )
        {
            try
            {
                var sql = @"SELECT * FROM KhaoThi_4_THPT_ThongTin_LanhDao WHERE 1=1";
                var parameters = new List<object>();
                var parameterNames = new List<string>();

                // Tìm kiếm tổng quát (HoTen, ChucVuDonVi, Email)
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    sql += " AND (HoTen LIKE @Search OR ChucVuDonVi LIKE @Search OR Email LIKE @Search)";
                    parameters.Add($"%{searchTerm}%");
                    parameterNames.Add("@Search");
                }

                // Lọc theo mã trường
                if (!string.IsNullOrEmpty(maTruong))
                {
                    sql += " AND MaTruong = @MaTruong";
                    parameters.Add(maTruong);
                    parameterNames.Add("@MaTruong");
                }

                // Lọc theo năm sinh
                if (namSinh.HasValue)
                {
                    sql += " AND NamSinh = @NamSinh";
                    parameters.Add(namSinh.Value);
                    parameterNames.Add("@NamSinh");
                }

                // Lọc theo CCCD
                if (!string.IsNullOrEmpty(cccd))
                {
                    sql += " AND CCCD = @CCCD";
                    parameters.Add(cccd);
                    parameterNames.Add("@CCCD");
                }

                // Sắp xếp mặc định để đảm bảo kết quả nhất quán
                sql += " ORDER BY HoTen";

                // Convert parameters to array
                object[] paramArray = parameters.ToArray();

                // Convert parameter names to array
                string[] paramNameArray = parameterNames.ToArray();

                return await _dataAccess.QueryPaginatedAsync<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(
                    sql,
                    page,
                    pageSize,
                    paramNameArray, // Pass the parameter names
                    paramArray      // Pass the parameter values
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách lãnh đạo phân trang và tìm kiếm");
                return (new List<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(), 0);
            }
        }
    }
}
