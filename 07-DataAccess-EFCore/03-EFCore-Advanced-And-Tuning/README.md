# Модуль: EF Core 10 Advanced & Database Fine-Tuning

Цей модуль присвячений просунутим технікам моделювання, генерації реляційних даних за допомогою **Bogus**, оптимізації продуктивності та тонкому налаштуванню взаємодії з базами даних в **Entity Framework Core 10**.

---

## Структура проєктів

```
03-EFCore-Advanced-And-Tuning/
├── EFCoreAdvancedAndTuning.slnx        # Файл рішення .NET 10
├── EFCore.AdvancedMapping/             # Просунуті техніки мапування схеми
│   ├── Models/
│   │   ├── Payments.cs                 # TPH (Table-per-Hierarchy) успадкування
│   │   ├── CustomerAndAddress.cs       # Owned Types (Value Objects)
│   │   └── ProductWithJson.cs          # Нативні JSON-стовпці (.ToJson())
│   ├── Data/
│   │   └── AdvancedMappingDbContext.cs
│   └── Program.cs
├── EFCore.Seeding.Bogus/               # Реалістичний сидинг бази даних
│   ├── Models/
│   │   └── Entities.cs                 # Автор -> Книга -> Відгук
│   ├── Data/
│   │   └── BookstoreDbContext.cs
│   ├── Seeding/
│   │   └── BookstoreDataSeeder.cs      # Генерація зв'язаних даних через Bogus
│   └── Program.cs
├── EFCore.Performance.Bulk/            # Продуктивність та оптимізація
│   ├── Models/
│   │   └── AuditableEntity.cs          # Інтерфейси IAuditable, ISoftDeletable
│   ├── Data/
│   │   ├── AuditSaveChangesInterceptor.cs # Автоматичний аудит міток часу
│   │   └── PerformanceDbContext.cs     # Global Query Filters (Soft Delete)
│   └── Program.cs                      # ExecuteUpdateAsync, AsSplitQuery, Compiled Query
└── Docs/                               # Теоретичні посібники українською
    ├── 01_Inheritance_Mapping_TPH_TPT_TPC.md
    ├── 02_Bogus_Seeding_And_Data_Generation.md
    └── 03_Performance_BulkOps_And_Interceptors.md
```

---

## Навчальні матеріали
1. [Розділ 1: Стратегії успадкування, Owned Types та JSON-стовпці](file:///c:/Users/Yurii/source/repos/CSharp-OOP-Mastery/07-DataAccess-EFCore/03-EFCore-Advanced-And-Tuning/Docs/01_Inheritance_Mapping_TPH_TPT_TPC.md)
2. [Розділ 2: Генерація даних та сидинг через Bogus](file:///c:/Users/Yurii/source/repos/CSharp-OOP-Mastery/07-DataAccess-EFCore/03-EFCore-Advanced-And-Tuning/Docs/02_Bogus_Seeding_And_Data_Generation.md)
3. [Розділ 3: Масові операції, Global Query Filters та Інтерцептори](file:///c:/Users/Yurii/source/repos/CSharp-OOP-Mastery/07-DataAccess-EFCore/03-EFCore-Advanced-And-Tuning/Docs/03_Performance_BulkOps_And_Interceptors.md)

---

## Інструкція запуску

```powershell
# 1. Запуск розширеного мапування (TPH, Owned, JSON):
dotnet run --project EFCore.AdvancedMapping/EFCore.AdvancedMapping.csproj

# 2. Запуск генерації та сидингу даних через Bogus:
dotnet run --project EFCore.Seeding.Bogus/EFCore.Seeding.Bogus.csproj

# 3. Запуск демонстрації продуктивності та масових операцій:
dotnet run --project EFCore.Performance.Bulk/EFCore.Performance.Bulk.csproj
```
