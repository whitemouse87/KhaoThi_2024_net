using Fluxor;
using KhaoThi_2024_net_client.Services.Logging;
using System.Text.Json;

namespace KhaoThi_2024_net_client.Middleware
{
    public class FluxorLoggingMiddleware : Fluxor.Middleware
    {
        private readonly ILoggingService _logger;
        private readonly bool _isDebug;

        public FluxorLoggingMiddleware(ILoggingService logger)
        {
            _logger = logger;

#if DEBUG
            _isDebug = true;
#else
            _isDebug = false;
#endif
        }

        public override Task InitializeAsync(IDispatcher dispatcher, IStore store)
        {
            if (_isDebug)
            {
                _logger.LogInfoAsync("Fluxor store initialized");
            }
            return Task.CompletedTask;
        }

        public override void AfterInitializeAllMiddlewares()
        {
            if (_isDebug)
            {
                _logger.LogInfoAsync("All Fluxor middlewares initialized");
            }
        }

        public override bool MayDispatchAction(object action)
        {
            return true;
        }

        public override void BeforeDispatch(object action)
        {
            try
            {
                if (_isDebug)
                {
                    _logger.LogDebugAsync($"Dispatching action: {action.GetType().Name}");
                    var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
                    var json = JsonSerializer.Serialize(action, jsonOptions);
                    _logger.LogDebugAsync($"Action payload: {json}");
                }
            }
            catch (Exception ex)
            {
                // Chỉ log lỗi, không ném ngoại lệ để tránh lỗi khi publish
                _logger.LogErrorAsync("Error in BeforeDispatch middleware", ex);
            }
        }

        public override void AfterDispatch(object action)
        {
            if (_isDebug)
            {
                _logger.LogDebugAsync($"Action dispatched: {action.GetType().Name}");
            }
        }
    }
}