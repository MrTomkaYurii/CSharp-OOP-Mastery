using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EX_02_async_await
{
    internal class Program
    {
        internal class Bacon { }
        internal class Coffee { }
        internal class Egg { }
        internal class Juice { }
        internal class Toast { }


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
            Console.WriteLine("1) Синхронне приготування обіду");
            Console.WriteLine("2) Асинхронне приготування обіду");
            Console.WriteLine("0) Exit");
            Console.Write("\r\nSelect an option: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("================================");
                    Console.WriteLine("Синхронний сніданок");
                    Console.WriteLine("================================");

                    Coffee cup = PourCoffee();
                    Console.WriteLine("coffee is ready");

                    Egg eggs = FryEggs(2);
                    Console.WriteLine("eggs are ready");

                    Bacon bacon = FryBacon(3);
                    Console.WriteLine("bacon is ready");

                    Toast toast = ToastBread(2);
                    ApplyButter(toast);
                    ApplyJam(toast);
                    Console.WriteLine("toast is ready");

                    Juice oj = PourOJ();
                    Console.WriteLine("oj is ready");
                    Console.WriteLine("Breakfast is ready!");

                    Console.ReadKey();
                    return true;


                case "2":
                    Coffee cofee_cup = PourCoffeeSync();
                    Console.WriteLine("coffee is ready");

                    var eggsTask = FryEggsAsync(2);
                    var baconTask = FryBaconAsync(3);
                    var toastTask = MakeToastWithButterAndJamAsync(2);

                    var breakfastTasks = new List<Task> { eggsTask, baconTask, toastTask };
                    while (breakfastTasks.Count > 0)
                    {
                        Task finishedTask = await Task.WhenAny(breakfastTasks);
                        if (finishedTask == eggsTask)
                        {
                            Console.WriteLine("eggs are ready");
                        }
                        else if (finishedTask == baconTask)
                        {
                            Console.WriteLine("bacon is ready");
                        }
                        else if (finishedTask == toastTask)
                        {
                            Console.WriteLine("toast is ready");
                        }
                        breakfastTasks.Remove(finishedTask);
                    }
                    Juice juice_oj = PourOJSync();
                    Console.WriteLine("oj is ready");
                    Console.WriteLine("Breakfast is ready!");

                    Console.ReadKey();
                    return true;
                case "0":
                    return false;
                default:
                    return true;
            }
        }

        // --- 01 --- СИНХРОННА РЕАЛІЗАЦІЯ
        private static Juice PourOJ()
        {
            Console.WriteLine("Pouring orange juice");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) =>
            Console.WriteLine("Putting jam on the toast");

        private static void ApplyButter(Toast toast) =>
            Console.WriteLine("Putting butter on the toast");

        private static Toast ToastBread(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start toasting...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        private static Bacon FryBacon(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            Task.Delay(3000).Wait();
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        private static Egg FryEggs(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            Task.Delay(3000).Wait();
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }

        private static Coffee PourCoffee()
        {
            Console.WriteLine("Pouring coffee");
            return new Coffee();
        }



        // --- 02 --- АСИНХРОННА РЕАЛІЗАЦІЯ
        static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
        {
            var toast = await ToastBreadAsync(number);
            ApplyButterSync(toast);
            ApplyJamSync(toast);

            return toast;
        }

        private static Juice PourOJSync()
        {
            Console.WriteLine("Pouring orange juice");
            return new Juice();
        }

        private static void ApplyJamSync(Toast toast) =>
            Console.WriteLine("Putting jam on the toast");

        private static void ApplyButterSync(Toast toast) =>
            Console.WriteLine("Putting butter on the toast");

        private static async Task<Toast> ToastBreadAsync(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start toasting...");
            await Task.Delay(3000);
            Console.WriteLine("Remove toast from toaster");

            return new Toast();
        }

        private static async Task<Bacon> FryBaconAsync(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            await Task.Delay(3000);
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            await Task.Delay(3000);
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        private static async Task<Egg> FryEggsAsync(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            await Task.Delay(3000);
            Console.WriteLine($"cracking {howMany} eggs");
            Console.WriteLine("cooking the eggs ...");
            await Task.Delay(3000);
            Console.WriteLine("Put eggs on plate");

            return new Egg();
        }

        private static Coffee PourCoffeeSync()
        {
            Console.WriteLine("Pouring coffee");
            return new Coffee();
        }
    }
}