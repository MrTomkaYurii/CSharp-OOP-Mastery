using AutoMapper;
using AutoMapper.QueryableExtensions;
using AutoMapperDemo.Domain;
using AutoMapperDemo.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AutoMapperDemo.Features.Advanced
{
    // Кілька просунутих прикладів: custom resolver, enum conversion, PreserveReferences, ProjectTo (LINQ projection)
    public class AdvancedFeature : IFeature
    {
        private readonly IMapper _mapper;
        public string Title => "Advanced AutoMapper: Custom Resolvers, Converters, ProjectTo and PreserveReferences";

        public AdvancedFeature(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task RunAsync()
        {
            // CUSTOM RESOLVER приклад (MappingProfile вказує ComputedValueResolver)
            var s = new SourceCustom { A = 5, B = 7 };
            var d = _mapper.Map<DestCustom>(s);
            Console.WriteLine($"Computed (resolver) = {d.Computed}");

            // ENUM конвертація
            var status = OrderStatus.Processing;
            var asString = _mapper.Map<string>(status); // ConvertUsing в профілі
            Console.WriteLine($"OrderStatus as string: {asString}");

            // PreserveReferences (циклічна структура)
            var root = new Node { Name = "root" };
            var child = new Node { Name = "child", Parent = root };
            root.Children.Add(child);

            var rootDto = _mapper.Map<NodeDto>(root);
            Console.WriteLine($"RootDto name: {rootDto.Name}, Child parent is set? {(rootDto.Children.FirstOrDefault()?.Parent != null)}");

            // ProjectTo приклад (псевдо-LINQ, тут демонстрація: потрібно IQueryable)
            var orders = new List<Order>
            {
                new Order { Id = 1, Total = 100, Items = new List<Item>{ new Item{ Sku="X", Price=50, Quantity=2 } } },
                new Order { Id = 2, Total = 200, Items = new List<Item>{ new Item{ Sku="Y", Price=100, Quantity=2 } } }
            }.AsQueryable();

            // ProjectTo будує SELECT, використовуючи налаштування AutoMapper (в реальному EF це буде SQL)
            var projected = orders.ProjectTo<OrderDto>(_mapper.ConfigurationProvider).ToList();
            Console.WriteLine("Projected orders via ProjectTo:");
            foreach (var p in projected)
                Console.WriteLine($" - Order {p.Id} Amount: {p.Amount}, Items: {p.Items.Count}");

            return Task.CompletedTask;
        }
    }

    // Допоміжні типи для advanced
    public class SourceCustom { public int A { get; set; } public int B { get; set; } }
    public class DestCustom { public int Computed { get; set; } }
    public enum OrderStatus { New = 0, Processing = 1, Completed = 2 }

    public class NodeDto
    {
        public string Name { get; set; } = "";
        public NodeDto? Parent { get; set; }
        public List<NodeDto> Children { get; set; } = new();
    }


}