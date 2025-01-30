namespace KhaoThi_2024_net_client.Services.Logging
{
    public static class Logger
    {
        private static ILoggingService _loggingService;

        public static void Initialize(ILoggingService loggingService)
        {
            _loggingService = loggingService;
        }

        public static async Task Debug(string message, string component = null)
        {
            if (_loggingService != null)
                await _loggingService.LogDebugAsync(message, component);
        }

        public static async Task Info(string message, string component = null)
        {
            if (_loggingService != null)
                await _loggingService.LogInfoAsync(message, component);
        }

        public static async Task Error(string message, Exception ex = null, string component = null)
        {
            if (_loggingService != null)
                await _loggingService.LogErrorAsync(message, ex, component);
        }
        public static async Task Warning(string message, string component = null)
        {
            if (_loggingService != null)
                await _loggingService.LogWarningAsync(message, component);
        }
    }
}
