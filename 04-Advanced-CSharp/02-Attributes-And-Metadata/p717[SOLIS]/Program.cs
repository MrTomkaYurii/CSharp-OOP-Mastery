using System;

/// <summary>
/// =========================================================================================
/// ВЛАСНІ АТРИБУТИ: Перевірка метаданих через Type.IsDefined (Деніел Соліс, с. 717)
/// =========================================================================================
/// Демонструє приклад зі сторінки 717 книги Daniel Solis "Illustrated C#".
/// Створення класу власного атрибута ReviewCommentAttribute та його детекція
/// в рантаймі за допомогою методу IsDefined() без зайвого інстанціювання об'єкта.
/// 
/// ТЕОРЕТИЧНІ АСПЕКТИ:
/// 1. Будь-який кастомний атрибут має успадковуватись від абстрактного базового класу System.Attribute.
/// 2. [AttributeUsage] обмежує ціль використання (тут: AttributeTargets.Class).
/// 3. Оптимізація продуктивності через IsDefined:
///    - Type.IsDefined(attributeType, inherit) лише сканує таблицю метаданих CustomAttribute (0x0C).
///    - На відміну від GetCustomAttributes(), метод IsDefined НЕ створює екземпляр об'єкта в Heap!
///    - Якщо потрібно лише перевірити наявність мітки (наприклад, [Serializable] чи [Authorize]),
///      метод IsDefined працює у рази швидше та не навантажує Garbage Collector.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class ReviewCommentAttribute : System.Attribute
{
    public string Description { get; set; }
    public string VersionNumber { get; set; }
    public string ReviewerID { get; set; }

    // Конструктор атрибута приймає позиційні параметри:
    public ReviewCommentAttribute(string desc, string ver)
    {
        Description = desc;
        VersionNumber = ver;
    }
}

// Застосовуємо наш кастомний атрибут до класу:
[ReviewComment("Check it out", "2.4")]
class MyClass
{
}

class Program
{
    static void Main()
    {
        MyClass mc = new MyClass();   // Створення екземпляра класу
        Type t = mc.GetType();        // Отримання Type через рефлексію
        
        // Швидка перевірка метаданих: inherit = false (не шукати в базових класах Object):
        bool isDefined = t.IsDefined(typeof(ReviewCommentAttribute), inherit: false);

        if (isDefined)
            Console.WriteLine($"ReviewComment is applied to type {t.Name}");
    }
}
