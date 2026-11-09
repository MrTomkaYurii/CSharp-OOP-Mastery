using System;
using System.Threading.Tasks;

namespace EX_05_exceptions
{
    /// <summary>
    /// =========================================================================================
    /// ЕКСПЕРИМЕНТ 5: Обробка винятків в асинхронному коді (Exception Handling in TAP)
    /// =========================================================================================
    /// Комплексна лабораторія аналізу поведінки помилок в асинхронному середовищі:
    /// 1. Класичний try/catch навколо await: розгортання першого винятку.
    /// 2. Пастка async void: виняток НЕ перехоплюється викликаючим кодом і викликає аварію процесу.
    /// 3. Локальна обробка всередині async void: єдиний захист від аварійного завершення.
    /// 4. Перевірка статусу задачі: task.IsFaulted, task.Status, task.Exception.
    /// 5. Агрегація множинних винятків у Task.WhenAll: отримання всіх InnerExceptions.
    /// 
    /// ТЕОРЕТИЧНИЙ АСПЕКТ: ЧОМУ AGGREGATEEXCEPTION ЧАСТО ВВАЖАЮТЬ АНТИПАТЕРНОМ?
    /// 1. Поява AggregateException у прикладному коді — головний маркер блокуючого коду
    ///    "Sync-over-Async" (виклики .Result, .Wait(), task.GetAwaiter().GetResult()).
    /// 2. Три проблеми AggregateException як "матрьошки":
    ///    - Приховує справжню причину: замість конкретного ArgumentException чи HttpRequestException
    ///      летить загальна обгортка, яку доводиться розгортати вручну через ex.Flatten().
    ///    - Ламає стандартні блоки catch: блок 'catch (ArgumentException)' ніколи не спрацює,
    ///      бо виняток упакований в AggregateException.
    ///    - Спотворює стек викликів (Stack Trace), вказуючи на рядок із .Wait(), а не на місце аварії.
    /// 3. Як 'await' вирішує цю проблему:
    ///    - Компілятор Roslyn при очікуванні через 'await' АВТОМАТИЧНО розпаковує перше виключення
    ///      і кидає його в первинному вигляді, дозволяючи використовувати звичайні типізовані catch!
    /// 4. Де AggregateException законний:
    ///    - У паралельних комбінаторах (Task.WhenAll, PLINQ), коли ОДНОЧАСНО впали кілька незалежних задач.
    /// </summary>
    internal class Program
    {
        // =====================================================================================
        // БЛОК 1: Консольне меню та маршрутизація сценаріїв
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
            Console.WriteLine(" ЕКСПЕРИМЕНТ 5: Обробка винятків в асинхронному середовищі");
            Console.WriteLine("=============================================================");
            Console.WriteLine("1) await у блоці try/catch (розгортання винятку)");
            Console.WriteLine("2) async void: виняток назовні НЕ передається (Process Crash)");
            Console.WriteLine("3) async void: внутрішня обробка винятку всередині методу");
            Console.WriteLine("4) Дослідження об'єкта Task: IsFaulted, Status, InnerException");
            Console.WriteLine("5) Обробка декількох паралельних винятків у Task.WhenAll");
            Console.WriteLine("0) Вихід");
            Console.Write("\r\nОберіть опцію: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("\n[1] Нормальний перехоплювач try/catch для Task:");
                    try
                    {
                        await Print1Async("Hello SUPERCSHARP.COM");
                        await Print1Async("Hi"); // Тут генерується помилка, бо довжина < 3
                    }
                    catch (ArgumentException ex)
                    {
                        Console.WriteLine($"[ПЕРЕХОПЛЕНО]: {ex.Message}");
                    }

                    Console.ReadKey();
                    return true;

                case "2":
                    Console.WriteLine("\n[2] ДЕМОНСТРАЦІЯ АВАРІЇ: виклик async void з помилкою:");
                    try
                    {
                        Print2Async("Hello");
                        // УВАГА: Цей виклик кине виняток у потік синхронізації,
                        // зовнішній catch НЕ зможе його перехопити!
                        Print2Async("Hi");
                        await Task.Delay(500);
                    }
                    catch (Exception ex)
                    {
                        // Сюди керування НІКОЛИ не потрапить!
                        Console.WriteLine($"Цей блок ніколи не виконається: {ex.Message}");
                    }

                    Console.ReadKey();
                    return true;

                case "3":
                    Console.WriteLine("\n[3] Безпечний async void: try/catch всередині самого методу:");
                    Print3Async("Hello");
                    Print3Async("Hi"); // Помилка обробляється локально
                    await Task.Delay(500);

                    Console.ReadKey();
                    return true;

                case "4":
                    Console.WriteLine("\n[4] Дослідження стану Faulted у Task:");
                    var task = Print4Async("Hi");
                    try
                    {
                        await task;
                    }
                    catch
                    {
                        // Дослідження властивостей завершеної з помилкою задачі
                        Console.WriteLine($"Повідомлення: {task.Exception?.InnerException?.Message}");
                        Console.WriteLine($"Властивість task.IsFaulted: {task.IsFaulted}");
                        Console.WriteLine($"Властивість task.Status: {task.Status}");
                    }

                    Console.ReadKey();
                    return true;

                case "5":
                    Console.WriteLine("\n[5] Множинні винятки у Task.WhenAll:");
                    // Запуск двох задач, обидві з яких викинуть помилку
                    var task1 = Print5Async("A");
                    var task2 = Print5Async("B");
                    var allTasks = Task.WhenAll(task1, task2);

                    try
                    {
                        await allTasks;
                    }
                    catch (Exception ex)
                    {
                        // 'await' розпакував лише ПЕРШИЙ виняток
                        Console.WriteLine($"[await розпакував першу помилку]: {ex.Message}");
                        Console.WriteLine($"allTasks.IsFaulted = {allTasks.IsFaulted}");

                        // Але в колекції allTasks.Exception збережено ВСІ помилки!
                        if (allTasks.Exception is not null)
                        {
                            Console.WriteLine("\n--- Повний список InnerExceptions з AggregateException: ---");
                            foreach (var inner in allTasks.Exception.InnerExceptions)
                            {
                                Console.WriteLine($"-> {inner.Message}");
                            }
                        }
                    }

                    Console.ReadKey();
                    return true;

                case "0":
                    return false;

                default:
                    return true;
            }
        }

