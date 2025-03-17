using Fluxor;
using KhaoThi_2024_net_client.Services.BC_2_NhomMonDonVi;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Services.Logging;
using static KhaoThi_2024_net_client.Services.BC_2_NhomMonDonVi.BCNhomMonDonViActions;

namespace KhaoThi_2024_net_client.State.BC_2_NhomMonDonVi
{
    public class BCNhomMonDonViEffects
    {
        private readonly IBCNhomMonDonViService _bcNhomMonService;

        public BCNhomMonDonViEffects(IBCNhomMonDonViService bcNhomMonService)
        {
            _bcNhomMonService = bcNhomMonService;
        }
        [EffectMethod]
        public async Task HandleLoadPaginatedNhomMons(LoadPaginatedNhomMonsAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var result = await _bcNhomMonService.GetPaginatedAsync(
                    action.Page,
                    action.PageSize,
                    action.SearchTerm,
                    action.MaTruong,
                    action.MinSoLuong);

                dispatcher.Dispatch(new LoadPaginatedNhomMonsSuccessAction(result));
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi mạng cụ thể
                await Logger.Error(
                    $"Lỗi kết nối khi tải danh sách nhóm môn: Page={action.Page}, PageSize={action.PageSize}",
                    ex,
                    nameof(BCNhomMonDonViEffects));

                dispatcher.Dispatch(new LoadPaginatedNhomMonsFailureAction("Lỗi kết nối đến máy chủ. Vui lòng thử lại sau."));
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                await Logger.Error(
                    $"Lỗi xử lý khi tải danh sách nhóm môn: Page={action.Page}, PageSize={action.PageSize}",
                    ex,
                    nameof(BCNhomMonDonViEffects));

                dispatcher.Dispatch(new LoadPaginatedNhomMonsFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }
        [EffectMethod]
        public async Task HandleGetNhomMonByMaTruong(GetNhomMonByMaTruongAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var nhomMons = await _bcNhomMonService.GetByMaTruongAsync(action.MaTruong);

                if (nhomMons != null && nhomMons.Any())
                {
                    dispatcher.Dispatch(new GetNhomMonByMaTruongSuccessAction(nhomMons));
                }
                else
                {
                    dispatcher.Dispatch(new GetNhomMonByMaTruongSuccessAction(Enumerable.Empty<KhaoThi_2_THPT_NhomMon_DonViModel>()));
                }
            }
            catch (Exception ex)
            {
                await Logger.Error(
                    $"Lỗi khi lấy thông tin nhóm môn theo mã trường: {action.MaTruong}",
                    ex,
                    nameof(BCNhomMonDonViEffects));

                dispatcher.Dispatch(new GetNhomMonByMaTruongFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }
        [EffectMethod]
        public async Task HandleCreateNhomMon(CreateNhomMonAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var id = await _bcNhomMonService.CreateAsync(action.NhomMon);

                if (id > 0)
                {
                    dispatcher.Dispatch(new CreateNhomMonSuccessAction(id));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Tạo mới nhóm môn thành công",
                        "success"
                    ));

                    // Nếu có mã trường, tải lại danh sách nhóm môn của trường đó
                    if (!string.IsNullOrEmpty(action.NhomMon.MaTruong))
                    {
                        dispatcher.Dispatch(new GetNhomMonByMaTruongAction(action.NhomMon.MaTruong));
                    }
                }
                else
                {
                    dispatcher.Dispatch(new CreateNhomMonFailureAction("Không thể tạo nhóm môn. Nhóm môn này có thể đã tồn tại."));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Không thể tạo nhóm môn. Vui lòng kiểm tra lại thông tin.",
                        "error"
                    ));
                }
            }
            catch (Exception ex)
            {
                var errorMessage = "Có lỗi xảy ra khi tạo nhóm môn";
                await Logger.Error(
                    $"Lỗi khi tạo nhóm môn: {action.NhomMon.MaTruong}, {action.NhomMon.MonLuaChon_1}, {action.NhomMon.MonLuaChon_2}",
                    ex,
                    nameof(BCNhomMonDonViEffects)
                );

                dispatcher.Dispatch(new CreateNhomMonFailureAction(errorMessage));
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
        public async Task HandleUpdateNhomMon(UpdateNhomMonAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var success = await _bcNhomMonService.UpdateAsync(action.NhomMon);

                if (success)
                {
                    dispatcher.Dispatch(new UpdateNhomMonSuccessAction(action.NhomMon));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Cập nhật nhóm môn thành công",
                        "success"
                    ));

                    // Nếu có mã trường, tải lại danh sách nhóm môn của trường đó
                    if (!string.IsNullOrEmpty(action.NhomMon.MaTruong))
                    {
                        dispatcher.Dispatch(new GetNhomMonByMaTruongAction(action.NhomMon.MaTruong));
                    }
                }
                else
                {
                    dispatcher.Dispatch(new UpdateNhomMonFailureAction("Không thể cập nhật nhóm môn"));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Không thể cập nhật nhóm môn. Vui lòng thử lại",
                        "error"
                    ));
                }
            }
            catch (Exception ex)
            {
                var errorMessage = "Có lỗi xảy ra khi cập nhật nhóm môn";
                await Logger.Error(
                    $"Lỗi khi cập nhật nhóm môn ID: {action.NhomMon.ID}",
                    ex,
                    nameof(BCNhomMonDonViEffects)
                );

                dispatcher.Dispatch(new UpdateNhomMonFailureAction(errorMessage));
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
        public async Task HandleDeleteNhomMon(DeleteNhomMonAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var success = await _bcNhomMonService.DeleteAsync(action.Id);

                if (success)
                {
                    dispatcher.Dispatch(new DeleteNhomMonSuccessAction(action.Id));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Xóa nhóm môn thành công",
                        "success"
                    ));
                }
                else
                {
                    dispatcher.Dispatch(new DeleteNhomMonFailureAction($"Không thể xóa nhóm môn ID: {action.Id}"));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Xóa nhóm môn thất bại. Vui lòng thử lại.",
                        "error"
                    ));
                }
            }
            catch (Exception ex)
            {
                await Logger.Error($"Lỗi khi xóa nhóm môn ID: {action.Id}", ex, nameof(BCNhomMonDonViEffects));
                dispatcher.Dispatch(new DeleteNhomMonFailureAction(ex.Message));
                dispatcher.Dispatch(new ShowNotificationAction(
                    $"Lỗi khi xóa nhóm môn: {ex.Message}",
                    "error"
                ));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleCheckNhomMonExist(CheckNhomMonExistAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var exists = await _bcNhomMonService.IsNhomMonExistAsync(
                    action.MaTruong,
                    action.MonLuaChon1,
                    action.MonLuaChon2);

                dispatcher.Dispatch(new CheckNhomMonExistSuccessAction(exists));
            }
            catch (Exception ex)
            {
                await Logger.Error(
                    $"Lỗi khi kiểm tra tồn tại nhóm môn: {action.MaTruong}, {action.MonLuaChon1}, {action.MonLuaChon2}",
                    ex,
                    nameof(BCNhomMonDonViEffects));

                dispatcher.Dispatch(new CheckNhomMonExistFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleChangeLock(ChangeLockAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));

                // Lấy thông tin nhóm môn hiện tại
                var paginatedResult = await _bcNhomMonService.GetPaginatedAsync(1, 1, null, null, null);
                var nhomMon = paginatedResult.Items.FirstOrDefault(x => x.ID == action.Id);

                if (nhomMon != null)
                {
                    // Cập nhật trạng thái khóa
                    nhomMon.Lock = action.Lock;
                    var success = await _bcNhomMonService.UpdateAsync(nhomMon);

                    if (success)
                    {
                        dispatcher.Dispatch(new ChangeLockSuccessAction(action.Id));
                        dispatcher.Dispatch(new ShowNotificationAction(
                            action.Lock ? "Khóa nhóm môn thành công" : "Mở khóa nhóm môn thành công",
                            "success"
                        ));

                        // Nếu có mã trường, tải lại danh sách nhóm môn của trường đó
                        if (!string.IsNullOrEmpty(nhomMon.MaTruong))
                        {
                            dispatcher.Dispatch(new GetNhomMonByMaTruongAction(nhomMon.MaTruong));
                        }
                    }
                    else
                    {
                        dispatcher.Dispatch(new ChangeLockFailureAction("Không thể cập nhật trạng thái khóa cho nhóm môn"));
                        dispatcher.Dispatch(new ShowNotificationAction(
                            "Không thể cập nhật trạng thái khóa. Vui lòng thử lại.",
                            "error"
                        ));
                    }
                }
                else
                {
                    dispatcher.Dispatch(new ChangeLockFailureAction($"Không tìm thấy nhóm môn ID: {action.Id}"));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Không tìm thấy nhóm môn cần cập nhật",
                        "error"
                    ));
                }
            }
            catch (Exception ex)
            {
                await Logger.Error($"Lỗi khi thay đổi trạng thái khóa nhóm môn ID: {action.Id}", ex, nameof(BCNhomMonDonViEffects));
                dispatcher.Dispatch(new ChangeLockFailureAction(ex.Message));
                dispatcher.Dispatch(new ShowNotificationAction(
                    $"Lỗi khi thay đổi trạng thái khóa: {ex.Message}",
                    "error"
                ));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleLoadNhomMons(LoadNhomMonsAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var nhomMons = await _bcNhomMonService.GetPaginatedAsync(1, 100, null);
                dispatcher.Dispatch(new LoadNhomMonsSuccessAction(nhomMons.Items));
            }
            catch (Exception ex)
            {
                await Logger.Error("Lỗi khi tải danh sách nhóm môn", ex, nameof(BCNhomMonDonViEffects));
                dispatcher.Dispatch(new LoadNhomMonsFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }
    }
}
