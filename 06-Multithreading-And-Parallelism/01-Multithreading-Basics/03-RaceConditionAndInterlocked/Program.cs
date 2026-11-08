using System.Threading;

namespace RaceConditionAndInterlocked;

/// <summary>
/// ПРОБЛЕМНИЙ КЛАС (Bad): Звичайний інкремент (_balance++) НЕ є атомарним!
/// На рівні процесора операція _balance++ складається з 3 окремих інструкцій:
///   1. READ:   Зчитати значення з пам'яті в регістр CPU
///   2. MODIFY: Додати 1 у регістрі CPU
///   3. WRITE:  Записати нове значення назад у пам'ять
/// Коли два потоки виконують це одночасно, один перезаписує результат іншого.
/// </summary>
public sealed class UnsafeTicketCounter
{
    private int _soldTickets;
    public int SoldTickets => _soldTickets;

    public void SellTicket()
    {
        // Стан гонитви (Race Condition): частина інкрементів губиться!
        _soldTickets++;
    }
}

/// <summary>
/// ЕТАЛОННИЙ КЛАС (Good): Використовує апаратну атомарність процесора через клас Interlocked.
/// Інструкція LOCK XADD на рівні CPU виконує читання та додавання як одну неподільну операцію.
/// </summary>
public sealed class AtomicTicketCounter
{
    private int _soldTickets;
    private long _totalRevenue;

    public int SoldTickets => Volatile.Read(ref _soldTickets);
    public long TotalRevenue => Interlocked.Read(ref _totalRevenue);

    public void SellTicket(int price)
    {
        // Атомарне збільшення на 1 без використання важких блокувань (Lock-Free)
        Interlocked.Increment(ref _soldTickets);

        // Атомарне додавання довільного числа
        Interlocked.Add(ref _totalRevenue, price);
    }

    /// <summary>
    /// Приклад атомарного CAS (Compare-And-Swap): оновлюємо максимум лише якщо нова ціна більша.
    /// </summary>
    public static bool TryUpdateMaxPrice(ref int currentMax, int candidatePrice)
    {
        while (true)
        {
            int snapshot = Volatile.Read(ref currentMax);
            if (candidatePrice <= snapshot)
            {
                return false;
            }

            // Якщо currentMax все ще дорівнює snapshot, записуємо candidatePrice:
            if (Interlocked.CompareExchange(ref currentMax, candidatePrice, snapshot) == snapshot)
            {
                return true;
            }
        }
    }
}

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("==========================================================================");
        Console.WriteLine(" 03. СТАН ГОНИТВИ (RACE CONDITION) ТА АТОМАРНІ ОПЕРАЦІЇ INTERLOCKED");
        Console.WriteLine(" Чому операція 'x++' губить дані у багатопотоковому коді та як це виправити");
        Console.WriteLine("==========================================================================\n");

        const int threadsCount = 8;
        const int incrementsPerThread = 100_000;
        const int expectedTotal = threadsCount * incrementsPerThread;

        // 1. Демонстрація втрати даних через Race Condition
        Console.WriteLine($"--- 1. Антипатерн: Несинхронізований лічильник ({threadsCount} потоків по {incrementsPerThread:N0} операцій) ---");
        var unsafeCounter = new UnsafeTicketCounter();
        RunInParallel(threadsCount, () =>
        {
            for (int i = 0; i < incrementsPerThread; i++)
            {
                unsafeCounter.SellTicket();
            }
        });

        Console.WriteLine($"  Очікувана кількість квитків: {expectedTotal:N0}");
        Console.WriteLine($"  Фактично отримано (Unsafe):  {unsafeCounter.SoldTickets:N0}");
        Console.WriteLine($"  ВТРАЧЕНО ЧЕРЕЗ ГОНИТВУ:      {expectedTotal - unsafeCounter.SoldTickets:N0} інкрементів!\n");

        // 2. Вирішення через атомарні операції Interlocked
        Console.WriteLine("--- 2. Еталон: Атомарний лічильник на базі System.Threading.Interlocked ---");
        var atomicCounter = new AtomicTicketCounter();
        RunInParallel(threadsCount, () =>
        {
            for (int i = 0; i < incrementsPerThread; i++)
            {
                atomicCounter.SellTicket(price: 100);
            }
        });

        Console.WriteLine($"  Очікувана кількість квитків: {expectedTotal:N0}");
        Console.WriteLine($"  Фактично отримано (Atomic):  {atomicCounter.SoldTickets:N0}");
        Console.WriteLine($"  Загальна виручка (Atomic):   {atomicCounter.TotalRevenue:N0} грн (Втрат: 0!)\n");

        // 3. Демонстрація CompareExchange (Lock-Free оновлення стану)
        Console.WriteLine("--- 3. Атомарна заміна Interlocked.CompareExchange (CAS) ---");
        int maxBid = 500;
        bool updatedLow = AtomicTicketCounter.TryUpdateMaxPrice(ref maxBid, 450);
        bool updatedHigh = AtomicTicketCounter.TryUpdateMaxPrice(ref maxBid, 1200);
        Console.WriteLine($"  Спроба встановити ставку 450 (при поточній 500):  Успіх = {updatedLow},  Максимум = {maxBid}");
        Console.WriteLine($"  Спроба встановити ставку 1200 (при поточній 500): Успіх = {updatedHigh}, Максимум = {maxBid}");
    }

    private static void RunInParallel(int threadCount, Action work)
    {
        var threads = new Thread[threadCount];
        for (int i = 0; i < threadCount; i++)
        {
            threads[i] = new Thread(() => work());
            threads[i].Start();
        }
        for (int i = 0; i < threadCount; i++)
        {
            threads[i].Join();
        }
    }
}
