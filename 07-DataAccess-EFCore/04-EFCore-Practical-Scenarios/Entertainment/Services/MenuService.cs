using Microsoft.Extensions.Logging;
using Spectre.Console;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Entertainment.Services;

/// <summary>
/// Сервіс для відображення інтерактивного меню та виконання запитів
/// ВИПРАВЛЕНО: Використовує QueryServiceFactory.ExecuteQueryAsync() для правильного scope management
/// </summary>
public class MenuService : IMenuService
{
    private readonly IQueryServiceFactory _queryServiceFactory;
    private readonly ILogger<MenuService> _logger;

    public MenuService(
        IQueryServiceFactory queryServiceFactory,
        ILogger<MenuService> logger)
    {
        _queryServiceFactory = queryServiceFactory;
        _logger = logger;
    }

    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        // Відображаємо привітання
        DisplayWelcomeBanner();

        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                // Отримуємо метадані всіх доступних запитів (БЕЗ створення екземплярів)
                var queryMetadata = _queryServiceFactory.GetAllQueryMetadata().ToList();

                if (!queryMetadata.Any())
                {
                    AnsiConsole.MarkupLine("[red]Не знайдено жодного запиту![/]");
                    break;
                }

                // Створюємо меню вибору
                var choices = queryMetadata.Select(q => q.QueryName).ToList();
                choices.Add("[yellow]Вихід[/]");

                var selection = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[green]Оберіть запит для виконання:[/]")
                        .PageSize(15)
                        .MoreChoicesText("[grey](Прокрутіть вниз для більше варіантів)[/]")
                        .AddChoices(choices));

                // Перевіряємо чи користувач обрав вихід
                if (selection == "[yellow]Вихід[/]")
                {
                    AnsiConsole.MarkupLine("[yellow]Завершення роботи...[/]");
                    break;
                }

                // Знаходимо метадані обраного запиту
                var selectedMetadata = queryMetadata.FirstOrDefault(q => q.QueryName == selection);

                if (selectedMetadata != null)
                {
                    // Виконуємо запит через фабрику (вона створить новий scope)
                    await ExecuteQueryWithProgressAsync(selectedMetadata, cancellationToken);
                }

                // Чекаємо на натискання клавіші перед наступним запитом
                AnsiConsole.WriteLine();
                AnsiConsole.Markup("[grey]Натисніть будь-яку клавішу для продовження...[/]");
                Console.ReadKey(true);
                AnsiConsole.Clear();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Помилка при виконанні меню");
                AnsiConsole.WriteException(ex);

                AnsiConsole.Markup("[grey]Натисніть будь-яку клавішу для продовження...[/]");
                Console.ReadKey(true);
            }
        }
    }

    /// <summary>
    /// Відображає банер привітання
    /// </summary>
    private void DisplayWelcomeBanner()
    {
        AnsiConsole.Clear();

        var rule = new Rule("[yellow]Entertainment Database Query System[/]")
        {
            Justification = Justify.Center
        };
        AnsiConsole.Write(rule);
        AnsiConsole.WriteLine();

        var panel = new Panel(
            "[cyan]Ласкаво просимо до системи запитів бази даних Entertainment![/]\n" +
            "[grey]Оберіть запит з меню нижче для перегляду результатів.[/]")
        {
            Border = BoxBorder.Rounded,
            BorderStyle = new Style(Color.Yellow)
        };

        AnsiConsole.Write(panel);
        AnsiConsole.WriteLine();
    }

    /// <summary>
    /// Виконує запит з відображенням прогресу
    /// ВИПРАВЛЕНО: Викликає фабрику, яка сама керує scope
    /// </summary>
    private async Task ExecuteQueryWithProgressAsync(
        QueryServiceMetadata metadata,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Виконання запиту: {QueryName}", metadata.QueryName);

        await AnsiConsole.Status()
            .Spinner(Spinner.Known.Dots)
            .SpinnerStyle(Style.Parse("green bold"))
            .StartAsync($"[green]Виконання запиту:[/] {metadata.QueryName}...", async ctx =>
            {
                // Фабрика створює scope, виконує запит, і dispose scope
                await _queryServiceFactory.ExecuteQueryAsync(metadata.QueryKey, cancellationToken);
            });

        _logger.LogInformation("Запит завершено: {QueryName}", metadata.QueryName);
    }
}