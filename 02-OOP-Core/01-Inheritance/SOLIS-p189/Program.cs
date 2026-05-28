using System;

/*
 * ── Віртуальні властивості (Solis, с. 189) ──────────────────────────
 *
 * Модифікатори `virtual`/`override` працюють не лише для методів, а й
 * для властивостей. Поліморфізм тут такий самий: викликається аксесор
 * реального типу об'єкта.
 *
 * Зверніть увагу: кожен клас має власне поле `_myInt`. Похідний аксесор
 * читає саме поле похідного класу (10), тому обидва рядки виводу однакові.
 *
 * Очікуваний вивід:
 *   10
 *   10
 */

// ── Базовий клас ─────────────────────────────────────────────────────
class MyBaseClass
{
    private int _myInt = 5;

    public virtual int MyProperty
    {
        get { return _myInt; }
    }
}

// ── Похідний клас ────────────────────────────────────────────────────
class MyDerivedClass : MyBaseClass
{
    private int _myInt = 10;

    public override int MyProperty   // перевизначає властивість базового класу
    {
        get { return _myInt; }
    }
}

class Program
{
    static void Main()
    {
        MyDerivedClass derived = new MyDerivedClass();
        MyBaseClass mybc = (MyBaseClass)derived;

        Console.WriteLine(derived.MyProperty);   // 10
        Console.WriteLine(mybc.MyProperty);      // теж 10 — спрацював поліморфізм властивості
    }
}
