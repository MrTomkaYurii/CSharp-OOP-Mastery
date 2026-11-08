using System.Diagnostics;
using System.Text;

namespace MemoizationBasics;

/// <summary>
/// 01. Що таке Динамічне програмування "на пальцях"?
/// 
/// Уявіть, що вас запитали: "Скільки буде 1 + 1 + 1 + 1 + 1?"
/// Ви порахували на пальцях і відповіли: "5".
/// Тепер поруч дописали ще "+ 1" і питають: "А тепер скільки?"
/// Ви ж не рахуєте всі одинички спочатку! Ви просто пам'ятаєте, що було 5, і додаєте 1 -> "6".
/// 
/// Оце "запам'ятовування результатів попередніх обчислень, щоб не рахувати одне й те саме двічі"
/// і називається Динамічним програмуванням!
/// 
/// Підхід ЗГОРИ-ВНИЗ (Top-Down / Мемоізація) = Звичайна Рекурсія + "Блокнот" (Словник-кеш).
/// </summary>
internal class Program
{
    private static long _naiveStairsCalls = 0;
    private static long _memoStairsCalls = 0;

    private static long _naiveFibCalls = 0;
    private static long _memoFibCalls = 0;

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;

        Console.WriteLine("================================================================================");
        Console.WriteLine("  01. ОСНОВИ МЕМОІЗАЦІЇ (TOP-DOWN DP = РЕКУРСІЯ + БЛОКНОТ-КЕШ)");
        Console.WriteLine("================================================================================\n");

