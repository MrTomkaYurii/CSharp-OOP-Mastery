using System.Collections.Generic;

namespace AutoMapperDemo.Domain
{
    public class OrderWithItems
    {
        public int Id { get; set; }
        public List<Item> Items { get; set; } = new();
    }
}