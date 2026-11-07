using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace RawSqlTransactionsAndConcurrency;

// ============================================================================
// ПРОЄКТ 06: Сирі SQL-запити, Транзакції, Токени паралелізму та Пакетні операції
// ============================================================================
// Завершальний фундаментальний проєкт охоплює просунуті механізми роботи з БД за Metanit:
// 1. Сирі SQL-запити (Raw SQL) з автоматичною параметризацією (захист від SQL Injection):
//    - FromSqlInterpolated / FromSql (для сутностей з композицією LINQ Where/OrderBy)
//    - Database.SqlQuery<T> (для довільних немодельних DTO та скалярних типів, з EF Core 8+)
//    - Database.ExecuteSqlInterpolatedAsync (для команд UPDATE / DELETE / INSERT).
// 2. Явне керування транзакціями (Database.BeginTransactionAsync) та точками збереження
//    (CreateSavepointAsync / RollbackToSavepointAsync).
// 3. Оптимістичний паралелізм (Optimistic Concurrency Control):
//    Атрибут [ConcurrencyCheck], моделювання гонитви двох користувачів та розв'язання
//    конфлікту через перехоплення DbUpdateConcurrencyException.
// 4. Глобальні фільтри запитів (HasQueryFilter для Soft Delete + IgnoreQueryFilters)
//    та швидкі пакетні операції ExecuteUpdateAsync / ExecuteDeleteAsync.
// ============================================================================

public class BankAccount
{
    public int Id { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string Owner { get; set; } = string.Empty;

    /// <summary>
    /// Захищаємо баланс від втраченого оновлення (Lost Update) за допомогою токена паралелізму.
    /// При кожному UPDATE EF Core перевірятиме у WHERE, чи не змінився Balance або VersionTag у БД з моменту читання!
    /// </summary>
    [ConcurrencyCheck]
    public decimal Balance { get; set; }

    [ConcurrencyCheck]
    public Guid VersionToken { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Прапорець «м'якого видалення» (Soft Delete), який фільтрується глобальним фільтром HasQueryFilter.
    /// </summary>
    public bool IsArchived { get; set; }
}

/// <summary>
/// Немодельний тип (не зареєстрований як DbSet) для отримання результатів сирого SQL через Database.SqlQuery<T>.
/// </summary>
public record AccountAuditRow(string AccountNumber, string Owner, decimal Balance);

public class BankingDbContext : DbContext
{
    public DbSet<BankAccount> Accounts => Set<BankAccount>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=banking_advanced.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BankAccount>(builder =>
        {
            builder.ToTable("BankAccounts");
            builder.Property(a => a.Balance).HasColumnType("decimal(18,2)");

            // Глобальний фільтр запитів: всі LINQ-запити автоматично відкидатимуть архівні рахунки (WHERE IsArchived = 0)
            builder.HasQueryFilter(a => !a.IsArchived);
        });
    }
}

public static class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("================================================================================");
        Console.WriteLine(" 06. Сирі SQL-запити, Транзакції, Токени паралелізму та Пакетні операції");
        Console.WriteLine("================================================================================\n");

        await SeedBankAccountsAsync();

