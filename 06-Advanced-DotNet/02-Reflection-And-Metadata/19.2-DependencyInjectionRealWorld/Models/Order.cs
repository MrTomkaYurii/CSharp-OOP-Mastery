namespace DependencyInjection.Models;

/// <summary>
/// Доменна модель оформленого замовлення.
/// </summary>
public class Order
{
    public string OrderNumber { get; set; } = string.Empty;
    public int CustomerId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Order()
    {
    }

    public Order(string orderNumber, int customerId, int productId, int quantity, decimal totalAmount)
    {
        OrderNumber = orderNumber;
        CustomerId = customerId;
        ProductId = productId;
        Quantity = quantity;
        TotalAmount = totalAmount;
        Status = OrderStatus.Pending;
        CreatedAt = DateTime.UtcNow;
    }

    public override string ToString() =>
        $"Замовлення #{OrderNumber} | Клієнт ID: {CustomerId} | Товар ID: {ProductId} (кількість: {Quantity}) | Сума: {TotalAmount:C} | Статус: {Status}";
}
