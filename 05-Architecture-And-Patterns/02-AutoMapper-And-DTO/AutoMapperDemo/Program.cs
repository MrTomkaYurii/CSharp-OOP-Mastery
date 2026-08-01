using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AutoMapperDemo.Features;
using AutoMapperDemo.Mapping;
using AutoMapperDemo.Features.Advanced;

namespace AutoMapperDemo
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            using IHost host = Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    // Регіструємо AutoMapper з профілем(ями)
                    services.AddAutoMapper(typeof(MappingProfile));

                    // Регіструємо усі фічи (як IFeature)
                    services.AddTransient<IFeature, BasicMappingFeature>();
                    services.AddTransient<IFeature, ComplexMappingFeature>();
                    services.AddTransient<IFeature, PrimitiveMappingFeature>();
                    services.AddTransient<IFeature, ReverseMappingFeature>();
                    services.AddTransient<IFeature, ConditionalMappingFeature>();
                    services.AddTransient<IFeature, IgnoreMappingFeature>();
                    services.AddTransient<IFeature, FixedDynamicValuesFeature>();
                    services.AddTransient<IFeature, CollectionsFeature>();
                    services.AddTransient<IFeature, AdvancedFeature>();

                    // Додаємо фічу для порівняння з ручним мапінгом
                    services.AddTransient<IFeature, ManualMappingFeature>();
                })
                .Build();

            // отримуємо всі фічи
            var features = host.Services.GetServices<IFeature>().ToList();

            Console.WriteLine("AutoMapper demo (Generic Host)");
            Console.WriteLine("Виберіть приклад:");

            while (true)
            {
                for (int i = 0; i < features.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {features[i].Title}");
                }
                Console.WriteLine("0. Вихід");

                Console.Write("Вибір: ");
                var key = Console.ReadLine();
                if (!int.TryParse(key, out int choice))
                {
                    Console.WriteLine("Невірний ввід, спробуйте ще.");
                    continue;
                }

                if (choice == 0) break;
                if (choice < 1 || choice > features.Count)
                {
                    Console.WriteLine("Невірний вибір.");
                    continue;
                }

                var feature = features[choice - 1];
                Console.WriteLine($"\n--- {feature.Title} ---\n");
                try
                {
                    await feature.RunAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка під час виконання фічі: {ex}");
                }
                Console.WriteLine("\n--- Кінець прикладу ---\n");
            }

            await host.StopAsync();
        }
    }
}