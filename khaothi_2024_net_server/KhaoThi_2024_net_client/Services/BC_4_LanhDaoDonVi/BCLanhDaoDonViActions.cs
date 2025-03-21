using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;

namespace KhaoThi_2024_net_client.Services.BC_4_LanhDaoDonVi
{
    public static class BCLanhDaoDonViActionTypes
    {
        // Tải danh sách lãnh đạo
        public const string LOAD_LANHDAOS = "[Lãnh đạo] Tải danh sách";
        public const string LOAD_LANHDAOS_SUCCESS = "[Lãnh đạo] Tải danh sách thành công";
        public const string LOAD_LANHDAOS_FAILURE = "[Lãnh đạo] Tải danh sách thất bại";

        // Lựa chọn lãnh đạo
        public const string SELECT_LANHDAO = "[Lãnh đạo] Lựa chọn lãnh đạo";

        // Tìm lãnh đạo theo mã trường
        public const string GET_LANHDAO_BY_MA_TRUONG = "[Lãnh đạo] Tìm theo mã trường";
        public const string GET_LANHDAO_BY_MA_TRUONG_SUCCESS = "[Lãnh đạo] Tìm theo mã trường thành công";
        public const string GET_LANHDAO_BY_MA_TRUONG_FAILURE = "[Lãnh đạo] Tìm theo mã trường thất bại";

        // Tìm lãnh đạo theo mã trường và CCCD
        public const string GET_LANHDAO_BY_MA_TRUONG_AND_CCCD = "[Lãnh đạo] Tìm theo mã trường và CCCD";
        public const string GET_LANHDAO_BY_MA_TRUONG_AND_CCCD_SUCCESS = "[Lãnh đạo] Tìm theo mã trường và CCCD thành công";
        public const string GET_LANHDAO_BY_MA_TRUONG_AND_CCCD_FAILURE = "[Lãnh đạo] Tìm theo mã trường và CCCD thất bại";

        // Phân trang
        public const string LOAD_PAGINATED = "[Lãnh đạo] Tải danh sách phân trang";
        public const string LOAD_PAGINATED_SUCCESS = "[Lãnh đạo] Tải phân trang thành công";
        public const string LOAD_PAGINATED_FAILURE = "[Lãnh đạo] Tải phân trang thất bại";

        // Tạo mới lãnh đạo
        public const string CREATE_LANHDAO = "[Lãnh đạo] Tạo mới lãnh đạo";
        public const string CREATE_LANHDAO_SUCCESS = "[Lãnh đạo] Tạo mới thành công";
        public const string CREATE_LANHDAO_FAILURE = "[Lãnh đạo] Tạo mới thất bại";

        // Cập nhật thông tin
        public const string UPDATE_LANHDAO = "[Lãnh đạo] Cập nhật thông tin";
        public const string UPDATE_LANHDAO_SUCCESS = "[Lãnh đạo] Cập nhật thành công";
        public const string UPDATE_LANHDAO_FAILURE = "[Lãnh đạo] Cập nhật thất bại";

        // Xóa lãnh đạo
        public const string DELETE_LANHDAO = "[Lãnh đạo] Xóa lãnh đạo";
        public const string DELETE_LANHDAO_SUCCESS = "[Lãnh đạo] Xóa thành công";
        public const string DELETE_LANHDAO_FAILURE = "[Lãnh đạo] Xóa thất bại";

        // Kiểm tra tồn tại
        public const string CHECK_LANHDAO_EXIST = "[Lãnh đạo] Kiểm tra tồn tại";
        public const string CHECK_LANHDAO_EXIST_SUCCESS = "[Lãnh đạo] Kiểm tra tồn tại thành công";
        public const string CHECK_LANHDAO_EXIST_FAILURE = "[Lãnh đạo] Kiểm tra tồn tại thất bại";

        // Thông báo và trạng thái
        public const string SHOW_NOTIFICATION = "[Lãnh đạo] Hiển thị thông báo";
        public const string SET_LOADING = "[Lãnh đạo] Đang tải";
        public const string CLEAR_ERROR = "[Lãnh đạo] Xóa lỗi";
    }

    public interface IBCLanhDaoDonViAction
    {
        string Type { get; }
        DateTime Timestamp { get; }
    }

