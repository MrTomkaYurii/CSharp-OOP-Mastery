using System.Threading.Tasks;

namespace AutoMapperDemo.Features
{
    public interface IFeature
    {
        string Title { get; }
        Task RunAsync();
    }
}