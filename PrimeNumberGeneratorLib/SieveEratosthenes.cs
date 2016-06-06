using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrimeNumberGeneratorLib
{
    /// <summary>
    /// Prime number generator that implements Sieve of Eratosthenes algorithm.
    /// </summary>
    public class SieveEratosthenes
    {
        /// <summary>
        /// Generates all prime numbers under a given limit.
        /// </summary>
        /// <param name="limit">The upper limit for prime number generator. </param>
        /// <returns>An <see cref="IEnumerable{int}"/> of prime numbers</returns>
        public IEnumerable<int> GetPrimeNumbers(int limit)
        {
            int sieveLimit = (int)Math.Sqrt(limit);
            List<int> primeNums = new List<int>();
            BitArray primeBitArr = new BitArray(limit + 1, true);
            primeBitArr[0] = false;
            primeBitArr[1] = false;

            for(int i = 2; i <= sieveLimit; i++)
            {
                if (primeBitArr[i])
                {
                    primeNums.Add(i);
                    for (int j = i * i; j <= limit; j+= i)
                    {
                        primeBitArr[j] = false;
                    }
                }
            }
            
            for (int i = sieveLimit + 1; i < primeBitArr.Count; i++)
            {
                if (primeBitArr[i])
                {
                    primeNums.Add(i);
                }
            }
            return primeNums;
        }
    }
}
