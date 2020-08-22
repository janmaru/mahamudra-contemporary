using Mahamudra.Core.Entity;
using Microsoft.Extensions.Logging;
using Primes;
using System.Collections.Generic;
using System.Linq;

namespace Mahamudra.Contemporary.BenchMark
{
    public class PollyTestService
    {
        private readonly IEnumerable<NumberOfPrimes> _primes;
        private readonly ParallelFactory _parallelFactory;
        private readonly IHttpErrorService _httpService;
        public PollyTestService(IHttpErrorService httpService)
        {
            using var loggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
            var logger = loggerFactory.CreateLogger<ParallelFactory>();
 
            _primes = PrimeFactory.FirstNIntegers(10).Select(x => new NumberOfPrimes(x)); // find all first 10 integer numbers
            _parallelFactory = new ParallelFactory(logger);
            _httpService = httpService;
        }

        public void ExecuteParallelResult()
        {
            _parallelFactory.ExecuteAsyncResult<BaseEntity<int>, int>(_primes, _httpService.Get);
        }

        public void ExecuteParallelResultWillFail()
        {
            var fakes = new NumberOfPrimes[] { new NumberOfPrimes(0) };
            _parallelFactory.ExecuteAsyncResult<BaseEntity<int>, int>(fakes, _httpService.Get);
        }
    }
}
