using System.Collections.Generic;
using System.Threading.Tasks;
using SerializationDemo.Models;

namespace SerializationDemo.Interfaces
{
    public interface ISerializationService
    {
        // JSON (рядок)
        string SerializeToJson(List<Customer> customers, bool useCamelCase);
        List<Customer>? DeserializeFromJson(string json, bool useCamelCase);

        // XML (рядок)
        string SerializeToXml(List<Customer> customers);
        List<Customer>? DeserializeFromXml(string xml);

        // JSON (файли)
        Task SerializeToJsonFileAsync(List<Customer> customers, string filePath, bool useCamelCase);
        Task<List<Customer>?> DeserializeFromJsonFileAsync(string filePath, bool useCamelCase);

        // XML (файли)
        Task SerializeToXmlFileAsync(List<Customer> customers, string filePath);
        Task<List<Customer>?> DeserializeFromXmlFileAsync(string filePath);
    }
}