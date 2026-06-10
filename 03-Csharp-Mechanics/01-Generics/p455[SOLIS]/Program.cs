// Приклад 455: узагальнений клас — стек фіксованого розміру.
// Один код працює для будь-якого типу елементів без boxing та приведення типів.

class MyStack<T>
{
    const int MaxStack = 10;                    // Максимальна місткість стека

    readonly T[] StackArray;                    // Сховище елементів
    int StackPointer = 0;                       // Індекс наступної вільної позиції

    bool IsStackFull { get { return StackPointer >= MaxStack; } }
    bool IsStackEmpty { get { return StackPointer <= 0; } }

    public MyStack()
    {
        StackArray = new T[MaxStack];
    }

    // Додає елемент на вершину. Якщо стек повний — елемент мовчки ігнорується.
    public void Push(T x)
    {
        if (!IsStackFull)
            StackArray[StackPointer++] = x;
    }

    // Знімає елемент з вершини.
    // Увага: для порожнього стека повертає StackArray[0] (default(T) або старе значення),
    // а не кидає виняток — це спрощення навчального прикладу.
    public T Pop()
    {
        return (!IsStackEmpty)
        ? StackArray[--StackPointer]
        : StackArray[0];
    }

    // Виводить вміст стека від вершини до дна
    public void Print()
    {
        for (int i = StackPointer - 1; i >= 0; i--)
            Console.WriteLine($" Value: {StackArray[i]}");
    }
}

class Program
{
    static void Main()
    {
        // Два незалежні закриті типи від одного узагальненого класу
        MyStack<int> StackInt = new MyStack<int>();
        MyStack<string> StackString = new MyStack<string>();

        StackInt.Push(3);
        StackInt.Push(5);
        StackInt.Push(7);
        StackInt.Push(9);
        StackInt.Print();

        StackString.Push("This is fun");
        StackString.Push("Hi there! ");
        StackString.Print();
    }
}
