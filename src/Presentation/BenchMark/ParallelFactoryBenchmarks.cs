using BenchmarkDotNet.Attributes;
using Mahamudra.Core.Entity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Primes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mahamudra.Contemporary.BenchMark
{
    [MemoryDiagnoser]
    public class ParallelFactoryBenchmarks
    {
        private static Task<int> Sum(BaseEntity<int> number)
        {
            return Task.FromResult(PrimeFactory.PrimeNumbersLessEqualToN(number.Id).Sum());
        }

        private readonly IEnumerable<NumberOfPrimes> _primes;
        private readonly ParallelFactory _parallelFactory;
        public ParallelFactoryBenchmarks()
        {
            //
            //using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            //var logger = loggerFactory.CreateLogger<ParallelFactory>();
            //
            var logger = new NullLogger<ParallelFactory>();
            _primes = PrimeFactory.PrimeNumbersLessEqualToN(10000).Select(x => new NumberOfPrimes(x)); // find all primes less or equal to 10000 --> 3,5,7,,,
            _parallelFactory = new ParallelFactory(logger);
        }

        [Benchmark(Baseline = true)]
        public void ExecuteParallelResult()
        {
            _parallelFactory.ExecuteAsyncResult<BaseEntity<int>, int>(_primes, Sum);
        }
    }
}
