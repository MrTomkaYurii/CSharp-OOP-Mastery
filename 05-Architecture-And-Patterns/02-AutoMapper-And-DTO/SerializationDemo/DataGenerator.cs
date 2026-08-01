using SerializationDemo.Interfaces;
using SerializationDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SerializationDemo
{
    public class DataGenerator : IDataGenerator
    {
        private readonly Random _random = new Random();

        private static readonly string[] FirstNames =
        {
            "Іван", "Петро", "Марія", "Олена", "Андрій", "Юрій", "Наталія", "Світлана"
        };

        private static readonly string[] LastNames =
        {
            "Шевченко", "Коваленко", "Бондар", "Мельник", "Сидоренко", "Ткачук"
        };

        private static readonly string[] Cities =
        {
            "Київ", "Львів", "Одеса", "Харків", "Дніпро"
        };

        private static readonly string[] Streets =
        {
            "Шевченка", "Грушевського", "Бандери", "Свободи", "Сагайдачного"
        };

        private static readonly string[] ProductNames =
        {
            "Ноутбук", "Смартфон", "Монітор", "Клавіатура", "Миша", "Навушники"
        };

        public Task<List<Customer>> GenerateCustomersAsync(int count)
        {
            var customers = new List<Customer>();
            for (int i = 1; i <= count; i++)
            {
                var customer = new Customer
                {
                    Id = i,
                    Name = $"{RandomItem(FirstNames)} {RandomItem(LastNames)}",
                    Email = $"user{i}@example.com",
                    Address = GenerateAddress()
                };

                // Кількість замовлень для кожного клієнта
                int ordersCount = _random.Next(1, 5);
                for (int j = 0; j < ordersCount; j++)
                {
                    var order = GenerateOrder(customer.Id);
                    customer.Orders.Add(order);
                }

                customers.Add(customer);
            }

            return Task.FromResult(customers);
        }

        private Address GenerateAddress()
        {
            return new Address
            {
                City = RandomItem(Cities),
                Street = RandomItem(Streets),
                Building = _random.Next(1, 100).ToString(),
                Apartment = _random.Next(0, 2) == 0 ? null : _random.Next(1, 200).ToString(),
                PostalCode = $"{_random.Next(01000, 09999)}"
            };
        }

        private Order GenerateOrder(int customerId)
        {
            var order = new Order
            {
                Id = _random.Next(1000, 9999),
                CustomerId = customerId,
                OrderDate = DateTime.UtcNow.AddDays(-_random.Next(0, 365)),
                Status = RandomEnum<OrderStatus>(),
            };

            int itemsCount = _random.Next(1, 6);
            for (int i = 0; i < itemsCount; i++)
            {
                var item = GenerateOrderItem();
                order.Items.Add(item);
            }

            order.TotalAmount = order.Items.Sum(i => i.LineTotal);

            return order;
        }

        private OrderItem GenerateOrderItem()
        {
            var name = RandomItem(ProductNames);
            var quantity = _random.Next(1, 5);
            var unitPrice = (decimal)(_random.Next(500, 5000) / 10.0);

            return new OrderItem
            {
                ProductId = _random.Next(1, 100),
                ProductName = name,
                Quantity = quantity,
                UnitPrice = unitPrice
            };
        }

        private T RandomItem<T>(IReadOnlyList<T> list) => list[_random.Next(list.Count)];

        private T RandomEnum<T>() where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(_random.Next(values.Length))!;
        }
    }
}