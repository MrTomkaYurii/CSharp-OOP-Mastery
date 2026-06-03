// Приклад демонструє явну (explicit) реалізацію інтерфейсів.
// На відміну від попередніх прикладів, тут для IIfc1 і IIfc2
// написано ОКРЕМІ реалізації методу PrintOut, а не одну спільну.
// Явна реалізація ховає метод від типу класу: викликати PrintOut
// можна лише через посилання відповідного інтерфейсу (IIfc1 або IIfc2),
// а не напряму через змінну типу MyClass.
interface IIfc1 { void PrintOut(string s); }        // Оголошення першого інтерфейсу.
interface IIfc2 { void PrintOut(string t); }        // Оголошення другого інтерфейсу.

class MyClass : IIfc1, IIfc2
{
    void IIfc1.PrintOut(string s)                    // Явна реалізація методу саме для IIfc1.
    {
        Console.WriteLine($"IIfc1: {s}");
    }

    void IIfc2.PrintOut(string s)                    // Явна реалізація методу саме для IIfc2.
    {
        Console.WriteLine($"IIfc2: {s}");
    }
}

class Program
{
    static void Main()
    {
        MyClass mc = new MyClass();                     // Створення об'єкта класу.

        IIfc1 ifc1 = (IIfc1)mc;                        // Отримати посилання на IIfc1.
        ifc1.PrintOut("interface 1");                 // Виклик явної реалізації для IIfc1.

        IIfc2 ifc2 = (IIfc2)mc;                        // Отримати посилання на IIfc2.
        ifc2.PrintOut("interface 2");                 // Виклик явної реалізації для IIfc2.
    }
}