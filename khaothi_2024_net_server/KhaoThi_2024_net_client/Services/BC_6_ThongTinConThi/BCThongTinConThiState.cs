using Fluxor;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;

namespace KhaoThi_2024_net_client.Services.BC_6_ThongTinConThi
{
    [FeatureState]
    public class BCThongTinConThiState
    {
        // Constructor mặc định gọi đến GetInitialState
        public BCThongTinConThiState()
            : this(0, null, false, null, null, null, null, false, null, null)
        {
        }

        public BCThongTinConThiState(
            int id,
            string? maTruong,
            bool isLoading,
            string? errorMessage,
            IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi>? conThiList,
            KhaoThi_5_THPT_ThongTin_ConThi? selectedConThi,
            PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThi>? paginatedConThi,
            bool isInitialized,
            string? notificationMessage = null,
            string? notificationType = null)
        {
            Id = id;
            MaTruong = maTruong;
            IsLoading = isLoading;
            ErrorMessage = errorMessage;
            ConThiList = conThiList;
            SelectedConThi = selectedConThi;
            PaginatedConThi = paginatedConThi;
            IsInitialized = isInitialized;
            NotificationMessage = notificationMessage;
            NotificationType = notificationType;
        }

        public int Id { get; }
        public string? MaTruong { get; }
        public bool IsLoading { get; }
        public string? ErrorMessage { get; }
        public IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi>? ConThiList { get; }
        public KhaoThi_5_THPT_ThongTin_ConThi? SelectedConThi { get; }
        public PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThi>? PaginatedConThi { get; }
        public bool IsInitialized { get; }
        public string? NotificationMessage { get; }
        public string? NotificationType { get; }

        public static BCThongTinConThiState GetInitialState() => new(
            id: 0,
            maTruong: null,
            isLoading: false,
            errorMessage: null,
            conThiList: null,
            selectedConThi: null,
            paginatedConThi: null,
            isInitialized: false,
            notificationMessage: null,
            notificationType: null
        );

        public BCThongTinConThiState With(
            int? id = null,
            string? maTruong = null,
            bool? isLoading = null,
            string? errorMessage = null,
            IEnumerable<KhaoThi_5_THPT_ThongTin_ConThi>? conThiList = null,
            KhaoThi_5_THPT_ThongTin_ConThi? selectedConThi = null,
            PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThi>? paginatedConThi = null,
            bool? isInitialized = null,
            string? notificationMessage = null,
            string? notificationType = null)
        {
            return new BCThongTinConThiState(
                id ?? Id,
                maTruong ?? MaTruong,
                isLoading ?? IsLoading,
                errorMessage ?? ErrorMessage,
                conThiList ?? ConThiList,
                selectedConThi ?? SelectedConThi,
                paginatedConThi ?? PaginatedConThi,
                isInitialized ?? IsInitialized,
                notificationMessage ?? NotificationMessage,
                notificationType ?? NotificationType
            );
        }
    }
}
