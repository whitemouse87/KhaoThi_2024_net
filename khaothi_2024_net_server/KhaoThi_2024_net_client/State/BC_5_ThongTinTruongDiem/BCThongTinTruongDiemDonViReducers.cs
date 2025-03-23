using Fluxor;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Services.BC_5_TruongDiemDonVi;
using static KhaoThi_2024_net_client.Services.BC_5_TruongDiemDonVi.BCThongTinTruongDiemAction;

namespace KhaoThi_2024_net_client.State.BC_5_TruongDiemDonVi
{
    public class BCThongTinTruongDiemReducers
    {
        #region Load Paginated TruongDiem Reducers
        /// <summary>
        /// Reducer xử lý action tải danh sách trường điểm phân trang
        /// </summary>
        [ReducerMethod]
        public static BCThongTinTruongDiemState ReduceLoadPaginatedTruongDiemsAction(BCThongTinTruongDiemState state, LoadPaginatedTruongDiemsAction action) =>
            new(id: 1,
                maTruong: action.MaTruong ?? state.MaTruong,
                isLoading: true,
                errorMessage: null,
                selectedTruongDiem: state.SelectedTruongDiem,
                paginatedTruongDiems: state.PaginatedTruongDiems,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        [ReducerMethod]
        public static BCThongTinTruongDiemState ReduceLoadPaginatedTruongDiemsSuccessAction(BCThongTinTruongDiemState state, LoadPaginatedTruongDiemsSuccessAction action) =>
            new(id: 2,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                selectedTruongDiem: state.SelectedTruongDiem,
                paginatedTruongDiems: action.PaginatedTruongDiems,
                isInitialized: true,
                notificationMessage: "Đã tải danh sách lãnh đạo điểm thi thành công",
                notificationType: "success");

        [ReducerMethod]
        public static BCThongTinTruongDiemState ReduceLoadPaginatedTruongDiemsFailureAction(BCThongTinTruongDiemState state, LoadPaginatedTruongDiemsFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 3,
                 maTruong: state.MaTruong,
                 isLoading: false,
                 errorMessage: errorMsg,
                 selectedTruongDiem: state.SelectedTruongDiem,
                 paginatedTruongDiems: state.PaginatedTruongDiems,
                 isInitialized: state.IsInitialized,
                 notificationMessage: $"Lỗi tải danh sách lãnh đạo điểm thi: {action.ErrorMessage}",
                 notificationType: "error");
        }
        #endregion

        #region Select TruongDiem Reducers
        /// <summary>
        /// Reducer xử lý action chọn thông tin trường điểm
        /// </summary>
        [ReducerMethod]
        public static BCThongTinTruongDiemState ReduceSelectTruongDiemAction(BCThongTinTruongDiemState state, SelectTruongDiemAction action)
        {
            if (action.TruongDiem == null)
            {
                return state;
            }

            return new(id: 4,
                maTruong: action.TruongDiem.MaTruong,
                isLoading: false,
                errorMessage: null,
                selectedTruongDiem: action.TruongDiem,
                paginatedTruongDiems: state.PaginatedTruongDiems,
                isInitialized: state.IsInitialized,
                notificationMessage: "Đã chọn thông tin lãnh đạo điểm thi",
                notificationType: "success");
        }
        #endregion

        #region Update TruongDiem Reducers
        /// <summary>
        /// Reducer xử lý action cập nhật thông tin trường điểm
        /// </summary>
        [ReducerMethod]
        public static BCThongTinTruongDiemState ReduceUpdateTruongDiemAction(BCThongTinTruongDiemState state, UpdateTruongDiemAction action) =>
            new(id: 5,
                maTruong: state.MaTruong,
                isLoading: true,
                errorMessage: null,
                selectedTruongDiem: state.SelectedTruongDiem,
                paginatedTruongDiems: state.PaginatedTruongDiems,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        [ReducerMethod]
        public static BCThongTinTruongDiemState ReduceUpdateTruongDiemSuccessAction(BCThongTinTruongDiemState state, UpdateTruongDiemSuccessAction action)
        {
            if (action.TruongDiem == null)
            {
                return state;
            }

            // Cập nhật trường điểm trong danh sách phân trang
            var updatedPaginatedTruongDiems = state.PaginatedTruongDiems == null ? null :
                new KhaoThi_2024_net_client.Components.PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(
                    state.PaginatedTruongDiems.Items.Select(td =>
                        (td.MaTruong == action.TruongDiem.MaTruong && td.CCCD == action.TruongDiem.CCCD)
                            ? action.TruongDiem
                            : td
                    ),
                    state.PaginatedTruongDiems.TotalCount,
                    state.PaginatedTruongDiems.Page,
                    state.PaginatedTruongDiems.PageSize
                );

            return new(id: 6,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                selectedTruongDiem: action.TruongDiem, // Cập nhật thông tin đang chọn
                paginatedTruongDiems: updatedPaginatedTruongDiems,
                isInitialized: true,
                notificationMessage: "Cập nhật thông tin lãnh đạo điểm thi thành công",
                notificationType: "success");
        }

        [ReducerMethod]
        public static BCThongTinTruongDiemState ReduceUpdateTruongDiemFailureAction(BCThongTinTruongDiemState state, UpdateTruongDiemFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 7,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                selectedTruongDiem: state.SelectedTruongDiem,
                paginatedTruongDiems: state.PaginatedTruongDiems,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi cập nhật thông tin lãnh đạo điểm thi: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Utility Reducers
        /// <summary>
        /// Reducer xử lý action set loading
        /// </summary>
        [ReducerMethod]
        public static BCThongTinTruongDiemState ReduceSetLoadingAction(BCThongTinTruongDiemState state, SetLoadingAction action)
        {
            var errorMsg = string.IsNullOrEmpty(state.ErrorMessage) ? null : $"{state.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 8,
               maTruong: state.MaTruong,
               isLoading: action.IsLoading,
               errorMessage: errorMsg,
               selectedTruongDiem: state.SelectedTruongDiem,
               paginatedTruongDiems: state.PaginatedTruongDiems,
               isInitialized: state.IsInitialized,
               notificationMessage: state.NotificationMessage,
               notificationType: state.NotificationType
               );
        }

        /// <summary>
        /// Reducer xử lý action clear error
        /// </summary>
        [ReducerMethod]
        public static BCThongTinTruongDiemState ReduceClearErrorAction(BCThongTinTruongDiemState state, ClearErrorAction action) =>
            new(id: 9,
                maTruong: state.MaTruong,
                isLoading: state.IsLoading,
                errorMessage: null,
                selectedTruongDiem: state.SelectedTruongDiem,
                paginatedTruongDiems: state.PaginatedTruongDiems,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        [ReducerMethod]
        public static BCThongTinTruongDiemState ReduceShowNotificationAction(BCThongTinTruongDiemState state, ShowNotificationAction action)
        {
            var errorMsg = string.IsNullOrEmpty(state.ErrorMessage) ? null : $"{state.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 10,
                maTruong: state.MaTruong,
                isLoading: state.IsLoading,
                errorMessage: errorMsg,
                selectedTruongDiem: state.SelectedTruongDiem,
                paginatedTruongDiems: state.PaginatedTruongDiems,
                isInitialized: state.IsInitialized,
                notificationMessage: action.Message,
                notificationType: action.NotificationType
            );
        }
        #endregion
    }
}