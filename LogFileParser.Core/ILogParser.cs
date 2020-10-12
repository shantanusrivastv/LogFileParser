﻿namespace LogFileParser.Core
{
    public interface ILogParser
    {
        T Parse<T>(params string[] logFields) where T : class, new();
        T TryParse<T>(params string[] logFields) where T : class, new();
    }
}