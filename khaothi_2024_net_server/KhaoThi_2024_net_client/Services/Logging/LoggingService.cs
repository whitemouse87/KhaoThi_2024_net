using KhaoThi_2024_net_client.Models;
using Serilog;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
namespace KhaoThi_2024_net_client.Services.Logging
{
    public class LoggingService : ILoggingService
    {
        private readonly Serilog.ILogger _logger;
        private readonly HttpClient _httpClient;

        public LoggingService(IHttpClientFactory httpClientFactory)
        {
            _logger = Serilog.Log.ForContext<LoggingService>();
            _httpClient = httpClientFactory?.CreateClient("API") ??
                throw new ArgumentNullException(nameof(httpClientFactory));
        }
        public async Task LogDebugAsync(string message, string component = null)
        {
            // Chỉ log debug trong môi trường development
#if DEBUG
            _logger.Debug("[CLIENT] [{Component}] {Message}", component, message);
#endif
        }

        public async Task LogInfoAsync(string message, string component = null)
        {
            // Chỉ log info trong môi trường development
#if DEBUG
            _logger.Information("[CLIENT] [{Component}] {Message}", component, message);
#endif
        }

        public async Task LogWarningAsync(string message, string component = null)
        {
            _logger.Warning("[CLIENT] [{Component}] {Message}", component, message);
            await SendToServer("Warning", message, null, component);
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
            // Chỉ gửi Warning và Error
            if (level != "Warning" && level != "Error")
                return;

            const int maxRetries = 3;
            var retryCount = 0;

            while (retryCount < maxRetries)
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

                    using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                    await _httpClient.PostAsJsonAsync("logs", logEntry, cts.Token);
                    return; // Nếu thành công thì thoát
                }
                catch (Exception ex) when (retryCount < maxRetries - 1)
                {
                    retryCount++;
                    await Task.Delay(1000 * retryCount); // Delay tăng dần theo số lần retry
                    continue;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "[CLIENT] Không thể gửi log tới server sau {RetryCount} lần thử", retryCount);
                    break;
                }
            }
        }


    }
}
