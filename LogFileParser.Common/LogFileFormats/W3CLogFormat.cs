using System;

namespace LogFileParser.Common.LogFileFormats
{
    public class W3CLogFormat : ILogFormat
    {
        public DateTime Date;

        public TimeSpan Time;

        public string ClientIpAddress;

        public string UserName;

        public string ServerIpAddress;

        public ushort ServerPort;

        public string Method;

        public string UriStem;

        public string UriQuery;

        public ushort StatusCode;

        public string UserAgent;
    }
}