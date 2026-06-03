// АГРЕГАЦІЯ (Aggregation) - контейнер "має" частини, але ці частини
// можуть існувати незалежно від контейнера і навіть переходити з одного
// контейнера в інший. Тут Library має книги, проте самі книги створюються
// окремо і не знищуються разом із бібліотекою.
//
// НАВІЩО ТУТ ІНТЕРФЕЙСИ: Library зберігає список ILendable, а не
// List<Book>. Завтра можна додати клас AudioBook чи Magazine, що теж
// реалізує ILendable, - і той самий метод AddBook() прийме його без
// жодної зміни коду Library.
namespace AggregationDemo;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== АГРЕГАЦІЯ: Бібліотека і книги ===\n");

        // Книги створюються НЕЗАЛЕЖНО від будь-якої бібліотеки.
        var book1 = new Book("Clean Code", "Robert Martin", "978-0132350884");
        var book2 = new Book("Design Patterns", "Gang of Four", "978-0201633610");
        var book3 = new Book("Refactoring", "Martin Fowler", "978-0201485677");

        var centralLibrary = new Library("Центральна бібліотека");
        var universityLibrary = new Library("Університетська бібліотека");

        centralLibrary.AddBook(book1);
        centralLibrary.AddBook(book2);
        universityLibrary.AddBook(book2);   // Та сама книга - в іншій бібліотеці.
        universityLibrary.AddBook(book3);

        centralLibrary.ShowCatalog();
        universityLibrary.ShowCatalog();

        Console.WriteLine("\nВидача книги:");
        var found = centralLibrary.FindBook("Clean Code");
        found?.Lend();

        Console.WriteLine("\nВидаляємо книгу з каталогу центральної бібліотеки:");
        centralLibrary.RemoveBook(book1);

        // Ключовий момент агрегації: книгу вилучено з КАТАЛОГУ, але сам
        // об'єкт book1 і далі повністю робочий - його можна додати в іншу
        // бібліотеку чи використовувати самостійно.
        Console.WriteLine($"\nКнига \"{book1.Title}\" видалена з каталогу,");
        Console.WriteLine($"але об'єкт продовжує існувати: статус = {(book1.IsAvailable ? "доступна" : "видана")}");
    }
}
