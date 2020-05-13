using System;
using System.IO;
using System.Threading.Tasks;

namespace Crawler.App.Core
{
    public class FileSystemProvider : IDataProvider
    {
        public async Task<string> GetFrom(string url)
        {
            return await File.ReadAllTextAsync(new Uri(url).AbsolutePath);
        }
    }
}
