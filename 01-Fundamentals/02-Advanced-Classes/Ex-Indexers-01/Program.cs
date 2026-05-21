// ============================================================================
// Тема: Індексатор, що повертає об'єкт (клас-обгортка над масивом об'єктів)
// ----------------------------------------------------------------------------
// Тут клас Company інкапсулює масив співробітників Person[] і надає до нього
// доступ через індексатор company[i]. Такий підхід приховує внутрішнє
// зберігання: користувач працює з "компанією як зі списком людей", не знаючи,
// що всередині звичайний масив.
// Додатково приклад демонструє компактний синтаксис:
//   * expression-bodied конструктор:  public Person(string name) => Name = name;
//   * expression-bodied акcесори:      get => ...;  set => ...;
//   * автовластивість тільки для читання: public string Name { get; }
// ============================================================================

class Person
{
    public string Name { get; }                       // тільки для читання: задається в конструкторі
    public Person(string name) => Name = name;        // короткий запис тіла конструктора
}

class Company
{
    Person[] personal;                                // приватне сховище — масив співробітників
    public Company(Person[] people) => personal = people;

    // Індексатор: ключ типу int, значення типу Person
    public Person this[int index]
    {
        get => personal[index];                       // company[i]        -> читання елемента масиву
        set => personal[index] = value;               // company[i] = obj  -> заміна елемента (value = obj)
    }
}

class Program
{
    static void Main()
    {
        var microsoft = new Company(new[]{
                                            new Person("Tom"),
                                            new Person("Bob"),
                                            new Person("Sam"),
                                            new Person("Alice")
                                          });

        // Отримуємо об'єкт через індексатор (виклик get, index = 0)
        Person firstPerson = microsoft[0];
        Console.WriteLine(firstPerson.Name);   // Tom

        // Перевстановлюємо об'єкт через індексатор (виклик set, index = 0, value = new Person("Mike"))
        microsoft[0] = new Person("Mike");
        Console.WriteLine(microsoft[0].Name);  // Mike
    }
}

// ----------------------------------------------------------------------------
// Що можна додати:
//  * Перевірку меж index у get/set із зрозумілим повідомленням про помилку.
//  * Властивість Count і метод перебору (foreach) — реалізувати IEnumerable<Person>.
//  * Другий індексатор за іменем: public Person this[string name] => пошук у масиві.
//  * Захист від зовнішньої зміни масиву (копіювання у конструкторі).
// ----------------------------------------------------------------------------
