# Підтримка декількох СУБД, фабрики контекстів та міграції CLI

Entity Framework Core дозволяє одній і тій самій доменній моделі одночасно працювати з різними СУБД (наприклад, **PostgreSQL** та **Microsoft SQL Server**).

---

## 1. Архітектура з двома контекстами

У рішенні створено два окремих контексти:
1. `PostgresDbContext` — налаштований через провайдер `Npgsql.EntityFrameworkCore.PostgreSQL`.
2. `SqlServerDbContext` — налаштований через `Microsoft.EntityFrameworkCore.SqlServer`.

Обидва наслідують одну й ту саму схему через Fluent API, проте генерують власні діалекто-специфічні міграції.

---

## 2. Фабрики для часу проектування (`IDesignTimeDbContextFactory`)

Фабрика дозволяє інструментам CLI (`dotnet-ef`) створювати контекст бази даних без запуску всього додатку:

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

public class PostgresDbContextFactory : IDesignTimeDbContextFactory<PostgresDbContext>
{
    public PostgresDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<PostgresDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Database=VideoRentalDb;Username=postgres;Password=secret");

        return new PostgresDbContext(optionsBuilder.Options);
    }
}
```

---

## 3. Команди `dotnet-ef` CLI для управління міграціями

Для керування міграціями встановіть глобальний інструмент (якщо ще не встановлено):
```powershell
dotnet tool install --global dotnet-ef
```

### 3.1. Робота з PostgreSQL:
```powershell
# Створення нової міграції
dotnet-ef migrations add PostgreSqlInitialMigration --project CodeFirst.DataAccess --context PostgresDbContext

# Застосування міграцій до бази даних
dotnet-ef database update --project CodeFirst.DataAccess --context PostgresDbContext

# Видалення бази даних
dotnet-ef database drop --project CodeFirst.DataAccess --context PostgresDbContext
```

### 3.2. Робота з MS SQL Server:
```powershell
# Створення міграції
dotnet-ef migrations add SqlServerInitialMigration --project CodeFirst.DataAccess --context SqlServerDbContext

# Застосування міграцій
dotnet-ef database update --project CodeFirst.DataAccess --context SqlServerDbContext
```

---

## 4. Патерн Generic Repository

Для абстрагування бізнес-логіки від деталей EF Core реалізовано патерн Репозиторій:

```csharp
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}
```
Базова реалізація `BaseRepository<T>` містить стандартні операції над `DbSet<T>`.
