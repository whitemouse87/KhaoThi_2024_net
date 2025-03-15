using Fluxor;
using KhaoThi_2024_net_client.Services.BC_1_ThongTinDonVi;
using static KhaoThi_2024_net_client.Services.BC_1_ThongTinDonVi.BCThongTinDonViAction;
namespace KhaoThi_2024_net_client.State.BC_1_ThongTinDonVi
{
    public class BCThongTinDonViReducers
    {

        #region Load Paginated đơn vị Reducers
        /// <summary>
        /// Reducer xử lý action tải danh sách người dùng phân trang
        /// </summary>
        [ReducerMethod]
        public static BCThongTinDonViState ReduceLoadPaginatedDonVisAction(BCThongTinDonViState state, LoadPaginatedDonVisAction action) =>
            new(id: 1,
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
        public static BCThongTinDonViState ReduceLoadPaginatedDonVisSuccessAction(BCThongTinDonViState state, LoadPaginatedDonVisSuccessAction action) =>
            new(id: 2,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: action.PaginatedDonVis, // Cập nhật giá trị paginatedDonVis từ action
                isInitialized: true,
                notificationMessage: "Đã tải danh sách đơn vị thành công",
                notificationType: "success");

        [ReducerMethod]
        public static BCThongTinDonViState ReduceLoadPaginatedDonVisFailureAction(BCThongTinDonViState state, LoadPaginatedDonVisFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 3,
                 maTruong: state.MaTruong,
                 isLoading: false,
                 errorMessage: errorMsg,
                 donVis: state.DonVis,
                 selectedDonVi: state.SelectedDonVi,
                 paginatedDonVis: state.PaginatedDonVis,
                 isInitialized: state.IsInitialized,
                 notificationMessage: $"Lỗi tải danh sách đơn vị: {action.ErrorMessage}",
                 notificationType: "error");
        }
        #endregion

