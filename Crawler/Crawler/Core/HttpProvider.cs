using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Crawler.App.Core
{
    public class HttpProvider : IDataProvider, IDisposable
    {
        private readonly HttpClient Client = new HttpClient();

        public async Task<string> GetFrom(string url)
        {
            var uri = new Uri(url);
            var response = await Client.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            string responceBody = await response.Content.ReadAsStringAsync();
            return responceBody;
        }

        public void Dispose()
        {
            Client.Dispose();
        }
    }
}
