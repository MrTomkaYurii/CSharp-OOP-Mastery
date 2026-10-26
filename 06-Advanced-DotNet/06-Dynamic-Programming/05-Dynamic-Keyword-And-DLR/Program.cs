using System.Diagnostics;

namespace DynamicKeywordAndDlr;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("=== 05. Ключове слово dynamic та архітектура DLR (Dynamic Language Runtime) ===\n");

        // 1. Різниця між object, var та dynamic
        Console.WriteLine("--- 1. Порівняння object vs var vs dynamic ---");
        object objValue = "Привіт, світ!";
        // objValue.ToUpper(); // Помилка компіляції: object не містить методу ToUpper! Потрібен явний каст:
        string explicitCast = ((string)objValue).ToUpper();
        Console.WriteLine($"object (з явним приведенням типу): {explicitCast}");

        var inferredValue = "Привіт, світ!"; // Компілятор вивів тип string на етапі компіляції
        Console.WriteLine($"var (статичний строгий тип):          {inferredValue.ToUpper()}");

        dynamic dynamicValue = "Привіт, світ!"; // Компілятор пропускає статичні перевірки
        Console.WriteLine($"dynamic (runtime пізнє зв'язування):   {dynamicValue.ToUpper()}\n");

        // 2. Робота DLR CallSite та кешування викликів
        Console.WriteLine("--- 2. Кешування місць виклику DLR (CallSite Cache) ---");
        dynamic calculator = new MathService();

        // Перший виклик: DLR створює та компілює CallSite (холодний старт)
        var sw = Stopwatch.StartNew();
        int res1 = calculator.Add(10, 20);
        sw.Stop();
        long coldMs = sw.ElapsedTicks;

        // Наступні виклики: використання скомпільованого L1/L2 кешу
        sw.Restart();
        int res2 = calculator.Add(30, 40);
        sw.Stop();
        long warmMs = sw.ElapsedTicks;

        Console.WriteLine($"Перший виклик DLR (холодний): {coldMs} тактів (компіляція біндеру)");
        Console.WriteLine($"Другий виклик DLR (гарячий):  {warmMs} тактів (виклик із кешу CallSite)");
        Console.WriteLine($"Прискорення завдяки CallSite: у {((double)coldMs / Math.Max(warmMs, 1)):F1} разів швидше!\n");

        // 3. Динамічна мультидиспетчеризація (Dynamic Multidispatch)
        Console.WriteLine("--- 3. Динамічна диспетчеризація перевантажених методів ---");
        // У стандартному C# перевантаження обирається на етапі компіляції за типом змінної.
        // Завдяки dynamic вибір методу відбувається у RUNTIME за фактичним типом об'єкта!
        object item1 = new Order { Id = 101, Total = 250.0m };
        object item2 = new Customer { Name = "Олена", Tier = "VIP" };
        object item3 = "Простий текстовий лог";

        Console.WriteLine("Виклик ProcessEntity() без жодного if/switch чи явного касту:");
        DispatchEntity(item1);
        DispatchEntity(item2);
        DispatchEntity(item3);
        Console.WriteLine();
    }

    /// <summary>
    /// Метод-диспетчер: приймає dynamic і DLR автоматично обирає потрібне перевантаження.
    /// </summary>
    private static void DispatchEntity(dynamic entity)
    {
        var processor = new EventProcessor();
        // DLR в рантаймі знайде перевантажений метод Handle, що відповідає точному типу entity
        processor.Handle(entity);
    }
}

public class MathService
{
    public int Add(int a, int b) => a + b;
}

public class Order
{
    public int Id { get; set; }
    public decimal Total { get; set; }
}

public class Customer
{
    public string Name { get; set; } = string.Empty;
    public string Tier { get; set; } = string.Empty;
}

public class EventProcessor
{
    public void Handle(Order order) =>
        Console.WriteLine($"  [Обробка Замовлення]: Id={order.Id}, Сума=${order.Total}");

    public void Handle(Customer customer) =>
        Console.WriteLine($"  [Обробка Клієнта]: Ім'я={customer.Name}, Рівень={customer.Tier}");

    public void Handle(object unknown) =>
        Console.WriteLine($"  [Універсальна обробка невідомого типу]: {unknown}");
}
