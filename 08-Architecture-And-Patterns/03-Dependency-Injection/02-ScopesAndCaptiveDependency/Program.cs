using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ScopesAndCaptiveDependency;

// ============================================================================
// ПРОЄКТ 02: Управління областями видимості (IServiceScopeFactory)
//            та пастка Captive Dependency («Ув'язнена залежність»)
// ============================================================================
// Демонструє:
// 1. Що таке Captive Dependency: коли Singleton-сервіс або BackgroundService
//    напряму інжектує у конструктор Scoped-залежність (наприклад, DbContext чи UnitOfWork),
//    перетворюючи її на небезпечний прихований Singleton.
// 2. Як вбудована перевірка ValidateScopes = true та ValidateOnBuild = true
//    миттєво ловить Captive Dependency ще під час побудови IHost.
// 3. Еталонне виправлення: інжекція IServiceScopeFactory у Singleton/BackgroundService
//    та створення свіжого IServiceScope для кожної окремої ітерації роботи.
// ============================================================================

internal static class Program
{
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  02. IServiceScopeFactory та пастка Captive Dependency у Generic Host        ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════════╝\n");

        // ЧАСТИНА 1: Демонстрація виявлення Captive Dependency через ValidateScopes / ValidateOnBuild
        Console.WriteLine("1. Спроба зареєструвати Singleton, який напряму вимагає Scoped-репозиторій:");
        DemonstrateCaptiveDependencyDetection();

        // ЧАСТИНА 2: Правильна архітектура з IServiceScopeFactory
        Console.WriteLine("\n2. Еталонне рішення: використання IServiceScopeFactory у Singleton-координаторі:");
        await RunSafeScopedCoordinatorAsync(args);
    }

    private static void DemonstrateCaptiveDependencyDetection()
    {
        HostApplicationBuilder brokenBuilder = Host.CreateApplicationBuilder();

        // Явно вмикаємо строгу перевірку графа залежностей та областей видимості
        brokenBuilder.ConfigureContainer(new DefaultServiceProviderFactory(new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true
        }));

        brokenBuilder.Services.AddScoped<ScopedUnitOfWork>();
        // ❌ ПОМИЛКА: Singleton намагається захопити ScopedUnitOfWork у свій конструктор!
        brokenBuilder.Services.AddSingleton<BrokenSingletonProcessor>();

        try
        {
            using IHost brokenHost = brokenBuilder.Build();
        }
        catch (AggregateException ex)
        {
            Console.WriteLine("   ⚡ Контейнер DI заблокував збірку хоста (ValidateOnBuild + ValidateScopes)!");
            foreach (Exception inner in ex.InnerExceptions)
            {
                Console.WriteLine($"      ❌ Деталі помилки: {inner.Message}");
            }
        }
    }

    private static async Task RunSafeScopedCoordinatorAsync(string[] args)
    {
        HostApplicationBuilder safeBuilder = Host.CreateApplicationBuilder(args);

        safeBuilder.ConfigureContainer(new DefaultServiceProviderFactory(new ServiceProviderOptions
        {
            ValidateScopes = true,
            ValidateOnBuild = true
        }));

        safeBuilder.Services.AddScoped<ScopedUnitOfWork>();
        safeBuilder.Services.AddSingleton<SafeBatchCoordinator>();

        using IHost host = safeBuilder.Build();

        SafeBatchCoordinator coordinator = host.Services.GetRequiredService<SafeBatchCoordinator>();

        await coordinator.ProcessJobAsync("Пакет синхронізації складу #A1");
        await coordinator.ProcessJobAsync("Пакет синхронізації складу #A2");
    }
}

/// <summary>
/// Імітація контексту бази даних або одиниці роботи (Unit of Work),
/// яка НЕ є потокобезпечною і обов'язково має жити лише в межах однієї операції (Scoped).
/// </summary>
public sealed class ScopedUnitOfWork : IDisposable
{
    private static int _counter;
    public int ContextNumber { get; } = Interlocked.Increment(ref _counter);

    public ScopedUnitOfWork()
    {
        Console.WriteLine($"    [ScopedUnitOfWork #{ContextNumber}] Відкрито новий ізольований сеанс БД.");
    }

    public Task SaveBatchAsync(string jobName)
    {
        Console.WriteLine($"    [ScopedUnitOfWork #{ContextNumber}] Успішно збережено зміни для «{jobName}».");
        return Task.CompletedTask;
    }

    public void Dispose()
    {
        Console.WriteLine($"    [ScopedUnitOfWork #{ContextNumber}] Сеанс закрито та з'єднання повернуто в пул.");
    }
}

/// <summary>
/// ❌ АНТИПАТЕРН (Captive Dependency): Singleton напряму залежить від Scoped-сервісу.
/// Якби контейнер не заблокував це, <see cref="ScopedUnitOfWork"/> жив би вічно і ніколи не звільнявся б!
/// </summary>
public sealed class BrokenSingletonProcessor
{
    public ScopedUnitOfWork CapturedUnitOfWork { get; }

    public BrokenSingletonProcessor(ScopedUnitOfWork capturedUnitOfWork)
    {
        CapturedUnitOfWork = capturedUnitOfWork;
    }
}

/// <summary>
/// ✅ ЕТАЛОН: Singleton-сервіс (або BackgroundService) інжектує <see cref="IServiceScopeFactory"/>
/// і відкриває короткоживучий Scope лише на час виконання конкретної задачі.
/// </summary>
public sealed class SafeBatchCoordinator
{
    private readonly IServiceScopeFactory _scopeFactory;

    public SafeBatchCoordinator(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    public async Task ProcessJobAsync(string jobName)
    {
        Console.WriteLine($"   ─── Початок обробки завдання: {jobName} ───");

        // Створюємо асинхронну область видимості спеціально для цієї операції
        await using AsyncServiceScope scope = _scopeFactory.CreateAsyncScope();

        var unitOfWork = scope.ServiceProvider.GetRequiredService<ScopedUnitOfWork>();
        await unitOfWork.SaveBatchAsync(jobName);
    }
}
