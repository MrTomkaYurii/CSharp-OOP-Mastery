using System;

/*
 * ── Низхідне приведення типів (Downcasting) ───────────────────────────
 *
 * Downcasting — це приведення посилання базового типу назад до
 * похідного типу. Компілятор не може гарантувати його коректність,
 * тому воно завжди явне: (Dog)animal.
 *
 * Якщо реальний тип об'єкта не збігається з цільовим, приведення
 * кидає InvalidCastException. Щоб цього уникнути, перед приведенням
 * перевіряють тип оператором `is` (або одразу використовують
 * `if (animal is Dog dog)`).
 *
 * Очікуваний вивід (другий рядок залежить від випадкового вибору
 * у GetRandomAnimal; випадок 2 з Cat не друкує нічого):
 *   Dog barks
 *   Dog barks   АБО   Cat meows
 */

// ── Базовий клас ─────────────────────────────────────────────────────
class Animal
{
    public void Speak()
    {
        Console.WriteLine("Animal speaks");
    }
}

// ── Похідні класи ────────────────────────────────────────────────────
class Dog : Animal
{
    public void Bark()
    {
        Console.WriteLine("Dog barks");
    }
}

class Cat : Animal
{
    public void Meow()
    {
        Console.WriteLine("Cat meows");
    }
}

class Program
{
    static void Main()
    {
        // 1. Об'єкт Dog "схований" за посиланням типу Animal.
        Animal animal = new Dog();

        // Перевіряємо реальний тип і лише тоді робимо низхідне приведення.
        if (animal is Dog)
        {
            Dog dog = (Dog)animal;
            dog.Bark();   // Спрацює: усередині справді Dog.
        }

        // 2. Тут за посиланням Animal схований Cat.
        Animal anotherAnimal = new Cat();

        if (anotherAnimal is Dog)
        {
            Dog anotherDog = (Dog)anotherAnimal;
            anotherDog.Bark();   // НЕ спрацює: перевірка `is Dog` повертає false.
        }

        // 3. Тип об'єкта відомий лише під час виконання — обираємо гілку за `is`.
        Animal someAnimal = GetRandomAnimal();

        if (someAnimal is Dog)
        {
            Dog someDog = (Dog)someAnimal;
            someDog.Bark();
        }
        else if (someAnimal is Cat)
        {
            Cat someCat = (Cat)someAnimal;
            someCat.Meow();
        }
    }

    // Повертає Dog або Cat випадковим чином — тип відомий лише під час виконання.
    static Animal GetRandomAnimal()
    {
        Random random = new Random();

        if (random.Next(0, 2) == 0)
            return new Dog();

        return new Cat();
    }
}
