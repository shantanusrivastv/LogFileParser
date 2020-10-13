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
        private readonly ILogViewer _viewer;

        public ConsoleW3CLogViewer(IFileParser<W3CLogFormat> fileParser,
                                    ILogger<ConsoleW3CLogViewer> logger,
                                    ILogViewer viewer)
        {
            _fileParser = fileParser;
            _logger = logger;
            _viewer = viewer;
        }

        public void Run()
        {
            _logger.LogInformation("Starting Log Parsing");
            var logResults = _fileParser.GetAllLogsAsync(_samplePath).Result;
            _logger.LogInformation("Parsing Completed");

            Func<W3CLogFormat, string> clientIPGrouping = (x) => x.ClientIpAddress;
            Func<W3CLogFormat, string> serverIPGrouping = (x) => x.ServerIpAddress;
            Func<W3CLogFormat, int> statusCodeGrouping = (x) => x.StatusCode;
            Func<W3CLogFormat, string> userAgentGrouping = (x) => x.UserAgent;

            _viewer.DisplayWithGrouping(logResults, clientIPGrouping);
            _viewer.DisplayWithGrouping(logResults, serverIPGrouping);
            _viewer.DisplayWithGrouping(logResults, statusCodeGrouping);
            _viewer.DisplayWithGrouping(logResults, userAgentGrouping);

            _viewer.DisplayWithFilters(logResults, x => x.Date >= DateTime.Now.AddYears(-4));
        }
    }
}