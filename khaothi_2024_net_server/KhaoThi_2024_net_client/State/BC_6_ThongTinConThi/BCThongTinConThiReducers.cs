using Fluxor;
using khaothi_2024_net_client.Services.BC_5_ThongTinConThi;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using static khaothi_2024_net_client.Services.BC_5_ThongTinConThi.BCThongTinConThiAction;

namespace khaothi_2024_net_client.State.BC_5_ThongTinConThi
{
    public class BCThongTinConThiReducers
    {
        #region Load Paginated ConThi Reducers
        /// <summary>
        /// Reducer xử lý action tải danh sách con thi phân trang
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceLoadPaginatedConThisAction(BCThongTinConThiState state, LoadPaginatedConThisAction action) =>
            new(id: 1,
                maTruong: state.MaTruong,
                isLoading: true,
                errorMessage: null,
                conThis: state.ConThis,
                selectedConThi: state.SelectedConThi,
                paginatedConThis: state.PaginatedConThis,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        /// <summary>
        /// Reducer xử lý khi tải danh sách con thi phân trang thành công
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceLoadPaginatedConThisSuccessAction(BCThongTinConThiState state, LoadPaginatedConThisSuccessAction action) =>
            new(id: 2,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                conThis: state.ConThis,
                selectedConThi: state.SelectedConThi,
                paginatedConThis: action.PaginatedConThis,
                isInitialized: true,
                notificationMessage: "Đã tải danh sách con thi thành công",
                notificationType: "success");

        /// <summary>
        /// Reducer xử lý khi tải danh sách con thi phân trang thất bại
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceLoadPaginatedConThisFailureAction(BCThongTinConThiState state, LoadPaginatedConThisFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 3,
                 maTruong: state.MaTruong,
                 isLoading: false,
                 errorMessage: errorMsg,
                 conThis: state.ConThis,
                 selectedConThi: state.SelectedConThi,
                 paginatedConThis: state.PaginatedConThis,
                 isInitialized: state.IsInitialized,
                 notificationMessage: $"Lỗi tải danh sách con thi: {action.ErrorMessage}",
                 notificationType: "error");
        }
        #endregion

