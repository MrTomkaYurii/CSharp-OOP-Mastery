using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronousProgramming
{
    /// <summary>
    /// =========================================================================================
    /// КООПЕРАТИВНЕ СКАСУВАННЯ: CancellationTokenSource та CancellationToken
    /// =========================================================================================
    /// В .NET операції не «вбиваються» примусово (це викликало б витік пам'яті та блокування ресурсів).
    /// Замість цього застосовується модель двосторонньої згоди (кооперативне скасування):
    /// 1. Джерело (CancellationTokenSource) генерує сигнал скасування (наприклад, по таймауту або кнопці "Cancel").
    /// 2. Споживач (асинхронний метод) приймає CancellationToken і перевіряє стан через
    ///    token.ThrowIfCancellationRequested() або передає токен у вбудовані методи (наприклад, Task.Delay).
    /// </summary>
    internal class Program
    {
        // =====================================================================================
        // БЛОК 1: Точка входу в програму
        // =====================================================================================
        static void Main(string[] args)
        {
            SomeMethod();
            Console.ReadKey();
        }

        // =====================================================================================
        // БЛОК 2: Метод-ініціатор із таймером скасування
        // Створює CancellationTokenSource із часом життя 5 секунд.
        // Перехоплює OperationCanceledException / TaskCanceledException.
        // =====================================================================================
        private static async void SomeMethod()
        {
            int count = 10;
            Console.WriteLine("SomeMethod Started");

            // Налаштування таймауту скасування на 5000 мс (5 секунд)
            using var cancellationTokenSource = new CancellationTokenSource(5000);

            try
            {
                // Передача токена у тривалу операцію
                await LongRunningTask(count, cancellationTokenSource.Token);
            }
            catch (OperationCanceledException ex)
            {
                // Стандартне перехоплення скасування задачі
                Console.WriteLine($"\n[СКАСОВАНО]: Операцію перервано по таймауту. Повідомлення: {ex.Message}");
            }

            Console.WriteLine("SomeMethod Completed");
        }

        // =====================================================================================
        // БЛОК 3: Тривала фонова задача, чутлива до скасування
        // =====================================================================================
        public static async Task LongRunningTask(int count, CancellationToken token)
        {
            var stopwatch = Stopwatch.StartNew();
            Console.WriteLine("\nLongRunningTask Started");

            for (int i = 1; i <= count; i++)
            {
                // Передаємо токен у Task.Delay: якщо прийде скасування, очікування перерветься негайно
                await Task.Delay(1000, token);
                Console.WriteLine($"LongRunningTask Step {i}/{count} Processing....");

                // Ідіоматична перевірка: якщо надійшов запит на скасування,
                // цей виклик автоматично згенерує OperationCanceledException(token)
                token.ThrowIfCancellationRequested();
            }

            stopwatch.Stop();
            Console.WriteLine($"LongRunningTask Took {stopwatch.ElapsedMilliseconds / 1000.0} Seconds");
        }
    }
}
