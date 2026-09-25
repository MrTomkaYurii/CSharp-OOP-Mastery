namespace EFCore.AdvancedMapping.Models;

/// <summary>
/// Value Object (Owned Entity) - Адреса клієнта, яка в БД зберігається в тій же таблиці Customers.
/// </summary>
public class Address
{
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = "Україна";
}

public class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    // Вбудований об'єкт-значення (Owned Type)
    public Address ShippingAddress { get; set; } = new();
}
