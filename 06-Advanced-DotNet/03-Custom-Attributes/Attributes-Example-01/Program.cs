using System;
using System.Linq;

// ═══════════════════════════════════════════════════════════════════════════
// ЩО ТАКЕ АТРИБУТ?
// ═══════════════════════════════════════════════════════════════════════════
// Атрибут - це як "наліпка" або "ярлик", який ми клеїмо на клас, метод чи інший елемент коду.
// Ця "наліпка" містить додаткову інформацію, яку можна прочитати пізніше через Reflection.
// 
// Приклад з реального життя:
// - Книга має ярлик "Автор: Тарас Шевченко"
// - Клас може мати атрибут [Author("John Doe")]
// ═══════════════════════════════════════════════════════════════════════════

// КРОК 1: СТВОРЮЄМО ВЛАСНИЙ АТРИБУТ
// ════════════════════════════════════════════════════════════════════════════

// AttributeUsage - визначає ДЕ можна використовувати цей атрибут
// AttributeTargets.Class - означає "тільки на класах"
// Можна було б вказати: Method (методи), Property (властивості), All (скрізь) тощо
[AttributeUsage(AttributeTargets.Class)]
class AuthorAttribute : Attribute  // ОБОВ'ЯЗКОВО успадковуємо від Attribute
{
    // Властивість для зберігання імені автора
    public string Name { get; set; }

    // Дата створення класу
    public string CreationDate { get; set; }

    // Версія класу
    public string Version { get; set; }

    // Конструктор - викликається коли ми пишемо [Author("John Doe")]
    // Параметр name - це ім'я автора, яке ми передаємо в атрибут
    public AuthorAttribute(string name)
    {
        Name = name;
        // Встановлюємо значення за замовчуванням
        CreationDate = "Невідомо";
        Version = "1.0";
    }
}

// КРОК 2: СТВОРЮЄМО ДРУГИЙ АТРИБУТ ДЛЯ ДЕМОНСТРАЦІЇ
// ════════════════════════════════════════════════════════════════════════════

// Цей атрибут буде описувати призначення класу
[AttributeUsage(AttributeTargets.Class)]
class DescriptionAttribute : Attribute
{
    public string Text { get; set; }

    public DescriptionAttribute(string text)
    {
        Text = text;
    }
}

// КРОК 3: ВИКОРИСТОВУЄМО АТРИБУТИ НА КЛАСАХ
// ════════════════════════════════════════════════════════════════════════════

// Можна застосувати КІЛЬКА атрібутів до одного класу
// Синтаксис: [Назва атрибута(параметри)]

[Author("Іван Петренко")]  // "Наліпка" з інформацією про автора
[Description("Клас для зберігання інформації про людину")]  // "Наліпка" з описом
class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int Age { get; set; }

    public Person(string firstName, string lastName, int age)
    {
        FirstName = firstName;
        LastName = lastName;
        Age = age;
    }

    public void PrintInfo()
    {
        Console.WriteLine($"Ім'я: {FirstName} {LastName}, Вік: {Age}");
    }
}

// Ще один клас з атрибутами, але з додатковими параметрами
[Author("Марія Коваленко", CreationDate = "2025-01-15", Version = "2.5")]
[Description("Клас для роботи з банківськими рахунками")]
class BankAccount
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }

    public BankAccount(string accountNumber, decimal balance)
    {
        AccountNumber = accountNumber;
        Balance = balance;
    }
}

// Клас БЕЗ атрибутів - для порівняння
class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}

// КРОК 4: ГОЛОВНА ПРОГРАМА - ЧИТАЄМО АТРИБУТИ
// ════════════════════════════════════════════════════════════════════════════

