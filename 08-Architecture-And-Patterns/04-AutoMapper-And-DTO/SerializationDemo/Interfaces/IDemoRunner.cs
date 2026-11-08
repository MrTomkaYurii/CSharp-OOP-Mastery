using System.Threading.Tasks;

namespace SerializationDemo.Interfaces
{
    public interface IDemoRunner
    {
        Task RunJsonDemoAsync();
        Task RunXmlDemoAsync();
        Task RunJsonSettingsDemoAsync();
    }
}