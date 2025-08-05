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
            var logFilePath = @"C:\Logs\application.log";
            _parser = new FileParserService(logFilePath);
        }

        [HttpGet("parse")]
        public IActionResult Parse()
        {
            var result = _parser.ParseLogFile();
            return Ok(result);
        }
    }
}