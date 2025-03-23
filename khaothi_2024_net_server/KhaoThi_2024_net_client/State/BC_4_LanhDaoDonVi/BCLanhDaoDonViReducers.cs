using Fluxor;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Services.BC_4_LanhDaoDonVi;
using static KhaoThi_2024_net_client.Services.BC_4_LanhDaoDonVi.BCLanhDaoDonViAction;

namespace KhaoThi_2024_net_client.State.BC_4_LanhDaoDonVi
{
    public class BCLanhDaoDonViReducers
    {
        #region Load Paginated LanhDao Reducers
        /// <summary>
        /// Reducer xử lý action tải danh sách lãnh đạo phân trang
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceLoadPaginatedLanhDaosAction(BCLanhDaoDonViState state, LoadPaginatedLanhDaosAction action) =>
            new(id: 1,
                maTruong: state.MaTruong,
                isLoading: true,
                errorMessage: null,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        /// <summary>
        /// Reducer xử lý khi tải danh sách lãnh đạo phân trang thành công
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceLoadPaginatedLanhDaosSuccessAction(BCLanhDaoDonViState state, LoadPaginatedLanhDaosSuccessAction action) =>
            new(id: 2,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: action.PaginatedLanhDaos,
                isInitialized: true,
                notificationMessage: "Đã tải danh sách lãnh đạo thành công",
                notificationType: "success");

        /// <summary>
        /// Reducer xử lý khi tải danh sách lãnh đạo phân trang thất bại
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceLoadPaginatedLanhDaosFailureAction(BCLanhDaoDonViState state, LoadPaginatedLanhDaosFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 3,
                 maTruong: state.MaTruong,
                 isLoading: false,
                 errorMessage: errorMsg,
                 lanhDaos: state.LanhDaos,
                 selectedLanhDao: state.SelectedLanhDao,
                 paginatedLanhDaos: state.PaginatedLanhDaos,
                 isInitialized: state.IsInitialized,
                 notificationMessage: $"Lỗi tải danh sách lãnh đạo: {action.ErrorMessage}",
                 notificationType: "error");
        }
        #endregion

