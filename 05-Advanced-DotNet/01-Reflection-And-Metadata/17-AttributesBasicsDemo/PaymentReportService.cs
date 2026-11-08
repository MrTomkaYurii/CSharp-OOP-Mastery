using System.Runtime.CompilerServices;

namespace AttributesBasicsDemo;

/// <summary>
/// Бізнес-клас, розмічений стандартними та користувацькими атрибутами.
/// </summary>
[AuthorInfo("Олександр Коваленко", Version = "1.0", LastModified = "2026-09-15")]
[AuthorInfo("Марія Бойко", Version = "2.1", LastModified = "2026-10-20")]
public class PaymentReportService
{
    // 1. Вбудований атрибут компілятора [Obsolete]
    // Компілятор сам зчитує цей атрибут і видає попередження CS0618 при спробі виклику.
    [Obsolete("Цей метод застарів. Використовуйте метод GenerateSummaryReport() замість нього.")]
    public void PrintOldFormatReport()
    {
        Console.WriteLine("  [Legacy]: Друкуємо звіт у старому текстовому форматі...");
    }

    // 2. Вбудовані атрибути компілятора для трасування викликів
    public void LogExecutionStep(
        string message,
        [CallerMemberName] string callerName = "",
        [CallerLineNumber] int lineNumber = 0)
    {
        Console.WriteLine($"  [Log]: Викликано з методу '{callerName}' (рядок {lineNumber}) -> {message}");
    }

    // 3. Методи, розмічені нашим власним атрибутом [ConsoleCommand]
    [ConsoleCommand("help", "Виводить список усіх доступних команд сервісу")]
    public void ShowHelp()
    {
        Console.WriteLine("  [Help]: Довідкова система платіжного модуля активна.");
    }

    [ConsoleCommand("status", "Перевіряє стан підключення до платіжного шлюзу")]
    public void CheckGatewayStatus()
    {
        Console.WriteLine("  [Status]: Платіжний шлюз працює у штатному режимі (Ping: 12ms).");
    }

    [ConsoleCommand("reset-cache", "Очищує фінансовий кеш (потребує прав Admin)")]
    [RequiresPermission("Admin")]
    public void ResetFinancialCache()
    {
        Console.WriteLine("  [Admin Action]: Фінансовий кеш успішно очищено!");
    }
}
