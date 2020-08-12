using Mahamudra.Contemporary;
using Mahamudra.Patterns;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Primes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnitTestsContemporary
{
    [TestClass]
    public class UnitTestOfP
    {
        private P _p;
        private IEnumerable<BaseEntity<int>> _primes;
        [TestInitialize]
        public void Init()
        {
            _p = new P(new NullLogger<P>());
            _primes = PrimeFactory.PrimeNumbersLessEqualToN(10).Select(x => new NumberOfPrimes(x)); // find all primes less or equal to 10 --> 2,3,5,7
        }

        private Task<int> Sum(BaseEntity<int> number)
        {
            return Task.FromResult(PrimeFactory.PrimeNumbersLessEqualToN(number.Id).Sum());
        }

        [TestMethod]
        public void Execute_ShouldComputeFirstPrimes_True()
        {
            var list = _p.Execute<BaseEntity<int>, int>(_primes, Sum);
            Assert.AreEqual(list.Sum(x => x.Value), 2 + 5 + 10 + 17);
        }

    }
}
