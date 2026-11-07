using System.Text;
using Microsoft.EntityFrameworkCore;

namespace RelationshipsAndLoadingStrategies;

// ============================================================================
// ПРОЄКТ 03: Зв'язки сутностей (1:1, 1:N, N:M), Каскадне видалення та Завантаження даних
// ============================================================================
// Цей приклад охоплює ключові теми зв'язків та навігаційних властивостей за Metanit:
// 1. Зв'язок Один-до-Одного (1:1): Employee <-> EmployeePassport (Principal та Dependent).
// 2. Зв'язок Один-до-Багатьох (1:N): Department -> Employees, та необов'язковий зв'язок Mentor -> MentoredEmployees.
// 3. Зв'язок Багато-до-Багатьох (N:M) з явною проміжною сутністю та корисним навантаженням (Payload):
//    Employee <-> ProjectAssignment (Role, HoursAllocated, AssignedAt) <-> Project.
// 4. Поведінка при видаленні (DeleteBehavior): Cascade, SetNull, Restrict.
// 5. Три стратегії завантаження пов'язаних даних:
//    - Eager Loading (Include / ThenInclude / Filtered Include)
//    - Explicit Loading (Entry(...).Collection(...).LoadAsync() та Query())
//    - Порівняння з Lazy Loading та пояснення проблеми N+1 запитів.
// ============================================================================

/// <summary>
/// Головна сутність у зв'язку 1:N (Відділ компанії).
/// </summary>
public class Department
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    // Навігаційна колекція співробітників (1:N)
    public List<Employee> Employees { get; set; } = new();
}

/// <summary>
/// Сутність Співробітника. Бере участь у зв'язках:
/// - N:1 з Department (обов'язковий зовнішній ключ DepartmentId, каскадне видалення)
/// - 1:1 з EmployeePassport (опціональний паспорт/профіль безпеки)
/// - N:M з Project через таблицю призначень ProjectAssignment
/// </summary>
public class Employee
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;

    // Обов'язковий зовнішній ключ до відділу
    public int DepartmentId { get; set; }
    public Department Department { get; set; } = null!;

    // Опціональний зовнішній ключ на керівника/ментора (Self-referencing relationship, DeleteBehavior.SetNull)
    public int? MentorId { get; set; }
    public Employee? Mentor { get; set; }
    public List<Employee> MentoredJuniors { get; set; } = new();

    // Навігаційна властивість 1:1
    public EmployeePassport? Passport { get; set; }

    // Навігаційна властивість N:M
    public List<Project> Projects { get; set; } = new();
    public List<ProjectAssignment> ProjectAssignments { get; set; } = new();
}

/// <summary>
/// Залежна сутність у зв'язку 1:1 (Службовий паспорт/перепустка співробітника).
/// </summary>
public class EmployeePassport
{
    public int Id { get; set; }
    public string PassportNumber { get; set; } = string.Empty;
    public string SecurityClearanceLevel { get; set; } = "L1";

    // У зв'язку 1:1 залежна сторона (Dependent) обов'язково містить унікальний зовнішній ключ на головну (Principal)
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;
}

/// <summary>
/// Сутність Проєкту для демонстрації зв'язку Багато-до-Багатьох (N:M).
/// </summary>
public class Project
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateOnly Deadline { get; set; }

    public List<Employee> Employees { get; set; } = new();
    public List<ProjectAssignment> ProjectAssignments { get; set; } = new();
}

/// <summary>
/// Явна проміжна сутність (Join Entity with Payload) для зв'язку N:M між Employee та Project.
/// Містить додаткові бізнес-дані про участь конкретного співробітника у проєкті.
/// </summary>
public class ProjectAssignment
{
    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = null!;

    public int ProjectId { get; set; }
    public Project Project { get; set; } = null!;

    public string RoleOnProject { get; set; } = "Developer";
    public int AllocatedHoursPerWeek { get; set; }
}