        RunStaircaseDemo();
        Console.WriteLine(new string('-', 80) + "\n");
        RunFibonacciDemo();
    }

    /// <summary>
    /// ДЕМО 1: Найпростіша життєва задача — "Підйом по сходинках" (Climbing Stairs).
    /// Умова: Є сходи з N сходинок. За один крок можна піднятися на 1 або на 2 сходинки.
    /// Питання: Скількома різними способами можна піднятися на N-ту сходинку?
    /// 
    /// Логіка "на пальцях":
    /// Як можна опинитися на сходинці №5?
    /// Або зробити 1 крок зі сходинки №4, або перестрибнути через одну зі сходинки №3!
    /// Отже: Способи(5) = Способи(4) + Способи(3).
    /// Загальна формула: Ways(n) = Ways(n - 1) + Ways(n - 2).
    /// </summary>
    private static void RunStaircaseDemo()
    {
        Console.WriteLine("--- ПРИКЛАД 1: Задача про сходинки (Climbing Stairs) ---");
        Console.WriteLine("Умова: За 1 крок ми можемо ступити на 1 або 2 сходинки вгору.");
        Console.WriteLine("Як дістатися на сходинку N? Зі сходинки (N-1) або зі сходинки (N-2).");
        Console.WriteLine("Формула: Ways(N) = Ways(N - 1) + Ways(N - 2)\n");

        // Спочатку покажемо покроково для маленького N = 5, як працює "блокнот" (кеш)
        int smallSteps = 5;
        var stepByStepNotebook = new Dictionary<int, long>();
        Console.WriteLine($"Покроковий розбір зі словником-блокнотом для N = {smallSteps} сходинок:");
        long ways5 = CountWaysWithVerboseNotebook(smallSteps, stepByStepNotebook, depth: 1);
        Console.WriteLine($"=> Відповідь для {smallSteps} сходинок: {ways5} способів.\n");

        // Тепер порівняємо кількість викликів для N = 35 сходинок БЕЗ кешу та З кешем
        int n = 35;
        Console.WriteLine($"Порівняння швидкодії на N = {n} сходинках:");

        _naiveStairsCalls = 0;
        var sw = Stopwatch.StartNew();
        long naiveWays = CountWaysNaive(n);
        sw.Stop();
        long naiveMs = sw.ElapsedMilliseconds;

        _memoStairsCalls = 0;
        var cache = new Dictionary<int, long>();
        sw.Restart();
        long memoWays = CountWaysMemoized(n, cache);
        sw.Stop();
        double memoUs = sw.Elapsed.TotalMicroseconds;

        Console.WriteLine($"  [БЕЗ КЕШУ]  Способів: {naiveWays:N0} | Викликів функції: {_naiveStairsCalls,13:N0} | Час: {naiveMs} мс");
        Console.WriteLine($"  [З КЕШЕМ]   Способів: {memoWays:N0} | Викликів функції: {_memoStairsCalls,13:N0} | Час: {memoUs:F1} мкс");
        Console.WriteLine($"  Висновок: Завдяки словнику Dictionary<int, long> кількість викликів впала з {_naiveStairsCalls:N0} до {_memoStairsCalls}!\n");
    }

    /// <summary>
    /// Наочна демонстрація того, як рекурсія записує результати у "блокнот" і дістає їх звідти.
    /// </summary>
    private static long CountWaysWithVerboseNotebook(int step, Dictionary<int, long> notebook, int depth)
    {
        string indent = new string(' ', depth * 2);

        if (step == 1)
        {
            Console.WriteLine($"{indent}-> Сходинка 1: базовий випадок (1 спосіб: [1])");
            return 1;
        }
        if (step == 2)
        {
            Console.WriteLine($"{indent}-> Сходинка 2: базовий випадок (2 способи: [1+1] або [2])");
            return 2;
        }

        // Перевіряємо наш блокнот!
        if (notebook.TryGetValue(step, out long savedWays))
        {
            Console.WriteLine($"{indent}-> Сходинка {step}: ЗНАЙДЕНО В БЛОКНОТІ! Не рахуємо заново, беремо {savedWays}.");
            return savedWays;
        }

        Console.WriteLine($"{indent}Рахуємо сходинку {step} = Ways({step - 1}) + Ways({step - 2})...");
        long result = CountWaysWithVerboseNotebook(step - 1, notebook, depth + 1)
                    + CountWaysWithVerboseNotebook(step - 2, notebook, depth + 1);

        // Записуємо в блокнот перед тим, як повернути результат
        notebook[step] = result;
        Console.WriteLine($"{indent}[Записали в блокнот]: Сходинка {step} -> {result} способів");
        return result;
    }

    private static long CountWaysNaive(int step)
    {
        _naiveStairsCalls++;
        if (step <= 2) return step;
        return CountWaysNaive(step - 1) + CountWaysNaive(step - 2);
    }

    private static long CountWaysMemoized(int step, Dictionary<int, long> memo)
    {
        _memoStairsCalls++;
        if (step <= 2) return step;

        // КРОК 1: Якщо ми вже рахували цю сходинку — миттєво повертаємо відповідь із кешу
        if (memo.TryGetValue(step, out long cached))
        {
            return cached;
        }

        // КРОК 2: Якщо ще не рахували — обчислюємо і кладемо в кеш
        long result = CountWaysMemoized(step - 1, memo) + CountWaysMemoized(step - 2, memo);
        memo[step] = result;
        return result;
    }

    /// <summary>
    /// ДЕМО 2: Класичні числа Фібоначчі для N = 40.
    /// Показує драматичну різницю між O(2^n) (сотні мільйонів викликів) та O(n) (79 викликів).
    /// </summary>
    private static void RunFibonacciDemo()
    {
        Console.WriteLine("--- ПРИКЛАД 2: Числа Фібоначчі (N = 40) ---");
        int n = 40;

        // 1. Наївна рекурсія: O(2^n) час
        Console.WriteLine($"1. Запуск звичайної рекурсії для Fib({n}) (без кешу)...");
        _naiveFibCalls = 0;
        var sw = Stopwatch.StartNew();
        long naiveResult = NaiveFibonacci(n);
        sw.Stop();
        Console.WriteLine($"   Результат:          Fib({n}) = {naiveResult:N0}");
        Console.WriteLine($"   Час виконання:      {sw.ElapsedMilliseconds} мс");
        Console.WriteLine($"   Кількість викликів: {_naiveFibCalls:N0} (понад 331 мільйон разів!)\n");

        // 2. Мемоізована рекурсія: O(n) час, O(n) пам'ять
        Console.WriteLine($"2. Запуск рекурсії з мемоізацією (Dictionary<int, long>) для Fib({n})...");
        _memoFibCalls = 0;
        var memo = new Dictionary<int, long>();
        sw.Restart();
        long memoResult = MemoizedFibonacci(n, memo);
        sw.Stop();
        Console.WriteLine($"   Результат:          Fib({n}) = {memoResult:N0}");
        Console.WriteLine($"   Час виконання:      {sw.Elapsed.TotalMicroseconds:F1} мкс");
        Console.WriteLine($"   Кількість викликів: {_memoFibCalls:N0} (усього {_memoFibCalls} викликів!)\n");

        // 3. Підсумок
        Console.WriteLine("--- Головне правило Мемоізації (Top-Down DP) ---");
        Console.WriteLine($"Прискорення за кількістю операцій: у {(_naiveFibCalls / (double)_memoFibCalls):N0} разів!");
        Console.WriteLine("1) Беремо звичайну рекурсивну функцію.");
        Console.WriteLine("2) Додаємо словник (або масив) memo.");
        Console.WriteLine("3) На початку функції перевіряємо: if (memo.TryGetValue(key, out var val)) return val;");
        Console.WriteLine("4) Перед return зберігаємо результат: memo[key] = result;");
    }

    /// <summary>
    /// Наївна рекурсія. Обчислює кожне піддерево заново, породжуючи O(2^n) викликів.
    /// </summary>
    private static long NaiveFibonacci(int n)
    {
        _naiveFibCalls++;
        if (n <= 1) return n;
        return NaiveFibonacci(n - 1) + NaiveFibonacci(n - 2);
    }

    /// <summary>
    /// Мемоізована рекурсія (Top-Down).
    /// Перед обчисленням перевіряє наявність результату в словнику-кеші.
    /// </summary>
    private static long MemoizedFibonacci(int n, Dictionary<int, long> memo)
    {
        _memoFibCalls++;
        if (n <= 1) return n;

        // Якщо підзадача вже розв'язана — повертаємо готове значення з кешу
        if (memo.TryGetValue(n, out long cachedValue))
        {
            return cachedValue;
        }

        // Інакше розв'язуємо підзадачу та зберігаємо результат у кеші
        long result = MemoizedFibonacci(n - 1, memo) + MemoizedFibonacci(n - 2, memo);
        memo[n] = result;
        return result;
    }
}
