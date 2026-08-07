using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;

namespace AsynchronousProgramming
{
    class Program
    {
        //Allowing Maximum 3 tasks to be executed at a time
        static SemaphoreSlim semaphoreSlim = new SemaphoreSlim(3);
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            Console.WriteLine($"Main Thread Started");

            //Generating 15 Credit Cards
            List<CreditCard> creditCards = CreditCard.GenerateCreditCards(15);
            Console.WriteLine($"Credit Card Generated : {creditCards.Count}");

            ProcessCreditCards(creditCards);

            Console.WriteLine($"Main Thread Completed");
            Console.ReadKey();
        }

        public static async void ProcessCreditCards(List<CreditCard> creditCards)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var tasks = new List<Task<string>>();

            //Need to use async lambda expression
            tasks = creditCards.Select(async card =>
            {
                await semaphoreSlim.WaitAsync();

                try
                {
                    return await ProcessCard(card);
                }
                finally
                {
                    semaphoreSlim.Release();
                }

            }).ToList();

            //Return the response a string array
            var Responses = await Task.WhenAll(tasks);

            //Creating a collection to hold the responses
            List<CreditCardResponse> creditCardResponses = new List<CreditCardResponse>();

            //Looping through the string array
            foreach (var response in Responses)
            {
                //Here, the string is a JSON string
                //Converting the JSON String to .NET Object (CreditCardResponse) using
                //JsonConvert class DeserializeObject
                CreditCardResponse creditCardResponse = JsonConvert.DeserializeObject<CreditCardResponse>(response);

                //Adding the .NET Object into the resposne collection
                creditCardResponses.Add(creditCardResponse);
            }

            //Printing all the approved credit cards using a foreach loop
            Console.WriteLine("\nApproved Credit Cards");
            foreach (var item in creditCardResponses.Where(card => card.IsProcessed == true))
            {
                Console.WriteLine($"Card Number: {item.CardNumber}, Name: {item.Name}");
            }

            //Printing all the rejected credit cards using a foreach loop
            Console.WriteLine("\nRejected Credit Cards");
            foreach (var item in creditCardResponses.Where(card => card.IsProcessed == false))
            {
                Console.WriteLine($"Card Number: {item.CardNumber}, Name: {item.Name}");
            }
        }

        public static async Task<string> ProcessCard(CreditCard creditCard)
        {
            await Task.Delay(1000);

            var creditCardResponse = new CreditCardResponse
            {
                CardNumber = creditCard.CardNumber,
                Name = creditCard.Name,

                //Logic to Decide whether the card is processed or rejected
                //If modulus 2 is 0, the processed else rejected
                IsProcessed = creditCard.CardNumber % 2 == 0 ? true : false
            };

            //Converting the .NET Object to JSON string
            string jsonString = JsonConvert.SerializeObject(creditCardResponse);

            //Return the JSON String
            return jsonString;
        }
    }

    public class CreditCard
    {
        public long CardNumber { get; set; }
        public string Name { get; set; }

        public static List<CreditCard> GenerateCreditCards(int number)
        {
            List<CreditCard> creditCards = new List<CreditCard>();
            for (int i = 0; i < number; i++)
            {
                CreditCard card = new CreditCard()
                {
                    CardNumber = 10000000 + i,
                    Name = "CreditCard-" + i
                };

                creditCards.Add(card);
            }

            return creditCards;
        }
    }

    //This class will hold the response after processing the Credit card
    public class CreditCardResponse
    {
        public long CardNumber { get; set; }
        public string Name { get; set; }
        public bool IsProcessed { get; set; }
    }
}