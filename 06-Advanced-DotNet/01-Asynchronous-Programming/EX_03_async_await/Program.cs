using System;
using System.Threading.Tasks;

namespace EX_03_async_await
{
    /// <summary>
    /// Допоміжний клас рахунку з подією нарахування коштів
    /// Використовується для демонстрації єдиного легітимного сценарію async void — асинхронного обробника події.
    /// </summary>
    class Account
    {
        private int sum = 0;
        public event EventHandler<string>? Added;

        public void Put(int sum)
        {
            this.sum += sum;
            Added?.Invoke(this, $"На рахунок поступило {sum} $");
        }
    }

    /// <summary>
    /// =========================================================================================
    /// ЕКСПЕРИМЕНТ 3: Типи значень, що повертаються з асинхронних методів (Return Types)
    /// =========================================================================================
    /// Демонструє 4 типи сигнатур асинхронних методів у сучасному .NET:
    /// 1. async void: небезпечний Fire-and-Forget (винятки не перехоплюються).
    /// 2. async void у подіях (Event Handlers): єдиний виправданий випадок async void у C#.
    /// 3. async Task: операція без значення (аналог синхронного void), але дозволяє await.
    /// 4. async Task<T>: операція з результатом типу T.
    /// 5. ValueTask<T>: структура без виділення пам'яті в купі (Zero Allocation) для швидких результатів.
    /// </summary>
    internal class Program
    {
        // =====================================================================================
        // БЛОК 1: Консольне меню перевірки типів повернення
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
            Console.WriteLine(" ЕКСПЕРИМЕНТ 3: Типи повернення в асинхронних методах");
            Console.WriteLine("=============================================================");
            Console.WriteLine("1) async void: Fire-and-Forget");
            Console.WriteLine("2) async void: Обробник подій (Event Handler)");
            Console.WriteLine("3) async Task: Операція без повернення значення");
            Console.WriteLine("4) async Task<T>: Операція з поверненням результату T");
            Console.WriteLine("5) ValueTask<T>: Оптимізована структура (Zero Allocation)");
            Console.WriteLine("0) Вихід");
            Console.Write("\r\nОберіть опцію: ");

            switch (Console.ReadLine())
            {
                case "1":
                    Console.WriteLine("\n[1] Запуск async void (Main не чекає на завершення):");
                    PrintAsync("Hello World!!!");
                    PrintAsync("Hello GUYS!!!");
                    Console.WriteLine("Main продовжує роботу негайно...");
                    await Task.Delay(3000);
                    Console.ReadKey();
                    return true;

                case "2":
                    Console.WriteLine("\n[2] Легітимне використання async void: підписка на подію:");
                    Account account = new Account();
                    account.Added += PrintAsyncEvent;
                    account.Put(500);
                    await Task.Delay(2000);
                    Console.ReadKey();
                    return true;

                case "3":
                    Console.WriteLine("\n[3] async Task: Метод очікується через await:");
                    await HelloAsync("Hello Guys!");
                    Console.WriteLine("Main продовжив роботу ПІСЛЯ завершення HelloAsync");
                    Console.ReadKey();
                    return true;

                case "4":
                    Console.WriteLine("\n[4] async Task<T>: Конкурентний розрахунок квадратів:");
                    var square5 = SquareAsync(5);
                    var square6 = SquareAsync(6);

                    Console.WriteLine("Паралельні дії в Main до очікування результатів...");
                    int n1 = await square5;
                    int n2 = await square6;
                    Console.WriteLine($"Результати: n1={n1}, n2={n2}");
                    Console.ReadKey();
                    return true;

                case "5":
                    Console.WriteLine("\n[5] ValueTask<T>: Миттєве повернення без алокації в Heap:");
                    var result = await AddAsync(4, 5);
                    Console.WriteLine($"Сума: {result}");
                    Console.ReadKey();
                    return true;

                case "0":
                    return false;

                default:
                    return true;
            }
        }

        // =====================================================================================
        // БЛОК 2: Реалізація методів із різними типами повернення
        // =====================================================================================

        // 1. async void: Викликаючий код не може написати await
        private async static void PrintAsync(string message)
        {
            await Task.Delay(2000);
            Console.WriteLine($"[async void]: {message}");
        }

        // 2. async void як обробник події: відповідає сигнатурі EventHandler
        private async static void PrintAsyncEvent(object? sender, string message)
        {
            await Task.Delay(1000);
            Console.WriteLine($"[Event]: {message}");
        }

        // 3. async Task: Дозволяє await та перехоплення винятків
        private async static Task HelloAsync(string message)
        {
            await Task.Delay(1000);
            Console.WriteLine($"[async Task]: {message}");
        }

        // 4. async Task<T>: Повертає значення типу int
        private async static Task<int> SquareAsync(int n)
        {
            await Task.Delay(1000);
            var result = n * n;
            Console.WriteLine($"Квадрат числа {n} дорівнює {result}");
            return result;
        }

        // 5. ValueTask<T>: Створюється на стеку без аллокації Task у Heap, якщо результат готовий синхронно
        private static ValueTask<int> AddAsync(int a, int b)
        {
            return new ValueTask<int>(a + b);
        }
    }
}
