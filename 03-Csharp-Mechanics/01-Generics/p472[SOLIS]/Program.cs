// Приклад 472: неузагальнений клас реалізує один узагальнений інтерфейс двічі
// з різними аргументами типу (IMyIfc<int> та IMyIfc<string>).

interface IMyIfc<T>                             // Узагальнений інтерфейс
{
    T ReturnIt(T inValue);
}

class Simple : IMyIfc<int>, IMyIfc<string>      // Неузагальнений клас
{
    public int ReturnIt(int inValue)             // Реалізація для int
    {
        return inValue;
    }

    public string ReturnIt(string inValue)       // Реалізація для string
    {
        return inValue;
    }
}

class Program
{
    static void Main()
    {
        Simple trivial = new Simple();

        // Перевантаження обирається за типом аргументу
        Console.WriteLine($"{trivial.ReturnIt(5)}");
        Console.WriteLine($"{trivial.ReturnIt("Hi there.")}");
    }
}
