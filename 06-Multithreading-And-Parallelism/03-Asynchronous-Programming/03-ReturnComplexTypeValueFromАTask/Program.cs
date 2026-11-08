using System;
using System.Threading.Tasks;

namespace TaskBasedAsynchronousProgramming
{
    /// <summary>
    /// =========================================================================================
    /// ПОВЕРНЕННЯ СКЛАДНИХ ТИПІВ (Complex Types in Task<T>): Робота з об'єктами сутностей
    /// =========================================================================================
    /// Демонструє, як асинхронні методи інкапсулюють передачу користувацьких типів (DTO, Entity)
    /// через Task<TEntity>. Після 'await' результат є повністю типізованим посиланням на об'єкт.
    /// </summary>
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Головний потік Main розпочав роботу.");

            await SomeMethodAsync();

            Console.WriteLine("Головний потік Main завершив роботу.");
        }

        private static async Task SomeMethodAsync()
        {
            // Отримання складного об'єкта через await без блокування потоку
            Employee emp = await GetEmployeeDetailsAsync();

            Console.WriteLine($"Отримано дані співробітника: ID: {emp.ID}, Ім'я: {emp.Name}, Зарплата: ${emp.Salary}");
        }

        private static async Task<Employee> GetEmployeeDetailsAsync()
        {
            // Імітація асинхронної паузи (наприклад, читання з бази даних або дискового сховища)
            await Task.Delay(100);

            return new Employee
            {
                ID = 101,
                Name = "Олександр Коваленко",
                Salary = 45000.00
            };
        }
    }

    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Salary { get; set; }
    }
}
