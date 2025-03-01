using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.UserManagement.DTOs;

namespace khaothi_2024_net_server.Infrastructure.Repositories
{
    public class KhaoThiUserRepository : IKhaoThiUserRepository
    {
        private readonly IDataAccessLayer _dataAccess;
        private readonly ILogger<KhaoThiUserRepository> _logger;
        private const string TableName = "KhaoThi_User";

        public KhaoThiUserRepository(IDataAccessLayer dataAccess, ILogger<KhaoThiUserRepository> logger)
        {
            _dataAccess = dataAccess;
            _logger = logger;
        }

        public async Task<KhaoThiUser> GetByIdAsync(int id)
        {
            const string sql = @"SELECT * FROM KhaoThi_User WHERE ID = @Id";
            return await _dataAccess.QueryFirstOrDefaultAsync<KhaoThiUser>(sql, "@Id", id);
        }

        public async Task<KhaoThiUser> GetByUsernameAsync(string username)
        {
            const string sql = @"SELECT * FROM KhaoThi_User WHERE TenDangNhap = @TenDangNhap";
            return await _dataAccess.QueryFirstOrDefaultAsync<KhaoThiUser>(sql, "@TenDangNhap", username);
        }

        public async Task<IEnumerable<KhaoThiUser>> GetAllAsync()
        {
            const string sql = @"SELECT * FROM KhaoThi_User ORDER BY ID DESC";
            return await _dataAccess.QueryAsync<KhaoThiUser>(sql);
        }

        public async Task<(IEnumerable<KhaoThiUser> Items, int TotalCount)> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null)
        {
            var sql = "SELECT * FROM KhaoThi_User WHERE 1=1";
            var parameters = new List<object>();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                sql += " AND (TenDangNhap LIKE @Search OR HoTen LIKE @Search)";
                parameters.AddRange(new[] { "@Search", $"%{searchTerm}%" });
            }

