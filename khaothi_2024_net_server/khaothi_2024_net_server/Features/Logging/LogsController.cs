using khaothi_2024_net_server.Features.Logging.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace khaothi_2024_net_server.Features.Logging
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogsController : ControllerBase
    {
        private readonly ILogger<LogsController> _logger;

        public LogsController(ILogger<LogsController> logger)
        {
            _logger = logger;
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
    }
}
