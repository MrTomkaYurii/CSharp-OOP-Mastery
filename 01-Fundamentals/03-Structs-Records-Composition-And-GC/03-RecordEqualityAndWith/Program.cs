namespace RecordEqualityAndWith;

/// <summary>
/// Звичайний клас: оператор '==' порівнює адреси в пам'яті (Reference Equality).
/// </summary>
public sealed class MoneyClass
{
    public decimal Amount { get; }
    public string Currency { get; }

    public MoneyClass(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }
}

/// <summary>
/// Позиційний запис (record): компілятор автоматично генерує:
///   1. Порівняння за значенням усіх властивостей (Equals, GetHashCode, оператори == та !=).
///   2. Красиве форматування у ToString().
///   3. Підтримку неруйнівного копіювання через вираз 'with'.
///   4. Метод Deconstruct для розпакування в кортеж.
/// </summary>
public sealed record SubscriptionPlan(string Name, decimal MonthlyPrice, int MaxUsers);

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("==========================================================================");
        Console.WriteLine(" 03. ПОРІВНЯННЯ ОБ'ЄКТІВ: REFERENCE EQUALITY (CLASS) ПРОТИ VALUE EQUALITY (RECORD)");
        Console.WriteLine(" Як працюють позиційні записи record, оператор 'with' та деконструкція");
        Console.WriteLine("==========================================================================\n");

        // 1. Порівняння двох однакових за даними екземплярів Class
        Console.WriteLine("--- 1. Класи порівнюються за посиланням у пам'яті (Reference Equality) ---");
        var price1 = new MoneyClass(500m, "UAH");
        var price2 = new MoneyClass(500m, "UAH");
        Console.WriteLine($"  price1 == price2 (Class):      {price1 == price2} (різні адреси в Купі)");
        Console.WriteLine($"  ReferenceEquals(price1, price2): {ReferenceEquals(price1, price2)}\n");

        // 2. Порівняння двох однакових за даними екземплярів Record
        Console.WriteLine("--- 2. Записи (record) порівнюються за вмістом полів (Value Equality) ---");
        var planA = new SubscriptionPlan("Pro", 29.99m, MaxUsers: 10);
        var planB = new SubscriptionPlan("Pro", 29.99m, MaxUsers: 10);
        Console.WriteLine($"  planA == planB (Record):       {planA == planB} (усі поля збігаються!)");
        Console.WriteLine($"  ReferenceEquals(planA, planB): {ReferenceEquals(planA, planB)} (але це два різні об'єкти в пам'яті)\n");

        // 3. Неруйнівне клонування через оператор 'with' та Deconstruct
        Console.WriteLine("--- 3. Клонування зі зміною властивості через 'with' та Deconstruct ---");
        SubscriptionPlan enterprisePlan = planA with { Name = "Enterprise", MaxUsers = 100 };
        Console.WriteLine($"  Оригінал (planA):       {planA}");
        Console.WriteLine($"  Клон (enterprisePlan):  {enterprisePlan}");

        // Деконструкція запису в окремі змінні:
        var (planName, monthlyCost, usersLimit) = enterprisePlan;
        Console.WriteLine($"  Деконструкція: Тариф='{planName}', Ціна={monthlyCost}$, Ліміт={usersLimit} користувачів.");
    }
}
