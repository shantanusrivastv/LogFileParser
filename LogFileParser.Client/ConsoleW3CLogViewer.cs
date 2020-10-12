using System;
using System.Collections.Concurrent;
using System.Linq;
using LogFileParser.Common.LogFileFormats;
using LogFileParser.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace LogFileParser.Client
{
    public class ConsoleW3CLogViewer
    {
        private readonly string _samplePath = Environment.CurrentDirectory + "\\W3C.log";

        private readonly IFileParser<W3CLogFormat> _fileParser;
        private readonly ILogger<ConsoleW3CLogViewer> _logger;

        public ConsoleW3CLogViewer(IFileParser<W3CLogFormat> fileParser, ILogger<ConsoleW3CLogViewer> logger)
        {
            _fileParser = fileParser;
            _logger = logger;
        }

        public void Run()
        {
            _logger.LogInformation("Starting Log Parsing");
            var logResults = _fileParser.GetAllLogsAsync(_samplePath).Result;
            _logger.LogInformation("Parsing Completed");
            ShowAllValues(logResults);
            ShowWithGrouping(logResults);
        }

        private static void ShowWithGrouping(ConcurrentBag<W3CLogFormat> logResults)
        {
            var groupedCollection = logResults.GroupBy(x => x.ClientIpAddress)
                                   .Select(g => new
                                   {
                                       key = g.Key,
                                       count = g.Count()
                                   })
                                   .OrderByDescending(y => y.count);

            foreach (var item in groupedCollection)
            {
                Console.WriteLine($"The server IP is {item.key}, no of hits are {item.count}  ");
            }
        }

        private static void ShowAllValues(ConcurrentBag<W3CLogFormat> logResults)
        {
            foreach (var item in logResults)
            {
                Console.WriteLine($"Printing for {item.ServerIpAddress}");
                foreach (var p in item.GetType().GetFields())
                {
                    Console.Write(p.Name + " : " + p.GetValue(item));
                    Console.WriteLine();
                }
            }
        }
    }
}