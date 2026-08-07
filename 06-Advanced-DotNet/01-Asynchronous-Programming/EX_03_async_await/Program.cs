using System;
using System.Threading.Tasks;

namespace EX_03_async_await
{
    class Account
    {
        int sum = 0;
        public event EventHandler<string>? Added;
        public void Put(int sum)
        {
            this.sum += sum;
            Added?.Invoke(this, $"На рахунок поступило {sum} $");
        }
    }


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
            Console.WriteLine("1) void");
            Console.WriteLine("2) void - обробка подій");
            Console.WriteLine("3) Task");
            Console.WriteLine("4) Task<T>");
            Console.WriteLine("5) ValueTask<T>");
            Console.WriteLine("0) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    PrintAsync("Hello World!!!");
                    PrintAsync("Hello GUYS!!!");
                    Console.WriteLine("Main End");
                    await Task.Delay(3000);
                    Console.ReadKey();
                    return true;

                case "2":
                    Account account = new Account();
                    account.Added += PrintAsync;
                    account.Put(500);
                    await Task.Delay(2000);
                    Console.ReadKey();
                    return true;

                case "3":
                    await HelloAsync("Hello Guys!");
                    Console.WriteLine("Main Works");
                    Console.ReadKey();
                    return true;

                case "4":
                    var square5 = SquareAsync(5);
                    var square6 = SquareAsync(6);

                    Console.WriteLine("Решта дій в методі Main");

                    int n1 = await square5;
                    int n2 = await square6;
                    Console.WriteLine($"n1={n1}  n2={n2}"); // n1=25  n2=36

                    Console.ReadKey();
                    return true;

                case "5":
                    var result = await AddAsync(4, 5);
                    Console.WriteLine(result);

                    Console.ReadKey();
                    return true;
                case "0":
                    return false;
                default:
                    return true;
            }
        }
        // --- 01 ---
        async static void PrintAsync(string message)
        {
            await Task.Delay(2000);     
            Console.WriteLine(message);
        }
        // --- 02 ---
        async static void PrintAsync(object? obj, string message)
        {
            await Task.Delay(1000);     
            Console.WriteLine(message);
        }

        // --- 03 ---
        async static Task HelloAsync(string message)
        {
            await Task.Delay(1000);     
            Console.WriteLine(message);
        }

        // --- 04 ---
        async static Task<int> SquareAsync(int n)
        {
            await Task.Delay(1000);
            var result = n * n;
            Console.WriteLine($"Квадрат числа {n} дорівнює {result}");
            return result;
        }

        // --- 05 ---
        static ValueTask<int> AddAsync(int a, int b)
        {
            return new ValueTask<int>(a + b);
        }
    }
}
