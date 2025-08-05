using System;
using System.IO;

namespace LogParserAPI.Services
{
    public class FileParserService
    {
        private readonly string _logFilePath;

        public FileParserService(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        public LogParserResult ParseLogFile()
        {
            var result = new LogParserResult();

            if (!File.Exists(_logFilePath))
                throw new FileNotFoundException("Log file not found", _logFilePath);

            var lines = File.ReadAllLines(_logFilePath);

            foreach (var line in lines)
            {
                if (line.Contains("ERROR", StringComparison.OrdinalIgnoreCase))
                {
                    result.Errors.Add(line);
                }
                else if (line.Contains("WARNING", StringComparison.OrdinalIgnoreCase))
                {
                    result.Warnings.Add(line);
                }
                else if (line.Contains("INFO", StringComparison.OrdinalIgnoreCase))
                {
                    result.Infos.Add(line);
                }
            }

            return result;
        }
    }
}