// IDisposable - клас, що володіє справжнім ресурсом (файлом).
//
// LogWriter обгортає StreamWriter, який тримає відкритий файл. Оскільки
// LogWriter володіє одноразовим об'єктом, він сам мусить бути IDisposable
// і в своєму Dispose() звільнити StreamWriter - "хто володіє, той і закриває".
//
// Також показано:
//   - using-оголошення (C# 8): using var x = ...; - об'єкт звільняється
//     наприкінці області видимості, без додаткових фігурних дужок;
//   - повторний виклик Dispose() має бути безпечним;
//   - користуватися об'єктом після Dispose() не можна - типова поведінка
//     це ObjectDisposedException.
namespace DisposableOwnerDemo;

class LogWriter : IDisposable
{
    private readonly StreamWriter writer;
    private bool disposed;

    public LogWriter(string path)
    {
        writer = new StreamWriter(path); // відкриваємо файл для запису
    }

    public void Write(string message)
    {
        if (disposed)
            throw new ObjectDisposedException(nameof(LogWriter));

        writer.WriteLine(message);
    }

    public void Dispose()
    {
        if (disposed)
            return; // повторний виклик нічого не робить

        writer.Dispose(); // закриваємо файл і скидаємо буфер на диск
        disposed = true;
        Console.WriteLine("  [-] Файл журналу закрито");
    }
}

class Program
{
    static void Main()
    {
        string path = Path.GetTempFileName();

        // 1. using-оголошення: Dispose() спрацює, коли метод дійде до кінця.
        Console.WriteLine("=== 1. using-оголошення ===");
        WriteLog(path);
        Console.WriteLine("  Вміст файлу після виходу з методу:");
        Console.WriteLine("  " + File.ReadAllText(path).TrimEnd().Replace(Environment.NewLine, Environment.NewLine + "  "));

        // 2. Повторний Dispose() безпечний, а робота після нього - помилка.
        Console.WriteLine("\n=== 2. Dispose двічі та використання після Dispose ===");
        var log = new LogWriter(path);
        log.Dispose();
        log.Dispose(); // друге закриття мовчки ігнорується

        try
        {
            log.Write("запис після закриття");
        }
        catch (ObjectDisposedException)
        {
            Console.WriteLine("  Write() після Dispose() -> ObjectDisposedException");
        }

        File.Delete(path);
    }

    static void WriteLog(string path)
    {
        using var log = new LogWriter(path);
        log.Write("Програму запущено");
        log.Write("Виконано роботу");

        // Поки log не закрито, файл ще зайнятий для запису (у Windows)
        // і дані можуть лишатися в буфері.
        try
        {
            Console.WriteLine("  Читаємо файл, поки він ще відкритий:");
            Console.WriteLine("  " + File.ReadAllText(path));
        }
        catch (IOException)
        {
            Console.WriteLine("  Файл ще зайнятий - записувач не закрито");
        }

        Console.WriteLine("  Кінець методу WriteLog");
        // Тут, після останнього рядка, автоматично викликається log.Dispose().
    }
}
