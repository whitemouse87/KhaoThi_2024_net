using Fluxor;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;

namespace KhaoThi_2024_net_client.Services.BC_4_LanhDaoDonVi
{
    [FeatureState]
    public class BCLanhDaoDonViState
    {
        // Constructor mặc định gọi đến GetInitialState
        public BCLanhDaoDonViState()
            : this(0, null, false, null, null, null, null, false, null, null)
        {
        }


        public BCLanhDaoDonViState(
            int id,
            string? maTruong,
            bool isLoading,
            string? errorMessage,
            IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>? lanhDaos,
            KhaoThi_4_THPT_ThongTin_LanhDaoModel? selectedLanhDao,
            PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>? paginatedLanhDaos,
            bool isInitialized,
            string? notificationMessage = null,
            string? notificationType = null)
        {
            Id = id;
            MaTruong = maTruong;
            IsLoading = isLoading;
            ErrorMessage = errorMessage;
            LanhDaos = lanhDaos;
            SelectedLanhDao = selectedLanhDao;
            PaginatedLanhDaos = paginatedLanhDaos;
            IsInitialized = isInitialized;
            NotificationMessage = notificationMessage;
            NotificationType = notificationType;
        }

        public int Id { get; }
        public string? MaTruong { get; }
        public bool IsLoading { get; }
        public string? ErrorMessage { get; }
        public IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>? LanhDaos { get; }
        public KhaoThi_4_THPT_ThongTin_LanhDaoModel? SelectedLanhDao { get; }
        public PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>? PaginatedLanhDaos { get; }
        public bool IsInitialized { get; }
        public string? NotificationMessage { get; }
        public string? NotificationType { get; }

        public static BCLanhDaoDonViState GetInitialState() => new(
            id: 0,
            maTruong: null,
            isLoading: false,
            errorMessage: null,
            lanhDaos: null,
            selectedLanhDao: null,
            paginatedLanhDaos: null,
            isInitialized: false,
            notificationMessage: null,
            notificationType: null
        );

        public BCLanhDaoDonViState With(
            int? id = null,
            string? maTruong = null,
            bool? isLoading = null,
            string? errorMessage = null,
            IEnumerable<KhaoThi_4_THPT_ThongTin_LanhDaoModel>? lanhDaos = null,
            KhaoThi_4_THPT_ThongTin_LanhDaoModel? selectedLanhDao = null,
            PaginatedResult<KhaoThi_4_THPT_ThongTin_LanhDaoModel>? paginatedLanhDaos = null,
            bool? isInitialized = null,
            string? notificationMessage = null,
            string? notificationType = null)
        {
            return new BCLanhDaoDonViState(
                id ?? Id,
                maTruong ?? MaTruong,
                isLoading ?? IsLoading,
                errorMessage ?? ErrorMessage,
                lanhDaos ?? LanhDaos,
                selectedLanhDao ?? SelectedLanhDao,
                paginatedLanhDaos ?? PaginatedLanhDaos,
                isInitialized ?? IsInitialized,
                notificationMessage ?? NotificationMessage,
                notificationType ?? NotificationType
            );
        }

    }
}