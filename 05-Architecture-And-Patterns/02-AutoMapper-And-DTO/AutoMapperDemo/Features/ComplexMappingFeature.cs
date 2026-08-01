using AutoMapper;
using AutoMapperDemo.Domain;
using AutoMapperDemo.Dtos;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AutoMapperDemo.Features
{
    // Демонструє складніші випадки: nested mapping, AfterMap, та використання DTO для Employee
    public class ComplexMappingFeature : IFeature
    {
        private readonly IMapper _mapper;
        public string Title => "AutoMapper Complex Mapping in C# (Nested types, AfterMap)";

        public ComplexMappingFeature(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task RunAsync()
        {
            var employee = new Employee
            {
                Name = "Oksana",
                Salary = 150000m,
                WorkAddress = new Address
                {
                    Street = "Main 5",
                    City = "Kyiv",
                    Country = "Ukraine"
                }
            };

            // Map Employee -> EmployeeDto (Address -> AddressDto is mapped automatically because we created map)
            var dto = _mapper.Map<EmployeeDto>(employee);

            Console.WriteLine($"Name: {dto.Name}");
            Console.WriteLine($"Work City: {dto.WorkAddress?.City}");
            Console.WriteLine($"Is high earner? {dto.IsHighEarner}");

            return Task.CompletedTask;
        }
    }
}