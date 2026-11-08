# Підмодуль 03: Впровадження залежностей (Dependency Injection) у .NET 10

Цей підмодуль безпосередньо продовжує тему `Generic Host` і детально розбирає промисловий контейнер інверсії керування **`Microsoft.Extensions.DependencyInjection`**: життєві цикли сервісів, безпечну роботу з областями видимості (`IServiceScopeFactory`), захист від антипатерну `Captive Dependency` та сучасні іменовані стратегії `Keyed Services`.

---

## Навчальні посібники (`Docs/`)

1. **[01. Часи життя сервісів: `Transient`, `Scoped` та `Singleton`](Docs/01_Service_Lifetimes_Transient_Scoped_Singleton.md)**
   - Правила конструювання, спільне використання екземплярів у межах `IServiceScope` та автоматичне звільнення ресурсів (`IDisposable`).
2. **[02. Управління областями видимості (`IServiceScopeFactory`) та пастка `Captive Dependency`](Docs/02_IServiceScopeFactory_And_Captive_Dependency.md)**
   - Чому `Singleton` не може напряму інжектувати `Scoped`-сервіс, як `ValidateScopes` та `ValidateOnBuild` ловлять помилку на старті та як правильно відкривати `Scope` через `IServiceScopeFactory`.
3. **[03. Множинні реалізації інтерфейсів та `Keyed Services` (`.NET 8+`)](Docs/03_Multiple_Implementations_And_Keyed_Services.md)**
   - Широкомовне впровадження через `IEnumerable<T>`, захисна реєстрація `TryAddEnumerable`, вибір стратегії за ключем `[FromKeyedServices]` та фабричні делегати.

---

## Структура проєктів у рішенні `OOP - Dependency Injection.sln`

| Solution-тека | Проєкт | Ключові концепції |
|:---|:---|:---|
| **`01. Часи життя сервісів: Transient, Scoped та Singleton`** | [`01-ServiceLifetimesDemo`](01-ServiceLifetimesDemo/Program.cs) | Покрокова візуалізація `AddTransient`, `AddScoped`, `AddSingleton` у межах кількох послідовних бізнес-операцій `IServiceScope` |
| **`02. Управління областями видимості (IServiceScopeFactory) та пастка Captive Dependency`** | [`02-ScopesAndCaptiveDependency`](02-ScopesAndCaptiveDependency/Program.cs) | Детекція `Captive Dependency` через `ValidateScopes = true` / `ValidateOnBuild = true` та еталонна робота з `IServiceScopeFactory.CreateAsyncScope()` |
| **`03. Множинні реалізації інтерфейсів та Keyed Services (.NET 8+)`** | [`03-MultipleAndKeyedServices`](03-MultipleAndKeyedServices/Program.cs) | `IEnumerable<INotificationChannel>`, `TryAddEnumerable`, `AddKeyedScoped`, `[FromKeyedServices("sms")]`, фабричний декоратор |

---

## Збірка та запуск

```powershell
# Збірка всього рішення підмодуля
dotnet build "08-Architecture-And-Patterns/03-Dependency-Injection/OOP - Dependency Injection.sln" -c Release

# Почерговий запуск демонстраційних проєктів
dotnet run --project "08-Architecture-And-Patterns/03-Dependency-Injection/01-ServiceLifetimesDemo/01-ServiceLifetimesDemo.csproj"
dotnet run --project "08-Architecture-And-Patterns/03-Dependency-Injection/02-ScopesAndCaptiveDependency/02-ScopesAndCaptiveDependency.csproj"
dotnet run --project "08-Architecture-And-Patterns/03-Dependency-Injection/03-MultipleAndKeyedServices/03-MultipleAndKeyedServices.csproj"
```
