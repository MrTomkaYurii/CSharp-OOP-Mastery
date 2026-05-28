using System;

/*
 * ── Абстрактні класи та абстрактні методи (Solis, с. 207) ────────────
 *
 * Абстрактний клас:
 *   • не можна створити оператором `new` — лише успадкувати;
 *   • може містити як звичайні члени з реалізацією (IdentifyBase),
 *     так і абстрактні члени без тіла (IdentifyDerived).
 *
 * Похідний неабстрактний клас ЗОБОВ'ЯЗАНИЙ надати реалізацію (`override`)
 * для кожного абстрактного члена.
 *
 * Очікуваний вивід:
 *   I am AbClass
 *   I am DerivedClass
 */

// ── Абстрактний базовий клас ────────────────────────────────────────
abstract class AbClass
{
    // Звичайний метод із реалізацією — успадковується як є.
    public void IdentifyBase()
    {
        Console.WriteLine("I am AbClass");
    }

    // Абстрактний метод — лише оголошення, без тіла.
    public abstract void IdentifyDerived();
}

// ── Похідний клас ────────────────────────────────────────────────────
class DerivedClass : AbClass
{
    // Обов'язкова реалізація абстрактного методу.
    public override void IdentifyDerived()
    {
        Console.WriteLine("I am DerivedClass");
    }
}

class Program
{
    static void Main()
    {
        // AbClass a = new AbClass();   // ПОМИЛКА: не можна створити екземпляр
        // a.IdentifyDerived();         // абстрактного класу.

        DerivedClass b = new DerivedClass();

        b.IdentifyBase();      // успадкований звичайний метод
        b.IdentifyDerived();   // реалізація абстрактного методу
    }
}
