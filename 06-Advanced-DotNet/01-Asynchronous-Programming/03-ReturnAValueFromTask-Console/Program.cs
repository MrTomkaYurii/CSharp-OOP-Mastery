using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AsynchronousProgramming
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Main Method Started......");
            Console.WriteLine("Enter the Name: ");
            string Name = Console.ReadLine();

            SomeMethod(Name);

            Console.WriteLine("Main Method End");
            Console.ReadKey();
        }

        public async static void SomeMethod(string Name)
        {
            Console.WriteLine("Some Method Started......");
            var GreetingSMessage = await Greetings(Name);
            Console.WriteLine($"\n{GreetingSMessage}");
            Console.WriteLine("Some Method End");
        }

        public static async Task<string> Greetings(string Name)
        {
            string message = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7118/");

                HttpResponseMessage response = await client.GetAsync($"api/greetings/{Name}");
                message = await response.Content.ReadAsStringAsync();
            }
            return message;
        }
    }
}