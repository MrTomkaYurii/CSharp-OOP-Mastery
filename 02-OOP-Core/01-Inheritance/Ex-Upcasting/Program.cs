using System;

/*
 * ── Висхідне приведення типів (Upcasting) ─────────────────────────────
 *
 * Upcasting — це приведення посилання похідного класу до типу базового
 * класу. Воно завжди безпечне й виконується неявно, бо об'єкт похідного
 * класу "є" також об'єктом базового класу.
 *
 * Головне обмеження: через посилання базового типу видно ЛИШЕ члени,
 * оголошені в базовому класі. Власні методи похідного класу стають
 * недоступними, доки ми не виконаємо зворотне (низхідне) приведення.
 *
 * Очікуваний вивід:
 *   Animal speaks
 *   Animal speaks
 *   Animal speaks
 *   Animal speaks
 *   Animal speaks
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
    // Метод, який є лише в Dog і недоступний через посилання типу Animal.
    public void Bark()
    {
        Console.WriteLine("Dog barks");
    }
}

class Cat : Animal
{
    // Метод, який є лише в Cat і недоступний через посилання типу Animal.
    public void Meow()
    {
        Console.WriteLine("Cat meows");
    }
}

class Program
{
    static void Main()
    {
        // 1. Приводимо об'єкт Dog до посилання типу Animal (неявний upcast).
        Animal animal1 = new Dog();
        animal1.Speak();   // Викликається Animal.Speak()
        // animal1.Bark();  // ПОМИЛКА: тип посилання — Animal, метод Bark() йому невідомий.

        // 2. Те саме з об'єктом Cat.
        Animal animal2 = new Cat();
        animal2.Speak();   // Викликається Animal.Speak()
        // animal2.Meow();  // ПОМИЛКА: тип посилання — Animal, метод Meow() йому невідомий.

        // 3. Масив похідних об'єктів можна трактувати як масив базового типу.
        Animal[] animals =
        {
            new Dog(),
            new Dog(),
            new Dog()
        };

        foreach (Animal animal in animals)
        {
            animal.Speak();   // Для кожного елемента викликається Animal.Speak()
            // animal.Bark();  // ПОМИЛКА з тієї ж причини, що й вище.
        }
    }
}