public class CompanyRelationsDbContext : DbContext
{
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<EmployeePassport> Passports => Set<EmployeePassport>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<ProjectAssignment> ProjectAssignments => Set<ProjectAssignment>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=company_relations.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // 1. Зв'язок 1:1 (Employee <-> EmployeePassport)
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Passport)
            .WithOne(p => p.Employee)
            .HasForeignKey<EmployeePassport>(p => p.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);

        // 2. Зв'язок 1:N (Department -> Employees)
        modelBuilder.Entity<Department>()
            .HasMany(d => d.Employees)
            .WithOne(e => e.Department)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.Cascade);

        // 3. Самопосилання 1:N (Mentor -> MentoredJuniors) з DeleteBehavior.SetNull
        // Якщо ментора звільнять (видалять), у підопічних MentorId стане NULL, але самі вони залишаться в БД!
        modelBuilder.Entity<Employee>()
            .HasOne(e => e.Mentor)
            .WithMany(m => m.MentoredJuniors)
            .HasForeignKey(e => e.MentorId)
            .OnDelete(DeleteBehavior.SetNull);

        // 4. Зв'язок Багато-до-Багатьох (N:M) через явну сутність ProjectAssignment
        modelBuilder.Entity<Employee>()
            .HasMany(e => e.Projects)
            .WithMany(p => p.Employees)
            .UsingEntity<ProjectAssignment>(
                right => right
                    .HasOne(pa => pa.Project)
                    .WithMany(p => p.ProjectAssignments)
                    .HasForeignKey(pa => pa.ProjectId),
                left => left
                    .HasOne(pa => pa.Employee)
                    .WithMany(e => e.ProjectAssignments)
                    .HasForeignKey(pa => pa.EmployeeId),
                join =>
                {
                    join.HasKey(pa => new { pa.EmployeeId, pa.ProjectId });
                    join.Property(pa => pa.AllocatedHoursPerWeek).HasDefaultValue(20);
                });
    }
}

public static class Program
{
    public static async Task Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("================================================================================");
        Console.WriteLine(" 03. Зв'язки (1:1, 1:N, N:M), Каскадне видалення та Стратегії завантаження");
        Console.WriteLine("================================================================================\n");

        await SeedDatabaseAsync();

        // --------------------------------------------------------------------
        // 1. EAGER LOADING (Жадібне завантаження через Include та ThenInclude)
        // --------------------------------------------------------------------
        Console.WriteLine("--- КРОК 1: Eager Loading (Include + Filtered Include + ThenInclude) ---");
        await using (var context = new CompanyRelationsDbContext())
        {
            // Завантажуємо відділи разом із їхніми співробітниками (відсортованими за ім'ям),
            // паспортами (1:1) та призначеннями на проєкти (N:M) за один запит
            var departments = await context.Departments
                .Include(d => d.Employees.OrderBy(e => e.FullName))
                    .ThenInclude(e => e.Passport)
                .Include(d => d.Employees)
                    .ThenInclude(e => e.ProjectAssignments)
                        .ThenInclude(pa => pa.Project)
                .AsNoTracking()
                .ToListAsync();

            foreach (var dept in departments)
            {
                Console.WriteLine($"Відділ: {dept.Name} (Співробітників: {dept.Employees.Count})");
                foreach (var emp in dept.Employees)
                {
                    string passportInfo = emp.Passport is not null
                        ? $"{emp.Passport.PassportNumber} [{emp.Passport.SecurityClearanceLevel}]"
                        : "немає";
                    Console.WriteLine($"   -> {emp.FullName} ({emp.Position}), Перепустка: {passportInfo}");

                    foreach (var pa in emp.ProjectAssignments)
                    {
                        Console.WriteLine($"      * Проєкт: «{pa.Project.Title}» | Роль: {pa.RoleOnProject} | Годин/тиждень: {pa.AllocatedHoursPerWeek}");
                    }
                }
            }
        }

        // --------------------------------------------------------------------
        // 2. EXPLICIT LOADING (Явне довантаження на вимогу для вже завантаженої сутності)
        // --------------------------------------------------------------------
        Console.WriteLine("\n--- КРОК 2: Explicit Loading (context.Entry(entity).Collection(...).Query()) ---");
        await using (var context = new CompanyRelationsDbContext())
        {
            // Спочатку завантажуємо ТІЛЬКИ сам відділ без співробітників
            var rndDept = await context.Departments.FirstAsync(d => d.Name == "R&D Інженерія");
            Console.WriteLine($"До виклику LoadAsync(): кількість елементів у rndDept.Employees = {rndDept.Employees.Count}");

            // Ми можемо виконати агрегатний запит у БД без завантаження всієї колекції в пам'ять!
            int totalEmployeesInDb = await context.Entry(rndDept)
                .Collection(d => d.Employees)
                .Query()
                .CountAsync();
            Console.WriteLine($"Підрахунок через Entry().Collection().Query().CountAsync() у БД: {totalEmployeesInDb} співробітників");

            // А тепер явно довантажимо в колекцію лише Senior-інженерів:
            await context.Entry(rndDept)
                .Collection(d => d.Employees)
                .Query()
                .Where(e => e.Position.Contains("Senior") || e.Position.Contains("Lead"))
                .LoadAsync();

            Console.WriteLine($"Після фільтрованого Explicit LoadAsync(): у пам'яті {rndDept.Employees.Count} провідних інженерів:");
            foreach (var senior in rndDept.Employees)
            {
                // Явне довантаження одиничного посилання (Reference) для зв'язку 1:1
                await context.Entry(senior).Reference(e => e.Passport).LoadAsync();
                Console.WriteLine($"   * {senior.FullName} ({senior.Position}), Допуск: {senior.Passport?.SecurityClearanceLevel}");
            }
        }

