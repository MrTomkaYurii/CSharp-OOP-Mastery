using System.Net;

namespace AutoMapperDemo.Domain
{
    public class Employee
    {
        public string Name { get; set; } = "";
        public Address WorkAddress { get; set; } = new();
        public decimal Salary { get; set; }
    }
}