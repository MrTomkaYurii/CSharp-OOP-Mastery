using System;

/*
 * ── `new` розриває ланцюжок перевизначень (Solis, с. 188) ────────────
 *
 * Ланцюжок: MyBaseClass → MyDerivedClass (override) → SecondDerived (new).
 *
 * SecondDerived ховає метод через `new`, а не перевизначає його. Тому
 * в таблиці віртуальних методів "останнім" перевизначенням лишається
 * MyDerivedClass.Print().
 *
 *   derived.Print()  → тип змінної SecondDerived → "This is the second derived class."
 *   mybc.Print()     → віртуальний виклик зупиняється на MyDerivedClass
 *                      → "This is the first derived class."
 *
 * Очікуваний вивід:
 *   This is the second derived class.
 *   This is the first derived class.
 */

// ── Базовий клас ─────────────────────────────────────────────────────
class MyBaseClass
{
    public virtual void Print()
    {
        Console.WriteLine("This is the base class.");
    }
}

// ── Перший рівень: перевизначає метод (override) ─────────────────────
class MyDerivedClass : MyBaseClass
{
    public override void Print()
    {
        Console.WriteLine("This is the first derived class.");
    }
}

// ── Другий рівень: ХОВАЄ метод (new), а не перевизначає ──────────────
class SecondDerived : MyDerivedClass
{
    public new void Print()
    {
        Console.WriteLine("This is the second derived class.");
    }
}

class Program
{
    static void Main()
    {
        SecondDerived derived = new SecondDerived();
        MyBaseClass mybc = (MyBaseClass)derived;

        derived.Print();   // виклик за типом змінної SecondDerived
        mybc.Print();      // віртуальний виклик — але SecondDerived його не перевизначав
    }
}
