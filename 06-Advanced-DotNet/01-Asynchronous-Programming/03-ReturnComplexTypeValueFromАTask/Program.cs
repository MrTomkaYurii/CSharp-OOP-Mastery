using System;
using System.Threading.Tasks;

namespace TaskBasedAsynchronousProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine($"Main Thread Started");
            SomeMethod();
            Console.WriteLine($"Main Thread Completed");
            Console.ReadKey();
        }

        private async static void SomeMethod()
        {
            Employee emp = await GetEmployeeDetails();
            Console.WriteLine($"ID: {emp.ID}, Name : {emp.Name}, Salary : {emp.Salary}");
        }

        static async Task<Employee> GetEmployeeDetails()
        {
            Employee employee = new Employee()
            {
                ID = 101,
                Name = "James",
                Salary = 10000
            };

            return employee;
        }
    }

    public class Employee
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public double Salary { get; set; }
    }
}