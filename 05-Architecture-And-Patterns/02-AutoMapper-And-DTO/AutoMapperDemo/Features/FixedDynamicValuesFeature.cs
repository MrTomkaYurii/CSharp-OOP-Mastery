using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperDemo.Domain;

namespace AutoMapperDemo.Features
{
    // Приклад з фіксованими і динамічними значеннями в Destination
    public class FixedDynamicValuesFeature : IFeature
    {
        private readonly IMapper _mapper;
        public string Title => "Fixed and Dynamic Values in Destination Property in AutoMapper";

        public FixedDynamicValuesFeature(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task RunAsync()
        {
            var src = new SetValuesSource { Id = 1, Name = "Alpha" };
            var dest = _mapper.Map<SetValuesDest>(src);

            Console.WriteLine($"FixedValue (should be STATIC): {dest.FixedValue}");
            Console.WriteLine($"TimeGenerated (UTC now): {dest.TimeGenerated:O}");

            return Task.CompletedTask;
        }
    }

    public class SetValuesSource
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
    }

    public class SetValuesDest
    {
        public string FixedValue { get; set; } = "";
        public DateTime TimeGenerated { get; set; }
    }
}