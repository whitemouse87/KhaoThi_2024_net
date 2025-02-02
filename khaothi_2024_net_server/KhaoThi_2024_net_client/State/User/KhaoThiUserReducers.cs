using Fluxor;
using KhaoThi_2024_net_client.Services.User;
using static KhaoThi_2024_net_client.Services.User.KhaoThiUserActions;

namespace KhaoThi_2024_net_client.State.User
{
    public static class KhaoThiUserReducers
    {
        // Reducers cho tải danh sách người dùng
        [ReducerMethod]
        public static KhaoThiUserState ReduceLoadUsersAction(KhaoThiUserState state, LoadUsersAction action)
        {
            return new KhaoThiUserState(
                isLoading: true,
                errorMessage: null,
                users: null,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized
            );
        }

        [ReducerMethod]
        public static KhaoThiUserState ReduceLoadUsersSuccess(KhaoThiUserState state, LoadUsersSuccessAction action)
        {
            return new KhaoThiUserState(
                isLoading: false,
                errorMessage: null,
                users: action.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: true
            );
        }

        [ReducerMethod]
        public static KhaoThiUserState ReduceLoadUsersFailure(KhaoThiUserState state, LoadUsersFailureAction action)
        {
            return new KhaoThiUserState(
                isLoading: false,
                errorMessage: action.ErrorMessage,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized
            );
        }

        // Reducers cho tải danh sách phân trang
        [ReducerMethod]
        public static KhaoThiUserState ReduceLoadPaginatedUsers(KhaoThiUserState state, LoadPaginatedUsersAction action)
        {
            return new KhaoThiUserState(
                isLoading: true,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized
            );
        }

        [ReducerMethod]
        public static KhaoThiUserState ReduceLoadPaginatedUsersSuccess(KhaoThiUserState state, LoadPaginatedUsersSuccessAction action)
        {
            return new KhaoThiUserState(
                isLoading: false,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: action.PaginatedUsers,
                isInitialized: true
            );
        }

        [ReducerMethod]
        public static KhaoThiUserState ReduceLoadPaginatedUsersFailure(KhaoThiUserState state, LoadPaginatedUsersFailureAction action)
        {
            return new KhaoThiUserState(
                isLoading: false,
                errorMessage: action.ErrorMessage,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized
            );
        }

        // Reducer cho chọn user
        [ReducerMethod]
        public static KhaoThiUserState ReduceSelectUser(KhaoThiUserState state, SelectUserAction action)
        {
            return new KhaoThiUserState(
                isLoading: false,
                errorMessage: null,
                users: state.Users,
                selectedUser: action.User,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized
            );
        }

        // Reducers cho tạo mới user
        [ReducerMethod]
        public static KhaoThiUserState ReduceCreateUser(KhaoThiUserState state, CreateUserAction action)
        {
            return new KhaoThiUserState(
                isLoading: true,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized
            );
        }

        [ReducerMethod]
        public static KhaoThiUserState ReduceCreateUserSuccess(KhaoThiUserState state, CreateUserSuccessAction action)
        {
            var updatedUsers = state.Users?.Append(action.CreatedUser) ?? new[] { action.CreatedUser };

            return new KhaoThiUserState(
                isLoading: false,
                errorMessage: null,
                users: updatedUsers,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized
            );
        }

        [ReducerMethod]
        public static KhaoThiUserState ReduceCreateUserFailure(KhaoThiUserState state, CreateUserFailureAction action)
        {
            return new KhaoThiUserState(
                isLoading: false,
                errorMessage: action.ErrorMessage,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized
            );
        }

        // Reducers cho cập nhật user
        [ReducerMethod]
        public static KhaoThiUserState ReduceUpdateUser(KhaoThiUserState state, UpdateUserAction action)
        {
            return new KhaoThiUserState(
                isLoading: true,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized
            );
        }

        [ReducerMethod]
        public static KhaoThiUserState ReduceUpdateUserSuccess(KhaoThiUserState state, UpdateUserSuccessAction action)
        {
            var updatedUsers = state.Users?.Select(u => u.ID == action.UpdatedUser.ID ? action.UpdatedUser : u);

            return new KhaoThiUserState(
                isLoading: false,
                errorMessage: null,
                users: updatedUsers,
                selectedUser: action.UpdatedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized
            );
        }

        [ReducerMethod]
        public static KhaoThiUserState ReduceUpdateUserFailure(KhaoThiUserState state, UpdateUserFailureAction action)
        {
            return new KhaoThiUserState(
                isLoading: false,
                errorMessage: action.ErrorMessage,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized
            );
        }

        // Reducers cho xóa user
        [ReducerMethod]
        public static KhaoThiUserState ReduceDeleteUser(KhaoThiUserState state, DeleteUserAction action)
        {
            return new KhaoThiUserState(
                isLoading: true,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized
            );
        }

        [ReducerMethod]
        public static KhaoThiUserState ReduceDeleteUserSuccess(KhaoThiUserState state, DeleteUserSuccessAction action)
        {
            var updatedUsers = state.Users?.Where(u => u.ID != action.Id);
            var updatedSelectedUser = state.SelectedUser?.ID == action.Id ? null : state.SelectedUser;

            return new KhaoThiUserState(
                isLoading: false,
                errorMessage: null,
                users: updatedUsers,
                selectedUser: updatedSelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized
            );
        }

        [ReducerMethod]
        public static KhaoThiUserState ReduceDeleteUserFailure(KhaoThiUserState state, DeleteUserFailureAction action)
        {
            return new KhaoThiUserState(
                isLoading: false,
                errorMessage: action.ErrorMessage,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized
            );
        }

        // Reducers cho kiểm tra username
        [ReducerMethod]
        public static KhaoThiUserState ReduceCheckUsername(KhaoThiUserState state, CheckUsernameAction action)
        {
            return new KhaoThiUserState(
                isLoading: true,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized
            );
        }

        [ReducerMethod]
        public static KhaoThiUserState ReduceCheckUsernameSuccess(KhaoThiUserState state, CheckUsernameSuccessAction action)
        {
            return new KhaoThiUserState(
                isLoading: false,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: state.PaginatedUsers,
                isInitialized: state.IsInitialized
            );
        }
    }
}
