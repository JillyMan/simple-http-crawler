using System.Threading.Tasks;

namespace Crawler.App.Core
{
    public interface IDataProvider
    {
        Task<string> GetFrom(string url);
    }
}
