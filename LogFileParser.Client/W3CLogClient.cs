using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using LogFileParser.Common.LogFileFormats;
using LogFileParser.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace LogFileParser.Client
{
    public class W3CLogClient
    {
        private readonly IFileParser<W3CLogFormat> _fileParser;
        private readonly ILogger<W3CLogClient> _logger;
        private readonly IConfiguration _config;
        private readonly ILogViewer _viewer;

        public W3CLogClient(IFileParser<W3CLogFormat> fileParser,
                                    ILogger<W3CLogClient> logger,
                                    IConfiguration config,
                                    ILogViewer viewer)
        {
            _fileParser = fileParser;
            _logger = logger;
            _config = config;
            _viewer = viewer;
        }

        public async Task StartAsync()
        {
            _logger.LogInformation("Starting Log Parsing");
            ConcurrentBag<W3CLogFormat> logResults;

            var folderPath = (string)_config.GetValue(typeof(string), "LogFolder") ??
                                    Environment.CurrentDirectory;
            try
            {
                logResults = await _fileParser.GetAllLogsAsync(folderPath);
            }
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    _logger.LogError("Error while excuting Parsing Task ", e.Message);
                }
                return; //Exiting
            }
            catch (Exception ex)
            {
                _logger.LogError("Something went wrong, look at logs for more details "
                                 , ex.Message);
                return; //Exiting
            }
            _logger.LogInformation("Parsing Completed");

            //Sampple Grouping Filters
            Func<W3CLogFormat, string> UrlGrouping = (x) => x.UriStem; //Main requirement
            Func<W3CLogFormat, string> clientIPGrouping = (x) => x.ClientIpAddress;
            Func<W3CLogFormat, string> serverIPGrouping = (x) => x.ServerIpAddress;
            Func<W3CLogFormat, int> statusCodeGrouping = (x) => x.StatusCode;
            Func<W3CLogFormat, string> userAgentGrouping = (x) => x.UserAgent;

            //Sample Grouping Results based on above filters
            _viewer.DisplayWithGrouping(logResults, UrlGrouping);
            _viewer.DisplayWithGrouping(logResults, clientIPGrouping);
            _viewer.DisplayWithGrouping(logResults, serverIPGrouping);
            _viewer.DisplayWithGrouping(logResults, statusCodeGrouping);
            _viewer.DisplayWithGrouping(logResults, userAgentGrouping);

            //Sample Filtering Results
            _viewer.DisplayWithFilters(logResults, x => x.Date >= DateTime.Now.AddYears(-1)); //within a year
        }
    }
}