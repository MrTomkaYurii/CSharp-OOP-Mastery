namespace DependencyInjection.Models;

/// <summary>
/// Доменна модель покупця.
/// </summary>
public class Customer
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Balance { get; set; }

    public Customer()
    {
    }

    public Customer(int id, string fullName, string email, decimal balance)
    {
        Id = id;
        FullName = fullName;
        Email = email;
        Balance = balance;
    }

    public override string ToString() => $"[Клієнт #{Id}] {FullName} ({Email}), Баланс: {Balance:C}";
}
