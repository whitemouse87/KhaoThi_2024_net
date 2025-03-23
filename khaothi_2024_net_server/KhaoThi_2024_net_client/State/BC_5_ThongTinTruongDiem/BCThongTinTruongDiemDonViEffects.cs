using Fluxor;
using KhaoThi_2024_net_client.Services.BC_5_TruongDiemDonVi;
using KhaoThi_2024_net_client.Services.Logging;
using static KhaoThi_2024_net_client.Services.BC_5_TruongDiemDonVi.BCThongTinTruongDiemAction;

namespace KhaoThi_2024_net_client.State.BC_5_TruongDiemDonVi
{
    public class BCThongTinTruongDiemEffects
    {
        private readonly IBCThongTinTruongDiemDonViService _bcTruongDiemService;

        public BCThongTinTruongDiemEffects(IBCThongTinTruongDiemDonViService bcTruongDiemService)
        {
            _bcTruongDiemService = bcTruongDiemService;
        }

        [EffectMethod]
        public async Task HandleLoadPaginatedTruongDiems(LoadPaginatedTruongDiemsAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var result = await _bcTruongDiemService.GetPaginatedAsync(
                    action.Page,
                    action.PageSize,
                    action.SearchTerm,
                    action.MaTruong);

                dispatcher.Dispatch(new LoadPaginatedTruongDiemsSuccessAction(result));
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi mạng cụ thể
                await Logger.Error(
                    $"Lỗi kết nối khi tải danh sách lãnh đạo điểm thi: Page={action.Page}, PageSize={action.PageSize}",
                    ex,
                    nameof(BCThongTinTruongDiemEffects));

                dispatcher.Dispatch(new LoadPaginatedTruongDiemsFailureAction("Lỗi kết nối đến máy chủ. Vui lòng thử lại sau."));
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                await Logger.Error(
                    $"Lỗi xử lý khi tải danh sách lãnh đạo điểm thi: Page={action.Page}, PageSize={action.PageSize}",
                    ex,
                    nameof(BCThongTinTruongDiemEffects));

                dispatcher.Dispatch(new LoadPaginatedTruongDiemsFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleUpdateTruongDiem(UpdateTruongDiemAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var success = await _bcTruongDiemService.UpdateAsync(action.TruongDiem);

                if (success)
                {
                    dispatcher.Dispatch(new UpdateTruongDiemSuccessAction(action.TruongDiem));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Cập nhật thông tin lãnh đạo điểm thi thành công",
                        "success"
                    ));
                }
                else
                {
                    dispatcher.Dispatch(new UpdateTruongDiemFailureAction("Không thể cập nhật thông tin lãnh đạo điểm thi"));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Không thể cập nhật thông tin lãnh đạo điểm thi. Vui lòng thử lại",
                        "error"
                    ));
                }
            }
            catch (Exception ex)
            {
                var errorMessage = "Có lỗi xảy ra khi cập nhật thông tin lãnh đạo điểm thi";
                await Logger.Error(
                    $"Lỗi khi cập nhật thông tin lãnh đạo điểm thi. MaTruong: {action.TruongDiem.MaTruong}, CCCD: {action.TruongDiem.CCCD}",
                    ex,
                    nameof(BCThongTinTruongDiemEffects)
                );

                dispatcher.Dispatch(new UpdateTruongDiemFailureAction(errorMessage));
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
    }
}