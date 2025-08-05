using System.Collections.Generic;

namespace LogParserAPI.Services
{
    public class LogParserResult
    {
        public List<string> Errors { get; } = new();
        public List<string> Warnings { get; } = new();
        public List<string> Infos { get; } = new();
    }
}