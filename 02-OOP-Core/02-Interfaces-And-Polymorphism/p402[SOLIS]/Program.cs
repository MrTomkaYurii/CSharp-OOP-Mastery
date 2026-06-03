// Приклад показує, що коли два різних інтерфейси оголошують метод
// з однаковою сигнатурою, клас може реалізувати обидва інтерфейси
// ОДНИМ спільним методом - компілятор вважає такий метод реалізацією
// одразу для IIfc1 і для IIfc2, оскільки їхні сигнатури збігаються.
using System;

interface IIfc1
{
    void PrintOut(string s);
}

interface IIfc2
{
    void PrintOut(string t);
}

class MyClass : IIfc1, IIfc2              // Реалізує обидва інтерфейси.
{
    public void PrintOut(string s)       // Одна реалізація обслуговує обидва інтерфейси одразу.
    {
        Console.WriteLine($"Calling through:  {s}");
    }
}

class Program
{
    static void Main()
    {
        MyClass mc = new MyClass();
        mc.PrintOut("object");
    }
}