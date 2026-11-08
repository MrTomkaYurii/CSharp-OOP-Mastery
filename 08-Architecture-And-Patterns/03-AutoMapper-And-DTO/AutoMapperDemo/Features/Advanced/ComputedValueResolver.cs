using AutoMapper;

namespace AutoMapperDemo.Features.Advanced
{
    // Окремий файл для ComputedValueResolver (якщо потрібно)
    public class ComputedValueResolver : IValueResolver<SourceCustom, DestCustom, int>
    {
        public int Resolve(SourceCustom source, DestCustom destination, int destMember, ResolutionContext context)
        {
            return source.A * 100 + source.B;
        }
    }
}