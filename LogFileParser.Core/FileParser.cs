using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using LogFileParser.Common.LogFormats;
using System.Linq;

namespace LogFileParser.Core
{
    public class FileParser<LogFileFormat> where LogFileFormat : class, new()
    {
        private string path = @"D:\VS\LogFileParser\Sample-logs\W3C.txt";
        private Parser parser;

        public FileParser()
        {
            parser = new Parser();
        }

        public async Task<ConcurrentBag<LogFileFormat>> GetAllLogsAsync()
        {
            var threadSafeCollection = new ConcurrentBag<LogFileFormat>();
            var allLogs = await File.ReadAllLinesAsync(path);
            Parallel.ForEach(allLogs, (log) =>
            {
                if (!log.StartsWith("#"))
                {
                    var fields = GetLogFields(log);
                    var parsedLog = parser.TryParse<LogFileFormat>(fields);
                    threadSafeCollection.Add(parsedLog);
                }
            });

            return threadSafeCollection;
        }

        private string[] GetLogFields(string log)
        {
            switch (typeof(LogFileFormat))
            {
                case Type format when (format == typeof(W3C) || format == typeof(W3Cv1dot1)):
                    return log.Split();

                case Type format when format == typeof(IIS):
                    return log.Split(",").SkipLast(1).ToArray(); //For IIS Log last element ends with comma

                default:
                    throw new ArgumentException("Unsupported Type");
            }
        }
    }
}