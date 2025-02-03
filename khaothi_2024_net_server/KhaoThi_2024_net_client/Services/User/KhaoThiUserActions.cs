using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.Users;

namespace KhaoThi_2024_net_client.Services.User
{
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
        public record ShowNotificationAction(string Message, string Type);

        
    }
}
