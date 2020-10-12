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
        private Parser parser;

        public FileParser()
        {
            parser = new Parser();
        }

        public async Task<ConcurrentBag<LogFileFormat>> GetAllLogsAsync(string path)
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
                case Type format when (format == typeof(W3CLogFormat) || format == typeof(W3Cv1LogFormat)):
                    return log.Split();

                case Type format when format == typeof(IISLogFormat):
                    return log.Split(",").SkipLast(1).ToArray(); //For IISLogFormat Log last element ends with comma

                default:
                    throw new ArgumentException("Unsupported Type");
            }
        }
    }
}