# Підмодуль 05: Сучасні консольні інтерфейси зі Spectre.Console (Modern CLI Development)

Цей комплексний підмодуль демонструє повний спектр можливостей розробки професійних консольних інтерфейсів (TUI) та CLI-утиліт за допомогою екосистеми **Spectre.Console**.

---

## Навігація по навчальних матеріалах

1. [Основи Spectre.Console: Розмітка, Кольори та Текстові Ефекти](Docs/01_Spectre_Console_Basics_And_Markup.md)
2. [Інтерактивні діалоги та візуальні віджети](Docs/02_Interactive_Prompts_And_Widgets.md)
3. [Архітектура консольних застосунків: Spectre.Console.Cli](Docs/03_CLI_Commands_And_Architecture.md)

---

## Архітектура та структура проєктів у рішенні

Рішення `SpectreConsoleExamples.sln` містить 37 проєктів, розділених на логічні категорії:

```mermaid
graph TD
    subgraph "Console UI Віджети"
        Widgets["Tables, Trees, Panels, Grids, Charts, Calendars, Canvas"]
        Interactive["Prompt, Progress, Status, Live, LiveTable, AlternateScreen"]
        Styling["Colors, Decorations, Emojis, Figlet, Borders, Rules, Links, Json"]
    end

    subgraph "Cli Архітектура"
        CliApp["Demo, Dynamic, Help, Delegates"]
        Enterprise["Injection (DI Container), Logging (Serilog Enricher), Testing (Spectre.Console.Testing)"]
    end

    Shared["Shared (Кольорові палітри та допоміжні методи)"]
    Shared --> Widgets
    Shared --> Interactive
    Shared --> CliApp
```

### Детальна класифікація проєктів:

| Директорія | Проєкти | Ключові концепції |
| :--- | :--- | :--- |
| **`Console/`** | `AlternateScreen`, `Borders`, `Calendars`, `Canvas`, `Charts`, `Colors`, `Columns`, `Cursor`, `Decorations`, `Emojis`, `Exceptions`, `Figlet`, `Grids`, `Info`, `Json`, `Layout`, `Links`, `Live`, `LiveTable`, `Minimal`, `Panels`, `Paths`, `Progress`, `Prompt`, `Rules`, `Showcase`, `Status`, `Tables`, `Trees` | Відображення розширених TUI-елементів: анімація статусів, прогрес-бари, графіки, JSON-форматування, рендеринг зображень у символи (`Canvas`), інтерактивні опитування. |
| **`Cli/`** | `Demo`, `Dynamic`, `Help`, `Delegates`, `Injection`, `Logging`, `Testing` | Побудова CLI-команд: підкоманди, валідація параметрів, налаштування довідки, впровадження залежностей (IoC), логування та модульне тестування CLI команд. |
| **`Shared/`** | `Shared` | Спільна бібліотека утиліт та палітр для демонстраційних проєктів. |

---

## Як зібрати та запустити

```powershell
# Збірка всього рішення в конфігурації Release
dotnet build "08-Architecture-And-Patterns/05-CLI-Spectre-Console/SpectreConsoleExamples.sln" -c Release

# Запуск інтерактивної вітрини віджетів
dotnet run --project "08-Architecture-And-Patterns/05-CLI-Spectre-Console/Console/Showcase/Showcase.csproj"

# Запуск прикладу CLI-команд
dotnet run --project "08-Architecture-And-Patterns/05-CLI-Spectre-Console/Cli/Demo/Demo.csproj" -- --help
```
