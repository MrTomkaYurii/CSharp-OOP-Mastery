# 01-EFCore-Complete-Mastery: Повний фундаментальний курс Entity Framework Core 10

Цей підмодуль є систематизованим покроковим посібником і практичним курсом із **Entity Framework Core (.NET 10)**, побудованим за класичною структурою курсу Metanit та офіційної документації Microsoft.

Усі **6 консольних проєктів** об'єднані в єдине рішення **`OOP-EFCore-Complete-Mastery.sln`**, розкладені по пронумерованих папках рішення і працюють повністю автономно на вбудованій реляційній СУБД **SQLite** (без потреби встановлювати зовнішній сервер баз даних).

---

## Структура курсу та проєктів

| № | Проєкт | Теми, що демонструються на практиці | Документація |
| :-: | :--- | :--- | :--- |
| **01** | [`01-DbContextCrudAndChangeTracker`](./01-DbContextCrudAndChangeTracker/Program.cs) | `DbContext`, `DbSet<T>`, `EnsureCreatedAsync`, логування SQL (`LogTo`), CRUD, кеш першого рівня `FindAsync`, стани `EntityState` (`Detached`, `Added`, `Unchanged`, `Modified`, `Deleted`), `OriginalValue` vs `CurrentValue`, `ChangeTracker.DebugView`. | [01_DbContext_CRUD_And_ChangeTracker.md](./Docs/01_DbContext_CRUD_And_ChangeTracker.md) |
| **02** | [`02-EntityModelingAnnotationsAndFluentApi`](./02-EntityModelingAnnotationsAndFluentApi/Program.cs) | Конвенції, Data Annotations проти Fluent API, винесення конфігурацій у `IEntityTypeConfiguration<T>`, складені ключі (`HasKey`), альтернативні ключі (`HasAlternateKey`), індекси, `HasCheckConstraint`, `HasDefaultValue`, `[NotMapped]` / `Ignore`, початкові дані `HasData`. | [02_Entity_Modeling_Annotations_And_FluentAPI.md](./Docs/02_Entity_Modeling_Annotations_And_FluentAPI.md) |
| **03** | [`03-RelationshipsAndLoadingStrategies`](./03-RelationshipsAndLoadingStrategies/Program.cs) | Зв'язки `1:1`, `1:N` (включно із самопосиланням `Mentor -> Juniors`), `N:M` з явною проміжною сутністю `ProjectAssignment`, каскадне видалення (`Cascade`, `SetNull`), стратегії **Eager Loading** (`Include`/`ThenInclude`), **Explicit Loading** (`Entry().Collection().Query()`) та проблема $N+1$. | [03_Relationships_Cascade_And_Loading.md](./Docs/03_Relationships_Cascade_And_Loading.md) |
| **04** | [`04-LinqToEntitiesQueriesAndTracking`](./04-LinqToEntitiesQueriesAndTracking/Program.cs) | `IQueryable<T>` та трансляція у SQL (`ToQueryString()`), `EF.Functions.Like`, сортування, пагінація (`Skip`/`Take`), проєкція у `record` DTO через `Select`, аналітичне групування `GroupBy` (`Count`, `Sum`, `Average`, `Max`), різниця між `AsNoTracking()` та `AsNoTrackingWithIdentityResolution()`. | [04_Linq_To_Entities_Queries_And_Tracking.md](./Docs/04_Linq_To_Entities_Queries_And_Tracking.md) |
| **05** | [`05-InheritanceStrategiesTphTptTpc`](./05-InheritanceStrategiesTphTptTpc/Program.cs) | Відображення ієрархій наслідування ООП у реляційну БД: **TPH** (одна таблиця з дискримінатором), **TPT** (`UseTptMappingStrategy`), **TPC** (`UseTpcMappingStrategy`), поліморфні запити, фільтрація `.OfType<T>()` та вбудовані об'єкти-значення `OwnsOne`. | [05_Inheritance_Strategies_TPH_TPT_TPC.md](./Docs/05_Inheritance_Strategies_TPH_TPT_TPC.md) |
| **06** | [`06-RawSqlTransactionsAndConcurrency`](./06-RawSqlTransactionsAndConcurrency/Program.cs) | Безпечний сирий SQL (`FromSqlInterpolated`, `Database.SqlQuery<T>`, `ExecuteSqlInterpolatedAsync`), транзакції та точки збереження (`CreateSavepointAsync` / `RollbackToSavepointAsync`), оптимістичний паралелізм (`[ConcurrencyCheck]`, `DbUpdateConcurrencyException`), глобальні фільтри `HasQueryFilter` та пакетне оновлення `ExecuteUpdateAsync`. | [06_RawSql_Transactions_And_Concurrency.md](./Docs/06_RawSql_Transactions_And_Concurrency.md) |

---

## Як зібрати та запустити проєкти

```powershell
# Збірка всього рішення курсу
dotnet build OOP-EFCore-Complete-Mastery.sln

# Почерговий запуск навчальних модулів
dotnet run --project 01-DbContextCrudAndChangeTracker
dotnet run --project 02-EntityModelingAnnotationsAndFluentApi
dotnet run --project 03-RelationshipsAndLoadingStrategies
dotnet run --project 04-LinqToEntitiesQueriesAndTracking
dotnet run --project 05-InheritanceStrategiesTphTptTpc
dotnet run --project 06-RawSqlTransactionsAndConcurrency
```
