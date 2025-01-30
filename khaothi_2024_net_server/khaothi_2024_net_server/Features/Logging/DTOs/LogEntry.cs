namespace khaothi_2024_net_server.Features.Logging.DTOs
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string? Level { get; set; }  // DEBUG, INFO, ERROR
        public string? Message { get; set; }
        public string? Component { get; set; }
        public Exception? Exception { get; set; }
    }
}
