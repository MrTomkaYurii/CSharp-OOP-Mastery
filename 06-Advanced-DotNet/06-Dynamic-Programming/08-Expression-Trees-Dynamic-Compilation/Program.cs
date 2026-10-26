using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTreesDynamicCompilation;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("=== 08. Динамічна компіляція дерев виразів (Expression Trees) ===\n");

        // 1. Динамічний конструктор фільтрів (Dynamic Filter Builder)
        RunDynamicFilterDemo();

        // 2. Скомпільована фабрика створення об'єктів
        RunExpressionFactoryDemo();
    }

    private static void RunDynamicFilterDemo()
    {
        Console.WriteLine("--- 1. Побудова динамічного предикату валідації в Runtime ---");
        var catalog = new List<Device>
        {
            new("MacBook Pro", Price: 2200, InStock: true, Rating: 4.9),
            new("Dell XPS 15", Price: 1800, InStock: false, Rating: 4.6),
            new("Lenovo ThinkPad", Price: 1300, InStock: true, Rating: 4.7),
            new("Asus ROG", Price: 2500, InStock: true, Rating: 4.8),
            new("Acer Aspire", Price: 600, InStock: true, Rating: 3.9)
        };

        // Задаємо динамічні умови відбору (наприклад, із форми пошуку користувача):
        decimal minPrice = 1000m;
        bool requireStock = true;
        double minRating = 4.5;

        Console.WriteLine($"Умови пошуку: Price >= {minPrice}, InStock == {requireStock}, Rating >= {minRating}");

        // Динамічно будуємо дерево виразів: device => device.Price >= 1000 && device.InStock == true && device.Rating >= 4.5
        var parameter = Expression.Parameter(typeof(Device), "d");

        var priceProp = Expression.Property(parameter, nameof(Device.Price));
        var priceCondition = Expression.GreaterThanOrEqual(priceProp, Expression.Constant(minPrice));

        var stockProp = Expression.Property(parameter, nameof(Device.InStock));
        var stockCondition = Expression.Equal(stockProp, Expression.Constant(requireStock));

        var ratingProp = Expression.Property(parameter, nameof(Device.Rating));
        var ratingCondition = Expression.GreaterThanOrEqual(ratingProp, Expression.Constant(minRating));

        // Об'єднуємо умови через логічне І (Expression.AndAlso):
        var combinedBody = Expression.AndAlso(Expression.AndAlso(priceCondition, stockCondition), ratingCondition);

        // Компілюємо вираз у нативний типізований предикат:
        var filterPredicate = Expression.Lambda<Func<Device, bool>>(combinedBody, parameter).Compile();

        Console.WriteLine("\nЗнайдені пристрої за скомпільованим деревом виразів:");
        foreach (var item in catalog.Where(filterPredicate))
        {
            Console.WriteLine($"  * {item.Name}: ${item.Price}, Рейтинг: {item.Rating}, На складі: {item.InStock}");
        }
        Console.WriteLine();
    }

    private static void RunExpressionFactoryDemo()
    {
        Console.WriteLine("--- 2. Порівняння швидкодії: Activator проти Compiled Expression Factory ---");
        ConstructorInfo ctor = typeof(Device).GetConstructor(new[] { typeof(string), typeof(decimal), typeof(bool), typeof(double) })!;

        // Створюємо скомпільовану фабрику: (name, price, stock, rating) => new Device(name, price, stock, rating)
        var pName = Expression.Parameter(typeof(string), "name");
        var pPrice = Expression.Parameter(typeof(decimal), "price");
        var pStock = Expression.Parameter(typeof(bool), "stock");
        var pRating = Expression.Parameter(typeof(double), "rating");

        var newExpression = Expression.New(ctor, pName, pPrice, pStock, pRating);
        var factory = Expression.Lambda<Func<string, decimal, bool, double, Device>>(
            newExpression, pName, pPrice, pStock, pRating).Compile();

        int iterations = 100_000;

        // Тест Activator.CreateInstance
        var sw = Stopwatch.StartNew();
        object[] args = { "TestDevice", 999m, true, 4.5 };
        for (int i = 0; i < iterations; i++)
        {
            _ = Activator.CreateInstance(typeof(Device), args);
        }
        sw.Stop();
        long activatorTime = sw.ElapsedMilliseconds;

        // Тест Compiled Expression Factory
        sw.Restart();
        for (int i = 0; i < iterations; i++)
        {
            _ = factory("TestDevice", 999m, true, 4.5);
        }
        sw.Stop();
        long factoryTime = sw.ElapsedMilliseconds;

        Console.WriteLine($"100,000 створень через Activator.CreateInstance: {activatorTime} ms");
        Console.WriteLine($"100,000 створень через Compiled Expression Tree:   {factoryTime} ms");
        Console.WriteLine($"Прискорення: у {((double)activatorTime / Math.Max(factoryTime, 1)):F1} разів швидше!\n");
    }
}

public record Device(string Name, decimal Price, bool InStock, double Rating);
