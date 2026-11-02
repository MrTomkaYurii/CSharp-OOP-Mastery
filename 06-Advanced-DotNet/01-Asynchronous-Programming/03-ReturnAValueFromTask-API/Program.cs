// =========================================================================================
// ASP.NET CORE: Конфігурація та запуск хоста Web API
// =========================================================================================
// Налаштовує Kestrel та конвеєр обробки HTTP-запитів (Middleware pipeline).

var builder = WebApplication.CreateBuilder(args);

// БЛОК 1: Реєстрація сервісів MVC / Контролерів
builder.Services.AddControllers();

var app = builder.Build();

// БЛОК 2: Конфігурація HTTP Middleware конвеєра
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// БЛОК 3: Запуск веб-сервера (неблокуючий цикл прослуховування портів)
app.Run();
