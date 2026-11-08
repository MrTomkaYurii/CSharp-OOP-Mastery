using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace AsynchronousProgramming
{
    /// <summary>
    /// =========================================================================================
    /// АГРЕГАЦІЯ ВІДПОВІДЕЙ МНОЖИННИХ ЗАДАЧ: Збір, десеріалізація та фільтрація результатів
    /// =========================================================================================
    /// Демонструє повний виробничий цикл:
    /// 1. Троттлінг паралельних запитів (SemaphoreSlim)
    /// 2. Одночасне очікування всіх відповідей (Task.WhenAll)
    /// 3. Десеріалізація відповідей із JSON у типізовані об'єкти
    /// 4. Пост-обробка, групування та бізнес-фільтрація (Approved vs Rejected)
    /// </summary>
    internal class Program
    {
        // =====================================================================================
        // БЛОК 1: Обмежувач конкурентності (максимум 3 задачі одночасно)
        // =====================================================================================
        private static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(3);

        static void Main(string[] args)
        {
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("Main Thread Started");

            // Генерація 15 карток для обробки
            List<CreditCard> creditCards = CreditCard.GenerateCreditCards(15);
            Console.WriteLine($"Credit Card Generated : {creditCards.Count}");

            // Запуск обробки та агрегації відповідей
            ProcessCreditCards(creditCards);

            Console.WriteLine("Main Thread Completed");
            Console.ReadKey();
        }

        // =====================================================================================
        // БЛОК 2: Асинхронний конвеєр збору та обробки відповідей
        // =====================================================================================
        public static async void ProcessCreditCards(List<CreditCard> creditCards)
        {
            var stopwatch = Stopwatch.StartNew();

            // 1. Формування пулу задач із контролем доступу через семафор
            var tasks = creditCards.Select(async card =>
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

            // 2. Очікування всіх відповідей (повертає масив JSON-рядків string[])
            string[] rawResponses = await Task.WhenAll(tasks);

            // 3. Десеріалізація отриманих JSON-відповідей у типізовані об'єкти DTO
            var creditCardResponses = new List<CreditCardResponse>();
            foreach (var responseJson in rawResponses)
            {
                if (responseJson != null)
                {
                    CreditCardResponse? parsedResponse = JsonConvert.DeserializeObject<CreditCardResponse>(responseJson);
                    if (parsedResponse != null)
                    {
                        creditCardResponses.Add(parsedResponse);
                    }
                }
            }

            // 4. Бізнес-аналітика та фільтрація результатів (LINQ)
            Console.WriteLine("\n=== Схвалені картки (Approved) ===");
            foreach (var item in creditCardResponses.Where(c => c.IsProcessed))
            {
                Console.WriteLine($"Card: {item.CardNumber}, Name: {item.Name}");
            }

            Console.WriteLine("\n=== Відхилені картки (Rejected) ===");
            foreach (var item in creditCardResponses.Where(c => !c.IsProcessed))
            {
                Console.WriteLine($"Card: {item.CardNumber}, Name: {item.Name}");
            }

            stopwatch.Stop();
            Console.WriteLine($"\nОбробку завершено за {stopwatch.ElapsedMilliseconds / 1000.0} сек");
        }

        // =====================================================================================
        // БЛОК 3: Імітація обробки картки банківським шлюзом
        // Повертає результат у форматі JSON
        // =====================================================================================
        public static async Task<string> ProcessCard(CreditCard creditCard)
        {
            await Task.Delay(1000);

            var creditCardResponse = new CreditCardResponse
            {
                CardNumber = creditCard.CardNumber,
                Name = creditCard.Name,
                // Парні номери схвалюються, непарні - відхиляються
                IsProcessed = creditCard.CardNumber % 2 == 0
            };

            return JsonConvert.SerializeObject(creditCardResponse);
        }
    }

    // =========================================================================================
    // БЛОК 4: Моделі даних запиту та відповіді
    // =========================================================================================
    public class CreditCard
    {
        public long CardNumber { get; set; }
        public string Name { get; set; } = string.Empty;

        public static List<CreditCard> GenerateCreditCards(int number)
        {
            var creditCards = new List<CreditCard>(number);
            for (int i = 0; i < number; i++)
            {
                creditCards.Add(new CreditCard
                {
                    CardNumber = 10000000 + i,
                    Name = "CreditCard-" + i
                });
            }
            return creditCards;
        }
    }

    public class CreditCardResponse
    {
        public long CardNumber { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsProcessed { get; set; }
    }
}
