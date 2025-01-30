using KhaoThi_2024_net_client.Models;
using Serilog;
using System.Net.Http.Json;

namespace KhaoThi_2024_net_client.Services.Logging
{
    public class LoggingService : ILoggingService
    {
        private readonly Serilog.ILogger _logger;
        private readonly HttpClient _httpClient;

        public LoggingService(HttpClient httpClient)
        {
            _logger = Serilog.Log.ForContext<LoggingService>();
            _httpClient = httpClient;
        }

        public async Task LogDebugAsync(string message, string component = null)
        {
            _logger.Debug("[CLIENT] [{Component}] {Message}", component, message);
            await SendToServer("Debug", message, null, component);
        }

        public async Task LogInfoAsync(string message, string component = null)
        {
            _logger.Information("[CLIENT] [{Component}] {Message}", component, message);
            await SendToServer("Information", message, null, component);
        }
        public async Task LogWarningAsync(string message, string component = null)
        {
            _logger.Warning("[CLIENT] [{Component}] {Message}", component, message);
            await SendToServer("Information", message, null, component);
        }

        public async Task LogErrorAsync(string message, Exception ex = null, string component = null)
        {
            if (ex != null)
                _logger.Error(ex, "[CLIENT] [{Component}] {Message}", component, message);
            else
                _logger.Error("[CLIENT] [{Component}] {Message}", component, message);

            await SendToServer("Error", message, ex?.ToString(), component);
        }

        private async Task SendToServer(string level, string message, string exception = null, string component = null)
        {
            try
            {
                var logEntry = new LogEntry
                {
                    Timestamp = DateTime.UtcNow,
                    Level = level,
                    Message = message,
                    Component = component ?? "Client",
                    Exception = exception != null ? new Exception(exception) : null
                };

                await _httpClient.PostAsJsonAsync("logs", logEntry);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "[CLIENT] Không thể gửi log tới server");
            }
        }

       
    }
}
