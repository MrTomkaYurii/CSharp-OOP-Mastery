using System.Collections.Concurrent;
using System.Dynamic;
using System.Linq.Expressions;

namespace DynamicPipelineEngine;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("=== 09. Фінальний синтез: Динамічний конвеєр розрахунку замовлень (Dynamic Pipeline Engine) ===\n");

        var engine = new OrderProcessingPipeline();

        // Тестове замовлення 1: Звичайний клієнт, доставка у регіон Lviv
        var order1 = new Order
        {
            Id = 1001,
            Customer = new RegularCustomer { Name = "Іван Петренко" },
            DeliveryRegion = "Lviv",
            Items = new List<OrderItem>
            {
                new("Клавіатура", Weight: 1, Value: 1200),
                new("Монітор", Weight: 4, Value: 8500),
                new("Мишка", Weight: 1, Value: 800)
            }
        };

        // Тестове замовлення 2: Корпоративний клієнт, доставка в Kyiv
        var order2 = new Order
        {
            Id = 1002,
            Customer = new CorporateClient { CompanyName = "TechCorp UA", ContractDiscount = 0.15m },
            DeliveryRegion = "Kyiv",
            Items = new List<OrderItem>
            {
                new("Серверний блок", Weight: 6, Value: 25000),
                new("Маршрутизатор", Weight: 2, Value: 5400)
            }
        };

        engine.Process(order1);
        Console.WriteLine(new string('-', 70));
        engine.Process(order2);
        Console.WriteLine(new string('-', 70));

        // Повторна обробка замовлення в той самий регіон для перевірки алгоритмічної мемоізації тарифів
        Console.WriteLine("Повторний розрахунок доставки для регіону Lviv (перевірка мемоізації DP):");
        engine.Process(order1);
    }
}

#region Моделі замовлення та клієнтів

public class Order
{
    public int Id { get; set; }
    public object Customer { get; set; } = null!;
    public string DeliveryRegion { get; set; } = string.Empty;
    public List<OrderItem> Items { get; set; } = new();
}

public record OrderItem(string Name, int Weight, decimal Value);

public class RegularCustomer
{
    public string Name { get; set; } = string.Empty;
}

public class CorporateClient
{
    public string CompanyName { get; set; } = string.Empty;
    public decimal ContractDiscount { get; set; }
}

#endregion

#region Архітектурний конвеєр Dynamic Pipeline Engine

public class OrderProcessingPipeline
{
    private readonly Func<string, decimal> _memoizedShippingCalculator;
    private readonly Func<Order, bool> _compiledHighValuePredicate;

    public OrderProcessingPipeline()
    {
        // 1. АЛГОРИТМІЧНЕ ДИНАМІЧНЕ ПРОГРАМУВАННЯ:
        // Створюємо мемоізовану функцію розрахунку вартості регіональної доставки (Top-Down DP)
        _memoizedShippingCalculator = CreateMemoizedShippingRateCalculator();

        // 2. ДИНАМІЧНЕ МЕТАПРОГРАМУВАННЯ (Expression Trees):
        // Динамічно компілюємо правило перевірки преміум-замовлення (Order.TotalValue >= 10000)
        _compiledHighValuePredicate = CompileHighValueRule(10000m);
    }

    public void Process(Order order)
    {
        Console.WriteLine($"[Обробка Замовлення #{order.Id}]");

        decimal subtotal = order.Items.Sum(i => i.Value);
        Console.WriteLine($"  1. Базова сума товарів:  ${subtotal:N2}");

        // 1. Алгоритмічне DP: Рюкзак (Knapsack DP) для пакування в стандартну посилку (ліміт 5 кг)
        int boxCapacity = 5;
        var (packedWeight, packedItems) = KnapsackOptimizer.PackOptimalBox(order.Items, boxCapacity);
        Console.WriteLine($"  2. Алгоритмічне DP (Knapsack): упаковано в експрес-коробку ({boxCapacity} кг):");
        foreach (var item in packedItems)
        {
            Console.WriteLine($"     * {item.Name} ({item.Weight} кг, ${item.Value})");
        }

        // 2. Алгоритмічне DP: Мемоізований тариф доставки
        decimal shippingCost = _memoizedShippingCalculator(order.DeliveryRegion);
        Console.WriteLine($"  3. Мемоізований DP-тариф доставки ({order.DeliveryRegion}): ${shippingCost:N2}");

        // 3. DLR: ExpandoObject для динамічного контексту бізнес-правил
        dynamic dynamicContext = new ExpandoObject();
        dynamicContext.ProcessedAt = DateTime.Now;
        dynamicContext.IsHolidayPromo = true;
        dynamicContext.RegionRiskLevel = order.DeliveryRegion == "Kyiv" ? "Low" : "Standard";

        // 4. Диспетчеризація через dynamic (DLR Multidispatch) для знижки за клієнтом
        decimal discountRate = ResolveCustomerDiscount(order.Customer);
        decimal discountAmount = subtotal * discountRate;
        Console.WriteLine($"  4. DLR Multidispatch: знижка клієнта ({discountRate:P0}): -${discountAmount:N2}");

        // 5. Перевірка динамічно скомпільованого правила через Expression Trees
        bool isHighValue = _compiledHighValuePredicate(order);
        if (isHighValue)
        {
            Console.WriteLine("  5. Expression Tree Rule: Активовано безкоштовну преміум-страховку (High-Value Order)!");
        }

        decimal finalTotal = (subtotal - discountAmount) + shippingCost;
        Console.WriteLine($"  РАЗОМ ДО СПЛАТИ: ${finalTotal:N2}");
    }

