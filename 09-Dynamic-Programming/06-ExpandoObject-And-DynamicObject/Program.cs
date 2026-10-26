using System.Dynamic;

namespace ExpandoAndDynamicObject;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("=== 06. Динамічні типи даних: ExpandoObject та DynamicObject ===\n");

        // 1. Демонстрація System.Dynamic.ExpandoObject
        RunExpandoObjectDemo();

        // 2. Демонстрація власного класу на базі System.Dynamic.DynamicObject
        RunDynamicObjectDemo();
    }

    private static void RunExpandoObjectDemo()
    {
        Console.WriteLine("--- 1. Робота з System.Dynamic.ExpandoObject ---");

        // ExpandoObject дозволяє додавати властивості та методи безпосередньо в runtime
        dynamic employee = new ExpandoObject();
        employee.FirstName = "Тарас";
        employee.LastName = "Шевченко";
        employee.Age = 35;
        employee.Position = "Team Lead";

        // Додавання методу як динамічного делегата
        employee.GetFullName = new Func<string>(() => $"{employee.FirstName} {employee.LastName}");
        employee.GiveRaise = new Action<decimal>(percent =>
        {
            Console.WriteLine($"  [Підвищення]: Зарплату працівника {employee.FirstName} підвищено на {percent}%!");
        });

        Console.WriteLine($"Працівник: {employee.GetFullName()}, Посада: {employee.Position}, Вік: {employee.Age}");
        employee.GiveRaise(15.0m);

        // ExpandoObject під капотом реалізує IDictionary<string, object?>
        Console.WriteLine("\nПриведення ExpandoObject до IDictionary<string, object?>:");
        var dictionary = (IDictionary<string, object?>)employee;
        foreach (var pair in dictionary)
        {
            Console.WriteLine($"  Ключ: {pair.Key,-12} -> Значення: {pair.Value}");
        }
        Console.WriteLine();
    }

    private static void RunDynamicObjectDemo()
    {
        Console.WriteLine("--- 2. Власний динамічний провайдер на базі DynamicObject ---");

        // DynamicConfiguration імітує динамічну роботу з конфігурацією без оголошення класів
        dynamic config = new DynamicConfiguration();

        // Запис властивостей (викликає TrySetMember)
        config.ServerHost = "127.0.0.1";
        config.Port = 8080;
        config.Environment = "Production";

        // Читання властивостей (викликає TryGetMember)
        Console.WriteLine($"Хост сервера:  {config.ServerHost}");
        Console.WriteLine($"Порт сервера:  {config.Port}");
        Console.WriteLine($"Середовище:    {config.Environment}");

        // Динамічний виклик методу (викликає TryInvokeMember)
        config.PrintSummary("Конфігурація системи");

        // Спроба звернутися до неіснуючої властивості
        Console.WriteLine($"Звернення до відсутнього ключа: {config.TimeoutMs ?? "null (не знайдено)"}\n");
    }
}

/// <summary>
/// Користувацький динамічний об'єкт.
/// Перехоплює звернення до властивостей та виклики методів під час виконання.
/// </summary>
public class DynamicConfiguration : DynamicObject
{
    private readonly Dictionary<string, object?> _members = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Перехоплює операцію встановлення властивості (наприклад, config.Port = 8080).
    /// </summary>
    public override bool TrySetMember(SetMemberBinder binder, object? value)
    {
        _members[binder.Name] = value;
        return true;
    }

    /// <summary>
    /// Перехоплює операцію отримання властивості (наприклад, var port = config.Port).
    /// </summary>
    public override bool TryGetMember(GetMemberBinder binder, out object? result)
    {
        // Не кидаємо виняток, а повертаємо null, якщо ключ не знайдено
        if (_members.TryGetValue(binder.Name, out result))
        {
            return true;
        }

        result = null;
        return true;
    }

    /// <summary>
    /// Перехоплює виклик динамічного методу (наприклад, config.PrintSummary("Title")).
    /// </summary>
    public override bool TryInvokeMember(InvokeMemberBinder binder, object?[]? args, out object? result)
    {
        if (binder.Name == "PrintSummary")
        {
            string title = args?.Length > 0 ? args[0]?.ToString() ?? "Звіт" : "Звіт";
            Console.WriteLine($"  [{title}]: У конфігурації збережено {_members.Count} параметрів.");
            result = true;
            return true;
        }

        result = null;
        return false;
    }
}
