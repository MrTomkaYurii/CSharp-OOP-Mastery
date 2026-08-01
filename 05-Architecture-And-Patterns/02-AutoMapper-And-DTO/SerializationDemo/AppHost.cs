using SerializationDemo.Interfaces;
using System;
using System.Threading.Tasks;

namespace SerializationDemo
{
    public class AppHost
    {
        private readonly IDataGenerator _dataGenerator;
        private readonly IDemoRunner _demoRunner;

        public AppHost(IDataGenerator dataGenerator, IDemoRunner demoRunner)
        {
            _dataGenerator = dataGenerator;
            _demoRunner = demoRunner;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Демонстрація серіалізації / десеріалізації (.NET Generic Host) ===");
                Console.WriteLine("1. Згенерувати дані та показати статистику");
                Console.WriteLine("2. Демонстрація JSON (серіалізація / десеріалізація)");
                Console.WriteLine("3. Демонстрація XML (серіалізація / десеріалізація)");
                Console.WriteLine("4. Демонстрація налаштувань JSON (CamelCase / PascalCase)");
                Console.WriteLine("5. Вихід");
                Console.Write("Оберіть пункт меню: ");

                var key = Console.ReadLine();

                switch (key)
                {
                    case "1":
                        await ShowDataStatsAsync();
                        break;
                    case "2":
                        await _demoRunner.RunJsonDemoAsync();
                        break;
                    case "3":
                        await _demoRunner.RunXmlDemoAsync();
                        break;
                    case "4":
                        await _demoRunner.RunJsonSettingsDemoAsync();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Невідомий пункт меню. Натисніть будь-яку клавішу...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private async Task ShowDataStatsAsync()
        {
            var customers = await _dataGenerator.GenerateCustomersAsync(10);
            Console.WriteLine();
            Console.WriteLine($"Згенеровано клієнтів: {customers.Count}");

            var totalOrders = 0;
            var totalOrderItems = 0;
            foreach (var c in customers)
            {
                foreach (var o in c.Orders)
                {
                    totalOrders++;
                    totalOrderItems += o.Items.Count;
                }
            }

            Console.WriteLine($"Загальна кількість замовлень: {totalOrders}");
            Console.WriteLine($"Загальна кількість позицій в замовленнях: {totalOrderItems}");

            Console.WriteLine();
            Console.WriteLine("Натисніть будь-яку клавішу для повернення в меню...");
            Console.ReadKey();
        }
    }
}