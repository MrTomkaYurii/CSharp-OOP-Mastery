// Посилальний тип передається БЕЗ ref (за значенням).
// Ключова ідея: метод отримує копію посилання.
//  - зміна ЧЛЕНА об'єкта через цю копію — видима зовні;
//  - переприсвоєння самого параметра (new MyClass()) — НЕ видиме зовні,
//    бо змінюється лише локальна копія посилання.

class MyClass
{
    public int Val = 20;
}

class Program
{
    static void RefAsParameter(MyClass f1)
    {
        f1.Val = 50; // Змінюємо поле спільного об'єкта — a1.Val теж стане 50.
        Console.WriteLine($"After member assignment:    {f1.Val}");

        f1 = new MyClass(); // f1 тепер вказує на новий об'єкт; a1 і далі вказує на старий.
        Console.WriteLine($"After new object creation:  {f1.Val}");
    }
    static void Main()
    {
        MyClass a1 = new MyClass();
        Console.WriteLine($"Before method call:         {a1.Val}");

        RefAsParameter(a1);
        // Виведе 50: побачили зміну поля, але не побачили створення нового об'єкта.
        Console.WriteLine($"After method call:          {a1.Val}");
    }
}