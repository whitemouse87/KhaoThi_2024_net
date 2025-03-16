using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT_HS12_NhomMon.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT_HS12_NhomMon.Interfaces;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuThiTHPT_HS12_NhomMon
{
    public class BCThongTinNhomMonRepository : IBCThongTinNhomMonRepository
    {
        private readonly IDataAccessLayer _dataAccess;
        private readonly ILogger<BCThongTinNhomMonRepository> _logger;
        private const string TableName = "KhaoThi_5_THPT_ThongTin_ToHop";
        public BCThongTinNhomMonRepository(IDataAccessLayer dataAccess, ILogger<BCThongTinNhomMonRepository> logger)
        {
            _dataAccess = dataAccess;
            _logger = logger;
        }
        public async Task<int> CreateAsync(KhaoThi_2_THPT_NhomMonModel model)
        {
            // Kiểm tra trùng lặp trước khi tạo mới
            bool exists = await IsNhomMonExistAsync(model.MaTruong, model.MonLuaChon_1, model.MonLuaChon_2);
            if (exists)
            {
                // Có thể throw exception hoặc trả về mã lỗi tùy thuộc vào thiết kế của ứng dụng
                throw new InvalidOperationException("Tổ hợp môn này đã tồn tại cho trường này!");
                // Hoặc trả về -1 để báo hiệu lỗi: return -1;
            }

            string sql = @"INSERT INTO KhaoThi_5_THPT_ThongTin_ToHop (
                            TenNhom,
                            MonLuaChon_1,
                            MonLuaChon_2,
                            SoLuong,
                            MaTruong,
                            Lock
                        ) VALUES (
                            @TenNhom,
                            @MonLuaChon_1,
                            @MonLuaChon_2,
                            @SoLuong,
                            @MaTruong,
                            0
                        );
                        SELECT CAST(SCOPE_IDENTITY() as int)";

            return await _dataAccess.ExecuteScalarAsync<int>(sql,
                "@TenNhom", model.TenNhom,
                "@MonLuaChon_1", model.MonLuaChon_1,
                "@MonLuaChon_2", model.MonLuaChon_2,
                "@SoLuong", model.SoLuong,
                "@MaTruong", model.MaTruong
            );
        }

        public async Task<bool> DeleteAsync(int ID)
        {
            const string sql = "DELETE FROM KhaoThi_5_THPT_ThongTin_ToHop WHERE ID = @ID";
            var result = await _dataAccess.ExecuteAsync(sql, "@ID", ID);
            return result > 0;
        }

        public async Task<IEnumerable<KhaoThi_2_THPT_NhomMonModel>> GetByMaDonViAsync(string MaTruong)
        {
            string sql = @"SELECT ID,
                      TenNhom,
                      MonLuaChon_1,
                      MonLuaChon_2,
                      SoLuong,
                      MaTruong
                  FROM KhaoThi_5_THPT_ThongTin_ToHop 
                  WHERE MaTruong = @MaTruong
                  ORDER BY MaTruong ASC,TenNhom ASC,SoLuong DESC";

            var result = await _dataAccess.QueryAsync<KhaoThi_2_THPT_NhomMonModel>(
                sql,
                "@MaTruong", MaTruong
            );

            return result;
        }

        public async Task<(IEnumerable<KhaoThi_2_THPT_NhomMonModel> Items, int TotalCount)> GetPaginatedAsync(
      int page,
      int pageSize,
      string? searchTerm = null,
      string? maTruong = null,
      int? minSoLuong = 0)
        {
            var sql = "SELECT * FROM KhaoThi_5_THPT_ThongTin_ToHop WHERE 1=1";
            var parameters = new List<object>();

            // Tìm kiếm theo từ khóa
            if (!string.IsNullOrEmpty(searchTerm))
            {
                sql += " AND (TenNhom LIKE @Search OR MonLuaChon_1 LIKE @Search OR MonLuaChon_2 LIKE @Search)";
                parameters.AddRange(new object[] { "@Search", $"%{searchTerm}%" });
            }

            // Lọc theo mã trường
            if (!string.IsNullOrEmpty(maTruong))
            {
                sql += " AND MaTruong = @MaTruong";
                parameters.AddRange(new object[] { "@MaTruong", maTruong });
            }

            // Lọc theo số lượng tối thiểu
            if (minSoLuong.HasValue && minSoLuong.Value > 0)
            {
                sql += " AND SoLuong >= @MinSoLuong";
                parameters.AddRange(new object[] { "@MinSoLuong", minSoLuong.Value });
            }

            // Thêm sắp xếp để đảm bảo kết quả nhất quán
            sql += " ORDER BY TenNhom";

            return await _dataAccess.QueryPaginatedAsync<KhaoThi_2_THPT_NhomMonModel>(
                sql,
                page,
                pageSize,
                parameters.ToArray());
        }

        public async Task<bool> IsNhomMonExistAsync(string MaTruong, string MonLuaChon1, string MonLuaChon2)
        {
            string sql = @"SELECT COUNT(1) 
                  FROM KhaoThi_5_THPT_ThongTin_ToHop 
                  WHERE MaTruong = @MaTruong 
                  AND ((MonLuaChon_1 = @MonLuaChon1 AND MonLuaChon_2 = @MonLuaChon2)
                       OR (MonLuaChon_1 = @MonLuaChon2 AND MonLuaChon_2 = @MonLuaChon1))";

            int count = await _dataAccess.ExecuteScalarAsync<int>(sql,
                "@MaTruong", MaTruong,
                "@MonLuaChon1", MonLuaChon1,
                "@MonLuaChon2", MonLuaChon2
            );

            return count > 0;
        }


        public async Task<bool> UpdateAsync(KhaoThi_2_THPT_NhomMonModel NhomMon)
        {
            string updateSql = @"UPDATE KhaoThi_5_THPT_ThongTin_ToHop
                      SET SoLuong = @SoLuong
                      WHERE Id = @Id";

            int rowsAffected = await _dataAccess.ExecuteAsync(
                updateSql,
                "@Id", NhomMon.ID,
                "@SoLuong", NhomMon.SoLuong
            );

            return rowsAffected > 0;
        }
    }
}
