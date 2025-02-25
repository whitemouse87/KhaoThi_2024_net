using Fluxor;
using KhaoThi_2024_net_client.Services.Logging;

namespace KhaoThi_2024_net_client.Middleware
{
    public class FluxorLoggingMiddleware : Fluxor.Middleware
    {
        //private readonly ILoggingService _loggingService;
        private IStore _store;
        private readonly Dictionary<object, DateTime> _actionStartTimes = new();

        public FluxorLoggingMiddleware(ILoggingService loggingService)
        {
            //_loggingService = loggingService;
            _store = null!; // Initialize _store to a non-null value to satisfy the compiler
        }

        public override async Task InitializeAsync(IDispatcher dispatcher, IStore store)
        {
            _store = store;
            await Logger.Info("🚀 [Fluxor] Middleware Ghi Log đã khởi tạo.", "Fluxor");

            foreach (var feature in _store.Features)
            {
                await Logger.Info($"📌 [Fluxor] Trạng thái ban đầu của {feature.Key}: {feature.Value.GetState()}", "Fluxor");
            }
        }

        public override async void AfterInitializeAllMiddlewares()
        {
            await Logger.Info("✅ [Fluxor] Tất cả Middleware đã khởi tạo hoàn tất.", "Fluxor");
        }

        public override async void BeforeDispatch(object action)
        {
            await Logger.Debug($"📢 [Fluxor] Chuẩn bị thực thi action: {action.GetType().Name}", "Fluxor");

            // Chỉ theo dõi thời gian của các action do người dùng dispatch, không phải action hệ thống
            if (action.GetType().Name != "StoreInitializedAction")
            {
                _actionStartTimes[action] = DateTime.UtcNow;
            }
        }

        public override async void AfterDispatch(object action)
        {
            await Logger.Info($"✅ [Fluxor] Đã thực thi action: {action.GetType().Name}", "Fluxor");

            // Kiểm tra xem action có trong dictionary không trước khi tính toán thời gian thực thi
            if (_actionStartTimes.ContainsKey(action))
            {
                var elapsedTime = DateTime.UtcNow - _actionStartTimes[action];
                await Logger.Info($"⏱ [Fluxor] Action {action.GetType().Name} đã hoàn thành sau {elapsedTime.TotalMilliseconds} ms", "Fluxor");

                _actionStartTimes.Remove(action);
            }

            // Log trạng thái mới của từng feature
            if (_store != null)
            {
                foreach (var feature in _store.Features)
                {
                    await Logger.Info($"📌 [Fluxor] Trạng thái mới của {feature.Key}: {feature.Value.GetState()}", "Fluxor");
                }
            }
        }
    }
}