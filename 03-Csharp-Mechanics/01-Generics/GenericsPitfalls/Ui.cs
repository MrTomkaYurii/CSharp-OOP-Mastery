namespace GenericsPitfalls;

// Допоміжний клас для кольорового виводу в консоль
static class Ui
{
    public static void Title(string text)
    {
        Console.WriteLine();
        Print(ConsoleColor.Cyan, $"=== {text} ===");
    }

    public static void Info(string text) => Console.WriteLine(text);

    public static void Bad(string text) => Print(ConsoleColor.Red, text);

    public static void Good(string text) => Print(ConsoleColor.Green, text);

    public static void Fix(string text) => Print(ConsoleColor.Yellow, "Виправлення: " + text);

    public static void Exception(Exception ex) =>
        Bad($"  [ВИНЯТОК] {ex.GetType().Name}: {ex.Message}");

    // Опис помилки компіляції: сам код не збирається, тому він захований за #if
    public static void CompileError(string symbol, string wrongCode, string errorText)
    {
        Bad("Помилка КОМПІЛЯЦІЇ (код захований за директивою #if, тому програма збирається):");
        Console.WriteLine($"    {wrongCode}");
        Console.WriteLine($"    -> {errorText}");
        Info($"  Щоб побачити її самому: розкоментуйте  // #define {symbol}  на початку Demos\\CompileErrors.cs");
    }

    static void Print(ConsoleColor color, string text)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}
