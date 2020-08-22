using Mahamudra.Core.Entity;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mahamudra.Contemporary.BenchMark
{
    public class HttpErrorService : IHttpErrorService
    {
        private readonly HttpClient _client;

        public HttpErrorService(HttpClient client)
        {
            this._client = client;
        }

        public HttpErrorService()
        {
            this._client = new HttpClient();
        }

        public async Task<int> Get(BaseEntity<int> number)
        { 
            HttpResponseMessage response = await _client.GetAsync($"https://projecteuler.net3/problem={number}");
            response.EnsureSuccessStatusCode();
            await response.Content.ReadAsStringAsync();
            return number.Id;
        }
    }
}
