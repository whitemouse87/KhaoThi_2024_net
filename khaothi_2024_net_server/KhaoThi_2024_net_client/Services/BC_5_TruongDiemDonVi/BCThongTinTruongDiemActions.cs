using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;

namespace KhaoThi_2024_net_client.Services.BC_5_TruongDiemDonVi
{
    public static class BCThongTinTruongDiemActionTypes
    {
        // Lựa chọn trường điểm
        public const string SELECT_TRUONGDIEM = "[Lãnh đạo điểm thi] Lựa chọn lãnh đạo điểm thi";

        // Phân trang
        public const string LOAD_PAGINATED = "[Lãnh đạo điểm thi] Tải danh sách phân trang";
        public const string LOAD_PAGINATED_SUCCESS = "[Lãnh đạo điểm thi] Tải phân trang thành công";
        public const string LOAD_PAGINATED_FAILURE = "[Lãnh đạo điểm thi] Tải phân trang thất bại";

        // Cập nhật thông tin
        public const string UPDATE_TRUONGDIEM = "[Lãnh đạo điểm thi] Cập nhật thông tin";
        public const string UPDATE_TRUONGDIEM_SUCCESS = "[Lãnh đạo điểm thi] Cập nhật thành công";
        public const string UPDATE_TRUONGDIEM_FAILURE = "[Lãnh đạo điểm thi] Cập nhật thất bại";

        // Thông báo và trạng thái
        public const string SHOW_NOTIFICATION = "[Lãnh đạo điểm thi] Hiển thị thông báo";
        public const string SET_LOADING = "[Lãnh đạo điểm thi] Đang tải";
        public const string CLEAR_ERROR = "[Lãnh đạo điểm thi] Xóa lỗi";
    }

    public interface IBCThongTinTruongDiemAction
    {
        string Type { get; }
        DateTime Timestamp { get; }
    }

    public static class BCThongTinTruongDiemAction
    {
        // Actions phân trang
        public record LoadPaginatedTruongDiemsAction(int Page, int PageSize, string? SearchTerm, string? MaTruong = null) : IBCThongTinTruongDiemAction
        {
            public string Type => BCThongTinTruongDiemActionTypes.LOAD_PAGINATED;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadPaginatedTruongDiemsSuccessAction(PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel> PaginatedTruongDiems) : IBCThongTinTruongDiemAction
        {
            public string Type => BCThongTinTruongDiemActionTypes.LOAD_PAGINATED_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadPaginatedTruongDiemsFailureAction(string ErrorMessage) : IBCThongTinTruongDiemAction
        {
            public string Type => BCThongTinTruongDiemActionTypes.LOAD_PAGINATED_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions lựa chọn trường điểm
        public record SelectTruongDiemAction(KhaoThi_4_THPT_ThongTin_LanhDaoModel TruongDiem) : IBCThongTinTruongDiemAction
        {
            public string Type => BCThongTinTruongDiemActionTypes.SELECT_TRUONGDIEM;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions cập nhật thông tin
        public record UpdateTruongDiemAction(KhaoThi_4_THPT_ThongTin_LanhDaoModel TruongDiem) : IBCThongTinTruongDiemAction
        {
            public string Type => BCThongTinTruongDiemActionTypes.UPDATE_TRUONGDIEM;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record UpdateTruongDiemSuccessAction(KhaoThi_4_THPT_ThongTin_LanhDaoModel TruongDiem) : IBCThongTinTruongDiemAction
        {
            public string Type => BCThongTinTruongDiemActionTypes.UPDATE_TRUONGDIEM_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record UpdateTruongDiemFailureAction(string ErrorMessage) : IBCThongTinTruongDiemAction
        {
            public string Type => BCThongTinTruongDiemActionTypes.UPDATE_TRUONGDIEM_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions cho trạng thái loading và clear lỗi
        public record SetLoadingAction(bool IsLoading) : IBCThongTinTruongDiemAction
        {
            public string Type => BCThongTinTruongDiemActionTypes.SET_LOADING;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record ClearErrorAction() : IBCThongTinTruongDiemAction
        {
            public string Type => BCThongTinTruongDiemActionTypes.CLEAR_ERROR;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record ShowNotificationAction : IBCThongTinTruongDiemAction
        {
            public string Type => BCThongTinTruongDiemActionTypes.SHOW_NOTIFICATION;
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
