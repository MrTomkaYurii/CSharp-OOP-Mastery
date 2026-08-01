using System.Collections.Generic;

namespace AutoMapperDemo.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public List<ItemDto> Items { get; set; } = new();
    }
}