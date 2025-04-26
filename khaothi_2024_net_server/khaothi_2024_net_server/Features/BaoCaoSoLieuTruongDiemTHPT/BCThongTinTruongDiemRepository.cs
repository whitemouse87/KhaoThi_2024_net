using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.BaoCaoSoLieuTruongDiemTHPT.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuTruongDiemTHPT.Interfaces;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuTruongDiemTHPT
{
    public class BCThongTinTruongDiemRepository : IBCThongTinTruongDiemRepository
    {
        private readonly IDataAccessLayer _dataAccess;
        private readonly ILogger<BCThongTinTruongDiemRepository> _logger;
        private const string TableName = "KhaoThi_4_THPT_ThongTin_LanhDao";

        public BCThongTinTruongDiemRepository(
            IDataAccessLayer dataAccess,
            ILogger<BCThongTinTruongDiemRepository> logger)
        {
            _dataAccess = dataAccess;
            _logger = logger;
        }

        public async Task<bool> UpdateAsync(KhaoThi_4_THPT_ThongTin_LanhDaoModel lanhDaoDiemThi)
        {
            string sql = $@"UPDATE {TableName}
                          SET HoTen = @HoTen,
                              NamSinh = @NamSinh,
                              ChucVuDonVi = @ChucVuDonVi,
                              CoiThiTS10 = @CoiThiTS10,
                              ChucVuCoiThiTS10 = @ChucVuCoiThiTS10,
                              LyDoKhongThamGiaTS10 = @LyDoKhongThamGiaTS10,
                              CoiThiTHPT = @CoiThiTHPT,
                              ChucVuCoiThiTHPT = @ChucVuCoiThiTHPT,
                              LyDoKhongThamGiaTHPT = @LyDoKhongThamGiaTHPT
                          WHERE MaTruong = @MaTruong AND CCCD = @CCCD";

            int rowsAffected = await _dataAccess.ExecuteAsync(sql,
                "@MaTruong", lanhDaoDiemThi.MaTruong,
                "@CCCD", lanhDaoDiemThi.CCCD,
                "@HoTen", lanhDaoDiemThi.HoTen,
                "@NamSinh", lanhDaoDiemThi.NamSinh,
                "@ChucVuDonVi", lanhDaoDiemThi.ChucVuDonVi,
                "@CoiThiTS10", lanhDaoDiemThi.CoiThiTS10,
                "@ChucVuCoiThiTS10", lanhDaoDiemThi.ChucVuCoiThiTS10,
                "@LyDoKhongThamGiaTS10", lanhDaoDiemThi.LyDoKhongThamGiaTS10,
                "@CoiThiTHPT", lanhDaoDiemThi.CoiThiTHPT,
                "@ChucVuCoiThiTHPT", lanhDaoDiemThi.ChucVuCoiThiTHPT,
                "@LyDoKhongThamGiaTHPT", lanhDaoDiemThi.LyDoKhongThamGiaTHPT
            );

            return rowsAffected > 0;
        }
        public async Task<IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>> GetByMaTruongAsync(string maTruong)
        {
            string sql = $@"SELECT * FROM {TableName} 
                           WHERE MaTruong = @MaTruong
                           ORDER BY ChucVuDonVi,CoiThiTS10,ChucVuCoiThiTS10,CoiThiTHPT,ChucVuCoiThiTHPT";

            var result = await _dataAccess.QueryAsync<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(
                sql,
                "@MaTruong", maTruong
            );

            return result;
        }

        public async Task<(IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel> Items, int TotalCount)> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null)
        {
            try
            {
                // Sử dụng SELECT TOP 100 PERCENT để cho phép ORDER BY trong subquery
                var sql = $"SELECT * FROM {TableName}  WHERE 1=1";
                var parameters = new List<object>();

                // Tìm kiếm theo từ khóa
                if (!string.IsNullOrEmpty(searchTerm))
                {
                    sql += " AND (HoTen LIKE @Search OR ChucVuDonVi LIKE @Search)";
                    parameters.AddRange(new object[] { "@Search", $"%{searchTerm}%" });
                }

                // Lọc theo mã trường
                if (!string.IsNullOrEmpty(maTruong))
                {
                    sql += " AND MaTruong = @MaTruong";
                    parameters.AddRange(new object[] { "@MaTruong", maTruong });
                }

                // Log câu truy vấn để debug
                //_logger.LogInformation($"Executing SQL: {sql}");
                //_logger.LogInformation($"Parameters: {string.Join(", ", parameters.Where((p, i) => i % 2 == 0).Select(p => p.ToString()))}");

                var result = await _dataAccess.QueryPaginatedAsync<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(
                    sql,
                    page,
                    pageSize,
                    parameters.ToArray());

                //_logger.LogInformation($"Query completed. Items count: {result.Items.Count()}");
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi truy vấn dữ liệu từ bảng KhaoThi_4_THPT_ThongTin_LanhDao");
                throw;
            }
        }
    }
}