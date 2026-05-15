using System;

// Те саме, що й p105, але з "inline out variables" (C# 7):
// змінну для out-параметра можна оголосити прямо у виклику — out MyClass a1, out int a2.
// Після виклику ці змінні доступні в решті методу.

class MyClass
{
    public int Val = 20; // Ініціалізація поля значенням 20.
}

class Program
{
    static void MyMethod(out MyClass f1, out int f2)
    {
        f1 = new MyClass(); // Створення об'єкта класу.
        f1.Val = 25; // Присвоєння значення полю класу.
        f2 = 15; // Присвоєння значення параметру типу int.
    }

    static void Main()
    {
        // a1 та a2 оголошуються тут же, у списку аргументів.
        MyMethod(out MyClass a1, out int a2);   // Виклик методу.
        Console.WriteLine(a2);                  // a2 вже видима і має значення 15

        a2 += 5;                                  // Звичайна локальна змінна — можна змінювати
        Console.WriteLine(a2);
    }
}