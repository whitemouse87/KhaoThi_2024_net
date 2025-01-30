namespace KhaoThi_2024_net_client.Services.Logging
{
    public interface ILoggingService
    {
        Task LogDebugAsync(string message, string component = null);
        Task LogInfoAsync(string message, string component = null);
        Task LogWarningAsync(string message, string component = null);
        Task LogErrorAsync(string message, Exception ex = null, string component = null);
        
    }
}
