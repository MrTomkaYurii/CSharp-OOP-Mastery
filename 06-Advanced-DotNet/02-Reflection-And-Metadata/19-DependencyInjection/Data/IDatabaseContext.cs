using DependencyInjection.Models;

namespace DependencyInjection.Data;

/// <summary>
/// Контракт контексту бази даних.
/// Забезпечує структурований доступ до сутностей системи та фіксацію змін.
/// </summary>
public interface IDatabaseContext
{
    /// <summary>
    /// Колекція доступних товарів.
    /// </summary>
    ICollection<Product> Products { get; }

    /// <summary>
    /// Колекція зареєстрованих клієнтів.
    /// </summary>
    ICollection<Customer> Customers { get; }

    /// <summary>
    /// Колекція збережених замовлень.
    /// </summary>
    ICollection<Order> Orders { get; }

    /// <summary>
    /// Фіксує зміни в контексті бази даних.
    /// </summary>
    void SaveChanges();
}
