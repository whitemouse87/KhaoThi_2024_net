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
        public KhaoThiUserState() { }

        public KhaoThiUserState(
            bool isLoading,
            string? errorMessage,
            IEnumerable<KhaoThiUserModel>? users,
            KhaoThiUserModel? selectedUser,
            PaginatedResult<KhaoThiUserModel>? paginatedUsers,
            bool isInitialized,
            string? notificationMessage = null,
            string? notificationType = null)
        {
            IsLoading = isLoading;
            ErrorMessage = errorMessage;
            Users = users;
            SelectedUser = selectedUser;
            PaginatedUsers = paginatedUsers;
            IsInitialized = isInitialized;
            NotificationMessage = notificationMessage;
            NotificationType = notificationType;
        }

        public bool IsLoading { get; }
        public string? ErrorMessage { get; }
        public IEnumerable<KhaoThiUserModel>? Users { get; }
        public KhaoThiUserModel? SelectedUser { get; }
        public PaginatedResult<KhaoThiUserModel>? PaginatedUsers { get; }
        public bool IsInitialized { get; }
        public string? NotificationMessage { get; }
        public string? NotificationType { get; }

        public static KhaoThiUserState GetInitialState() => new(
            isLoading: false,
            errorMessage: null,
            users: null,
            selectedUser: null,
            paginatedUsers: null,
            isInitialized: false,
            notificationMessage: null,
            notificationType: null
        );
    }

}
