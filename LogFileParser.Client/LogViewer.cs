using System;
using System.Collections.Concurrent;
using System.Linq;

namespace LogFileParser.Client
{
    public class LogViewer : ILogViewer
    {
        public void DisplayWithGrouping<TFileFormat, TKey>(ConcurrentBag<TFileFormat> logResults,
                                        Func<TFileFormat, TKey> keySelector)
        {
            var groupedCollection = logResults.GroupBy(keySelector)
                                               .Select(g => new
                                               {
                                                   key = g.Key,
                                                   count = g.Count()
                                               })
                                   .OrderByDescending(y => y.count);

            foreach (var item in groupedCollection)
            {
                Console.WriteLine($"The grouping key is {item.key}," +
                                   $" total count {(item.count > 1 ? "are" : "is")}  {item.count}");
            }
        }

        public void DisplayWithFilters<TFileFormat>(ConcurrentBag<TFileFormat> logResults,
                                        Func<TFileFormat, bool> predicate)
        {
            var filteredResult = logResults.Where(predicate);
            Console.WriteLine(Environment.NewLine + "Showing Filtered Result :" + Environment.NewLine);
            foreach (var item in filteredResult)
            {
                foreach (var p in item.GetType().GetFields())
                {
                    if (p.FieldType == typeof(DateTime))
                    {
                        var onlyDateValue = ((DateTime)p.GetValue(item)).ToShortDateString();
                        Console.WriteLine(p.Name + " : " + onlyDateValue);
                        continue;
                    }
                    Console.Write(p.Name + " : " + p.GetValue(item));
                    Console.WriteLine();
                }

                Console.Write("---------Next Record----------------" + Environment.NewLine);
            }
        }
    }
}