            return await _dataAccess.QueryPaginatedAsync<KhaoThiUser>(
                sql,
                page,
                pageSize,
                parameters.ToArray());
        }

        public async Task<int> CreateAsync(KhaoThiUser user)
        {
            const string sql = @"
            INSERT INTO KhaoThi_User (
                TenDangNhap, MatKhau, HoTen, NgaySinh, 
                MaDonVi, TenDonVi, NgayTao, MaChucVu, 
                Email, SDT, SoTaiKhoan, TenNganHang, 
                DiaChi, MaSoThue, Active, RefreshToken, 
                RefreshTokenExpiryTime
            ) VALUES (
                @TenDangNhap, @MatKhau, @HoTen, @NgaySinh,
                @MaDonVi, @TenDonVi, @NgayTao, @MaChucVu,
                @Email, @SDT, @SoTaiKhoan, @TenNganHang,
                @DiaChi, @MaSoThue, @Active, @RefreshToken,
                @RefreshTokenExpiryTime
            );
            SELECT CAST(SCOPE_IDENTITY() as int)";

            // Xử lý giá trị mặc định và null
            user.NgayTao = DateTime.Now;
            var parameters = new
            {
                TenDangNhap = user.TenDangNhap ?? string.Empty,
                MatKhau = user.MatKhau ?? string.Empty,
                HoTen = user.HoTen ?? string.Empty,
                NgaySinh = user.NgaySinh ?? (object)DBNull.Value,
                MaDonVi = user.MaDonVi ?? string.Empty,
                TenDonVi = user.TenDonVi ?? string.Empty,
                NgayTao = user.NgayTao,
                MaChucVu = user.MaChucVu ?? string.Empty,
                Email = user.Email ?? string.Empty,
                SDT = user.SDT ?? string.Empty,
                SoTaiKhoan = user.SoTaiKhoan ?? string.Empty,
                TenNganHang = user.TenNganHang ?? string.Empty,
                DiaChi = user.DiaChi ?? string.Empty,
                MaSoThue = user.MaSoThue ?? string.Empty,
                Active = user.Active,
                RefreshToken = user.RefreshToken ?? (object)DBNull.Value,
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime ?? (object)DBNull.Value
            };

            return await _dataAccess.ExecuteScalarAsync<int>(sql,
                "@TenDangNhap", parameters.TenDangNhap,
                "@MatKhau", parameters.MatKhau,
                "@HoTen", parameters.HoTen,
                "@NgaySinh", parameters.NgaySinh,
                "@MaDonVi", parameters.MaDonVi,
                "@TenDonVi", parameters.TenDonVi,
                "@NgayTao", parameters.NgayTao,
                "@MaChucVu", parameters.MaChucVu,
                "@Email", parameters.Email,
                "@SDT", parameters.SDT,
                "@SoTaiKhoan", parameters.SoTaiKhoan,
                "@TenNganHang", parameters.TenNganHang,
                "@DiaChi", parameters.DiaChi,
                "@MaSoThue", parameters.MaSoThue,
                "@Active", parameters.Active,
                "@RefreshToken", parameters.RefreshToken,
                "@RefreshTokenExpiryTime", parameters.RefreshTokenExpiryTime
            );
        }

        public async Task<bool> UpdateAsync(KhaoThiUser user)
        {
            const string sql = @"
            UPDATE KhaoThi_User 
            SET TenDangNhap = @TenDangNhap,
                MatKhau = @MatKhau,
                HoTen = @HoTen,
                NgaySinh = @NgaySinh,
                MaDonVi = @MaDonVi,
                TenDonVi = @TenDonVi,
                MaChucVu = @MaChucVu,
                Email = @Email,
                SDT = @SDT,
                SoTaiKhoan = @SoTaiKhoan,
                TenNganHang = @TenNganHang,
                DiaChi = @DiaChi,
                MaSoThue = @MaSoThue,
                Active = @Active,
                RefreshToken = @RefreshToken,
                RefreshTokenExpiryTime = @RefreshTokenExpiryTime
            WHERE ID = @Id";

            // Xử lý các giá trị null
            var parameters = new
            {
                Id = user.ID,
                TenDangNhap = user.TenDangNhap ?? string.Empty,
                MatKhau = user.MatKhau ?? string.Empty,
                HoTen = user.HoTen ?? string.Empty,
                NgaySinh = user.NgaySinh ?? (object)DBNull.Value,
                MaDonVi = user.MaDonVi ?? string.Empty,
                TenDonVi = user.TenDonVi ?? string.Empty,
                MaChucVu = user.MaChucVu ?? string.Empty,
                Email = user.Email ?? string.Empty,
                SDT = user.SDT ?? string.Empty,
                SoTaiKhoan = user.SoTaiKhoan ?? string.Empty,
                TenNganHang = user.TenNganHang ?? string.Empty,
                DiaChi = user.DiaChi ?? string.Empty,
                MaSoThue = user.MaSoThue ?? string.Empty,
                Active = user.Active,
                RefreshToken = user.RefreshToken ?? (object)DBNull.Value, // Xử lý giá trị null
                RefreshTokenExpiryTime = user.RefreshTokenExpiryTime ?? (object)DBNull.Value, // Xử lý giá trị null
            };

            var result = await _dataAccess.ExecuteAsync(sql,
                "@Id", parameters.Id,
                "@TenDangNhap", parameters.TenDangNhap,
                "@MatKhau", parameters.MatKhau,
                "@HoTen", parameters.HoTen,
                "@NgaySinh", parameters.NgaySinh,
                "@MaDonVi", parameters.MaDonVi,
                "@TenDonVi", parameters.TenDonVi,
                "@MaChucVu", parameters.MaChucVu,
                "@Email", parameters.Email,
                "@SDT", parameters.SDT,
                "@SoTaiKhoan", parameters.SoTaiKhoan,
                "@TenNganHang", parameters.TenNganHang,
                "@DiaChi", parameters.DiaChi,
                "@MaSoThue", parameters.MaSoThue,
                "@Active", parameters.Active,
                "@RefreshToken", parameters.RefreshToken,
                "@RefreshTokenExpiryTime", parameters.RefreshTokenExpiryTime
            );

            return result > 0;
        }

        public async Task<bool> UpdateActive(int id, bool active)
        {
            if (id <= 0)
            {
                throw new ArgumentException("ID must be greater than 0", nameof(id));
            }

            try
            {
                const string sql = @"UPDATE KhaoThi_User 
                                    SET Active = @Active
                                    WHERE ID = @Id";

                // Giả sử _dataAccess.ExecuteAsync có thể nhận parameters theo cách này
                var result = await _dataAccess.ExecuteAsync(sql,
                    "@Id", id,
                    "@Active", active
                );

                return result > 0;
            }
            catch (Exception ex)
            {
                // Log exception - phụ thuộc vào logging framework của bạn
                _logger.LogError(ex, "Error updating active status for user {UserId}", id);

                // Tùy theo thiết kế của bạn, có thể throw lại exception hoặc trả về false
                throw; // hoặc return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            const string sql = "DELETE FROM KhaoThi_User WHERE ID = @Id";
            var result = await _dataAccess.ExecuteAsync(sql, "@Id", id);
            return result > 0;
        }

        public async Task<bool> IsUsernameExistAsync(string username)
        {
            const string sql = "SELECT COUNT(1) FROM KhaoThi_User WHERE TenDangNhap = @TenDangNhap";
            var count = await _dataAccess.ExecuteScalarAsync<int>(sql, "@TenDangNhap", username);
            return count > 0;
        }

        public async Task<IEnumerable<KhaoThiUser>> GetByMaDonViAsync(string maDonVi)
        {
            const string sql = "SELECT * FROM KhaoThi_User WHERE MaDonVi = @MaDonVi";
            return await _dataAccess.QueryAsync<KhaoThiUser>(sql, "@MaDonVi", maDonVi);
        }

        public async Task BulkInsertUsers(IEnumerable<KhaoThiUser> users)
        {
            await _dataAccess.BulkInsertWithBulkCopyAsync(TableName, users);
        }

        public async Task BulkUpdateUsers(IEnumerable<KhaoThiUser> users)
        {
            await _dataAccess.BulkUpdateAsync(TableName, users, "ID");
        }

        public async Task<KhaoThiUser> GetByRefreshTokenAsync(string refreshToken)
        {
            const string sql = @"SELECT * FROM KhaoThi_User WHERE RefreshToken = @RefreshToken";
            return await _dataAccess.QueryFirstOrDefaultAsync<KhaoThiUser>(sql, "@RefreshToken", refreshToken);
        }
        public async Task<bool> UpdatePasswordAsync(int userId, string newHashedPassword)
        {
            try
            {
                const string sql = @"
                UPDATE KhaoThi_User 
                SET MatKhau = @MatKhau
                   
                WHERE ID = @Id";

                var result = await _dataAccess.ExecuteAsync(sql,
                    "@Id", userId,
                    "@MatKhau", newHashedPassword

                );

                _logger.LogInformation($"✅ Đã cập nhật mật khẩu cho user ID: {userId}");
                return result > 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"🔥 Lỗi khi cập nhật mật khẩu cho user ID: {userId}");
                throw;
            }
        }

        public async Task<string> GetPasswordHashAsync(int userId)
        {
            try
            {
                const string sql = @"SELECT MatKhau FROM KhaoThi_User WHERE ID = @Id";

                var hashedPassword = await _dataAccess.ExecuteScalarAsync<string>(sql, "@Id", userId);

                if (string.IsNullOrEmpty(hashedPassword))
                {
                    _logger.LogWarning($"⚠️ Không tìm thấy mật khẩu cho user ID: {userId}");
                    return null;
                }

                return hashedPassword;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"🔥 Lỗi khi lấy mật khẩu cho user ID: {userId}");
                throw;
            }
        }
    }
}
