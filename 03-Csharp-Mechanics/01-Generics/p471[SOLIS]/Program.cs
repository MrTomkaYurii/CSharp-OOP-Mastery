// Приклад 471: узагальнений інтерфейс, реалізований узагальненим класом.

interface IMyIfc<T>                 // Узагальнений інтерфейс
{
    T ReturnIt(T inValue);
}

class Simple<S> : IMyIfc<S>         // Узагальнений клас
{
    public S ReturnIt(S inValue)     // Реалізація узагальненого інтерфейсу
    {
        return inValue;
    }
}

class Program
{
    static void Main()
    {
        var trivInt = new Simple<int>();
        var trivString = new Simple<string>();

        Console.WriteLine($"{trivInt.ReturnIt(5)}");
        Console.WriteLine($"{trivString.ReturnIt("Hi there.")}");
    }
}
