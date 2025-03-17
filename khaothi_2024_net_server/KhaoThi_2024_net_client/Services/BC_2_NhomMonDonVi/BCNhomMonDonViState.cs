using Fluxor;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Services.BC_1_ThongTinDonVi;
using Radzen;
namespace KhaoThi_2024_net_client.Services.BC_2_NhomMonDonVi
{
    [FeatureState]
    public class BCNhomMonDonViState
    {
        public BCNhomMonDonViState()
             : this(0, null, false, null, null, null, null, false, null, null)
        {
        }
        public int Id { get; }
        public string? MaTruong { get; }
        public bool IsLoading { get; }
        public string? ErrorMessage { get; }
        public IEnumerable<KhaoThi_2_THPT_NhomMon_DonViModel>? DonVis { get; }
        public KhaoThi_2_THPT_NhomMon_DonViModel? SelectedDonVi { get; }
        public PaginatedResult<KhaoThi_2_THPT_NhomMon_DonViModel>? PaginatedDonVis { get; }
        public bool IsInitialized { get; }
        public string? NotificationMessage { get; }
        public string? NotificationType { get; }
        public BCNhomMonDonViState(
           int id,
           string? maTruong,
           bool isLoading,
           string? errorMessage,
           IEnumerable<KhaoThi_2_THPT_NhomMon_DonViModel>? donVis,
           KhaoThi_2_THPT_NhomMon_DonViModel? selectedDonVi,
           PaginatedResult<KhaoThi_2_THPT_NhomMon_DonViModel>? paginatedDonVis,
           bool isInitialized,
           string? notificationMessage = null,
           string? notificationType = null)
        {
            Id = id;
            MaTruong = maTruong;
            IsLoading = isLoading;
            ErrorMessage = errorMessage;
            DonVis = donVis;
            SelectedDonVi = selectedDonVi;
            PaginatedDonVis = paginatedDonVis;
            IsInitialized = isInitialized;
            NotificationMessage = notificationMessage;
            NotificationType = notificationType;
        }
        public static BCNhomMonDonViState GetInitialState() => new(
           id: 0,
           maTruong: null,
           isLoading: false,
           errorMessage: null,
           donVis: null,
           selectedDonVi: null,
           paginatedDonVis: null,
           isInitialized: false,
           notificationMessage: null,
           notificationType: null
       );

        public BCNhomMonDonViState With(
            int? id = null,
            string? maTruong = null,
            bool? isLoading = null,
            string? errorMessage = null,
            IEnumerable<KhaoThi_2_THPT_NhomMon_DonViModel>? donVis = null,
            KhaoThi_2_THPT_NhomMon_DonViModel? selectedDonVi = null,
            PaginatedResult<KhaoThi_2_THPT_NhomMon_DonViModel>? paginatedDonVis = null,
            bool? isInitialized = null,
            string? notificationMessage = null,
            string? notificationType = null)
        {
            return new BCNhomMonDonViState(
                id ?? Id,
                maTruong ?? MaTruong,
                isLoading ?? IsLoading,
                errorMessage ?? ErrorMessage,
                donVis ?? DonVis,
                selectedDonVi ?? SelectedDonVi,
                paginatedDonVis ?? PaginatedDonVis,
                isInitialized ?? IsInitialized,
                notificationMessage ?? NotificationMessage,
                notificationType ?? NotificationType
            );
        }
    }
}
