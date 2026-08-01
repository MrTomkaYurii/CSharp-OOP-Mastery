using AutoMapper;
using AutoMapperDemo.Domain;
using AutoMapperDemo.Dtos;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AutoMapperDemo.Features
{
    // Простий приклад: Basic mapping + flattening + ForMember
    public class BasicMappingFeature : IFeature
    {
        private readonly IMapper _mapper;
        public string Title => "AutoMapper in C# (Basic mapping, Flattening, ForMember)";

        public BasicMappingFeature(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task RunAsync()
        {
            var person = new Person
            {
                FirstName = "Ivan",
                LastName = "Petrenko",
                Address = new Address
                {
                    Street = "Shevchenko 1",
                    City = "Lviv",
                    Country = "Ukraine"
                }
            };

            // Тут відбувається мапінг Person -> PersonDto згідно MappingProfile
            var dto = _mapper.Map<PersonDto>(person);

            Console.WriteLine($"FullName: {dto.FullName}");
            Console.WriteLine($"Address city (flattened): {dto.AddressCity}");

            return Task.CompletedTask;
        }
    }
}