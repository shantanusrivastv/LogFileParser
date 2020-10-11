using System;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
            ShowWithGrouping(mycollection);
            Console.WriteLine("Operation Ended, press any key to close the windows");
            Console.ReadKey();
        }

        private static void ShowWithGrouping(ConcurrentBag<W3C> mycollection)
        {
            var groupedCollection = mycollection.GroupBy(x => x.UserAgent)
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

        private static void ShowAllValues(ConcurrentBag<W3C> mycollection)
        {
            foreach (var item in mycollection)
            {
                Console.WriteLine($"Printing for {item.ServerIpAddress}");
                foreach (var p in item.GetType().GetFields())
                {
                    Console.WriteLine(p.Name + " : " + p.GetValue(item));
                }
            }
        }
    }
}