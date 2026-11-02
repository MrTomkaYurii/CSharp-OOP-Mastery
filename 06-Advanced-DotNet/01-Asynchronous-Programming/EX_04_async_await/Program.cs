using System;
using System.Threading.Tasks;

namespace EX_04_async_await
{
    /// <summary>
    /// =========================================================================================
    /// ЕКСПЕРИМЕНТ 4: Комбінатори задач Task.WhenAll та Task.WhenAny
    /// =========================================================================================
    /// Лабораторне дослідження методів синхронізації групи задач:
    /// 1. Послідовний await (блокує запуск наступних завдань).
    /// 2. Недетермінований паралельний вивід (старт усіх задач одночасно).
    /// 3. Task.WhenAll: спільне очікування всіх задач до завершення останньої.
    /// 4. Task.WhenAny: реакція на першу виконану задачу (гонка операцій / Racing Pattern).
    /// 5. Агрегація масиву результатів через Task.WhenAll(task1, task2...).
    /// 6. Безпечний доступ до task.Result після завершення Task.WhenAll.
    /// </summary>
    internal class Program
    {
        // =====================================================================================
        // БЛОК 1: Консольне меню та обробник демонстраційних кейсів
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
            Console.WriteLine("=============================================================");
            Console.WriteLine(" ЕКСПЕРИМЕНТ 4: Комбінатори Task.WhenAll та Task.WhenAny");
            Console.WriteLine("=============================================================");
            Console.WriteLine("1) Послідовне очікування оператором await (3 * 2с = 6с)");
            Console.WriteLine("2) Недетермінований конкурентний запуск (~2с)");
            Console.WriteLine("3) Синхронізація групи через Task.WhenAll");
            Console.WriteLine("4) Реакція на першу завершену задачу через Task.WhenAny");
            Console.WriteLine("5) Отримання масиву результатів T[] із Task.WhenAll");
            Console.WriteLine("6) Доступ до окремих результатів після Task.WhenAll");
            Console.WriteLine("0) Вихід");
            Console.Write("\r\nОберіть опцію: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("\n[1] Послідовне виконання (кожне повідомлення чекає 2 сек):");
                    await PrintAsync("Hello C#");
                    await PrintAsync("Hello World");
                    await PrintAsync("Hello GUYS!");
                    Console.ReadKey();
                    return true;

                case "2":
                    Console.WriteLine("\n[2] Запуск усіх 3 задач одночасно (недетермінований порядок):");
                    var task1 = PrintAsync("Hello C#");
                    var task2 = PrintAsync("Hello World");
                    var task3 = PrintAsync("Hello Guys!");

                    await task1;
                    await task2;
                    await task3;
                    Console.ReadKey();
                    return true;

                case "3":
                    Console.WriteLine("\n[3] Синхронізація через Task.WhenAll:");
                    var task01 = PrintAsync("Hello C#");
                    var task02 = PrintAsync("Hello World");
                    var task03 = PrintAsync("Hello ZZZ.COM");

                    // Чекаємо завершення ВСІХ трьох задач
                    await Task.WhenAll(task01, task02, task03);
                    Console.WriteLine("Усі три задачі завершились!");
                    Console.ReadKey();
                    return true;

                case "4":
                    Console.WriteLine("\n[4] Гонка задач через Task.WhenAny (рандомний час 1-2 сек):");
                    var task001 = PrintAsync_04("Задача 1");
                    var task002 = PrintAsync_04("Задача 2");
                    var task003 = PrintAsync_04("Задача 3");

                    // Очікуємо найпершу виконану задачу
                    Task fastestTask = await Task.WhenAny(task001, task002, task003);
                    Console.WriteLine("Перша задача дісталася фінішу!");
                    Console.ReadKey();
                    return true;

                case "5":
                    Console.WriteLine("\n[5] Отримання масиву обчислених результатів:");
                    var taskx = SquareAsync(4);
                    var tasky = SquareAsync(5);
                    var taskz = SquareAsync(6);

                    // WhenAll автоматично пакує результати у масив int[]
                    int[] results = await Task.WhenAll(taskx, tasky, taskz);
                    foreach (int result in results)
                    {
                        Console.WriteLine($"Результат із масиву: {result}");
                    }
                    Console.ReadKey();
                    return true;

                case "6":
                    Console.WriteLine("\n[6] Читання індивідуального результату з конкретної задачі:");
                    var task0001 = SquareAsync(4);
                    var task0002 = SquareAsync(5);
                    var task0003 = SquareAsync(6);

                    await Task.WhenAll(task0001, task0002, task0003);

                    // Тепер безпечно читати .Result, бо задача 100% завершена (RanToCompletion)
                    Console.WriteLine($"Окремий результат task0002: {task0002.Result}");
                    Console.ReadKey();
                    return true;

                case "0":
                    return false;

                default:
                    return true;
            }
        }

        // =====================================================================================
        // БЛОК 2: Допоміжні асинхронні задачі
        // =====================================================================================
        private static async Task PrintAsync(string message)
        {
            await Task.Delay(2000);
            Console.WriteLine(message);
        }

        private static async Task PrintAsync_04(string message)
        {
            int delay = Random.Shared.Next(1000, 2000);
            await Task.Delay(delay);
            Console.WriteLine($"{message} (час затримки: {delay} мс)");
        }

        private static async Task<int> SquareAsync(int n)
        {
            await Task.Delay(1000);
            return n * n;
        }
    }
}
