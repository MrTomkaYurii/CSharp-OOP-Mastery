// Базовий приклад інтерфейсу в C#: два зовсім різні за внутрішньою структурою
// класи (CA і CB) реалізують один і той самий контракт IInfo. Це демонструє
// головну ідею інтерфейсу — уніфікацію різних реалізацій під спільний тип,
// щоб код, який працює з IInfo (метод PrintInfo), не залежав від того,
// як саме зберігаються дані всередині конкретного класу.
using System;

// Інтерфейс IInfo описує лише "що вміє робити" об'єкт (отримати ім'я та вік),
// але нічого не каже про те, як саме ці дані зберігаються всередині класу.
interface IInfo
{
    string GetName();
    string GetAge();
}

// CA зберігає ім'я одним полем Name і вік як int.
class CA : IInfo
{
    public string Name;
    public int Age;
    // Реалізація методів інтерфейсу — просто повертають наявні поля.
    public string GetName() { return Name; }
    public string GetAge() { return Age.ToString(); }
}

// CB зберігає дані інакше: ім'я розбите на First/Last, а вік — double.
// Незважаючи на іншу внутрішню структуру, клас все одно реалізує IInfo,
// формуючи потрібний результат "на льоту" всередині методів.
class CB : IInfo
{
    public string First;
    public string Last;
    public double PersonsAge;
    public string GetName() { return First + " " + Last; }
    public string GetAge() { return PersonsAge.ToString(); }
}

class Program
{
    // Метод приймає параметр типу інтерфейсу IInfo, а не конкретного класу.
    // Завдяки цьому сюди можна передати як об'єкт CA, так і CB — виклик
    // item.GetName()/item.GetAge() відбувається через інтерфейсний тип,
    // а фактично виконується код того класу, чий об'єкт передано (поліморфізм).
    static void PrintInfo(IInfo item)
    {
        Console.WriteLine("Name: {0}, Age {1}", item.GetName(), item.GetAge());
    }

    static void Main()
    {
        CA a = new CA() { Name = "John Doe", Age = 35 };
        CB b = new CB() { First = "Jane", Last = "Doe", PersonsAge = 33 };

        // Обидва об'єкти передаються в один і той самий метод через IInfo,
        // хоча є екземплярами різних класів з різною внутрішньою реалізацією.
        PrintInfo(a);
        PrintInfo(b);
    }
}
