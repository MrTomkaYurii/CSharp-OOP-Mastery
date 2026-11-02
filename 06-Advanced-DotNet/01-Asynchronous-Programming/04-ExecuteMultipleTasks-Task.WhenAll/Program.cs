using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace AsynchronousProgramming
{
    /// <summary>
    /// =========================================================================================
    /// КОМБІНАТОР TASK.WHENALL: Послідовне vs Паралельне асинхронне виконання
    /// =========================================================================================
    /// Демонструє кардинальну різницю у продуктивності:
    /// 1. Послідовний await у циклі: кожна ітерація чекає завершення попередньої (10 * 1с = 10 секунд).
    /// 2. Паралельний комбінатор Task.WhenAll: всі 10 задач запускаються одночасно (загальний час ~1 секунда).
    /// </summary>
    internal class Program
    {
        // =====================================================================================
        // БЛОК 1: Точка входу в програму
        // =====================================================================================
        static void Main(string[] args)
        {
            Console.WriteLine("Main Thread Started");

            // Генерація 10 карток для наочного порівняння часу виконання
            List<CreditCard> creditCards = CreditCard.GenerateCreditCards(10);
            Console.WriteLine($"Credit Card Generated : {creditCards.Count}");

            // Демонстрація виконання через Task.WhenAll
            ProcessCreditCardsWithWhenAll(creditCards);

            Console.WriteLine("Main Thread Completed");
            Console.ReadKey();
        }

        // =====================================================================================
        // БЛОК 2 (ВАРІАНТ 1): Послідовне очікування в циклі (Sequential Await)
        // АНТИПАТТЕРН для незалежних операцій: блокує прогрес наступної задачі до завершення поточної.
        // Час виконання: O(N * T), де N - кількість елементів, T - час однієї операції.
        // =====================================================================================
        public static async Task ProcessCreditCardsSequentially(List<CreditCard> creditCards)
        {
            var stopwatch = Stopwatch.StartNew();

            foreach (var creditCard in creditCards)
            {
                // Кожен await зупиняє метод на 1 секунду
                var response = await ProcessCard(creditCard);
            }

            stopwatch.Stop();
            Console.WriteLine($"[ПОСЛІДОВНО]: {creditCards.Count} карток оброблено за {stopwatch.ElapsedMilliseconds / 1000.0} секунд (Очікувано ~10 сек)");
        }

        // =====================================================================================
        // БЛОК 3 (ВАРІАНТ 2): Паралельний комбінатор Task.WhenAll
        // ЕТАЛОННИЙ ПАТЕРН: всі задачі стартують одночасно, WhenAll очікує завершення найповільнішої.
        // Час виконання: O(max(T)), тобто приблизно 1 секунда для всіх 10 задач!
        // =====================================================================================
        public static async void ProcessCreditCardsWithWhenAll(List<CreditCard> creditCards)
        {
            var stopwatch = Stopwatch.StartNew();
            var tasks = new List<Task<string>>();

            // Крок 1: Запуск усіх асинхронних операцій без негайного await
            foreach (var creditCard in creditCards)
            {
                var task = ProcessCard(creditCard);
                tasks.Add(task);
            }

            // Крок 2: Одночасне очікування завершення всієї групи задач
            string[] results = await Task.WhenAll(tasks);

            stopwatch.Stop();
            Console.WriteLine($"[ПАРАЛЕЛЬНО Task.WhenAll]: {creditCards.Count} карток оброблено за {stopwatch.ElapsedMilliseconds / 1000.0} секунд (Очікувано ~1 сек)");
        }

        // =====================================================================================
        // БЛОК 4: Імітація виклику API обробки картки (затримка 1 секунда)
        // =====================================================================================
        public static async Task<string> ProcessCard(CreditCard creditCard)
        {
            await Task.Delay(1000);
            string message = $"Credit Card Number: {creditCard.CardNumber} Name: {creditCard.Name} Processed";
            Console.WriteLine($"Card {creditCard.CardNumber} processed.");
            return message;
        }
    }

    // =========================================================================================
    // БЛОК 5: Допоміжний клас моделі даних
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
