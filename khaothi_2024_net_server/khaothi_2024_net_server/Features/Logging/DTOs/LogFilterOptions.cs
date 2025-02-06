namespace khaothi_2024_net_server.Features.Logging.DTOs
{
    public class LogFilterOptions
    {
        public string? Level { get; set; }
        public string? Component { get; set; }
        public string? Search { get; set; }
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}
