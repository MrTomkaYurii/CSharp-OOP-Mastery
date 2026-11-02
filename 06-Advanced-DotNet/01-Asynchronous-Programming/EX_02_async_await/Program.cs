using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EX_02_async_await
{
    /// <summary>
    /// =========================================================================================
    /// ЕКСПЕРИМЕНТ 2: Метафора «Асинхронний сніданок» (Синхронно vs Task.WhenAny)
    /// =========================================================================================
    /// Хрестоматійний приклад Microsoft Docs. Пояснює різницю між кухарем, який чекає кожну
    /// страву послідовно (сніданок охолоне і готуватиметься 30 хвилин), та асинхронним шефом,
    /// який одночасно ставить каву, смажить яйця, бекон і тости, обробляючи кожну страву
    /// в міру її готовності через комбінатор Task.WhenAny.
    /// </summary>
    internal class Program
    {
        // Доменні сутності страв сніданку
        internal class Bacon { }
        internal class Coffee { }
        internal class Egg { }
        internal class Juice { }
        internal class Toast { }

        // =====================================================================================
        // БЛОК 1: Консольне меню перемикання режимів
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
            Console.WriteLine(" ЕКСПЕРИМЕНТ 2: Асинхронний сніданок (Task.WhenAny)");
            Console.WriteLine("=============================================================");
            Console.WriteLine("1) Синхронне приготування сніданку (послідовне блокування)");
            Console.WriteLine("2) Асинхронне приготування сніданку (обробка по мірі готовності)");
            Console.WriteLine("0) Вихід");
            Console.Write("\r\nОберіть опцію: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("\n--- Синхронний сніданок (Кожна дія чекає попередню) ---");
                    Coffee cup = PourCoffee();
                    Console.WriteLine("1. Кава налита");

                    Egg eggs = FryEggs(2);
                    Console.WriteLine("2. Яєчня готова");

                    Bacon bacon = FryBacon(3);
                    Console.WriteLine("3. Бекон посмажений");

                    Toast toast = ToastBread(2);
                    ApplyButter(toast);
                    ApplyJam(toast);
                    Console.WriteLine("4. Тости з маслом і джемом готові");

                    Juice oj = PourOJ();
                    Console.WriteLine("5. Сік налитий");
                    Console.WriteLine("\n Сніданок повністю готовий!");

                    Console.ReadKey();
                    return true;

                case "2":
                    Console.WriteLine("\n--- Асинхронний сніданок (Паралельне приготування) ---");
                    Coffee coffeeCup = PourCoffeeSync();
                    Console.WriteLine("1. Каву налито (синхронна миттєва дія)");

                    // Одночасний запуск усіх теплових процесів (без негайного await!)
                    var eggsTask = FryEggsAsync(2);
                    var baconTask = FryBaconAsync(3);
                    var toastTask = MakeToastWithButterAndJamAsync(2);

                    // Реєстрація активних процесів у списку
                    var breakfastTasks = new List<Task> { eggsTask, baconTask, toastTask };

                    // Обробка страв у міру їх готовності за допомогою Task.WhenAny
                    while (breakfastTasks.Count > 0)
                    {
                        Task finishedTask = await Task.WhenAny(breakfastTasks);
                        if (finishedTask == eggsTask)
                        {
                            Console.WriteLine("-> Яєчня посмажилась!");
                        }
                        else if (finishedTask == baconTask)
                        {
                            Console.WriteLine("-> Бекон хрусткий і готовий!");
                        }
                        else if (finishedTask == toastTask)
                        {
                            Console.WriteLine("-> Тости підсмажені та намащені!");
                        }
                        breakfastTasks.Remove(finishedTask);
                    }

                    Juice juiceOj = PourOJSync();
                    Console.WriteLine("-> Апельсиновий сік налито");
                    Console.WriteLine("\n Всі страви гарячі та готові одночасно!");

                    Console.ReadKey();
                    return true;

                case "0":
                    return false;

                default:
                    return true;
            }
        }

        // =====================================================================================
        // БЛОК 2: Синхронні методи з блокуванням потоку через Thread.Sleep
        // =====================================================================================
        private static Juice PourOJ()
        {
            Console.WriteLine("Наливаємо апельсиновий сік...");
            return new Juice();
        }

        private static void ApplyJam(Toast toast) =>
            Console.WriteLine("Намащуємо джем на тост...");

        private static void ApplyButter(Toast toast) =>
            Console.WriteLine("Намащуємо масло на тост...");

        private static Toast ToastBread(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Кладемо скибку хліба в тостер...");
            }
            Console.WriteLine("Починаємо підсмажування тостів...");
            Thread.Sleep(3000);
            Console.WriteLine("Дістаємо підсмажені тости з тостера...");
            return new Toast();
        }

        private static Bacon FryBacon(int slices)
        {
            Console.WriteLine($"Кладемо {slices} скибок бекону на сковорідку...");
            Console.WriteLine("Смажимо перший бік бекону...");
            Thread.Sleep(3000);
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Перевертаємо скибку бекону...");
            }
            Console.WriteLine("Смажимо другий бік бекону...");
            Thread.Sleep(3000);
            Console.WriteLine("Викладаємо бекон на тарілку...");
            return new Bacon();
        }

        private static Egg FryEggs(int howMany)
        {
            Console.WriteLine("Розігріваємо сковорідку...");
            Thread.Sleep(3000);
            Console.WriteLine($"Розбиваємо {howMany} яєць на сковорідку...");
            Console.WriteLine("Смажимо яйця...");
            Thread.Sleep(3000);
            Console.WriteLine("Викладаємо яєчню на тарілку...");
            return new Egg();
        }

        private static Coffee PourCoffee()
        {
            Console.WriteLine("Наливаємо свіжу каву у чашку...");
            return new Coffee();
        }

        // =====================================================================================
        // БЛОК 3: Асинхронні операції приготування (Неблокуючі, Task.Delay)
        // =====================================================================================
        private static async Task<Toast> MakeToastWithButterAndJamAsync(int number)
        {
            var plainToast = await ToastBreadAsync(number);
            ApplyButter(plainToast);
            ApplyJam(plainToast);
            return plainToast;
        }

        private static async Task<Toast> ToastBreadAsync(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Кладемо скибку хліба в тостер...");
            }
            Console.WriteLine("Початок підсмажування тостів...");
            await Task.Delay(3000);
            Console.WriteLine("Тости вискочили з тостера!");
            return new Toast();
        }

        private static async Task<Bacon> FryBaconAsync(int slices)
        {
            Console.WriteLine($"Кладемо {slices} скибок бекону на сковорідку...");
            Console.WriteLine("Смажимо перший бік бекону...");
            await Task.Delay(3000);
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Перевертаємо скибку бекону...");
            }
            Console.WriteLine("Смажимо другий бік бекону...");
            await Task.Delay(3000);
            Console.WriteLine("Бекон готовий!");
            return new Bacon();
        }

        private static async Task<Egg> FryEggsAsync(int howMany)
        {
            Console.WriteLine("Розігріваємо сковорідку для яєчні...");
            await Task.Delay(3000);
            Console.WriteLine($"Розбиваємо {howMany} яєць...");
            Console.WriteLine("Яйця смажаться...");
            await Task.Delay(3000);
            Console.WriteLine("Яєчня готова!");
            return new Egg();
        }

        private static Coffee PourCoffeeSync()
        {
            Console.WriteLine("Наливаємо гарячу каву...");
            return new Coffee();
        }

        private static Juice PourOJSync()
        {
            Console.WriteLine("Наливаємо склянку апельсинового соку...");
            return new Juice();
        }
    }
}
