using System.Text;
using DI.GenericHost.Worker.Configuration;
using DI.GenericHost.Worker.Services;
using DI.GenericHost.Worker.Workers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("=================================================================");
Console.WriteLine("  .NET 10 GENERIC HOST & WORKER SERVICES ДЕМОНСТРАЦІЯ");
Console.WriteLine("=================================================================\n");

// Створення білдера Generic Host за сучасним шаблоном HostApplicationBuilder
var builder = Host.CreateApplicationBuilder(args);

// 1. Конфігурація та валідація опцій (Options Pattern)
builder.Services.AddOptions<WorkerOptions>()
    .Bind(builder.Configuration.GetSection(WorkerOptions.SectionName))
    .ValidateDataAnnotations()
    .ValidateOnStart();

// 2. Реєстрація бізнес-сервісів
builder.Services.AddScoped<IOrderProcessingService, OrderProcessingService>();

// 3. Реєстрація фонових служб (IHostedService та BackgroundService)
builder.Services.AddHostedService<LifecycleMonitorService>();
builder.Services.AddHostedService<OrderProcessingWorker>();

// 4. Побудова хоста та запуск життєвого циклу
var host = builder.Build();

await host.RunAsync();

Console.WriteLine("\n[Generic Host успішно завершив роботу]");
