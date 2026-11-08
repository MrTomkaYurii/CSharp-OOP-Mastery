# Основи Spectre.Console: Розмітка, Кольори та Текстові Ефекти

**Spectre.Console** — це сучасна .NET бібліотека, яка перетворює стандартний текстовий термінал на повноцінний графічний інтерфейс користувача (TUI / CLI) з підтримкою кольорів (TrueColor/ANSI), стилізованого тексту, віджетів та інтерактивних меню.

---

## 1. Фасад `AnsiConsole` та розмітка тексту (Markup)

Замість традиційного `Console.WriteLine()`, Spectre.Console використовує статичний клас `AnsiConsole.MarkupLine()`:

```csharp
using Spectre.Console;

// Виведення тексту з кольором та форматуванням
AnsiConsole.MarkupLine("[bold green]Успіх:[/] Операцію виконано коректно!");
AnsiConsole.MarkupLine("[italic red]Помилка:[/] Некоректні вхідні дані.");
AnsiConsole.MarkupLine("Відвідайте наш [link=https://github.com]вебсайт[/].");
```

### Синтаксис розмітки:
- `[bold]`, `[italic]`, `[underline]`, `[strikethrough]`, `[invert]` — модифікатори шрифту.
- `[red]`, `[blue]`, `[yellow]`, `[rgb(255,100,50)]`, `[#ff00aa]` — кольори переднього плану (foreground).
- `[white on blue]` — комбінація тексту і кольору фону.
- `[/]` — закриття останнього відкритого тегу стилізації.
- Екранування дужок: для виведення символу `[` використовується `[[`.

---

## 2. Фіглет та банери (`FigletText`)

Створення великих стилізованих ASCII-заголовків:

```csharp
AnsiConsole.Write(
    new FigletText("C# Mastery")
        .LeftJustified()
        .Color(Color.Yellow));
```

---

## 3. Емодзі та спеціальні символи

Spectre.Console містить повноцінну підтримку Unicode і шорткодів емодзі:

```csharp
AnsiConsole.MarkupLine("Статус: :rocket: Запущено, :warning: Попередження, :check_mark_button: Готово!");
```

---

## 4. Правила та розділювачі (`Rule`)

Горизонтальні лінії з інтегрованими назвами секцій:

```csharp
var rule = new Rule("[yellow]Розділ 1: Інспекція процесу[/]");
rule.LeftJustified();
rule.RuleStyle("grey");
AnsiConsole.Write(rule);
```
