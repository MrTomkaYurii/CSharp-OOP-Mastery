# Підмодуль 02: Generic Host, Конфігурація та Система логування в .NET 10

Цей підмодуль присвячений фундаменту побудови сучасних консольних і фонових застосунків на платформі .NET — універсальному хосту **`Generic Host` (`Microsoft.Extensions.Hosting`)**, роботі з ієрархічними конфігураційними файлами (`appsettings.json`, патерн `IOptions<T>`) та промисловій системі структурованого логування (`ILogger<T>`).

---

## Навчальні посібники (`Docs/`)

1. **[01. Архітектура Generic Host та Фонові служби (`IHost`, `BackgroundService`)](Docs/01_Generic_Host_And_Background_Services.md)**
   - Життєвий цикл хоста (`IHostApplicationLifetime`), різниця між `IHostedService` та `BackgroundService`, кооперативна зупинка через `CancellationToken`.
2. **[02. Конфігураційні файли та Патерн Options (`IConfiguration`, `appsettings.json`, `IOptions`)](Docs/02_Configuration_And_Options_Pattern.md)**
   - Каскадне злиття налаштувань, строга прив'язка до C#-моделей (`IOptions<T>`, `IOptionsSnapshot<T>`, `IOptionsMonitor<T>`) та рання валідація при старті (`ValidateDataAnnotations`, `ValidateOnStart`).
3. **[03. Система логування в .NET (`ILogger`, Рівні логування, Структуроване логування та Scopes)](Docs/03_Structured_Logging_And_Scopes.md)**
   - Категорії та рівні `LogLevel`, фільтрація у конфігурації, структуровані шаблони повідомлень, контекстні області `BeginScope` та генератор `[LoggerMessage]`.

---

## Структура проєктів у рішенні `OOP - Generic Host Configuration and Logging.sln`

| Solution-тека | Проєкт | Ключові концепції |
|:---|:---|:---|
| **`01. Архітектура Generic Host та Фонові служби (IHost, BackgroundService)`** | [`01-GenericHostLifecycleAndWorkers`](01-GenericHostLifecycleAndWorkers/Program.cs) | `Host.CreateApplicationBuilder`, `IHostApplicationLifetime` (`ApplicationStarted`, `ApplicationStopping`, `ApplicationStopped`), `IHostedService`, `BackgroundService`, `PeriodicTimer` |
| **`02. Конфігураційні файли та Патерн Options (IConfiguration, appsettings.json, IOptions)`** | [`02-ConfigurationAndOptionsPattern`](02-ConfigurationAndOptionsPattern/Program.cs) | `appsettings.json` + `appsettings.Development.json`, змінні середовища, CLI-аргументи, `IOptions<T>` / `IOptionsSnapshot<T>` / `IOptionsMonitor<T>`, `ValidateDataAnnotations()`, `ValidateOnStart()` |
| **`03. Система логування в .NET (ILogger, Рівні логування, Структуроване логування та Scopes)`** | [`03-StructuredLoggingAndScopes`](03-StructuredLoggingAndScopes/Program.cs) | `ILogger<T>`, `ILoggerFactory`, фільтрація `LogLevel` за категоріями, структуроване логування, `BeginScope`, `[LoggerMessage]` |

---

## Збірка та запуск

```powershell
# Збірка всього рішення підмодуля
dotnet build "08-Architecture-And-Patterns/02-Generic-Host-Configuration-And-Logging/OOP - Generic Host Configuration and Logging.sln" -c Release

# Почерговий запуск демонстраційних проєктів
dotnet run --project "08-Architecture-And-Patterns/02-Generic-Host-Configuration-And-Logging/01-GenericHostLifecycleAndWorkers/01-GenericHostLifecycleAndWorkers.csproj"
dotnet run --project "08-Architecture-And-Patterns/02-Generic-Host-Configuration-And-Logging/02-ConfigurationAndOptionsPattern/02-ConfigurationAndOptionsPattern.csproj"
dotnet run --project "08-Architecture-And-Patterns/02-Generic-Host-Configuration-And-Logging/03-StructuredLoggingAndScopes/03-StructuredLoggingAndScopes.csproj"
```
