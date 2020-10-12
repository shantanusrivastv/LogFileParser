using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace LogFileParser.Core.Interfaces
{
    public interface IFileParser<TLogFileFormat> where TLogFileFormat : class, new()
    {
        Task<ConcurrentBag<TLogFileFormat>> GetAllLogsAsync(string path);
    }
}