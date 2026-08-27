// 🔹 Підключаємо бібліотеку Spectre.Console —
// вона дозволяє красиво оформлювати консольний вивід
using Spectre.Console;

// 🔹 Перевіряємо, чи термінал підтримує альтернативні буфери екрану.
// Alternate screen buffer — це "другий екран" консолі, який відкривається поверх основного.
// Після закриття він зникає, а користувач повертається до попереднього вмісту терміналу.
if (!AnsiConsole.Profile.Capabilities.AlternateBuffer)
{
    // Якщо термінал НЕ підтримує цю функцію —
    // показуємо повідомлення червоним і жовтим кольором.
    AnsiConsole.MarkupLine(
        "[red]Alternate screen buffers are not supported " +
        "by your terminal[/] [yellow]:([/]");

    // Завершуємо виконання програми
    return;
}

// 🔹 Пишемо текст у звичайний буфер (тобто "нормальний всесвіт")
AnsiConsole.Write(new Rule("[yellow]Normal universe[/]")); // горизонтальна лінія з заголовком
AnsiConsole.Write(new Panel("Hello World!"));               // простий панельний блок
AnsiConsole.MarkupLine("[grey]Press a key to continue[/]"); // інструкція сірим кольором

// Очікуємо натискання будь-якої клавіші
AnsiConsole.Console.Input.ReadKey(true);

// 🔹 Переходимо у "альтернативний буфер"
// Усе, що відбувається всередині цього блоку, з’явиться в окремому тимчасовому екрані.
// Коли блок завершиться — цей буфер очиститься і користувач повернеться назад.
AnsiConsole.AlternateScreen(() =>
{
    // 🔹 Тепер ми в іншому екранному буфері — ніби "дзеркальний всесвіт"
    AnsiConsole.Write(new Rule("[red]Mirror universe[/]"));               // новий заголовок
    AnsiConsole.Write(new Panel("[red]Welcome to the upside down![/]"));  // нова панель
    AnsiConsole.MarkupLine("[grey]Press a key to return[/]");             // повідомлення сірим кольором

    // Чекаємо натискання клавіші перед поверненням у попередній екран
    AnsiConsole.Console.Input.ReadKey(true);
});

// 🔹 Після виходу з AlternateScreen()
// консоль автоматично повертається до початкового стану
// і знову показує "Normal universe"
