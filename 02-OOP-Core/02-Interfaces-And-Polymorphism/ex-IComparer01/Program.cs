// Приклад демонструє інтерфейс IComparer.
// На відміну від IComparable (де правило порівняння "вшите" в сам клас),
// IComparer виносить логіку порівняння в окремий зовнішній клас-компаратор.
// Це дозволяє мати кілька різних способів порівняння одних і тих самих
// об'єктів, не змінюючи сам клас Person.
using System;
using System.Collections;

public class Person
{
    public string Name { get; set; }
    public int Age { get; set; }
}

// Окремий клас-компаратор, що реалізує IComparer
// і задає правило порівняння двох об'єктів Person за віком
public class AgeComparer : IComparer
{
    // Реалізація методу Compare з інтерфейсу IComparer
    public int Compare(object x, object y)
    {
        if (x is Person personX && y is Person personY)
        {
            // Порівнюємо за віком
            return personX.Age.CompareTo(personY.Age);
        }
        else
        {
            // Компаратор розрахований лише на об'єкти типу Person
            throw new ArgumentException("Objects are not of type Person");
        }
    }
}

class Program
{
    static void Main()
    {
        Person person1 = new Person { Name = "Alice", Age = 30 };
        Person person2 = new Person { Name = "Bob", Age = 25 };

        // Створення екземпляра класу-компаратора AgeComparer
        AgeComparer ageComparer = new AgeComparer();

        // Порівняння виконується через окремий компаратор, а не через сам об'єкт Person
        int result = ageComparer.Compare(person1, person2);

        if (result < 0)
            Console.WriteLine($"{person1.Name} is younger than {person2.Name}");
        else if (result > 0)
            Console.WriteLine($"{person1.Name} is older than {person2.Name}");
        else
            Console.WriteLine($"{person1.Name} and {person2.Name} are the same age");
    }
}
