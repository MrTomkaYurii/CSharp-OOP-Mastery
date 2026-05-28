using System;

/*
 * ── Доступ до успадкованих членів (Solis, с. 177) ─────────────────────
 *
 * Об'єкт похідного класу містить у собі всі члени базового класу.
 * Тому через змінну похідного типу можна звертатися як до власних
 * членів (Field2, Method2), так і до успадкованих (Field1, Method1) —
 * так, ніби вони оголошені в самому похідному класі.
 *
 * Очікуваний вивід:
 *   Base class -- Method1: base class field
 *   Base class -- Method1: derived class field
 *   Derived class -- Method2: base class field
 *   Derived class -- Method2: derived class field
 */

// ── Базовий клас ─────────────────────────────────────────────────────
class SomeClass
{
    public string Field1 = "base class field ";

    public void Method1(string value)
    {
        Console.WriteLine($"Base class -- Method1: {value}");
    }
}

// ── Похідний клас ────────────────────────────────────────────────────
class OtherClass : SomeClass
{
    public string Field2 = "derived class field";

    public void Method2(string value)
    {
        Console.WriteLine($"Derived class -- Method2: {value}");
    }
}

class Program
{
    static void Main()
    {
        OtherClass oc = new OtherClass();

        // Усі чотири комбінації "метод + поле" доступні через один об'єкт похідного класу:
        oc.Method1(oc.Field1);   // успадкований метод + успадковане поле
        oc.Method1(oc.Field2);   // успадкований метод + власне поле
        oc.Method2(oc.Field1);   // власний метод      + успадковане поле
        oc.Method2(oc.Field2);   // власний метод      + власне поле
    }
}
