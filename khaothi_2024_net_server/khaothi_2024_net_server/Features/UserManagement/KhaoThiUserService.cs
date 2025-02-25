using khaothi_2024_net_server.Core.Interfaces;
using khaothi_2024_net_server.Features.UserManagement.DTOs;

namespace khaothi_2024_net_server.Features.UserManagement
{
    public class KhaoThiUserService : IKhaoThiUserService
    {
        private readonly IKhaoThiUserRepository _userRepository;
        private readonly ILogger<KhaoThiUserService> _logger;

        /// <summary>
        /// Constructor với Dependency Injection
        /// </summary>
        public KhaoThiUserService(
            IKhaoThiUserRepository userRepository,
            ILogger<KhaoThiUserService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }
        /// <summary>
        /// Lấy thông tin người dùng theo ID
        /// </summary>
        public async Task<KhaoThiUser?> GetByIdAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning($"[Warning] ID không hợp lệ: {id}");
                throw new ArgumentException("ID phải lớn hơn 0", nameof(id));
            }

            try
            {
                _logger.LogInformation($"🔍 Đang tìm user với ID: {id}");

                var user = await _userRepository.GetByIdAsync(id);

                if (user is null)
                {
                    _logger.LogWarning($"❌ Không tìm thấy user với ID {id}");
                    return null; // Trả về null thay vì throw
                }

                _logger.LogInformation($"✅ Tìm thấy user với ID {id}: {user.HoTen}");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"🔥 Lỗi khi truy vấn user ID {id}");
                throw;
            }
        }

        /// <summary>
        /// Lấy thông tin người dùng theo tên đăng nhập
        /// </summary>
        public async Task<KhaoThiUser> GetByUsernameAsync(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw new ArgumentException("Tên đăng nhập không được để trống");
                }

                return await _userRepository.GetByUsernameAsync(username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy thông tin người dùng với username: {username}");
                throw;
            }
        }
        /// <summary>
        /// Lấy danh sách tất cả người dùng
        /// </summary>
        public async Task<IEnumerable<KhaoThiUser>> GetAllAsync()
        {
            try
            {
                return await _userRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách tất cả người dùng");
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách người dùng theo trang
        /// </summary>
        public async Task<(IEnumerable<KhaoThiUser> Items, int TotalCount)> GetPaginatedAsync(
            int page,
            int pageSize,
            string? searchTerm = null)
        {
            try
            {
                if (page < 1) page = 1;
                if (pageSize < 1) pageSize = 10;
                if (pageSize > 100) pageSize = 100; // Giới hạn kích thước trang tối đa

                return await _userRepository.GetPaginatedAsync(page, pageSize, searchTerm);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách người dùng phân trang");
                throw;
            }
        }
        /// <summary>
        /// Tạo mới người dùng
        /// </summary>
        public async Task<KhaoThiUser> CreateAsync(KhaoThiUser user)
        {
            try
            {
                // Validate dữ liệu đầu vào
                ValidateUserData(user);

                // Kiểm tra username đã tồn tại
                if (await IsUsernameExistAsync(user.TenDangNhap))
                {
                    throw new InvalidOperationException($"Tên đăng nhập {user.TenDangNhap} đã tồn tại");
                }

                // Set các giá trị mặc định
                user.NgayTao = DateTime.Now;
                user.Active = true;

                var id = await _userRepository.CreateAsync(user);
                user.ID = id;
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo mới người dùng");
                throw;
            }
        }
        /// <summary>
        /// Cập nhật thông tin người dùng
        /// </summary>
        public async Task<bool> UpdateAsync(KhaoThiUser user)
        {
            try
            {
                // Validate dữ liệu đầu vào
                ValidateUserData(user);

                var existingUser = await GetByIdAsync(user.ID);
                if (existingUser == null)
                {
                    return false;
                }

                // Kiểm tra nếu thay đổi username
                if (existingUser.TenDangNhap != user.TenDangNhap)
                {
                    if (await IsUsernameExistAsync(user.TenDangNhap))
                    {
                        throw new InvalidOperationException($"Tên đăng nhập {user.TenDangNhap} đã tồn tại");
                    }
                }

                return await _userRepository.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi cập nhật người dùng ID: {user.ID}");
                throw;
            }
        }

        /// <summary>
        /// Xóa người dùng
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                return await _userRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi xóa người dùng ID: {id}");
                throw;
            }
        }

        /// <summary>
        /// Kiểm tra username đã tồn tại
        /// </summary>
        public async Task<bool> IsUsernameExistAsync(string username)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username))
                {
                    throw new ArgumentException("Tên đăng nhập không được để trống");
                }

                return await _userRepository.IsUsernameExistAsync(username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi kiểm tra tên đăng nhập: {username}");
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách người dùng theo mã đơn vị
        /// </summary>
        public async Task<IEnumerable<KhaoThiUser>> GetByMaDonViAsync(string maDonVi)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(maDonVi))
                {
                    throw new ArgumentException("Mã đơn vị không được để trống");
                }

                return await _userRepository.GetByMaDonViAsync(maDonVi);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Lỗi khi lấy danh sách người dùng theo mã đơn vị: {maDonVi}");
                throw;
            }
        }

        /// <summary>
        /// Thêm nhiều người dùng cùng lúc
        /// </summary>
        public async Task BulkInsertUsersAsync(IEnumerable<KhaoThiUser> users)
        {
            try
            {
                if (users == null || !users.Any())
                {
                    throw new ArgumentException("Danh sách người dùng không được để trống");
                }

                // Validate từng user trong danh sách
                foreach (var user in users)
                {
                    ValidateUserData(user);
                }

                await _userRepository.BulkInsertUsers(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi thêm nhiều người dùng");
                throw;
            }
        }

        /// <summary>
        /// Cập nhật nhiều người dùng cùng lúc
        /// </summary>
        public async Task BulkUpdateUsersAsync(IEnumerable<KhaoThiUser> users)
        {
            try
            {
                if (users == null || !users.Any())
                {
                    throw new ArgumentException("Danh sách người dùng không được để trống");
                }

                // Validate từng user trong danh sách
                foreach (var user in users)
                {
                    ValidateUserData(user);
                }

                await _userRepository.BulkUpdateUsers(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật nhiều người dùng");
                throw;
            }
        }

        /// <summary>
        /// Validate dữ liệu người dùng
        /// </summary>
        private void ValidateUserData(KhaoThiUser user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (string.IsNullOrWhiteSpace(user.TenDangNhap))
            {
                throw new ArgumentException("Tên đăng nhập không được để trống");
            }

            if (string.IsNullOrWhiteSpace(user.HoTen))
            {
                throw new ArgumentException("Họ tên không được để trống");
            }

            if (string.IsNullOrWhiteSpace(user.MaDonVi))
            {
                throw new ArgumentException("Mã đơn vị không được để trống");
            }

            // Có thể thêm các validation khác tùy theo yêu cầu nghiệp vụ
        }
        public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordRequest request)
        {
            try
            {
                _logger.LogInformation($"🔄 Bắt đầu xử lý đổi mật khẩu cho user ID: {userId}");

                // Validate request
                if (request == null)
                {
                    throw new ArgumentNullException(nameof(request));
                }

                // Validate current password
                var currentHashedPassword = await _userRepository.GetPasswordHashAsync(userId);
                if (currentHashedPassword == null)
                {
                    throw new InvalidOperationException("Không tìm thấy thông tin người dùng");
                }

                // Verify current password
                if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, currentHashedPassword))
                {
                    _logger.LogWarning($"❌ Mật khẩu hiện tại không đúng cho user ID: {userId}");
                    throw new InvalidOperationException("Mật khẩu hiện tại không đúng");
                }

                // Check if new password is same as current
                if (BCrypt.Net.BCrypt.Verify(request.NewPassword, currentHashedPassword))
                {
                    throw new InvalidOperationException("Mật khẩu mới không được trùng với mật khẩu hiện tại");
                }

                // Hash new password
                string newHashedPassword = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

                // Update password
                var result = await _userRepository.UpdatePasswordAsync(userId, newHashedPassword);

                if (result)
                {
                    _logger.LogInformation($"✅ Đổi mật khẩu thành công cho user ID: {userId}");
                }
                else
                {
                    _logger.LogWarning($"⚠️ Không thể cập nhật mật khẩu cho user ID: {userId}");
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"🔥 Lỗi khi đổi mật khẩu cho user ID: {userId}");
                throw;
            }
        }
    }
}
