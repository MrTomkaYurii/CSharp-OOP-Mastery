using System.Threading;

namespace LockAndMonitorDemo;

/// <summary>
/// Демонструє три способи критичної секції (взаємного виключення потоків):
///   1. Класичний оператор lock(object), який компілюється у Monitor.Enter / Monitor.Exit.
///   2. Явне використання System.Threading.Monitor.TryEnter із таймаутом.
///   3. Сучасний тип System.Threading.Lock (.NET 9 / .NET 10), оптимізований без SyncBlock.
/// </summary>
public sealed class InventoryWarehouse
{
    // 1. Класичний об'єкт синхронізації (використовує SyncBlock в заголовку об'єкта)
    private readonly object _classicSyncRoot = new();

    // 2. Сучасний спеціалізований примітив блокування (.NET 9 / .NET 10)
    private readonly Lock _modernLock = new();

    private int _stockQuantity = 100;
    public int StockQuantity => _stockQuantity;

    /// <summary>
    /// Варіант 1: Класичний оператор lock (_classicSyncRoot).
    /// Компілятор розгортає його у try { Monitor.Enter(_classicSyncRoot, ref lockTaken); ... } finally { ... }.
    /// </summary>
    public bool TryReserveClassicLock(int quantity)
    {
        lock (_classicSyncRoot)
        {
            if (_stockQuantity < quantity)
            {
                return false;
            }

            _stockQuantity -= quantity;
            return true;
        }
    }

    /// <summary>
    /// Варіант 2: Сучасний тип System.Threading.Lock (.NET 9+).
    /// Компілятор автоматично викликає _modernLock.EnterScope() у блоці using.
    /// </summary>
    public void RestockModernLock(int quantity)
    {
        lock (_modernLock)
        {
            _stockQuantity += quantity;
        }
    }

    /// <summary>
    /// Варіант 3: Неблокуюча спроба входу в критичну секцію з таймаутом через Monitor.TryEnter.
    /// </summary>
    public bool TryInspectWithTimeout(TimeSpan timeout, out int currentStock)
    {
        bool lockTaken = false;
        try
        {
            Monitor.TryEnter(_classicSyncRoot, timeout, ref lockTaken);
            if (!lockTaken)
            {
                currentStock = -1;
                return false;
            }

            currentStock = _stockQuantity;
            return true;
        }
        finally
        {
            if (lockTaken)
            {
                Monitor.Exit(_classicSyncRoot);
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
        Console.WriteLine(" 01. КРИТИЧНІ СЕКЦІЇ: LOCK(OBJECT), MONITOR ТА НОВИЙ SYSTEM.THREADING.LOCK");
        Console.WriteLine(" Як захистити спільний стан, коли потрібні складні інваріанти (перевірка + зміна)");
        Console.WriteLine("==========================================================================\n");

        var warehouse = new InventoryWarehouse();

        // 1. Багатопотокове бронювання товарів зі складу (початковий залишок = 100 шт.)
        Console.WriteLine("--- 1. Одночасне списання товару 15 потоками (по 10 шт., на складі 100 шт.) ---");
        int successfulOrders = 0;
        int rejectedOrders = 0;

        var threads = new Thread[15];
        for (int i = 0; i < threads.Length; i++)
        {
            threads[i] = new Thread(() =>
            {
                if (warehouse.TryReserveClassicLock(10))
                {
                    Interlocked.Increment(ref successfulOrders);
                }
                else
                {
                    Interlocked.Increment(ref rejectedOrders);
                }
            });
            threads[i].Start();
        }

        foreach (Thread t in threads)
        {
            t.Join();
        }

        Console.WriteLine($"  Успішно зарезервовано замовлень (по 10 шт.): {successfulOrders} (Очікувано: 10)");
        Console.WriteLine($"  Відхилено через нестачу на складі:           {rejectedOrders} (Очікувано: 5)");
        Console.WriteLine($"  Залишок на складі після 15 потоків:          {warehouse.StockQuantity} шт. (Інваріант >= 0 збережено!)\n");

        // 2. Поповнення складу через сучасний System.Threading.Lock (.NET 9 / .NET 10)
        Console.WriteLine("--- 2. Поповнення складу через сучасний тип System.Threading.Lock (.NET 9+) ---");
        warehouse.RestockModernLock(50);
        Console.WriteLine($"  Залишок після RestockModernLock(50): {warehouse.StockQuantity} шт.\n");

        // 3. Спроба входу з таймаутом через Monitor.TryEnter
        Console.WriteLine("--- 3. Безпечне очікування з таймаутом через Monitor.TryEnter ---");
        if (warehouse.TryInspectWithTimeout(TimeSpan.FromMilliseconds(100), out int stock))
        {
            Console.WriteLine($"  Monitor.TryEnter успішно отримав доступ! Поточний залишок: {stock} шт.");
        }
    }
}