    /// <summary>
    /// Динамічна диспетчеризація на основі DLR (вибір методу в runtime без if/else)
    /// </summary>
    private decimal ResolveCustomerDiscount(dynamic customer)
    {
        return CalculateDiscountInternal(customer);
    }

    private decimal CalculateDiscountInternal(RegularCustomer regular) => 0.05m; // 5% знижка
    private decimal CalculateDiscountInternal(CorporateClient corporate) => corporate.ContractDiscount;
    private decimal CalculateDiscountInternal(object unknown) => 0.0m;

    /// <summary>
    /// Створення мемоізованого розрахунку доставки (Top-Down DP)
    /// </summary>
    private Func<string, decimal> CreateMemoizedShippingRateCalculator()
    {
        var cache = new ConcurrentDictionary<string, decimal>();

        return region => cache.GetOrAdd(region, r =>
        {
            Console.WriteLine($"     [DP Cache Miss] Обчислення тарифу для регіону '{r}'...");
            return r switch
            {
                "Kyiv" => 75.0m,
                "Lviv" => 90.0m,
                "Odesa" => 85.0m,
                _ => 120.0m
            };
        });
    }

    /// <summary>
    /// Динамічна компіляція дерева виразів у нативний предикат
    /// </summary>
    private Func<Order, bool> CompileHighValueRule(decimal threshold)
    {
        var orderParam = Expression.Parameter(typeof(Order), "o");
        var itemsProp = Expression.Property(orderParam, nameof(Order.Items));

        // Побудова виклику: order.Items.Sum(i => i.Value) >= threshold
        var sumMethod = typeof(Enumerable).GetMethods()
            .First(m => m.Name == nameof(Enumerable.Sum) 
                     && m.GetParameters().Length == 2 
                     && m.ReturnType == typeof(decimal))
            .MakeGenericMethod(typeof(OrderItem));

        var itemParam = Expression.Parameter(typeof(OrderItem), "i");
        var valueProp = Expression.Property(itemParam, nameof(OrderItem.Value));
        var valueLambda = Expression.Lambda<Func<OrderItem, decimal>>(valueProp, itemParam);

        var sumCall = Expression.Call(sumMethod, itemsProp, valueLambda);
        var thresholdConst = Expression.Constant(threshold);
        var comparison = Expression.GreaterThanOrEqual(sumCall, thresholdConst);

        return Expression.Lambda<Func<Order, bool>>(comparison, orderParam).Compile();
    }
}

#endregion

#region Алгоритмічне динамічне програмування (Knapsack DP)

public static class KnapsackOptimizer
{
    /// <summary>
    /// Класичний алгоритм 0/1 Knapsack: максимізує сумарну вартість товарів у межах обмеження ваги коробки.
    /// </summary>
    public static (int PackedWeight, List<OrderItem> PackedItems) PackOptimalBox(List<OrderItem> items, int capacity)
    {
        int n = items.Count;
        int[,] dp = new int[n + 1, capacity + 1];

        for (int i = 1; i <= n; i++)
        {
            var item = items[i - 1];
            for (int w = 0; w <= capacity; w++)
            {
                if (item.Weight <= w)
                {
                    dp[i, w] = Math.Max(dp[i - 1, w], dp[i - 1, w - item.Weight] + (int)item.Value);
                }
                else
                {
                    dp[i, w] = dp[i - 1, w];
                }
            }
        }

        // Відновлення вибраних товарів
        var selected = new List<OrderItem>();
        int currW = capacity;
        for (int i = n; i > 0 && currW > 0; i--)
        {
            if (dp[i, currW] != dp[i - 1, currW])
            {
                var item = items[i - 1];
                selected.Add(item);
                currW -= item.Weight;
            }
        }

        return (capacity - currW, selected);
    }
}

#endregion
