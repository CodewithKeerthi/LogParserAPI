using Microsoft.AspNetCore.Mvc;
using LogParserAPI.Services;

namespace LogParserAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogParserController : ControllerBase
    {
        private readonly FileParserService _parser;

        public LogParserController()
        {
            var logFilePath = @"C:\Logs\application.log"; // This can also be injected from config
            _parser = new FileParserService(logFilePath);
        }

        [HttpGet("parse")]
        public IActionResult Parse()
        {
            var result = _parser.ParseLogFile();
            return Ok(result);
        }

        [HttpPost("generate-logs")]
        public IActionResult GenerateLogs([FromQuery] string outputPath)
        {
            try
            {
                _parser.GenerateFilteredLogs(outputPath);
                return Ok(new { message = "Logs generated successfully", path = outputPath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
