using DI.WebHost.MinimalApi.Middleware;
using DI.WebHost.MinimalApi.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// 1. Реєстрація сервісів у DI контейнері
builder.Services.AddOpenApi();
builder.Services.AddSingleton<ICatalogService, InMemoryCatalogService>();

// Реєстрація Keyed Services для різних платіжних шлюзів
builder.Services.AddKeyedSingleton<IPaymentGateway, StripePaymentGateway>("stripe");
builder.Services.AddKeyedSingleton<IPaymentGateway, LiqPayPaymentGateway>("liqpay");

// Реєстрація IMiddleware в DI
builder.Services.AddTransient<RequestTimingMiddleware>();

var app = builder.Build();

// 2. Конфігурація HTTP Pipeline (Middleware)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<RequestTimingMiddleware>();

// 3. Маршрути Minimal API з різними варіантами DI-ін'єкції
app.MapGet("/", () => Results.Ok(new
{
    Message = "Ласкаво просимо до API демонстрації ASP.NET Core Web Host & Dependency Injection!",
    Docs = "/openapi/v1.json",
    Endpoints = new[]
    {
        "GET /api/products",
        "GET /api/products/{id}",
        "POST /api/checkout/stripe?amount=500",
        "POST /api/checkout/liqpay?amount=500"
    }
}));

var apiGroup = app.MapGroup("/api");

// Ін'єкція сервісу безпосередньо в обробник ендпоінту
apiGroup.MapGet("/products", (ICatalogService catalog) =>
{
    return Results.Ok(catalog.GetProducts());
});

apiGroup.MapGet("/products/{id:int}", (int id, ICatalogService catalog) =>
{
    var product = catalog.GetById(id);
    return product is not null ? Results.Ok(product) : Results.NotFound(new { Message = $"Товар з ID {id} не знайдено." });
});

// Ін'єкція Keyed Service за атрибутом [FromKeyedServices]
apiGroup.MapPost("/checkout/{provider}", async (
    string provider,
    [FromQuery] decimal amount,
    IServiceProvider serviceProvider,
    CancellationToken ct) =>
{
    if (amount <= 0)
    {
        return Results.BadRequest(new { Error = "Сума платежу повинна бути більшою за нуль." });
    }

    var gateway = serviceProvider.GetKeyedService<IPaymentGateway>(provider.ToLowerInvariant());
    if (gateway is null)
    {
        return Results.BadRequest(new { Error = $"Невідомий платіжний провайдер '{provider}'. Доступні: stripe, liqpay" });
    }

    var result = await gateway.ProcessPaymentAsync(amount, "UAH", ct);
    return Results.Ok(result);
});

app.Run();
