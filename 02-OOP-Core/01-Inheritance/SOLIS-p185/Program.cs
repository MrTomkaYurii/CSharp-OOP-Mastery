namespace SOLIS_p185;

/*
 * ── Поліморфізм: `virtual` та `override` (Solis, с. 185) ─────────────
 *
 * Якщо метод базового класу позначено `virtual`, а в похідному —
 * `override`, то виклик завжди спрямовується до реалізації РЕАЛЬНОГО
 * типу об'єкта, навіть коли звертаємось через посилання базового типу.
 *
 *   derived.Print()  → MyDerivedClass  → "This is the derived class."
 *   mybc.Print()     → об'єкт усе одно MyDerivedClass → "This is the derived class."
 *
 * Порівняйте з с. 183 (`new`), де другий рядок був би "This is the base class."
 *
 * Очікуваний вивід:
 *   This is the derived class.
 *   This is the derived class.
 */

// ── Базовий клас ─────────────────────────────────────────────────────
class MyBaseClass
{
    public virtual void Print()
    {
        Console.WriteLine("This is the base class.");
    }
}

// ── Похідний клас ────────────────────────────────────────────────────
class MyDerivedClass : MyBaseClass
{
    public override void Print()   // перевизначає MyBaseClass.Print()
    {
        Console.WriteLine("This is the derived class.");
    }
}

class Program
{
    static void Main()
    {
        MyDerivedClass derived = new MyDerivedClass();
        MyBaseClass mybc = (MyBaseClass)derived;   // тип посилання інший, об'єкт той самий

        derived.Print();   // "This is the derived class."
        mybc.Print();      // теж "This is the derived class." — спрацював поліморфізм
    }
}
