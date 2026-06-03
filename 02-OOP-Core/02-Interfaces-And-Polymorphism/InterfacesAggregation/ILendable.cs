// Контракт "того, що можна видати і повернути" - книга, диск, інструмент
// тощо. Library працює через цей інтерфейс, а не через конкретний клас
// Book, тому легко додати інший тип видань, не змінюючи код бібліотеки.
namespace AggregationDemo;

public interface ILendable
{
    string Title { get; }
    string Author { get; }
    bool IsAvailable { get; }
    void Lend();
    void Return();
}
