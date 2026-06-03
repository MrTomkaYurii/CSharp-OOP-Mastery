// Приклад показує, що метод можна викликати як через посилання на клас,
// так і через посилання на інтерфейс, який цей клас реалізує.
// Демонструється явне приведення (cast) посилання на об'єкт класу
// до типу інтерфейсу IIfc1 - при цьому виконується той самий метод,
// оскільки інтерфейсне посилання просто вказує на той самий об'єкт.
using System;

interface IIfc1
{
    void PrintOut(string s);
}

class MyClass : IIfc1
{
    public void PrintOut(string s)
    {
        Console.WriteLine($"Calling through: {s}");
    }
}

class Program
{
    static void Main()
    {
        MyClass mc = new MyClass();   // Створення об'єкта класу.
        mc.PrintOut("object");      // Виклик методу через посилання на клас.

        IIfc1 ifc = (IIfc1)mc;       // Приведення посилання на клас до посилання на інтерфейс.
        ifc.PrintOut("interface");  // Виклик того ж методу, але через інтерфейсний тип.
    }
}