using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.Users;

namespace KhaoThi_2024_net_client.Services.User
{
    /// <summary>
    /// Định nghĩa các loại action cho quản lý người dùng
    /// </summary>
    public static class KhaoThiUserActionTypes
    {
        // Tải danh sách người dùng
        public const string LOAD_USERS = "[Người dùng] Tải danh sách";
        public const string LOAD_USERS_SUCCESS = "[Người dùng] Tải danh sách thành công";
        public const string LOAD_USERS_FAILURE = "[Người dùng] Tải danh sách thất bại";

        // Phân trang
        public const string LOAD_PAGINATED = "[Người dùng] Tải danh sách phân trang";
        public const string LOAD_PAGINATED_SUCCESS = "[Người dùng] Tải phân trang thành công";
        public const string LOAD_PAGINATED_FAILURE = "[Người dùng] Tải phân trang thất bại";

        // Cập nhật thông tin
        public const string UPDATE_USER = "[Người dùng] Cập nhật thông tin";
        public const string UPDATE_USER_SUCCESS = "[Người dùng] Cập nhật thành công";
        public const string UPDATE_USER_FAILURE = "[Người dùng] Cập nhật thất bại";

        // Thông báo và trạng thái
        public const string SHOW_NOTIFICATION = "[Người dùng] Hiển thị thông báo";
        public const string SET_LOADING = "[Người dùng] Đang tải";
        public const string CLEAR_ERROR = "[Người dùng] Xóa lỗi";
    }

    /// <summary>
    /// Giao diện cơ sở cho tất cả các action
    /// </summary>
    public interface IKhaoThiUserAction
    {
        string Type { get; }
        DateTime Timestamp { get; }
    }

    public static class KhaoThiUserActions
    {
        // Actions cho tải danh sách và phân trang
        public record LoadUsersAction();
        public record LoadUsersSuccessAction(IEnumerable<KhaoThiUserModel> Users);
        public record LoadUsersFailureAction(string ErrorMessage);

        public record LoadPaginatedUsersAction(int Page, int PageSize, string? SearchTerm);
        public record LoadPaginatedUsersSuccessAction(PaginatedResult<KhaoThiUserModel> PaginatedUsers);
        public record LoadPaginatedUsersFailureAction(string ErrorMessage);

        // Actions cho thao tác với user đơn lẻ
        public record SelectUserAction(KhaoThiUserModel User);

        public record GetUserByIdAction(int Id);
        public record GetUserByIdSuccessAction(KhaoThiUserModel User);
        public record GetUserByIdFailureAction(string ErrorMessage);

        public record CreateUserAction(KhaoThiUserModel User);
        public record CreateUserSuccessAction(KhaoThiUserModel CreatedUser);
        public record CreateUserFailureAction(string ErrorMessage);

        public record UpdateUserAction(KhaoThiUserModel User);
        public record UpdateUserSuccessAction(KhaoThiUserModel UpdatedUser);
        public record UpdateUserFailureAction(string ErrorMessage);

        public record DeleteUserAction(int Id);
        public record DeleteUserSuccessAction(int Id);
        public record DeleteUserFailureAction(string ErrorMessage);

        //Actions cập nhật mật khẩu
        public record ChangePasswordAction(int id, ChangePasswordModel info);
        public record ChangePasswordSuccessAction(int Id);
        public record ChangePasswordFailureAction(string ErrorMessage);

        //Actions cập active
        public record ChangeActiveAction(int id, bool active);
        public record ChangeActiveSuccessAction(int Id);
        public record ChangeActiveFailureAction(string ErrorMessage);

        // Actions cho thao tác theo đơn vị
        public record LoadUsersByDonViAction(string MaDonVi);
        public record LoadUsersByDonViSuccessAction(IEnumerable<KhaoThiUserModel> Users);
        public record LoadUsersByDonViFailureAction(string ErrorMessage);

        // Actions cho thao tác hàng loạt
        public record BulkInsertUsersAction(IEnumerable<KhaoThiUserModel> Users);
        public record BulkInsertUsersSuccessAction();
        public record BulkInsertUsersFailureAction(string ErrorMessage);

        public record BulkUpdateUsersAction(IEnumerable<KhaoThiUserModel> Users);
        public record BulkUpdateUsersSuccessAction();
        public record BulkUpdateUsersFailureAction(string ErrorMessage);

        // Actions cho kiểm tra username
        public record CheckUsernameAction(string Username);
        public record CheckUsernameSuccessAction(bool Exists);
        public record CheckUsernameFailureAction(string ErrorMessage);

        // Actions cho trạng thái loading và clear lỗi
        public record SetLoadingAction(bool IsLoading);
        public record ClearErrorAction();
        public record ShowNotificationAction : IKhaoThiUserAction
        {
            public string Type => KhaoThiUserActionTypes.SHOW_NOTIFICATION;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
            public string Message { get; }
            public string NotificationType { get; }  // Đổi tên từ Type thành NotificationType

            public ShowNotificationAction(string message, string notificationType)
            {
                if (string.IsNullOrWhiteSpace(message))
                    throw new ArgumentException("Nội dung thông báo không được để trống", nameof(message));

                Message = message;
                NotificationType = notificationType.ToLower().Trim(); // Normalize type

                // Validate notification type
                if (NotificationType != "success" && NotificationType != "error")
                {
                    throw new ArgumentException(
                        "Loại thông báo phải là 'success' hoặc 'error'",
                        nameof(notificationType)
                    );
                }
            }
        }





    }
}
