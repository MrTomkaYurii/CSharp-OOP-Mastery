using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ex_03_Yield
{
    internal class RandomNumberGenerator
    {
        private readonly Random random;
        public RandomNumberGenerator()
        {
            random = new Random();
        }

        public IEnumerable<int> GenerateRandomNumbers(int min, int max, int number)
        {
            for (int i = 1; i <= number; i++)
            {
                //Generate a random number between the min and max value.
                int randomNumber = random.Next(min, max);

                //return each generated number one at a time using yield.
                yield return randomNumber;
            }
        }

        public IEnumerable<int> GenerateRandomNumbersUntil(int min, int max, int match)
        {
            while (true)
            {
                //Generate a random number between the min and max value.
                int randomNumber = random.Next(min, max);

                //Numbers will keep getting generated and returned until 
                if (randomNumber == match)
                    yield break; //Stop the iteration when we get to the desired number.
                else
                    yield return randomNumber; //return each generated number one at a time using yield.
            }
        }

    }
}
