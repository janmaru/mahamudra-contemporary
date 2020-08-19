using BenchmarkDotNet.Attributes;
using Mahamudra.Core.Entity;
using Microsoft.Extensions.Logging;
using Primes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mahamudra.Contemporary.BenchMark
{
    [MemoryDiagnoser]
    [RPlotExporter]
    public class ParallelAsyncFactoryBenchmarks : IParallelAsyncFactoryBenchmarks
    {

        private readonly IEnumerable<NumberOfPrimes> _primes;
        private readonly ParallelAsyncFactory _parallelFactory;
        private readonly IHttpService _httpService;

        public ParallelAsyncFactoryBenchmarks()
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<ParallelAsyncFactory>();

            _primes = PrimeFactory.FirstNIntegers(10).Select(x => new NumberOfPrimes(x)); // find all first 10 integer numbers
            _parallelFactory = new ParallelAsyncFactory(logger);
            _httpService = new HttpService();
        }

 

        [Benchmark(Baseline = true)]
        public async Task ExecuteAsyncResult()
        {
            await _parallelFactory.ExecuteAsyncResult<BaseEntity<int>, int>(_primes, _httpService.Get);
        }
    }
}
