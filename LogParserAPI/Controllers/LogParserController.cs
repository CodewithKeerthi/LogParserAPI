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

        /// <summary>
        /// Parse the default log file from disk.
        /// </summary>
        [HttpGet("parse")]
        public IActionResult Parse()
        {
            var result = _parser.ParseLogFile();
            return Ok(result);
        }

        /// <summary>
        /// Parse logs from an uploaded file.
        /// </summary>
        [HttpPost("parse-upload")]
        public IActionResult ParseUploadedFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            using var stream = file.OpenReadStream();
            var result = _parser.ParseLogStream(stream);

            return Ok(result);
        }

        /// <summary>
        /// Generate filtered logs to a specific path.
        /// </summary>
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
