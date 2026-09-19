# Розділ 3: Web Host, Minimal API та Розширені Патерни DI

## 1. Web Host vs Generic Host
Починаючи з .NET 6, Web Host був об'єднаний з концепцією хостингу через клас `WebApplicationBuilder` (`WebApplication.CreateBuilder(args)`):
- **Generic Host (`HostApplicationBuilder`)**: оптимізований для фонових процесів, CLI, консольних демонів без HTTP стеку.
- **Web Host (`WebApplicationBuilder`)**: розширює базовий хост вбудованим HTTP-сервером Kestrel, конфігурацією Middleware Pipeline, Endpoint Routing та HTTP-контекстом (`HttpContext`).

---

## 2. Ін'єкція залежностей у Minimal APIs

ASP.NET Core Minimal APIs автоматично зіставляють параметри лямбда-виразів ендпоінтів із DI контейнером:

```csharp
// 1. Автоматична ін'єкція сервісу (не обов'язково писати [FromServices]):
app.MapGet("/api/products", (ICatalogService catalog) =>
{
    return Results.Ok(catalog.GetProducts());
});

// 2. Комбінація параметрів маршруту, запиту та DI:
app.MapGet("/api/orders/{id:guid}", (
    Guid id,
    [FromQuery] bool includeDetails,
    IOrderRepository repository,
    CancellationToken ct) =>
{
    // ...
});

// 3. Ін'єкція Keyed Services через IServiceProvider:
app.MapPost("/api/pay/{gateway}", async (
    string gateway,
    [FromQuery] decimal amount,
    IServiceProvider sp,
    CancellationToken ct) =>
{
    var paymentService = sp.GetKeyedService<IPaymentGateway>(gateway);
    return paymentService is not null
        ? Results.Ok(await paymentService.ProcessPaymentAsync(amount, "UAH", ct))
        : Results.BadRequest();
});
```

---

## 3. Middleware та Dependency Injection: Конвенційні vs IMiddleware

В ASP.NET Core існує два підходи до створення проміжного ПЗ (Middleware):

### Підхід А: Звичайне (Конвенційне) Middleware
- Створюється як **Singleton** під час формування конвеєра запитів (`IApplicationBuilder`).
- **Увага:** Конструктор конвенційного Middleware викликається лише один раз! Тому в конструктор можна інжектувати лише `Singleton` або `Transient` залежності.
- Якщо потрібні `Scoped` сервіси (наприклад, `DbContext`), їх необхідно приймати в метод `InvokeAsync(HttpContext context, ScopedService scoped)`!

### Підхід Б: Фабричне Middleware через `IMiddleware`
- Клас реалізує інтерфейс `IMiddleware`.
- Обов'язково реєструється в DI контейнері: `builder.Services.AddTransient<RequestTimingMiddleware>()`.
- Оскільки екземпляр створюється через контейнер на кожен запит (якщо зареєстровано як Transient/Scoped), можна безпечно інжектувати залежності прямо в конструктор:

```csharp
public class RequestTimingMiddleware : IMiddleware
{
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(ILogger<RequestTimingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Логіка до виконання запиту
        await next(context);
        // Логіка після виконання запиту
    }
}
```

---

## 4. Чек-лист найкращих практик DI (Best Practices)

- [x] **Уникайте Service Locator:** Не передавайте `IServiceProvider` безпосередньо у бізнес-методи класів, окрім спеціалізованих фабрик або динамічного вибору Keyed Services.
- [x] **Завжди вмикайте валідацію контейнера:** `ValidateScopes = true` та `ValidateOnBuild = true` рятують від 90% прихованих помилок багатопоточності у production.
- [x] **Не викликайте `BuildServiceProvider()` усередині `ConfigureServices`:** Це створює другий ізольований копійований контейнер та призводить до дублювання синглтонів.
- [x] **Контролюйте межі Scoped у фонових сервісах:** Будь-який `BackgroundService` повинен створювати власний `IServiceScope` для кожної ітерації циклу через `IServiceScopeFactory`.
- [x] **Користуйтесь Keyed Services:** Замість складних `Dictionary<string, Func<T>>` фабрик використовуйте стандартні .NET 8+ `AddKeyedSingleton / AddKeyedScoped`.
