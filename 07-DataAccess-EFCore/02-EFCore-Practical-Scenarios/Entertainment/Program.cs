using Entertainment;
using Entertainment.Configuration;
using Entertainment.Features;
using Entertainment.HostedServices;
using Entertainment.Models;
using Entertainment.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quieries.Interfaces;
using Serilog;
using System;


// Цей клас є точкою входу програми, яка налаштовує консоль на UTF-8 для коректної української мови,
// конфігурує логування через Serilog, зчитує конфігураційні файли, реєструє всі потрібні сервіси
// (включно з DbContext, запитами, кешем, меню та додатковими можливостями) у контейнері залежностей,
// підключає фонові служби для ініціалізації бази даних та запуску інтерактивного меню, а потім створює
// й запускає хост .NET, забезпечуючи повний життєвий цикл роботи застосунку

// Налаштування кодування консолі для підтримки української мови
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

// Налаштування Serilog для логування
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
    .WriteTo.File("logs/entertainment-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

try
{
    Log.Information("Запуск додатку Entertainment");

    var host = Host.CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((context, config) =>
        {
            // Додаємо конфігураційні файли
            config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                  .AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                              optional: true, reloadOnChange: true)
                  .AddEnvironmentVariables()
                  .AddCommandLine(args);
        })
        .ConfigureServices((context, services) =>
        {
            // Налаштовуємо Options Pattern для strongly-typed configuration
            services.Configure<DatabaseInitializationOptions>(
                context.Configuration.GetSection(DatabaseInitializationOptions.SectionName));
            services.Configure<FeaturesOptions>(
                context.Configuration.GetSection(FeaturesOptions.SectionName));

            // Додаємо валідацію опцій
            services.AddSingleton<IValidateOptions<DatabaseInitializationOptions>,
                DatabaseInitializationOptionsValidator>();

            // Отримуємо рядок підключення з конфігурації
            var connectionString = context.Configuration.GetConnectionString("EntertainmentDb");

            // Реєструємо DbContext
            services.AddDbContext<EntertainmentDbContext>(options =>
            {
                options.UseSqlite(connectionString);
                options.EnableSensitiveDataLogging(context.HostingEnvironment.IsDevelopment());
            });

            // Реєструємо сервіси для запитів
            services.AddScoped<IQueryService, HighestGrossingMoviesQuery>();
            services.AddScoped<IQueryService, ProductionsByRatingsQuery>();
            services.AddScoped<IQueryService, ActorsMultipleCharactersQuery>();
            services.AddScoped<IQueryService, RatingsBySourceQuery>();
            services.AddScoped<IQueryService, SeriesFewestEpisodesQuery>();
            services.AddScoped<IQueryService, FilteredProductionsQuery>();
            services.AddScoped<IQueryService, ActorsPlayingThemselvesQuery>();
            services.AddScoped<IQueryService, ProductionsWithTheQuery>();
            services.AddScoped<IQueryService, SummerMoviesQuery>();
            services.AddScoped<IQueryService, HighestRatingQuery>();

            // Розширені запити з додатковими функціями
            services.AddScoped<IQueryService, CachedMoviesQuery>();
            services.AddScoped<IQueryService, ExportableQuery>();
            services.AddScoped<IQueryService, ConfigurableQueryService>();

            // Реєструємо фабрику для створення запитів
            services.AddSingleton<IQueryServiceFactory, QueryServiceFactory>();

            // Реєструємо сервіс меню
            services.AddSingleton<IMenuService, MenuService>();

            // Додаткові сервіси для розширених можливостей
            services.AddSingleton<ICacheService, CacheService>();
            services.AddSingleton<IPerformanceMonitor, PerformanceMonitor>();
            services.AddSingleton<IExportService, ExportService>();


            // Реєструємо Hosted Service для ініціалізації БД
            services.AddHostedService<DatabaseInitializationService>();
            // Реєструємо головний Hosted Service додатку
            services.AddHostedService<EntertainmentHostedService>();
        })
        .UseSerilog() // Використовуємо Serilog
        .Build();

    await host.RunAsync();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Додаток завершився з критичною помилкою");
}
finally
{
    Log.CloseAndFlush();
}