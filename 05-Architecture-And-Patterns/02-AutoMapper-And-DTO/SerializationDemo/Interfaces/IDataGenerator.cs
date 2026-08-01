using System.Collections.Generic;
using System.Threading.Tasks;
using SerializationDemo.Models;

namespace SerializationDemo.Interfaces
{
    public interface IDataGenerator
    {
        Task<List<Customer>> GenerateCustomersAsync(int count);
    }
}