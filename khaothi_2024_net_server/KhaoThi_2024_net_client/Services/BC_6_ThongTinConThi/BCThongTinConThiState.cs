using Fluxor;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;


namespace khaothi_2024_net_client.Services.BC_5_ThongTinConThi
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
            IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel>? conThis,
            KhaoThi_5_THPT_ThongTin_ConThiModel? selectedConThi,
            PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThiModel>? paginatedConThis,
            bool isInitialized,
            string? notificationMessage = null,
            string? notificationType = null)
        {
            Id = id;
            MaTruong = maTruong;
            IsLoading = isLoading;
            ErrorMessage = errorMessage;
            ConThis = conThis;
            SelectedConThi = selectedConThi;
            PaginatedConThis = paginatedConThis;
            IsInitialized = isInitialized;
            NotificationMessage = notificationMessage;
            NotificationType = notificationType;
        }

        public int Id { get; }
        public string? MaTruong { get; }
        public string? KyThiThamDu { get; }
        public bool IsLoading { get; }
        public string? ErrorMessage { get; }
        public IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel>? ConThis { get; }
        public KhaoThi_5_THPT_ThongTin_ConThiModel? SelectedConThi { get; }
        public PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThiModel>? PaginatedConThis { get; }
        public bool IsInitialized { get; }
        public string? NotificationMessage { get; }
        public string? NotificationType { get; }

        public static BCThongTinConThiState GetInitialState() => new(
             id: 0,
             maTruong: null,
             isLoading: false,
             errorMessage: null,
             conThis: null,
             selectedConThi: null,
             paginatedConThis: null,
             isInitialized: false,
             notificationMessage: null,
             notificationType: null
         );

        public BCThongTinConThiState With(
            int? id = null,
            string? maTruong = null,
            bool? isLoading = null,
            string? errorMessage = null,
            IEnumerable<KhaoThi_5_THPT_ThongTin_ConThiModel>? conThis = null,
            KhaoThi_5_THPT_ThongTin_ConThiModel? selectedConThi = null,
            PaginatedResult<KhaoThi_5_THPT_ThongTin_ConThiModel>? paginatedConThis = null,
            bool? isInitialized = null,
            string? notificationMessage = null,
            string? notificationType = null)
        {
            return new BCThongTinConThiState(
                id ?? Id,
                maTruong ?? MaTruong,
                isLoading ?? IsLoading,
                errorMessage ?? ErrorMessage,
                conThis ?? ConThis,
                selectedConThi ?? SelectedConThi,
                paginatedConThis ?? PaginatedConThis,
                isInitialized ?? IsInitialized,
                notificationMessage ?? NotificationMessage,
                notificationType ?? NotificationType
            );
        }
    }
}