using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Services.User;

namespace KhaoThi_2024_net_client.Services.BC_1_ThongTinDonVi
{
    public static class BCThongTinDonViActionTypes
    {
        public const string LOAD_DONVIS = "[Đơn vị] Tải danh sách";
        public const string LOAD_DONVIS_SUCCESS = "[Đơn vị] Tải danh sách thành công";
        public const string LOAD_DONVIS_FAILURE = "[Đơn vị] Tải danh sách thất bại";

        // Lựa chọn đơn vị
        public const string SELECT_DONVI = "[Đơn vị] Lựa chọn đơn vị";

        // Tìm đơn vị theo mã trường
        public const string GET_DONVI_BY_MA_TRUONG = "[Đơn vị] Tìm theo mã trường";
        public const string GET_DONVI_BY_MA_TRUONG_SUCCESS = "[Đơn vị] Tìm theo mã trường thành công";
        public const string GET_DONVI_BY_MA_TRUONG_FAILURE = "[Đơn vị] Tìm theo mã trường thất bại";

        // Phân trang
        public const string LOAD_PAGINATED = "[Đơn vị] Tải danh sách phân trang";
        public const string LOAD_PAGINATED_SUCCESS = "[Đơn vị] Tải phân trang thành công";
        public const string LOAD_PAGINATED_FAILURE = "[Đơn vị] Tải phân trang thất bại";

        // Cập nhật thông tin
        public const string UPDATE_DONVI = "[Đơn vị] Cập nhật thông tin báo cáo";
        public const string UPDATE_DONVI_SUCCESS = "[Đơn vị] Cập nhật báo cáo thành công";
        public const string UPDATE_DONVI_FAILURE = "[Đơn vị] Cập nhật báo cáo thất bại";

        // Cập nhật active
        public const string UPDATE_ACTIVE = "[Đơn vị] Thông báo sở hoàn thành bước 1";
        public const string UPDATE_ACTIVE_SUCCESS = "[Đơn vị] Gửi báo cáo thành công";
        public const string UPDATE_ACTIVE_FAILURE = "[Đơn vị] Gửi báo cáo thất bại";

        // Cập nhật Lock
        public const string UPDATE_LOCK = "[Đơn vị] Khóa báo cáo bước 1";
        public const string UPDATE_LOCK_SUCCESS = "[Đơn vị] Khóa thành công";
        public const string UPDATE_LOCK_FAILURE = "[Đơn vị] Khóa thất bại";

        // Thông báo và trạng thái
        public const string SHOW_NOTIFICATION = "[Đơn vị] Hiển thị thông báo";
        public const string SET_LOADING = "[Đơn vị] Đang tải";
        public const string CLEAR_ERROR = "[Đơn vị] Xóa lỗi";
    }

    public interface IBCThongTinDonViAction
    {
        string Type { get; }
        DateTime Timestamp { get; }
    }

    public static class BCThongTinDonViAction
    {
        // Actions tải danh sách đơn vị
        public record LoadDonVisAction() : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.LOAD_DONVIS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadDonVisSuccessAction(IEnumerable<KhaoThi_1_THPT_ThongTin_DonViModel> DonVis) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.LOAD_DONVIS_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadDonVisFailureAction(string ErrorMessage) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.LOAD_DONVIS_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions phân trang
        public record LoadPaginatedDonVisAction(int Page, int PageSize, string? SearchTerm) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.LOAD_PAGINATED;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadPaginatedDonVisSuccessAction(PaginatedResult<KhaoThi_1_THPT_ThongTin_DonViModel> PaginatedDonVis) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.LOAD_PAGINATED_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadPaginatedDonVisFailureAction(string ErrorMessage) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.LOAD_PAGINATED_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions lựa chọn đơn vị
        public record SelectDonViAction(KhaoThi_1_THPT_ThongTin_DonViModel DonVi) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.SELECT_DONVI;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions cập nhật thông tin
        public record UpdateDonViAction(KhaoThi_1_THPT_ThongTin_DonViModel DonVi) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.UPDATE_DONVI;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record UpdateDonViSuccessAction(KhaoThi_1_THPT_ThongTin_DonViModel DonVi) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.UPDATE_DONVI_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record UpdateDonViFailureAction(string ErrorMessage) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.UPDATE_DONVI_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions cập nhật Active
        public record ChangeActiveAction(string MaTruong, bool Active) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.UPDATE_ACTIVE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record ChangeActiveSuccessAction(string MaTruong) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.UPDATE_ACTIVE_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record ChangeActiveFailureAction(string ErrorMessage) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.UPDATE_ACTIVE_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions cập nhật Lock
        public record ChangeLockAction(string MaTruong, bool Lock) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.UPDATE_LOCK;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record ChangeLockSuccessAction(string MaTruong) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.UPDATE_LOCK_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record ChangeLockFailureAction(string ErrorMessage) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.UPDATE_LOCK_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions tìm trường qua mã trường
        public record GetDonViByMaTruongAction(string MaTruong) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.GET_DONVI_BY_MA_TRUONG;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record GetDonViByMaTruongSuccessAction(KhaoThi_1_THPT_ThongTin_DonViModel DonVi) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.GET_DONVI_BY_MA_TRUONG_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record GetDonViByMaTruongFailureAction(string ErrorMessage) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.GET_DONVI_BY_MA_TRUONG_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions cho trạng thái loading và clear lỗi
        public record SetLoadingAction(bool IsLoading) : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.SET_LOADING;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record ClearErrorAction() : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.CLEAR_ERROR;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record ShowNotificationAction : IBCThongTinDonViAction
        {
            public string Type => BCThongTinDonViActionTypes.SHOW_NOTIFICATION;
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