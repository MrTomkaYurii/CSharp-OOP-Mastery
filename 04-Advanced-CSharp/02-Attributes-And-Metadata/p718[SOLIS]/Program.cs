using System;

/// <summary>
/// =========================================================================================
/// ВЛАСНІ АТРИБУТИ: Отримання об'єкта через GetCustomAttributes (Деніел Соліс, с. 718)
/// =========================================================================================
/// Демонструє приклад зі сторінки 718 книги Daniel Solis "Illustrated C#".
/// Зчитування та десеріалізація параметрів кастомного атрибута в рантаймі.
/// 
/// ТЕОРЕТИЧНІ АСПЕКТИ:
/// 1. При виклику GetCustomAttributes() середина CLR:
///    - Знаходить відповідний запис у таблиці CustomAttribute (0x0C).
///    - Зчитує бінарні аргументи з потоку #Blob збірки.
///    - Викликає конструктор класу MyAttributeAttribute і повертає справжній об'єкт у керованій купі.
/// 2. Позиційні аргументи (desc, ver) передаються в конструктор обов'язково.
/// 3. Іменовані аргументи (ReviewerID) можна встановлювати через синтаксис ініціалізації властивостей:
///    [MyAttribute("Опис", "1.0", ReviewerID = "REV_42")].
/// 4. Сучасна альтернатива:
///    У сучасному .NET для більшої типізації рекомендується викликати узагальнений метод
///    t.GetCustomAttribute<MyAttributeAttribute>() замість застарілого нетипізованого GetCustomAttributes(bool).
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class MyAttributeAttribute : System.Attribute
{
    public string Description { get; set; }
    public string VersionNumber { get; set; }
    public string ReviewerID { get; set; }

    public MyAttributeAttribute(string desc, string ver)
    {
        Description = desc;
        VersionNumber = ver;
    }
}

[MyAttribute("Check it out", "2.4")]
class MyClass
{
}

class Program
{
    static void Main()
    {
        Type t = typeof(MyClass);

        // Отримання масиву об'єктів атрибутів:
        object[] AttArr = t.GetCustomAttributes(false);

        foreach (Attribute a in AttArr)
        {
            MyAttributeAttribute attr = a as MyAttributeAttribute;
            if (null != attr)
            {
                Console.WriteLine($"Description    : {attr.Description}");
                Console.WriteLine($"Version Number : {attr.VersionNumber}");
                Console.WriteLine($"Reviewer ID    : {attr.ReviewerID}");
            }
        }
    }
}
