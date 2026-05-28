using System;

/*
 * ── Приховування методу vs тип посилання (Solis, с. 183) ─────────────
 *
 * Метод, оголошений з `new`, лише ХОВАЄ однойменний метод базового класу.
 * Це не поліморфізм: CLR обирає реалізацію за ТИПОМ ЗМІННОЇ на етапі
 * компіляції, а не за реальним типом об'єкта.
 *
 *   derived.Print()  → тип змінної MyDerivedClass  → "This is the derived class."
 *   mybc.Print()     → тип змінної MyBaseClass     → "This is the base class."
 *
 * Порівняйте з прикладом на с. 185, де `virtual`/`override` дають
 * зовсім іншу поведінку.
 *
 * Очікуваний вивід:
 *   This is the derived class.
 *   This is the base class.
 */

// ── Базовий клас ─────────────────────────────────────────────────────
class MyBaseClass
{
    public void Print()
    {
        Console.WriteLine("This is the base class.");
    }
}

// ── Похідний клас ────────────────────────────────────────────────────
class MyDerivedClass : MyBaseClass
{
    public int var1;

    public new void Print()   // ховає MyBaseClass.Print()
    {
        Console.WriteLine("This is the derived class.");
    }
}

class Program
{
    static void Main()
    {
        MyDerivedClass derived = new MyDerivedClass();
        MyBaseClass mybc = (MyBaseClass)derived;   // те саме посилання, але іншого типу

        derived.Print();   // виклик через тип MyDerivedClass
        mybc.Print();      // виклик через тип MyBaseClass
        //mybc.var1 = 5;  // ПОМИЛКА: через посилання базового типу не видно членів похідного класу.
    }
}
