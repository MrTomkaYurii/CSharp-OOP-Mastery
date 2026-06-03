// Приклад демонструє реалізацію стандартного вбудованого інтерфейсу IComparable
// у власному класі. Це продовження ідеї з попереднього прикладу (p390): щоб
// Array.Sort() міг відсортувати масив об'єктів довільного (нашого) класу,
// клас має явно реалізувати CompareTo() — метод, через який Array.Sort()
// порівнює елементи між собою, не знаючи нічого про MyClass, окрім того,
// що це IComparable.
using System;

class MyClass : IComparable                  // Клас реалізує інтерфейс IComparable.
{
    public int TheValue;
    // Реалізація методу інтерфейсу: саме через нього Array.Sort() порівнюватиме
    // об'єкти MyClass між собою (менше / більше / рівні).
    public int CompareTo(object obj)
    {
        MyClass mc = (MyClass)obj;

        if (this.TheValue < mc.TheValue)
            return -1;
        if (this.TheValue > mc.TheValue)
            return 1;

        return 0;
    }
}

class Program
{
    static void PrintOut(string s, MyClass[] mc)
    {
        Console.Write(s);
        foreach (var m in mc)
            Console.Write($"{m.TheValue} ");

        Console.WriteLine("");
    }
    static void Main()
    {
        var myInt = new[] { 20, 4, 16, 9, 2 };

        MyClass[] mcArr = new MyClass[5];      // Створюємо масив об'єктів MyClass.
        for (int i = 0; i < 5; i++)          // Заповнюємо масив значеннями.
        {
            mcArr[i] = new MyClass();
            mcArr[i].TheValue = myInt[i];
        }

        PrintOut("Initial Order:  ", mcArr); // Виводимо початковий порядок.

        // Array.Sort викликає CompareTo() кожного елемента через тип IComparable,
        // тому працює однаково для будь-якого класу, що реалізує цей інтерфейс.
        Array.Sort(mcArr);                   // Сортуємо масив об'єктів.
        PrintOut("Sorted Order:   ", mcArr); // Виводимо відсортований порядок.
    }
}