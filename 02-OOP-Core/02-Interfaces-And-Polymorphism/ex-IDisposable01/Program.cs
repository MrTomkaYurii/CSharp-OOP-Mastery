// IDisposable + using - гарантоване звільнення ресурсу.
//
// Деякі об'єкти тримають ресурси, які збирач сміття не звільняє вчасно:
// файли, мережеві з'єднання, підключення до бази даних тощо. Інтерфейс
// IDisposable має єдиний метод Dispose(), у якому об'єкт "прибирає за собою".
//
// Блок using - це короткий запис try/finally: Dispose() викличеться
// автоматично при виході з блоку - і при нормальному завершенні, і при
// винятку. Тут ресурс імітується виводом у консоль, щоб було видно момент
// відкриття і закриття.
namespace DisposableUsingDemo;

class Connection : IDisposable
{
    private readonly string name;

    public Connection(string name)
    {
        this.name = name;
        Console.WriteLine($"  [+] З'єднання '{name}' відкрито");
    }

    public void Send(string message)
    {
        Console.WriteLine($"      '{name}' надсилає: {message}");
    }

    public void Dispose()
    {
        Console.WriteLine($"  [-] З'єднання '{name}' закрито");
    }
}

class Program
{
    static void Main()
    {
        // 1. Без using: закривати треба вручну - легко забути,
        //    особливо якщо між відкриттям і закриттям виникне виняток.
        Console.WriteLine("=== 1. Ручний виклик Dispose ===");
        var manual = new Connection("A");
        manual.Send("привіт");
        manual.Dispose();

        // 2. З using: Dispose() викликається автоматично в кінці блоку.
        Console.WriteLine("\n=== 2. Блок using ===");
        using (var connection = new Connection("B"))
        {
            connection.Send("привіт");
        }
        Console.WriteLine("  (вийшли з блоку using)");

        // 3. Виняток усередині using: з'єднання все одно буде закрито,
        //    причому ДО того, як керування потрапить у catch.
        Console.WriteLine("\n=== 3. Виняток усередині using ===");
        try
        {
            using (var connection = new Connection("C"))
            {
                connection.Send("привіт");
                throw new InvalidOperationException("Щось пішло не так");
            }
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"  Спіймано виняток: {ex.Message}");
        }

        // 4. Ось що насправді означає using - це просто try/finally.
        Console.WriteLine("\n=== 4. using = try/finally ===");
        var connectionD = new Connection("D");
        try
        {
            connectionD.Send("привіт");
        }
        finally
        {
            connectionD.Dispose();
        }
    }
}
