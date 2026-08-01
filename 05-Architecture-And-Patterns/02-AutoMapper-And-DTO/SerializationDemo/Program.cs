using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SerializationDemo;
using SerializationDemo.Interfaces;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        // Реєстрація сервісів у DI контейнері
        services.AddSingleton<IDataGenerator, DataGenerator>();
        services.AddSingleton<ISerializationService, SerializationService>();
        services.AddSingleton<IDemoRunner, DemoRunner>();
        services.AddSingleton<AppHost>();
    })
    .Build();

// Отримуємо головний "хост" застосунку і запускаємо його
var app = host.Services.GetRequiredService<AppHost>();
await app.RunAsync();