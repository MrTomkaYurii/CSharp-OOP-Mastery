using Microsoft.EntityFrameworkCore;
using Testing.Core.Domain.Data;
using Testing.EFCore.InMemoryTests.Data;

namespace Testing.EFCore.InMemoryTests.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly TestShopDbContext _context;

    public CustomerRepository(TestShopDbContext context)
    {
        _context = context;
    }

    public async Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task AddAsync(Customer customer, CancellationToken ct = default)
    {
        ArgumentNullException.ThrowIfNull(customer);
        await _context.Customers.AddAsync(customer, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task<IReadOnlyList<Customer>> GetTopByLoyaltyAsync(int count, CancellationToken ct = default)
    {
        return await _context.Customers
            .AsNoTracking()
            .OrderByDescending(c => c.LoyaltyPoints)
            .Take(count)
            .ToListAsync(ct);
    }
}
