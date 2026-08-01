using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperDemo.Domain;
using AutoMapperDemo.Dtos;

namespace AutoMapperDemo.Features
{
    // Приклади з колекціями і циклами.
    // Детально коментовано, що відбувається під час мапінгу колекцій.
    public class CollectionsFeature : IFeature
    {
        private readonly IMapper _mapper;
        public string Title => "AutoMapper with Collections, Loops and Mapping nested collections";

        public CollectionsFeature(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task RunAsync()
        {
            var order = new OrderWithItems
            {
                Id = 99,
                Items = new List<Item>
                {
                    new Item { Sku = "A1", Price = 10m, Quantity = 2 },
                    new Item { Sku = "B2", Price = 5m, Quantity = 5 }
                }
            };

            // Ось ключова частина: AutoMapper мапить OrderWithItems -> OrderWithItemsDto.
            // Порядок дій AutoMapper:
            // 1) Знаходить CreateMap<OrderWithItems, OrderWithItemsDto>().
            // 2) Для кожного властивості, яку потрібно мапити, застосовує відповідні правила.
            // 3) Для Items (List<Item> -> List<ItemDto>) він перевіряє, чи є мапінг Item -> ItemDto.
            //    - Якщо є, AutoMapper створює новий List<ItemDto> (тип призначення залежить від конкретного типу у DTO)
            //    - Для кожного Item виконується мапінг у ItemDto і додається в колекцію.
            // 4) Якщо джерело Items == null, то стандартна поведінка: у destination буде null або порожня колекція
            //    залежно від налаштувань / конструкторів в DTO.
            //
            // Зауваження:
            // - AutoMapper робить "глибоку" операцію мапінгу для елементів (mapping each element),
            //   але не робить deep clone для властивостей, які є reference-типами, якщо для них не заданий мапінг.
            // - Якщо потрібно зберегти посилання (щоб уникнути дублювання об'єктів при циклах) —
            //   можна використати PreserveReferences() в профілі.
            // - Типи колекцій: AutoMapper коректно мапить між List<>, ICollection<>, IEnumerable<>, масивами тощо.
            //   Якщо у destination встановлено конкретний тип (наприклад ObservableCollection), AutoMapper намагатиметься створити його,
            //   але в деяких випадках може знадобитися кастомна конфігурація.
            var dto = _mapper.Map<OrderWithItemsDto>(order);

            Console.WriteLine($"Order Id: {dto.Id}");
            foreach (var it in dto.Items)
                Console.WriteLine($"Item {it.Sku} x{it.Quantity} @ {it.Price}");

            // Додатковий приклад: мапінг значення для підрахунків або агрегацій.
            // Можна додати, наприклад, поле TotalQuantity у DTO і заповнити його через ForMember/MapFrom:
            // .ForMember(d => d.TotalQuantity, opt => opt.MapFrom(src => src.Items.Sum(i => i.Quantity)))
            //
            // Також можна показати мапінг колекції в колекцію різних типів:
            var simpleItems = new List<Item>
            {
                new Item { Sku = "C3", Price = 1m, Quantity = 10 }
            };
            // Якщо Item -> ItemDto існує, то List<Item> -> List<ItemDto> також буде працювати.
            var itemDtos = _mapper.Map<List<ItemDto>>(simpleItems);
            Console.WriteLine($"Mapped {itemDtos.Count} items to ItemDto list.");

            return Task.CompletedTask;
        }
    }
}