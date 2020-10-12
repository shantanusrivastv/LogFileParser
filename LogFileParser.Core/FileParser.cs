using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using LogFileParser.Common.LogFormats;
using System.Linq;

namespace LogFileParser.Core
{
    public class FileParser<TLogFileFormat> where TLogFileFormat : class, new()
    {
        private readonly ILogParser _logParser;

        public FileParser()
        {
            _logParser = new LogParser();
        }

        public async Task<ConcurrentBag<TLogFileFormat>> GetAllLogsAsync(string path)
        {
            var threadSafeCollection = new ConcurrentBag<TLogFileFormat>();
            var allLogs = await File.ReadAllLinesAsync(path);
            Parallel.ForEach(allLogs, (log) =>
            {
                if (!log.StartsWith("#"))
                {
                    var fields = GetLogFields(log);
                    var parsedLog = _logParser.TryParse<TLogFileFormat>(fields);
                    threadSafeCollection.Add(parsedLog);
                }
            });

            return threadSafeCollection;
        }

        private string[] GetLogFields(string log)
        {
            switch (typeof(TLogFileFormat))
            {
                case { } format when (format == typeof(W3CLogFormat) || format == typeof(W3Cv1LogFormat)):
                    return log.Split();

                case { } format when format == typeof(IISLogFormat):
                    return log.Split(",").SkipLast(1).ToArray(); //For IISLogFormat last element ends with comma

                default:
                    throw new ArgumentException("Unsupported Type");
            }
        }
    }
}