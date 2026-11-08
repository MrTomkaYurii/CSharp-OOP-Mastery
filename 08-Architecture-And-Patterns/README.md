# Глава 08: Архітектура програмного забезпечення, SOLID, DI, AutoMapper та CLI

Ця глава є узагальнюючою архітектурною ланкою курсу, яка йде **після** вивчення метапрограмування (`05`), багатопотоковості (`06`) та баз даних `Entity Framework Core` (`07`). Тут розбираються принципи проектування гнучких корпоративних систем та їх інтеграція в єдине ціле.

---

## Підмодулі глави

| № | Підмодуль | Фокус вивчення | Рішення Visual Studio |
|:--:|:---|:---|:---|
| **01** | **[01-SOLID-Principles](01-SOLID-Principles/)** | П'ять принципів SOLID (`SRP`, `OCP`, `LSP`, `ISP`, `DIP`) за схемою «Антипатерн (Bad) -> Еталон (Good)» та еволюція `DIP -> IoC -> DI -> Container` | `OOP - SOLID/OOP - SOLID.sln` |
| **02** | **[02-Dependency-Injection-And-Hosting](02-Dependency-Injection-And-Hosting/)** | Промисловий контейнер `Microsoft.Extensions.DependencyInjection` (`Transient`, `Scoped`, `Singleton`, `Captive Dependency`, `Keyed Services`), `Generic Host` (`BackgroundService`) та `Web Host` (`Minimal API`) | `OOP - Dependency Injection and Hosting.sln` |
| **03** | **[03-AutoMapper-And-DTO](03-AutoMapper-And-DTO/)** | Патерн Data Transfer Object (DTO), профілі `AutoMapper`, сплющення ієрархій (`Flattening`), кастомні резолвери `IValueResolver` та конвеєри серіалізації | `OOP - Serialization-AutoMapper.sln` |
| **04** | **[04-CLI-Spectre-Console](04-CLI-Spectre-Console/)** | Побудова багатих консольних клієнтських інтерфейсів (`Spectre.Console` та `Spectre.Console.Cli`): таблиці, дерева, інтерактивні промпти, прогрес-бари, діаграми та DI у командах CLI | `SpectreConsoleExamples.sln` |
| **05** | **[05-EFCore-Clean-Architecture-Demo](05-EFCore-Clean-Architecture-Demo/)** | **Інтеграційний Capstone-проєкт**: поєднання `EF Core (SQLite)` + `SOLID (Repository / Strategy)` + `AutoMapper (DTO)` + `Dependency Injection` + `Spectre.Console UI` | `OOP - EFCore Clean Architecture.sln` |