    public static class BCLanhDaoDonViAction
    {
        // Actions tải danh sách lãnh đạo
        public record LoadLanhDaosAction() : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.LOAD_LANHDAOS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadLanhDaosSuccessAction(IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel> LanhDaos) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.LOAD_LANHDAOS_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadLanhDaosFailureAction(string ErrorMessage) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.LOAD_LANHDAOS_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions phân trang
        public record LoadPaginatedLanhDaosAction(int Page, int PageSize, string? SearchTerm, string? MaTruong = null, int? NamSinh = null, string? CCCD = null) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.LOAD_PAGINATED;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadPaginatedLanhDaosSuccessAction(PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel> PaginatedLanhDaos) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.LOAD_PAGINATED_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record LoadPaginatedLanhDaosFailureAction(string ErrorMessage) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.LOAD_PAGINATED_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions lựa chọn lãnh đạo
        public record SelectLanhDaoAction(KhaoThi_4_THPT_ThongTin_LanhDaoModel LanhDao) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.SELECT_LANHDAO;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions tạo lãnh đạo mới
        public record CreateLanhDaoAction(KhaoThi_4_THPT_ThongTin_LanhDaoModel LanhDao) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.CREATE_LANHDAO;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record CreateLanhDaoSuccessAction() : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.CREATE_LANHDAO_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record CreateLanhDaoFailureAction(string ErrorMessage) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.CREATE_LANHDAO_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions cập nhật thông tin
        public record UpdateLanhDaoAction(KhaoThi_4_THPT_ThongTin_LanhDaoModel LanhDao) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.UPDATE_LANHDAO;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record UpdateLanhDaoSuccessAction(KhaoThi_4_THPT_ThongTin_LanhDaoModel LanhDao) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.UPDATE_LANHDAO_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record UpdateLanhDaoFailureAction(string ErrorMessage) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.UPDATE_LANHDAO_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions xóa lãnh đạo
        public record DeleteLanhDaoAction(string MaTruong, string CCCD) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.DELETE_LANHDAO;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record DeleteLanhDaoSuccessAction(string MaTruong, string CCCD) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.DELETE_LANHDAO_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record DeleteLanhDaoFailureAction(string ErrorMessage) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.DELETE_LANHDAO_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions tìm lãnh đạo qua mã trường
        public record GetLanhDaoByMaTruongAction(string MaTruong) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.GET_LANHDAO_BY_MA_TRUONG;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record GetLanhDaoByMaTruongSuccessAction(IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel> LanhDaos) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.GET_LANHDAO_BY_MA_TRUONG_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record GetLanhDaoByMaTruongFailureAction(string ErrorMessage) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.GET_LANHDAO_BY_MA_TRUONG_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions tìm lãnh đạo qua mã trường và CCCD
        public record GetLanhDaoByMaTruongAndCCCDAction(string MaTruong, string CCCD) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.GET_LANHDAO_BY_MA_TRUONG_AND_CCCD;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record GetLanhDaoByMaTruongAndCCCDSuccessAction(KhaoThi_4_THPT_ThongTin_LanhDaoModel LanhDao) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.GET_LANHDAO_BY_MA_TRUONG_AND_CCCD_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record GetLanhDaoByMaTruongAndCCCDFailureAction(string ErrorMessage) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.GET_LANHDAO_BY_MA_TRUONG_AND_CCCD_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions kiểm tra tồn tại lãnh đạo
        public record CheckLanhDaoExistAction(string MaTruong, string CCCD) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.CHECK_LANHDAO_EXIST;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record CheckLanhDaoExistSuccessAction(bool Exists) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.CHECK_LANHDAO_EXIST_SUCCESS;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record CheckLanhDaoExistFailureAction(string ErrorMessage) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.CHECK_LANHDAO_EXIST_FAILURE;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        // Actions cho trạng thái loading và clear lỗi
        public record SetLoadingAction(bool IsLoading) : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.SET_LOADING;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record ClearErrorAction() : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.CLEAR_ERROR;
            public DateTime Timestamp { get; } = DateTime.UtcNow;
        }

        public record ShowNotificationAction : IBCLanhDaoDonViAction
        {
            public string Type => BCLanhDaoDonViActionTypes.SHOW_NOTIFICATION;
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