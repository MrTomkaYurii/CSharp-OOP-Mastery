# Підмодуль 06: Тришаровий консольний застосунок на Spectre.Console та EF Core (SQLite)

Цей підсумковий проєкт демонструє практичне поєднання всіх архітектурних патернів розділу у вигляді повноцінного **тришарового застосунку керування складом і замовленнями магазину** (`Data Access Layer`  `Business Logic Layer`  `Presentation Layer`).

---

## Документація (`Docs/`)

- **[01. Тришарова архітектура застосунку: EF Core (SQLite) + Business Logic (DTO, AutoMapper) + Spectre.Console](Docs/01_Three_Tier_Architecture_EFCore_And_SpectreConsole.md)**

---

## Структура рішення `OOP - SpectreConsole EFCore Tiered App.sln`

| Solution-тека | Проєкт | Тип збірки | Відповідальність шару |
|:---|:---|:---|:---|
| **`01. Шар роботи з даними (Data Access Layer - EF Core & Repositories)`** | [`StoreApp.DataAccess`](StoreApp.DataAccess/) | Class Library (`net10.0`) | `StoreDbContext` (SQLite), сутності (`Category`, `Product`, `CustomerOrder`, `OrderLine`), Fluent API, `ProductRepository`, `OrderRepository` (транзакції, `Include`, `AsNoTracking`) |
| **`02. Шар бізнес-логіки (Business Logic Layer - Services, DTO & AutoMapper)`** | [`StoreApp.BusinessLogic`](StoreApp.BusinessLogic/) | Class Library (`net10.0`) | Сервіси `CatalogService`, `OrderProcessingService`, `AnalyticsService`, моделі DTO, профіль `StoreMappingProfile` (`AutoMapper`), конфігурація `StoreSettings` (`IOptions<StoreSettings>`) та логування `ILogger<T>` |
| **`03. Презентаційний консольний шар (Presentation Layer - Generic Host & Spectre.Console)`** | [`StoreApp.ConsoleUI`](StoreApp.ConsoleUI/) | Console Exe (`net10.0`) | `Generic Host` (`appsettings.json`, DI-реєстрація, `AsyncServiceScope`) та багатий інтерфейс на `Spectre.Console` (таблиці каталогу, панелі квитанцій, графіки `BarChart` і `BreakdownChart`) |

---

## Збірка та запуск

```powershell
# Збірка всього тришарового рішення
dotnet build "08-Architecture-And-Patterns/06-SpectreConsole-EFCore-Tiered-App/OOP - SpectreConsole EFCore Tiered App.sln" -c Release

# Запуск презентаційного консольного застосунку
dotnet run --project "08-Architecture-And-Patterns/06-SpectreConsole-EFCore-Tiered-App/StoreApp.ConsoleUI/StoreApp.ConsoleUI.csproj"
```
