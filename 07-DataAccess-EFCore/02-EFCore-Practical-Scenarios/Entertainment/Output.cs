using Microsoft.EntityFrameworkCore;
using Spectre.Console;
using Spectre.Console.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Entertainment;

/// <summary>
/// Допоміжний статичний клас для виводу результатів
/// </summary>
public static class Output
{
    /// <summary>
    /// Виводить результати запиту у вигляді панелі з SQL та таблицею
    /// ЦЕ ГОЛОВНИЙ МЕТОД ДЛЯ ВИКОРИСТАННЯ!
    /// </summary>
    public static void Results(IQueryable query, string title, IRenderable results)
    {
        // Чекаємо поки Serilog закінчить писати логи
        Thread.Sleep(50);

        // Додаємо порожній рядок
        AnsiConsole.WriteLine();

        AnsiConsole.Write(
            new Panel(
                    new Grid()
                        .AddColumn()
                        .AddRow(new Text(""))
                        .AddRow(new Markup($"[skyblue1]{query.ToQueryString()}[/]"))
                        .AddRow(new Text(""))
                        .AddRow(results)
                )
                .Header(title)
                .BorderColor(Color.Yellow3)
                .RoundedBorder()
        );

        AnsiConsole.WriteLine();
    }
}

/// <summary>
/// Extension методи для зручної роботи з таблицями Spectre.Console
/// </summary>
public static class TableExtensions
{
    /// <summary>
    /// Додає рядки до таблиці з використанням селекторів для кожної колонки
    /// </summary>
    public static Table AddRows<T>(
        this Table table,
        IEnumerable<T> items,
        params Func<T, object>[] columnSelectors)
    {
        foreach (var item in items)
        {
            var columns = columnSelectors
                .Select(selector => selector(item)?.ToString() ?? string.Empty)
                .ToArray();

            table.AddRow(columns);
        }

        return table;
    }

    /// <summary>
    /// Додає колонки до таблиці
    /// </summary>
    public static Table AddColumns(this Table table, params string[] columns)
    {
        foreach (var column in columns)
        {
            table.AddColumn(column);
        }

        return table;
    }
}