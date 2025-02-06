using khaothi_2024_net_server.Features.Logging.DTOs;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace khaothi_2024_net_server.Features.Logging
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly ILogger<LogsController> _logger;
        private readonly IWebHostEnvironment _env;

        public LogsController(ILogger<LogsController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        [HttpPost]
        public IActionResult Post([FromBody] LogEntry logEntry)
        {
            try
            {
                _logger.LogInformation(
                    "[{Component}] {Message} {Exception}",
                    logEntry.Component,
                    logEntry.Message,
                    logEntry.Exception?.ToString() ?? string.Empty
                );
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi ghi log");
                return StatusCode(500, "Lỗi server khi ghi log");
            }
        }

        [HttpGet]
        public IActionResult GetLogs([FromQuery] LogFilterOptions options)
        {
            try
            {
                var currentDate = DateTime.Now.ToString("yyyyMMdd");
                var logPath = Path.Combine("logs", $"log-{currentDate}.txt");

                if (!System.IO.File.Exists(logPath))
                {
                    return NotFound(new { message = "Không tìm thấy file log cho ngày hôm nay" });
                }

                var logs = new List<LogEntry>();
                using (var fs = new FileStream(logPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var sr = new StreamReader(fs))
                {
                    string? line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        var logEntry = ParseLogLine(line);
                        if (ShouldIncludeLog(logEntry, options))
                        {
                            logs.Add(logEntry);
                        }
                    }
                }

                // Áp dụng phân trang
                var totalCount = logs.Count;
                var pageSize = options.PageSize ?? 50;
                var pageNumber = options.Page ?? 1;
                var pagedLogs = logs
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new
                {
                    totalCount,
                    pageSize,
                    currentPage = pageNumber,
                    totalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    logs = pagedLogs
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi đọc log");
                return StatusCode(500, new { message = "Lỗi khi đọc log", error = ex.Message });
            }
        }

        [HttpGet("dates")]
        public IActionResult GetAvailableDates()
        {
            try
            {
                var logsDirectory = Path.Combine("logs");
                if (!Directory.Exists(logsDirectory))
                {
                    return Ok(new { dates = new List<string>() });
                }

                var logFiles = Directory.GetFiles(logsDirectory, "log-*.txt");
                var dates = logFiles
                    .Select(f => Path.GetFileName(f))
                    .Where(f => Regex.IsMatch(f, @"log-\d{8}\.txt"))
                    .Select(f => DateTime.ParseExact(f.Substring(4, 8), "yyyyMMdd", null))
                    .OrderByDescending(d => d)
                    .Select(d => d.ToString("yyyy-MM-dd"))
                    .ToList();

                return Ok(new { dates });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách ngày có log");
                return StatusCode(500, new { message = "Lỗi khi lấy danh sách ngày", error = ex.Message });
            }
        }

        private LogEntry ParseLogLine(string line)
        {
            try
            {
                // Mẫu regex để parse log line
                var match = Regex.Match(line, @"(\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}.\d{3} [+-]\d{2}:\d{2}) \[(\w+)\] \[([^\]]*)\] (.*?)(?:\s*\{([^}]*)\})?$");

                if (match.Success)
                {
                    return new LogEntry
                    {
                        Timestamp = DateTime.Parse(match.Groups[1].Value),
                        Level = match.Groups[2].Value,
                        Component = match.Groups[3].Value,
                        Message = match.Groups[4].Value.Trim(),
                        Exception = match.Groups[5].Success ? match.Groups[5].Value : null
                    };
                }

                // Fallback nếu không match được format
                return new LogEntry
                {
                    Timestamp = DateTime.Now,
                    Level = "INFO",
                    Component = "Unknown",
                    Message = line
                };
            }
            catch
            {
                return new LogEntry
                {
                    Timestamp = DateTime.Now,
                    Level = "ERROR",
                    Component = "Parser",
                    Message = "Could not parse log line",
                    Exception = line
                };
            }
        }

        private bool ShouldIncludeLog(LogEntry log, LogFilterOptions options)
        {
            if (!string.IsNullOrEmpty(options.Level) &&
                !log.Level.Equals(options.Level, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(options.Component) &&
                !log.Component.Contains(options.Component, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (!string.IsNullOrEmpty(options.Search) &&
                !log.Message.Contains(options.Search, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return true;
        }
    
}
}
