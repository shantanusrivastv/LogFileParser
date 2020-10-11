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
            var threadSafeCollection = new ConcurrentBag<T>();
            var allLines = await File.ReadAllLinesAsync(path);
            IList<T> parsedList = new List<T>();
            Parallel.ForEach(allLines, (line) =>
            {
                if (!line.StartsWith("#"))
                {
                    var props = line.Split();
                    var parsedLines = parser.TryParse<T>(props);
                    threadSafeCollection.Add(parsedLines);
                }
            });

            return threadSafeCollection;
        }
    }
}