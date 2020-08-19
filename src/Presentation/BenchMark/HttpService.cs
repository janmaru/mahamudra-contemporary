using Mahamudra.Core.Entity;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mahamudra.Contemporary.BenchMark
{
    public class HttpService : IHttpService
    {
        private readonly HttpClient _client;

        public HttpService(HttpClient client)
        {
            this._client = client;
        }

        public HttpService()
        {
            this._client = new HttpClient();
        }

        public async Task<int> Get(BaseEntity<int> number)
        {
            HttpResponseMessage response = await _client.GetAsync($"https://projecteuler.net/problem={number}");
            response.EnsureSuccessStatusCode();
            await response.Content.ReadAsStringAsync();
            return number.Id;
        }
    }
}
