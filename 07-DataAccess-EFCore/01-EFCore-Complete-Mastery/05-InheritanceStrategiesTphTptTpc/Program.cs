using System.Text;
using Microsoft.EntityFrameworkCore;

namespace InheritanceStrategiesTphTptTpc;

// ============================================================================
// ПРОЄКТ 05: Стратегії успадкування ООП у БД (TPH, TPT, TPC) та Owned Types
// ============================================================================
// Однією з найважливіших тем ООП при роботі з реляційними БД є відображення
// ієрархії наслідування класів C# на таблиці бази даних (Глава «Наследование» на Metanit):
// 1. TPH (Table Per Hierarchy — за замовчуванням):
//    Вся ієрархія зберігається в ОДНІЙ таблиці з технічним стовпцем Discriminator.
//    Найшвидші запити (без JOIN та UNION), але специфічні стовпці похідних класів є NULLABLE.
// 2. TPT (Table Per Type — UseTptMappingStrategy()):
//    Базовий клас має свою таблицю, а кожен похідний клас — окрему таблицю зі своїми
//    властивостями та спільним Primary/Foreign Key. Нормалізована схема, але поліморфні
//    запити вимагають JOIN між таблицями.
// 3. TPC (Table Per Concrete Type — UseTpcMappingStrategy()):
//    Кожен конкретний (неабстрактний) клас отримує повністю незалежну таблицю з усіма
//    успадкованими стовпцями. Жодних NULL-стовпців і жодних JOIN при запиті конкретного типу!
// 4. Owned Entity Types (OwnsOne): вбудовані об'єкти-значення (Value Objects з DDD),
//    властивості яких зберігаються прямо в таблиці власника.
// ============================================================================

/// <summary>
/// Вбудований тип-значення (Value Object) для фінансової винагороди.
/// Моделюється через OwnsOne — не має власного Id і зберігається в таблиці власника.
/// </summary>
public class CompensationPackage
{
    public decimal BaseAmount { get; set; }
    public string Currency { get; set; } = "UAH";
}

// ----------------------------------------------------------------------------
// Ієрархія 1: TPH (Table Per Hierarchy) — Стратегія за замовчуванням
// ----------------------------------------------------------------------------
public abstract class TphStaffMember
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public CompensationPackage Compensation { get; set; } = new();
}

public class TphFullTimeDeveloper : TphStaffMember
{
    public string PrimaryStack { get; set; } = ".NET 10";
    public int AnnualBonusPercent { get; set; }
}

public class TphExternalContractor : TphStaffMember
{
    public string AgencyName { get; set; } = string.Empty;
    public DateOnly ContractEndDate { get; set; }
}

// ----------------------------------------------------------------------------
// Ієрархія 2: TPT (Table Per Type) — Нормалізовані таблиці зі спільним PK/FK
// ----------------------------------------------------------------------------
public abstract class TptVehicle
{
    public int Id { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
}

public class TptElectricCar : TptVehicle
{
    public int BatteryCapacityKwh { get; set; }
    public int RangeKm { get; set; }
}

public class TptDieselTruck : TptVehicle
{
    public double MaxPayloadTons { get; set; }
    public int AxlesCount { get; set; }
}

// ----------------------------------------------------------------------------
// Ієрархія 3: TPC (Table Per Concrete Type) — Незалежні таблиці для кожного класу
// ----------------------------------------------------------------------------
public abstract class TpcPaymentMethod
{
    // Для TPC у SQLite використовуємо Guid або явно задані ключі, оскільки в SQLite немає об'єктів SEQUENCE
    public Guid Id { get; set; } = Guid.NewGuid();
    public string OwnerName { get; set; } = string.Empty;
}

public class TpcBankCardPayment : TpcPaymentMethod
{
    public string MaskedCardNumber { get; set; } = string.Empty;
    public string BankName { get; set; } = string.Empty;
}

public class TpcCryptoWalletPayment : TpcPaymentMethod
{
    public string Network { get; set; } = string.Empty;
    public string WalletAddress { get; set; } = string.Empty;
}

public class InheritanceDbContext : DbContext
{
    // TPH DbSets
    public DbSet<TphStaffMember> TphStaff => Set<TphStaffMember>();
    public DbSet<TphFullTimeDeveloper> TphDevelopers => Set<TphFullTimeDeveloper>();
    public DbSet<TphExternalContractor> TphContractors => Set<TphExternalContractor>();

    // TPT DbSets
    public DbSet<TptVehicle> TptVehicles => Set<TptVehicle>();
    public DbSet<TptElectricCar> TptElectricCars => Set<TptElectricCar>();
    public DbSet<TptDieselTruck> TptDieselTrucks => Set<TptDieselTruck>();

    // TPC DbSets
    public DbSet<TpcPaymentMethod> TpcPayments => Set<TpcPaymentMethod>();
    public DbSet<TpcBankCardPayment> TpcBankCards => Set<TpcBankCardPayment>();
    public DbSet<TpcCryptoWalletPayment> TpcCryptoWallets => Set<TpcCryptoWalletPayment>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=oop_inheritance.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ====================================================================
        // 1. Налаштування TPH (Table Per Hierarchy) + OwnsOne
        // ====================================================================
        modelBuilder.Entity<TphStaffMember>(builder =>
        {
            builder.ToTable("TphStaffMembers");

            // Налаштовуємо кастомну назву стовпця-дискримінатора та його значення
            builder.HasDiscriminator<string>("MemberRoleType")
                .HasValue<TphFullTimeDeveloper>("InHouseDeveloper")
                .HasValue<TphExternalContractor>("Contractor");

            // Вбудований Value Object (Owned Type) — стовпці Compensation_BaseAmount і Compensation_Currency
            // будуть створені прямо всередині таблиці TphStaffMembers
            builder.OwnsOne(s => s.Compensation, comp =>
            {
                comp.Property(c => c.BaseAmount).HasColumnName("SalaryAmount");
                comp.Property(c => c.Currency).HasColumnName("SalaryCurrency").HasMaxLength(3);
            });
        });

