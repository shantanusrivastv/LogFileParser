using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace LogFileParser.Core
{
    public class FileParser<FileFormat> where FileFormat : class, new()
    {
        private string path = @"D:\sample-log.txt";
        private Parser parser;

        public FileParser()
        {
            parser = new Parser();
        }

        public async Task<ConcurrentBag<FileFormat>> GetAllLogsAsync()
        {
            var threadSafeCollection = new ConcurrentBag<FileFormat>();
            var allLines = await File.ReadAllLinesAsync(path);
            Parallel.ForEach(allLines, (line) =>
            {
                if (!line.StartsWith("#"))
                {
                    var props = line.Split();
                    var parsedLog = parser.TryParse<FileFormat>(props);
                    threadSafeCollection.Add(parsedLog);
                }
            });

            return threadSafeCollection;
        }
    }
}