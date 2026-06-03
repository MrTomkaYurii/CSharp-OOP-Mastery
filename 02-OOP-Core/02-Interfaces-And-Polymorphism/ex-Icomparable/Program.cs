// Приклад демонструє інтерфейс IComparable.
// IComparable дозволяє задати "природний" (стандартний) порядок сортування
// для об'єктів власного класу, реалізувавши метод CompareTo.
// Завдяки цьому такі об'єкти можна сортувати стандартними методами
// колекцій та масивів (наприклад, Array.Sort), без написання окремого компаратора.
using System;
using System.Collections.Generic;

class GFG
{

    // Driver code
    static public void Main()
    {

        // Create an array of employees
        // using collection initializer
        Employee[] employees = new Employee[]
        {
            new Employee(1, "Susmita"),
            new Employee(5, "Soniya"),
            new Employee(3, "Rohit"),
            new Employee(2, "Mohit"),
            new Employee(4, "Pihu")
        };

        // Displaying the employee's array before sorting
        Console.WriteLine("Before sorting employees array");
        foreach (var emp in employees)
        {
            Console.WriteLine("ID - {0}, Employee Name - {1}",
                                emp.ID, emp.EmployeeName);
        }

        // Array.Sort сам викликає CompareTo() у кожного елемента,
        // тому масив сортується за зростанням ID співробітника
        Array.Sort(employees);

        Console.WriteLine();

        // Printing the employee's array after sorting
        Console.WriteLine("After sorting employees array");
        foreach (var emp in employees)
        {
            Console.WriteLine("ID - {0}, Employee Name - {1}",
                                emp.ID, emp.EmployeeName);
        }
    }
}

// Клас Employee реалізує інтерфейс IComparable,
// щоб визначити власне правило порівняння співробітників
public class Employee : IComparable
{

    public int ID;
    public string EmployeeName;

    // Конструктор класу Employee
    public Employee(int id, string employeename)
    {
        this.ID = id;
        this.EmployeeName = employeename;
    }

    // Реалізація методу CompareTo() з інтерфейсу IComparable.
    // Метод приймає об'єкт для порівняння з поточним і повертає:
    // від'ємне число - якщо поточний об'єкт "менший",
    // нуль - якщо об'єкти рівні за критерієм порівняння,
    // додатне число - якщо поточний об'єкт "більший".
    public int CompareTo(object incomingobject)
    {

        // Приводимо вхідний об'єкт до типу Employee,
        // щоб мати доступ до його поля ID
        Employee incomingemployee = incomingobject as Employee;

        // Порівнюємо співробітників за їхнім ID
        return this.ID.CompareTo(incomingemployee.ID);
    }
}
