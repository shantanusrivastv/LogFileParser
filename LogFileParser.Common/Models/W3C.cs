using System;

namespace LogFileParser.Common
{
    public class W3C
    {
        public DateTimeOffset Date;

        public DateTimeOffset Time;

        public string ClientIpAddress;

        public string UserName;

        public string ServerIpAddress;

        public string ServerPort;

        public string Method;

        public string UriStem;

        public string UriQuery;

        public string StatusCode;

        public string UserAgent;
    }
}