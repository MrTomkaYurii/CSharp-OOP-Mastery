# Розділ 3: Продуктивність, Масові Операції та Інтерцептори в EF Core

## 1. Прямі масові операції: `ExecuteUpdateAsync` та `ExecuteDeleteAsync`

До версії EF Core 7 для оновлення 1000 записів було необхідно:
1. Виконати SQL `SELECT` і завантажити 1000 об'єктів у пам'ять.
2. Поставити їх під відстеження Change Tracker.
3. Змінити поля об'єктів.
4. Викликати `SaveChangesAsync()`, що генерувало 1000 окремих або пачкованих SQL `UPDATE`.

### Сучасний підхід (.NET 7/8/9/10):
`ExecuteUpdateAsync` та `ExecuteDeleteAsync` генерують **один прямий SQL-запит** безпосередньо на стороні сервера баз даних:

```csharp
// Прямий SQL UPDATE без завантаження сутностей в пам'ять:
await db.Orders
    .Where(o => o.Status == "Pending" && o.CreatedAt < cutOffDate)
    .ExecuteUpdateAsync(s => s
        .SetProperty(o => o.Status, "Expired")
        .SetProperty(o => o.UpdatedAt, DateTime.UtcNow));

// Прямий SQL DELETE:
await db.Orders
    .Where(o => o.Status == "Cancelled")
    .ExecuteDeleteAsync();
```
Це підвищує швидкість масових модифікацій у 10-100 разів та суттєво зменшує споживання оперативної пам'яті (RAM).

---

## 2. Глобальні фільтри запитів (Global Query Filters) та Soft Delete

Для реалізації м'якого видалення (Soft Delete) або мультиорендності (Multi-tenancy) використовується `HasQueryFilter`:

```csharp
modelBuilder.Entity<Order>()
    .HasQueryFilter(o => !o.IsDeleted);
```
Кожен LINQ-запит автоматично доповнюється умовою `WHERE IsDeleted = 0`.

### Як отримати доступ до видалених записів?
Для адміністративних панелей або операцій відновлення використовується метод `IgnoreQueryFilters()`:
```csharp
var allOrdersIncludingDeleted = await db.Orders
    .IgnoreQueryFilters()
    .ToListAsync();
```

---

## 3. Оптимізація вибірок даних

### `AsNoTrackingWithIdentityResolution()`
Для вибірок лише для читання (`Read-only`) з декількома `Include`:
- `AsNoTracking()` вимикає трекер змін, значно прискорюючи запит.
- `AsNoTrackingWithIdentityResolution()` гарантує, що якщо один і той самий екземпляр зустрічається кілька разів у результатах, в пам'яті буде створено лише один C#-об'єкт.

### Запобігання Cartesian Explosion через `AsSplitQuery()`
Коли запит містить кілька зв'язків один-до-багатьох (`Include(o => o.Items).Include(o => o.Logs)`), реляційна база повертає декартовий добуток (Cartesian Product), дублюючи дані батьківської сутності.
Метод `AsSplitQuery()` розбиває вибірку на кілька швидких, незалежних SQL-запитів.

### Скомпільовані запити (`EF.CompileAsyncQuery`)
Для високонавантажених запитів, що виконуються тисячі разів на секунду, парсинг виразу LINQ та генерація SQL-дерева стає помітним вузьким місцем:
```csharp
private static readonly Func<PerformanceDbContext, string, Task<Order?>> GetByNumberCompiled =
    EF.CompileAsyncQuery((PerformanceDbContext ctx, string num) =>
        ctx.Orders.AsNoTracking().FirstOrDefault(o => o.OrderNumber == num));
```

---

## 4. Інтерцептори (`SaveChangesInterceptor`)

Інтерцептори дозволяють декларативно перехоплювати життєвий цикл роботи EF Core (виконання SQL команд, відкриття з'єднань, збереження змін).
Типовий приклад — автоматичний аудит сутностей:

```csharp
public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken ct = default)
    {
        var context = eventData.Context;
        if (context is not null)
        {
            foreach (var entry in context.ChangeTracker.Entries<IAuditable>())
            {
                if (entry.State == EntityState.Added)
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                else if (entry.State == EntityState.Modified)
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
            }
        }
        return base.SavingChangesAsync(eventData, result, ct);
    }
}
```
Інтерцептор підключається через `optionsBuilder.AddInterceptors(new AuditSaveChangesInterceptor())`.
