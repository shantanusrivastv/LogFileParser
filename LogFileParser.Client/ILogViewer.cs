using System;
using System.Collections.Concurrent;

namespace LogFileParser.Client
{
    public interface ILogViewer
    {
        void DisplayWithGrouping<TFileFormat, TKey>(ConcurrentBag<TFileFormat> logResults, Func<TFileFormat, TKey> keySelector);

        void DisplayWithFilters<TFileFormat>(ConcurrentBag<TFileFormat> logResults, Func<TFileFormat, bool> predicate);
    }
}