class Program
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║       ДЕМОНСТРАЦІЯ РОБОТИ З CUSTOM ATTRIBUTES             ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝\n");

        // ════════════════════════════════════════════════════════════════════
        // ПРИКЛАД 1: Читання атрибута з класу Person
        // ════════════════════════════════════════════════════════════════════

        Console.WriteLine("🔍 ПРИКЛАД 1: Читання інформації про клас Person");
        Console.WriteLine(new string('─', 60));

        // typeof(Person) - отримуємо ТИП класу Person (не створюємо об'єкт!)
        // Це як дивитись на "креслення" класу, а не на конкретну людину
        Type personType = typeof(Person);
        Console.WriteLine($"Назва класу: {personType.Name}");

        // GetCustomAttributes - КЛЮЧОВИЙ МЕТОД для читання атрибутів
        // Повертає масив ВСІХ атрибутів, які "навішані" на клас Person
        Attribute[] allAttributes = Attribute.GetCustomAttributes(personType);
        Console.WriteLine($"Кількість атрибутів на класі: {allAttributes.Length}");

        // Перебираємо ВСІ знайдені атрибути
        foreach (var attr in allAttributes)
        {
            Console.WriteLine($"  • Знайдено атрибут: {attr.GetType().Name}");
        }

        Console.WriteLine();

        // ════════════════════════════════════════════════════════════════════
        // СПОСІБ 1: Пошук конкретного атрибута через OfType<T>() та Single()
        // ════════════════════════════════════════════════════════════════════

        // OfType<AuthorAttribute>() - фільтрує масив, залишає ТІЛЬКИ AuthorAttribute
        // Single() - бере ОДИН елемент (якщо більше або менше - буде помилка)
        var authorAttribute = allAttributes.OfType<AuthorAttribute>().Single();

        Console.WriteLine("📝 Інформація про автора класу Person:");
        Console.WriteLine($"   Автор: {authorAttribute.Name}");
        Console.WriteLine($"   Дата створення: {authorAttribute.CreationDate}");
        Console.WriteLine($"   Версія: {authorAttribute.Version}");

        // Шукаємо атрибут Description
        var descriptionAttribute = allAttributes.OfType<DescriptionAttribute>().Single();
        Console.WriteLine($"   Опис: {descriptionAttribute.Text}");

        Console.WriteLine("\n" + new string('═', 60) + "\n");

        // ════════════════════════════════════════════════════════════════════
        // ПРИКЛАД 2: Читання атрибутів з класу BankAccount
        // ════════════════════════════════════════════════════════════════════

        Console.WriteLine("🔍 ПРИКЛАД 2: Читання інформації про клас BankAccount");
        Console.WriteLine(new string('─', 60));

        Type bankAccountType = typeof(BankAccount);

        // СПОСІБ 2: Отримання конкретного типу атрибута одразу
        // GetCustomAttribute<T>() - повертає ОДИН атрибут певного типу (або null)
        var bankAuthor = (AuthorAttribute)Attribute.GetCustomAttribute(
            bankAccountType,
            typeof(AuthorAttribute)
        );

        // Перевірка чи знайдено атрибут (може бути null якщо атрибута немає)
        if (bankAuthor != null)
        {
            Console.WriteLine("📝 Інформація про автора класу BankAccount:");
            Console.WriteLine($"   Автор: {bankAuthor.Name}");
            Console.WriteLine($"   Дата створення: {bankAuthor.CreationDate}");
            Console.WriteLine($"   Версія: {bankAuthor.Version}");
        }

        var bankDescription = (DescriptionAttribute)Attribute.GetCustomAttribute(
            bankAccountType,
            typeof(DescriptionAttribute)
        );

        if (bankDescription != null)
        {
            Console.WriteLine($"   Опис: {bankDescription.Text}");
        }

        Console.WriteLine("\n" + new string('═', 60) + "\n");

        // ════════════════════════════════════════════════════════════════════
        // ПРИКЛАД 3: Клас БЕЗ атрибутів
        // ════════════════════════════════════════════════════════════════════

        Console.WriteLine("🔍 ПРИКЛАД 3: Клас Product (БЕЗ атрибутів)");
        Console.WriteLine(new string('─', 60));

        Type productType = typeof(Product);
        var productAttributes = Attribute.GetCustomAttributes(productType);

        Console.WriteLine($"Кількість атрібутів на класі Product: {productAttributes.Length}");

        // БЕЗПЕЧНА перевірка наявності атрибута
        var productAuthor = productAttributes.OfType<AuthorAttribute>().FirstOrDefault();

        if (productAuthor == null)
        {
            Console.WriteLine("❌ Атрибут Author НЕ знайдено на класі Product");
            Console.WriteLine("   (це нормально - ми його не додавали)");
        }

        Console.WriteLine("\n" + new string('═', 60) + "\n");

        // ════════════════════════════════════════════════════════════════════
        // ПРИКЛАД 4: Створення об'єктів класів (для порівняння)
        // ════════════════════════════════════════════════════════════════════

        Console.WriteLine("🎯 ВАЖЛИВО: Атрибути НЕ впливають на роботу класу!");
        Console.WriteLine(new string('─', 60));

        // Створюємо РЕАЛЬНІ об'єкти класів
        Person person = new Person("Олександр", "Шевченко", 30);
        BankAccount account = new BankAccount("UA123456789", 5000.50m);
        Product product = new Product { Name = "Ноутбук", Price = 25000m };

        Console.WriteLine("\n✓ Об'єкт Person створено:");
        person.PrintInfo();

        Console.WriteLine("\n✓ Об'єкт BankAccount створено:");
        Console.WriteLine($"Рахунок: {account.AccountNumber}, Баланс: {account.Balance} грн");

        Console.WriteLine("\n✓ Об'єкт Product створено:");
        Console.WriteLine($"Товар: {product.Name}, Ціна: {product.Price} грн");

        Console.WriteLine("\n💡 Як бачите, атрібути [Author] та [Description] НЕ впливають");
        Console.WriteLine("   на створення об'єктів та роботу методів!");
        Console.WriteLine("   Вони просто зберігають МЕТАДАНІ про клас.");

        Console.WriteLine("\n" + new string('═', 60) + "\n");

        // ════════════════════════════════════════════════════════════════════
        // ПРИКЛАД 5: Універсальний метод для аналізу будь-якого класу
        // ════════════════════════════════════════════════════════════════════

        Console.WriteLine("🔧 ПРИКЛАД 5: Універсальний аналізатор класів");
        Console.WriteLine(new string('─', 60));

        AnalyzeClass(typeof(Person));
        Console.WriteLine();
        AnalyzeClass(typeof(BankAccount));
        Console.WriteLine();
        AnalyzeClass(typeof(Product));

        Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                    ПІДСУМОК                                ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
        Console.WriteLine("\n✓ Атрібути - це МЕТАДАНІ (додаткова інформація) про код");
        Console.WriteLine("✓ typeof(ClassName) - отримуємо тип класу");
        Console.WriteLine("✓ GetCustomAttributes() - читаємо атрібути через Reflection");
        Console.WriteLine("✓ Атрібути НЕ впливають на виконання коду");
        Console.WriteLine("✓ Їх можна використати для документації, валідації, конфігурації\n");
    }

    // ════════════════════════════════════════════════════════════════════════
    // ДОПОМІЖНИЙ МЕТОД: Аналізує будь-який клас та виводить інформацію
    // ════════════════════════════════════════════════════════════════════════
    static void AnalyzeClass(Type classType)
    {
        Console.WriteLine($"📊 Аналіз класу: {classType.Name}");

        // Отримуємо всі атрібути
        var attributes = Attribute.GetCustomAttributes(classType);

        if (attributes.Length == 0)
        {
            Console.WriteLine("   ⚠️ Атрібутів не знайдено");
            return;
        }

        Console.WriteLine($"   Знайдено атрібутів: {attributes.Length}");

        // Перевіряємо кожен тип атрібута окремо
        var author = attributes.OfType<AuthorAttribute>().FirstOrDefault();
        if (author != null)
        {
            Console.WriteLine($"   👤 Автор: {author.Name}");
            Console.WriteLine($"      Дата: {author.CreationDate}, Версія: {author.Version}");
        }

        var description = attributes.OfType<DescriptionAttribute>().FirstOrDefault();
        if (description != null)
        {
            Console.WriteLine($"   📝 Опис: {description.Text}");
        }
    }
}