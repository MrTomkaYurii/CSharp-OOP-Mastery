// Контракт бібліотеки - працює з абстрактним ILendable, тому не важливо,
// що саме зберігається в каталозі: книга, журнал чи майбутній AudioBook.
namespace AggregationDemo;

public interface ILibrary
{
    string Name { get; }
    void AddBook(ILendable book);
    void RemoveBook(ILendable book);
    ILendable? FindBook(string title);
    void ShowCatalog();
}
