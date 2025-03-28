using Fluxor;
using khaothi_2024_net_client.Services.BC_5_ThongTinConThi;
using KhaoThi_2024_net_client.Models.BaoCaoSoLieu;
using KhaoThi_2024_net_client.Services.Logging;
using KhaoThi_2024_net_client.State.BC_4_LanhDaoDonVi;
using static khaothi_2024_net_client.Services.BC_5_ThongTinConThi.BCThongTinConThiAction;


namespace khaothi_2024_net_client.State.BC_5_ThongTinConThi
{
    public class BCThongTinConThiEffects
    {
        private readonly IBCThongTinConThiService _bcConThiService;

        public BCThongTinConThiEffects(IBCThongTinConThiService bcConThiService)
        {
            _bcConThiService = bcConThiService;
        }


        [EffectMethod]
        public async Task HandleLoadConThis(LoadConThisAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));

                // Sử dụng GetPaginatedAsync như ban đầu, không tham chiếu đến MaTruong
                var paginatedResult = await _bcConThiService.GetPaginatedAsync(1, 1000);

                if (paginatedResult != null && paginatedResult.Items != null)
                {
                    dispatcher.Dispatch(new LoadConThisSuccessAction(paginatedResult.Items));

                    // Thêm log để debug nếu cần
                    await Logger.Info(
                        $"Đã tải danh sách con thi: {paginatedResult.Items.Count()} items",
                        nameof(BCThongTinConThiEffects));
                }
                else
                {
                    dispatcher.Dispatch(new LoadConThisSuccessAction(
                        Enumerable.Empty<KhaoThi_5_THPT_ThongTin_ConThiModel>()));

                    await Logger.Info(
                        "Danh sách con thi rỗng hoặc null",
                        nameof(BCThongTinConThiEffects));
                }
            }
            catch (Exception ex)
            {
                await Logger.Error("Lỗi khi tải danh sách con thi", ex, nameof(BCThongTinConThiEffects));
                dispatcher.Dispatch(new LoadConThisFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleLoadPaginatedConThis(LoadPaginatedConThisAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));

                // Log thông tin request để debug
                await Logger.Info(
                    $"Đang gọi GetPaginatedAsync: page={action.Page}, pageSize={action.PageSize}, maTruong={action.MaTruong}",
                    nameof(BCThongTinConThiEffects));

                var result = await _bcConThiService.GetPaginatedAsync(
                    action.Page,
                    action.PageSize,
                    action.SearchTerm,
                    action.MaTruong,
                    action.KyThiThamDu);

                // Log kết quả để debug
                await Logger.Info(
                    $"Kết quả GetPaginatedAsync: totalCount={result.TotalCount}, items count={result.Items.Count()}",
                    nameof(BCThongTinConThiEffects));

                dispatcher.Dispatch(new LoadPaginatedConThisSuccessAction(result));
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi mạng cụ thể
                await Logger.Error(
                    $"Lỗi kết nối khi tải danh sách con thi: Page={action.Page}, PageSize={action.PageSize}, Error={ex.Message}",
                    ex,
                    nameof(BCThongTinConThiEffects));

                dispatcher.Dispatch(new LoadPaginatedConThisFailureAction("Lỗi kết nối đến máy chủ. Vui lòng thử lại sau."));
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                await Logger.Error(
                    $"Lỗi xử lý khi tải danh sách con thi: Page={action.Page}, PageSize={action.PageSize}, Error={ex.Message}",
                    ex,
                    nameof(BCThongTinConThiEffects));

                dispatcher.Dispatch(new LoadPaginatedConThisFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleGetConThiByMaTruong(GetConThiByMaTruongAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));

                // Log thông tin request để debug
                await Logger.Info(
                    $"Đang gọi GetByMaTruongAsync: maTruong={action.MaTruong}",
                    nameof(BCThongTinConThiEffects));

                var conThis = await _bcConThiService.GetByMaTruongAsync(action.MaTruong);

                // Log kết quả để debug
                if (conThis != null)
                {
                    await Logger.Info(
                        $"Kết quả GetByMaTruongAsync: items count={conThis.Count()}",
                        nameof(BCThongTinConThiEffects));

                    dispatcher.Dispatch(new GetConThiByMaTruongSuccessAction(conThis));
                }
                else
                {
                    await Logger.Info(
                        $"Kết quả GetByMaTruongAsync: không có dữ liệu (null)",
                        nameof(BCThongTinConThiEffects));

                    dispatcher.Dispatch(new GetConThiByMaTruongSuccessAction(
                        Enumerable.Empty<KhaoThi_5_THPT_ThongTin_ConThiModel>()));
                }
            }
            catch (Exception ex)
            {
                await Logger.Error(
                    $"Lỗi khi lấy thông tin con thi theo mã trường: {action.MaTruong}, Error={ex.Message}",
                    ex,
                    nameof(BCThongTinConThiEffects));

                dispatcher.Dispatch(new GetConThiByMaTruongFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleCreateConThi(CreateConThiAction action, IDispatcher dispatcher)
        {


            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var success = await _bcConThiService.CreateAsync(action.ConThi);

                if (success)
                {
                    dispatcher.Dispatch(new CreateConThiSuccessAction());
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Tạo mới con thi thành công",
                        "success"
                    ));

                    // Nếu có mã trường, tải lại danh sách lãnh đạo của trường đó
                    if (!string.IsNullOrEmpty(action.ConThi.MaTruong))
                    {
                        dispatcher.Dispatch(new GetConThiByMaTruongAction(action.ConThi.MaTruong));
                    }
                }
                else
                {
                    var errorMessage = "Không thể tạo lãnh đạo. Thông tin lãnh đạo này có thể đã tồn tại.";
                    dispatcher.Dispatch(new CreateConThiFailureAction(errorMessage));
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
                    $"Lỗi khi tạo lãnh đạo: {action.ConThi.MaTruong}, {action.ConThi.CCCD}, {action.ConThi.HoTen}",
                    ex,
                    nameof(BCLanhDaoDonViEffects)
                );

                dispatcher.Dispatch(new CreateConThiFailureAction(errorMessage));
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
        public async Task HandleUpdateConThi(UpdateConThiAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));

                // Log thông tin cập nhật
                await Logger.Info(
                    $"Đang cập nhật con thi: MaTruong={action.ConThi.MaTruong}, CCCD={action.ConThi.CCCD}, MaDinhDanh={action.ConThi.MaDinhDanhCuaCon}",
                    nameof(BCThongTinConThiEffects));

                var success = await _bcConThiService.UpdateAsync(action.ConThi);

                // Log kết quả cập nhật
                await Logger.Info(
                    $"Kết quả cập nhật: success={success}",
                    nameof(BCThongTinConThiEffects));

                if (success)
                {
                    dispatcher.Dispatch(new UpdateConThiSuccessAction(action.ConThi));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Cập nhật thông tin con thi thành công",
                        "success"
                    ));

                    // Đợi một chút cho server xử lý hoàn tất
                    await Task.Delay(500);

                    // Tải lại danh sách đảm bảo dữ liệu mới nhất
                    dispatcher.Dispatch(new GetConThiByMaTruongAction(action.ConThi.MaTruong));
                }
                else
                {
                    dispatcher.Dispatch(new UpdateConThiFailureAction("Không thể cập nhật thông tin con thi"));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Không thể cập nhật thông tin con thi. Vui lòng thử lại",
                        "error"
                    ));
                }
            }
            catch (Exception ex)
            {
                var errorMessage = "Có lỗi xảy ra khi cập nhật thông tin con thi";
                await Logger.Error(
                    $"Lỗi khi cập nhật thông tin con thi: {action.ConThi.MaTruong}, {action.ConThi.CCCD}, {action.ConThi.MaDinhDanhCuaCon}, Error={ex.Message}",
                    ex,
                    nameof(BCThongTinConThiEffects));

                dispatcher.Dispatch(new UpdateConThiFailureAction(errorMessage));
                dispatcher.Dispatch(new ShowNotificationAction($"{errorMessage}: {ex.Message}", "error"));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleDeleteConThi(DeleteConThiAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));

                // Log thông tin xóa
                await Logger.Info(
                    $"Đang xóa con thi: MaTruong={action.MaTruong}, CCCD={action.CCCD}, MaDinhDanh={action.MaDinhDanhCuaCon}",
                    nameof(BCThongTinConThiEffects));

                var success = await _bcConThiService.DeleteAsync(action.MaTruong, action.CCCD, action.MaDinhDanhCuaCon);

                // Log kết quả xóa
                await Logger.Info(
                    $"Kết quả xóa: success={success}",
                    nameof(BCThongTinConThiEffects));

                if (success)
                {
                    dispatcher.Dispatch(new DeleteConThiSuccessAction(action.MaTruong, action.CCCD, action.MaDinhDanhCuaCon));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Xóa thông tin con thi thành công",
                        "success"
                    ));

                    // Đợi một chút cho server xử lý hoàn tất
                    await Task.Delay(500);

                    // Tải lại danh sách
                    dispatcher.Dispatch(new GetConThiByMaTruongAction(action.MaTruong));
                }
                else
                {
                    dispatcher.Dispatch(new DeleteConThiFailureAction(
                        $"Không thể xóa thông tin con thi với mã trường {action.MaTruong}, CCCD {action.CCCD}, mã định danh {action.MaDinhDanhCuaCon}"));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Xóa thông tin con thi thất bại. Vui lòng thử lại.",
                        "error"
                    ));
                }
            }
            catch (Exception ex)
            {
                await Logger.Error(
                    $"Lỗi khi xóa thông tin con thi: {action.MaTruong}, {action.CCCD}, {action.MaDinhDanhCuaCon}, Error={ex.Message}",
                    ex,
                    nameof(BCThongTinConThiEffects));

                dispatcher.Dispatch(new DeleteConThiFailureAction(ex.Message));
                dispatcher.Dispatch(new ShowNotificationAction($"Lỗi khi xóa thông tin con thi: {ex.Message}", "error"));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        [EffectMethod]
        public async Task HandleCheckConThiExist(CheckConThiExistAction action, IDispatcher dispatcher)
        {
            // Effect này dường như chỉ để kiểm tra và có thể hiển thị thông báo, không thay đổi state chính nhiều
            // Nên có thể giữ nguyên SetLoading trong finally
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));

                await Logger.Info(
                    $"Đang kiểm tra tồn tại: MaTruong={action.MaTruong}, CCCD={action.CCCD}, MaDinhDanh={action.MaDinhDanhCuaCon}",
                    nameof(BCThongTinConThiEffects));

                var exists = await _bcConThiService.IsConThiExistAsync(
                    action.MaTruong,
                    action.CCCD,
                    action.MaDinhDanhCuaCon);

                await Logger.Info(
                    $"Kết quả kiểm tra tồn tại từ service: exists={exists}",
                    nameof(BCThongTinConThiEffects));

                dispatcher.Dispatch(new CheckConThiExistSuccessAction(exists)); // Có thể reducer không làm gì

                if (exists)
                {
                    // Chỉ thông báo nếu được yêu cầu hoặc theo logic nghiệp vụ
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Thông tin con thi này đã tồn tại trong hệ thống",
                        "warning" // Dùng warning
                    ));
                }
                else
                {
                    // Có thể thông báo "Thông tin hợp lệ để thêm mới" nếu cần
                    // dispatcher.Dispatch(new ShowNotificationAction(
                    //     "Thông tin hợp lệ để thêm mới.",
                    //     "info"
                    // ));
                }
            }
            catch (Exception ex)
            {
                var errorMsg = $"Lỗi khi kiểm tra tồn tại thông tin con thi: {ex.Message}";
                await Logger.Error(
                    $"Lỗi Exception khi kiểm tra tồn tại: MaTruong={action.MaTruong}, Error={ex.Message}",
                    ex,
                    nameof(BCThongTinConThiEffects));
                dispatcher.Dispatch(new CheckConThiExistFailureAction(ex.Message));
                dispatcher.Dispatch(new ShowNotificationAction(errorMsg, "error"));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }
    }
}
