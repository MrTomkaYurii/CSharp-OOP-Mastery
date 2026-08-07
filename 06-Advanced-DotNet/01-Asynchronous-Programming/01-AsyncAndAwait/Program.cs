using System;
using System.Threading;
namespace AsynchronousProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Main Method Started......");

            SomeMethod();

            Console.WriteLine("Main Method End");
            Console.ReadKey();
        }

        public static void SomeMethod()
        {
            Console.WriteLine("Some Method Started......");

            Thread.Sleep(TimeSpan.FromSeconds(10));
            Console.WriteLine("\n");
            Console.WriteLine("Some Method End");
        }
    }
}