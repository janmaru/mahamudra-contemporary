using Mahamudra.Contemporary;
using Mahamudra.Core.Entity;
using Mahamudra.Result.Core.Patterns;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Primes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestsContemporary
{
    [TestClass]
    public class UnitTestParallelAsyncFactory
    {
        private ParallelAsyncFactory _p;
        private IEnumerable<BaseEntity<int>> _primes;
        [TestInitialize]
        public void Init()
        {
            _p = new ParallelAsyncFactory(new NullLogger<ParallelFactory>());
            _primes = PrimeFactory.PrimeNumbersLessEqualToN(10).Select(x => new NumberOfPrimes(x)); // find all primes less or equal to 10 --> 2,3,5,7
        }

        private Task<int> SumAsync(BaseEntity<int> number)
        {
            return Task.FromResult(PrimeFactory.PrimeNumbersLessEqualToN(number.Id).Sum());
        }

        private int Sum(BaseEntity<int> number)
        {
            return PrimeFactory.PrimeNumbersLessEqualToN(number.Id).Sum();
        }

        private Task<int> SumAsyncWithException(BaseEntity<int> number)
        {
            return Task.FromResult(PrimeFactory.PrimeNumbersLessEqualToNThrowsExceptionOn5(number.Id).Sum());
        }

        private int SumWithException(BaseEntity<int> number)
        {
            return PrimeFactory.PrimeNumbersLessEqualToNThrowsExceptionOn5(number.Id).Sum();
        }

        [TestMethod]
        public async Task ExecuteAsync_ShouldComputeSumOfFirstPrimesLessEqualToTen_True()
        {
            var list = await _p.ExecuteAsync<BaseEntity<int>, int>(_primes, SumAsync);
            Assert.AreEqual(list.Sum(x => x.Value), 2 + 5 + 10 + 17);
        }
 
        [TestMethod]
        public async Task ExecuteAsyncResult_ShouldComputeSumOfFirstPrimesLessEqualToTenWithError_True()
        {
            var list = await _p.ExecuteAsyncResult<BaseEntity<int>, int>(_primes, SumAsyncWithException);

            var successList = list.Where(x => x.Key is Success<BaseEntity<int>, string>);
            Assert.AreEqual(successList.Sum(x => x.Value), 2 + 5 + 17);
            var failureList = list.Where(x => x.Key is Failure<BaseEntity<int>, string>);
            Assert.AreEqual(failureList.Count(), 1);
        } 
       
    }
}