        #region Load đơn vị Reducers
        /// <summary>
        /// Reducer xử lý action tải danh sách đơn vị
        /// </summary>
        [ReducerMethod]
        public static BCThongTinDonViState ReduceLoadDonVisAction(BCThongTinDonViState state, LoadDonVisAction action) =>
            new(
                id: 4,
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
        public static BCThongTinDonViState ReduceLoadDonVisSuccessAction(BCThongTinDonViState state, LoadDonVisSuccessAction action) =>
            new(id: 5,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                donVis: action.DonVis, // Cập nhật danh sách từ action
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: true,
                notificationMessage: "Đã tải thông tin đơn vị thành công",
                notificationType: "success");


        [ReducerMethod]
        public static BCThongTinDonViState ReduceLoadDonVisFailureAction(BCThongTinDonViState state, LoadDonVisFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 6,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi tải thông tin đơn vị: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Single Đơn vị Management Reducers
        /// <summary>
        /// Reducer xử lý action chọn đơn vị
        /// </summary>
        [ReducerMethod]
        public static BCThongTinDonViState ReduceSelectDonViAction(BCThongTinDonViState state, SelectDonViAction action)
        {
            // Kiểm tra action.DonVi có null không
            if (action.DonVi == null)
            {
                return state; // Giữ nguyên state nếu không có dữ liệu
            }

            return new(id: 7,
                maTruong: action.DonVi.MaTruong,
                isLoading: false,
                errorMessage: null,
                donVis: state.DonVis,
                selectedDonVi: action.DonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: "Đã chọn đơn vị",
                notificationType: "success");
        }

        /// <summary>
        /// Reducer xử lý action lấy thông tin đơn vị theo mã trường
        /// </summary>
        [ReducerMethod]
        public static BCThongTinDonViState ReduceGetDonViByMaTruongAction(BCThongTinDonViState state, GetDonViByMaTruongAction action) =>
            new(id: 8,
                maTruong: action.MaTruong, // Lấy mã trường từ action
                isLoading: true,
                errorMessage: null,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        [ReducerMethod]
        public static BCThongTinDonViState ReduceGetDonViByMaTruongSuccessAction(BCThongTinDonViState state, GetDonViByMaTruongSuccessAction action)
        {
            // Kiểm tra action.DonVi có null không
            if (action.DonVi == null)
            {
                return new(id: 9,
                    maTruong: state.MaTruong,
                    isLoading: false,
                    errorMessage: "Không tìm thấy dữ liệu đơn vị",
                    donVis: state.DonVis,
                    selectedDonVi: null,
                    paginatedDonVis: state.PaginatedDonVis,
                    isInitialized: true,
                    notificationMessage: "Không tìm thấy thông tin đơn vị",
                    notificationType: "error");
            }

            // Log để debug
            Console.WriteLine($"ReduceGetDonViByMaTruongSuccessAction: Nhận được đơn vị với MaTruong={action.DonVi.MaTruong}");

            return new(id: 9,
                maTruong: action.DonVi.MaTruong,
                isLoading: false,
                errorMessage: null,
                donVis: state.DonVis,
                selectedDonVi: action.DonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: true,
                notificationMessage: "Đã tải thông tin đơn vị thành công",
                notificationType: "success");
        }

        [ReducerMethod]
        public static BCThongTinDonViState ReduceGetDonViByMaTruongFailureAction(BCThongTinDonViState state, GetDonViByMaTruongFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            //Console.WriteLine($"ReduceGetDonViByMaTruongFailureAction: {errorMsg}");

            return new(id: 10,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                donVis: state.DonVis,
                selectedDonVi: null, // Reset selectedDonVi khi có lỗi
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi tải thông tin đơn vị: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Update đơn vị Reducers
        /// <summary>
        /// Reducer xử lý action cập nhật đơn vị
        /// </summary>
        [ReducerMethod]
        public static BCThongTinDonViState ReduceUpdateDonViAction(BCThongTinDonViState state, UpdateDonViAction action) =>
            new(id: 14,
                maTruong: state.MaTruong,
                isLoading: true,
                errorMessage: null,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi, // Giữ đơn vị đã chọn
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        [ReducerMethod]
        public static BCThongTinDonViState ReduceUpdateDonViSuccessAction(BCThongTinDonViState state, UpdateDonViSuccessAction action)
        {
            // Kiểm tra action.DonVi có null không
            if (action.DonVi == null)
            {
                return state; // Giữ nguyên state nếu không có dữ liệu
            }

            return new(id: 15,
                maTruong: action.DonVi.MaTruong,
                isLoading: false,
                errorMessage: null,
                donVis: state.DonVis,
                selectedDonVi: action.DonVi, // Cập nhật đơn vị đã chọn
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: true,
                notificationMessage: "Cập nhật thông tin đơn vị thành công",
                notificationType: "success");
        }

        [ReducerMethod]
        public static BCThongTinDonViState ReduceUpdateDonViFailureAction(BCThongTinDonViState state, UpdateDonViFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 16,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi, // Giữ đơn vị đã chọn
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi cập nhật đơn vị: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Utility Reducers
        /// <summary>
        /// Reducer xử lý action set loading
        /// </summary>
        [ReducerMethod]
        public static BCThongTinDonViState ReduceSetLoadingAction(BCThongTinDonViState state, SetLoadingAction action)
        {
            var errorMsg = string.IsNullOrEmpty(state.ErrorMessage) ? null : $"{state.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 11,
               maTruong: state.MaTruong,
               isLoading: action.IsLoading,
               errorMessage: errorMsg,
               donVis: state.DonVis,
               selectedDonVi: state.SelectedDonVi,
               paginatedDonVis: state.PaginatedDonVis,
               isInitialized: state.IsInitialized,
               notificationMessage: state.NotificationMessage,
               notificationType: state.NotificationType
               );
        }

        /// <summary>
        /// Reducer xử lý action clear error
        /// </summary>
        [ReducerMethod]
        public static BCThongTinDonViState ReduceClearErrorAction(BCThongTinDonViState state, ClearErrorAction action) =>
            new(id: 12,
                maTruong: state.MaTruong,
                isLoading: state.IsLoading,
                errorMessage: null,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        [ReducerMethod]
        public static BCThongTinDonViState ReduceShowNotificationAction(BCThongTinDonViState state, ShowNotificationAction action)
        {
            var errorMsg = string.IsNullOrEmpty(state.ErrorMessage) ? null : $"{state.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 13,
                maTruong: state.MaTruong,
                isLoading: state.IsLoading,
                errorMessage: errorMsg,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: action.Message,
                notificationType: action.Type
            );
        }
        #endregion
    }
}