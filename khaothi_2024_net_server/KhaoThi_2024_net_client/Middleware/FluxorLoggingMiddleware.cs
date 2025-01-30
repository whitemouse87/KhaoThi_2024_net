using Fluxor;

namespace KhaoThi_2024_net_client.Middleware
{
    public class FluxorLoggingMiddleware : Fluxor.Middleware
    {
        public override Task InitializeAsync(IDispatcher dispatcher, IStore store)
        {
            //Console.WriteLine("=== Fluxor đã được khởi tạo ===");
            return Task.CompletedTask;
        }

        //public override void BeforeDispatch(object action)
        //{
        //    Console.WriteLine($">>> Chuẩn bị dispatch action: {action.GetType().Name}");
        //    if (action is Services.Auth.AuthActions.LoginAction loginAction)
        //    {
        //        Console.WriteLine($">>> Thông tin đăng nhập: {loginAction.Request?.TenDangNhap}");
        //    }
        //}

        //public override void AfterDispatch(object action)
        //{
        //    Console.WriteLine($"<<< Đã xử lý xong action: {action.GetType().Name}");
        //}
    }
}