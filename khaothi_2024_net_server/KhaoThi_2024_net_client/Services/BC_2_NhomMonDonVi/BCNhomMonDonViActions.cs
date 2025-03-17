using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;

namespace KhaoThi_2024_net_client.Services.BC_2_NhomMonDonVi
{
    public static class BCNhomMonActionTypes
    {
        // Tải danh sách nhóm môn
        public const string LOAD_NHOMMONS = "[Nhóm môn] Tải danh sách";
        public const string LOAD_NHOMMONS_SUCCESS = "[Nhóm môn] Tải danh sách thành công";
        public const string LOAD_NHOMMONS_FAILURE = "[Nhóm môn] Tải danh sách thất bại";

        // Lựa chọn nhóm môn
        public const string SELECT_NHOMMON = "[Nhóm môn] Lựa chọn nhóm môn";

        // Tìm nhóm môn theo mã trường
        public const string GET_NHOMMON_BY_MA_TRUONG = "[Nhóm môn] Tìm theo mã trường";
        public const string GET_NHOMMON_BY_MA_TRUONG_SUCCESS = "[Nhóm môn] Tìm theo mã trường thành công";
        public const string GET_NHOMMON_BY_MA_TRUONG_FAILURE = "[Nhóm môn] Tìm theo mã trường thất bại";

        // Phân trang
        public const string LOAD_PAGINATED = "[Nhóm môn] Tải danh sách phân trang";
        public const string LOAD_PAGINATED_SUCCESS = "[Nhóm môn] Tải phân trang thành công";
        public const string LOAD_PAGINATED_FAILURE = "[Nhóm môn] Tải phân trang thất bại";

        // Tạo mới nhóm môn
        public const string CREATE_NHOMMON = "[Nhóm môn] Tạo mới nhóm môn";
        public const string CREATE_NHOMMON_SUCCESS = "[Nhóm môn] Tạo mới thành công";
        public const string CREATE_NHOMMON_FAILURE = "[Nhóm môn] Tạo mới thất bại";

        // Cập nhật thông tin
        public const string UPDATE_NHOMMON = "[Nhóm môn] Cập nhật thông tin";
        public const string UPDATE_NHOMMON_SUCCESS = "[Nhóm môn] Cập nhật thành công";
        public const string UPDATE_NHOMMON_FAILURE = "[Nhóm môn] Cập nhật thất bại";

        // Xóa nhóm môn
        public const string DELETE_NHOMMON = "[Nhóm môn] Xóa nhóm môn";
        public const string DELETE_NHOMMON_SUCCESS = "[Nhóm môn] Xóa thành công";
        public const string DELETE_NHOMMON_FAILURE = "[Nhóm môn] Xóa thất bại";

        // Kiểm tra tồn tại
        public const string CHECK_NHOMMON_EXIST = "[Nhóm môn] Kiểm tra tồn tại";
        public const string CHECK_NHOMMON_EXIST_SUCCESS = "[Nhóm môn] Kiểm tra tồn tại thành công";
        public const string CHECK_NHOMMON_EXIST_FAILURE = "[Nhóm môn] Kiểm tra tồn tại thất bại";

        // Cập nhật Lock
        public const string UPDATE_LOCK = "[Nhóm môn] Khóa nhóm môn";
        public const string UPDATE_LOCK_SUCCESS = "[Nhóm môn] Khóa thành công";
        public const string UPDATE_LOCK_FAILURE = "[Nhóm môn] Khóa thất bại";

        // Thông báo và trạng thái
        public const string SHOW_NOTIFICATION = "[Nhóm môn] Hiển thị thông báo";
        public const string SET_LOADING = "[Nhóm môn] Đang tải";
        public const string CLEAR_ERROR = "[Nhóm môn] Xóa lỗi";
    }
    public interface IBCNhomMonDonViAction
    {
        string Type { get; }
        DateTime Timestamp { get; }
    }
    public static class BCNhomMonDonViActions
    {
        public record LoadNhomMonsAction() : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.LOAD_NHOMMONS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }
        public record LoadNhomMonsSuccessAction(IEnumerable<KhaoThi_2_THPT_NhomMon_DonViModel> NhomMons) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.LOAD_NHOMMONS_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadNhomMonsFailureAction(string ErrorMessage) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.LOAD_NHOMMONS_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions phân trang
        public record LoadPaginatedNhomMonsAction(int Page, int PageSize, string? SearchTerm, string? MaTruong = null, int? MinSoLuong = null) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.LOAD_PAGINATED;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadPaginatedNhomMonsSuccessAction(PaginatedResult<KhaoThi_2_THPT_NhomMon_DonViModel> PaginatedNhomMons) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.LOAD_PAGINATED_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadPaginatedNhomMonsFailureAction(string ErrorMessage) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.LOAD_PAGINATED_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions lựa chọn nhóm môn
        public record SelectNhomMonAction(KhaoThi_2_THPT_NhomMon_DonViModel NhomMon) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.SELECT_NHOMMON;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions tạo nhóm môn mới
        public record CreateNhomMonAction(KhaoThi_2_THPT_NhomMon_DonViModel NhomMon) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.CREATE_NHOMMON;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record CreateNhomMonSuccessAction(int Id) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.CREATE_NHOMMON_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record CreateNhomMonFailureAction(string ErrorMessage) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.CREATE_NHOMMON_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions cập nhật thông tin
        public record UpdateNhomMonAction(KhaoThi_2_THPT_NhomMon_DonViModel NhomMon) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.UPDATE_NHOMMON;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record UpdateNhomMonSuccessAction(KhaoThi_2_THPT_NhomMon_DonViModel NhomMon) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.UPDATE_NHOMMON_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record UpdateNhomMonFailureAction(string ErrorMessage) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.UPDATE_NHOMMON_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions xóa nhóm môn
        public record DeleteNhomMonAction(int Id) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.DELETE_NHOMMON;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record DeleteNhomMonSuccessAction(int Id) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.DELETE_NHOMMON_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record DeleteNhomMonFailureAction(string ErrorMessage) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.DELETE_NHOMMON_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions cập nhật Lock
        public record ChangeLockAction(int Id, bool Lock) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.UPDATE_LOCK;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record ChangeLockSuccessAction(int Id) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.UPDATE_LOCK_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record ChangeLockFailureAction(string ErrorMessage) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.UPDATE_LOCK_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions tìm nhóm môn qua mã trường
        public record GetNhomMonByMaTruongAction(string MaTruong) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.GET_NHOMMON_BY_MA_TRUONG;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record GetNhomMonByMaTruongSuccessAction(IEnumerable<KhaoThi_2_THPT_NhomMon_DonViModel> NhomMons) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.GET_NHOMMON_BY_MA_TRUONG_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record GetNhomMonByMaTruongFailureAction(string ErrorMessage) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.GET_NHOMMON_BY_MA_TRUONG_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions kiểm tra tồn tại nhóm môn
        public record CheckNhomMonExistAction(string MaTruong, string MonLuaChon1, string MonLuaChon2) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.CHECK_NHOMMON_EXIST;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record CheckNhomMonExistSuccessAction(bool Exists) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.CHECK_NHOMMON_EXIST_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record CheckNhomMonExistFailureAction(string ErrorMessage) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.CHECK_NHOMMON_EXIST_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions cho trạng thái loading và clear lỗi
        public record SetLoadingAction(bool IsLoading) : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.SET_LOADING;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record ClearErrorAction() : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.CLEAR_ERROR;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record ShowNotificationAction : IBCNhomMonDonViAction
        {
            public string Type => BCNhomMonActionTypes.SHOW_NOTIFICATION;
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
