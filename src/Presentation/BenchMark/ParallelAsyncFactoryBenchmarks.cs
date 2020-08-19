using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using Mahamudra.Core.Entity;
using Microsoft.Extensions.Logging.Abstractions;
using Primes;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mahamudra.Contemporary.BenchMark
{
    [MemoryDiagnoser]
    [RPlotExporter]
    public class ParallelAsyncFactoryBenchmarks
    {
 
        private readonly IEnumerable<NumberOfPrimes> _primes;
        private readonly ParallelAsyncFactory _parallelFactory;
        public ParallelAsyncFactoryBenchmarks()
        {
            //using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            //var logger = loggerFactory.CreateLogger<ParallelAsyncFactory>();
            var logger = new NullLogger<ParallelFactory>();
            _primes = PrimeFactory.FirstNIntegers(10).Select(x => new NumberOfPrimes(x)); // find all first 10 integer numbers
            _parallelFactory = new ParallelAsyncFactory(logger);
        }

        [Benchmark(Baseline = true)]
        public async Task ExecuteAsyncResult()
        {
            await _parallelFactory.ExecuteAsyncResult<BaseEntity<int>, int>(_primes, HttpService.Get);
        }
    }
}
