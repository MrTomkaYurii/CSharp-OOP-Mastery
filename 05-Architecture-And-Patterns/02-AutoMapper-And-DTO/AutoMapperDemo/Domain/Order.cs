using System.Collections.Generic;

namespace AutoMapperDemo.Domain
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Total { get; set; }

        public List<Item> Items { get; set; } = new();
    }
}