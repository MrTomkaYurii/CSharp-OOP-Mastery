// Приклад 478: контраваріантність (модифікатор in) в узагальненому делегаті.
// Делегат, що приймає Animal, можна використати там, де потрібен делегат для Dog,
// бо будь-який Dog — це Animal.

class Animal { public int NumberOfLegs = 4; }

class Dog : Animal { }

class Program
{
    delegate void Action1<in T>(T a);       // in — T лише вхідний параметр (контраваріантний)

    static void ActOnAnimal(Animal a)
    {
        Console.WriteLine(a.NumberOfLegs);
    }

    static void Main()
    {
        Action1<Animal> act1 = ActOnAnimal;
        Action1<Dog> dog1 = act1;           // Дозволено завдяки контраваріантності
        dog1(new Dog());
    }
}
