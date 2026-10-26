namespace DependencyInjection.Models;

/// <summary>
/// Статус життєвого циклу замовлення.
/// </summary>
public enum OrderStatus
{
    Pending,
    Paid,
    Shipped,
    Cancelled
}
