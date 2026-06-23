using System.Security.Cryptography;

namespace ex_03_Yield
{
    internal class Program
    {
        static void Main(string[] args)
        {
            foreach (var number in yieldDemo())
                Console.WriteLine(number);


            Console.WriteLine("--------");


            var rng = new RandomNumberGenerator();

            //Iterate over IEnumerable type.
            foreach (var number in rng.GenerateRandomNumbers(0, 10, 5))
                Console.WriteLine(number);


            Console.WriteLine("--------");


            //Keep iterating over the IEnumerable type until it manages to generate the number 15.
            foreach (var number in rng.GenerateRandomNumbersUntil(0, 10, 7))
                Console.WriteLine(number);


            Console.ReadLine();
        }

        public static IEnumerable<int> yieldDemo()
        {
            yield return 1;
            yield return 3;
            yield return 3;
            yield return 7;
        }


    }
}
