using Fluxor;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Services.BC_2_NhomMonDonVi;
using static KhaoThi_2024_net_client.Services.BC_2_NhomMonDonVi.BCNhomMonDonViActions;
namespace KhaoThi_2024_net_client.State.BC_2_NhomMonDonVi
{
    public class BCNhomMonDonViReducers
    {
        #region Load Paginated NhomMon Reducers
        [ReducerMethod]
        public static BCNhomMonDonViState ReduceLoadPaginatedNhomMonsAction(BCNhomMonDonViState state, LoadPaginatedNhomMonsAction action) =>
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
        public static BCNhomMonDonViState ReduceLoadPaginatedNhomMonsSuccessAction(BCNhomMonDonViState state, LoadPaginatedNhomMonsSuccessAction action) =>
            new(id: 2,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: action.PaginatedNhomMons,
                isInitialized: true,
                notificationMessage: "Đã tải danh sách nhóm môn thành công",
                notificationType: "success");

        [ReducerMethod]
        public static BCNhomMonDonViState ReduceLoadPaginatedNhomMonsFailureAction(BCNhomMonDonViState state, LoadPaginatedNhomMonsFailureAction action)
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
                 notificationMessage: $"Lỗi tải danh sách nhóm môn: {action.ErrorMessage}",
                 notificationType: "error");
        }
        #endregion
        #region Load NhomMon Reducers
        [ReducerMethod]
        public static BCNhomMonDonViState ReduceLoadNhomMonsAction(BCNhomMonDonViState state, LoadNhomMonsAction action) =>
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
        public static BCNhomMonDonViState ReduceLoadNhomMonsSuccessAction(BCNhomMonDonViState state, LoadNhomMonsSuccessAction action) =>
            new(id: 5,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                donVis: action.NhomMons,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: true,
                notificationMessage: "Đã tải thông tin nhóm môn thành công",
                notificationType: "success");

        [ReducerMethod]
        public static BCNhomMonDonViState ReduceLoadNhomMonsFailureAction(BCNhomMonDonViState state, LoadNhomMonsFailureAction action)
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
                notificationMessage: $"Lỗi tải thông tin nhóm môn: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion
        #region Select NhomMon Reducers
        [ReducerMethod]
        public static BCNhomMonDonViState ReduceSelectNhomMonAction(BCNhomMonDonViState state, SelectNhomMonAction action)
        {
            if (action.NhomMon == null)
            {
                return state;
            }

            return new(id: 7,
                maTruong: action.NhomMon.MaTruong,
                isLoading: false,
                errorMessage: null,
                donVis: state.DonVis,
                selectedDonVi: action.NhomMon,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: "Đã chọn nhóm môn",
                notificationType: "success");
        }
        #endregion


        #region Get NhomMon By MaTruong Reducers
        [ReducerMethod]
        public static BCNhomMonDonViState ReduceGetNhomMonByMaTruongAction(BCNhomMonDonViState state, GetNhomMonByMaTruongAction action) =>
            new(id: 8,
                maTruong: action.MaTruong,
                isLoading: true,
                errorMessage: null,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        [ReducerMethod]
        public static BCNhomMonDonViState ReduceGetNhomMonByMaTruongSuccessAction(BCNhomMonDonViState state, GetNhomMonByMaTruongSuccessAction action)
        {
            if (action.NhomMons == null || !action.NhomMons.Any())
            {
                return new(id: 9,
                    maTruong: state.MaTruong,
                    isLoading: false,
                    errorMessage: null,
                    donVis: Enumerable.Empty<KhaoThi_2_THPT_NhomMon_DonViModel>(),
                    selectedDonVi: state.SelectedDonVi,
                    paginatedDonVis: state.PaginatedDonVis,
                    isInitialized: true,
                    notificationMessage: "Không tìm thấy nhóm môn cho trường này",
                    notificationType: "info");
            }

            return new(id: 9,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                donVis: action.NhomMons,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: true,
                notificationMessage: "Đã tải danh sách nhóm môn thành công",
                notificationType: "success");
        }

        [ReducerMethod]
        public static BCNhomMonDonViState ReduceGetNhomMonByMaTruongFailureAction(BCNhomMonDonViState state, GetNhomMonByMaTruongFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 10,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi tải nhóm môn theo mã trường: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Create NhomMon Reducers
        [ReducerMethod]
        public static BCNhomMonDonViState ReduceCreateNhomMonAction(BCNhomMonDonViState state, CreateNhomMonAction action) =>
            new(id: 11,
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
        public static BCNhomMonDonViState ReduceCreateNhomMonSuccessAction(BCNhomMonDonViState state, CreateNhomMonSuccessAction action)
        {
            // Không thay đổi danh sách hiện tại vì cần thông tin đầy đủ của nhóm môn mới được tạo
            // Sẽ tải lại danh sách ở effect
            return new(id: 12,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: "Tạo mới nhóm môn thành công",
                notificationType: "success");
        }

        [ReducerMethod]
        public static BCNhomMonDonViState ReduceCreateNhomMonFailureAction(BCNhomMonDonViState state, CreateNhomMonFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 13,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi tạo nhóm môn: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Update NhomMon Reducers
        [ReducerMethod]
        public static BCNhomMonDonViState ReduceUpdateNhomMonAction(BCNhomMonDonViState state, UpdateNhomMonAction action) =>
            new(id: 14,
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
        public static BCNhomMonDonViState ReduceUpdateNhomMonSuccessAction(BCNhomMonDonViState state, UpdateNhomMonSuccessAction action)
        {
            if (action.NhomMon == null)
            {
                return state;
            }

            // Cập nhật nhóm môn trong danh sách
            var updatedDonVis = state.DonVis?.Select(d => d.ID == action.NhomMon.ID ? action.NhomMon : d);

            // Cập nhật nhóm môn trong danh sách phân trang
            var updatedPaginatedDonVis = state.PaginatedDonVis == null ? null :
                new PaginatedResult<KhaoThi_2_THPT_NhomMon_DonViModel>(
                    state.PaginatedDonVis.Items.Select(d => d.ID == action.NhomMon.ID ? action.NhomMon : d),
                    state.PaginatedDonVis.TotalCount,
                    state.PaginatedDonVis.Page,
                    state.PaginatedDonVis.PageSize
                );

            return new(id: 15,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                donVis: updatedDonVis,
                selectedDonVi: action.NhomMon,
                paginatedDonVis: updatedPaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: "Cập nhật nhóm môn thành công",
                notificationType: "success");
        }

        [ReducerMethod]
        public static BCNhomMonDonViState ReduceUpdateNhomMonFailureAction(BCNhomMonDonViState state, UpdateNhomMonFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 16,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi cập nhật nhóm môn: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Delete NhomMon Reducers
        [ReducerMethod]
        public static BCNhomMonDonViState ReduceDeleteNhomMonAction(BCNhomMonDonViState state, DeleteNhomMonAction action) =>
            new(id: 17,
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
        public static BCNhomMonDonViState ReduceDeleteNhomMonSuccessAction(BCNhomMonDonViState state, DeleteNhomMonSuccessAction action)
        {
            // Xóa nhóm môn khỏi danh sách
            var updatedDonVis = state.DonVis?.Where(d => d.ID != action.Id);

            // Xóa nhóm môn khỏi danh sách phân trang
            var updatedPaginatedDonVis = state.PaginatedDonVis == null ? null :
                new PaginatedResult<KhaoThi_2_THPT_NhomMon_DonViModel>(
                    state.PaginatedDonVis.Items.Where(d => d.ID != action.Id),
                    state.PaginatedDonVis.TotalCount - 1,
                    state.PaginatedDonVis.Page,
                    state.PaginatedDonVis.PageSize
                );

            return new(id: 18,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                donVis: updatedDonVis,
                selectedDonVi: state.SelectedDonVi?.ID == action.Id ? null : state.SelectedDonVi,
                paginatedDonVis: updatedPaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: "Xóa nhóm môn thành công",
                notificationType: "success");
        }

        [ReducerMethod]
        public static BCNhomMonDonViState ReduceDeleteNhomMonFailureAction(BCNhomMonDonViState state, DeleteNhomMonFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 19,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi xóa nhóm môn: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Check NhomMon Exist Reducers
        [ReducerMethod]
        public static BCNhomMonDonViState ReduceCheckNhomMonExistAction(BCNhomMonDonViState state, CheckNhomMonExistAction action) =>
            new(id: 20,
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
        public static BCNhomMonDonViState ReduceCheckNhomMonExistSuccessAction(BCNhomMonDonViState state, CheckNhomMonExistSuccessAction action) =>
            state; // Không cần thay đổi state vì kết quả được xử lý trực tiếp bởi component

        [ReducerMethod]
        public static BCNhomMonDonViState ReduceCheckNhomMonExistFailureAction(BCNhomMonDonViState state, CheckNhomMonExistFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 21,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi kiểm tra nhóm môn: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Change Lock Reducers
        [ReducerMethod]
        public static BCNhomMonDonViState ReduceChangeLockAction(BCNhomMonDonViState state, ChangeLockAction action) =>
            new(id: 22,
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
        public static BCNhomMonDonViState ReduceChangeLockSuccessAction(BCNhomMonDonViState state, ChangeLockSuccessAction action)
        {
            // Cập nhật trạng thái khóa của nhóm môn trong danh sách
            var updatedDonVis = state.DonVis?.Select(d =>
            {
                if (d.ID == action.Id)
                {
                    var updatedItem = new KhaoThi_2_THPT_NhomMon_DonViModel
                    {
                        ID = d.ID,
                        MaTruong = d.MaTruong,
                        TenNhom = d.TenNhom,
                        MonLuaChon_1 = d.MonLuaChon_1,
                        MonLuaChon_2 = d.MonLuaChon_2,
                        SoLuong = d.SoLuong,
                        Lock = !d.Lock // Đảo ngược trạng thái Lock
                    };
                    return updatedItem;
                }
                return d;
            });

            // Cập nhật trạng thái khóa của nhóm môn trong danh sách phân trang
            var updatedPaginatedDonVis = state.PaginatedDonVis == null ? null :
                new PaginatedResult<KhaoThi_2_THPT_NhomMon_DonViModel>(
                    state.PaginatedDonVis.Items.Select(d =>
                    {
                        if (d.ID == action.Id)
                        {
                            var updatedItem = new KhaoThi_2_THPT_NhomMon_DonViModel
                            {
                                ID = d.ID,
                                MaTruong = d.MaTruong,
                                TenNhom = d.TenNhom,
                                MonLuaChon_1 = d.MonLuaChon_1,
                                MonLuaChon_2 = d.MonLuaChon_2,
                                SoLuong = d.SoLuong,
                                Lock = !d.Lock // Đảo ngược trạng thái Lock
                            };
                            return updatedItem;
                        }
                        return d;
                    }),
                    state.PaginatedDonVis.TotalCount,
                    state.PaginatedDonVis.Page,
                    state.PaginatedDonVis.PageSize
                );

            // Cập nhật trạng thái khóa của nhóm môn đã chọn
            var updatedSelectedDonVi = state.SelectedDonVi?.ID == action.Id
                ? new KhaoThi_2_THPT_NhomMon_DonViModel
                {
                    ID = state.SelectedDonVi.ID,
                    MaTruong = state.SelectedDonVi.MaTruong,
                    TenNhom = state.SelectedDonVi.TenNhom,
                    MonLuaChon_1 = state.SelectedDonVi.MonLuaChon_1,
                    MonLuaChon_2 = state.SelectedDonVi.MonLuaChon_2,
                    SoLuong = state.SelectedDonVi.SoLuong,
                    Lock = !state.SelectedDonVi.Lock // Đảo ngược trạng thái Lock
                }
                : state.SelectedDonVi;

            return new(id: 23,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                donVis: updatedDonVis,
                selectedDonVi: updatedSelectedDonVi,
                paginatedDonVis: updatedPaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: "Thay đổi trạng thái khóa thành công",
                notificationType: "success");
        }

        [ReducerMethod]
        public static BCNhomMonDonViState ReduceChangeLockFailureAction(BCNhomMonDonViState state, ChangeLockFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 24,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi thay đổi trạng thái khóa: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Utility Reducers
        [ReducerMethod]
        public static BCNhomMonDonViState ReduceSetLoadingAction(BCNhomMonDonViState state, SetLoadingAction action)
        {
            var errorMsg = string.IsNullOrEmpty(state.ErrorMessage) ? null : $"{state.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 25,
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

        [ReducerMethod]
        public static BCNhomMonDonViState ReduceClearErrorAction(BCNhomMonDonViState state, ClearErrorAction action) =>
            new(id: 26,
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
        public static BCNhomMonDonViState ReduceShowNotificationAction(BCNhomMonDonViState state, ShowNotificationAction action)
        {
            var errorMsg = string.IsNullOrEmpty(state.ErrorMessage) ? null : $"{state.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 27,
                maTruong: state.MaTruong,
                isLoading: state.IsLoading,
                errorMessage: errorMsg,
                donVis: state.DonVis,
                selectedDonVi: state.SelectedDonVi,
                paginatedDonVis: state.PaginatedDonVis,
                isInitialized: state.IsInitialized,
                notificationMessage: action.Message,
                notificationType: action.NotificationType
            );
        }
        #endregion
    }
}
