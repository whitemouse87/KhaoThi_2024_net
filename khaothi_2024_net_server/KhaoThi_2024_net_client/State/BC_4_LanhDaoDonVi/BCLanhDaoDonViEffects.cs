using Fluxor;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Services.BC_4_LanhDaoDonVi;
using KhaoThi_2024_net_client.Services.Logging;
using static KhaoThi_2024_net_client.Services.BC_4_LanhDaoDonVi.BCLanhDaoDonViAction;

namespace KhaoThi_2024_net_client.State.BC_4_LanhDaoDonVi
{
    public class BCLanhDaoDonViEffects
    {
        private readonly IBCLanhDaoDonViService _bcLanhDaoService;

        public BCLanhDaoDonViEffects(IBCLanhDaoDonViService bcLanhDaoService)
        {
            _bcLanhDaoService = bcLanhDaoService;
        }

        [EffectMethod]
        public async Task HandleLoadLanhDaos(LoadLanhDaosAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var lanhDaos = await _bcLanhDaoService.GetAllAsync();
                dispatcher.Dispatch(new LoadLanhDaosSuccessAction(lanhDaos));
            }
            catch (Exception ex)
            {
                await Logger.Error("Lỗi khi tải danh sách lãnh đạo", ex, nameof(BCLanhDaoDonViEffects));
                dispatcher.Dispatch(new LoadLanhDaosFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleLoadPaginatedLanhDaos(LoadPaginatedLanhDaosAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var result = await _bcLanhDaoService.GetPaginatedAsync(
                    action.Page,
                    action.PageSize,
                    action.SearchTerm,
                    action.MaTruong,
                    action.NamSinh,
                    action.CCCD);

                dispatcher.Dispatch(new LoadPaginatedLanhDaosSuccessAction(result));
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi mạng cụ thể
                await Logger.Error(
                    $"Lỗi kết nối khi tải danh sách lãnh đạo: Page={action.Page}, PageSize={action.PageSize}",
                    ex,
                    nameof(BCLanhDaoDonViEffects));

                dispatcher.Dispatch(new LoadPaginatedLanhDaosFailureAction("Lỗi kết nối đến máy chủ. Vui lòng thử lại sau."));
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                await Logger.Error(
                    $"Lỗi xử lý khi tải danh sách lãnh đạo: Page={action.Page}, PageSize={action.PageSize}",
                    ex,
                    nameof(BCLanhDaoDonViEffects));

                dispatcher.Dispatch(new LoadPaginatedLanhDaosFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleGetLanhDaoByMaTruong(GetLanhDaoByMaTruongAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var lanhDaos = await _bcLanhDaoService.GetByMaTruongAsync(action.MaTruong);

                if (lanhDaos != null && lanhDaos.Any())
                {
                    dispatcher.Dispatch(new GetLanhDaoByMaTruongSuccessAction(lanhDaos));
                }
                else
                {
                    dispatcher.Dispatch(new GetLanhDaoByMaTruongSuccessAction(Enumerable.Empty<KhaoThi_4_THPT_ThongTin_LanhDaoModel>()));
                }
            }
            catch (Exception ex)
            {
                await Logger.Error(
                    $"Lỗi khi lấy thông tin lãnh đạo theo mã trường: {action.MaTruong}",
                    ex,
                    nameof(BCLanhDaoDonViEffects));

                dispatcher.Dispatch(new GetLanhDaoByMaTruongFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleGetLanhDaoByMaTruongAndCCCD(GetLanhDaoByMaTruongAndCCCDAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var lanhDao = await _bcLanhDaoService.GetByMaTruongAndCCCDAsync(action.MaTruong, action.CCCD);

                if (lanhDao != null)
                {
                    dispatcher.Dispatch(new GetLanhDaoByMaTruongAndCCCDSuccessAction(lanhDao));
                }
                else
                {
                    dispatcher.Dispatch(new GetLanhDaoByMaTruongAndCCCDFailureAction($"Không tìm thấy lãnh đạo với mã trường {action.MaTruong} và CCCD {action.CCCD}"));
                }
            }
            catch (Exception ex)
            {
                await Logger.Error(
                    $"Lỗi khi lấy thông tin lãnh đạo theo mã trường và CCCD: {action.MaTruong}, {action.CCCD}",
                    ex,
                    nameof(BCLanhDaoDonViEffects));

                dispatcher.Dispatch(new GetLanhDaoByMaTruongAndCCCDFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleCreateLanhDao(CreateLanhDaoAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var success = await _bcLanhDaoService.CreateAsync(action.LanhDao);

                if (success)
                {
                    dispatcher.Dispatch(new CreateLanhDaoSuccessAction());
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Tạo mới lãnh đạo thành công",
                        "success"
                    ));

                    // Nếu có mã trường, tải lại danh sách lãnh đạo của trường đó
                    if (!string.IsNullOrEmpty(action.LanhDao.MaTruong))
                    {
                        dispatcher.Dispatch(new GetLanhDaoByMaTruongAction(action.LanhDao.MaTruong));
                    }
                }
                else
                {
                    var errorMessage = "Không thể tạo lãnh đạo. Thông tin lãnh đạo này có thể đã tồn tại.";
                    dispatcher.Dispatch(new CreateLanhDaoFailureAction(errorMessage));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        errorMessage,
                        "error"
                    ));
                }
            }
            catch (Exception ex)
            {
                var errorMessage = "Có lỗi xảy ra khi tạo lãnh đạo";
                await Logger.Error(
                    $"Lỗi khi tạo lãnh đạo: {action.LanhDao.MaTruong}, {action.LanhDao.CCCD}, {action.LanhDao.HoTen}",
                    ex,
                    nameof(BCLanhDaoDonViEffects)
                );

                dispatcher.Dispatch(new CreateLanhDaoFailureAction(errorMessage));
                dispatcher.Dispatch(new ShowNotificationAction(
                    $"{errorMessage}: {ex.Message}",
                    "error"
                ));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleUpdateLanhDao(UpdateLanhDaoAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var success = await _bcLanhDaoService.UpdateAsync(action.LanhDao);

                if (success)
                {
                    dispatcher.Dispatch(new UpdateLanhDaoSuccessAction(action.LanhDao));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Cập nhật lãnh đạo thành công",
                        "success"
                    ));

                    // Nếu có mã trường, tải lại danh sách lãnh đạo của trường đó
                    if (!string.IsNullOrEmpty(action.LanhDao.MaTruong))
                    {
                        dispatcher.Dispatch(new GetLanhDaoByMaTruongAction(action.LanhDao.MaTruong));
                    }
                }
                else
                {
                    dispatcher.Dispatch(new UpdateLanhDaoFailureAction("Không thể cập nhật lãnh đạo"));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Không thể cập nhật lãnh đạo. Vui lòng thử lại",
                        "error"
                    ));
                }
            }
            catch (Exception ex)
            {
                var errorMessage = "Có lỗi xảy ra khi cập nhật lãnh đạo";
                await Logger.Error(
                    $"Lỗi khi cập nhật lãnh đạo: {action.LanhDao.MaTruong}, {action.LanhDao.CCCD}",
                    ex,
                    nameof(BCLanhDaoDonViEffects)
                );

                dispatcher.Dispatch(new UpdateLanhDaoFailureAction(errorMessage));
                dispatcher.Dispatch(new ShowNotificationAction(
                    $"{errorMessage}: {ex.Message}",
                    "error"
                ));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleDeleteLanhDao(DeleteLanhDaoAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var success = await _bcLanhDaoService.DeleteAsync(action.MaTruong, action.CCCD);

                if (success)
                {
                    dispatcher.Dispatch(new DeleteLanhDaoSuccessAction(action.MaTruong, action.CCCD));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Xóa lãnh đạo thành công",
                        "success"
                    ));

                    // Tải lại danh sách lãnh đạo sau khi xóa
                    if (!string.IsNullOrEmpty(action.MaTruong))
                    {
                        dispatcher.Dispatch(new GetLanhDaoByMaTruongAction(action.MaTruong));
                    }
                }
                else
                {
                    dispatcher.Dispatch(new DeleteLanhDaoFailureAction($"Không thể xóa lãnh đạo với mã trường {action.MaTruong} và CCCD {action.CCCD}"));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Xóa lãnh đạo thất bại. Vui lòng thử lại.",
                        "error"
                    ));
                }
            }
            catch (Exception ex)
            {
                await Logger.Error($"Lỗi khi xóa lãnh đạo: {action.MaTruong}, {action.CCCD}", ex, nameof(BCLanhDaoDonViEffects));
                dispatcher.Dispatch(new DeleteLanhDaoFailureAction(ex.Message));
                dispatcher.Dispatch(new ShowNotificationAction(
                    $"Lỗi khi xóa lãnh đạo: {ex.Message}",
                    "error"
                ));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleCheckLanhDaoExist(CheckLanhDaoExistAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var exists = await _bcLanhDaoService.ExistsAsync(action.MaTruong, action.CCCD);

                dispatcher.Dispatch(new CheckLanhDaoExistSuccessAction(exists));
            }
            catch (Exception ex)
            {
                await Logger.Error(
                    $"Lỗi khi kiểm tra tồn tại lãnh đạo: {action.MaTruong}, {action.CCCD}",
                    ex,
                    nameof(BCLanhDaoDonViEffects));

                dispatcher.Dispatch(new CheckLanhDaoExistFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }
        [EffectMethod]
        public async Task HandleCheckLanhDaoEmpty(CheckLanhDaoEmptyAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var lanhDaos = await _bcLanhDaoService.GetByMaTruongAsync(action.MaTruong);
                bool isEmpty = lanhDaos == null || !lanhDaos.Any();

                dispatcher.Dispatch(new CheckLanhDaoEmptySuccessAction(isEmpty));

                if (isEmpty)
                {
                    // Hiển thị thông báo lỗi (màu đỏ) thay vì thông báo info
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Trường của bạn chưa khai báo thông tin lãnh đạo. Vui lòng quay lại Bước 3 để khai báo thông tin lãnh đạo trước khi tiếp tục.",
                        "error"
                    ));
                }
            }
            catch (Exception ex)
            {
                await Logger.Error(
                    $"Lỗi khi kiểm tra danh sách lãnh đạo rỗng: {action.MaTruong}",
                    ex,
                    nameof(BCLanhDaoDonViEffects));

                dispatcher.Dispatch(new CheckLanhDaoEmptyFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }
    }
}