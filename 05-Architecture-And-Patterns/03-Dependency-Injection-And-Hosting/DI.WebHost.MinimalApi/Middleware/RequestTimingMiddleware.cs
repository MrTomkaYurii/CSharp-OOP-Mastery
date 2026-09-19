using System.Diagnostics;

namespace DI.WebHost.MinimalApi.Middleware;

/// <summary>
/// Фабричне middleware (реалізує IMiddleware), яке реєструється в DI-контейнері
/// та може інжектувати Scoped-залежності безпосередньо в свій конструктор.
/// </summary>
public class RequestTimingMiddleware : IMiddleware
{
    private readonly ILogger<RequestTimingMiddleware> _logger;

    public RequestTimingMiddleware(ILogger<RequestTimingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var sw = Stopwatch.StartNew();
        context.Response.OnStarting(() =>
        {
            sw.Stop();
            context.Response.Headers["X-Execution-Time-Ms"] = sw.ElapsedMilliseconds.ToString();
            return Task.CompletedTask;
        });

        _logger.LogInformation("Початок обробки HTTP {Method} {Path}", context.Request.Method, context.Request.Path);
        await next(context);
        _logger.LogInformation("Завершено обробку HTTP {Method} {Path} за {Elapsed} мс зі статусом {StatusCode}",
            context.Request.Method, context.Request.Path, sw.ElapsedMilliseconds, context.Response.StatusCode);
    }
}
