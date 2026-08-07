using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AsynchronousProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Main Thread Started");

            List<CreditCard> creditCards = CreditCard.GenerateCreditCards(10);
            Console.WriteLine($"Credit Card Generated : {creditCards.Count}");

            ProcessCreditCards(creditCards);

            Console.WriteLine($"Main Thread Completed");
            Console.ReadKey();
        }

        //Execution without Task.WhenAll Method in C#
        public static async void ProcessCreditCards(List<CreditCard> creditCards)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            foreach (var creditCard in creditCards)
            {
                var response = await ProcessCard(creditCard);
            }

            stopwatch.Stop();
            Console.WriteLine($"Processing of {creditCards.Count} Credit Cards Done in {stopwatch.ElapsedMilliseconds / 1000.0} Seconds");
        }

        //Execution with Task.WhenAll Method in C#
        //public static async void ProcessCreditCards(List<CreditCard> creditCards)
        //{
        //    var stopwatch = new Stopwatch();
        //    stopwatch.Start();
        //    var tasks = new List<Task<string>>();

        //    //Processing the creditCards using foreach loop
        //    foreach (var creditCard in creditCards)
        //    {
        //        var response = ProcessCard(creditCard);
        //        tasks.Add(response);
        //    }

        //    //It will execute all the tasks concurrently
        //    await Task.WhenAll(tasks);
        //    stopwatch.Stop();
        //    Console.WriteLine($"Processing of {creditCards.Count} Credit Cards Done in {stopwatch.ElapsedMilliseconds / 1000.0} Seconds");
        //    //foreach(var item in tasks)
        //    //{
        //    //    Console.WriteLine(item.Result);
        //    //}
        //}

        public static async Task<string> ProcessCard(CreditCard creditCard)
        {
            //Here we can do any API Call to Process the Credit Card
            //But for simplicity we are just delaying the execution for 1 second
            await Task.Delay(1000);
            string message = $"Credit Card Number: {creditCard.CardNumber} Name: {creditCard.Name} Processed";
            Console.WriteLine($"Credit Card Number: {creditCard.CardNumber} Processed");
            return message;
        }
    }

    public class CreditCard
    {
        public string CardNumber { get; set; }
        public string Name { get; set; }

        public static List<CreditCard> GenerateCreditCards(int number)
        {
            List<CreditCard> creditCards = new List<CreditCard>();
            for (int i = 0; i < number; i++)
            {
                CreditCard card = new CreditCard()
                {
                    CardNumber = "10000000" + i,
                    Name = "CreditCard-" + i
                };

                creditCards.Add(card);
            }

            return creditCards;
        }
    }
}