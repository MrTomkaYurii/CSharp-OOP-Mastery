using System;
using System.Threading;
using System.Threading.Tasks;

namespace EX_01_async_await
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
            Console.WriteLine("1) Асинхронні методи async та await");
            Console.WriteLine("2) Затримка асинхронної операції Task.Delay");
            Console.WriteLine("3) Переваги асинхронності - СИНХРОННО");
            Console.WriteLine("4) Переваги асинхронності - АСИНХРОННО");
            Console.WriteLine("5) Лямбда при визначенні асинхронної операції");
            Console.WriteLine("0) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("================================");
                    Console.WriteLine("Дії в методі Main");
                    Console.WriteLine("================================");
                    await PrintAsync_01();
                    return true;
                case "2":
                    Console.WriteLine("================================");
                    Console.WriteLine("Дії в методі Main");
                    Console.WriteLine("================================");
                    await PrintAsync_02();
                    return true;
                case "3":
                    PrintName("Tom");
                    PrintName("Bob");
                    PrintName("Sam");
                    Console.ReadKey();
                    return true;
                case "4":
                    var tomTask = PrintNameAsync("Tom");
                    var bobTask = PrintNameAsync("Bob");
                    var samTask = PrintNameAsync("Sam");

                    await tomTask;
                    await bobTask;
                    await samTask;
                    Console.ReadKey();
                    return true;
                case "5":
                    var t = Task<int>.Run(() => {
                        int max = 1000000;
                        int ctr = 0;
                        for (ctr = 0; ctr <= max; ctr++)
                        {
                            if (ctr == max / 2 && DateTime.Now.Hour <= 12)
                            {
                                ctr++;
                                break;
                            }
                        }
                        return ctr;
                    });
                    Console.WriteLine("Finished {0:N0} iterations.", t.Result);
                    Console.ReadKey();
                    return true;
                case "0":
                    return false;
                default:
                    return true;
            }
        }

        // --- 01 --- Асинхронні методи, async и await
        private static void Print()
        {
            Thread.Sleep(3000);     
            Console.WriteLine("Hello Guys!");
        }


        // визначення асинхронного метода
        private static async Task PrintAsync_01()
        {
            Console.WriteLine("Початок методу PrintAsync"); 
            await Task.Run(
                () => Print());
            Console.WriteLine("Кінець методу PrintAsync");
            Console.ReadKey();
        }



        // --- 02 --- Затримка асинхронної операції та Task.Delay
        private static async Task PrintAsync_02()
        {
            await Task.Delay(3000);     
                                        //або так
                                        //await Task.Delay(TimeSpan.FromMilliseconds(3000));
            Console.WriteLine("Hello GUYS");
            Console.ReadKey();
        }



        // ---03 --- Перевага асинхронности
        private static void PrintName(string name)
        {
            Thread.Sleep(1000);     
            Console.WriteLine(name);
        }
        // визначення асинхронного метода
        private static async Task PrintNameAsync(string name)
        {
            await Task.Delay(1000);     
            Console.WriteLine(name);
        }
    }
}
