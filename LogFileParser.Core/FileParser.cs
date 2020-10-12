using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LogFileParser.Common.LogFileFormats;
using LogFileParser.Core.Interfaces;

namespace LogFileParser.Core
{
    public class FileParser<TLogFileFormat> : IFileParser<TLogFileFormat> where TLogFileFormat : class, new()
    {
        private readonly ILogParser _logParser;

        public FileParser(ILogParser logParser)
        {
            _logParser = logParser;
        }

        public async Task<ConcurrentBag<TLogFileFormat>> GetAllLogsAsync(string path)
        {
            var threadSafeCollection = new ConcurrentBag<TLogFileFormat>();
            var allLogs = await File.ReadAllLinesAsync(path);
            Parallel.ForEach(allLogs, log =>
            {
                if (!log.StartsWith("#")) //Ignoring Commented lines
                {
                    var fields = GetLogFields(log);
                    var parsedLog = _logParser.TryParse<TLogFileFormat>(fields);
                    threadSafeCollection.Add(parsedLog);
                }
            });

            return threadSafeCollection;
        }

        private static string[] GetLogFields(string log)
        {
            switch (typeof(TLogFileFormat))
            {
                case { } format when format == typeof(W3CLogFormat) || format == typeof(W3Cv1LogFormat):
                    return log.Split();

                case { } format when format == typeof(IISLogFormat):
                    return log.Split(",").SkipLast(1).ToArray(); //For IISLogFormat last element ends with comma

                default:
                    throw new ArgumentException("Unsupported Type");
            }
        }
    }
}