        // --------------------------------------------------------------------
        // 3. Перевірка DeleteBehavior.SetNull (видалення ментора)
        // --------------------------------------------------------------------
        Console.WriteLine("\n--- КРОК 3: Демонстрація DeleteBehavior.SetNull та Cascade ---");
        await using (var context = new CompanyRelationsDbContext())
        {
            var mentor = await context.Employees.FirstAsync(e => e.MentorId == null);
            var juniorBefore = await context.Employees.FirstAsync(e => e.MentorId == mentor.Id);
            Console.WriteLine($"До видалення ментора «{mentor.FullName}»: у «{juniorBefore.FullName}» MentorId = {juniorBefore.MentorId}");

            // Видаляємо ментора: його паспорт (1:1 Cascade) і призначення (Cascade) видаляться автоматично,
            // а у його підопічного джуніора поле MentorId стане NULL (DeleteBehavior.SetNull)!
            context.Employees.Remove(mentor);
            await context.SaveChangesAsync();

            var juniorAfter = await context.Employees.AsNoTracking().FirstAsync(e => e.Id == juniorBefore.Id);
            int passportsLeftForDeletedMentor = await context.Passports.CountAsync(p => p.EmployeeId == mentor.Id);

            Console.WriteLine($"Після видалення ментора: у «{juniorAfter.FullName}» MentorId = {(juniorAfter.MentorId.HasValue ? $"{juniorAfter.MentorId.Value}" : "NULL")}");
            Console.WriteLine($"Паспортів видаленого ментора в БД залишилось (Cascade): {passportsLeftForDeletedMentor}");
        }
    }

    private static async Task SeedDatabaseAsync()
    {
        await using var context = new CompanyRelationsDbContext();
        await context.Database.EnsureDeletedAsync();
        await context.Database.EnsureCreatedAsync();

        var rnd = new Department { Name = "R&D Інженерія" };

        var lead = new Employee
        {
            FullName = "Олександр Коваленко",
            Position = "Tech Lead",
            Department = rnd,
            Passport = new EmployeePassport { PassportNumber = "UA-PASS-001", SecurityClearanceLevel = "L3-Secret" }
        };

        var senior = new Employee
        {
            FullName = "Ірина Мельник",
            Position = "Senior Backend Developer",
            Department = rnd,
            Mentor = lead,
            Passport = new EmployeePassport { PassportNumber = "UA-PASS-002", SecurityClearanceLevel = "L2-Internal" }
        };

        var junior = new Employee
        {
            FullName = "Максим Бондаренко",
            Position = "Junior C# Developer",
            Department = rnd,
            Mentor = lead,
            Passport = new EmployeePassport { PassportNumber = "UA-PASS-003", SecurityClearanceLevel = "L1-Basic" }
        };

        var aiPlatform = new Project { Title = "AI Core Analytics", Deadline = new DateOnly(2026, 12, 31) };
        var cloudMigration = new Project { Title = "Cloud Native Migration", Deadline = new DateOnly(2026, 9, 15) };

        context.ProjectAssignments.AddRange(
            new ProjectAssignment { Employee = lead, Project = aiPlatform, RoleOnProject = "Solution Architect", AllocatedHoursPerWeek = 25 },
            new ProjectAssignment { Employee = lead, Project = cloudMigration, RoleOnProject = "Tech Advisor", AllocatedHoursPerWeek = 15 },
            new ProjectAssignment { Employee = senior, Project = aiPlatform, RoleOnProject = "Lead API Engineer", AllocatedHoursPerWeek = 40 },
            new ProjectAssignment { Employee = junior, Project = cloudMigration, RoleOnProject = "Junior Engineer", AllocatedHoursPerWeek = 35 }
        );

        await context.SaveChangesAsync();
    }
}
