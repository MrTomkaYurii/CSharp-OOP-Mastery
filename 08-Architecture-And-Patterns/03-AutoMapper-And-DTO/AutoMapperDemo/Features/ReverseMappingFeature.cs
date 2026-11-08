using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperDemo.Domain;
using AutoMapperDemo.Dtos;

namespace AutoMapperDemo.Features
{
    // Reverse mapping приклад
    public class ReverseMappingFeature : IFeature
    {
        private readonly IMapper _mapper;
        public string Title => "AutoMapper Reverse Mapping in C#";

        public ReverseMappingFeature(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task RunAsync()
        {
            var orderDto = new OrderDto
            {
                Id = 10,
                Amount = 2500m,
                Items = { new ItemDto { Sku = "ABC", Price = 100m, Quantity = 2 } }
            };

            // Ми налаштували OrderDto -> Order з ReverseMap у профілі
            var order = _mapper.Map<Domain.Order>(orderDto);
            Console.WriteLine($"Order total mapped from Amount: {order.Total}");
            Console.WriteLine($"Items count: {order.Items.Count}");

            // ReverseMap дозволяє також мапити назад Order -> OrderDto
            var back = _mapper.Map<OrderDto>(order);
            Console.WriteLine($"Back to DTO amount: {back.Amount}");

            return Task.CompletedTask;
        }
    }
}