        // ====================================================================
        // 2. Налаштування TPT (Table Per Type)
        // ====================================================================
        modelBuilder.Entity<TptVehicle>().UseTptMappingStrategy().ToTable("TptVehicles");
        modelBuilder.Entity<TptElectricCar>().ToTable("TptElectricCars");
        modelBuilder.Entity<TptDieselTruck>().ToTable("TptDieselTrucks");

        // ====================================================================
        // 3. Налаштування TPC (Table Per Concrete Type)
        // ====================================================================
        modelBuilder.Entity<TpcPaymentMethod>().UseTpcMappingStrategy();
        modelBuilder.Entity<TpcBankCardPayment>().ToTable("TpcBankCards");
        modelBuilder.Entity<TpcCryptoWalletPayment>().ToTable("TpcCryptoWallets");
    }
}

public static class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("================================================================================");
        Console.WriteLine(" 05. Стратегії успадкування ООП у БД (TPH, TPT, TPC) та Owned Types");
        Console.WriteLine("================================================================================\n");

        await using var context = new InheritanceDbContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        // Показуємо DDL-скрипт, щоб наочно порівняти структуру таблиць TPH, TPT і TPC
        Console.WriteLine("--- КРОК 1: Порівняння DDL-схем таблиць для TPH, TPT та TPC ---");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(context.Database.GenerateCreateScript());
        Console.ResetColor();

        // Заповнюємо дані для всіх трьох ієрархій
        context.TphStaff.AddRange(
            new TphFullTimeDeveloper
            {
                FullName = "Наталія Сидоренко",
                PrimaryStack = "C# / ASP.NET Core",
                AnnualBonusPercent = 20,
                Compensation = new CompensationPackage { BaseAmount = 145000m, Currency = "UAH" }
            },
            new TphExternalContractor
            {
                FullName = "Дмитро Ткаченко",
                AgencyName = "DevSecOps Consulting LLC",
                ContractEndDate = new DateOnly(2026, 11, 30),
                Compensation = new CompensationPackage { BaseAmount = 4200m, Currency = "USD" }
            });

        context.TptVehicles.AddRange(
            new TptElectricCar { LicensePlate = "KA 7777 YA", Brand = "Rivian R1T", BatteryCapacityKwh = 135, RangeKm = 510 },
            new TptDieselTruck { LicensePlate = "BC 4512 CI", Brand = "Volvo FH16", MaxPayloadTons = 24.5, AxlesCount = 4 });

        context.TpcPayments.AddRange(
            new TpcBankCardPayment { OwnerName = "Юрій Петренко", BankName = "Monobank", MaskedCardNumber = "5375 **** **** 8821" },
            new TpcCryptoWalletPayment { OwnerName = "Юрій Петренко", Network = "Ethereum (ERC-20)", WalletAddress = "0x71C...9A4B" });

        await context.SaveChangesAsync();

        // --------------------------------------------------------------------
        // Поліморфні запити та фільтрація за типом OfType<TDerived>()
        // --------------------------------------------------------------------
        Console.WriteLine("--- КРОК 2: Поліморфний запит TPH (одна таблиця, стовпець MemberRoleType + Owned Type) ---");
        var tphQuery = context.TphStaff.AsNoTracking();
        Console.WriteLine("SQL TPH: " + tphQuery.ToQueryString().Replace(Environment.NewLine, " "));
        foreach (var member in await tphQuery.ToListAsync())
        {
            Console.WriteLine($"   * [{member.GetType().Name}] {member.FullName} — Винагорода: {member.Compensation.BaseAmount} {member.Compensation.Currency}");
        }

        Console.WriteLine("\n--- КРОК 3: Поліморфний запит TPT (LEFT JOIN між базовою та похідними таблицями) ---");
        var tptQuery = context.TptVehicles.AsNoTracking();
        Console.WriteLine("SQL TPT: " + tptQuery.ToQueryString().Replace(Environment.NewLine, " "));
        foreach (var vehicle in await tptQuery.ToListAsync())
        {
            Console.WriteLine($"   * [{vehicle.GetType().Name}] {vehicle.Brand} ({vehicle.LicensePlate})");
        }

        Console.WriteLine("\n--- КРОК 4: Поліморфний запит TPC (UNION ALL між незалежними таблицями) та OfType<T>() ---");
        var tpcPolymorphicQuery = context.TpcPayments.AsNoTracking();
        Console.WriteLine("SQL TPC (всі типи): " + tpcPolymorphicQuery.ToQueryString().Replace(Environment.NewLine, " "));

        // А при запиті конкретного похідного типу TPC звертається ТІЛЬКИ до однієї таблиці без JOIN і без UNION!
        var onlyCardsQuery = context.TpcPayments.OfType<TpcBankCardPayment>().AsNoTracking();
        Console.WriteLine("SQL TPC (OfType<TpcBankCardPayment>): " + onlyCardsQuery.ToQueryString().Replace(Environment.NewLine, " "));
        foreach (var card in await onlyCardsQuery.ToListAsync())
        {
            Console.WriteLine($"   * Картка: {card.BankName} ({card.MaskedCardNumber}), Власник: {card.OwnerName}");
        }
    }
}
