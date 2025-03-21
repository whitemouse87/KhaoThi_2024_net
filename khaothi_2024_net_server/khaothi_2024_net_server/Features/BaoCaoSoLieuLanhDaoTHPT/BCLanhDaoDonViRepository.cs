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

        public BCLanhDaoDonViRepository(
            IDataAccessLayer dataAccess,
            ILogger<BCLanhDaoDonViRepository> logger)
        {
            _dataAccess = dataAccess;
            _logger = logger;
        }

        public async Task<KhaoThi_4_THPT_ThongTin_LanhDaoModel?> GetByMaTruongAndCCCDAsync(string maTruong, string cccd)
        {
            string sql = $@"SELECT * FROM {TableName} 
                           WHERE MaTruong = @MaTruong AND CCCD = @CCCD";

            var result = await _dataAccess.QueryFirstOrDefaultAsync<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(
                sql,
                "@MaTruong", maTruong,
                "@CCCD", cccd
            );

            return result;
        }

        public async Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetByMaTruongAsync(string maTruong)
        {
            string sql = $@"SELECT * FROM {TableName} 
                           WHERE MaTruong = @MaTruong
                           ORDER BY HoTen ASC";

            var result = await _dataAccess.QueryAsync<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(
                sql,
                "@MaTruong", maTruong
            );

            return result;
        }

        public async Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetAllAsync()
        {
            string sql = $@"SELECT * FROM {TableName} ORDER BY MaTruong ASC, HoTen ASC";

            var result = await _dataAccess.QueryAsync<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(sql);

            return result;
        }

        public async Task<bool> CreateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao)
        {
            // Kiểm tra trùng lặp trước khi tạo mới
            bool exists = await ExistsAsync(lanhDao.MaTruong, lanhDao.CCCD);
            if (exists)
            {
                throw new InvalidOperationException("Thông tin lãnh đạo với CCCD này đã tồn tại cho trường này!");
            }

            string sql = $@"INSERT INTO {TableName} (
                            MaTruong,
                            CCCD,
                            HoTen,
                            NamSinh,
                            ChucVuDonVi,
                            DiaChiNha,
                            QuanNha,
                            SDTDiDong,
                            Email,
                            CoiThiTS10,
                            ChucVuCoiThiTS10,
                            LyDoKhongThamGiaTS10,
                            CoiThiTHPT,
                            ChucVuCoiThiTHPT,
                            LyDoKhongThamGiaTHPT,
                            CumChuyenMon,
                            ChucVuCumChuyenMon
                        ) VALUES (
                            @MaTruong,
                            @CCCD,
                            @HoTen,
                            @NamSinh,
                            @ChucVuDonVi,
                            @DiaChiNha,
                            @QuanNha,
                            @SDTDiDong,
                            @Email,
                            @CoiThiTS10,
                            @ChucVuCoiThiTS10,
                            @LyDoKhongThamGiaTS10,
                            @CoiThiTHPT,
                            @ChucVuCoiThiTHPT,
                            @LyDoKhongThamGiaTHPT,
                            @CumChuyenMon,
                            @ChucVuCumChuyenMon
                        )";

            int rowsAffected = await _dataAccess.ExecuteAsync(sql,
                "@MaTruong", lanhDao.MaTruong,
                "@CCCD", lanhDao.CCCD,
                "@HoTen", lanhDao.HoTen,
                "@NamSinh", lanhDao.NamSinh,
                "@ChucVuDonVi", lanhDao.ChucVuDonVi,
                "@DiaChiNha", lanhDao.DiaChiNha,
                "@QuanNha", lanhDao.QuanNha,
                "@SDTDiDong", lanhDao.SDTDiDong,
                "@Email", lanhDao.Email,
                "@CoiThiTS10", lanhDao.CoiThiTS10,
                "@ChucVuCoiThiTS10", lanhDao.ChucVuCoiThiTS10,
                "@LyDoKhongThamGiaTS10", lanhDao.LyDoKhongThamGiaTS10,
                "@CoiThiTHPT", lanhDao.CoiThiTHPT,
                "@ChucVuCoiThiTHPT", lanhDao.ChucVuCoiThiTHPT,
                "@LyDoKhongThamGiaTHPT", lanhDao.LyDoKhongThamGiaTHPT,
                "@CumChuyenMon", lanhDao.CumChuyenMon,
                "@ChucVuCumChuyenMon", lanhDao.ChucVuCumChuyenMon
            );

            return rowsAffected > 0;
        }

        public async Task<bool> UpdateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDao)
        {
            string sql = $@"UPDATE {TableName}
                          SET HoTen = @HoTen,
                              NamSinh = @NamSinh,
                              ChucVuDonVi = @ChucVuDonVi,
                              DiaChiNha = @DiaChiNha,
                              QuanNha = @QuanNha,
                              SDTDiDong = @SDTDiDong,
                              Email = @Email,
                              CoiThiTS10 = @CoiThiTS10,
                              ChucVuCoiThiTS10 = @ChucVuCoiThiTS10,
                              LyDoKhongThamGiaTS10 = @LyDoKhongThamGiaTS10,
                              CoiThiTHPT = @CoiThiTHPT,
                              ChucVuCoiThiTHPT = @ChucVuCoiThiTHPT,
                              LyDoKhongThamGiaTHPT = @LyDoKhongThamGiaTHPT,
                              CumChuyenMon = @CumChuyenMon,
                              ChucVuCumChuyenMon = @ChucVuCumChuyenMon
                          WHERE MaTruong = @MaTruong AND CCCD = @CCCD";

            int rowsAffected = await _dataAccess.ExecuteAsync(sql,
                "@MaTruong", lanhDao.MaTruong,
                "@CCCD", lanhDao.CCCD,
                "@HoTen", lanhDao.HoTen,
                "@NamSinh", lanhDao.NamSinh,
                "@ChucVuDonVi", lanhDao.ChucVuDonVi,
                "@DiaChiNha", lanhDao.DiaChiNha,
                "@QuanNha", lanhDao.QuanNha,
                "@SDTDiDong", lanhDao.SDTDiDong,
                "@Email", lanhDao.Email,
                "@CoiThiTS10", lanhDao.CoiThiTS10,
                "@ChucVuCoiThiTS10", lanhDao.ChucVuCoiThiTS10,
                "@LyDoKhongThamGiaTS10", lanhDao.LyDoKhongThamGiaTS10,
                "@CoiThiTHPT", lanhDao.CoiThiTHPT,
                "@ChucVuCoiThiTHPT", lanhDao.ChucVuCoiThiTHPT,
                "@LyDoKhongThamGiaTHPT", lanhDao.LyDoKhongThamGiaTHPT,
                "@CumChuyenMon", lanhDao.CumChuyenMon,
                "@ChucVuCumChuyenMon", lanhDao.ChucVuCumChuyenMon
            );

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(string maTruong, string cccd)
        {
            string sql = $"DELETE FROM {TableName} WHERE MaTruong = @MaTruong AND CCCD = @CCCD";
            int rowsAffected = await _dataAccess.ExecuteAsync(sql, "@MaTruong", maTruong, "@CCCD", cccd);
            return rowsAffected > 0;
        }

        public async Task<bool> ExistsAsync(string maTruong, string cccd)
        {
            string sql = $"SELECT COUNT(1) FROM {TableName} WHERE MaTruong = @MaTruong AND CCCD = @CCCD";
            int count = await _dataAccess.ExecuteScalarAsync<int>(sql, "@MaTruong", maTruong, "@CCCD", cccd);
            return count > 0;
        }

        public async Task<(IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel> Items, int TotalCount)> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null,
            int? namSinh = null,
            string? cccd = null)
        {
            var sql = $"SELECT * FROM {TableName} WHERE 1=1";
            var parameters = new List<object>();

            // Tìm kiếm theo từ khóa
            if (!string.IsNullOrEmpty(searchTerm))
            {
                sql += " AND (HoTen LIKE @Search OR ChucVuDonVi LIKE @Search OR Email LIKE @Search)";
                parameters.AddRange(new object[] { "@Search", $"%{searchTerm}%" });
            }

            // Lọc theo mã trường
            if (!string.IsNullOrEmpty(maTruong))
            {
                sql += " AND MaTruong = @MaTruong";
                parameters.AddRange(new object[] { "@MaTruong", maTruong });
            }

            // Lọc theo năm sinh
            if (namSinh.HasValue && namSinh.Value > 0)
            {
                sql += " AND NamSinh = @NamSinh";
                parameters.AddRange(new object[] { "@NamSinh", namSinh.Value });
            }

            // Lọc theo CCCD
            if (!string.IsNullOrEmpty(cccd))
            {
                sql += " AND CCCD LIKE @CCCD";
                parameters.AddRange(new object[] { "@CCCD", $"%{cccd}%" });
            }

            // Thêm sắp xếp để đảm bảo kết quả nhất quán
            sql += " ORDER BY MaTruong ASC, HoTen ASC";

            return await _dataAccess.QueryPaginatedAsync<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(
                sql,
                page,
                pageSize,
                parameters.ToArray());
        }
    }
}