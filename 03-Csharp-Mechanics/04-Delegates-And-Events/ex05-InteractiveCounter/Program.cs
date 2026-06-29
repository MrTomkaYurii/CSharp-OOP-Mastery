using System;

// ============================================================================
// КАСТОМНИЙ ПРИКЛАД 5: Інтерактивний лічильник (Event-Driven Programming)
// ============================================================================
// НАВІЩО ЦЕЙ ПРИКЛАД:
// Показати подієво-орієнтоване програмування в реальному часі за стандартом .NET:
//   1. Створення власного класу аргументів події (EventArgs).
//   2. Використання стандартного делегата EventHandler<TEventArgs>.
//   3. Канонічний захищений віртуальний метод On<EventName>(...).
//   4. Інтерактивна реакція на дії користувача (натискання клавіш).
// ============================================================================

namespace ex05_InteractiveCounter;

// ============================================================================
// [КРОК 1] КЛАС АРГУМЕНТІВ ПОДІЇ (EventArgs)
// ============================================================================
public class ThresholdReachedEventArgs : EventArgs
{
    public int Threshold { get; }
    public int CurrentTotal { get; }
    public DateTime TimeReached { get; }

    public ThresholdReachedEventArgs(int threshold, int currentTotal)
    {
        Threshold = threshold;
        CurrentTotal = currentTotal;
        TimeReached = DateTime.Now;
    }
}

// ============================================================================
// [КРОК 2] КЛАС-ВИДАВЕЦЬ (Publisher): Counter
// ============================================================================
public class Counter
{
    private readonly int _threshold;
    public int Total { get; private set; }

    // Стандартна подія .NET:
    public event EventHandler<ThresholdReachedEventArgs>? ThresholdReached;

    public Counter(int threshold)
    {
        _threshold = threshold;
    }

    public void Add(int amount)
    {
        Total += amount;
        Console.WriteLine($"   [Лічильник] +{amount} | Поточна сума: {Total} / Поріг: {_threshold}");

        // Якщо умова досягнута — генеруємо подію:
        if (Total >= _threshold)
        {
            var args = new ThresholdReachedEventArgs(_threshold, Total);
            OnThresholdReached(args);
        }
    }

    // КАНОНІЧНИЙ ПАТЕРН .NET:
    // 1. Метод має префікс 'On' + назва події: OnThresholdReached.
    // 2. Модифікатор 'protected virtual' дозволяє нащадкам перевизначити подію.
    // 3. '?.Invoke(this, e)' забезпечує потокобезпечний виклик.
    protected virtual void OnThresholdReached(ThresholdReachedEventArgs e)
    {
        ThresholdReached?.Invoke(this, e);
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Приклад 5: Інтерактивний лічильник (Event-Driven) ===\n");

        // --------------------------------------------------------------------
        // [КРОК 3] Створення видавця з порогом
        // --------------------------------------------------------------------
        int randomThreshold = new Random().Next(3, 7);
        Counter counter = new(randomThreshold);

        // --------------------------------------------------------------------
        // [КРОК 4] Реєстрація підписників (+ =)
        // --------------------------------------------------------------------
        // Підписник 1: іменований метод для виведення звіту
        counter.ThresholdReached += OnCounterThresholdReached;

        // Підписник 2: лямбда для візуального сигналу
        counter.ThresholdReached += (sender, e) =>
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("   ⚠️ [СИГНАЛ ТРИВОГИ] Лічильник перетнув ліміт безпеки!");
            Console.ResetColor();
        };

        // --------------------------------------------------------------------
        // [КРОК 5] Інтерактивний цикл очікування подій від користувача
        // --------------------------------------------------------------------
        Console.WriteLine($"Встановлено порогове значення: {randomThreshold}");
        Console.WriteLine("Натискайте клавішу 'a' або '+' для кроку лічильника.");
        Console.WriteLine("Натисніть 'q' для передчасного виходу.\n");

        bool reached = false;

        while (!reached)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);

            if (key.KeyChar == 'a' || key.KeyChar == '+')
            {
                counter.Add(1);
                if (counter.Total >= randomThreshold)
                {
                    reached = true;
                }
            }
            else if (key.KeyChar == 'q' || key.KeyChar == 'Q')
            {
                Console.WriteLine("\n[Користувач перервав роботу].");
                break;
            }
        }

        // --------------------------------------------------------------------
        // [КРОК 6] Відписка обробників (- =)
        // --------------------------------------------------------------------
        counter.ThresholdReached -= OnCounterThresholdReached;

        Console.WriteLine("\n[Завершення]: Обробники відписані. Роботу завершено успішно.");
    }

    // Обробник за стандартом .NET: (object? sender, EventArgs e)
    static void OnCounterThresholdReached(object? sender, ThresholdReachedEventArgs e)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"\n🎯 [ПОДІЯ СПРАЦЮВАЛА] Поріг {e.Threshold} успішно досягнуто!");
        Console.WriteLine($"   Фінальна сума: {e.CurrentTotal}, Час фіксації: {e.TimeReached:HH:mm:ss}");
        Console.ResetColor();
    }
}
