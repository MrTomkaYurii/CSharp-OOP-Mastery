using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SerializationDemo.Interfaces;
using SerializationDemo.Models;

namespace SerializationDemo
{
    public class SerializationService : ISerializationService
    {
        // Налаштування JSON, коли ми хочемо PascalCase (як у властивостей C#)
        private static readonly JsonSerializerOptions PascalCaseOptions = new JsonSerializerOptions
        {
            WriteIndented = true,       // Форматований вивід (з відступами)
            PropertyNamingPolicy = null // Null означає, що використовуються імена властивостей як є (PascalCase)
        };

        // Налаштування JSON, коли ми хочемо camelCase (поширений стиль у JS/JSON)
        private static readonly JsonSerializerOptions CamelCaseOptions = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        public string SerializeToJson(List<Customer> customers, bool useCamelCase)
        {
            var options = useCamelCase ? CamelCaseOptions : PascalCaseOptions;
            var json = JsonSerializer.Serialize(customers, options);
            return json;
        }

        public List<Customer>? DeserializeFromJson(string json, bool useCamelCase)
        {
            var options = useCamelCase ? CamelCaseOptions : PascalCaseOptions;
            var customers = JsonSerializer.Deserialize<List<Customer>>(json, options);
            return customers;
        }

        public string SerializeToXml(List<Customer> customers)
        {
            var serializer = new XmlSerializer(typeof(List<Customer>));
            using var stringWriter = new Utf8StringWriter();
            serializer.Serialize(stringWriter, customers);
            return stringWriter.ToString();
        }

        public List<Customer>? DeserializeFromXml(string xml)
        {
            var serializer = new XmlSerializer(typeof(List<Customer>));
            using var stringReader = new StringReader(xml);
            var result = serializer.Deserialize(stringReader) as List<Customer>;
            return result;
        }

        // ===== JSON у файли =====

        public async Task SerializeToJsonFileAsync(List<Customer> customers, string filePath, bool useCamelCase)
        {
            var options = useCamelCase ? CamelCaseOptions : PascalCaseOptions;

            // Відкриваємо файл для запису (створюємо або перезаписуємо)
            await using var fileStream = File.Create(filePath);

            // Серіалізація напряму в потік файлу
            await JsonSerializer.SerializeAsync(fileStream, customers, options);

            // Важливо: переконатись, що потік закривається (через using) — тоді дані гарантовано записуються
        }

        public async Task<List<Customer>?> DeserializeFromJsonFileAsync(string filePath, bool useCamelCase)
        {
            if (!File.Exists(filePath))
                return null;

            var options = useCamelCase ? CamelCaseOptions : PascalCaseOptions;

            await using var fileStream = File.OpenRead(filePath);

            // Десеріалізуємо напряму з файлового потоку
            var customers = await JsonSerializer.DeserializeAsync<List<Customer>>(fileStream, options);
            return customers;
        }

        // ===== XML у файли =====

        public async Task SerializeToXmlFileAsync(List<Customer> customers, string filePath)
        {
            var serializer = new XmlSerializer(typeof(List<Customer>));

            // XmlSerializer сам уміє писати в потік, але API синхронне
            await using var fileStream = File.Create(filePath);

            // Обгортаємо FileStream у StreamWriter з UTF-8
            using var streamWriter = new StreamWriter(fileStream, new UTF8Encoding(false));

            serializer.Serialize(streamWriter, customers);

            // Хоча XmlSerializer синхронний, використання await using для fileStream коректно закриє файл
            await streamWriter.FlushAsync();
        }

        public async Task<List<Customer>?> DeserializeFromXmlFileAsync(string filePath)
        {
            if (!File.Exists(filePath))
                return null;

            var serializer = new XmlSerializer(typeof(List<Customer>));

            await using var fileStream = File.OpenRead(filePath);
            using var streamReader = new StreamReader(fileStream, Encoding.UTF8);

            var result = serializer.Deserialize(streamReader) as List<Customer>;
            return result;
        }

        /// <summary>
        /// Допоміжний клас, щоб XmlSerializer видавав XML у UTF-8 замість UTF-16, коли ми працюємо зі StringWriter.
        /// </summary>
        private class Utf8StringWriter : StringWriter
        {
            public override Encoding Encoding => Encoding.UTF8;
        }
    }
}