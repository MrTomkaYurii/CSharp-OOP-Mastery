# Модуль: Dependency Injection & Modern Hosting у .NET 10

Цей модуль присвячений поглибленому вивченню архітектури **Inversion of Control (IoC)**, вбудованого контейнера **Dependency Injection (DI)**, а також сучасних моделей хостингу додатків: **Generic Host (`HostApplicationBuilder`)** та **Web Host (`WebApplicationBuilder`)**.

Модуль логічно продовжує принципи **SOLID** (зокрема *Dependency Inversion Principle*), показуючи, як проектувати тестовані, масштабовані та відмовостійкі сервіси у production-середовищі.

---

## Структура проєктів

```
03-Dependency-Injection-And-Hosting/
├── DependencyInjectionAndHosting.slnx   # Файл рішення .NET 10
├── DI.CoreConcepts/                    # Фундаментальні механізми DI-контейнера
│   ├── Services/
│   │   ├── Lifetimes.cs                # Transient, Scoped, Singleton
│   │   ├── KeyedServices.cs            # Іменовані сервіси (.NET 8/9/10)
│   │   ├── CaptiveDependencyDemo.cs    # Антипатерн захоплення scope та ValidateScopes
│   │   └── DisposalDemo.cs             # Автоматичний виклик IDisposable / IAsyncDisposable
│   └── Program.cs
├── DI.GenericHost.Worker/              # Фоновий демон на Generic Host
│   ├── Configuration/
│   │   └── WorkerOptions.cs            # Options pattern з валідацією DataAnnotations
│   ├── Services/
│   │   └── IOrderProcessingService.cs  # Scoped бізнес-сервіс
│   ├── Workers/
│   │   ├── OrderProcessingWorker.cs    # BackgroundService із керуванням Scope
│   │   └── LifecycleMonitorService.cs  # IHostedService та події IHostApplicationLifetime
│   ├── appsettings.json
│   └── Program.cs
├── DI.WebHost.MinimalApi/              # Веб-сервіс на базі WebApplication Minimal API
│   ├── Middleware/
│   │   └── RequestTimingMiddleware.cs  # IMiddleware через DI-фабрику
│   ├── Services/
│   │   ├── ICatalogService.cs          # Каталог товарів
│   │   └── IPaymentGateway.cs          # Keyed-сервіси (Stripe, LiqPay)
│   └── Program.cs
└── Docs/                               # Повний теоретичний посібник українською
    ├── 01_DI_Principles_And_Lifetimes.md
    ├── 02_Generic_Host_And_Worker_Services.md
    └── 03_Web_Host_And_Advanced_DI.md
```

---

## Навчальні матеріали
1. [Розділ 1: Принципи DI, життєві цикли та Captive Dependencies](file:///c:/Users/Yurii/source/repos/CSharp-OOP-Mastery/05-Architecture-And-Patterns/03-Dependency-Injection-And-Hosting/Docs/01_DI_Principles_And_Lifetimes.md)
2. [Розділ 2: Generic Host, Worker Services та Options Pattern](file:///c:/Users/Yurii/source/repos/CSharp-OOP-Mastery/05-Architecture-And-Patterns/03-Dependency-Injection-And-Hosting/Docs/02_Generic_Host_And_Worker_Services.md)
3. [Розділ 3: Web Host, Minimal APIs та Розширені Патерни DI](file:///c:/Users/Yurii/source/repos/CSharp-OOP-Mastery/05-Architecture-And-Patterns/03-Dependency-Injection-And-Hosting/Docs/03_Web_Host_And_Advanced_DI.md)

---

## Як запустити проєкти

### 1. Перевірка роботи DI.CoreConcepts
```powershell
dotnet run --project DI.CoreConcepts/DI.CoreConcepts.csproj
```

### 2. Запуск фонового демона DI.GenericHost.Worker
```powershell
dotnet run --project DI.GenericHost.Worker/DI.GenericHost.Worker.csproj
```

### 3. Запуск веб-сервісу DI.WebHost.MinimalApi
```powershell
dotnet run --project DI.WebHost.MinimalApi/DI.WebHost.MinimalApi.csproj
```
