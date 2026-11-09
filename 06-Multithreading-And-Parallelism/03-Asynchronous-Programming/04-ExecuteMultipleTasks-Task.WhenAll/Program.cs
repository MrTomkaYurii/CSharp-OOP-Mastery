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
    /// 
    /// ТЕОРЕТИЧНІ АСПЕКТИ: ПОСЛІДОВНИЙ AWAIT ПРОТИ TASK.WHENALL
    /// 1. Різниця в часі:
    ///    - Послідовний await у циклі foreach: 15 карток * 250 мс = ~3750 мс (~3.8 сек).
    ///    - Конкурентний Task.WhenAll: усі 15 карток стартують паралельно, сумарний час = ~250 мс (у 15 разів швидше!).
    /// 2. Як працює Task.WhenAll під капотом:
    ///    - Створює одну зведену задачу Task, яка переходить у статус RanToCompletion,
    ///      коли ВСІ дочірні задачі завершилися успішно.
    /// 3. Поведінка у разі винятків:
    ///    - Якщо кілька задач завершилися помилкою, оператор await розпакує і викине лише ПЕРШИЙ виняток.
    ///    - Повний перелік усіх помилок зберігається у властивості task.Exception.InnerExceptions (AggregateException).
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
                    CardNumber = $"CARD-{i:D3}",
                    Name = $"Клієнт #{i}"
                });
            }
            return list;
        }
    }
}
