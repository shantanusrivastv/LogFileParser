using System;
using System.Collections.Concurrent;
using System.Linq;
using LogFileParser.Common;
using LogFileParser.Common.LogFormats;
using LogFileParser.Core;

namespace LogFileParser.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            FileParser<W3C> fileParser = new FileParser<W3C>();
            var mycollection = fileParser.GetAllLogsAsync().Result;
            ShowAllValues(mycollection);
            ShowWithGrouping(mycollection);
            Console.WriteLine("Operation Ended, press any key to close the windows");
            Console.ReadKey();
        }

        private static void ShowWithGrouping(ConcurrentBag<W3C> mycollection)
        {
            var groupedCollection = mycollection.GroupBy(x => x.ClientIpAddress)
                                   .Select(g => new
                                   {
                                       key = g.Key,
                                       count = g.Count()
                                   })
                                   .OrderByDescending(y => y.count);

            foreach (var item in groupedCollection)
            {
                Console.WriteLine($"The server IP is {item.key}, no of hits are {item.count}  ");
            }
        }

        private static void ShowAllValues(ConcurrentBag<W3C> mycollection)
        {
            foreach (var item in mycollection)
            {
                Console.WriteLine($"Printing for {item.ServerIpAddress}");
                foreach (var p in item.GetType().GetFields())
                {
                    Console.Write(p.Name + " : " + p.GetValue(item));
                    Console.WriteLine();
                }
            }
        }
    }
}