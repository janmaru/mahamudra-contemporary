using Mahamudra.Patterns; 

namespace Primes
{
    public class NumberOfPrimes : BaseEntity<int>
    {
        public NumberOfPrimes(int id)
        {
            this.Id = id;
        }

        public override string ToString()
        {
            return this.Id.ToString();
        }
    }
}
