namespace CompositionVsAggregation;

/// <summary>
/// Деталь замовлення (частина Композиції): не має сенсу без самого Замовлення (Order).
/// Створюється та знищується виключно всередині класу Order.
/// </summary>
public sealed class OrderLineItem
{
    public string ProductTitle { get; }
    public int Quantity { get; }
    public decimal UnitPrice { get; }
    public decimal LineTotal => Quantity * UnitPrice;

    public OrderLineItem(string productTitle, int quantity, decimal unitPrice)
    {
        ProductTitle = productTitle;
        Quantity = quantity;
        UnitPrice = unitPrice;
    }
}

/// <summary>
/// Покупець (частина Агрегації): існує незалежно від конкретного замовлення.
/// Передається в Order ззовні через конструктор.
/// </summary>
public sealed class Customer
{
    public string FullName { get; }
    public string Email { get; }

    public Customer(string fullName, string email)
    {
        FullName = fullName;
        Email = email;
    }
}

/// <summary>
/// Клас Замовлення демонструє обидва види зв'язку "Has-A" (Має в собі):
///   1. АГРЕГАЦІЯ (Aggregation): посилання на Customer передається ззовні (слабкий зв'язок).
///   2. КОМПОЗИЦІЯ (Composition): колекція OrderLineItem створюється всередині Order (сильний зв'язок).
/// </summary>
public sealed class Order
{
    public int OrderNumber { get; }

    // Агрегація: Customer живе своїм життям і може робити інші замовлення
    public Customer Buyer { get; }

    // Композиція: рядки замовлення повністю належать цьому Order
    private readonly List<OrderLineItem> _items = new();
    public IReadOnlyList<OrderLineItem> Items => _items;

    public Order(int orderNumber, Customer buyer)
    {
        OrderNumber = orderNumber;
        Buyer = buyer;
    }

    public void AddItem(string title, int qty, decimal price)
    {
        // Order сам конструює власні внутрішні частини (Композиція)
        _items.Add(new OrderLineItem(title, qty, price));
    }

    public decimal CalculateTotal() => _items.Sum(i => i.LineTotal);
}

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("==========================================================================");
        Console.WriteLine(" 01. ВЗАЄМОДІЯ КЛАСІВ (ЗВ'ЯЗОК HAS-A): КОМПОЗИЦІЯ ПРОТИ АГРЕГАЦІЇ");
        Console.WriteLine(" Як будувати складні доменні моделі з простих класів перед вивченням спадкування");
        Console.WriteLine("==========================================================================\n");

        // 1. Створюємо незалежний об'єкт Покупця (існує до і після замовлення -> Агрегація)
        var customer = new Customer("Іван Франко", "franko@lviv.ua");

        // 2. Створюємо Замовлення і передаємо туди Покупця
        var order = new Order(orderNumber: 501, buyer: customer);

        // 3. Замовлення всередині себе створює рядки позицій (Композиція)
        order.AddItem("Механічна клавіатура Keychron", qty: 1, price: 4_500m);
        order.AddItem("Бездротова миша Logitech MX", qty: 2, price: 3_200m);

        Console.WriteLine($"Замовлення #{order.OrderNumber} для клієнта: {order.Buyer.FullName} ({order.Buyer.Email})");
        Console.WriteLine("Склад замовлення (Композиція):");
        foreach (OrderLineItem item in order.Items)
        {
            Console.WriteLine($"  - {item.ProductTitle,-32} | {item.Quantity} шт. x {item.UnitPrice:N0} грн = {item.LineTotal:N0} грн");
        }
        Console.WriteLine($"Загальна сума: {order.CalculateTotal():N0} грн\n");

        Console.WriteLine("Висновок:");
        Console.WriteLine("  - Агрегація (Customer): об'єкт передано ззовні; якщо видалити Order, Customer залишиться.");
        Console.WriteLine("  - Композиція (OrderLineItem): об'єкт створено всередині Order і гине разом із ним.");
    }
}
