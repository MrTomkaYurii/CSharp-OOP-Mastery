using System;

// ============================================================================
// ПРИКЛАД: p382[SOLIS] - Стандартний шаблон подій .NET (EventHandler<TEventArgs>)
// ============================================================================
// Тема: Загальноприйняті правила (конвенція) платформи .NET для подій:
//       1) Передача даних через клас, успадкований від EventArgs;
//       2) Використання стандартного узагальненого делегата EventHandler<T>;
//       3) Стандартна сигнатура обробника: (object sender, TEventArgs e).
// ============================================================================

// 1. ВЛАСНИЙ КЛАС ДАНИХ ПОДІЇ (Event Arguments):
// За конвенцією .NET, будь-яка додаткова інформація про подію повинна оформлюватися
// у вигляді класу, успадкованого від базового системного класу EventArgs.
// Назва класу зазвичай закінчується на 'EventArgs'.
public class IncrementerEventArgs : EventArgs
{
    // Властивість, яка передає номер ітерації, на якій сталася подія
    public int IterationCount { get; set; }
}

// 2. ВИДАВЕЦЬ (PUBLISHER):
class Incrementer
{
    // Замість створення власного типу делегата через 'delegate void ...',
    // в .NET прийнято використовувати готовий вбудований узагальнений делегат:
    // EventHandler<TEventArgs>, де TEventArgs — наш клас із даними події.
    // Сигнатура EventHandler<T> завжди приймає (object? sender, T e) і повертає void.
    public event EventHandler<IncrementerEventArgs>? CountedADozen;

    public void DoCount()
    {
        IncrementerEventArgs args = new IncrementerEventArgs();

        for (int i = 1; i < 100; i++)
        {
            if (i % 12 == 0 && CountedADozen != null)
            {
                // Заповнюємо дані про подію
                args.IterationCount = i;

                // ВИКЛИК ПОДІЇ ЗА СТАНДАРТОМ .NET:
                // 1-й аргумент: 'this' — посилання на сам об'єкт-видавець (sender)
                // 2-й аргумент: 'args' — об'єкт із додатковими параметрами події
                CountedADozen(this, args);
            }
        }
    }
}

// 3. ПІДПИСНИК (SUBSCRIBER):
class Dozens
{
    public int DozensCount { get; private set; }

    public Dozens(Incrementer incrementer)
    {
        DozensCount = 0;
        // Підписуємо метод IncrementDozensCount на подію видавця
        incrementer.CountedADozen += IncrementDozensCount;
    }

    // МЕТОД-ОБРОБНИК ЗА СТАНДАРТОМ .NET:
    // Завжди має два параметри:
    //  - object source (або sender): хто ініціював подію (тут це екземпляр Incrementer)
    //  - IncrementerEventArgs e: пакет з інформацією про те, що саме сталося
    void IncrementDozensCount(object? source, IncrementerEventArgs e)
    {
        Console.WriteLine($"Incremented at iteration: {e.IterationCount} in {source.ToString()}");
        DozensCount++;
    }
}

class Program
{
    static void Main()
    {
        Incrementer incrementer = new Incrementer();
        Dozens dozensCounter = new Dozens(incrementer);

        incrementer.DoCount();

        Console.WriteLine($"Number of dozens = {dozensCounter.DozensCount}");
    }
}