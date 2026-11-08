using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AsynchronousProgramming
{
    /// <summary>
    /// =========================================================================================
    /// МНОЖИННІ ЗАДАЧІ: Розвантаження викликаючого потоку (Thread Offloading через Task.Run)
    /// =========================================================================================
    /// Демонструє запуск великої кількості асинхронних операцій (100,000 карток).
    /// Пояснює різницю між синхронним створенням 100 тисяч об'єктів Task у головному потоці
    /// та винесенням циклу планування в фоновий потік ThreadPool через Task.Run.
    /// </summary>
    internal class Program
    {
        // =====================================================================================
        // БЛОК 1: Точка входу в програму (Замір часу роботи Main)
        // =====================================================================================
        static void Main(string[] args)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Console.WriteLine("Main Thread Started");

            // Генерація великого пулу даних (100,000 карток)
            List<CreditCard> creditCards = CreditCard.GenerateCreditCards(100000);
            Console.WriteLine($"Credit Card Generated : {creditCards.Count}");

            // Запуск асинхронної обробки карток
            ProcessCreditCards(creditCards);

            Console.WriteLine("Main Thread Completed");
            Console.WriteLine($"Main Thread Execution Time {stopwatch.ElapsedMilliseconds / 1000.0} Seconds");
            Console.ReadKey();
        }

        // =====================================================================================
        // БЛОК 2: Асинхронний метод планування пулу задач
        // Створює список задач та очікує їх паралельне завершення через Task.WhenAll.
        // =====================================================================================
        public static async void ProcessCreditCards(List<CreditCard> creditCards)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var tasks = new List<Task<string>>();

            // ВАРІАНТ А: Створення задач у поточному потоці
            // При дуже великих колекціях (100k+) створення об'єктів задач займає час процесора
            foreach (var creditCard in creditCards)
            {
                var response = ProcessCard(creditCard);
                tasks.Add(response);
            }

            // ВАРІАНТ Б (Альтернатива): Розвантаження головного потоку через Task.Run:
            // await Task.Run(() =>
            // {
            //     foreach (var creditCard in creditCards)
            //     {
            //         tasks.Add(ProcessCard(creditCard));
            //     }
            // });

            // Очікування завершення всіх задач у паралельному режимі
            await Task.WhenAll(tasks);
            stopwatch.Stop();
            Console.WriteLine($"Processing of {creditCards.Count} Credit Cards Done in {stopwatch.ElapsedMilliseconds / 1000.0} Seconds");
        }

        // =====================================================================================
        // БЛОК 3: Імітація обробки однієї кредитної картки (I/O затримка)
        // =====================================================================================
        public static async Task<string> ProcessCard(CreditCard creditCard)
        {
            await Task.Delay(1000);
            string message = $"Credit Card Number: {creditCard.CardNumber} Name: {creditCard.Name} Processed";
            return message;
        }
    }

    // =========================================================================================
    // БЛОК 4: Доменна сутність та генератор тестових даних
    // =========================================================================================
    public class CreditCard
    {
        public string CardNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public static List<CreditCard> GenerateCreditCards(int number)
        {
            List<CreditCard> creditCards = new List<CreditCard>(number);
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
