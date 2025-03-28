using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;

namespace khaothi_2024_net_client.Services.BC_5_ThongTinConThi
{
    public static class BCThongTinConThiActionTypes
    {
        // Tải danh sách con thi
        public const string LOAD_CONTHIS = "[Con thi] Tải danh sách";
        public const string LOAD_CONTHIS_SUCCESS = "[Con thi] Tải danh sách thành công";
        public const string LOAD_CONTHIS_FAILURE = "[Con thi] Tải danh sách thất bại";

        // Lựa chọn con thi
        public const string SELECT_CONTHI = "[Con thi] Lựa chọn con thi";

        // Tìm con thi theo mã trường
        public const string GET_CONTHI_BY_MA_TRUONG = "[Con thi] Tìm theo mã trường";
        public const string GET_CONTHI_BY_MA_TRUONG_SUCCESS = "[Con thi] Tìm theo mã trường thành công";
        public const string GET_CONTHI_BY_MA_TRUONG_FAILURE = "[Con thi] Tìm theo mã trường thất bại";

        // Tìm thông tin chi tiết con thi
        public const string GET_CONTHI_DETAIL = "[Con thi] Tìm thông tin chi tiết";
        public const string GET_CONTHI_DETAIL_SUCCESS = "[Con thi] Tìm thông tin chi tiết thành công";
        public const string GET_CONTHI_DETAIL_FAILURE = "[Con thi] Tìm thông tin chi tiết thất bại";

        // Phân trang
        public const string LOAD_PAGINATED = "[Con thi] Tải danh sách phân trang";
        public const string LOAD_PAGINATED_SUCCESS = "[Con thi] Tải phân trang thành công";
        public const string LOAD_PAGINATED_FAILURE = "[Con thi] Tải phân trang thất bại";

        // Tạo mới thông tin con thi
        public const string CREATE_CONTHI = "[Con thi] Tạo mới thông tin con thi";
        public const string CREATE_CONTHI_SUCCESS = "[Con thi] Tạo mới thành công";
        public const string CREATE_CONTHI_FAILURE = "[Con thi] Tạo mới thất bại";

        // Cập nhật thông tin
        public const string UPDATE_CONTHI = "[Con thi] Cập nhật thông tin";
        public const string UPDATE_CONTHI_SUCCESS = "[Con thi] Cập nhật thành công";
        public const string UPDATE_CONTHI_FAILURE = "[Con thi] Cập nhật thất bại";

        // Xóa thông tin con thi
        public const string DELETE_CONTHI = "[Con thi] Xóa thông tin con thi";
        public const string DELETE_CONTHI_SUCCESS = "[Con thi] Xóa thành công";
        public const string DELETE_CONTHI_FAILURE = "[Con thi] Xóa thất bại";

        // Kiểm tra tồn tại
        public const string CHECK_CONTHI_EXIST = "[Con thi] Kiểm tra tồn tại";
        public const string CHECK_CONTHI_EXIST_SUCCESS = "[Con thi] Kiểm tra tồn tại thành công";
        public const string CHECK_CONTHI_EXIST_FAILURE = "[Con thi] Kiểm tra tồn tại thất bại";

        // Thông báo và trạng thái
        public const string SHOW_NOTIFICATION = "[Con thi] Hiển thị thông báo";
        public const string SET_LOADING = "[Con thi] Đang tải";
        public const string CLEAR_ERROR = "[Con thi] Xóa lỗi";
    }

    public interface IBCThongTinConThiAction
    {
        string Type { get; }
        DateTime Timestamp { get; }
    }

    public static class BCThongTinConThiAction
    {
        // Actions tải danh sách con thi
        public record LoadConThisAction() : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.LOAD_CONTHIS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadConThisSuccessAction(IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel> ConThis) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.LOAD_CONTHIS_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadConThisFailureAction(string ErrorMessage) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.LOAD_CONTHIS_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions phân trang
        public record LoadPaginatedConThisAction(int Page, int PageSize, string? SearchTerm, string? MaTruong = null, string? KyThiThamDu = null) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.LOAD_PAGINATED;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadPaginatedConThisSuccessAction(PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThiModel> PaginatedConThis) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.LOAD_PAGINATED_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadPaginatedConThisFailureAction(string ErrorMessage) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.LOAD_PAGINATED_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions lựa chọn con thi
        public record SelectConThiAction(KhaoThi_5_THPT_ThongTin_ConThiModel ConThi) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.SELECT_CONTHI;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions tạo thông tin con thi mới
        public record CreateConThiAction(KhaoThi_5_THPT_ThongTin_ConThiModel ConThi) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.CREATE_CONTHI;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record CreateConThiSuccessAction() : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.CREATE_CONTHI_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record CreateConThiFailureAction(string ErrorMessage) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.CREATE_CONTHI_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions cập nhật thông tin
        public record UpdateConThiAction(KhaoThi_5_THPT_ThongTin_ConThiModel ConThi) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.UPDATE_CONTHI;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record UpdateConThiSuccessAction(KhaoThi_5_THPT_ThongTin_ConThiModel ConThi) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.UPDATE_CONTHI_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record UpdateConThiFailureAction(string ErrorMessage) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.UPDATE_CONTHI_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions xóa thông tin con thi
        public record DeleteConThiAction(string MaTruong, string CCCD, string MaDinhDanhCuaCon) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.DELETE_CONTHI;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record DeleteConThiSuccessAction(string MaTruong, string CCCD, string MaDinhDanhCuaCon) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.DELETE_CONTHI_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record DeleteConThiFailureAction(string ErrorMessage) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.DELETE_CONTHI_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions tìm con thi qua mã trường
        public record GetConThiByMaTruongAction(string MaTruong) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.GET_CONTHI_BY_MA_TRUONG;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record GetConThiByMaTruongSuccessAction(IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel> ConThis) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.GET_CONTHI_BY_MA_TRUONG_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record GetConThiByMaTruongFailureAction(string ErrorMessage) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.GET_CONTHI_BY_MA_TRUONG_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions tìm thông tin chi tiết con thi
        public record GetConThiDetailAction(string MaTruong, string CCCD, string MaDinhDanhCuaCon) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.GET_CONTHI_DETAIL;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record GetConThiDetailSuccessAction(KhaoThi_5_THPT_ThongTin_ConThiModel ConThi) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.GET_CONTHI_DETAIL_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record GetConThiDetailFailureAction(string ErrorMessage) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.GET_CONTHI_DETAIL_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions kiểm tra tồn tại thông tin con thi
        public record CheckConThiExistAction(string MaTruong, string CCCD, string MaDinhDanhCuaCon) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.CHECK_CONTHI_EXIST;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record CheckConThiExistSuccessAction(bool Exists) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.CHECK_CONTHI_EXIST_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record CheckConThiExistFailureAction(string ErrorMessage) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.CHECK_CONTHI_EXIST_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions cho trạng thái loading và clear lỗi
        public record SetLoadingAction(bool IsLoading) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.SET_LOADING;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record ClearErrorAction() : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.CLEAR_ERROR;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record ShowNotificationAction : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.SHOW_NOTIFICATION;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
            public string Message { get; }
            public string NotificationType { get; }

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