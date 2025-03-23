using Fluxor;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;

namespace KhaoThi_2024_net_client.Services.BC_5_TruongDiemDonVi
{
    [FeatureState]
    public class BCThongTinTruongDiemState
    {
        // Constructor mặc định gọi đến GetInitialState
        public BCThongTinTruongDiemState()
            : this(0, null, false, null, null, null, false, null, null)
        {
        }

        public BCThongTinTruongDiemState(
            int id,
            string? maTruong,
            bool isLoading,
            string? errorMessage,
            KhaoThi_4_THPT_ThongTin_LanhDaoModel? selectedTruongDiem,
            PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>? paginatedTruongDiems,
            bool isInitialized,
            string? notificationMessage = null,
            string? notificationType = null)
        {
            Id = id;
            MaTruong = maTruong;
            IsLoading = isLoading;
            ErrorMessage = errorMessage;
            SelectedTruongDiem = selectedTruongDiem;
            PaginatedTruongDiems = paginatedTruongDiems;
            IsInitialized = isInitialized;
            NotificationMessage = notificationMessage;
            NotificationType = notificationType;
        }

        public int Id { get; }
        public string? MaTruong { get; }
        public bool IsLoading { get; }
        public string? ErrorMessage { get; }
        public KhaoThi_4_THPT_ThongTin_LanhDaoModel? SelectedTruongDiem { get; }
        public PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>? PaginatedTruongDiems { get; }
        public bool IsInitialized { get; }
        public string? NotificationMessage { get; }
        public string? NotificationType { get; }

        public static BCThongTinTruongDiemState GetInitialState() => new(
            id: 0,
            maTruong: null,
            isLoading: false,
            errorMessage: null,
            selectedTruongDiem: null,
            paginatedTruongDiems: null,
            isInitialized: false,
            notificationMessage: null,
            notificationType: null
        );

        public BCThongTinTruongDiemState With(
            int? id = null,
            string? maTruong = null,
            bool? isLoading = null,
            string? errorMessage = null,
            KhaoThi_4_THPT_ThongTin_LanhDaoModel? selectedTruongDiem = null,
            PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>? paginatedTruongDiems = null,
            bool? isInitialized = null,
            string? notificationMessage = null,
            string? notificationType = null)
        {
            return new BCThongTinTruongDiemState(
                id ?? Id,
                maTruong ?? MaTruong,
                isLoading ?? IsLoading,
                errorMessage ?? ErrorMessage,
                selectedTruongDiem ?? SelectedTruongDiem,
                paginatedTruongDiems ?? PaginatedTruongDiems,
                isInitialized ?? IsInitialized,
                notificationMessage ?? NotificationMessage,
                notificationType ?? NotificationType
            );
        }
    }
}
