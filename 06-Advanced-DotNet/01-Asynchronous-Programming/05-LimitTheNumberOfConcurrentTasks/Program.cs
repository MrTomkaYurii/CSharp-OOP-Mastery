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
    /// Безконтрольний запуск тисяч паралельних задач через Task.WhenAll може покласти базу даних
    /// або вичерпати сокети. SemaphoreSlim(3) виступає як "турнікет", що дозволяє виконувати
    /// одночасно не більше 3 задач, ставлячи решту в неблокуючу асинхронну чергу.
    /// </summary>
    internal class Program
    {
        // =====================================================================================
        // БЛОК 1: Семафор обмеження конкурентності
        // Початкова кількість дозволів: 3 (максимум 3 задачі одночасно)
        // =====================================================================================
        private static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(initialCount: 3);

        static void Main(string[] args)
        {
            Console.WriteLine("Main Thread Started");

            // Генерація 15 карток для обробки
            List<CreditCard> creditCards = CreditCard.GenerateCreditCards(15);
            Console.WriteLine($"Credit Card Generated : {creditCards.Count}");

            // Запуск троттлінгу карток
            ProcessCreditCards(creditCards);

            Console.WriteLine("Main Thread Completed");
            Console.ReadKey();
        }

        // =====================================================================================
        // БЛОК 2: Асинхронний конвеєр із контрольованим шлюзом
        // Кожна лямбда чекає своєї черги через await semaphoreSlim.WaitAsync().
        // У блоці finally ОБОВ'ЯЗКОВО викликається semaphoreSlim.Release().
        // =====================================================================================
        public static async void ProcessCreditCards(List<CreditCard> creditCards)
        {
            var stopwatch = Stopwatch.StartNew();

            // Створення списку завдань із контролем доступу
            var tasks = creditCards.Select(async card =>
            {
                // Неблокуюче очікування вільного слота в семафорі
                await semaphoreSlim.WaitAsync();

                try
                {
                    // Одночасно тут перебуває щонайбільше 3 потоки/задачі
                    return await ProcessCard(card);
                }
                finally
                {
                    // Звільнення слота для наступної задачі в черзі
                    semaphoreSlim.Release();
                }
            }).ToList();

            // Очікування завершення всіх 15 операцій
            await Task.WhenAll(tasks);
            stopwatch.Stop();

            Console.WriteLine($"\nProcessing of {creditCards.Count} Credit Cards Done in {stopwatch.ElapsedMilliseconds / 1000.0} Seconds");
        }

        // =====================================================================================
        // БЛОК 3: Імітація обробки картки (тривалість 1 сек)
        // При 15 картках і ліміті 3 одночасних, загальний час: (15 / 3) * 1с = ~5 секунд.
        // =====================================================================================
        public static async Task<string> ProcessCard(CreditCard creditCard)
        {
            await Task.Delay(1000);
            string message = $"Credit Card Number: {creditCard.CardNumber} Processed";
            Console.WriteLine($"[Thread {Thread.CurrentThread.ManagedThreadId}] {message}");
            return message;
        }
    }

    // =========================================================================================
    // БЛОК 4: Допоміжний клас моделі даних
    // =========================================================================================
    public class CreditCard
    {
        public string CardNumber { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;

        public static List<CreditCard> GenerateCreditCards(int number)
        {
            var creditCards = new List<CreditCard>(number);
            for (int i = 0; i < number; i++)
            {
                creditCards.Add(new CreditCard
                {
                    CardNumber = "10000000" + i,
                    Name = "CreditCard-" + i
                });
            }
            return creditCards;
        }
    }
}