        // =====================================================================================
        // БЛОК 2: Допоміжні асинхронні методи для кейсів винятків
        // =====================================================================================

        // Кейс 1: Стандартний async Task
        private static async Task Print1Async(string message)
        {
            if (message.Length < 3)
                throw new ArgumentException($"Invalid string length: {message.Length}");

            await Task.Delay(500);
            Console.WriteLine(message);
        }

        // Кейс 2: Небезпечний async void без внутрішньої обробки
        private static async void Print2Async(string message)
        {
            if (message.Length < 3)
                throw new ArgumentException($"[Async Void Error]: Довжина {message.Length} < 3");

            await Task.Delay(100);
            Console.WriteLine(message);
        }

        // Кейс 3: Захищений async void із внутрішнім try/catch
        private static async void Print3Async(string message)
        {
            try
            {
                if (message.Length < 3)
                    throw new ArgumentException($"[Handled Void Error]: Довжина {message.Length} < 3");

                await Task.Delay(100);
                Console.WriteLine(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Локально перехоплено у async void]: {ex.Message}");
            }
        }

        // Кейс 4: async Task для аналізу IsFaulted
        private static async Task Print4Async(string message)
        {
            if (message.Length < 3)
                throw new ArgumentException($"Invalid string length: {message.Length}");

            await Task.Delay(500);
            Console.WriteLine(message);
        }

        // Кейс 5: Метод для паралельних винятків у Task.WhenAll
        private static async Task Print5Async(string message)
        {
            if (message.Length < 3)
                throw new ArgumentException($"Помилка валідації для рядка '{message}' (довжина {message.Length} < 3)");

            await Task.Delay(500);
            Console.WriteLine(message);
        }
    }
}
