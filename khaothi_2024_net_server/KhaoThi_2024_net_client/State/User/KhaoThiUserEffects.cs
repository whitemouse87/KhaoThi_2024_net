using Fluxor;
using KhaoThi_2024_net_client.Components;
using KhaoThi_2024_net_client.Models.Users;
using KhaoThi_2024_net_client.Services.Logging;
using KhaoThi_2024_net_client.Services.User;
using static KhaoThi_2024_net_client.Services.User.KhaoThiUserActions;

namespace KhaoThi_2024_net_client.State.User
{
    /// <summary>
    /// Class xử lý các hiệu ứng (Effects) khi thực hiện các hành động liên quan đến người dùng
    /// </summary>
    public class KhaoThiUserEffects
    {
        private readonly IUserService _userService;
        private readonly ILoggingService _logger;

        public KhaoThiUserEffects(IUserService userService, ILoggingService logger)
        {
            _userService = userService;
            _logger = logger;
        }
        /// <summary>
        /// Phương thức chung để xử lý gọi API và dispatch action tương ứng
        /// </summary>
        private async Task HandleApiCall<TRequest, TSuccess, TFailure>(
            TRequest requestAction,
            Func<TRequest, Task<object>> apiCall,
            Func<object, TSuccess> successAction,
            Func<string, TFailure> failureAction,
            string successMessage,
            string errorMessage,
            IDispatcher dispatcher)
        {
            try
            {
                var result = await apiCall(requestAction);
                dispatcher.Dispatch(successAction(result));
                dispatcher.Dispatch(new ShowNotificationAction(successMessage, "success"));
                await _logger.LogInfoAsync(successMessage);
            }
            catch (HttpRequestException ex)
            {
                await _logger.LogErrorAsync(errorMessage, ex);
                dispatcher.Dispatch(failureAction($"Lỗi mạng: {ex.Message}"));
                dispatcher.Dispatch(new ShowNotificationAction(errorMessage, "error"));
            }
            catch (System.Exception ex)
            {
                await _logger.LogErrorAsync(errorMessage, ex);
                dispatcher.Dispatch(failureAction($"Lỗi: {ex.Message}"));
                dispatcher.Dispatch(new ShowNotificationAction(errorMessage, "error"));
            }
        }
        /// <summary>
        /// Xử lý tải danh sách người dùng theo mã đơn vị
        /// </summary>
        [EffectMethod]
        public async Task HandleLoadUsersAction(LoadUsersAction action, IDispatcher dispatcher) =>
            await HandleApiCall(
                action,
                async (_) => await _userService.GetByMaDonViAsync("default"),
                (result) => new LoadUsersSuccessAction((IEnumerable<KhaoThiUserModel>)result),
                (error) => new LoadUsersFailureAction(error),
                "Tải danh sách người dùng thành công",
                "Lỗi khi tải danh sách người dùng",
                dispatcher
            );

        /// <summary>
        /// Xử lý tải danh sách người dùng có phân trang
        /// </summary>
        [EffectMethod]
        public async Task HandleLoadPaginatedUsersAction(LoadPaginatedUsersAction action, IDispatcher dispatcher) =>
            await HandleApiCall(
                action,
                async (req) => await _userService.GetPaginatedAsync(req.Page, req.PageSize, req.SearchTerm),
                (result) => new LoadPaginatedUsersSuccessAction((PaginatedResult<KhaoThiUserModel>)result),
                (error) => new LoadPaginatedUsersFailureAction(error),
                "Tải danh sách người dùng phân trang thành công",
                "Lỗi khi tải danh sách phân trang",
                dispatcher
            );

        /// <summary>
        /// Xử lý tạo mới người dùng
        /// </summary>
        [EffectMethod]
        public async Task HandleCreateUserAction(CreateUserAction action, IDispatcher dispatcher) =>
            await HandleApiCall(
                action,
                async (req) => await _userService.CreateAsync(req.User),
                (result) => new CreateUserSuccessAction((KhaoThiUserModel)result),
                (error) => new CreateUserFailureAction(error),
                "Tạo người dùng thành công",
                "Lỗi khi tạo người dùng",
                dispatcher
            );

        /// <summary>
        /// Xử lý cập nhật thông tin người dùng
        /// </summary>
        [EffectMethod]
        public async Task HandleUpdateUserAction(UpdateUserAction action, IDispatcher dispatcher) =>
            await HandleApiCall(
                action,
                async (req) => await _userService.UpdateAsync(req.User) ? req.User : throw new System.Exception("Cập nhật thất bại"),
                (result) => new UpdateUserSuccessAction((KhaoThiUserModel)result),
                (error) => new UpdateUserFailureAction(error),
                "Cập nhật người dùng thành công",
                "Lỗi khi cập nhật người dùng",
                dispatcher
            );

        /// <summary>
        /// Xử lý xóa người dùng
        /// </summary>
        [EffectMethod]
        public async Task HandleDeleteUserAction(DeleteUserAction action, IDispatcher dispatcher) =>
            await HandleApiCall(
                action,
                async (req) => await _userService.DeleteAsync(req.Id) ? req.Id : throw new System.Exception("Xóa thất bại"),
                (result) => new DeleteUserSuccessAction((int)result),
                (error) => new DeleteUserFailureAction(error),
                "Xóa người dùng thành công",
                "Lỗi khi xóa người dùng",
                dispatcher
            );

        /// <summary>
        /// Xử lý kiểm tra username đã tồn tại hay chưa
        /// </summary>
        [EffectMethod]
        public async Task HandleCheckUsernameAction(CheckUsernameAction action, IDispatcher dispatcher) =>
            await HandleApiCall(
                action,
                async (req) => await _userService.IsUsernameExistAsync(req.Username),
                (result) => new CheckUsernameSuccessAction((bool)result),
                (error) => new CheckUsernameFailureAction(error),
                "Kiểm tra username thành công",
                "Lỗi khi kiểm tra username",
                dispatcher
            );
    }

}

