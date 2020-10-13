using System;
using System.Collections.Concurrent;
using System.Linq;

namespace LogFileParser.Client
{
    public class LogViewer : ILogViewer
    {
        public void DisplayWithGrouping<TLogFileFormat, TKey>(ConcurrentBag<TLogFileFormat> logResults,
                                        Func<TLogFileFormat, TKey> keySelector)
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
                foreach (var field in item.GetType().GetFields())
                {
                    if (field.FieldType == typeof(DateTime))
                    {
                        var onlyDateValue = ((DateTime)field.GetValue(item)).ToShortDateString();
                        Console.WriteLine(field.Name + " : " + onlyDateValue);
                        continue;
                    }
                    Console.Write(field.Name + " : " + field.GetValue(item));
                    Console.WriteLine();
                }

                Console.Write("---------Next Record----------------" + Environment.NewLine);
            }
        }
    }
}