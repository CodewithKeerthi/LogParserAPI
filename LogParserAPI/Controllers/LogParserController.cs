using Microsoft.AspNetCore.Mvc;
using LogParserAPI.Services;
using System.IO;

namespace LogParserAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogParserController : ControllerBase
    {
        // Default parser using fixed file path
        private readonly FileParserService _pathParser;
        
        public LogParserController()
        {
            var logFilePath = @"C:\Logs\application.log"; // Or inject via configuration
            _pathParser = new FileParserService(logFilePath);
        }

        // GET: /api/logparser/parse
        [HttpGet("parse")]
        public IActionResult ParseFromPath()
        {
            var result = _pathParser.ParseLogFile();
            return Ok(result);
        }

        // POST: /api/logparser/parse-upload
        [HttpPost("parse-upload")]
        public IActionResult ParseUploadedFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            var parser = new FileParserService(stream);
            var result = parser.ParseLogFile();
            return Ok(result);
        }

        // POST: /api/logparser/generate-logs?outputPath=...
        [HttpPost("generate-logs")]
        public IActionResult GenerateLogs([FromQuery] string outputPath)
        {
            try
            {
                _pathParser.GenerateFilteredLogs(outputPath);
                return Ok(new { message = "Logs generated successfully", path = outputPath });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        // POST: /api/logparser/generate-logs-upload
        [HttpPost("generate-logs-upload")]
        public IActionResult GenerateLogsFromUpload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            using var stream = file.OpenReadStream();
            var parser = new FileParserService(stream);

            // Prepare a list of lines for each category
            var result = parser.ParseLogFile();

            using var memStream = new MemoryStream();
            using var writer = new StreamWriter(memStream);
            
            writer.WriteLine("=== ERRORS ===");
            result.Errors.ForEach(writer.WriteLine);
            writer.WriteLine("\n=== WARNINGS ===");
            result.Warnings.ForEach(writer.WriteLine);
            writer.WriteLine("\n=== INFO ===");
            result.Infos.ForEach(writer.WriteLine);
            writer.Flush();
            
            memStream.Position = 0;
            return File(memStream, "text/plain", "FilteredLogs.txt");
        }
    }
}
