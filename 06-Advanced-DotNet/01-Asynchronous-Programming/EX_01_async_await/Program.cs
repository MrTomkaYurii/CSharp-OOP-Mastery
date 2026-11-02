using System;
using System.Threading;
using System.Threading.Tasks;

namespace EX_01_async_await
{
    /// <summary>
    /// =========================================================================================
    /// ЕКСПЕРИМЕНТ 1: Порівняльний аналіз синхронних та асинхронних патернів у C#
    /// =========================================================================================
    /// Інтерактивна лабораторія, що дозволяє протестувати в реальному часі:
    /// 1. Перетворення синхронного коду на асинхронний через Task.Run.
    /// 2. Різницю між блокуючим Thread.Sleep та неблокуючим Task.Delay.
    /// 3. Послідовне синхронне виконання (Tom, Bob, Sam = 3 сек) vs конкурентний запуск (~1 сек).
    /// 4. Обчислювальні CPU-Bound задачі з асинхронними лямбда-виразами.
    /// </summary>
    internal class Program
    {
        // =====================================================================================
        // БЛОК 1: Консольне меню та цикл вибору демонстрацій
        // =====================================================================================
        static async Task Main(string[] args)
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = await MainMenuAsync();
            }
        }

        private static async Task<bool> MainMenuAsync()
        {
            Console.Clear();
            Console.WriteLine("======================================================");
            Console.WriteLine(" ЕКСПЕРИМЕНТ 1: Базові механізми async / await");
            Console.WriteLine("======================================================");
            Console.WriteLine("1) Асинхронні методи async та await (Task.Run)");
            Console.WriteLine("2) Затримка асинхронної операції Task.Delay");
            Console.WriteLine("3) Переваги асинхронності: СИНХРОННЕ блокування");
            Console.WriteLine("4) Переваги асинхронності: АСИНХРОННИЙ паралелізм");
            Console.WriteLine("5) Лямбда при визначенні CPU-Bound операції");
            Console.WriteLine("0) Вихід");
            Console.Write("\r\nОберіть опцію: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("\n[1] Виклик методу через Task.Run у фоновому потоці:");
                    await PrintAsync_01();
                    return true;

                case "2":
                    Console.WriteLine("\n[2] Неблокуюча пауза через Task.Delay:");
                    await PrintAsync_02();
                    return true;

                case "3":
                    Console.WriteLine("\n[3] Синхронний виклик: по черзі, кожен блокує на 1 сек (разом 3 сек)...");
                    PrintName("Tom");
                    PrintName("Bob");
                    PrintName("Sam");
                    Console.WriteLine("Натисніть будь-яку клавішу...");
                    Console.ReadKey();
                    return true;

                case "4":
                    Console.WriteLine("\n[4] Асинхронний виклик: всі три стартують одночасно (разом ~1 сек)...");
                    var tomTask = PrintNameAsync("Tom");
                    var bobTask = PrintNameAsync("Bob");
                    var samTask = PrintNameAsync("Sam");

                    await tomTask;
                    await bobTask;
                    await samTask;
                    Console.WriteLine("Натисніть будь-яку клавішу...");
                    Console.ReadKey();
                    return true;

                case "5":
                    Console.WriteLine("\n[5] CPU-Bound обчислення в пулі потоків через Task<int>.Run:");
                    var t = await Task.Run(() =>
                    {
                        int max = 1_000_000;
                        int ctr = 0;
                        for (ctr = 0; ctr <= max; ctr++)
                        {
                            if (ctr == max / 2 && DateTime.Now.Hour <= 24)
                            {
                                ctr++;
                                break;
                            }
                        }
                        return ctr;
                    });
                    Console.WriteLine("Finished {0:N0} iterations.", t);
                    Console.ReadKey();
                    return true;

                case "0":
                    return false;

                default:
                    return true;
            }
        }

        // =====================================================================================
        // БЛОК 2: Демонстраційні методи
        // =====================================================================================

        // Синхронний метод із важким блокуванням
        private static void Print()
        {
            Thread.Sleep(3000);
            Console.WriteLine("Hello Guys!");
        }

        // Асинхронна обгортка через Task.Run для винесення синхронного коду в ThreadPool
        private static async Task PrintAsync_01()
        {
            Console.WriteLine("Початок методу PrintAsync");
            await Task.Run(() => Print());
            Console.WriteLine("Кінець методу PrintAsync");
            Console.ReadKey();
        }

        // Чистий асинхронний I/O метод без блокування потоку
        private static async Task PrintAsync_02()
        {
            await Task.Delay(3000);
            Console.WriteLine("Hello GUYS (виконано після Task.Delay)");
            Console.ReadKey();
        }

        // Синхронний вивід із блокуванням потоку
        private static void PrintName(string name)
        {
            Thread.Sleep(1000);
            Console.WriteLine($"[Синхронно]: {name}");
        }

        // Асинхронний вивід з очікуванням таймера без блокування
        private static async Task PrintNameAsync(string name)
        {
            await Task.Delay(1000);
            Console.WriteLine($"[Асинхронно]: {name}");
        }
    }
}
