using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AsynchronousProgramming
{
    /// <summary>
    /// =========================================================================================
    /// МНОЖИННІ ЗАДАЧІ: Паралельне виконання пулу завдань через Task.WhenAll
    /// =========================================================================================
    /// Демонструє створення невеликого пулу карток (20 шт) та їх одночасну асинхронну обробку.
    /// Пояснює, чому Task.WhenAll чекає завершення всіх задач паралельно, а не послідовно.
    /// </summary>
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Main Thread розпочато...");

            // Генеруємо 20 карток (наочна кількість для студентів)
            int cardCount = 20;
            List<CreditCard> creditCards = CreditCard.GenerateCreditCards(cardCount);
            Console.WriteLine($"Згенеровано {creditCards.Count} карток для перевірки.\n");

            await ProcessCreditCardsAsync(creditCards);

            stopwatch.Stop();
            Console.WriteLine($"\nMain Thread завершено за {stopwatch.Elapsed.TotalSeconds:F2} сек.");
        }

        public static async Task ProcessCreditCardsAsync(List<CreditCard> creditCards)
        {
            var stopwatch = Stopwatch.StartNew();
            var tasks = new List<Task<string>>();

            foreach (var creditCard in creditCards)
            {
                tasks.Add(ProcessCardAsync(creditCard));
            }

            // Очікування завершення всіх 20 задач ОДНОЧАСНО:
            string[] results = await Task.WhenAll(tasks);

            stopwatch.Stop();
            Console.WriteLine($"Обробку {results.Length} карток виконано за {stopwatch.Elapsed.TotalSeconds:F2} сек.");
            Console.WriteLine($"(Якби обробка була синхронною, це зайняло б {creditCards.Count * 0.2:F1} сек!)");
        }

        public static async Task<string> ProcessCardAsync(CreditCard creditCard)
        {
            // Імітація звернення до банку (200 мс)
            await Task.Delay(200);
            return $"Картка #{creditCard.CardNumber} ({creditCard.Name}) успішно перевірена.";
        }
    }

    public class CreditCard
    {
        public string CardNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public static List<CreditCard> GenerateCreditCards(int number)
        {
            var list = new List<CreditCard>(number);
            for (int i = 1; i <= number; i++)
            {
                list.Add(new CreditCard
                {
                    CardNumber = $"4149-0000-{i:D4}",
                    Name = $"Клієнт #{i}"
                });
            }
            return list;
        }
    }
}
