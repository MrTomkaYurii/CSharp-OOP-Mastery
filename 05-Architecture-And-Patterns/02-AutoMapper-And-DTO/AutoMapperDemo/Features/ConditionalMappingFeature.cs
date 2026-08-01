using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperDemo.Domain;

namespace AutoMapperDemo.Features
{
    // Conditional mapping (Condition + PreCondition) приклад
    public class ConditionalMappingFeature : IFeature
    {
        private readonly IMapper _mapper;
        public string Title => "AutoMapper Conditional Mapping in C#";

        public ConditionalMappingFeature(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task RunAsync()
        {
            var src = new SourceConditional { Value = "" };
            // У MappingProfile ми вказали Condition: якщо Value пустий, значення не буде встановлене.
            var dest = _mapper.Map<DestinationConditional>(src);
            Console.WriteLine($"Destination.Value (should be default/null): '{dest.Value}'");

            src.Value = "Now I have value";
            dest = _mapper.Map<DestinationConditional>(src);
            Console.WriteLine($"Destination.Value (after set): '{dest.Value}'");

            return Task.CompletedTask;
        }
    }

    // моделі для цієї фічі
    public class SourceConditional
    {
        public string Value { get; set; } = "";
    }

    public class DestinationConditional
    {
        public string? Value { get; set; }
    }
}