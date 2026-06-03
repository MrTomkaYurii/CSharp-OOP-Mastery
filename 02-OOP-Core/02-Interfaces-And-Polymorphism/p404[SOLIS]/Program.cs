// Продовження попереднього прикладу: клас реалізує два інтерфейси
// однією спільною реалізацією методу PrintOut, а тут додатково
// демонструється, що викликати цей метод можна трьома способами -
// через сам об'єкт класу, через посилання на IIfc1 і через посилання
// на IIfc2. У всіх випадках виконується той самий код методу.
using System;

interface IIfc1 // Оголошення першого інтерфейсу.
{
    void PrintOut(string s);
}

interface IIfc2 // Оголошення другого інтерфейсу.
{
    void PrintOut(string s);
}

class MyClass : IIfc1, IIfc2 // Клас реалізує обидва інтерфейси.
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
        MyClass mc = new MyClass();

        IIfc1 ifc1 = (IIfc1)mc;               // Отримати посилання на IIfc1.
        IIfc2 ifc2 = (IIfc2)mc;               // Отримати посилання на IIfc2.

        mc.PrintOut("object");               // Виклик через посилання на клас.

        ifc1.PrintOut("interface 1");        // Виклик через посилання на IIfc1.
        ifc2.PrintOut("interface 2");        // Виклик через посилання на IIfc2.
    }
}