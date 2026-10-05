namespace Testing.Core.Domain.Data;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal Balance { get; set; }
    public int LoyaltyPoints { get; set; }
}

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(Customer customer, CancellationToken ct = default);
    Task<IReadOnlyList<Customer>> GetTopByLoyaltyAsync(int count, CancellationToken ct = default);
}
