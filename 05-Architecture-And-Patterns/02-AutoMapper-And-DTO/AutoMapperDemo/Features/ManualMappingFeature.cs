using AutoMapper;
using AutoMapperDemo.Domain;
using AutoMapperDemo.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AutoMapperDemo.Features
{
    // Порівняння: ручний мапінг vs AutoMapper.
    // Ця фіча демонструє "наочний" приклад складності ручного мапінгу,
    // особливо коли є вкладені об'єкти, колекції, умовні правила, ігнори, фіксовані/динамічні значення тощо.
    public class ManualMappingFeature : IFeature
    {
        private readonly IMapper _mapper;
        public string Title => "Manual mapping (no AutoMapper) — visual comparison";

        public ManualMappingFeature(IMapper mapper)
        {
            _mapper = mapper; // використовуємо його для порівняння результатів
        }

        public Task RunAsync()
        {
            Console.WriteLine("Створюємо об'єкти і порівняємо ручний мапінг з AutoMapper...");

            // 1) Простий Person -> PersonDto (flattening + concatenation)
            var person = new Person
            {
                FirstName = "Ivan",
                LastName = "Franko",
                Address = new Address { Street = "Stepana 3", City = "Odesa", Country = "Ukraine" }
            };

            // AutoMapper результат
            var amPersonDto = _mapper.Map<PersonDto>(person);

            // Ручний мапінг:
            var manualPersonDto = new PersonDto
            {
                FullName = $"{person.FirstName} {person.LastName}",
                AddressCity = person.Address != null ? person.Address.City : null
            };

            Console.WriteLine("Person -> PersonDto");
            Console.WriteLine($" AutoMapper: FullName='{amPersonDto.FullName}', AddressCity='{amPersonDto.AddressCity}'");
            Console.WriteLine($" Manual    : FullName='{manualPersonDto.FullName}', AddressCity='{manualPersonDto.AddressCity}'");
            Console.WriteLine();

            // 2) Employee -> EmployeeDto (nested Address and AfterMap-like logic)
            var employee = new Employee
            {
                Name = "Oksana",
                Salary = 150000m,
                WorkAddress = new Address { Street = "Main 5", City = "Kyiv", Country = "Ukraine" }
            };

            var amEmployeeDto = _mapper.Map<EmployeeDto>(employee);

            // Ручний варіант: потрібно вручну мапити вкладений Address і додати логіку IsHighEarner.
            var manualEmployeeDto = new EmployeeDto
            {
                Name = employee.Name,
                WorkAddress = employee.WorkAddress != null
                    ? new AddressDto
                    {
                        Street = employee.WorkAddress.Street,
                        City = employee.WorkAddress.City,
                        Country = employee.WorkAddress.Country
                    }
                    : null,
                IsHighEarner = employee.Salary > 100000m // ручна AfterMap-логіка
            };

            Console.WriteLine("Employee -> EmployeeDto");
            Console.WriteLine($" AutoMapper: Name='{amEmployeeDto.Name}', City='{amEmployeeDto.WorkAddress?.City}', IsHighEarner={amEmployeeDto.IsHighEarner}");
            Console.WriteLine($" Manual    : Name='{manualEmployeeDto.Name}', City='{manualEmployeeDto.WorkAddress?.City}', IsHighEarner={manualEmployeeDto.IsHighEarner}");
            Console.WriteLine();

            // 3) Колекції: OrderWithItems -> OrderWithItemsDto
            var order = new OrderWithItems
            {
                Id = 42,
                Items = new List<Item>
                {
                    new Item { Sku = "SKU1", Quantity = 2, Price = 10m },
                    new Item { Sku = "SKU2", Quantity = 5, Price = 3m }
                }
            };

            var amOrderDto = _mapper.Map<OrderWithItemsDto>(order);

            // Ручний мапінг колекції:
            var manualOrderDto = new OrderWithItemsDto { Id = order.Id, Items = new List<ItemDto>() };
            if (order.Items != null)
            {
                foreach (var it in order.Items)
                {
                    // Для кожного елемента треба явно створити і заповнити DTO
                    var itDto = new ItemDto
                    {
                        Sku = it.Sku,
                        Quantity = it.Quantity,
                        Price = it.Price
                    };
                    manualOrderDto.Items.Add(itDto);
                }
            }

            Console.WriteLine("OrderWithItems -> OrderWithItemsDto (collections)");
            Console.WriteLine($" AutoMapper: Id={amOrderDto.Id}, ItemsCount={amOrderDto.Items.Count}");
            Console.WriteLine($" Manual    : Id={manualOrderDto.Id}, ItemsCount={manualOrderDto.Items.Count}");
            Console.WriteLine(" Manual items detail:");
            foreach (var i in manualOrderDto.Items)
                Console.WriteLine($"  - {i.Sku} x{i.Quantity} @ {i.Price}");
            Console.WriteLine();

            // 4) Map complex -> primitive: Product -> string
            var product = new Product { Name = "Laptop", Price = 1299.99m };
            var amProductString = _mapper.Map<string>(product);

            // Ручне перетворення:
            var manualProductString = $"{product.Name} (${product.Price:F2})";

            Console.WriteLine("Product -> string");
            Console.WriteLine($" AutoMapper: '{amProductString}'");
            Console.WriteLine($" Manual    : '{manualProductString}'");
            Console.WriteLine();

            // 5) Reverse mapping: OrderDto -> Order (Amount -> Total) та Items
            var orderDto = new OrderDto
            {
                Id = 7,
                Amount = 500m,
                Items = new List<ItemDto> { new ItemDto { Sku = "X", Price = 50m, Quantity = 10 } }
            };

            var amOrderFromDto = _mapper.Map<Domain.Order>(orderDto);

            // Ручний переворот:
            var manualOrderFromDto = new Domain.Order
            {
                Id = orderDto.Id,
                Total = orderDto.Amount,
                Items = new List<Item>()
            };
            if (orderDto.Items != null)
            {
                foreach (var it in orderDto.Items)
                {
                    manualOrderFromDto.Items.Add(new Item
                    {
                        Sku = it.Sku,
                        Price = it.Price,
                        Quantity = it.Quantity
                    });
                }
            }

            Console.WriteLine("OrderDto -> Order (reverse mapping)");
            Console.WriteLine($" AutoMapper: Total={amOrderFromDto.Total}, ItemsCount={amOrderFromDto.Items.Count}");
            Console.WriteLine($" Manual    : Total={manualOrderFromDto.Total}, ItemsCount={manualOrderFromDto.Items.Count}");
            Console.WriteLine();

            // 6) Conditional mapping: пропускаємо пусті значення
            var srcConditional = new ConditionalExampleSource { Value = "" };
            var amCondDest = _mapper.Map<ConditionalExampleDest>(srcConditional);

            // Ручний варіант: потрібно явно перевіряти умову і не переписувати поле якщо пусто
            var manualCondDest = new ConditionalExampleDest { Value = "existing" }; // наприклад було вже значення
            if (!string.IsNullOrEmpty(srcConditional.Value))
            {
                manualCondDest.Value = srcConditional.Value;
            }

            Console.WriteLine("Conditional mapping");
            Console.WriteLine($" AutoMapper: Dest.Value='{amCondDest.Value ?? "null"}' (condition: skip empty)");
            Console.WriteLine($" Manual    : Dest.Value='{manualCondDest.Value}' (we kept existing because source empty)");
            Console.WriteLine();

            // 7) Ignore mapping: потрібно свідомо не копіювати поле
            var dtoWithIgnore = new DtoWithIgnore { Name = "T", IgnoredProp = "SHOULD NOT BE COPIED" };
            var amDomainWithIgnore = _mapper.Map<DomainWithIgnore>(dtoWithIgnore);

            // Ручний: якщо ми хочемо ігнорувати, ми просто не присвоюємо значення
            var manualDomainWithIgnore = new DomainWithIgnore { Name = dtoWithIgnore.Name, IgnoredProp = null };

            Console.WriteLine("Ignore mapping");
            Console.WriteLine($" AutoMapper: Name='{amDomainWithIgnore.Name}', IgnoredProp='{amDomainWithIgnore.IgnoredProp ?? "null"}'");
            Console.WriteLine($" Manual    : Name='{manualDomainWithIgnore.Name}', IgnoredProp='{manualDomainWithIgnore.IgnoredProp ?? "null"}'");
            Console.WriteLine();

            // 8) Fixed & dynamic destination values
            var setSrc = new FixedDynamicSrc { Id = 1, Name = "Alpha" };
            var amSetDest = _mapper.Map<FixedDynamicDest>(setSrc);

            var manualSetDest = new FixedDynamicDest
            {
                FixedValue = "STATIC", // потрібно пам'ятати самостійно задати
                TimeGenerated = DateTime.UtcNow
            };

            Console.WriteLine("Fixed & dynamic values");
            Console.WriteLine($" AutoMapper: FixedValue='{amSetDest.FixedValue}', TimeGenerated='{amSetDest.TimeGenerated:O}'");
            Console.WriteLine($" Manual    : FixedValue='{manualSetDest.FixedValue}', TimeGenerated='{manualSetDest.TimeGenerated:O}'");
            Console.WriteLine();

            // Висновок для користувача: покажемо коротку статистику (кількість рядків)
            Console.WriteLine("ВИСНОВОК:");
            Console.WriteLine(" - Ручний мапінг вимагає багато повторного коду (перевірки на null, створення нових об'єктів, цикли для колекцій, умовні присвоєння).");
            Console.WriteLine(" - AutoMapper дозволяє описати правила один раз (Profile) і потім використовувати прості виклики _mapper.Map<TDest>(src).");
            Console.WriteLine(" - Для простих сценаріїв AutoMapper зменшує boilerplate; для дуже специфічної або високопродуктивної логіки ручний мапінг дає кращий контроль.");
            Console.WriteLine(" - При додаванні нових полів або типів вручну потрібно модифікувати місця мапінгу у багатьох точках, тоді як AutoMapper централізує правила.");

            return Task.CompletedTask;
        }

        // Допоміжні типи для демонстрації умовного мапінгу та встановлення значень
        private class ConditionalExampleSource { public string Value { get; set; } = ""; }
        private class ConditionalExampleDest { public string? Value { get; set; } }

        private class FixedDynamicSrc { public int Id { get; set; } public string Name { get; set; } = ""; }
        private class FixedDynamicDest { public string FixedValue { get; set; } = ""; public DateTime TimeGenerated { get; set; } }
    }
}