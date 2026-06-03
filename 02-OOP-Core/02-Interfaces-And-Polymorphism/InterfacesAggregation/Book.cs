// АГРЕГАЦІЯ: Book створюється НЕЗАЛЕЖНО від будь-якої бібліотеки - вона б
// існувала і без Library. Бібліотека лише зберігатиме посилання на вже
// готову книгу, не створюючи і не знищуючи її сама (див. Library.cs).
namespace AggregationDemo;

public class Book : ILendable
{
    public string Title { get; }
    public string Author { get; }
    public string Isbn { get; }
    public bool IsAvailable { get; private set; } = true;

    public Book(string title, string author, string isbn)
    {
        Title = title;
        Author = author;
        Isbn = isbn;
    }

    public void Lend()
    {
        if (IsAvailable)
        {
            IsAvailable = false;
            Console.WriteLine($"  Видано: \"{Title}\" ({Author})");
        }
        else
        {
            Console.WriteLine($"  Книга \"{Title}\" вже видана");
        }
    }

    public void Return()
    {
        IsAvailable = true;
        Console.WriteLine($"  Повернено: \"{Title}\" ({Author})");
    }
}
