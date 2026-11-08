using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AsynchronousProgramming
{
    /// <summary>
    /// =========================================================================================
    /// Task.WhenAll: Паралельне очікування пачки завдань
    /// =========================================================================================
    /// Показує, як передати список Task у Task.WhenAll та отримати зведений масив результатів.
    /// </summary>
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Запуск демонстрації Task.WhenAll...");

            List<CreditCard> creditCards = CreditCard.GenerateCreditCards(15);
            Console.WriteLine($"Підготовлено {creditCards.Count} карток для транзакцій.\n");

            await ProcessCreditCardsAsync(creditCards);

            stopwatch.Stop();
            Console.WriteLine($"Загальний час виконання програми: {stopwatch.Elapsed.TotalSeconds:F2} сек.");
        }

        public static async Task ProcessCreditCardsAsync(List<CreditCard> creditCards)
        {
            var tasks = new List<Task<string>>();
            foreach (var card in creditCards)
            {
                tasks.Add(ProcessCardAsync(card));
            }

            // Task.WhenAll чекає завершення всіх задач у паралельному режимі
            string[] responses = await Task.WhenAll(tasks);

            Console.WriteLine($"Успішно отримано {responses.Length} відповідей:");
            foreach (var r in responses)
            {
                Console.WriteLine($"  * {r}");
            }
        }

        public static async Task<string> ProcessCardAsync(CreditCard creditCard)
        {
            await Task.Delay(250);
            return $"Картка {creditCard.CardNumber} авторизована";
        }
    }

    public class CreditCard
    {
        public string CardNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public static List<CreditCard> GenerateCreditCards(int count)
        {
            var list = new List<CreditCard>(count);
            for (int i = 1; i <= count; i++)
            {
                list.Add(new CreditCard
                {
                    CardNumber = $"5375-1000-{i:D4}",
                    Name = $"Користувач-{i}"
                });
            }
            return list;
        }
    }
}
