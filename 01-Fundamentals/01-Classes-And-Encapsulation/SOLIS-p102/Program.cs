// Той самий сценарій, що й на p100, але тепер параметр передано з ref.
// Тепер f1 — це псевдонім самої змінної a1, а не копія посилання.
// Тому навіть переприсвоєння f1 = new MyClass() змінює a1 у викликаючому коді.

class MyClass
{
    public int Val = 20;
}

class Program
{
    static void RefAsParameter(ref MyClass f1)
    {
        // Присвоєння значення члену об'єкта (a1.Val стане 50).
        f1.Val = 50;
        Console.WriteLine($"After member assignment:   {f1.Val}");

        // Створення нового об'єкта. Через ref a1 тепер теж вказує на цей новий об'єкт,
        // тому його Val (20 за замовчуванням) буде видно і в Main.
        f1 = new MyClass();
        Console.WriteLine($"After new object creation: {f1.Val}");
    }

    static void Main(string[] args)
    {
        MyClass a1 = new MyClass();

        Console.WriteLine($"Before method call:        {a1.Val}");
        RefAsParameter(ref a1);
        // Виведе 20 — на відміну від p100, де було 50.
        Console.WriteLine($"After method call:         {a1.Val}");
    }
}