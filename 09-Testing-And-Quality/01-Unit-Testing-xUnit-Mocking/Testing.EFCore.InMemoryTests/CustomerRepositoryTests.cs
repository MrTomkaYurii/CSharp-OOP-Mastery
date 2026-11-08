using Bogus;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Testing.Core.Domain.Data;
using Testing.EFCore.InMemoryTests.Data;
using Testing.EFCore.InMemoryTests.Repositories;
using Xunit;

namespace Testing.EFCore.InMemoryTests;

/// <summary>
/// Інтеграційне тестування репозиторіїв з реальною реляційною базою даних
/// через ізольоване SQLite In-Memory з'єднання та Bogus тестові дані.
/// </summary>
public class CustomerRepositoryTests : IAsyncLifetime, IDisposable
{
    private SqliteConnection _connection = null!;
    private TestShopDbContext _context = null!;
    private CustomerRepository _repository = null!;

    public async Task InitializeAsync()
    {
        // Для In-Memory SQLite з'єднання повинно залишатися відкритим протягом тесту,
        // інакше база буде видалена з пам'яті операційної системи.
        _connection = new SqliteConnection("DataSource=:memory:");
        await _connection.OpenAsync();

        var options = new DbContextOptionsBuilder<TestShopDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new TestShopDbContext(options);
        await _context.Database.EnsureCreatedAsync();

        _repository = new CustomerRepository(_context);
    }

    public async Task DisposeAsync()
    {
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }

    public void Dispose()
    {
        _context?.Dispose();
        _connection?.Dispose();
        GC.SuppressFinalize(this);
    }

    [Fact]
    public async Task AddAsync_ValidCustomer_PersistsCustomerInDatabase()
    {
        // Arrange
        var customer = new Customer
        {
            Name = "Андрій Мельник",
            Email = "andriy.melnyk@example.ua",
            Balance = 1500m,
            LoyaltyPoints = 120
        };

        // Act
        await _repository.AddAsync(customer);

        // Assert
        customer.Id.Should().BeGreaterThan(0);
        var found = await _repository.GetByIdAsync(customer.Id);
        found.Should().NotBeNull();
        found!.Name.Should().Be("Андрій Мельник");
        found.Email.Should().Be("andriy.melnyk@example.ua");
    }

    [Fact]
    public async Task AddAsync_DuplicateEmail_ThrowsDbUpdateExceptionDueToUniqueConstraint()
    {
        // Arrange
        var c1 = new Customer { Name = "Клієнт 1", Email = "same@ex.ua", Balance = 100m };
        var c2 = new Customer { Name = "Клієнт 2", Email = "same@ex.ua", Balance = 200m };

        await _repository.AddAsync(c1);

        // Act
        Func<Task> act = async () => await _repository.AddAsync(c2);

        // Assert - перевірка дотримання реляційного обмеження унікальності в SQLite In-Memory
        await act.Should().ThrowAsync<DbUpdateException>();
    }

    [Fact]
    public async Task GetTopByLoyaltyAsync_UsingBogusDataset_ReturnsSortedTopCustomers()
    {
        // Arrange: генеруємо 20 правдоподібних клієнтів за допомогою Bogus
        var faker = new Faker<Customer>("uk")
            .RuleFor(c => c.Name, f => f.Name.FullName())
            .RuleFor(c => c.Email, (f, c) => f.Internet.Email(f.Random.AlphaNumeric(6), c.Name))
            .RuleFor(c => c.Balance, f => f.Random.Decimal(100m, 50000m))
            .RuleFor(c => c.LoyaltyPoints, f => f.Random.Int(10, 1000));

        var testData = faker.Generate(20);
        await _context.Customers.AddRangeAsync(testData);
        await _context.SaveChangesAsync();

        // Act
        var top5 = await _repository.GetTopByLoyaltyAsync(5);

        // Assert
        top5.Should().HaveCount(5);
        top5.Should().BeInDescendingOrder(c => c.LoyaltyPoints);
    }
}