        // --------------------------------------------------------------------
        // 1. Сирі SQL-запити: FromSqlInterpolated та Database.SqlQuery<T>
        // --------------------------------------------------------------------
        Console.WriteLine("--- КРОК 1: Безпечні параметризовані SQL-запити (FromSqlInterpolated та SqlQuery<T>) ---");
        await using (var context = new BankingDbContext())
        {
            decimal minBalance = 20000m;

            // Інтерпольований рядок автоматично перетворюється на безпечний DbParameter (@p0)!
            // Більше того, поверх FromSqlInterpolated можна добудовувати LINQ-оператори (OrderBy, Where):
            var richAccounts = await context.Accounts
                .FromSqlInterpolated($"SELECT * FROM BankAccounts WHERE Balance >= {minBalance}")
                .OrderByDescending(a => a.Balance)
                .AsNoTracking()
                .ToListAsync();

            Console.WriteLine($"Рахунки з балансом >= {minBalance} грн (через FromSqlInterpolated + LINQ OrderBy):");
            foreach (var acc in richAccounts)
            {
                Console.WriteLine($"   * {acc.AccountNumber} ({acc.Owner}) — {acc.Balance} грн");
            }

            // Database.SqlQuery<T> дозволяє виконувати довільний SQL (наприклад, агрегацію або проєкцію у рекорд)
            var auditRows = await context.Database
                .SqlQuery<AccountAuditRow>($"SELECT AccountNumber, Owner, Balance FROM BankAccounts WHERE IsArchived = 0")
                .ToListAsync();
            Console.WriteLine($"Отримано {auditRows.Count} рядків аудиту через немодельний Database.SqlQuery<AccountAuditRow>().");
        }

        // --------------------------------------------------------------------
        // 2. Транзакції та Точки збереження (Savepoints)
        // --------------------------------------------------------------------
        Console.WriteLine("\n--- КРОК 2: Явні транзакції та відкат до точки збереження (RollbackToSavepointAsync) ---");
        await using (var context = new BankingDbContext())
        {
            await using var transaction = await context.Database.BeginTransactionAsync();

            var sender = await context.Accounts.FirstAsync(a => a.AccountNumber == "UA-ACC-001");
            var receiver = await context.Accounts.FirstAsync(a => a.AccountNumber == "UA-ACC-002");

            Console.WriteLine($"Баланси до переказу: Відправник = {sender.Balance} грн, Отримувач = {receiver.Balance} грн");

            // Етап 1: Успішний основний переказ 5 000 грн
            sender.Balance -= 5000m;
            sender.VersionToken = Guid.NewGuid();
            receiver.Balance += 5000m;
            receiver.VersionToken = Guid.NewGuid();
            await context.SaveChangesAsync();

            // Створюємо точку збереження (Savepoint) всередині відкритої транзакції
            await transaction.CreateSavepointAsync("AfterMainTransfer");
            Console.WriteLine("Створено точку збереження транзакції 'AfterMainTransfer'.");

            // Етап 2: Ризикована додаткова операція, яку ми вирішимо скасувати
            await context.Database.ExecuteSqlInterpolatedAsync(
                $"UPDATE BankAccounts SET Balance = Balance - 999999 WHERE AccountNumber = {sender.AccountNumber}");

            // Відкочуємо лише другу (помилкову) операцію до точки 'AfterMainTransfer', зберігаючи перший переказ!
            await transaction.RollbackToSavepointAsync("AfterMainTransfer");
            Console.WriteLine("Виконано RollbackToSavepointAsync('AfterMainTransfer') — помилкове списання скасовано.");

            await transaction.CommitAsync();
        }

        await using (var verifyCtx = new BankingDbContext())
        {
            var s = await verifyCtx.Accounts.FirstAsync(a => a.AccountNumber == "UA-ACC-001");
            var r = await verifyCtx.Accounts.FirstAsync(a => a.AccountNumber == "UA-ACC-002");
            Console.WriteLine($"Баланси після CommitAsync(): Відправник = {s.Balance} грн, Отримувач = {r.Balance} грн");
        }

        // --------------------------------------------------------------------
        // 3. Оптимістичний паралелізм (ConcurrencyCheck та DbUpdateConcurrencyException)
        // --------------------------------------------------------------------
        Console.WriteLine("\n--- КРОК 3: Моделювання конфлікту паралельного доступу (Optimistic Concurrency) ---");
        await SimulateConcurrencyConflictAsync();

