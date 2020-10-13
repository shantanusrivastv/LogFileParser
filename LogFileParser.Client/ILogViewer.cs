using System;
using System.Collections.Concurrent;

namespace LogFileParser.Client
{
    public interface ILogViewer
    {
        void DisplayWithGrouping<TLogFileFormat, TKey>(ConcurrentBag<TLogFileFormat> logResults, Func<TLogFileFormat, TKey> keySelector);

        void DisplayWithFilters<TLogFileFormat>(ConcurrentBag<TLogFileFormat> logResults, Func<TLogFileFormat, bool> predicate);
    }
}