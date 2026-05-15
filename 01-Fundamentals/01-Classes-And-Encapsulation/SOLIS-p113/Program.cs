// Локальна ref-змінна (ref local): y стає псевдонімом (другим ім'ям) для x,
// а не окремою копією. Обидва імені посилаються на ту саму комірку пам'яті.

class Program
{
    static void Main()
    {
        int x = 2;
        ref int y = ref x;   // y тепер "вказує" на ту саму змінну, що й x

        Console.WriteLine($"x = {x}, y = {y}");   // 2, 2
        x = 5;                                    // змінюємо через x
        Console.WriteLine($"x = {x}, y = {y}");   // 5, 5 — y "бачить" зміну
        y = 6;                                    // змінюємо через y
        Console.WriteLine($"x = {x}, y = {y}");   // 6, 6 — x теж змінився
    }
}