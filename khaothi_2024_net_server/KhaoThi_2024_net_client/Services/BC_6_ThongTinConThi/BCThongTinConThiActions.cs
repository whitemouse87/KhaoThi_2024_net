using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
namespace KhaoThi_2024_net_client.Services.BC_6_ThongTinConThi
{
    public static class BCThongTinConThiActionTypes
    {
        // Tải danh sách con thí sinh
        public const string LOAD_CONTHI = "[Con Thí Sinh] Tải danh sách";
        public const string LOAD_CONTHI_SUCCESS = "[Con Thí Sinh] Tải danh sách thành công";
        public const string LOAD_CONTHI_FAILURE = "[Con Thí Sinh] Tải danh sách thất bại";

        // Lựa chọn con thí sinh
        public const string SELECT_CONTHI = "[Con Thí Sinh] Lựa chọn con thí sinh";

        // Tìm con thí sinh theo mã trường và CCCD
        public const string GET_CONTHI_BY_MA_TRUONG_AND_CCCD = "[Con Thí Sinh] Tìm theo mã trường và CCCD";
        public const string GET_CONTHI_BY_MA_TRUONG_AND_CCCD_SUCCESS = "[Con Thí Sinh] Tìm theo mã trường và CCCD thành công";
        public const string GET_CONTHI_BY_MA_TRUONG_AND_CCCD_FAILURE = "[Con Thí Sinh] Tìm theo mã trường và CCCD thất bại";

        // Tìm con thí sinh theo các khóa chính
        public const string GET_CONTHI_BY_KEYS = "[Con Thí Sinh] Tìm theo khóa chính";
        public const string GET_CONTHI_BY_KEYS_SUCCESS = "[Con Thí Sinh] Tìm theo khóa chính thành công";
        public const string GET_CONTHI_BY_KEYS_FAILURE = "[Con Thí Sinh] Tìm theo khóa chính thất bại";

        // Phân trang
        public const string LOAD_PAGINATED = "[Con Thí Sinh] Tải danh sách phân trang";
        public const string LOAD_PAGINATED_SUCCESS = "[Con Thí Sinh] Tải phân trang thành công";
        public const string LOAD_PAGINATED_FAILURE = "[Con Thí Sinh] Tải phân trang thất bại";

        // Tạo mới con thí sinh
        public const string INSERT_CONTHI = "[Con Thí Sinh] Tạo mới con thí sinh";
        public const string INSERT_CONTHI_SUCCESS = "[Con Thí Sinh] Tạo mới thành công";
        public const string INSERT_CONTHI_FAILURE = "[Con Thí Sinh] Tạo mới thất bại";

        // Cập nhật thông tin
        public const string UPDATE_CONTHI = "[Con Thí Sinh] Cập nhật thông tin";
        public const string UPDATE_CONTHI_SUCCESS = "[Con Thí Sinh] Cập nhật thành công";
        public const string UPDATE_CONTHI_FAILURE = "[Con Thí Sinh] Cập nhật thất bại";

        // Xóa con thí sinh
        public const string DELETE_CONTHI = "[Con Thí Sinh] Xóa con thí sinh";
        public const string DELETE_CONTHI_SUCCESS = "[Con Thí Sinh] Xóa thành công";
        public const string DELETE_CONTHI_FAILURE = "[Con Thí Sinh] Xóa thất bại";

        // Kiểm tra tồn tại
        public const string CHECK_CONTHI_EXIST = "[Con Thí Sinh] Kiểm tra tồn tại";
        public const string CHECK_CONTHI_EXIST_SUCCESS = "[Con Thí Sinh] Kiểm tra tồn tại thành công";
        public const string CHECK_CONTHI_EXIST_FAILURE = "[Con Thí Sinh] Kiểm tra tồn tại thất bại";

        // Thông báo và trạng thái
        public const string SHOW_NOTIFICATION = "[Con Thí Sinh] Hiển thị thông báo";
        public const string SET_LOADING = "[Con Thí Sinh] Đang tải";
        public const string CLEAR_ERROR = "[Con Thí Sinh] Xóa lỗi";
    }
    public interface IBCThongTinConThiAction
    {
        string Type { get; }
        DateTime Timestamp { get; }
    }

    public static class BCThongTinConThiActions
    {
        // Actions tải danh sách con thí sinh
        public record LoadConThiAction() : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.LOAD_CONTHI;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadConThiSuccessAction(IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi> ConThiList) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.LOAD_CONTHI_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadConThiFailureAction(string ErrorMessage) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.LOAD_CONTHI_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions phân trang
        public record LoadPaginatedConThiAction(int Page, int PageSize, string? SearchTerm) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.LOAD_PAGINATED;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadPaginatedConThiSuccessAction(PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThi> PaginatedConThi) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.LOAD_PAGINATED_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadPaginatedConThiFailureAction(string ErrorMessage) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.LOAD_PAGINATED_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions lựa chọn con thí sinh
        public record SelectConThiAction(KhaoThi_5_THPT_ThongTin_ConThi ConThi) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.SELECT_CONTHI;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions tạo con thí sinh mới
        public record InsertConThiAction(KhaoThi_5_THPT_ThongTin_ConThi ConThi) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.INSERT_CONTHI;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record InsertConThiSuccessAction() : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.INSERT_CONTHI_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record InsertConThiFailureAction(string ErrorMessage) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.INSERT_CONTHI_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions cập nhật thông tin
        public record UpdateConThiAction(KhaoThi_5_THPT_ThongTin_ConThi ConThi) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.UPDATE_CONTHI;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record UpdateConThiSuccessAction(KhaoThi_5_THPT_ThongTin_ConThi ConThi) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.UPDATE_CONTHI_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record UpdateConThiFailureAction(string ErrorMessage) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.UPDATE_CONTHI_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions xóa con thí sinh
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

        // Actions tìm con thí sinh qua mã trường và CCCD
        public record GetConThiByMaTruongAndCCCDAction(string MaTruong, string CCCD) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.GET_CONTHI_BY_MA_TRUONG_AND_CCCD;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record GetConThiByMaTruongAndCCCDSuccessAction(IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi> ConThiList) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.GET_CONTHI_BY_MA_TRUONG_AND_CCCD_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record GetConThiByMaTruongAndCCCDFailureAction(string ErrorMessage) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.GET_CONTHI_BY_MA_TRUONG_AND_CCCD_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions tìm con thí sinh qua khóa chính
        public record GetConThiByKeysAction(string MaTruong, string CCCD, string MaDinhDanhCuaCon) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.GET_CONTHI_BY_KEYS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record GetConThiByKeysSuccessAction(KhaoThi_5_THPT_ThongTin_ConThi ConThi) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.GET_CONTHI_BY_KEYS_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }
        public record GetConThiByKeysFailureAction(string ErrorMessage) : IBCThongTinConThiAction
        {
            public string Type => BCThongTinConThiActionTypes.GET_CONTHI_BY_KEYS_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions kiểm tra tồn tại con thí sinh
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
