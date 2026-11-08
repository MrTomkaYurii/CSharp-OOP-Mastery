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
    /// АГРЕГАЦІЯ ВІДПОВІДЕЙ МНОЖИННИХ ЗАДАЧ: Збір, класифікація та звіт результатів
    /// =========================================================================================
    /// Демонструє повний цикл:
    /// 1. Троттлінг паралельних запитів (SemaphoreSlim)
    /// 2. Одночасне очікування всіх відповідей (Task.WhenAll)
    /// 3. Пост-обробка та групування результатів (успішні та відхилені платежі)
    /// </summary>
    internal class Program
    {
        private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(3);

        public static async Task Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Main розпочато. Запуск агрегації паралельних відповідей...\n");

            List<CreditCard> creditCards = CreditCard.GenerateCreditCards(10);
            Console.WriteLine($"Згенеровано {creditCards.Count} карток для транзакцій.\n");

            await ProcessCreditCardsAsync(creditCards);

            stopwatch.Stop();
            Console.WriteLine($"\nУвесь процес завершено за {stopwatch.Elapsed.TotalSeconds:F2} сек.");
        }

        public static async Task ProcessCreditCardsAsync(List<CreditCard> creditCards)
        {
            var tasks = creditCards.Select(async card =>
            {
                await _semaphore.WaitAsync();
                try
                {
                    return await ProcessCardAsync(card);
                }
                finally
                {
                    _semaphore.Release();
                }
            });

            PaymentResult[] responses = await Task.WhenAll(tasks);

            Console.WriteLine("\n--- ЗВЕДЕНИЙ ЗВІТ ОБРОБКИ ТРАНЗАКЦІЙ ---");
            var successful = responses.Where(r => r.IsSuccess).ToList();
            var failed = responses.Where(r => !r.IsSuccess).ToList();

            Console.WriteLine($"Успішно підтверджено: {successful.Count} операцій:");
            foreach (var item in successful)
            {
                Console.WriteLine($"  [Успіх] Картка: {item.CardNumber}, Код авторизації: {item.AuthCode}");
            }

            Console.WriteLine($"\nВідхилено банком: {failed.Count} операцій:");
            foreach (var item in failed)
            {
                Console.WriteLine($"  [Відхилено] Картка: {item.CardNumber}, Причина: {item.ErrorMessage}");
            }
        }

        public static async Task<PaymentResult> ProcessCardAsync(CreditCard card)
        {
            await Task.Delay(200);

            // Для наочності: картки з парним номером успішні, з непарним — відхилені
            if (card.Id % 3 == 0)
            {
                return new PaymentResult
                {
                    CardNumber = card.CardNumber,
                    IsSuccess = false,
                    ErrorMessage = "Недостатньо коштів на балансі"
                };
            }

            return new PaymentResult
            {
                CardNumber = card.CardNumber,
                IsSuccess = true,
                AuthCode = $"AUTH-{Guid.NewGuid().ToString()[..8].ToUpper()}"
            };
        }
    }

    public class PaymentResult
    {
        public string CardNumber { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
        public string AuthCode { get; set; } = string.Empty;
        public string ErrorMessage { get; set; } = string.Empty;
    }

    public class CreditCard
    {
        public int Id { get; set; }
        public string CardNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public static List<CreditCard> GenerateCreditCards(int count)
        {
            var list = new List<CreditCard>(count);
            for (int i = 1; i <= count; i++)
            {
                list.Add(new CreditCard
                {
                    Id = i,
                    CardNumber = $"5412-7500-{i:D4}",
                    Name = $"Клієнт-{i}"
                });
            }
            return list;
        }
    }
}
