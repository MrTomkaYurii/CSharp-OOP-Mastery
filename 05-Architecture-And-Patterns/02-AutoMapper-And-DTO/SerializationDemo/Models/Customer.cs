using System.Collections.Generic;

namespace SerializationDemo.Models
{
    public class Customer
    {
        // У більшості реальних сценаріїв серіалізація працює з публічними властивостями
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;

        public Address Address { get; set; } = new Address();

        // Колекція пов’язаних сутностей (Orders) — хороший приклад вкладених структур
        public List<Order> Orders { get; set; } = new List<Order>();
    }
}