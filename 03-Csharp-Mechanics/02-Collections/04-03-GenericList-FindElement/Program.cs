using System;
using System.Collections.Generic;

namespace ListCollectionDemo
{
    public class Program
    {
        public static void Main()
        {
            // Створення об'єктів Employee
            Employee Employee1 = new Employee() { ID = 101, Name = "Pranaya", Gender = "Male", Salary = 5000 };
            Employee Employee2 = new Employee() { ID = 102, Name = "Priyanka", Gender = "Female", Salary = 7000 };
            Employee Employee3 = new Employee() { ID = 103, Name = "Anurag", Gender = "Male", Salary = 5500 };
            Employee Employee4 = new Employee() { ID = 104, Name = "Sambit", Gender = "Male", Salary = 6500 };

            // Створення списку типу Employee та його ініціалізація
            List<Employee> listEmployees = new List<Employee>
            {
                Employee1,
                Employee2,
                Employee3,
                Employee4
            };

            // Метод Contains — перевіряє, чи міститься конкретний об'єкт у списку
            Console.WriteLine("Перевірка Contains для об'єкта Employee2:");
            if (listEmployees.Contains(Employee2))
            {
                Console.WriteLine("Об'єкт Employee2 міститься у списку");
            }
            else
            {
                Console.WriteLine("Об'єкта Employee2 немає у списку");
            }

            // Метод Exists — перевіряє, чи існує елемент, що відповідає умові
            Console.WriteLine("\nМетод Exists: ім'я починається з літери 'P'");
            if (listEmployees.Exists(x => x.Name.StartsWith("P")))
            {
                Console.WriteLine("У списку є працівник, чиє ім'я починається з 'P'");
            }
            else
            {
                Console.WriteLine("У списку немає працівників з іменем, що починається з 'P'");
            }

            // Find() — повертає перший елемент, що відповідає умові
            Console.WriteLine("\nМетод Find: перший працівник із Gender = Male");
            Employee? emp = listEmployees.Find(employee => employee.Gender == "Male");
            Console.WriteLine($"ID = {emp?.ID}, Name = {emp?.Name}, Gender = {emp?.Gender}, Salary = {emp?.Salary}");

            // FindLast() — повертає останній елемент, що відповідає умові
            Console.WriteLine("\nМетод FindLast: останній працівник із Gender = Male");
            Employee? lastMatchEmp = listEmployees.FindLast(employee => employee.Gender == "Male");
            Console.WriteLine($"ID = {lastMatchEmp?.ID}, Name = {lastMatchEmp?.Name}, Gender = {lastMatchEmp?.Gender}, Salary = {lastMatchEmp?.Salary}");

            // FindAll() — повертає усі елементи, що відповідають умові
            Console.WriteLine("\nМетод FindAll: усі працівники, у яких Gender = Male");
            List<Employee> filteredEmployees = listEmployees.FindAll(employee => employee.Gender == "Male");
            foreach (Employee femp in filteredEmployees)
            {
                Console.WriteLine($"ID = {femp.ID}, Name = {femp.Name}, Gender = {femp.Gender}, Salary = {femp.Salary}");
            }

            // FindIndex() — повертає індекс першого елемента, що відповідає умові
            Console.WriteLine($"\nІндекс першого працівника з Gender = Male: {listEmployees.FindIndex(employee => employee.Gender == "Male")}");

            // FindLastIndex() — повертає індекс останнього елемента, що відповідає умові
            Console.WriteLine($"Індекс останнього працівника з Gender = Male: {listEmployees.FindLastIndex(employee => employee.Gender == "Male")}");

            Console.ReadKey();
        }
    }

    // Клас Employee, що представляє працівника
    public class Employee
    {
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Gender { get; set; }
        public int Salary { get; set; }
    }
}
