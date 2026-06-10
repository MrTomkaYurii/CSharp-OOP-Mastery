// Приклад 482: коваріантність (модифікатор out) в узагальненому інтерфейсі.
// IMyIfc<Dog> можна присвоїти змінній IMyIfc<Animal>, бо T використовується лише як результат.

class Animal { public required string Name; }   // required — обов'язково задати при створенні
class Dog : Animal { };

interface IMyIfc<out T>                 // out — T лише повертається (коваріантний)
{
    T GetFirst();
}

class SimpleReturn<T> : IMyIfc<T>
{
    public T[] items = new T[2];
    public T GetFirst() { return items[0]; }
}

class Program
{
    static void DoSomething(IMyIfc<Animal> returner)
    {
        Console.WriteLine(returner.GetFirst().Name);
    }

    static void Main()
    {
        SimpleReturn<Dog> dogReturner = new SimpleReturn<Dog>();
        dogReturner.items[0] = new Dog() { Name = "Avonlea" };

        IMyIfc<Animal> animalReturner = dogReturner;    // Коваріантне приведення Dog → Animal

        DoSomething(dogReturner);                       // Працює й без явної змінної animalReturner
    }
}
