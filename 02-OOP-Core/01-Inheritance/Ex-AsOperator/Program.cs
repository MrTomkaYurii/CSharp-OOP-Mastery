using System;

namespace Ex_AsOperator;

/*
 * ── Оператор `as` проти класичного приведення `(Тип)` ──────────────
 *
 * Обидва способи роблять низхідне приведення (від базового типу до
 * похідного), але поводяться по-різному, коли тип НЕ підходить:
 *
 *   (Dog)animal    — якщо всередині не Dog, кидає InvalidCastException
 *                    (програма аварійно зупиняється, якщо не зловити виняток).
 *
 *   animal as Dog  — якщо всередині не Dog, просто повертає null.
 *                    Винятку немає, тому потім треба перевірити на null.
 *
 * Ще відмінності оператора `as`:
 *   • працює лише зі ссилковими типами та Nullable<T> (з `int` не можна);
 *   • не виконує визначених користувачем перетворень — лише перевірку типу.
 *
 * Очікуваний вивід:
 *   (Dog)someAnimal  ->  спіймали InvalidCastException
 *   someAnimal as Dog  ->  null (це не собака)
 *   Кіт нявкає: Мурчик
 */

class Animal
{
    public string Name;

    public Animal(string name)
    {
        Name = name;
    }
}

class Dog : Animal
{
    public Dog(string name) : base(name)
    {
    }

    public void Bark()
    {
        Console.WriteLine($"Пес гавкає: {Name}");
    }
}

class Cat : Animal
{
    public Cat(string name) : base(name)
    {
    }

    public void Meow()
    {
        Console.WriteLine($"Кіт нявкає: {Name}");
    }
}

class Program
{
    static void Main()
    {
        // Усередині — Cat, але тип змінної — Animal.
        Animal someAnimal = new Cat("Мурчик");

        // ── Варіант 1: класичне приведення (Тип) ──────────────────────
        // Тип не підходить (там Cat, а не Dog) -> буде InvalidCastException.
        try
        {
            Dog wrongDog = (Dog)someAnimal;
            wrongDog.Bark();
        }
        catch (InvalidCastException)
        {
            Console.WriteLine("(Dog)someAnimal  ->  спіймали InvalidCastException");
        }

        // ── Варіант 2: оператор as ───────────────────────────────────
        // Тип не підходить -> повертається null, без винятку.
        // Dog? означає: у змінній може бути об'єкт Dog АБО null.
        Dog? maybeDog = someAnimal as Dog;
        if (maybeDog == null)
        {
            Console.WriteLine("someAnimal as Dog  ->  null (це не собака)");
        }
        else
        {
            maybeDog.Bark();
        }

        // Тут тип збігається (Cat), тому as повертає справжній об'єкт.
        Cat? maybeCat = someAnimal as Cat;
        if (maybeCat != null)
        {
            maybeCat.Meow();
        }

        // int number = someAnimal as int;   // так НЕ можна: as не працює зі звичайним int
    }
}
