// Приклад 469: узагальнений делегат (multicast).

delegate void MyDelegate<T>(T value);                          // Узагальнений делегат

class Simple
{
    static public void PrintString(string s)                    // Сигнатура відповідає делегату
    {
        Console.WriteLine(s);
    }

    static public void PrintUpperString(string s)               // Сигнатура відповідає делегату
    {
        Console.WriteLine($"{s.ToUpper()}");
    }
}

class Program
{
    static void Main()
    {
        var myDel = new MyDelegate<string>(Simple.PrintString);  // Створюємо екземпляр делегата
        myDel += Simple.PrintUpperString;                        // Додаємо другий метод (список виклику)

        myDel("Hi There.");                                      // Викликаємо обидва методи по черзі
    }
}