        // --------------------------------------------------------------------
        // 4. Глобальні фільтри (HasQueryFilter / IgnoreQueryFilters) та ExecuteUpdateAsync
        // --------------------------------------------------------------------
        Console.WriteLine("\n--- КРОК 4: Глобальний фільтр (Soft Delete) та пакетне нарахування відсотків (ExecuteUpdateAsync) ---");
        await using (var context = new BankingDbContext())
        {
            int visibleCount = await context.Accounts.CountAsync();
            int totalWithArchivedCount = await context.Accounts.IgnoreQueryFilters().CountAsync();

            Console.WriteLine($"Звичайний запит (діє HasQueryFilter !IsArchived): видно {visibleCount} рахунків.");
            Console.WriteLine($"Запит з IgnoreQueryFilters(): всього у таблиці {totalWithArchivedCount} рахунків (включно з архівними).");

            // Пакетне нарахування +5% річних усім активним рахункам одним SQL UPDATE без завантаження сутностей у пам'ять!
            int updatedCount = await context.Accounts
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(a => a.Balance, a => a.Balance * 1.05m));

            Console.WriteLine($"ExecuteUpdateAsync нарахував +5% на {updatedCount} активних рахунків одним SQL-запитом.");
        }
    }

    private static async Task SimulateConcurrencyConflictAsync()
    {
        // Два окремі контексти імітують двох операторів банку, які одночасно відкрили той самий рахунок
        await using var operator1Context = new BankingDbContext();
        await using var operator2Context = new BankingDbContext();

        var accUser1 = await operator1Context.Accounts.FirstAsync(a => a.AccountNumber == "UA-ACC-001");
        var accUser2 = await operator2Context.Accounts.FirstAsync(a => a.AccountNumber == "UA-ACC-001");

        // Оператор 1 списує 1 000 грн і першим зберігає зміни в БД
        accUser1.Balance -= 1000m;
        accUser1.VersionToken = Guid.NewGuid();
        await operator1Context.SaveChangesAsync();
        Console.WriteLine($"   Оператор 1 успішно зберіг новий баланс: {accUser1.Balance} грн.");

        // Оператор 2 (у якого в пам'яті ще старий баланс і старий VersionToken) намагається додати 2 000 грн
        accUser2.Balance += 2000m;
        accUser2.VersionToken = Guid.NewGuid();

        try
        {
            await operator2Context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("   [Перехоплено DbUpdateConcurrencyException]: Дані рахунку були змінені іншим користувачем!");
            Console.ResetColor();

            // Отримуємо актуальні значення з БД (Database Values) і поєднуємо зміни (Client Merge)
            var entry = operator2Context.Entry(accUser2);
            var databaseValues = await entry.GetDatabaseValuesAsync();
            if (databaseValues is not null)
            {
                decimal currentDbBalance = databaseValues.GetValue<decimal>(nameof(BankAccount.Balance));
                Console.WriteLine($"   Розв'язання конфлікту: актуальний баланс у БД = {currentDbBalance} грн. Додаємо +2000 грн поверх нього.");

                // Оновлюємо OriginalValues до актуальних даних БД і застосовуємо коректний новий баланс
                entry.OriginalValues.SetValues(databaseValues);
                accUser2.Balance = currentDbBalance + 2000m;
                accUser2.VersionToken = Guid.NewGuid();

                await operator2Context.SaveChangesAsync();
                Console.WriteLine($"   Повторне збереження після вирішення конфлікту успішне! Підсумковий баланс: {accUser2.Balance} грн.");
            }
        }
    }

    private static async Task SeedBankAccountsAsync()
    {
        await using var context = new BankingDbContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        context.Accounts.AddRange(
            new BankAccount { AccountNumber = "UA-ACC-001", Owner = "Тарас Кравчук", Balance = 50000m, IsArchived = false },
            new BankAccount { AccountNumber = "UA-ACC-002", Owner = "Олена Савченко", Balance = 25000m, IsArchived = false },
            new BankAccount { AccountNumber = "UA-ACC-999", Owner = "Закритий ФОП (Архів)", Balance = 0m, IsArchived = true }
        );

        await context.SaveChangesAsync();
    }
}
