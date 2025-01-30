namespace KhaoThi_2024_net_client.Models
{
    public class LogEntry
    {
        public DateTime Timestamp { get; set; }
        public string? Level { get; set; }  // DEBUG, INFO, ERROR, WARNING
        public string? Message { get; set; }
        public string? Component { get; set; }
        public Exception? Exception { get; set; }
    }
}
