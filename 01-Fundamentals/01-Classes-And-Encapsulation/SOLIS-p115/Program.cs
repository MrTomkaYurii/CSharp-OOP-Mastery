// Поєднання ref-параметрів і ref return: метод повертає посилання на ту з двох
// змінних, що більша. Далі через ref local max можна змінювати саме цю змінну.

using static System.Console;

class Program
{
    static ref int Max(ref int p1, ref int p2)
    {
        if (p1 > p2)
            return ref p1; // Повертаємо посилання, а не значення.
        else
            return ref p2; // Повертаємо посилання, а не значення.
    }
    static void Main()
    {
        int v1 = 10;
        int v2 = 20;
        WriteLine("Start");
        WriteLine($"v1: {v1}, v2: {v2}\n");

        ref int max = ref Max(ref v1, ref v2);   // max — псевдонім для v2 (бо 20 > 10)
        WriteLine("After assignment");
        WriteLine($"max: {max}\n");

        max++;   // Інкремент через псевдонім змінює саме v2.
        WriteLine("After increment");
        WriteLine($"max: {max}, v1: {v1}, v2: {v2}");   // max: 21, v1: 10, v2: 21
    }
}