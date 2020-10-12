using System;

namespace LogFileParser.Common.LogFileFormats
{
    public class IISLogFormat
    {
        public string ClientIpAddress;

        public string UserName;

        public DateTime Date;

        public TimeSpan Time;

        public string ServiceInstance;

        public string ServerName;

        public string ServerIpAddress;

        public ushort TimeTaken;

        public ushort ClientBytesSent;

        public ushort ServerBytesSent;

        public ushort StatusCode;

        public ushort WinStatusCode;

        public string Method;

        public string Target;

        public string Parameters;
    }
}