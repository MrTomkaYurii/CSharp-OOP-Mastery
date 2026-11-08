using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.S.SRPGoodExample
{
    // 1. Модель даних (Entity / DTO) - відповідає лише за інкапсуляцію стану замовлення
    public class Order
    {
        public string Id { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public List<string> Items { get; set; } = new();
        public decimal TotalPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal FinalPrice => TotalPrice - Discount;
        public DateTime CreatedAt { get; set; }
    }
}
