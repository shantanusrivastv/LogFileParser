using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogFileParser.Core
{
    public class FileParser<T> where T : class, new()
    {
        private string path = @"D:\sample-log.txt";
        private Parser parser;

        public FileParser()
        {
            parser = new Parser();
        }

        public async Task<ConcurrentBag<T>> GetMappedCollectionAsync()
        {
            var mappedCollection = new ConcurrentBag<T>();
            var test = await File.ReadAllLinesAsync(path);
            IList<T> w3CEvents = new List<T>();
            Parallel.ForEach(test, (item) =>
            {
                if (!item.StartsWith("#"))
                {
                    var props = item.Split();
                    var row = parser.Parse<T>(props);
                    if (row != null)
                    {
                        mappedCollection.Add(row);
                    }
                }
            });

            return mappedCollection;
        }
    }
}