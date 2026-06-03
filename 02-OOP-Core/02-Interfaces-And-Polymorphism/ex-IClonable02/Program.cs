// Приклад демонструє інтерфейс ICloneable, але на відміну від
// попереднього прикладу тут виконується ГЛИБОКЕ клонування:
// клас Person містить посилальне поле - список Hobbies, і просте
// копіювання посилання призвело б до того, що оригінал і клон
// ділили б один і той самий список. Тому в Clone() список
// створюється заново, елемент за елементом.
using System;
using System.Collections.Generic;
using System.Linq;

public class Person : ICloneable
{
    public string Name { get; set; }
    public int Age { get; set; }
    public List<string> Hobbies { get; set; }

    public Person(string name, int age)
    {
        Name = name;
        Age = age;
        Hobbies = new List<string>();
    }

    // Реалізація методу Clone() з інтерфейсу ICloneable (глибоке копіювання)
    public object Clone()
    {
        // Створюємо новий об'єкт Person з тими самими простими значеннями
        Person clonedPerson = new Person(Name, Age);

        // Глибоко копіюємо список Hobbies: створюємо новий список
        // і нові рядки-копії, щоб клон не залежав від оригінального списку
        clonedPerson.Hobbies = Hobbies.Select(hobby => string.Copy(hobby)).ToList();

        return clonedPerson;
    }
}

class Program
{
    static void Main()
    {
        Person originalPerson = new Person("Alice", 30);
        originalPerson.Hobbies.Add("Reading");
        originalPerson.Hobbies.Add("Painting");

        // Глибоко клонуємо об'єкт (Clone() повертає object, тому потрібне приведення типу)
        Person clonedPerson = (Person)originalPerson.Clone();

        // Змінюємо оригінал і перевіряємо, що на клон це не впливає
        // (доказ того, що клонування дійсно глибоке)
        originalPerson.Name = "Bob";
        originalPerson.Hobbies.Remove("Reading");

        Console.WriteLine($"Original: {originalPerson.Name}, {string.Join(", ", originalPerson.Hobbies)}");
        Console.WriteLine($"Cloned: {clonedPerson.Name}, {string.Join(", ", clonedPerson.Hobbies)}");
    }
}
