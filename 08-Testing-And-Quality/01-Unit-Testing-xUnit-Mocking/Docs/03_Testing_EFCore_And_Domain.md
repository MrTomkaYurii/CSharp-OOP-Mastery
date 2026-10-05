# Розділ 3: Тестування Бази Даних (EF Core, SQLite In-Memory та Bogus)

## 1. Дилема тестування доступу до даних: InMemory vs SQLite In-Memory

При тестуванні шару доступу до даних (Data Access Layer / Repositories) розробники часто обирають між трьома підходами:

| Підхід | Переваги | Недоліки | Рекомендація |
| :--- | :--- | :--- | :--- |
| **EF Core InMemory Provider** | Працює швидко, не потребує налаштувань. | **Не є реляційною СУБД!** Ігнорує зовнішні ключі (FK), унікальні індекси, каскадні видалення, не підтримує нативний SQL чи транзакції. | **Не рекомендовано** для серйозних тестів. |
| **SQLite In-Memory (`DataSource=:memory:`)** | Повноцінна реляційна база, перевіряє FK, Unique Constraints, транзакції, працює миттєво в RAM без зовнішніх процесів. | Має незначні відмінності в синтаксисі SQL від PostgreSQL / SQL Server. | **Золотий стандарт** для швидких інтеграційних тестів репозиторіїв. |
| **Testcontainers (Docker)** | 100% відповідність продакшн СУБД (реальний PostgreSQL / SQL Server). | Потребує запущеного Docker демона, запуск контейнера займає кілька секунд. | Ідеально для фінальних End-to-End тестів на CI/CD. |

---

## 2. Налаштування SQLite In-Memory з `IAsyncLifetime` в xUnit

Особливість SQLite In-Memory: **база даних живе доти, доки відкрито активне з'єднання `SqliteConnection`**. Якщо закрити з'єднання, база повністю зникає з RAM.

Тому ми використовуємо інтерфейс `IAsyncLifetime`:

```csharp
public class CustomerRepositoryTests : IAsyncLifetime
{
    private SqliteConnection _connection = null!;
    private TestShopDbContext _context = null!;

    public async Task InitializeAsync()
    {
        // 1. Відкриваємо та утримуємо з'єднання
        _connection = new SqliteConnection("DataSource=:memory:");
        await _connection.OpenAsync();

        // 2. Будуємо DbContext на базі відкритого з'єднання
        var options = new DbContextOptionsBuilder<TestShopDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new TestShopDbContext(options);

        // 3. Створюємо схему БД в оперативній пам'яті
        await _context.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        // 4. Очищення ресурсів після завершення тесту
        await _context.DisposeAsync();
        await _connection.DisposeAsync();
    }
}
```

---

## 3. Генерація тестових фікстур через Bogus у тестах

Замість створення вручну десятків сутностей, підключаємо `Bogus.Faker<T>`:

```csharp
[Fact]
public async Task GetTopCustomers_ReturnsOrderedResults()
{
    var faker = new Faker<Customer>("uk")
        .RuleFor(c => c.Name, f => f.Name.FullName())
        .RuleFor(c => c.Email, (f, c) => f.Internet.Email(f.Random.AlphaNumeric(6), c.Name))
        .RuleFor(c => c.LoyaltyPoints, f => f.Random.Int(10, 1000));

    var generatedCustomers = faker.Generate(25);
    await _context.Customers.AddRangeAsync(generatedCustomers);
    await _context.SaveChangesAsync();

    var top = await _repository.GetTopByLoyaltyAsync(5);

    top.Should().HaveCount(5);
    top.Should().BeInDescendingOrder(c => c.LoyaltyPoints);
}
```

---

## 4. Перевірка реляційних обмежень у тестах

Оскільки SQLite In-Memory є справжньою реляційною СУБД, ми можемо перевірити, що обмеження унікальності `Unique Index` дійсно працюють:

```csharp
[Fact]
public async Task Add_DuplicateEmail_ThrowsDbUpdateException()
{
    var c1 = new Customer { Name = "Іван", Email = "duplicate@ex.ua" };
    var c2 = new Customer { Name = "Петро", Email = "duplicate@ex.ua" };

    await _repository.AddAsync(c1);

    Func<Task> act = async () => await _repository.AddAsync(c2);

    // Доводить, що схема бази захищає від дублікатів поштового індексу
    await act.Should().ThrowAsync<DbUpdateException>();
}
```
