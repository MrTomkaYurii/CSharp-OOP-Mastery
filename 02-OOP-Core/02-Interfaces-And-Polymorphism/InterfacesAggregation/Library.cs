// АГРЕГАЦІЯ: Library "має" книги (список ILendable), але не володіє ними
// на рівні життєвого циклу. Книги передаються ЗЗОВНІ через AddBook -
// бібліотека їх не створює. Видалення книги з каталогу чи навіть закриття
// бібліотеки не знищує сам об'єкт книги - він і далі існує в програмі.
namespace AggregationDemo;

public class Library : ILibrary
{
    public string Name { get; }
    private readonly List<ILendable> books = new();

    public Library(string name) => Name = name;

    public void AddBook(ILendable book)
    {
        books.Add(book);
        Console.WriteLine($"  Додано до \"{Name}\": {book.Title}");
    }

    public void RemoveBook(ILendable book)
    {
        if (books.Remove(book))
        {
            Console.WriteLine($"  Видалено з каталогу: {book.Title}");
            Console.WriteLine("  Сама книга продовжує існувати незалежно від бібліотеки.");
        }
    }

    public ILendable? FindBook(string title) =>
        books.FirstOrDefault(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

    public void ShowCatalog()
    {
        Console.WriteLine($"\nКаталог бібліотеки \"{Name}\":");
        foreach (var book in books)
        {
            var status = book.IsAvailable ? "доступна" : "видана";
            Console.WriteLine($"  [{status}] {book.Title} - {book.Author}");
        }
    }
}
