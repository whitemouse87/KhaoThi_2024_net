namespace khaothi_2024_net_server.Features.Logging.DTOs
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Component { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string? Exception { get; set; }
    }
}
