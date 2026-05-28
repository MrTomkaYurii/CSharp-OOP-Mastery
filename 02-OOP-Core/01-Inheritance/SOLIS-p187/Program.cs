using System;

/*
 * ── Перевизначення крізь кілька рівнів успадкування (Solis, с. 187) ──
 *
 * Ланцюжок: MyBaseClass → MyDerivedClass → SecondDerived.
 * Кожен рівень перевизначає віртуальний метод через `override`, тому
 * "найпохіднішою" реалізацією для об'єкта SecondDerived є саме
 * SecondDerived.Print(). Вона й викликається незалежно від того,
 * посилання якого типу ми використовуємо.
 *
 * Очікуваний вивід:
 *   This is the second derived class.
 *   This is the second derived class.
 */

// ── Базовий клас ─────────────────────────────────────────────────────
class MyBaseClass
{
    public virtual void Print()
    {
        Console.WriteLine("This is the base class.");
    }
}

// ── Перший рівень успадкування ───────────────────────────────────────
class MyDerivedClass : MyBaseClass
{
    public override void Print()
    {
        Console.WriteLine("This is the derived class.");
    }
}

// ── Другий рівень успадкування ───────────────────────────────────────
class SecondDerived : MyDerivedClass
{
    public override void Print()
    {
        Console.WriteLine("This is the second derived class.");
    }
}

class Program
{
    static void Main()
    {
        SecondDerived derived = new SecondDerived();
        MyBaseClass mybc = (MyBaseClass)derived;   // приводимо до найбазовішого типу

        derived.Print();   // "This is the second derived class."
        mybc.Print();      // теж "second derived" — поліморфізм працює крізь усі рівні
    }
}
