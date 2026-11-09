// =========================================================================================
// ВЛАСНІ АТРИБУТИ: Інспекція методів через Reflection API (MethodInfo та GetCustomAttributes)
// =========================================================================================
// Демонструє створення атрибута для документування методів і подальший автоматичний
// обхід метаданих членів класу за допомогою Type.GetMethods() та MemberInfo.GetCustomAttributes.
//
// ТЕОРЕТИЧНІ АСПЕКТИ:
// 1. Атрибути як інструмент внутрішньої аналітики та аудиту:
//    Замість жорсткого хардкоду списку методів, програма може динамічно в рантаймі
//    знаходити методи з певною міткою та виконувати з ними дії (наприклад, Swagger/OpenAPI генерація).
// 2. Метод classType.GetMethods() за замовчуванням повертає всі публічні екземплярні та статичні методи.
// 3. Параметр inherit у GetCustomAttributes(true):
//    Вказує, чи потрібно шукати атрибути у перевизначених віртуальних методах базових класів.
// =========================================================================================

using System;
using System.Reflection;
using System.Collections.Generic;

// Створення власного класу на основі класу Attribute
class CustomAttribute : Attribute
{
    // Оголошення приватних змінних
    private string name;      // Ім'я
    private string company;   // Компанія

    // Параметризований конструктор класу CustomAttribute
    public CustomAttribute(string name, string company)
    {
        this.name = name;
        this.company = company;
    }

    // Метод для відображення полів за допомогою класу Reflection
    public static void AttributeDisplay(Type classType)
    {
        Console.WriteLine("Всі методи класу {0}:", classType.Name);
        // Масив методів класу для зберігання значень атрибутів
        MethodInfo[] methods = classType.GetMethods();
        // Перебір значень атрибутів методів за допомогою циклу
        for (int i = 0; i < methods.GetLength(0); i++)
        {
            // Створення масиву для отримання всіх значень власних атрибутів
            object[] attributesArray = methods[i].GetCustomAttributes(true);
            // Цикл для читання значень через всі атрибути методів
            foreach (Attribute item in attributesArray)
            {
                if (item is CustomAttribute)
                {
                    // Виведення значень власних атрибутів
                    CustomAttribute attributeObject = (CustomAttribute)item;
                    Console.WriteLine("{0} - {1}, {2} ", methods[i].Name,
                        attributeObject.name, attributeObject.company);
                }
            }
        }
    }
}

// Клас Employer для створення полів працедавця
class Employer
{
    // Оголошення полів працедавця
    int employeeID;  // Ідентифікатор працівника
    string name;     // Ім'я

    // Параметризований конструктор класу Employer
    public Employer(int eID, string name)
    {
        this.employeeID = eID;
        this.name = name;
    }

    // Застосування власного атрибуту для методу getEmployeeID
    [CustomAttribute("Звернення до значень", "Генерує ідентифікатор працівника")]
    public int getEmployeeID()
    {
        return employeeID;
    }

    // Застосування власного атрибуту для методу getName
    [CustomAttribute("Звернення до значень", "Генерує ім'я працівника")]
    public string getName()
    {
        return name;
    }
}

// Створення класу Employee
class Employee
{
    // Оголошення змінних працівника
    int employeeID;  // Ідентифікатор працівника
    string name;     // Ім'я

    // Параметризований конструктор класу Employee
    public Employee(int eID, string name)
    {
        this.employeeID = eID;
        this.name = name;
    }

    // Застосування власного атрибуту CustomAttribute для методу getEmployeeID
    [CustomAttribute("Звернення до значень", "Генерує ідентифікатор працівника")]
    public int getEmployeeID()
    {
        return employeeID;
    }

    // Застосування власного атрибуту CustomAttribute для методу getName
    [CustomAttribute("Звернення до значень", "Генерує ім'я працівника")]
    public string getName()
    {
        return name;
    }
}

// Створення класу для відображення виводу
public class Program
{
    // Головний метод програми
    public static void Main(string[] args)
    {
        // Виклик статичного методу для відображення класу Employer
        CustomAttribute.AttributeDisplay(typeof(Employer));
        Console.WriteLine();
        // Виклик статичного методу для відображення класу Employee
        CustomAttribute.AttributeDisplay(typeof(Employee));
    }
}
