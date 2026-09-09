# Архітектура Generic Host та Hosted Services у консольних додатках

Сучасні консольні додатки на .NET використовують **Generic Host (`Microsoft.Extensions.Hosting`)**, що надає ті самі корпоративні можливості, які доступні в ASP.NET Core: інверсію керування (DI), структуроване конфігурування (`appsettings.json`), логування та життєвий цикл фонових служб (`IHostedService`).

---

## 1. Концепція архітектури та потік виконання

```mermaid
sequenceDiagram
    autonumber
    participant Program as Program.cs (Generic Host)
    participant InitService as DatabaseInitializationService (HostedService)
    participant HostedService as EntertainmentHostedService (HostedService)
    participant MenuService as MenuService (TUI Menu)
    participant Factory as QueryServiceFactory
    participant Scope as IServiceScope (Новий Scope)
    participant Db as DbContext & QueryService
    participant ConsoleUI as Spectre Console Layout

    Program->>InitService: StartAsync() -> Перевірка/Створення бази
    InitService-->>Program: База готова
    Program->>HostedService: StartAsync()
    HostedService->>MenuService: ShowMenuAsync()
    MenuService->>ConsoleUI: Відобразити меню
    MenuService->>Factory: ExecuteQueryAsync(queryId)
    Factory->>Scope: CreateScope()
    Scope->>Db: Виконати EF Core запит
    Db-->>Factory: Результати вибірки
    Factory->>ConsoleUI: Оновити нижню панель результатів
    Factory->>Scope: DisposeScope() (Звільнення DbContext)
```

---

## 2. Керування часом життя `DbContext` через Scoped Services

Оскільки консольний додаток є довгоживучим процесом, збереження одного екземпляра `DbContext` на весь час роботи програми є антипатерном (призводить до витоків пам'яті через ChangeTracker та проблем із застарілими даними).

### Патерн Scope Factory:
```csharp
public async Task<TResult> ExecuteQueryAsync<TResult>(Func<IQueryService, Task<TResult>> queryAction)
{
    // Створюється ізольований скоуп для одного запиту
    using var scope = _serviceScopeFactory.CreateScope();
    
    // Отримуємо DbContext та QueryService, прив'язані до цього скоупу
    var queryService = scope.ServiceProvider.GetRequiredService<IQueryService>();
    
    // Виконуємо операцію
    return await queryAction(queryService);
    
    // При виході з блоку using контекст коректно звільняється
}
```

---

## 3. Ініціалізація бази даних через `DatabaseInitializationService`

Перед початком роботи меню спеціальний HostedService автоматично перевіряє наявність SQLite бази даних, застосовує міграції або заповнює початковими даними (Seed Data):

```csharp
public class DatabaseInitializationService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<EntertainmentDbContext>();
        
        await dbContext.Database.MigrateAsync(cancellationToken);
        await SeedDataAsync(dbContext);
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
```
