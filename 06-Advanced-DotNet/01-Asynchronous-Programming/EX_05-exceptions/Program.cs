using System;
using System.Threading.Tasks;

namespace EX_04_async_await
{
    internal class Program
    {
        static Task Main(string[] args)
        {
            bool showMenu = true;
            while (showMenu)
            {
                showMenu = MainMenuAsync().Result;
            }

            return Task.CompletedTask;
        }

        private static async Task<bool> MainMenuAsync()
        {
            Console.Clear();
            Console.WriteLine("Choose an option:");
            Console.WriteLine("1) await in try");
            Console.WriteLine("2) тип void, виняток назовні не передається");
            Console.WriteLine("3) тип void, обробка винятку в самому асинхронному методі");
            Console.WriteLine("4) IsFaulted");
            Console.WriteLine("5) Обробка декількох винятків. WhenAll");
            Console.WriteLine("6) Отримання результату 02");
            Console.WriteLine("0) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    try
                    {
                        await Print1Async("Hello ZZZ.COM");
                        await Print1Async("Hi");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    async Task Print1Async(string message)
                    {
                        // якщо довжина рядка менше 3 символів, генеруємо виняток
                        if (message.Length < 3)
                            throw new ArgumentException($"Invalid string length: {message.Length}");
                        await Task.Delay(1000);     // імітація тривалої операції
                        Console.WriteLine(message);
                    }
                    Console.ReadKey();
                    return true;

                case "2":
                    try
                    {
                        Print2Async("Hello METANIT.COM");
                        Print2Async("Hi");       // тут програма згенерує виняток і аварійно зупиниться
                        await Task.Delay(1000); // чекаємо завершення задач
                    }
                    catch (Exception ex)    // виняток НЕ буде оброблено
                    {
                        Console.WriteLine(ex.Message);
                    }

                    async void Print2Async(string message)
                    {
                        // якщо довжина рядка менше 3 символів, генеруємо виняток
                        if (message.Length < 3)
                            throw new ArgumentException($"Invalid string length: {message.Length}");
                        await Task.Delay(100);     // імітація тривалої операції
                        Console.WriteLine(message);
                    }

                    Console.ReadKey();
                    return true;

                case "3":
                    Print3Async("Hello METANIT.COM");
                    Print3Async("Hi");
                    await Task.Delay(1000); // чекаємо завершення задач

                    async void Print3Async(string message)
                    {
                        try
                        {
                            // якщо довжина рядка менше 3 символів, генеруємо виняток
                            if (message.Length < 3)
                                throw new ArgumentException($"Invalid string length: {message.Length}");
                            await Task.Delay(100);     // імітація тривалої операції
                            Console.WriteLine(message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    Console.ReadKey();
                    return true;
                case "4":
                    var task = Print4Async("Hi");
                    try
                    {
                        await task;
                    }
                    catch
                    {
                        Console.WriteLine(task.Exception?.InnerException?.Message); // Invalid string length: 2
                        Console.WriteLine($"IsFaulted: {task.IsFaulted}");  // IsFaulted: True
                        Console.WriteLine($"Status: {task.Status}");        // Status: Faulted
                    }

                    async Task Print4Async(string message)
                    {
                        // якщо довжина рядка менше 3 символів, генеруємо виняток
                        if (message.Length < 3)
                            throw new ArgumentException($"Invalid string length: {message.Length}");
                        await Task.Delay(1000);     // імітація тривалої операції
                        Console.WriteLine(message);
                    }
                    Console.ReadKey();
                    return true;

                case "5":
                    // визначаємо і запускаємо задачі
                    var task1 = Print5Async("H");
                    var task2 = Print5Async("Hi");
                    var allTasks = Task.WhenAll(task1, task2);
                    try
                    {
                        await allTasks;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Exception: {ex.Message}");
                        Console.WriteLine($"IsFaulted: {allTasks.IsFaulted}");
                        if (allTasks.Exception is not null)
                        {
                            foreach (var exception in allTasks.Exception.InnerExceptions)
                            {
                                Console.WriteLine($"InnerException: {exception.Message}");
                            }
                        }
                    }

                    async Task Print5Async(string message)
                    {
                        // якщо довжина рядка менше 3 символів, генеруємо виняток
                        if (message.Length < 3)
                            throw new ArgumentException($"Invalid string: {message}");
                        await Task.Delay(1000);     // імітація тривалої операції
                        Console.WriteLine(message);
                    }

                    Console.ReadKey();
                    return true;
                case "0":
                    return false;
                default:
                    return true;
            }
        }

        // --- 01 ---
        // --- 02 ---
        // --- 03 ---
        // --- 04 ---
        // --- 05 ---
        // --- 06 ---
    }
}