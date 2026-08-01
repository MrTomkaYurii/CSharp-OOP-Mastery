using AutoMapper;
using AutoMapperDemo.Domain;
using AutoMapperDemo.Dtos;
using AutoMapperDemo.Features;
using AutoMapperDemo.Features.Advanced;
using System;
using System.Net;
using System.Xml.Linq;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AutoMapperDemo.Mapping
{
    // Профіль AutoMapper — тут описані всі правила мапінгу для проєкту.
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ======= Basic / Flattening / ForMember (Simple example) =======
            // Person -> PersonDto:
            // - FullName комбінується з FirstName + LastName (ForMember, MapFrom)
            // - Address.City "згладжується" у поле AddressCity (flattening).
            CreateMap<Person, PersonDto>()
                .ForMember(dest => dest.FullName,
                           opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.AddressCity,
                           opt => opt.MapFrom(src => src.Address.City));



            // ======= Complex Mapping: Nested types =======
            // Якщо є мапінг для Address -> AddressDto, то AutoMapper автоматично
            // використає його при мапінгу Employee.WorkAddress -> EmployeeDto.WorkAddress.
            CreateMap<Address, AddressDto>();
            CreateMap<Employee, EmployeeDto>()
                // AfterMap дозволяє виконати додаткову логіку після стандартного мапінгу
                .AfterMap((src, dest) =>
                {
                    if (src.Salary > 100000)
                        dest.IsHighEarner = true;
                });



            // ======= Map complex -> primitive (Converter) =======
            // Приклад, коли ми хочемо явно перетворити Product -> string
            // (ConvertUsing застосовується для всієї мапи типу)
            CreateMap<Product, string>().ConvertUsing(src => $"{src.Name} (${src.Price:F2})");



            // ======= Reverse Mapping =======
            // OrderDto -> Order (і .ReverseMap() додає зворотнє правило Order -> OrderDto)
            // Зауважте: OrderDto має Items: List<ItemDto>, Order має Items: List<Item>.
            // AutoMapper автоматично мапить колекції, якщо є мапінг Item <-> ItemDto.
            CreateMap<OrderDto, Order>()
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.Amount))
                .ReverseMap();



            // ======= Conditional Mapping =======
            CreateMap<SourceConditional, DestinationConditional>()
                .ForMember(d => d.Value, opt =>
                {
                    // opt.Condition => значення буде встановлено лише якщо умова повертає true
                    opt.Condition(src => !string.IsNullOrEmpty(src.Value));
                    opt.MapFrom(src => src.Value);
                });



            // ======= Ignore mapping =======
            CreateMap<DtoWithIgnore, DomainWithIgnore>()
                .ForMember(dest => dest.IgnoredProp, opt => opt.Ignore());



            // ======= Fixed and dynamic values =======
            CreateMap<SetValuesSource, SetValuesDest>()
                // Фіксоване значення — завжди "STATIC"
                .ForMember(d => d.FixedValue, opt => opt.MapFrom(_ => "STATIC"))
                // Динамічне значення — час генерації (UTC)
                .ForMember(d => d.TimeGenerated, opt => opt.MapFrom(_ => DateTime.UtcNow));



            // ======= Collections: map Item -> ItemDto =======
            // Щоб AutoMapper міг мапити колекції (наприклад List<Item> -> List<ItemDto>),
            // потрібно створити мапінг між елементами: Item -> ItemDto.
            // Після цього AutoMapper під час мапінгу колекцій створить новий список
            // і викличе мапінг для кожного елементу колекції.
            CreateMap<Item, ItemDto>();



            // Приклад класу, який містить колекцію:
            CreateMap<OrderWithItems, OrderWithItemsDto>();



            // ======= Advanced: Custom resolvers, converters, polymorphic, enum mapping =======
            CreateMap<SourceCustom, DestCustom>()
                .ForMember(d => d.Computed, opt => opt.MapFrom<ComputedValueResolver>());
            // Enum to string конвертація (великі літери)
            CreateMap<OrderStatus, string>().ConvertUsing(s => s.ToString().ToUpperInvariant());



            // Для демонстрації роботи з циклічними структурами і уникання повторного мапінгу:
            // PreserveReferences() дозволяє зберігати посилання на вже замаплені об'єкти,
            // MaxDepth(5) — захищає від надмірної рекурсії.
            CreateMap<Node, NodeDto>().PreserveReferences().MaxDepth(5);
        }
    }
}