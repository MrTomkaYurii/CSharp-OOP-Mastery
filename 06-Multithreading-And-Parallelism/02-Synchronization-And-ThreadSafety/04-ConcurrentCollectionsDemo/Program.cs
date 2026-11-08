using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ConcurrentCollectionsDemo;

/// <summary>
/// Сервіс підрахунку переглядів сторінок на базі потокобезпечного ConcurrentDictionary.
/// Використовує атомарні методи AddOrUpdate та GetOrAdd замість зовнішнього lock.
/// </summary>
public sealed class PageViewAnalytics
{
    private readonly ConcurrentDictionary<string, int> _viewsByUrl = new();

    public void RecordVisit(string url)
    {
        // Якщо ключа ще немає — додаємо зі значенням 1.
        // Якщо ключ вже є — атомарно викликаємо лямбду оновлення (current + 1).
        _viewsByUrl.AddOrUpdate(
            key: url,
            addValue: 1,
            updateValueFactory: (_, currentCount) => currentCount + 1);
    }

    public IReadOnlyDictionary<string, int> GetSnapshot() => _viewsByUrl;
}

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("==========================================================================");
        Console.WriteLine(" 04. ПОТОКОБЕЗПЕЧНІ КОЛЕКЦІЇ (SYSTEM.COLLECTIONS.CONCURRENT)");
        Console.WriteLine(" ConcurrentDictionary, ConcurrentQueue та BlockingCollection без ручних lock");
        Console.WriteLine("==========================================================================\n");

        // 1. ConcurrentDictionary та метод AddOrUpdate
        Console.WriteLine("--- 1. ConcurrentDictionary<string, int>: Паралельний підрахунок з 1000 ітерацій ---");
        var analytics = new PageViewAnalytics();
        string[] pages = ["/catalog", "/cart", "/checkout"];

        Parallel.For(0, 1000, i =>
        {
            string url = pages[i % pages.Length];
            analytics.RecordVisit(url);
        });

        int totalRecorded = 0;
        foreach (var kvp in analytics.GetSnapshot())
        {
            Console.WriteLine($"  Сторінка {kvp.Key,-10} -> {kvp.Value,4} переглядів");
            totalRecorded += kvp.Value;
        }
        Console.WriteLine($"  Усього зафіксовано переглядів: {totalRecorded} (Очікувано: 1000, жоден запис не втрачено!)\n");

        // 2. ConcurrentQueue (FIFO черга без блокувань)
        Console.WriteLine("--- 2. ConcurrentQueue<T>: Потокобезпечна черга FIFO (TryDequeue) ---");
        var queue = new ConcurrentQueue<string>();
        queue.Enqueue("Подія #1: Авторизація");
        queue.Enqueue("Подія #2: Додавання в кошик");
        queue.Enqueue("Подія #3: Оплата замовлення");

        while (queue.TryDequeue(out string? eventMessage))
        {
            Console.WriteLine($"  Оброблено з черги: {eventMessage}");
        }
    }
}
