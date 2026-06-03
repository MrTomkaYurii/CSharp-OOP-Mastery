// Приклад демонструє інтерфейс IComparable.
// Клас Person реалізує CompareTo, щоб можна було порівнювати
// двох людей за віком і визначати, хто "менший" чи "більший"
// у сенсі природного порядку сортування об'єктів цього типу.
using System;

public class Person : IComparable
{
    public string Name { get; set; }
    public int Age { get; set; }

    // Реалізація методу CompareTo з інтерфейсу IComparable
    public int CompareTo(object obj)
    {
        // За угодою: будь-який об'єкт вважається "більшим" за null
        if (obj == null) return 1;

        if (obj is Person otherPerson)
        {
            // Порівнюємо об'єкти за віком (Age)
            return this.Age.CompareTo(otherPerson.Age);
        }
        else
        {
            // Порівнювати Person можна лише з іншим Person
            throw new ArgumentException("Object is not a Person");
        }
    }
}

class Program
{
    static void Main()
    {
        Person person1 = new Person { Name = "Alice", Age = 30 };
        Person person2 = new Person { Name = "Bob", Age = 25 };

        // Викликаємо CompareTo напряму, щоб отримати результат порівняння віку
        int result = person1.CompareTo(person2);

        if (result < 0)
            Console.WriteLine($"{person1.Name} is younger than {person2.Name}");
        else if (result > 0)
            Console.WriteLine($"{person1.Name} is older than {person2.Name}");
        else
            Console.WriteLine($"{person1.Name} and {person2.Name} are the same age");
    }
}
