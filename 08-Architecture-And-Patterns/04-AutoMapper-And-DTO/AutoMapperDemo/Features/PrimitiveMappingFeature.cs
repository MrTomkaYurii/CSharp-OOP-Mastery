using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperDemo.Domain;

namespace AutoMapperDemo.Features
{
    // Демонструє мапінг складного об'єкта в примітив (ConvertUsing) та колекції
    public class PrimitiveMappingFeature : IFeature
    {
        private readonly IMapper _mapper;
        public string Title => "How to Map Complex Type to Primitive Type using AutoMapper in C#";

        public PrimitiveMappingFeature(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task RunAsync()
        {
            var product = new Product { Name = "Laptop", Price = 1299.99m };
            // В MappingProfile ми створили CreateMap<Product, string>().ConvertUsing(...)
            var productString = _mapper.Map<string>(product);

            Console.WriteLine($"Product as string: {productString}");

            // Колекція продуктів -> колекція рядків
            var products = new List<Product>
            {
                new Product { Name = "Phone", Price = 699.5m },
                new Product { Name = "Tablet", Price = 399.0m }
            };

            var strings = _mapper.Map<List<string>>(products);
            Console.WriteLine("Products mapped to strings:");
            foreach (var s in strings) Console.WriteLine($" - {s}");

            return Task.CompletedTask;
        }
    }
}