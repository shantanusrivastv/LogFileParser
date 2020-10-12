using System;

namespace LogFileParser.Common.LogFileFormats
{
    public class W3Cv1LogFormat
    {
        public DateTime Date;

        public TimeSpan Time;

        public string ServerIpAddress;

        public string Method;

        public string UriStem;

        public string UriQuery;

        public ushort ServerPort;

        public string UserName;

        public string ClientIpAddress;

        public string UserAgent;

        public ushort StatusCode;

        public ushort TimeTaken;
    }
}