        #region Load ConThi Reducers
        /// <summary>
        /// Reducer xử lý action tải tất cả danh sách con thi
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceLoadConThisAction(BCThongTinConThiState state, LoadConThisAction action) =>
            new(
                id: 4,
                maTruong: state.MaTruong,
                isLoading: true,
                errorMessage: null,
                conThis: state.ConThis,
                selectedConThi: state.SelectedConThi,
                paginatedConThis: state.PaginatedConThis,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        /// <summary>
        /// Reducer xử lý khi tải tất cả danh sách con thi thành công
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceLoadConThisSuccessAction(BCThongTinConThiState state, LoadConThisSuccessAction action) =>
            new(id: 5,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: null,
                conThis: action.ConThis, // Cập nhật danh sách từ action
                selectedConThi: state.SelectedConThi,
                paginatedConThis: state.PaginatedConThis,
                isInitialized: true,
                notificationMessage: "Đã tải thông tin lãnh đạo thành công",
                notificationType: "success");

        /// <summary>
        /// Reducer xử lý khi tải tất cả danh sách con thi thất bại
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceLoadConThisFailureAction(BCThongTinConThiState state, LoadConThisFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? null : $"{action.ErrorMessage} [{DateTime.Now.Ticks}]";
            return new(id: 6,
                maTruong: state.MaTruong,
                isLoading: false,
                errorMessage: errorMsg,
                conThis: state.ConThis,
                selectedConThi: state.SelectedConThi,
                paginatedConThis: state.PaginatedConThis,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi tải thông tin lãnh đạo: {action.ErrorMessage}",
                notificationType: "error");
        }
        #endregion

        #region Select ConThi Reducers
        /// <summary>
        /// Reducer xử lý action chọn con thi
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceSelectConThiAction(BCThongTinConThiState state, SelectConThiAction action)
        {
            // Kiểm tra action.ConThi có null không
            if (action.ConThi == null)
            {
                return state; // Giữ nguyên state nếu không có dữ liệu
            }

            return new(
                id: 7, // ID tùy ý để phân biệt lần thay đổi state
                maTruong: action.ConThi.MaTruong, // Cập nhật MaTruong từ con thi được chọn
                isLoading: false, // Giả định không cần loading khi chỉ chọn
                errorMessage: null, // Xóa lỗi cũ
                conThis: state.ConThis, // Giữ nguyên danh sách con thi
                selectedConThi: action.ConThi, // Cập nhật con thi được chọn
                paginatedConThis: state.PaginatedConThis, // Giữ nguyên danh sách phân trang
                isInitialized: state.IsInitialized, // Giữ nguyên trạng thái khởi tạo
                notificationMessage: "Đã chọn con thi", // Thông báo mới
                notificationType: "success" // Loại thông báo
            );
        }
        #endregion

        #region Get ConThi By MaTruong Reducers
        /// <summary>
        /// Reducer xử lý action lấy danh sách con thi theo mã trường
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceGetConThiByMaTruongAction(BCThongTinConThiState state, GetConThiByMaTruongAction action) =>
            new(id: 8,
                maTruong: action.MaTruong, // Cập nhật MaTruong từ action
                isLoading: true, // Đặt trạng thái đang tải
                errorMessage: null, // Xóa lỗi cũ
                conThis: state.ConThis, // Giữ nguyên danh sách hiện tại trong khi tải
                selectedConThi: state.SelectedConThi, // Giữ nguyên con thi đã chọn
                paginatedConThis: state.PaginatedConThis, // Giữ nguyên phân trang hiện tại
                isInitialized: state.IsInitialized,
                notificationMessage: null, // Xóa thông báo cũ
                notificationType: null);

        /// <summary>
        /// Reducer xử lý khi lấy danh sách con thi theo mã trường thành công
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceGetConThiByMaTruongSuccessAction(BCThongTinConThiState state, GetConThiByMaTruongSuccessAction action)
        {
            bool hasData = action.ConThis != null && action.ConThis.Any();

            return new BCThongTinConThiState(
                id: 9,
                maTruong: state.MaTruong, // Giữ nguyên mã trường đang xử lý
                isLoading: false, // Tải xong
                errorMessage: null, // Xóa lỗi
                conThis: hasData ? action.ConThis : Enumerable.Empty<KhaoThi_5_THPT_ThongTin_ConThiModel>(), // Cập nhật danh sách mới hoặc danh sách rỗng
                selectedConThi: state.SelectedConThi, // Giữ nguyên lựa chọn hiện tại
                paginatedConThis: state.PaginatedConThis, // Giữ nguyên phân trang (có thể cần cập nhật nếu logic yêu cầu)
                isInitialized: true, // Đánh dấu đã khởi tạo/tải dữ liệu
                notificationMessage: hasData ? "Đã tải danh sách con thi theo trường thành công" : "Không tìm thấy con thi cho trường này", // Thông báo phù hợp
                notificationType: hasData ? "success" : "info" // Loại thông báo
            );
        }

        /// <summary>
        /// Reducer xử lý khi lấy danh sách con thi theo mã trường thất bại
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceGetConThiByMaTruongFailureAction(BCThongTinConThiState state, GetConThiByMaTruongFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? "Lỗi không xác định" : action.ErrorMessage;
            var errorMsgWithTimestamp = $"{errorMsg} [{DateTime.Now.Ticks}]"; // Thêm timestamp để dễ debug

            return new BCThongTinConThiState(
                id: 10,
                maTruong: state.MaTruong, // Giữ nguyên mã trường đang xử lý
                isLoading: false, // Tải thất bại
                errorMessage: errorMsgWithTimestamp, // Lưu lỗi kèm timestamp
                conThis: state.ConThis, // Giữ nguyên danh sách cũ khi lỗi
                selectedConThi: state.SelectedConThi, // Giữ nguyên lựa chọn
                paginatedConThis: state.PaginatedConThis, // Giữ nguyên phân trang
                isInitialized: state.IsInitialized, // Trạng thái khởi tạo không đổi khi lỗi tải
                notificationMessage: $"Lỗi tải con thi theo mã trường: {errorMsg}", // Thông báo lỗi cho người dùng
                notificationType: "error"
            );
        }
        #endregion

        #region Get ConThi Detail Reducers
        /// <summary>
        /// Reducer xử lý action lấy thông tin chi tiết con thi
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceGetConThiDetailAction(BCThongTinConThiState state, GetConThiDetailAction action) =>
            new(id: 11,
                maTruong: action.MaTruong, // Có thể lấy MaTruong từ action hoặc state tùy logic
                isLoading: true, // Đang tải chi tiết
                errorMessage: null,
                conThis: state.ConThis, // Giữ nguyên danh sách
                selectedConThi: state.SelectedConThi, // Giữ nguyên lựa chọn hiện tại trong khi tải
                paginatedConThis: state.PaginatedConThis,
                isInitialized: state.IsInitialized,
                notificationMessage: null,
                notificationType: null);

        /// <summary>
        /// Reducer xử lý khi lấy thông tin chi tiết con thi thành công
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceGetConThiDetailSuccessAction(BCThongTinConThiState state, GetConThiDetailSuccessAction action)
        {
            // Kiểm tra action.ConThi có null không
            if (action.ConThi == null)
            {
                return new(
                    id: 12,
                    maTruong: state.MaTruong, // Giữ mã trường hiện tại
                    isLoading: false, // Tải xong nhưng không có dữ liệu
                    errorMessage: "Không tìm thấy dữ liệu chi tiết con thi", // Ghi nhận lỗi không tìm thấy
                    conThis: state.ConThis, // Giữ nguyên danh sách
                    selectedConThi: null, // Không có dữ liệu chi tiết nên reset lựa chọn
                    paginatedConThis: state.PaginatedConThis,
                    isInitialized: true, // Đã cố gắng tải
                    notificationMessage: "Không tìm thấy thông tin chi tiết con thi",
                    notificationType: "warning" // Dùng warning vì không phải lỗi hệ thống
                );
            }

            // Tìm thấy dữ liệu chi tiết
            return new(
                id: 12,
                maTruong: action.ConThi.MaTruong, // Cập nhật MaTruong từ dữ liệu chi tiết
                isLoading: false, // Tải xong
                errorMessage: null,
                conThis: state.ConThis, // Giữ nguyên danh sách chính
                selectedConThi: action.ConThi, // Cập nhật con thi chi tiết được chọn
                paginatedConThis: state.PaginatedConThis,
                isInitialized: true,
                notificationMessage: "Đã tải thông tin chi tiết con thi thành công",
                notificationType: "success"
            );
        }

        /// <summary>
        /// Reducer xử lý khi lấy thông tin chi tiết con thi thất bại
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceGetConThiDetailFailureAction(BCThongTinConThiState state, GetConThiDetailFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? "Lỗi không xác định" : action.ErrorMessage;
            var errorMsgWithTimestamp = $"{errorMsg} [{DateTime.Now.Ticks}]";

            return new(
                id: 13,
                maTruong: state.MaTruong, // Giữ mã trường hiện tại
                isLoading: false, // Tải thất bại
                errorMessage: errorMsgWithTimestamp,
                conThis: state.ConThis, // Giữ nguyên danh sách
                selectedConThi: null, // Reset selectedConThi khi có lỗi tải chi tiết
                paginatedConThis: state.PaginatedConThis,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi tải thông tin chi tiết con thi: {errorMsg}",
                notificationType: "error"
            );
        }
        #endregion

        #region Create ConThi Reducers
        /// <summary>
        /// Reducer xử lý action tạo mới con thi
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceCreateConThiAction(BCThongTinConThiState state, CreateConThiAction action) =>
            new(
                id: 14,
                maTruong: state.MaTruong, // Giữ mã trường hiện tại
                isLoading: true, // Đang thực hiện tạo mới
                errorMessage: null,
                conThis: state.ConThis,
                selectedConThi: state.SelectedConThi,
                paginatedConThis: state.PaginatedConThis,
                isInitialized: state.IsInitialized,
                notificationMessage: "Đang tạo mới con thi...", // Thông báo trạng thái
                notificationType: "info"
            );

        /// <summary>
        /// Reducer xử lý khi tạo mới con thi thành công
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceCreateConThiSuccessAction(BCThongTinConThiState state, CreateConThiSuccessAction action)
        {
            // Không thay đổi danh sách hiện tại vì cần thông tin đầy đủ của con thi mới được tạo
            // Sẽ tải lại danh sách ở effect
            return new(
                id: 15,
                maTruong: state.MaTruong,
                isLoading: true, // Vẫn loading vì effect sẽ tải lại danh sách ngay sau đó
                errorMessage: null,
                conThis: state.ConThis, // Chưa cập nhật danh sách ở đây
                selectedConThi: state.SelectedConThi,
                paginatedConThis: state.PaginatedConThis,
                isInitialized: state.IsInitialized,
                notificationMessage: "Tạo mới con thi thành công, đang tải lại danh sách...",
                notificationType: "success"
            );
        }

        /// <summary>
        /// Reducer xử lý khi tạo mới con thi thất bại
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceCreateConThiFailureAction(BCThongTinConThiState state, CreateConThiFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? "Lỗi không xác định khi tạo" : action.ErrorMessage;
            var errorMsgWithTimestamp = $"{errorMsg} [{DateTime.Now.Ticks}]";

            return new BCThongTinConThiState(
                id: 16,
                maTruong: state.MaTruong,
                isLoading: false, // Tạo thất bại, dừng loading
                errorMessage: errorMsgWithTimestamp, // Lưu thông báo lỗi từ action
                conThis: state.ConThis,
                selectedConThi: state.SelectedConThi,
                paginatedConThis: state.PaginatedConThis,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi tạo con thi: {errorMsg}",
                notificationType: "error"
            );
        }
        #endregion

        #region Update ConThi Reducers
        /// <summary>
        /// Reducer xử lý action cập nhật con thi
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceUpdateConThiAction(BCThongTinConThiState state, UpdateConThiAction action) =>
             new(
                id: 17,
                maTruong: state.MaTruong, // Giữ mã trường hiện tại
                isLoading: true, // Đang cập nhật
                errorMessage: null,
                conThis: state.ConThis,
                selectedConThi: state.SelectedConThi, // Giữ con thi đã chọn trong khi cập nhật
                paginatedConThis: state.PaginatedConThis,
                isInitialized: state.IsInitialized,
                notificationMessage: "Đang cập nhật thông tin con thi...",
                notificationType: "info"
            );

        /// <summary>
        /// Reducer xử lý khi cập nhật con thi thành công
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceUpdateConThiSuccessAction(BCThongTinConThiState state, UpdateConThiSuccessAction action)
        {
            // Kiểm tra action.ConThi có null không
            if (action.ConThi == null)
            {
                // Nếu không có dữ liệu trả về, coi như thất bại hoặc không cần làm gì
                return new BCThongTinConThiState(
                   id: 18, // ID khác
                   maTruong: state.MaTruong,
                   isLoading: false, // Cập nhật xong nhưng không có dữ liệu mới
                   errorMessage: "Dữ liệu trả về sau khi cập nhật không hợp lệ.",
                   conThis: state.ConThis,
                   selectedConThi: state.SelectedConThi,
                   paginatedConThis: state.PaginatedConThis,
                   isInitialized: state.IsInitialized,
                   notificationMessage: "Cập nhật thất bại: Dữ liệu trả về không hợp lệ.",
                   notificationType: "error"
                );
            }

            // Cập nhật con thi trong danh sách (nếu danh sách tồn tại)
            var updatedConThis = state.ConThis?.Select(ct =>
                (ct.MaTruong == action.ConThi.MaTruong &&
                 ct.CCCD == action.ConThi.CCCD &&
                 ct.MaDinhDanhCuaCon == action.ConThi.MaDinhDanhCuaCon)
                ? action.ConThi // Thay thế bằng con thi mới từ action
                : ct            // Giữ nguyên con thi cũ
            ).ToList(); // Chuyển thành List để có thể cập nhật

            // Cập nhật con thi trong danh sách phân trang (nếu danh sách tồn tại)
            var updatedPaginatedItems = state.PaginatedConThis?.Items?.Select(ct =>
                 (ct.MaTruong == action.ConThi.MaTruong &&
                  ct.CCCD == action.ConThi.CCCD &&
                  ct.MaDinhDanhCuaCon == action.ConThi.MaDinhDanhCuaCon)
                 ? action.ConThi
                 : ct
            ).ToList();

            var updatedPaginatedConThis = state.PaginatedConThis == null ? null :
                new PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThiModel>(
                    updatedPaginatedItems ?? Enumerable.Empty<KhaoThi_5_THPT_ThongTin_ConThiModel>(), // Danh sách items đã cập nhật
                    state.PaginatedConThis.TotalCount, // TotalCount có thể không đổi khi chỉ update 1 item
                    state.PaginatedConThis.Page,
                    state.PaginatedConThis.PageSize
                );

            // Effect sẽ tải lại toàn bộ danh sách, nhưng cập nhật cục bộ giúp UI phản hồi nhanh hơn
            return new BCThongTinConThiState(
                id: 18,
                maTruong: action.ConThi.MaTruong, // Cập nhật MaTruong nếu cần
                isLoading: true, // Vẫn loading vì effect sẽ tải lại danh sách ngay sau đó
                errorMessage: null,
                conThis: updatedConThis, // Danh sách đã cập nhật cục bộ
                selectedConThi: (state.SelectedConThi?.MaTruong == action.ConThi.MaTruong &&
                                 state.SelectedConThi?.CCCD == action.ConThi.CCCD &&
                                 state.SelectedConThi?.MaDinhDanhCuaCon == action.ConThi.MaDinhDanhCuaCon)
                                ? action.ConThi : state.SelectedConThi, // Cập nhật con thi đang được chọn nếu nó là cái vừa update
                paginatedConThis: updatedPaginatedConThis, // Phân trang đã cập nhật cục bộ
                isInitialized: true,
                notificationMessage: "Cập nhật thành công, đang tải lại danh sách...",
                notificationType: "success"
            );
        }


        /// <summary>
        /// Reducer xử lý khi cập nhật con thi thất bại
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceUpdateConThiFailureAction(BCThongTinConThiState state, UpdateConThiFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? "Lỗi không xác định khi cập nhật" : action.ErrorMessage;
            var errorMsgWithTimestamp = $"{errorMsg} [{DateTime.Now.Ticks}]";

            return new BCThongTinConThiState(
                id: 19,
                maTruong: state.MaTruong,
                isLoading: false, // Cập nhật thất bại
                errorMessage: errorMsgWithTimestamp,
                conThis: state.ConThis, // Giữ nguyên dữ liệu cũ
                selectedConThi: state.SelectedConThi, // Giữ nguyên con thi đã chọn
                paginatedConThis: state.PaginatedConThis,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi cập nhật con thi: {errorMsg}",
                notificationType: "error"
            );
        }
        #endregion

        #region Delete ConThi Reducers
        /// <summary>
        /// Reducer xử lý action xóa con thi
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceDeleteConThiAction(BCThongTinConThiState state, DeleteConThiAction action) =>
            new BCThongTinConThiState(
                id: 20,
                maTruong: state.MaTruong,
                isLoading: true, // Đang xóa
                errorMessage: null,
                conThis: state.ConThis,
                selectedConThi: state.SelectedConThi,
                paginatedConThis: state.PaginatedConThis,
                isInitialized: state.IsInitialized,
                notificationMessage: "Đang xóa thông tin con thi...",
                notificationType: "info"
            );

        /// <summary>
        /// Reducer xử lý khi xóa con thi thành công
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceDeleteConThiSuccessAction(BCThongTinConThiState state, DeleteConThiSuccessAction action)
        {
            // Xóa con thi khỏi danh sách (nếu tồn tại)
            var updatedConThis = state.ConThis?
                .Where(ct => !(ct.MaTruong == action.MaTruong &&
                               ct.CCCD == action.CCCD &&
                               ct.MaDinhDanhCuaCon == action.MaDinhDanhCuaCon))
                .ToList();

            // Xóa con thi khỏi danh sách phân trang (nếu tồn tại)
            var updatedPaginatedItems = state.PaginatedConThis?.Items?
                .Where(ct => !(ct.MaTruong == action.MaTruong &&
                               ct.CCCD == action.CCCD &&
                               ct.MaDinhDanhCuaCon == action.MaDinhDanhCuaCon))
                .ToList();

            // Ước tính TotalCount mới (giảm đi 1 nếu tìm thấy và xóa)
            // Lưu ý: Cách này không hoàn toàn chính xác nếu item bị xóa không nằm trong trang hiện tại.
            // Effect tải lại sẽ đảm bảo tính chính xác cuối cùng.
            int newTotalCount = state.PaginatedConThis?.TotalCount ?? 0;
            if (state.PaginatedConThis?.Items?.Any(ct => ct.MaTruong == action.MaTruong && ct.CCCD == action.CCCD && ct.MaDinhDanhCuaCon == action.MaDinhDanhCuaCon) ?? false)
            {
                newTotalCount = Math.Max(0, newTotalCount - 1);
            }

            var updatedPaginatedConThis = state.PaginatedConThis == null ? null :
               new PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThiModel>(
                   updatedPaginatedItems ?? Enumerable.Empty<KhaoThi_5_THPT_ThongTin_ConThiModel>(),
                   newTotalCount, // TotalCount ước tính
                   state.PaginatedConThis.Page, // Giữ nguyên trang
                   state.PaginatedConThis.PageSize // Giữ nguyên kích thước trang
               );

            // Reset con thi được chọn nếu nó là cái vừa bị xóa
            var newSelectedConThi = (state.SelectedConThi?.MaTruong == action.MaTruong &&
                                     state.SelectedConThi?.CCCD == action.CCCD &&
                                     state.SelectedConThi?.MaDinhDanhCuaCon == action.MaDinhDanhCuaCon)
                                    ? null : state.SelectedConThi;

            // Effect sẽ tải lại, nhưng cập nhật cục bộ giúp UI phản hồi nhanh
            return new BCThongTinConThiState(
                id: 21,
                maTruong: state.MaTruong,
                isLoading: true, // Vẫn loading vì effect sẽ tải lại danh sách
                errorMessage: null,
                conThis: updatedConThis, // Danh sách đã cập nhật cục bộ
                selectedConThi: newSelectedConThi, // Cập nhật lựa chọn nếu cần
                paginatedConThis: updatedPaginatedConThis, // Phân trang đã cập nhật cục bộ
                isInitialized: state.IsInitialized,
                notificationMessage: "Xóa thành công, đang tải lại danh sách...",
                notificationType: "success"
            );
        }

        /// <summary>
        /// Reducer xử lý khi xóa con thi thất bại
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceDeleteConThiFailureAction(BCThongTinConThiState state, DeleteConThiFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? "Lỗi không xác định khi xóa" : action.ErrorMessage;
            var errorMsgWithTimestamp = $"{errorMsg} [{DateTime.Now.Ticks}]";

            return new BCThongTinConThiState(
                id: 22,
                maTruong: state.MaTruong,
                isLoading: false, // Xóa thất bại
                errorMessage: errorMsgWithTimestamp,
                conThis: state.ConThis, // Giữ nguyên danh sách
                selectedConThi: state.SelectedConThi, // Giữ nguyên lựa chọn
                paginatedConThis: state.PaginatedConThis,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi xóa con thi: {errorMsg}",
                notificationType: "error"
            );
        }
        #endregion

        #region Check ConThi Exist Reducers
        /// <summary>
        /// Reducer xử lý action kiểm tra tồn tại con thi
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceCheckConThiExistAction(BCThongTinConThiState state, CheckConThiExistAction action) =>
            new BCThongTinConThiState(
                id: 23,
                maTruong: state.MaTruong,
                isLoading: true, // Đang kiểm tra
                errorMessage: null,
                conThis: state.ConThis,
                selectedConThi: state.SelectedConThi,
                paginatedConThis: state.PaginatedConThis,
                isInitialized: state.IsInitialized,
                notificationMessage: "Đang kiểm tra thông tin...",
                notificationType: "info"
             );

        /// <summary>
        /// Reducer xử lý khi kiểm tra tồn tại con thi thành công
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceCheckConThiExistSuccessAction(BCThongTinConThiState state, CheckConThiExistSuccessAction action) =>
            new BCThongTinConThiState( // Trả về state mới để clear loading và thông báo
               id: 23, // Cùng ID hoặc ID mới
               maTruong: state.MaTruong,
               isLoading: false, // Kiểm tra xong
               errorMessage: null,
               conThis: state.ConThis,
               selectedConThi: state.SelectedConThi,
               paginatedConThis: state.PaginatedConThis,
               isInitialized: state.IsInitialized,
               notificationMessage: action.Exists ? "Thông tin này đã tồn tại." : null, // Chỉ thông báo nếu tồn tại
               notificationType: action.Exists ? "warning" : null
            );

        /// <summary>
        /// Reducer xử lý khi kiểm tra tồn tại con thi thất bại
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceCheckConThiExistFailureAction(BCThongTinConThiState state, CheckConThiExistFailureAction action)
        {
            var errorMsg = string.IsNullOrEmpty(action.ErrorMessage) ? "Lỗi không xác định khi kiểm tra" : action.ErrorMessage;
            var errorMsgWithTimestamp = $"{errorMsg} [{DateTime.Now.Ticks}]";

            return new BCThongTinConThiState(
                id: 24,
                maTruong: state.MaTruong,
                isLoading: false, // Kiểm tra thất bại
                errorMessage: errorMsgWithTimestamp,
                conThis: state.ConThis,
                selectedConThi: state.SelectedConThi,
                paginatedConThis: state.PaginatedConThis,
                isInitialized: state.IsInitialized,
                notificationMessage: $"Lỗi kiểm tra con thi: {errorMsg}",
                notificationType: "error"
            );
        }
        #endregion

        #region Utility Reducers
        /// <summary>
        /// Reducer xử lý action set loading
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceSetLoadingAction(BCThongTinConThiState state, SetLoadingAction action)
        {
            // Giữ nguyên ErrorMessage và Notification khi chỉ thay đổi isLoading
            // Nếu muốn xóa lỗi/thông báo khi bắt đầu loading, cần dispatch ClearErrorAction trước SetLoadingAction(true)
            return new BCThongTinConThiState(
               id: 25,
               maTruong: state.MaTruong,
               isLoading: action.IsLoading, // Cập nhật isLoading từ action
               errorMessage: state.ErrorMessage, // Giữ nguyên lỗi
               conThis: state.ConThis,
               selectedConThi: state.SelectedConThi,
               paginatedConThis: state.PaginatedConThis,
               isInitialized: state.IsInitialized,
               notificationMessage: state.NotificationMessage, // Giữ nguyên thông báo
               notificationType: state.NotificationType
           );
        }

        /// <summary>
        /// Reducer xử lý action clear error
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceClearErrorAction(BCThongTinConThiState state, ClearErrorAction action) =>
            new BCThongTinConThiState(
                id: 26,
                maTruong: state.MaTruong,
                isLoading: state.IsLoading, // Giữ nguyên trạng thái loading
                errorMessage: null, // Xóa lỗi
                conThis: state.ConThis,
                selectedConThi: state.SelectedConThi,
                paginatedConThis: state.PaginatedConThis,
                isInitialized: state.IsInitialized,
                notificationMessage: null, // Xóa thông báo
                notificationType: null
            );


        /// <summary>
        /// Reducer xử lý action hiển thị thông báo
        /// </summary>
        [ReducerMethod]
        public static BCThongTinConThiState ReduceShowNotificationAction(BCThongTinConThiState state, ShowNotificationAction action)
        {
            // Giữ nguyên ErrorMessage khi hiển thị thông báo (trừ khi muốn ghi đè)
            return new BCThongTinConThiState(
                id: 27,
                maTruong: state.MaTruong,
                isLoading: state.IsLoading, // Giữ nguyên trạng thái loading
                errorMessage: state.ErrorMessage, // Giữ nguyên lỗi
                conThis: state.ConThis,
                selectedConThi: state.SelectedConThi,
                paginatedConThis: state.PaginatedConThis,
                isInitialized: state.IsInitialized,
                notificationMessage: action.Message, // Cập nhật thông báo từ action
                notificationType: action.NotificationType // Cập nhật loại thông báo
            );
        }
        #endregion
    }
}