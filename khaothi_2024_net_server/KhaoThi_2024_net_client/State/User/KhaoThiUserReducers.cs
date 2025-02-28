using Fluxor;
using KhaoThi_2024_net_client.Services.User;
using static KhaoThi_2024_net_client.Services.User.KhaoThiUserActions;


namespace KhaoThi_2024_net_client.State.User
{
    /// <summary>
    /// Class chứa các reducers để xử lý và cập nhật state người dùng
    /// </summary>
    /// 

    public static class KhaoThiUserReducers
    {
        #region Load Users Reducers
        /// <summary>
        /// Reducer xử lý action tải danh sách người dùng
        /// </summary>
        [ReducerMethod]
        public static KhaoThiUserState ReduceLoadUsersAction(KhaoThiUserState state, LoadUsersAction action) =>
            new(isLoading: true,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        [ReducerMethod]
        public static KhaoThiUserState ReduceLoadUsersSuccessAction(KhaoThiUserState state, LoadUsersSuccessAction action) =>
            new(isLoading: false,
                errorMessage: null,
                users: action.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: true,
                notificationMessage: "Tải danh sách người dùng thành công",
                notificationType: "success");


        [ReducerMethod]
        public static KhaoThiUserState ReduceLoadUsersFailureAction(KhaoThiUserState state, LoadUsersFailureAction action) =>
            new(isLoading: false,
                errorMessage: $"{action.ErrorMessage} [{DateTime.Now.Ticks}]",
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi tải danh sách: {action.ErrorMessage}",
                notificationType: "error");
        #endregion

        #region Load Paginated Users Reducers
        /// <summary>
        /// Reducer xử lý action tải danh sách người dùng phân trang
        /// </summary>
        [ReducerMethod]
        public static KhaoThiUserState ReduceLoadPaginatedUsersAction(KhaoThiUserState state, LoadPaginatedUsersAction action) =>
            new(isLoading: true,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        [ReducerMethod]
        public static KhaoThiUserState ReduceLoadPaginatedUsersSuccessAction(KhaoThiUserState state, LoadPaginatedUsersSuccessAction action) =>
            new(isLoading: false,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: action.PaginatedUsers,
                isInitialized: true,
                notificationMessage: null,
                notificationType: null);

        [ReducerMethod]
        public static KhaoThiUserState ReduceLoadPaginatedUsersFailureAction(KhaoThiUserState state, LoadPaginatedUsersFailureAction action) =>
            new(isLoading: false,
                errorMessage: $"{action.ErrorMessage} [{DateTime.Now.Ticks}]",
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi: {action.ErrorMessage}",
                notificationType: "error");
        #endregion

        #region Single User Management Reducers
        /// <summary>
        /// Reducer xử lý action chọn người dùng
        /// </summary>
        [ReducerMethod]
        public static KhaoThiUserState ReduceSelectUserAction(KhaoThiUserState state, SelectUserAction action) =>
            new(isLoading: false,
                errorMessage: null,
                users: state.Users,
                selectedUser: action.User,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized);

        /// <summary>
        /// Reducer xử lý action lấy thông tin người dùng theo ID
        /// </summary>
        [ReducerMethod]
        public static KhaoThiUserState ReduceGetUserByIdAction(KhaoThiUserState state, GetUserByIdAction action) =>
            new(isLoading: true,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized);

        [ReducerMethod]
        public static KhaoThiUserState ReduceGetUserByIdSuccessAction(KhaoThiUserState state, GetUserByIdSuccessAction action) =>
            new(isLoading: false,
                errorMessage: null,
                users: state.Users,
                selectedUser: action.User,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized);

        [ReducerMethod]
        public static KhaoThiUserState ReduceGetUserByIdFailureAction(KhaoThiUserState state, GetUserByIdFailureAction action) =>
            new(isLoading: false,
                errorMessage: $"{action.ErrorMessage} [{DateTime.Now.Ticks}]",
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized);
        #endregion

        #region Create User Reducers
        /// <summary>
        /// Reducer xử lý action tạo mới người dùng
        /// </summary>
        [ReducerMethod]
        public static KhaoThiUserState ReduceCreateUserAction(KhaoThiUserState state, CreateUserAction action) =>
            new(isLoading: true,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        [ReducerMethod]
        public static KhaoThiUserState ReduceCreateUserSuccessAction(KhaoThiUserState state, CreateUserSuccessAction action)
        {
            var updatedUsers = state.Users?.Append(action.CreatedUser) ?? new[] { action.CreatedUser };
            var updatedPaginatedUsers = state.PaginatedUsers == null ? null :
                new Components.PaginatedResult<Models.Users.KhaoThiUserModel>(
                    state.PaginatedUsers.Items.Append(action.CreatedUser),
                    state.PaginatedUsers.TotalPages + 1,
                    state.PaginatedUsers.PageSize,
                    state.PaginatedUsers.Page
                );

            return new(isLoading: false,
                errorMessage: null,
                users: updatedUsers,
                selectedUser: action.CreatedUser,
                paginatedUsers: updatedPaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: "Tạo người dùng mới thành công",
                notificationType: "success");
        }

        [ReducerMethod]
        public static KhaoThiUserState ReduceCreateUserFailureAction(KhaoThiUserState state, CreateUserFailureAction action) =>
            new(isLoading: false,
                errorMessage: action.ErrorMessage,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi tạo người dùng: {action.ErrorMessage}",
                notificationType: "error");
        #endregion

        #region Update User Reducers
        /// <summary>
        /// Reducer xử lý action cập nhật người dùng
        /// </summary>
        //[ReducerMethod]
        //public static KhaoThiUserState ReduceUpdateUserAction(KhaoThiUserState state, UpdateUserAction action) =>
        //    new(isLoading: true,
        //        errorMessage: null,
        //        users: state.Users,
        //        selectedUser: state.SelectedUser,
        //        paginatedUsers: state.PaginatedUsers,
        //        isInitialized: state.IsInitialized);
        [ReducerMethod]
        public static KhaoThiUserState ReduceUpdateUserAction(KhaoThiUserState state, UpdateUserAction action)
        {
            var updatedUser = action.User;

            // Giữ lại giá trị RefreshToken và RefreshTokenExpiryTime từ state.SelectedUser
            if (state.SelectedUser != null)
            {
                updatedUser.RefreshToken = state.SelectedUser.RefreshToken;
                updatedUser.RefreshTokenExpiryTime = state.SelectedUser.RefreshTokenExpiryTime;
            }

            return new KhaoThiUserState(
                isLoading: true,
                errorMessage: null,
                users: state.Users,
                selectedUser: updatedUser,  // Sử dụng updatedUser đã được cập nhật token
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null
            );
        }

        [ReducerMethod]
        public static KhaoThiUserState ReduceUpdateUserSuccessAction(KhaoThiUserState state, UpdateUserSuccessAction action)
        {
            var updatedUsers = state.Users?.Select(u => u.ID == action.UpdatedUser.ID ? action.UpdatedUser : u);
            var updatedPaginatedUsers = state.PaginatedUsers == null ? null :
                new Components.PaginatedResult<Models.Users.KhaoThiUserModel>(
                    state.PaginatedUsers.Items.Select(u => u.ID == action.UpdatedUser.ID ? action.UpdatedUser : u),
                    state.PaginatedUsers.TotalPages,
                    state.PaginatedUsers.PageSize,
                    state.PaginatedUsers.Page
                );

            return new(isLoading: false,
                errorMessage: null,
                users: updatedUsers,
                selectedUser: action.UpdatedUser,
                paginatedUsers: updatedPaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: "Cập nhật người dùng thành công",  // Thêm thông báo
                notificationType: "success"  // Thêm loại thông báo
                );

        }

        [ReducerMethod]
        public static KhaoThiUserState ReduceUpdateUserFailureAction(KhaoThiUserState state, UpdateUserFailureAction action) =>
            new(isLoading: false,
                errorMessage: action.ErrorMessage,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi cập nhật: {action.ErrorMessage}",  // Thêm thông báo lỗi
                notificationType: "error"  // Thêm loại thông báo
                );
        #endregion

        #region Change Password User Reducers
        [ReducerMethod]
        public static KhaoThiUserState ReduceChangePasswordAction(KhaoThiUserState state, ChangePasswordAction action)
        {
            return new KhaoThiUserState(
                isLoading: true,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null

            );
        }
        [ReducerMethod]
        public static KhaoThiUserState ReduceChangePasswordSuccessAction(KhaoThiUserState state, ChangePasswordSuccessAction action)
        {


            return new(isLoading: false,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: "Điều chỉnh mật khẩu thành công",  // Thêm thông báo
                notificationType: "success"  // Thêm loại thông báo
                );

        }
        [ReducerMethod]
        public static KhaoThiUserState ReduceChangPasswordFailureAction(KhaoThiUserState state, ChangePasswordFailureAction action)
        {


            return new(isLoading: false,
                errorMessage: $"{action.ErrorMessage} [{DateTime.Now.Ticks}]",
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: "Lỗi điều chỉnh mật khẩu: " + action.ErrorMessage,  // Thêm thông báo
                notificationType: "error"  // Thêm loại thông báo
                );


        }
        #endregion

        #region Delete User Reducers
        /// <summary>
        /// Reducer xử lý action xóa người dùng
        /// </summary>
        [ReducerMethod]
        public static KhaoThiUserState ReduceDeleteUserAction(KhaoThiUserState state, DeleteUserAction action) =>
            new(isLoading: true,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        [ReducerMethod]
        public static KhaoThiUserState ReduceDeleteUserSuccessAction(KhaoThiUserState state, DeleteUserSuccessAction action)
        {
            var updatedUsers = state.Users?.Where(u => u.ID != action.Id);
            var updatedPaginatedUsers = state.PaginatedUsers == null ? null :
                new Components.PaginatedResult<Models.Users.KhaoThiUserModel>(
                    state.PaginatedUsers.Items.Where(u => u.ID != action.Id),
                    state.PaginatedUsers.TotalPages - 1,
                    state.PaginatedUsers.PageSize,
                    state.PaginatedUsers.Page
                );

            return new(isLoading: false,
                errorMessage: null,
                users: updatedUsers,
                selectedUser: state.SelectedUser?.ID == action.Id ? null : state.SelectedUser,
                paginatedUsers: updatedPaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: "Xóa người dùng thành công",
                notificationType: "success");

        }

        [ReducerMethod]
        public static KhaoThiUserState ReduceDeleteUserFailureAction(KhaoThiUserState state, DeleteUserFailureAction action) =>
            new(isLoading: false,
                errorMessage: $"{action.ErrorMessage} [{DateTime.Now.Ticks}]",
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi xóa người dùng: {action.ErrorMessage}",
                notificationType: "error");
        #endregion

        //[ReducerMethod]
        //public static KhaoThiUserState ReduceUpdatePassword(KhaoThiUserState state, UpdatePasswordAction action)
        //{
        //    return new(
        //        isLoading: true,
        //        errorMessage: null,
        //        users: state.Users,
        //        selectedUser: state.SelectedUser,
        //        paginatedUsers: null


        //        );
        //}

        #region Load Users By DonVi Reducers
        /// <summary>
        /// Reducer xử lý action lấy danh sách người dùng theo đơn vị
        /// </summary>
        [ReducerMethod]
        public static KhaoThiUserState ReduceLoadUsersByDonViAction(KhaoThiUserState state, LoadUsersByDonViAction action) =>
            new(isLoading: true,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized);

        [ReducerMethod]
        public static KhaoThiUserState ReduceLoadUsersByDonViSuccessAction(KhaoThiUserState state, LoadUsersByDonViSuccessAction action) =>
            new(isLoading: false,
                errorMessage: null,
                users: action.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: true);

        [ReducerMethod]
        public static KhaoThiUserState ReduceLoadUsersByDonViFailureAction(KhaoThiUserState state, LoadUsersByDonViFailureAction action) =>
            new(isLoading: false,
                errorMessage: $"{action.ErrorMessage} [{DateTime.Now.Ticks}]",
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized);
        #endregion

        #region Bulk Operations Reducers
        /// <summary>
        /// Reducer xử lý action thêm nhiều người dùng
        /// </summary>
        [ReducerMethod]
        public static KhaoThiUserState ReduceBulkInsertUsersAction(KhaoThiUserState state, BulkInsertUsersAction action) =>
            new(isLoading: true,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized);

        [ReducerMethod]
        public static KhaoThiUserState ReduceBulkInsertUsersSuccessAction(KhaoThiUserState state, BulkInsertUsersSuccessAction action) =>
            new(isLoading: false,
                errorMessage: null,
                users: null, // Reset users để tải lại
                selectedUser: state.SelectedUser,
                paginatedUsers: null, // Reset paginatedUsers để tải lại
                isInitialized: false);

        [ReducerMethod]
        public static KhaoThiUserState ReduceBulkInsertUsersFailureAction(KhaoThiUserState state, BulkInsertUsersFailureAction action) =>
            new(isLoading: false,
                errorMessage: $"{action.ErrorMessage} [{DateTime.Now.Ticks}]",
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized);

        /// <summary>
        /// Reducer xử lý action cập nhật nhiều người dùng
        /// </summary>
        [ReducerMethod]
        public static KhaoThiUserState ReduceBulkUpdateUsersAction(KhaoThiUserState state, BulkUpdateUsersAction action) =>
            new(isLoading: true,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized);

        [ReducerMethod]
        public static KhaoThiUserState ReduceBulkUpdateUsersSuccessAction(KhaoThiUserState state, BulkUpdateUsersSuccessAction action) =>
            new(isLoading: false,
                errorMessage: null,
                users: null, // Reset users để tải lại
                selectedUser: state.SelectedUser,
                paginatedUsers: null, // Reset paginatedUsers để tải lại
                isInitialized: false);

        [ReducerMethod]
        public static KhaoThiUserState ReduceBulkUpdateUsersFailureAction(KhaoThiUserState state, BulkUpdateUsersFailureAction action) =>
            new(isLoading: false,
                errorMessage: $"{action.ErrorMessage} [{DateTime.Now.Ticks}]",
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized);
        #endregion

        #region Username Check Reducers
        /// <summary>
        /// Reducer xử lý action kiểm tra username
        /// </summary>
        [ReducerMethod]
        public static KhaoThiUserState ReduceCheckUsernameAction(KhaoThiUserState state, CheckUsernameAction action) =>
            new(isLoading: true,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized);

        [ReducerMethod]
        public static KhaoThiUserState ReduceCheckUsernameSuccessAction(KhaoThiUserState state, CheckUsernameSuccessAction action) =>
            state; // Không cần thay đổi state vì kết quả được xử lý trực tiếp bởi component

        [ReducerMethod]
        public static KhaoThiUserState ReduceCheckUsernameFailureAction(KhaoThiUserState state, CheckUsernameFailureAction action) =>
            new(isLoading: false,
                errorMessage: $"{action.ErrorMessage} [{DateTime.Now.Ticks}]",
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized);
        #endregion

        #region Utility Reducers
        /// <summary>
        /// Reducer xử lý action set loading
        /// </summary>
        [ReducerMethod]
        public static KhaoThiUserState ReduceSetLoadingAction(KhaoThiUserState state, SetLoadingAction action) =>
            new(isLoading: action.IsLoading,
                errorMessage: $"{state.ErrorMessage} [{DateTime.Now.Ticks}]",
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: state.NotificationMessage,
                notificationType: state.NotificationType
                );

        /// <summary>
        /// Reducer xử lý action clear error
        /// </summary>
        [ReducerMethod]
        public static KhaoThiUserState ReduceClearErrorAction(KhaoThiUserState state, ClearErrorAction action) =>
            new(isLoading: state.IsLoading,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        [ReducerMethod]
        public static KhaoThiUserState ReduceShowNotificationAction(KhaoThiUserState state, ShowNotificationAction action) =>
     new(
         isLoading: state.IsLoading,
         errorMessage: $"{state.ErrorMessage} [{DateTime.Now.Ticks}]",
         users: state.Users,
         selectedUser: state.SelectedUser,
         paginatedUsers: state.PaginatedUsers,
         isInitialized: state.IsInitialized,
         notificationMessage: action.Message,
         notificationType: action.Type
     );
        #endregion
    }
}
