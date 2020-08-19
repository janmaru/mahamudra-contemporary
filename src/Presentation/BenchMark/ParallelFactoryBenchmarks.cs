using BenchmarkDotNet.Attributes;
using Mahamudra.Core.Entity;
using Microsoft.Extensions.Logging.Abstractions;
using Primes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mahamudra.Contemporary.BenchMark
{
    [MemoryDiagnoser]
    [RPlotExporter]
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
            //_primes = PrimeFactory.PrimeNumbersLessEqualToN(100).Select(x => new NumberOfPrimes(x)); // find all primes less or equal to 100 --> 3,5,7,,,
            _primes = PrimeFactory.FirstNIntegers(10).Select(x => new NumberOfPrimes(x)); // find all first 10 integer numbers
            _parallelFactory = new ParallelFactory(logger);
        }

        [Benchmark(Baseline = true)]
        public void ExecuteParallelResult()
        {
            _parallelFactory.ExecuteAsyncResult<BaseEntity<int>, int>(_primes, Sum);
        }
    }
}
