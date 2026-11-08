using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronousProgramming
{
    /// <summary>
    /// =========================================================================================
    /// ТРОТТЛІНГ ТА КОНТРОЛЬ ПАРАЛЕЛІЗМУ: SemaphoreSlim для асинхронних операцій
    /// =========================================================================================
    /// SemaphoreSlim(3) виступає як "турнікет", що дозволяє виконувати одночасно не більше
    /// трьох задач, захищаючи зовнішній сервіс або базу даних від перевантаження.
    /// </summary>
    internal class Program
    {
        // Дозволяємо одночасно виконувати щонайбільше 3 асинхронні запити
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(initialCount: 3, maxCount: 3);

        public static async Task Main(string[] args)
        {
            Console.WriteLine("Main розпочато. Обмеження паралелізму: 3 одночасні задачі.\n");

            List<CreditCard> creditCards = CreditCard.GenerateCreditCards(12);
            Console.WriteLine($"Згенеровано {creditCards.Count} карток для пакетної обробки.\n");

            await ProcessCreditCardsAsync(creditCards);

            Console.WriteLine("\nMain успішно завершено.");
        }

        public static async Task ProcessCreditCardsAsync(List<CreditCard> creditCards)
        {
            var stopwatch = Stopwatch.StartNew();

            var tasks = creditCards.Select(async card =>
            {
                // Неблокуюче асинхронне очікування вільного слота турнікета:
                await _semaphore.WaitAsync();
                try
                {
                    return await ProcessCardAsync(card);
                }
                finally
                {
                    // Гарантоване звільнення слота для наступної задачі в черзі:
                    _semaphore.Release();
                }
            });

            string[] results = await Task.WhenAll(tasks);

            stopwatch.Stop();
            Console.WriteLine($"\nУсі {results.Length} карток оброблено за {stopwatch.Elapsed.TotalSeconds:F2} сек.");
        }

        public static async Task<string> ProcessCardAsync(CreditCard card)
        {
            Console.WriteLine($"  -> Початок обробки {card.CardNumber} (Потік #{Environment.CurrentManagedThreadId})");
            await Task.Delay(300); // Імітація роботи
            Console.WriteLine($"  <- Завершено обробку {card.CardNumber}");
            return $"OK: {card.CardNumber}";
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
                    CardNumber = $"CARD-{i:D3}",
                    Name = $"Клієнт #{i}"
                });
            }
            return list;
        }
    }
}
