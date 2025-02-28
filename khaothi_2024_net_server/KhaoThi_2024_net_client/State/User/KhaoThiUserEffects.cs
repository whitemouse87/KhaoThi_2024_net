using Fluxor;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.Users;
using KhaoThi_2024_net_client.Services.Logging;
using KhaoThi_2024_net_client.Services.User;
using static KhaoThi_2024_net_client.Services.User.KhaoThiUserActions;

namespace KhaoThi_2024_net_client.State.User
{
    /// <summary>
    /// Class xử lý các side effects trong quản lý state người dùng
    /// </summary>
    public class KhaoThiUserEffects
    {
        private readonly IUserService _userService;


        public KhaoThiUserEffects(IUserService userService)
        {
            _userService = userService;

        }

        /// <summary>
        /// Effect xử lý tải danh sách người dùng có phân trang
        /// </summary>
        [EffectMethod]
        public async Task HandleLoadPaginatedUsers(LoadPaginatedUsersAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var result = await _userService.GetPaginatedAsync(action.Page, action.PageSize, action.SearchTerm);
                dispatcher.Dispatch(new LoadPaginatedUsersSuccessAction(result));
            }
            catch (HttpRequestException ex)
            {
                // Xử lý lỗi mạng cụ thể
                await Logger.Error(
                    $"Lỗi kết nối khi tải danh sách người dùng: Page={action.Page}, PageSize={action.PageSize}",
                    ex,
                    nameof(KhaoThiUserEffects));

                dispatcher.Dispatch(new LoadPaginatedUsersFailureAction("Lỗi kết nối đến máy chủ. Vui lòng thử lại sau."));
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi khác
                await Logger.Error(
                    $"Lỗi xử lý khi tải danh sách người dùng: Page={action.Page}, PageSize={action.PageSize}",
                    ex,
                    nameof(KhaoThiUserEffects));

                dispatcher.Dispatch(new LoadPaginatedUsersFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        /// <summary>
        /// Effect xử lý lấy thông tin người dùng theo ID
        /// </summary>
        [EffectMethod]
        public async Task HandleGetUserById(GetUserByIdAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var user = await _userService.GetByIdAsync(action.Id);
                if (user != null)
                {
                    dispatcher.Dispatch(new GetUserByIdSuccessAction(user));
                }
                else
                {
                    dispatcher.Dispatch(new GetUserByIdFailureAction($"Không tìm thấy người dùng với ID: {action.Id}"));
                }
            }
            catch (Exception ex)
            {
                await Logger.Error($"Lỗi khi lấy thông tin người dùng ID: {action.Id}", ex, nameof(KhaoThiUserEffects));
                dispatcher.Dispatch(new GetUserByIdFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        /// <summary>
        /// Effect xử lý tạo mới người dùng
        /// </summary>
        [EffectMethod]
        public async Task HandleCreateUser(CreateUserAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var createdUser = await _userService.CreateAsync(action.User);
                dispatcher.Dispatch(new CreateUserSuccessAction(createdUser));
                dispatcher.Dispatch(new ShowNotificationAction("Tạo mới người dùng thành công", "success"));
            }
            catch (Exception ex)
            {
                await Logger.Error("Lỗi khi tạo mới người dùng", ex, nameof(KhaoThiUserEffects));
                dispatcher.Dispatch(new CreateUserFailureAction(ex.Message));
                dispatcher.Dispatch(new ShowNotificationAction("Tạo mới người dùng thất bại", "error"));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }


        [EffectMethod]
        public async Task HandleUpdateUser(UpdateUserAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var success = await _userService.UpdateAsync(action.User);

                if (success)
                {
                    dispatcher.Dispatch(new UpdateUserSuccessAction(action.User));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Cập nhật người dùng thành công",
                        "success"
                    ));
                }
                else
                {
                    dispatcher.Dispatch(new UpdateUserFailureAction("Không thể cập nhật người dùng"));
                    dispatcher.Dispatch(new ShowNotificationAction(
                        "Không thể cập nhật người dùng. Vui lòng thử lại",
                        "error"
                    ));
                }
            }
            catch (Exception ex)
            {
                var errorMessage = "Có lỗi xảy ra khi cập nhật người dùng";
                await Logger.Error(
                    $"Lỗi khi cập nhật người dùng ID: {action.User.ID}",
                    ex,
                    nameof(KhaoThiUserEffects)
                );

                dispatcher.Dispatch(new UpdateUserFailureAction(errorMessage));
                dispatcher.Dispatch(new ShowNotificationAction(
                    $"{errorMessage}: {ex.Message}",
                    "error"
                ));
            }

        }

        /// <summary>
        /// Effect xử lý xóa người dùng
        /// </summary>
        [EffectMethod]
        public async Task HandleDeleteUser(DeleteUserAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var success = await _userService.DeleteAsync(action.Id);
                if (success)
                {
                    dispatcher.Dispatch(new DeleteUserSuccessAction(action.Id));
                    dispatcher.Dispatch(new ShowNotificationAction("Xóa người dùng thành công", "success"));
                }
                else
                {
                    dispatcher.Dispatch(new DeleteUserFailureAction($"Không thể xóa người dùng ID: {action.Id}"));
                    dispatcher.Dispatch(new ShowNotificationAction("Xóa người dùng thất bại", "error"));
                }
            }
            catch (Exception ex)
            {
                await Logger.Error($"Lỗi khi xóa người dùng ID: {action.Id}", ex, nameof(KhaoThiUserEffects));
                dispatcher.Dispatch(new DeleteUserFailureAction(ex.Message));
                dispatcher.Dispatch(new ShowNotificationAction("Xóa người dùng thất bại", "error"));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        /// <summary>
        /// Effect xử lý lấy danh sách người dùng theo mã đơn vị
        /// </summary>
        [EffectMethod]
        public async Task HandleLoadUsersByDonVi(LoadUsersByDonViAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var users = await _userService.GetByMaDonViAsync(action.MaDonVi);
                dispatcher.Dispatch(new LoadUsersByDonViSuccessAction(users));
            }
            catch (Exception ex)
            {
                await Logger.Error($"Lỗi khi lấy danh sách người dùng theo mã đơn vị: {action.MaDonVi}", ex, nameof(KhaoThiUserEffects));
                dispatcher.Dispatch(new LoadUsersByDonViFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        /// <summary>
        /// Effect xử lý thêm nhiều người dùng cùng lúc
        /// </summary>
        [EffectMethod]
        public async Task HandleBulkInsertUsers(BulkInsertUsersAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                await _userService.BulkInsertUsersAsync(action.Users);
                dispatcher.Dispatch(new BulkInsertUsersSuccessAction());
                dispatcher.Dispatch(new ShowNotificationAction("Thêm nhiều người dùng thành công", "success"));
            }
            catch (Exception ex)
            {
                await Logger.Error("Lỗi khi thêm nhiều người dùng", ex, nameof(KhaoThiUserEffects));
                dispatcher.Dispatch(new BulkInsertUsersFailureAction(ex.Message));
                dispatcher.Dispatch(new ShowNotificationAction("Thêm nhiều người dùng thất bại", "error"));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        /// <summary>
        /// Effect xử lý cập nhật nhiều người dùng cùng lúc
        /// </summary>
        [EffectMethod]
        public async Task HandleBulkUpdateUsers(BulkUpdateUsersAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                await _userService.BulkUpdateUsersAsync(action.Users);
                dispatcher.Dispatch(new BulkUpdateUsersSuccessAction());
                dispatcher.Dispatch(new ShowNotificationAction("Cập nhật nhiều người dùng thành công", "success"));
            }
            catch (Exception ex)
            {
                await Logger.Error("Lỗi khi cập nhật nhiều người dùng", ex, nameof(KhaoThiUserEffects));
                dispatcher.Dispatch(new BulkUpdateUsersFailureAction(ex.Message));
                dispatcher.Dispatch(new ShowNotificationAction("Cập nhật nhiều người dùng thất bại", "error"));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }

        /// <summary>
        /// Effect xử lý kiểm tra tên đăng nhập đã tồn tại
        /// </summary>
        [EffectMethod]
        public async Task HandleCheckUsername(CheckUsernameAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var exists = await _userService.IsUsernameExistAsync(action.Username);
                dispatcher.Dispatch(new CheckUsernameSuccessAction(exists));
            }
            catch (Exception ex)
            {
                await Logger.Error($"Lỗi khi kiểm tra tên đăng nhập: {action.Username}", ex, nameof(KhaoThiUserEffects));
                dispatcher.Dispatch(new CheckUsernameFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }
        [EffectMethod]
        public async Task HandleLoadUsers(LoadUsersAction action, IDispatcher dispatcher)
        {
            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true));
                var users = await _userService.GetPaginatedAsync(1, 100, null);
                dispatcher.Dispatch(new LoadUsersSuccessAction(users.Items));
            }
            catch (Exception ex)
            {
                await Logger.Error("Lỗi khi tải danh sách người dùng", ex, nameof(KhaoThiUserEffects));
                dispatcher.Dispatch(new LoadUsersFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }
        [EffectMethod]
        public async Task HandleChangePassword(ChangePasswordAction action, IDispatcher dispatcher)
        {

            try
            {
                dispatcher.Dispatch(new SetLoadingAction(true)); // Bắt đầu loading
                var isSuccess = await _userService.ChangePassword(action.id, action.info);

                if (isSuccess)
                {
                    // Chỉ dispatch action thành công nếu service trả về true
                    dispatcher.Dispatch(new ChangePasswordSuccessAction(action.id));
                }
                else
                {
                    // Nếu service trả về false, dispatch action thất bại
                    dispatcher.Dispatch(new ChangePasswordFailureAction("Không thể thay đổi mật khẩu. Vui lòng kiểm tra lại thông tin."));
                }
            }
            catch (Exception ex)
            {
                await Logger.Error("Lỗi khi điều chỉnh mật khẩu", ex, nameof(KhaoThiUserEffects));
                dispatcher.Dispatch(new ChangePasswordFailureAction(ex.Message));
            }
            finally
            {
                dispatcher.Dispatch(new SetLoadingAction(false));
            }
        }
    }
}

