// Приклад демонструє інтерфейс ICloneable.
// ICloneable дозволяє класу самостійно визначити, як створювати
// свою копію через метод Clone(). Тут показано просту (поверхневу)
// реалізацію - для класу без посилальних полів вона фактично
// створює повністю незалежний клон об'єкта.
public class Person : ICloneable
{
    public string Name { get; set; }
    public int Age { get; set; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }

    // Реалізація методу Clone() з інтерфейсу ICloneable
    public object Clone()
    {
        // Створюємо новий об'єкт Person і копіюємо в нього значення полів
        return new Person(Name, Age);
    }
}

class Program
{
    static void Main()
    {
        Person originalPerson = new Person("Alice", 30);

        // Клонуємо об'єкт: Clone() повертає object, тому результат
        // потрібно явно привести до типу Person
        Person clonedPerson = (Person)originalPerson.Clone();

        Console.WriteLine($"Original: {originalPerson.Name}, {originalPerson.Age}");
        Console.WriteLine($"Cloned: {clonedPerson.Name}, {clonedPerson.Age}");
    }
}
