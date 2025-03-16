using Fluxor;
using KhaoThi_2024_net_client.Services.BC_1_ThongTinDonVi;
using KhaoThi_2024_net_client.Services.Logging;
using static KhaoThi_2024_net_client.Services.BC_1_ThongTinDonVi.BCThongTinDonViAction;
namespace KhaoThi_2024_net_client.State.BC_1_ThongTinDonVi
{
    public class BCThongTinDonViEffects
    {
        private readonly IBCThongTinDonViService _bcttdonviService;
        public BCThongTinDonViEffects(IBCThongTinDonViService bcttdonviService)
        {
            _bcttdonviService = bcttdonviService;

        }
        [EffectMethod]
        public async Task HandleLoadPaginatedDonVis(LoadPaginatedDonVisAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var result = await _bcttdonviService.GetPaginatedAsync(action.Page, action.PageSize, action.SearchTerm);
                dispatcher.Dispatch(new LoadPaginatedDonVisSuccessAction(result));
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi mạng cụ thể
                await Logger.Error(
                    $"Lỗi kết nối khi tải danh sách đơn vị: Page={action.Page}, PageSize={action.PageSize}",
                    ex,
                    nameof(BCThongTinDonViEffects));

                dispatcher.Dispatch(new LoadPaginatedDonVisFailureAction("Lỗi kết nối đến máy chủ. Vui lòng thử lại sau."));
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                await Logger.Error(
                    $"Lỗi xử lý khi tải danh sách đơn vị: Page={action.Page}, PageSize={action.PageSize}",
                    ex,
                    nameof(BCThongTinDonViEffects));

                dispatcher.Dispatch(new LoadPaginatedDonVisFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }
        [EffectMethod]
        public async Task HandleGetDonViByMaTruong(GetDonViByMaTruongAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var user = await _bcttdonviService.GetByMaTruongAsync(action.MaTruong);
                if (user != null)
                {
                    dispatcher.Dispatch(new GetDonViByMaTruongSuccessAction(user));
                }
                else
                {
                    dispatcher.Dispatch(new GetDonViByMaTruongFailureAction($"Không tìm thấy đơn vị theo mã trường: {action.MaTruong}"));
                }
            }
            catch (Exception ex)
            {
                await Logger.Error($"Lỗi khi lấy thông tin đơn vị theo mã trường: {action.MaTruong}", ex, nameof(BCThongTinDonViEffects));
                dispatcher.Dispatch(new GetDonViByMaTruongFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }
        [EffectMethod]
        public async Task HandleUpdateDonVi(UpdateDonViAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var success = await _bcttdonviService.UpdateAsync(action.DonVi);

                if (success)
                {

                    dispatcher.Dispatch(new UpdateDonViSuccessAction(action.DonVi));
                    //dispatcher.Dispatch(new ShowNotificationAction(
                    //    "Cập nhật người dùng thành công",
                    //    "success"
                    //));
                }
                else
                {

                    dispatcher.Dispatch(new UpdateDonViFailureAction("Không thể cập nhật đơn vị"));
                    //dispatcher.Dispatch(new ShowNotificationAction(
                    //    "Không thể cập nhật đơn vị. Vui lòng thử lại",
                    //    "error"
                    //));
                }
            }
            catch (Exception ex)
            {
                var errorMessage = "Có lỗi xảy ra khi cập nhật đơn vị";
                await Logger.Error(
                    $"Lỗi khi cập nhật đơn vị với mã trường: {action.DonVi.MaTruong}",
                    ex,
                    nameof(BCThongTinDonViEffects)
                );

                dispatcher.Dispatch(new UpdateDonViFailureAction(errorMessage));
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
        //[EffectMethod]
        //public async Task HandleChangeActive(ChangeActiveAction action, IDispatcher dispatcher)
        //{

        //    try
        //    {
        //        dispatcher.Dispatch(new SetLoadingAction(true)); // Bắt đầu loading
        //        var isSuccess = await _bcttdonviService.ChangeActive(action.id, action.active);

        //        if (isSuccess)
        //        {
        //            // Chỉ dispatch action thành công nếu service trả về true
        //            dispatcher.Dispatch(new ChangeActiveSuccessAction(action.id));
        //            //dispatcher.Dispatch(new LoadUsersAction());
        //            dispatcher.Dispatch(new GetUserByIdAction(action.id));
        //        }
        //        else
        //        {
        //            // Nếu service trả về false, dispatch action thất bại
        //            dispatcher.Dispatch(new ChangeActiveFailureAction("Không thể thay đổi trạng thái tài khoản. Vui lòng kiểm tra lại thông tin."));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await Logger.Error($"Lỗi khi điều chỉnh trạng thái tài khoản {action.id}", ex, nameof(KhaoThiUserEffects));
        //        dispatcher.Dispatch(new ChangeActiveFailureAction(ex.Message));
        //    }
        //    finally
        //    {
        //        dispatcher.Dispatch(new SetLoadingAction(false));
        //    }
        //}
    }
}
