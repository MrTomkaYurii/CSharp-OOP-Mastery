# Модуль 08.5: Інтеграційна Архітектура — EF Core + SOLID + AutoMapper + DI + Spectre.Console

Цей підмодуль є узагальнюючим практичним прикладом (Capstone Project), який поєднує всі ключові технології та архітектурні патерни курсу в єдиному додатку:
1. **Entity Framework Core (SQLite)** — робота з реляційною базою даних, зв'язок `Author (1) -> (*) Book`, жадібне завантаження `.Include(b => b.Author)` та `.AsNoTracking()`.
2. **Принципи SOLID** — розділення на шари `Domain`, `Infrastructure` (`IBookRepository`), `Application` (`IPricingStrategy`, `CatalogApplicationService`) та `Presentation` (`SpectreCatalogRenderer`).
3. **AutoMapper та патерн DTO** — автоматична трансформація графа сутностей `Book -> Author` у плоский контракт `BookCatalogDto`.
4. **Dependency Injection (`Microsoft.Extensions.DependencyInjection`)** — збирання всього графа залежностей у Composition Root з контролем `IServiceScope`.
5. **Spectre.Console** — побудова сучасного табличного UI в консолі.

---

## Структура проєкту

| Шар архітектури | Файл коду | Призначення |
|:---|:---|:---|
| **Domain** | [Entities.cs](EFCoreCleanArchitectureDemo/Domain/Entities.cs) | Доменні сутності `Author` та `Book` |
| **Infrastructure** | [BookstoreDataAccess.cs](EFCoreCleanArchitectureDemo/Infrastructure/BookstoreDataAccess.cs) | `BookstoreDbContext` та репозиторій `EfCoreBookRepository` |
| **Application** | [ContractsAndMapping.cs](EFCoreCleanArchitectureDemo/Application/ContractsAndMapping.cs) | `BookCatalogDto`, `BookstoreMappingProfile`, `IPricingStrategy` |
| **Presentation** | [CatalogServiceAndUi.cs](EFCoreCleanArchitectureDemo/Presentation/CatalogServiceAndUi.cs) | `CatalogApplicationService` та `SpectreCatalogRenderer` |
| **Composition Root** | [Program.cs](EFCoreCleanArchitectureDemo/Program.cs) | Налаштування `ServiceCollection` та запуск |

---

## Документація (Docs)
- [01. Як поєднати EF Core, SOLID, AutoMapper, DI та Spectre.Console](Docs/01_Clean_Architecture_With_EFCore_And_DI.md)
