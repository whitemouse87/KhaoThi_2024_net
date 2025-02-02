using Fluxor;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.Users;

namespace KhaoThi_2024_net_client.Services.User
{
    /// <summary>
    /// State quản lý trạng thái người dùng trong ứng dụng
    /// </summary>
    /// <summary>
    /// State quản lý trạng thái người dùng trong ứng dụng
    /// </summary>
    [FeatureState]
    public class KhaoThiUserState
    {
        /// <summary>
        /// Constructor mặc định cho Fluxor serialization
        /// </summary>
        public KhaoThiUserState() { }

        /// <summary>
        /// Constructor với tham số để khởi tạo state
        /// </summary>
        public KhaoThiUserState(
            bool isLoading,
            string? errorMessage,
            IEnumerable<KhaoThiUserModel>? users,
            KhaoThiUserModel? selectedUser,
            PaginatedResult<KhaoThiUserModel>? paginatedUsers,
            bool isInitialized)
        {
            IsLoading = isLoading;
            ErrorMessage = errorMessage;
            Users = users;
            SelectedUser = selectedUser;
            PaginatedUsers = paginatedUsers;
            IsInitialized = isInitialized;
        }

        /// <summary>
        /// Flag đánh dấu trạng thái đang tải dữ liệu
        /// </summary>
        public bool IsLoading { get; }

        /// <summary>
        /// Thông báo lỗi nếu có
        /// </summary>
        public string? ErrorMessage { get; }

        /// <summary>
        /// Danh sách người dùng hiện tại
        /// </summary>
        public IEnumerable<KhaoThiUserModel>? Users { get; }

        /// <summary>
        /// Người dùng đang được chọn
        /// </summary>
        public KhaoThiUserModel? SelectedUser { get; }

        /// <summary>
        /// Kết quả phân trang
        /// </summary>
        public PaginatedResult<KhaoThiUserModel>? PaginatedUsers { get; }

        /// <summary>
        /// Flag đánh dấu state đã được khởi tạo
        /// </summary>
        public bool IsInitialized { get; }

        /// <summary>
        /// State mặc định ban đầu
        /// </summary>
        public static KhaoThiUserState GetInitialState() => new(
            isLoading: false,
            errorMessage: null,
            users: null,
            selectedUser: null,
            paginatedUsers: null,
            isInitialized: false
        );
    }

}
