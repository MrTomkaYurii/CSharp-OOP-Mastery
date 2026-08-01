using System;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapperDemo.Domain;

namespace AutoMapperDemo.Features
{
    // Демонстрація Ignore в мапінгу
    public class IgnoreMappingFeature : IFeature
    {
        private readonly IMapper _mapper;
        public string Title => "AutoMapper Ignore Method in C#";

        public IgnoreMappingFeature(IMapper mapper)
        {
            _mapper = mapper;
        }

        public Task RunAsync()
        {
            var dto = new DtoWithIgnore { Name = "X", IgnoredProp = "SHOULD BE IGNORED" };
            var domain = _mapper.Map<DomainWithIgnore>(dto);

            Console.WriteLine($"Name mapped: {domain.Name}");
            Console.WriteLine($"IgnoredProp (should be default null): '{domain.IgnoredProp ?? "null"}'");

            return Task.CompletedTask;
        }
    }

    public class DtoWithIgnore
    {
        public string Name { get; set; } = "";
        public string IgnoredProp { get; set; } = "";
    }

    public class DomainWithIgnore
    {
        public string Name { get; set; } = "";
        public string? IgnoredProp { get; set; }
    }
}