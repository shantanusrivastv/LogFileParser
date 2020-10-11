using System;
using System.Collections.Concurrent;
using System.Linq;
using LogFileParser.Common;
using LogFileParser.Core;

namespace LogFileParser.ConsoleUI
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            FileParser<W3C> fileParser = new FileParser<W3C>();
            var mycollection = fileParser.GetMappedCollectionAsync().Result;
            Show(mycollection);
            Console.WriteLine("Operation Ended, press any key to close the windows");
            Console.ReadKey();
        }

        private static void ShowWithGrouping(ConcurrentBag<W3C> mycollection)
        {
            var groupedCollection = mycollection.GroupBy(x => x.ServerIpAddress)
                                   .Select(g => new
                                   {
                                       key = g.Key,
                                       count = g.Count()
                                   })
                                   .OrderByDescending(y => y.count);

            foreach (var item in groupedCollection)
            {
                Console.WriteLine($"The server IP is {item.key}, no of hits is {item.count}  ");
            }
        }

        private static void Show(ConcurrentBag<W3C> mycollection)
        {
            foreach (var item in mycollection)
            {
                Console.WriteLine($"The server IP is {item.ClientIpAddress}," +
                                  $"Verb request is { item.Method}, " +
                                  $"Http Status Code is {item.StatusCode}");
            }
        }
    }
}