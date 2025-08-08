using System;
using System.IO;

namespace LogParserAPI.Services
{
    public class FileParserService
    {
        private readonly string? _logFilePath;
        private readonly Stream? _logStream;

        // Constructor for file path usage
        public FileParserService(string logFilePath)
        {
            _logFilePath = logFilePath;
        }

        // Constructor for stream usage (e.g., file upload)
        public FileParserService(Stream logStream)
        {
            _logStream = logStream;
        }

        public LogParserResult ParseLogFile()
        {
            var result = new LogParserResult();
            string[] lines;

            if (_logStream != null)
            {
                // Read from uploaded file stream
                using var reader = new StreamReader(_logStream);
                var fileContent = reader.ReadToEnd();
                lines = fileContent.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            }
            else if (!string.IsNullOrEmpty(_logFilePath) && File.Exists(_logFilePath))
            {
                // Read from fixed file path
                lines = File.ReadAllLines(_logFilePath);
            }
            else
            {
                throw new FileNotFoundException("No valid log file source provided.");
            }

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

        public void GenerateFilteredLogs(string outputDirectory)
        {
            var result = ParseLogFile();

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            File.WriteAllLines(Path.Combine(outputDirectory, "Errors.log"), result.Errors);
            File.WriteAllLines(Path.Combine(outputDirectory, "Warnings.log"), result.Warnings);
            File.WriteAllLines(Path.Combine(outputDirectory, "Info.log"), result.Infos);
        }
    }
}
