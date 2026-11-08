using SerializationDemo.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SerializationDemo
{
    public class DemoRunner : IDemoRunner
    {
        private readonly IDataGenerator _dataGenerator;
        private readonly ISerializationService _serializationService;

        public DemoRunner(IDataGenerator dataGenerator, ISerializationService serializationService)
        {
            _dataGenerator = dataGenerator;
            _serializationService = serializationService;
        }

        public async Task RunJsonDemoAsync()
        {
            Console.Clear();
            Console.WriteLine("=== JSON Демонстрація ===");

            var customers = await _dataGenerator.GenerateCustomersAsync(5);

            // ---------- Серіалізація в пам'яті ----------
            Console.WriteLine();
            Console.WriteLine("Серіалізація у JSON (camelCase) в пам'яті...");
            var json = _serializationService.SerializeToJson(customers, useCamelCase: true);

            Console.WriteLine();
            Console.WriteLine("Фрагмент JSON (рядок в пам'яті):");
            Console.WriteLine(json.Length > 500 ? json[..500] + "..." : json);

            // ---------- Серіалізація в файл ----------
            var filePath = Path.Combine(Environment.CurrentDirectory, "customers.json");
            Console.WriteLine();
            Console.WriteLine($"Запис JSON у файл: {filePath}");
            await _serializationService.SerializeToJsonFileAsync(customers, filePath, useCamelCase: true);

            // ---------- Десеріалізація з пам'яті ----------
            Console.WriteLine();
            Console.WriteLine("Десеріалізація з JSON (рядок)...");
            var deserializedFromString = _serializationService.DeserializeFromJson(json, useCamelCase: true);
            Console.WriteLine($"Кількість клієнтів після десеріалізації з рядка: {deserializedFromString?.Count ?? 0}");

            // ---------- Десеріалізація з файлу ----------
            Console.WriteLine();
            Console.WriteLine("Десеріалізація з JSON файлу...");
            var deserializedFromFile = await _serializationService.DeserializeFromJsonFileAsync(filePath, useCamelCase: true);
            Console.WriteLine($"Кількість клієнтів після десеріалізації з файлу: {deserializedFromFile?.Count ?? 0}");

            if (deserializedFromFile is { Count: > 0 })
            {
                var first = deserializedFromFile.First();
                Console.WriteLine($"Перший клієнт (з файлу): {first.Name}, замовлень: {first.Orders.Count}");
            }

            Console.WriteLine();
            Console.WriteLine("Натисніть будь-яку клавішу для повернення в меню...");
            Console.ReadKey();
        }

        public async Task RunXmlDemoAsync()
        {
            Console.Clear();
            Console.WriteLine("=== XML Демонстрація ===");

            var customers = await _dataGenerator.GenerateCustomersAsync(3);

            // ---------- Серіалізація в пам'яті ----------
            Console.WriteLine();
            Console.WriteLine("Серіалізація у XML (рядок в пам'яті)...");
            var xml = _serializationService.SerializeToXml(customers);

            Console.WriteLine();
            Console.WriteLine("Фрагмент XML (рядок в пам'яті):");
            Console.WriteLine(xml.Length > 500 ? xml[..500] + "..." : xml);

            // ---------- Серіалізація в файл ----------
            var filePath = Path.Combine(Environment.CurrentDirectory, "customers.xml");
            Console.WriteLine();
            Console.WriteLine($"Запис XML у файл: {filePath}");
            await _serializationService.SerializeToXmlFileAsync(customers, filePath);

            // ---------- Десеріалізація з пам'яті ----------
            Console.WriteLine();
            Console.WriteLine("Десеріалізація з XML (рядок)...");
            var deserializedFromString = _serializationService.DeserializeFromXml(xml);
            Console.WriteLine($"Кількість клієнтів після десеріалізації з рядка: {deserializedFromString?.Count ?? 0}");

            // ---------- Десеріалізація з файлу ----------
            Console.WriteLine();
            Console.WriteLine("Десеріалізація з XML файлу...");
            var deserializedFromFile = await _serializationService.DeserializeFromXmlFileAsync(filePath);
            Console.WriteLine($"Кількість клієнтів після десеріалізації з файлу: {deserializedFromFile?.Count ?? 0}");

            if (deserializedFromFile is { Count: > 0 })
            {
                var first = deserializedFromFile.First();
                Console.WriteLine($"Перший клієнт (з файлу): {first.Name}, місто: {first.Address.City}");
            }

            Console.WriteLine();
            Console.WriteLine("Натисніть будь-яку клавішу для повернення в меню...");
            Console.ReadKey();
        }

        public async Task RunJsonSettingsDemoAsync()
        {
            Console.Clear();
            Console.WriteLine("=== Демонстрація налаштувань JSON (CamelCase / PascalCase) ===");

            var customers = await _dataGenerator.GenerateCustomersAsync(2);

            // Серіалізація з PascalCase (імена властивостей як у C#)
            Console.WriteLine();
            Console.WriteLine("JSON з PascalCase (PropertyNamingPolicy = null):");
            var jsonPascal = _serializationService.SerializeToJson(customers, useCamelCase: false);
            Console.WriteLine(jsonPascal.Length > 400 ? jsonPascal[..400] + "..." : jsonPascal);

            // Серіалізація з camelCase
            Console.WriteLine();
            Console.WriteLine("JSON з camelCase (PropertyNamingPolicy = JsonNamingPolicy.CamelCase):");
            var jsonCamel = _serializationService.SerializeToJson(customers, useCamelCase: true);
            Console.WriteLine(jsonCamel.Length > 400 ? jsonCamel[..400] + "..." : jsonCamel);

            Console.WriteLine();
            Console.WriteLine("Зверніть увагу на відмінність у іменах властивостей (Id vs id, Name vs name тощо).");
            Console.WriteLine();
            Console.WriteLine("Натисніть будь-яку клавішу для повернення в меню...");
            Console.ReadKey();
        }
    }
}