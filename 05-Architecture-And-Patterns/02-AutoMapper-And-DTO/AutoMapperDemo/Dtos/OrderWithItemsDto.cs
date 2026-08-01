using System.Collections.Generic;

namespace AutoMapperDemo.Dtos
{
    public class OrderWithItemsDto
    {
        public int Id { get; set; }
        public List<ItemDto> Items { get; set; } = new();
    }
}