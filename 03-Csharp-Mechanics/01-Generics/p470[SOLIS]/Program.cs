// Приклад 470: узагальнений делегат з кількома параметрами типу та значенням, що повертається.
// Примітка: у System уже є вбудований Func<T1, T2, TResult>. Тут власне оголошення
// (у глобальному просторі імен) має пріоритет над імпортованим із System — це лише для навчання.

public delegate TR Func<T1, T2, TR>(T1 p1, T2 p2);                   // Узагальнений делегат

class Simple
{
    static public string PrintString(int p1, int p2)                  // Сигнатура відповідає делегату
    {
        int total = p1 + p2;
        return total.ToString();
    }
}

class Program
{
    static void Main()
    {
        var myDel = new Func<int, int, string>(Simple.PrintString);    // Створюємо екземпляр делегата

        Console.WriteLine($"Total: {myDel(15, 13)}");                // Викликаємо делегат
    }
}
