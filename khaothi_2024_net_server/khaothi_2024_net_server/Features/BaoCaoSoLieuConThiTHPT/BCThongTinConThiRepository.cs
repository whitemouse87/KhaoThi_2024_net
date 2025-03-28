using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT.DTOs;
using khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT.Interfaces;

namespace khaothi_2024_net_server.Features.BaoCaoSoLieuConThiTHPT
{
    public class BCThongTinConThiRepository : IBCThongTinConThiRepository
    {
        private readonly IDataAccessLayer _dataAccess;
        private readonly ILogger<BCThongTinConThiRepository> _logger;
        private const string TableName = "KhaoThi_2_THPT_ThongTin_ConThi";

        public BCThongTinConThiRepository(
            IDataAccessLayer dataAccess,
            ILogger<BCThongTinConThiRepository> logger)
        {
            _dataAccess = dataAccess;
            _logger = logger;
        }

        public async Task<bool> CreateAsync(KhaoThi_5_THPT_ThongTin_ConThiModel conThi)
        {
            // Kiểm tra trùng lặp trước khi tạo mới
            bool exists = await IsConThiExistAsync(conThi.MaTruong, conThi.CCCD, conThi.MaDinhDanhCuaCon);
            if (exists)
            {
                throw new InvalidOperationException("Thông tin con thi này đã tồn tại cho người/trường này!");
            }

            string sql = $@"INSERT INTO {TableName} (
                            MaTruong,
                            CCCD,
                            HoTen,
                            ChucVuDonVi,
                            MaDinhDanhCuaCon,
                            HoTenCon,
                            ChucVuGiaDinh,
                            TenTruongDangHoc,
                            SDTDiDong,
                            KyThiThamDu
                        ) VALUES (
                            @MaTruong,
                            @CCCD,
                            @HoTen,
                            @ChucVuDonVi,
                            @MaDinhDanhCuaCon,
                            @HoTenCon,
                            @ChucVuGiaDinh,
                            @TenTruongDangHoc,
                            @SDTDiDong,
                            @KyThiThamDu
                        );
                        SELECT CAST(SCOPE_IDENTITY() as int)";



            int rowsAffected = await _dataAccess.ExecuteAsync(sql,
                "@MaTruong", conThi.MaTruong,
                "@CCCD", conThi.CCCD,
                "@HoTen", conThi.HoTen,
                "@ChucVuDonVi", conThi.ChucVuDonVi,
                "@MaDinhDanhCuaCon", conThi.MaDinhDanhCuaCon,
                "@HoTenCon", conThi.HoTenCon,
                "@ChucVuGiaDinh", conThi.ChucVuGiaDinh,
                "@TenTruongDangHoc", conThi.TenTruongDangHoc,
                "@SDTDiDong", conThi.SDTDiDong,
                "@KyThiThamDu", conThi.KyThiThamDu
           );

            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(string maTruong, string cccd, string maDinhDanhCuaCon)
        {
            string sql = $"DELETE FROM {TableName} WHERE MaTruong = @MaTruong AND CCCD = @CCCD AND MaDinhDanhCuaCon = @MaDinhDanhCuaCon";
            var result = await _dataAccess.ExecuteAsync(sql,
                "@MaTruong", maTruong,
                "@CCCD", cccd,
                "@MaDinhDanhCuaCon", maDinhDanhCuaCon
            );
            return result > 0;
        }

        public async Task<IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel>> GetByMaTruongAsync(string maTruong)
        {
            string sql = $@"SELECT MaTruong,
                      CCCD,
                      HoTen,
                      ChucVuDonVi,
                      MaDinhDanhCuaCon,
                      HoTenCon,
                      ChucVuGiaDinh,
                      TenTruongDangHoc,
                      SDTDiDong,
                      KyThiThamDu
                  FROM {TableName} 
                  WHERE MaTruong = @MaTruong
                  ORDER BY HoTen ASC, HoTenCon ASC";

            var result = await _dataAccess.QueryAsync<KhaoThi_5_THPT_ThongTin_ConThiModel>(
                sql,
                "@MaTruong", maTruong
            );

            return result;
        }

