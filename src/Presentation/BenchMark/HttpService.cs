using Mahamudra.Core.Entity;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mahamudra.Contemporary.BenchMark
{
    public class HttpService
    {
        private static readonly HttpClient client = new HttpClient();

        public static async Task<int> Get(BaseEntity<int> number)
        {
            HttpResponseMessage response = await client.GetAsync($"https://projecteuler.net/problem={number}");
            response.EnsureSuccessStatusCode();
            await response.Content.ReadAsStringAsync();
            return number.Id;
        }
    }
}
