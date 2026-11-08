using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ServiceLifetimesDemo;

// ============================================================================
// ПРОЄКТ 01: Часи життя сервісів у DI: Transient, Scoped та Singleton
// ============================================================================
// Демонструє:
// 1. Transient (AddTransient): новий екземпляр створюється при КОЖНОМУ запиті до контейнера,
//    навіть у межах однієї бізнес-операції (Scope).
// 2. Scoped (AddScoped): створюється рівно ОДИН екземпляр на одну бізнес-операцію (IServiceScope)
//    і спільно використовується всіма сервісами всередині цього Scope.
// 3. Singleton (AddSingleton): створюється ОДИН раз на весь час роботи IHost.
// 4. Автоматичний виклик Dispose() для Transient/Scoped наприкінці Scope та для Singleton при зупинці хоста.
// ============================================================================

internal static class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  01. Часи життя сервісів у DI: Transient, Scoped та Singleton (Generic Host) ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝\n");

        HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

        // Реєструємо три сервіси з різним часом життя та бізнес-координатор
        builder.Services.AddTransient<ITransientOperation, OperationTracker>();
        builder.Services.AddScoped<IScopedOperation, OperationTracker>();
        builder.Services.AddSingleton<ISingletonOperation, OperationTracker>();

        builder.Services.AddTransient<OrderStepAlpha>();
        builder.Services.AddTransient<OrderStepBeta>();

        using IHost host = builder.Build();

        // Виконуємо дві незалежні бізнес-операції (два окремі IServiceScope)
        ExecuteBusinessOperation(host.Services, operationNumber: 1, title: "Оформлення замовлення #101");
        Console.WriteLine();
        ExecuteBusinessOperation(host.Services, operationNumber: 2, title: "Оформлення замовлення #102");

        Console.WriteLine("\n[Main] Завершення роботи IHost (зараз звільниться Singleton)...");
    }

    private static void ExecuteBusinessOperation(IServiceProvider rootProvider, int operationNumber, string title)
    {
        Console.WriteLine($"┌─── Початок Scope #{operationNumber}: {title} ───────────────────────────────────┐");

        using (IServiceScope scope = rootProvider.CreateScope())
        {
            IServiceProvider sp = scope.ServiceProvider;

            // У межах одного Scope викликаємо два різні кроки обробки замовлення
            OrderStepAlpha stepAlpha = sp.GetRequiredService<OrderStepAlpha>();
            OrderStepBeta stepBeta = sp.GetRequiredService<OrderStepBeta>();

            stepAlpha.Execute();
            stepBeta.Execute();
        }

        Console.WriteLine($"└─── Scope #{operationNumber} закрито (усі Scoped та Transient сервіси звільнено) ────┘");
    }
}

public interface IOperation
{
    string InstanceId { get; }
}

public interface ITransientOperation : IOperation { }
public interface IScopedOperation : IOperation { }
public interface ISingletonOperation : IOperation { }

/// <summary>
/// Універсальний лічильник, який генерує короткий унікальний ідентифікатор екземпляра
/// та повідомляє про своє звільнення у методі <see cref="Dispose"/>.
/// </summary>
public sealed class OperationTracker :
    ITransientOperation,
    IScopedOperation,
    ISingletonOperation,
    IDisposable
{
    private static int _counter;
    public string InstanceId { get; }

    public OperationTracker()
    {
        int num = Interlocked.Increment(ref _counter);
        InstanceId = $"#{num:D2}-{Guid.NewGuid().ToString("N")[..4].ToUpperInvariant()}";
    }

    public void Dispose()
    {
        Console.WriteLine($"   ♻️ [Dispose] Звільнено екземпляр OperationTracker {InstanceId}");
    }
}

/// <summary>
/// Перший учасник бізнес-операції (наприклад, резервування товарів).
/// </summary>
public sealed class OrderStepAlpha
{
    private readonly ITransientOperation _transient;
    private readonly IScopedOperation _scoped;
    private readonly ISingletonOperation _singleton;

    public OrderStepAlpha(
        ITransientOperation transient,
        IScopedOperation scoped,
        ISingletonOperation singleton)
    {
        _transient = transient;
        _scoped = scoped;
        _singleton = singleton;
    }

    public void Execute()
    {
        Console.WriteLine("   [Крок Alpha — Перевірка кошика]:");
        Console.WriteLine($"     • Transient (новий щоразу):       {_transient.InstanceId}");
        Console.WriteLine($"     • Scoped    (спільний для Scope): {_scoped.InstanceId}");
        Console.WriteLine($"     • Singleton (єдиний для IHost):   {_singleton.InstanceId}");
    }
}

/// <summary>
/// Другий учасник тієї ж бізнес-операції (наприклад, виставлення рахунку).
/// Показує, що Scoped та Singleton збігаються з Кроком Alpha, а Transient — уже новий!
/// </summary>
public sealed class OrderStepBeta
{
    private readonly ITransientOperation _transient;
    private readonly IScopedOperation _scoped;
    private readonly ISingletonOperation _singleton;

    public OrderStepBeta(
        ITransientOperation transient,
        IScopedOperation scoped,
        ISingletonOperation singleton)
    {
        _transient = transient;
        _scoped = scoped;
        _singleton = singleton;
    }

    public void Execute()
    {
        Console.WriteLine("   [Крок Beta  — Проведення оплати у тому ж Scope]:");
        Console.WriteLine($"     • Transient (НОВИЙ екземпляр!):   {_transient.InstanceId}");
        Console.WriteLine($"     • Scoped    (ТОЙ САМИЙ у Scope):  {_scoped.InstanceId}");
        Console.WriteLine($"     • Singleton (ТОЙ САМИЙ у IHost):  {_singleton.InstanceId}");
    }
}
