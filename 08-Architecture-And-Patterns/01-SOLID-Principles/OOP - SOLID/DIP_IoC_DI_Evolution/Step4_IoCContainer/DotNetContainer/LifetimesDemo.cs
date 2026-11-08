using System;
using Microsoft.Extensions.DependencyInjection;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step4_IoCContainer.DotNetContainer
{
    // Демонстрація різниці трьох життєвих циклів (Service Lifetimes) у .NET:
    // 1. Transient (AddTransient)
    // 2. Scoped (AddScoped)
    // 3. Singleton (AddSingleton)
    public static class LifetimesDemo
    {
        public static void Run()
        {
            Console.WriteLine("============================================================");
            Console.WriteLine("  КРОК 4.2. ЖИТТЄВІ ЦИКЛИ (Service Lifetimes) У .NET        ");
            Console.WriteLine("  Порівняння: Transient vs Scoped vs Singleton              ");
            Console.WriteLine("============================================================\n");

            var services = new ServiceCollection();

            // 1. Transient: Новий екземпляр генерується при кожному запиті до контейнера
            services.AddTransient<ITransientOperation>(_ => new DefaultOperation("Transient"));

            // 2. Scoped: Створюється один раз на кожен Scope (наприклад, 1 HTTP-запит у ASP.NET Core)
            services.AddScoped<IScopedOperation>(_ => new DefaultOperation("Scoped"));

            // 3. Singleton: Створюється рівно 1 раз при першому запиті і живе весь час роботи процесу
            services.AddSingleton<ISingletonOperation>(_ => new DefaultOperation("Singleton"));

            using ServiceProvider rootProvider = services.BuildServiceProvider();

            Console.WriteLine("--- ОБЛАСТЬ 1 (Scope 1: наприклад, перший HTTP-запит) ---");
            using (IServiceScope scope1 = rootProvider.CreateScope())
            {
                var provider = scope1.ServiceProvider;

                var transient1 = provider.GetRequiredService<ITransientOperation>();
                var transient2 = provider.GetRequiredService<ITransientOperation>();

                var scoped1 = provider.GetRequiredService<IScopedOperation>();
                var scoped2 = provider.GetRequiredService<IScopedOperation>();

                var singleton1 = provider.GetRequiredService<ISingletonOperation>();
                var singleton2 = provider.GetRequiredService<ISingletonOperation>();

                Console.WriteLine($"[Scope 1] Transient 1 ID: {transient1.Id}");
                Console.WriteLine($"[Scope 1] Transient 2 ID: {transient2.Id}  <-- РІЗНІ (новий об'єкт щоразу!)");
                Console.WriteLine($"[Scope 1] Scoped 1    ID: {scoped1.Id}");
                Console.WriteLine($"[Scope 1] Scoped 2    ID: {scoped2.Id}  <-- ОДНАКОВІ (спільний у межах Scope 1)");
                Console.WriteLine($"[Scope 1] Singleton 1 ID: {singleton1.Id}");
                Console.WriteLine($"[Scope 1] Singleton 2 ID: {singleton2.Id}  <-- ОДНАКОВІ (єдиний на додаток)");
            }

            Console.WriteLine("\n--- ОБЛАСТЬ 2 (Scope 2: наприклад, другий HTTP-запит) ---");
            using (IServiceScope scope2 = rootProvider.CreateScope())
            {
                var provider = scope2.ServiceProvider;

                var transient3 = provider.GetRequiredService<ITransientOperation>();
                var scoped3 = provider.GetRequiredService<IScopedOperation>();
                var singleton3 = provider.GetRequiredService<ISingletonOperation>();

                Console.WriteLine($"[Scope 2] Transient 3 ID: {transient3.Id}  <-- РІЗНИЙ (знову новий об'єкт)");
                Console.WriteLine($"[Scope 2] Scoped 3    ID: {scoped3.Id}  <-- НОВИЙ (Scope змінився, створено новий екземпляр)");
                Console.WriteLine($"[Scope 2] Singleton 3 ID: {singleton3.Id}  <-- ТОЙ САМИЙ, ЩО Й У SCOPE 1!");
            }

            Console.WriteLine("\n[ВИСНОВОК З ЖИТТЄВИХ ЦИКЛІВ]:");
            Console.WriteLine("• Transient: Використовуйте для легких сервісів без збереження стану (Stateless).");
            Console.WriteLine("• Scoped: Стандарт для роботи з базами даних (DbContext), одиницями роботи (Unit of Work).");
            Console.WriteLine("• Singleton: Кеш у пам'яті, клієнти HTTP, фонові черги задач (Stateful single instance).");
        }
    }
}
