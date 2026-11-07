# 06. Сирі SQL-запити, Транзакції, Токени паралелізму та Пакетні операції

## 1. Виконання сирого SQL (Raw SQL) та захист від SQL-ін'єкцій

Коли запит занадто специфічний або використовує віконні функції/процедури СУБД, EF Core надає три безпечні інструменти, що приймають `FormattableString` і автоматично перетворюють змінні C# у параметри `DbParameter` (`@p0`, `@p1`):

1. **`dbSet.FromSqlInterpolated($"SELECT * FROM BankAccounts WHERE Balance >= {minBalance}")`** (або `FromSql`) — повертає `IQueryable<TEntity>`. Головна перевага: поверх цього сирого SQL можна продовжувати писати LINQ-вирази (`.Where(...)`, `.OrderByDescending(...)`, `.Include(...)`), які EF Core скомпонує в єдиний підзапит!
2. **`context.Database.SqlQuery<TResult>($"SELECT AccountNumber, Owner, Balance FROM ...")`** — виконує довільний SQL і мапить результат на **немодельний тип** (наприклад, `record AccountAuditRow` або скалярний `int`/`decimal`), який не потрібно реєструвати у `DbContext`.
3. **`await context.Database.ExecuteSqlInterpolatedAsync($"UPDATE ... WHERE Id = {id}")`** — виконує команди модифікації даних (`UPDATE`, `DELETE`, `INSERT`) і повертає кількість змінених рядків.

---

## 2. Явні транзакції та точки збереження (`Savepoints`)

За замовчуванням кожен виклик `SaveChangesAsync()` вже виконується у власній транзакції. Проте якщо бізнес-операція складається з декількох етапів (`SaveChangesAsync()` + сирий `ExecuteSqlInterpolatedAsync()`), транзакцією керують явно:

```csharp
await using var transaction = await context.Database.BeginTransactionAsync();
await context.SaveChangesAsync();

// Створюємо контрольну точку всередині транзакції
await transaction.CreateSavepointAsync("AfterMainTransfer");

// Якщо наступний крок не вдався — відкочуємо лише його, не втрачаючи попередніх змін!
await transaction.RollbackToSavepointAsync("AfterMainTransfer");

await transaction.CommitAsync();
```

---

## 3. Оптимістичний паралелізм (`[ConcurrencyCheck]` та `DbUpdateConcurrencyException`)

За замовчуванням у EF Core діє правило **«Last In Wins»** (останній перезаписує попередні зміни). У фінансових та складських системах це неприпустимо (проблема *Lost Update*).
Позначивши критичні властивості атрибутом **`[ConcurrencyCheck]`** (або `[Timestamp]` для `byte[] RowVersion` у SQL Server), ми змушуємо EF Core додавати старе значення у секцію `WHERE` під час оновлення:
```sql
UPDATE "BankAccounts" SET "Balance" = @p0, "VersionToken" = @p1
WHERE "Id" = @p2 AND "Balance" = @p3 AND "VersionToken" = @p4;
```
Якщо інший оператор встиг змінити рахунок на долю секунди раніше, `UPDATE` поверне `0` змінених рядків, і EF Core викине виняток **`DbUpdateConcurrencyException`**. У блоці `catch` ми можемо прочитати актуальні дані з БД через `await entry.GetDatabaseValuesAsync()` і коректно об'єднати зміни.

---

## 4. Глобальні фільтри (`HasQueryFilter`) та пакетні операції (`ExecuteUpdateAsync`)

- **`builder.HasQueryFilter(a => !a.IsArchived)`** — автоматично додає умову `WHERE IsArchived = 0` до всіх LINQ-запитів у застосунку (ідеально для **Soft Delete** та **Multi-tenancy**). Щоб побачити архівні записи в адмін-панелі, достатньо додати `.IgnoreQueryFilters()`.
- **`ExecuteUpdateAsync` та `ExecuteDeleteAsync`** — виконують масовий `UPDATE` або `DELETE` на боці сервера БД одним запитом без завантаження тисяч сутностей в оперативну пам'ять.
