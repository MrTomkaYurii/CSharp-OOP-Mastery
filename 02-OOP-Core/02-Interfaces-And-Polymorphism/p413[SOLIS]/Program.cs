// Приклад показує практичну користь інтерфейсів: не всі похідні класи
// базового класу Animal повинні мати одну й ту саму додаткову поведінку.
// Інтерфейс ILiveBirth реалізують лише ті класи (Cat, Dog), тварини
// яких народжуються живими; клас Bird його не реалізує.
// За допомогою оператора "as" перевіряється під час виконання,
// чи підтримує конкретний об'єкт масиву інтерфейс ILiveBirth,
// і лише тоді викликається його метод.
using System;

interface ILiveBirth // Інтерфейс "живонародження" - опціональна поведінка не для всіх тварин.
{
    string BabyCalled();
}

class Animal { } // Базовий клас Animal.

class Cat : Animal, ILiveBirth // Клас Cat успадковує Animal і реалізує ILiveBirth.
{
    string ILiveBirth.BabyCalled()
    {
        return "kitten";
    }
}

class Dog : Animal, ILiveBirth // Клас Dog успадковує Animal і реалізує ILiveBirth.
{
    string ILiveBirth.BabyCalled()
    {
        return "puppy";
    }
}

class Bird : Animal // Клас Bird - лише Animal, без ILiveBirth (пташенята з яєць).
{
}

class Program
{
    static void Main()
    {
        Animal[] animalArray = new Animal[3];        // Масив базового типу Animal.

        animalArray[0] = new Cat();                  // Додати об'єкт Cat.
        animalArray[1] = new Bird();                 // Додати об'єкт Bird.
        animalArray[2] = new Dog();                  // Додати об'єкт Dog.

        foreach (Animal a in animalArray)          // Перебір масиву елементів базового типу.
        {
            ILiveBirth b = a as ILiveBirth;           // Спроба привести до ILiveBirth; null, якщо не підтримується.
            if (b != null)
                Console.WriteLine($"Baby is called: {b.BabyCalled()}");
        }
    }
}