        #region Load LanhDao Reducers
        /// <summary>
        /// Reducer xử lý action tải tất cả danh sách lãnh đạo
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceLoadLanhDaosAction(BCLanhDaoDonViState state, LoadLanhDaosAction action) =>
            new(
                id: 4,
                maTruong: state.MaTruong,
                isLoading: true,
                errorMessage: null,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        /// <summary>
        /// Reducer xử lý khi tải tất cả danh sách lãnh đạo thành công
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceLoadLanhDaosSuccessAction(BCLanhDaoDonViState state, LoadLanhDaosSuccessAction action) =>
            new(id: 5,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                lanhDaos: action.LanhDaos, // Cập nhật danh sách từ action
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: true,
                notificationMessage: "Đã tải thông tin lãnh đạo thành công",
                notificationType: "success");

        /// <summary>
        /// Reducer xử lý khi tải tất cả danh sách lãnh đạo thất bại
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceLoadLanhDaosFailureAction(BCLanhDaoDonViState state, LoadLanhDaosFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 6,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi tải thông tin lãnh đạo: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Select LanhDao Reducers
        /// <summary>
        /// Reducer xử lý action chọn lãnh đạo
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceSelectLanhDaoAction(BCLanhDaoDonViState state, SelectLanhDaoAction action)
        {
            // Kiểm tra action.LanhDao có null không
            if (action.LanhDao == null)
            {
                return state; // Giữ nguyên state nếu không có dữ liệu
            }

            return new(id: 7,
                maTruong: action.LanhDao.MaTruong,
                isLoading: false,
                errorMessage: null,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: action.LanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: "Đã chọn lãnh đạo",
                notificationType: "success");
        }
        #endregion

        #region Get LanhDao By MaTruong Reducers
        /// <summary>
        /// Reducer xử lý action lấy danh sách lãnh đạo theo mã trường
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceGetLanhDaoByMaTruongAction(BCLanhDaoDonViState state, GetLanhDaoByMaTruongAction action) =>
            new(id: 8,
                maTruong: action.MaTruong, // Lấy mã trường từ action
                isLoading: true,
                errorMessage: null,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        /// <summary>
        /// Reducer xử lý khi lấy danh sách lãnh đạo theo mã trường thành công
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceGetLanhDaoByMaTruongSuccessAction(BCLanhDaoDonViState state, GetLanhDaoByMaTruongSuccessAction action)
        {
            if (action.LanhDaos == null || !action.LanhDaos.Any())
            {
                return new(id: 9,
                    maTruong: state.MaTruong,
                    isLoading: false,
                    errorMessage: null,
                    lanhDaos: Enumerable.Empty<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(),
                    selectedLanhDao: state.SelectedLanhDao,
                    paginatedLanhDaos: state.PaginatedLanhDaos,
                    isInitialized: true,
                    notificationMessage: "Không tìm thấy lãnh đạo cho trường này",
                    notificationType: "info");
            }

            return new(id: 9,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                lanhDaos: action.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: true,
                notificationMessage: "Đã tải danh sách lãnh đạo thành công",
                notificationType: "success");
        }

        /// <summary>
        /// Reducer xử lý khi lấy danh sách lãnh đạo theo mã trường thất bại
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceGetLanhDaoByMaTruongFailureAction(BCLanhDaoDonViState state, GetLanhDaoByMaTruongFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 10,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi tải lãnh đạo theo mã trường: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Get LanhDao By MaTruong And CCCD Reducers
        /// <summary>
        /// Reducer xử lý action lấy thông tin lãnh đạo theo mã trường và CCCD
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceGetLanhDaoByMaTruongAndCCCDAction(BCLanhDaoDonViState state, GetLanhDaoByMaTruongAndCCCDAction action) =>
            new(id: 11,
                maTruong: action.MaTruong,
                isLoading: true,
                errorMessage: null,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        /// <summary>
        /// Reducer xử lý khi lấy thông tin lãnh đạo theo mã trường và CCCD thành công
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceGetLanhDaoByMaTruongAndCCCDSuccessAction(BCLanhDaoDonViState state, GetLanhDaoByMaTruongAndCCCDSuccessAction action)
        {
            // Kiểm tra action.LanhDao có null không
            if (action.LanhDao == null)
            {
                return new(id: 12,
                    maTruong: state.MaTruong,
                    isLoading: false,
                    errorMessage: "Không tìm thấy dữ liệu lãnh đạo",
                    lanhDaos: state.LanhDaos,
                    selectedLanhDao: null,
                    paginatedLanhDaos: state.PaginatedLanhDaos,
                    isInitialized: true,
                    notificationMessage: "Không tìm thấy thông tin lãnh đạo",
                    notificationType: "error");
            }

            return new(id: 12,
                maTruong: action.LanhDao.MaTruong,
                isLoading: false,
                errorMessage: null,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: action.LanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: true,
                notificationMessage: "Đã tải thông tin lãnh đạo thành công",
                notificationType: "success");
        }

        /// <summary>
        /// Reducer xử lý khi lấy thông tin lãnh đạo theo mã trường và CCCD thất bại
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceGetLanhDaoByMaTruongAndCCCDFailureAction(BCLanhDaoDonViState state, GetLanhDaoByMaTruongAndCCCDFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 13,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: null, // Reset selectedLanhDao khi có lỗi
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi tải thông tin lãnh đạo: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Create LanhDao Reducers
        /// <summary>
        /// Reducer xử lý action tạo mới lãnh đạo
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceCreateLanhDaoAction(BCLanhDaoDonViState state, CreateLanhDaoAction action) =>
            new(id: 14,
                maTruong: state.MaTruong,
                isLoading: true,
                errorMessage: null,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        /// <summary>
        /// Reducer xử lý khi tạo mới lãnh đạo thành công
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceCreateLanhDaoSuccessAction(BCLanhDaoDonViState state, CreateLanhDaoSuccessAction action)
        {
            // Không thay đổi danh sách hiện tại vì cần thông tin đầy đủ của lãnh đạo mới được tạo
            // Sẽ tải lại danh sách ở effect
            return new(id: 15,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: "Tạo mới lãnh đạo thành công",
                notificationType: "success");
        }

        /// <summary>
        /// Reducer xử lý khi tạo mới lãnh đạo thất bại
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceCreateLanhDaoFailureAction(BCLanhDaoDonViState state, CreateLanhDaoFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 16,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg, // Lưu thông báo lỗi từ action
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi tạo lãnh đạo: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Update LanhDao Reducers
        /// <summary>
        /// Reducer xử lý action cập nhật lãnh đạo
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceUpdateLanhDaoAction(BCLanhDaoDonViState state, UpdateLanhDaoAction action) =>
            new(id: 17,
                maTruong: state.MaTruong,
                isLoading: true,
                errorMessage: null,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao, // Giữ lãnh đạo đã chọn
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        /// <summary>
        /// Reducer xử lý khi cập nhật lãnh đạo thành công
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceUpdateLanhDaoSuccessAction(BCLanhDaoDonViState state, UpdateLanhDaoSuccessAction action)
        {
            // Kiểm tra action.LanhDao có null không
            if (action.LanhDao == null)
            {
                return state; // Giữ nguyên state nếu không có dữ liệu
            }

            // Cập nhật lãnh đạo trong danh sách
            var updatedLanhDaos = state.LanhDaos?.Select(ld =>
                (ld.MaTruong == action.LanhDao.MaTruong && ld.CCCD == action.LanhDao.CCCD) ? action.LanhDao : ld);

            // Cập nhật lãnh đạo trong danh sách phân trang
            var updatedPaginatedLanhDaos = state.PaginatedLanhDaos == null ? null :
                new PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(
                    state.PaginatedLanhDaos.Items.Select(ld =>
                        (ld.MaTruong == action.LanhDao.MaTruong && ld.CCCD == action.LanhDao.CCCD) ? action.LanhDao : ld),
                    state.PaginatedLanhDaos.TotalCount,
                    state.PaginatedLanhDaos.Page,
                    state.PaginatedLanhDaos.PageSize
                );

            return new(id: 18,
                maTruong: action.LanhDao.MaTruong,
                isLoading: false,
                errorMessage: null,
                lanhDaos: updatedLanhDaos,
                selectedLanhDao: action.LanhDao, // Cập nhật lãnh đạo đã chọn
                paginatedLanhDaos: updatedPaginatedLanhDaos,
                isInitialized: true,
                notificationMessage: "Cập nhật thông tin lãnh đạo thành công",
                notificationType: "success");
        }

        /// <summary>
        /// Reducer xử lý khi cập nhật lãnh đạo thất bại
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceUpdateLanhDaoFailureAction(BCLanhDaoDonViState state, UpdateLanhDaoFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 19,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao, // Giữ lãnh đạo đã chọn
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi cập nhật lãnh đạo: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Delete LanhDao Reducers
        /// <summary>
        /// Reducer xử lý action xóa lãnh đạo
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceDeleteLanhDaoAction(BCLanhDaoDonViState state, DeleteLanhDaoAction action) =>
            new(id: 20,
                maTruong: state.MaTruong,
                isLoading: true,
                errorMessage: null,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        /// <summary>
        /// Reducer xử lý khi xóa lãnh đạo thành công
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceDeleteLanhDaoSuccessAction(BCLanhDaoDonViState state, DeleteLanhDaoSuccessAction action)
        {
            // Xóa lãnh đạo khỏi danh sách
            var updatedLanhDaos = state.LanhDaos?.Where(ld =>
                !(ld.MaTruong == action.MaTruong && ld.CCCD == action.CCCD));

            // Xóa lãnh đạo khỏi danh sách phân trang
            var updatedPaginatedLanhDaos = state.PaginatedLanhDaos == null ? null :
                new PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>(
                    state.PaginatedLanhDaos.Items.Where(ld =>
                        !(ld.MaTruong == action.MaTruong && ld.CCCD == action.CCCD)),
                    state.PaginatedLanhDaos.TotalCount - 1,
                    state.PaginatedLanhDaos.Page,
                    state.PaginatedLanhDaos.PageSize
                );

            return new(id: 21,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                lanhDaos: updatedLanhDaos,
                selectedLanhDao: (state.SelectedLanhDao?.MaTruong == action.MaTruong && state.SelectedLanhDao?.CCCD == action.CCCD)
                                ? null : state.SelectedLanhDao,
                paginatedLanhDaos: updatedPaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: "Xóa lãnh đạo thành công",
                notificationType: "success");
        }

        /// <summary>
        /// Reducer xử lý khi xóa lãnh đạo thất bại
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceDeleteLanhDaoFailureAction(BCLanhDaoDonViState state, DeleteLanhDaoFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 22,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi xóa lãnh đạo: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Check LanhDao Exist Reducers
        /// <summary>
        /// Reducer xử lý action kiểm tra tồn tại lãnh đạo
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceCheckLanhDaoExistAction(BCLanhDaoDonViState state, CheckLanhDaoExistAction action) =>
            new(id: 23,
                maTruong: state.MaTruong,
                isLoading: true,
                errorMessage: null,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        /// <summary>
        /// Reducer xử lý khi kiểm tra tồn tại lãnh đạo thành công
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceCheckLanhDaoExistSuccessAction(BCLanhDaoDonViState state, CheckLanhDaoExistSuccessAction action) =>
            state; // Không cần thay đổi state vì kết quả được xử lý trực tiếp bởi component

        /// <summary>
        /// Reducer xử lý khi kiểm tra tồn tại lãnh đạo thất bại
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceCheckLanhDaoExistFailureAction(BCLanhDaoDonViState state, CheckLanhDaoExistFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 24,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi kiểm tra lãnh đạo: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Check LanhDao Empty Reducers
        /// <summary>
        /// Reducer xử lý action kiểm tra danh sách lãnh đạo rỗng
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceCheckLanhDaoEmptyAction(BCLanhDaoDonViState state, CheckLanhDaoEmptyAction action) =>
            new(id: 28,
                maTruong: action.MaTruong,
                isLoading: true,
                errorMessage: null,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        /// <summary>
        /// Reducer xử lý khi kiểm tra danh sách lãnh đạo rỗng thành công
        /// </summary>

        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceCheckLanhDaoEmptySuccessAction(BCLanhDaoDonViState state, CheckLanhDaoEmptySuccessAction action)
        {
            // Nếu danh sách lãnh đạo rỗng, cập nhật state với thông báo lỗi
            if (action.IsEmpty)
            {
                return new(id: 30,
                    maTruong: state.MaTruong,
                    isLoading: false,
                    errorMessage: "Chưa có thông tin lãnh đạo",
                    lanhDaos: state.LanhDaos, // Giữ nguyên danh sách (rỗng)
                    selectedLanhDao: state.SelectedLanhDao,
                    paginatedLanhDaos: state.PaginatedLanhDaos,
                    isInitialized: true,
                    notificationMessage: "Trường của bạn chưa khai báo thông tin lãnh đạo. Vui lòng quay lại Bước 3 để khai báo thông tin lãnh đạo trước khi tiếp tục.",
                    notificationType: "success"); // Đặt kiểu thông báo là error để hiển thị màu đỏ
            }

            // Nếu có dữ liệu, giữ nguyên state
            return state;
        }

        /// <summary>
        /// Reducer xử lý khi kiểm tra danh sách lãnh đạo rỗng thất bại
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceCheckLanhDaoEmptyFailureAction(BCLanhDaoDonViState state, CheckLanhDaoEmptyFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 29,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi kiểm tra danh sách lãnh đạo: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Utility Reducers
        /// <summary>
        /// Reducer xử lý action set loading
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceSetLoadingAction(BCLanhDaoDonViState state, SetLoadingAction action)
        {
            var errorMsg = string.IsNullOrEmpty(state.ErrorMessage) ? null : $"{state.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 25,
               maTruong: state.MaTruong,
               isLoading: action.IsLoading,
               errorMessage: errorMsg,
               lanhDaos: state.LanhDaos,
               selectedLanhDao: state.SelectedLanhDao,
               paginatedLanhDaos: state.PaginatedLanhDaos,
               isInitialized: state.IsInitialized,
               notificationMessage: state.NotificationMessage,
               notificationType: state.NotificationType
               );
        }

        /// <summary>
        /// Reducer xử lý action clear error
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceClearErrorAction(BCLanhDaoDonViState state, ClearErrorAction action) =>
            new(id: 26,
                maTruong: state.MaTruong,
                isLoading: state.IsLoading,
                errorMessage: null,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        /// <summary>
        /// Reducer xử lý action hiển thị thông báo
        /// </summary>
        [ReducerMethod]
        public static BCLanhDaoDonViState ReduceShowNotificationAction(BCLanhDaoDonViState state, ShowNotificationAction action)
        {
            var errorMsg = string.IsNullOrEmpty(state.ErrorMessage) ? null : $"{state.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 27,
                maTruong: state.MaTruong,
                isLoading: state.IsLoading,
                errorMessage: errorMsg,
                lanhDaos: state.LanhDaos,
                selectedLanhDao: state.SelectedLanhDao,
                paginatedLanhDaos: state.PaginatedLanhDaos,
                isInitialized: state.IsInitialized,
                notificationMessage: action.Message,
                notificationType: action.NotificationType
            );
        }
        #endregion
    }
}
