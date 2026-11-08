using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronousProgramming
{
    /// <summary>
    /// =========================================================================================
    /// ОБРОБКА ПОМИЛОК В АСИНХРОННОМУ I/O: Відловлювання винятків у Task<T>
    /// =========================================================================================
    /// Демонструє виникнення помилок при асинхронній операції введення-виведення (I/O).
    /// Показує фундаментальну перевагу оператора 'await':
    /// - Класичний блок try/catch перехоплює виняток, розгорнутий оператором 'await'
    ///   (без необхідності вручну розпаковувати громіздкий AggregateException).
    /// - Демонструє відмінність між безпечним 'await' та блокуючим викликом '.Result'.
    /// </summary>
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("======================================================================");
            Console.WriteLine(" ОБРОБКА ПОМИЛОК В Task<T>: Відловлювання винятків через try / catch");
            Console.WriteLine("======================================================================\n");

            Console.WriteLine($"[Main Thread #{Thread.CurrentThread.ManagedThreadId}] Старт головної програми.\n");

            // Сценарій 1: Безпечна обробка помилки через оператор await
            Console.WriteLine("--- СЦЕНАРІЙ 1: Ідіоматична обробка через await (Розгортання винятку) ---");
            await LoadConfigurationWithAwaitAsync("non_existent_system_config.json");

            // Сценарій 2: Демонстрація того, як .Result загортає помилку в AggregateException
            Console.WriteLine("\n--- СЦЕНАРІЙ 2: Блокуючий виклик .Result (Антипатерн: AggregateException) ---");
            DemonstrateBlockingResultTrap("missing_archive.dat");

            Console.WriteLine($"\n[Main Thread #{Thread.CurrentThread.ManagedThreadId}] Програма успішно завершила всі демонстрації.");
        }

        // =====================================================================================
        // Сценарій 1: Рекомендований спосіб — try/catch разом з оператором await
        // =====================================================================================
        public static async Task LoadConfigurationWithAwaitAsync(string configFileName)
        {
            Console.WriteLine($"[Сценарій 1] Спроба асинхронно зчитати файл: '{configFileName}'...");

            try
            {
                // Оператор await автоматично розгортає AggregateException
                // і викидає конкретний тип винятку: FileNotFoundException
                string content = await ReadFileContentAsync(configFileName);
                Console.WriteLine($"Зчитано дані: {content}");
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"\n[Успішно перехоплено FileNotFoundException]:");
                Console.WriteLine($"  - Повідомлення: {ex.Message}");
                Console.WriteLine($"  - Відсутній файл: {ex.FileName}");
                Console.WriteLine($"  - Поведінка: Застосунок не впав, застосовано резервну конфігурацію за замовчуванням.\n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[Загальний виняток]: {ex.Message}");
            }
        }

        // =====================================================================================
        // Сценарій 2: Антипатерн — блокуюче звернення через .Result
        // =====================================================================================
        public static void DemonstrateBlockingResultTrap(string missingFileName)
        {
            Console.WriteLine($"[Сценарій 2] Звернення до задачі через блокуючу властивість .Result...");

            Task<string> task = ReadFileContentAsync(missingFileName);

            try
            {
                // УВАГА: звернення до .Result блокує потік та загортає помилку в AggregateException!
                string result = task.Result;
                Console.WriteLine($"Результат: {result}");
            }
            catch (AggregateException aggEx)
            {
                Console.WriteLine($"\n[Пастка .Result / .Wait()]: Виник AggregateException!");
                Console.WriteLine($"  - Тип винятку: {aggEx.GetType().Name}");
                Console.WriteLine($"  - Кількість внутрішніх винятків: {aggEx.InnerExceptions.Count}");
                foreach (var inner in aggEx.InnerExceptions)
                {
                    Console.WriteLine($"  -> Внутрішня помилка: {inner.GetType().Name}: {inner.Message}");
                }
                Console.WriteLine($"\n[Висновок]: Завжди використовуйте 'await', щоб уникнути AggregateException!");
            }
        }

        // =====================================================================================
        // Допоміжний асинхронний метод читання файлу
        // =====================================================================================
        public static async Task<string> ReadFileContentAsync(string fileName)
        {
            // Невелика асинхронна пауза (імітація перевірки дискової підсистеми)
            await Task.Delay(150);

            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), fileName);

            if (!File.Exists(fullPath))
            {
                // Генеруємо виняток відсутності файлу
                throw new FileNotFoundException($"Файл конфігурації '{fileName}' не знайдено на диску.", fileName);
            }

            return await File.ReadAllTextAsync(fullPath);
        }
    }
}
