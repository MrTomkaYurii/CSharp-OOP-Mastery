// Приклад 465: узагальнений метод у НЕузагальненому класі.

class Simple                                          // Неузагальнений клас
{
    static public void ReverseAndPrint<T>(T[] arr)     // Узагальнений метод
    {
        Array.Reverse(arr);                            // Розвертає масив на місці
        foreach (T item in arr)                        // Використовуємо аргумент типу T
            Console.Write($"{item}, ");                // Інтерполяція безпечна для null (на відміну від item.ToString())
        Console.WriteLine("");
    }
}

class Program
{
    static void Main()
    {
        // Масиви різних типів
        var intArray = new int[] { 3, 5, 7, 9, 11 };
        var stringArray = new string[] { "first", "second", "third" };
        var doubleArray = new double[] { 3.567, 7.891, 2.345 };

        Simple.ReverseAndPrint<int>(intArray);          // Тип вказано явно
        Simple.ReverseAndPrint(intArray);               // Тип виводиться компілятором (type inference)

        Simple.ReverseAndPrint<string>(stringArray);    // Тип вказано явно
        Simple.ReverseAndPrint(stringArray);            // Тип виводиться компілятором

        Simple.ReverseAndPrint<double>(doubleArray);    // Тип вказано явно
        Simple.ReverseAndPrint(doubleArray);            // Тип виводиться компілятором
    }
}