        public async Task<KhaoThi_5_THPT_ThongTin_ConThiModel?> GetThongTinCaNhan(string maTruong, string cccd, string maDinhDanhCuaCon)
        {
            string sql = $@"SELECT MaTruong,
                      CCCD,
                      HoTen,
                      ChucVuDonVi,
                      MaDinhDanhCuaCon,
                      HoTenCon,
                      ChucVuGiaDinh,
                      TenTruongDangHoc,
                      SDTDiDong,
                      KyThiThamDu
                  FROM {TableName} 
                  WHERE MaTruong = @MaTruong 
                    AND CCCD = @CCCD 
                    AND MaDinhDanhCuaCon = @MaDinhDanhCuaCon";

            var result = await _dataAccess.QueryFirstOrDefaultAsync<KhaoThi_5_THPT_ThongTin_ConThiModel>(
                sql,
                "@MaTruong", maTruong,
                "@CCCD", cccd,
                "@MaDinhDanhCuaCon", maDinhDanhCuaCon
            );

            return result;
        }

        public async Task<(IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel> Items, int TotalCount)> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null,
            string? maTruong = null,
            string? kyThiThamDu = null)
        {
            var sql = $"SELECT * FROM {TableName} WHERE 1=1";
            var parameters = new List<object>();

            // Tìm kiếm theo từ khóa
            if (!string.IsNullOrEmpty(searchTerm))
            {
                sql += " AND (HoTen LIKE @Search OR HoTenCon LIKE @Search OR ChucVuDonVi LIKE @Search OR ChucVuGiaDinh LIKE @Search)";
                parameters.AddRange(new object[] { "@Search", $"%{searchTerm}%" });
            }

            // Lọc theo mã trường
            if (!string.IsNullOrEmpty(maTruong))
            {
                sql += " AND MaTruong = @MaTruong";
                parameters.AddRange(new object[] { "@MaTruong", maTruong });
            }

            // Lọc theo kỳ thi tham dự
            if (!string.IsNullOrEmpty(kyThiThamDu))
            {
                sql += " AND KyThiThamDu = @KyThiThamDu";
                parameters.AddRange(new object[] { "@KyThiThamDu", kyThiThamDu });
            }

            // Thêm sắp xếp để đảm bảo kết quả nhất quán
            sql += " ORDER BY HoTen, HoTenCon";

            return await _dataAccess.QueryPaginatedAsync<KhaoThi_5_THPT_ThongTin_ConThiModel>(
                sql,
                page,
                pageSize,
                parameters.ToArray());
        }

        public async Task<bool> IsConThiExistAsync(string maTruong, string cccd, string maDinhDanhCuaCon)
        {
            string sql = $@"SELECT COUNT(1) 
                  FROM {TableName} 
                  WHERE MaTruong = @MaTruong 
                  AND CCCD = @CCCD
                  AND MaDinhDanhCuaCon = @MaDinhDanhCuaCon";

            int count = await _dataAccess.ExecuteScalarAsync<int>(sql,
                "@MaTruong", maTruong,
                "@CCCD", cccd,
                "@MaDinhDanhCuaCon", maDinhDanhCuaCon
            );

            return count > 0;
        }

        public async Task<bool> UpdateAsync(KhaoThi_5_THPT_ThongTin_ConThiModel conThi)
        {
            string updateSql = $@"UPDATE {TableName}
                      SET HoTen = @HoTen,
                          ChucVuDonVi = @ChucVuDonVi,
                          HoTenCon = @HoTenCon,
                          ChucVuGiaDinh = @ChucVuGiaDinh,
                          TenTruongDangHoc = @TenTruongDangHoc,
                          SDTDiDong = @SDTDiDong,
                          KyThiThamDu = @KyThiThamDu
                      WHERE MaTruong = @MaTruong 
                        AND CCCD = @CCCD
                        AND MaDinhDanhCuaCon = @MaDinhDanhCuaCon";

            int rowsAffected = await _dataAccess.ExecuteAsync(
                updateSql,
                "@MaTruong", conThi.MaTruong,
                "@CCCD", conThi.CCCD,
                "@MaDinhDanhCuaCon", conThi.MaDinhDanhCuaCon,
                "@HoTen", conThi.HoTen,
                "@ChucVuDonVi", conThi.ChucVuDonVi,
                "@HoTenCon", conThi.HoTenCon,
                "@ChucVuGiaDinh", conThi.ChucVuGiaDinh,
                "@TenTruongDangHoc", conThi.TenTruongDangHoc,
                "@SDTDiDong", conThi.SDTDiDong,
                "@KyThiThamDu", conThi.KyThiThamDu
            );

            return rowsAffected > 0;
        }
    }
}