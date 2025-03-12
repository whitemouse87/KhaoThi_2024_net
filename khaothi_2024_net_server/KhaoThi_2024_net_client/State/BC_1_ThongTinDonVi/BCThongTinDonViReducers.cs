using Fluxor;
using KhaoThi_2024_net_client.Services.BC_1_ThongTinDonVi;
using static KhaoThi_2024_net_client.Services.BC_1_ThongTinDonVi.BCThongTinDonViAction;

namespace KhaoThi_2024_net_client.State.BC_1_ThongTinDonVi
{
    public class BCThongTinDonViReducers
    {

        #region Load Paginated Users Reducers
        /// <summary>
        /// Reducer xử lý action tải danh sách người dùng phân trang
        /// </summary>
        [ReducerMethod]
        public static BCThongTinDonViState ReduceLoadPaginatedDonVisAction(BCThongTinDonViState state, LoadPaginatedDonVisAction action) =>
            new(id:1,
                maTruong: state.MaTruong,
                isLoading: true,
                errorMessage: null,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);
      
        [ReducerMethod]
        public static BCThongTinDonViState ReduceLoadPaginatedUsersSuccessAction(BCThongTinDonViState state, LoadPaginatedDonVisSuccessAction action) =>
            new(id: 5,
                isLoading: false,
                errorMessage: null,
                users: state.Users,
                selectedUser: state.SelectedUser,
                paginatedUsers: action.PaginatedUsers,
                isInitialized: true,
                notificationMessage: null,
                notificationType: null);

        [ReducerMethod]
        public static KhaoThiUserState ReduceLoadPaginatedUsersFailureAction(KhaoThiUserState state, LoadPaginatedDonVisFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            string? notificationMsg = errorMsg == null ? null : $"Lỗi: {action.ErrorMessage}";
            return new(id: 6,
                 isLoading: false,
                 errorMessage: errorMsg,
                 users: state.Users,
                 selectedUser: state.SelectedUser,
                 paginatedUsers: state.PaginatedUsers,
                 isInitialized: state.IsInitialized,
                 notificationMessage: notificationMsg,
                 notificationType: errorMsg == null ? null : "error");
        }
        #endregion
    }
}
