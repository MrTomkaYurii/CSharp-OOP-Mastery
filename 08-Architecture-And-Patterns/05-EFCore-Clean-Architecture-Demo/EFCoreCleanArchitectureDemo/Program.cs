using EFCoreCleanArchitectureDemo.Application;
using EFCoreCleanArchitectureDemo.Infrastructure;
using EFCoreCleanArchitectureDemo.Presentation;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EFCoreCleanArchitectureDemo;

internal class Program
{
    static async Task Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // 1. Відкриваємо підключення до реляційної БД SQLite у пам'яті (In-Memory Relational DB)
        using var keepAliveConnection = new SqliteConnection("DataSource=:memory:");
        await keepAliveConnection.OpenAsync();

        // 2. Налаштовуємо IoC/DI контейнер (Composition Root)
        var services = new ServiceCollection();

        // Реєструємо DbContext EF Core
        services.AddDbContext<BookstoreDbContext>(options =>
            options.UseSqlite(keepAliveConnection));

        // Реєструємо AutoMapper з профілем BookstoreMappingProfile
        services.AddAutoMapper(typeof(BookstoreMappingProfile).Assembly);

        // Реєструємо шари за принципами SOLID (DIP / OCP / SRP)
        services.AddScoped<IBookRepository, EfCoreBookRepository>();
        services.AddScoped<IPricingStrategy, AcademicDiscountPricingStrategy>();
        services.AddScoped<CatalogApplicationService>();
        services.AddSingleton<SpectreCatalogRenderer>();

        // 3. Будуємо провайдер сервісів і виконуємо бізнес-сценарій у Scoped-контексті
        await using ServiceProvider rootProvider = services.BuildServiceProvider(validateScopes: true);
        using IServiceScope scope = rootProvider.CreateScope();

        var appService = scope.ServiceProvider.GetRequiredService<CatalogApplicationService>();
        var uiRenderer = scope.ServiceProvider.GetRequiredService<SpectreCatalogRenderer>();

        IReadOnlyList<BookCatalogDto> catalog = await appService.GetCatalogDtosAsync();
        uiRenderer.RenderCatalog(catalog, appService.ActivePricingStrategyName);
    }
}
