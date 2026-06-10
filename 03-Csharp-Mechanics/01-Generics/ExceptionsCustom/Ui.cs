namespace ExceptionsCustom;

// Допоміжний клас для кольорового виводу в консоль
static class Ui
{
    public static void Title(string text)
    {
        Console.WriteLine();
        Print(ConsoleColor.Cyan, $"=== {text} ===");
    }

    public static void Info(string text) => Console.WriteLine(text);

    public static void Ok(string text) => Print(ConsoleColor.Green, text);

    public static void Bad(string text) => Print(ConsoleColor.Red, text);

    public static void Note(string text) => Print(ConsoleColor.Yellow, text);

    static void Print(ConsoleColor color, string text)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}
