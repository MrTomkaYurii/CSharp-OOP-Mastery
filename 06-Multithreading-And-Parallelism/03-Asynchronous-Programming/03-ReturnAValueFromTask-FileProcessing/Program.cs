using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronousProgramming
{
    /// <summary>
    /// =========================================================================================
    /// АСИНХРОННІ ОПЕРАЦІЇ ВВЕДЕННЯ-ВИВЕДЕННЯ (I/O-BOUND): Робота з файлами через FileStream
    /// =========================================================================================
    /// Цей проект демонструє справжню неблокуючу асинхронність на рівні операційної системи.
    /// Замість блокування потоку процесора, виклики File.WriteAllTextAsync та File.ReadAllTextAsync
    /// використовують Overlapped I/O та I/O Completion Ports (IOCP) ядра Windows/Linux.
    /// Потік повертається в пул потоків, доки контролер накопичувача (SSD/HDD) зчитує байти.
    /// </summary>
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("======================================================================");
            Console.WriteLine(" АСИНХРОННЕ ВВЕДЕННЯ-ВИВЕДЕННЯ: Генерація та аналіз звітів на диску");
            Console.WriteLine("======================================================================\n");

            Console.WriteLine($"[Main Thread #{Thread.CurrentThread.ManagedThreadId}] Початок роботи головного потоку.");

            string tempDirectory = Path.Combine(Path.GetTempPath(), "CSharp_Async_Mastery");
            Directory.CreateDirectory(tempDirectory);
            string reportFilePath = Path.Combine(tempDirectory, "monthly_performance_report.txt");

            try
            {
                // Крок 1: Асинхронний запис великого звіту на диск
                Console.WriteLine("\n--- КРОК 1: Асинхронний запис файлу звіту на диск ---");
                int linesWritten = await WriteReportFileAsync(reportFilePath, 500);
                Console.WriteLine($"[Успіх] Записано {linesWritten} рядків у файл: {reportFilePath}");

                // Крок 2: Асинхронне читання та обробка звіту з поверненням аналітики (Task<string>)
                Console.WriteLine("\n--- КРОК 2: Асинхронне зчитування та підрахунок статистики ---");
                string summary = await ReadAndAnalyzeReportAsync(reportFilePath);
                Console.WriteLine($"\n[Підсумковий результат аналітики]:\n{summary}");
            }
            finally
            {
                // Прибирання тимчасового файлу після завершення демонстрації
                if (File.Exists(reportFilePath))
                {
                    File.Delete(reportFilePath);
                    Console.WriteLine($"\n[Очищення]: Тимчасовий файл {Path.GetFileName(reportFilePath)} успішно видалено.");
                }
            }

            Console.WriteLine($"\n[Main Thread #{Thread.CurrentThread.ManagedThreadId}] Головний потік Main успішно завершив роботу.");
        }

        // =====================================================================================
        // Асинхронний запис: генерує контент і записує його без блокування потоку
        // =====================================================================================
        public static async Task<int> WriteReportFileAsync(string filePath, int recordCount)
        {
            Console.WriteLine($"  [Потік #{Thread.CurrentThread.ManagedThreadId}] Початок формування даних звіту...");

            var sb = new StringBuilder();
            sb.AppendLine("=== ЗВІТ ПРОДУКТИВНОСТІ СИСТЕМИ ЗА МІСЯЦЬ ===");
            sb.AppendLine($"Дата генерації: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
            sb.AppendLine("--------------------------------------------------");

            for (int i = 1; i <= recordCount; i++)
            {
                sb.AppendLine($"Транзакція #{i:D4}: Код операції 0x{i * 17:X4}, Статус: Успішно, Час: {i * 1.5:F1} мс");
            }

            // Асинхронний запис: потік звільняється на час запису на диск
            await File.WriteAllTextAsync(filePath, sb.ToString(), Encoding.UTF8);

            Console.WriteLine($"  [Потік #{Thread.CurrentThread.ManagedThreadId}] Асинхронний запис на диск завершено.");
            return recordCount;
        }

        // =====================================================================================
        // Асинхронне читання: повертає Task<string> з аналітичним звітом
        // =====================================================================================
        public static async Task<string> ReadAndAnalyzeReportAsync(string filePath)
        {
            Console.WriteLine($"  [Потік #{Thread.CurrentThread.ManagedThreadId}] Відкриваємо асинхронний потік для читання файлу...");

            // Неблокуюче зчитування всього тексту з диска:
            string content = await File.ReadAllTextAsync(filePath, Encoding.UTF8);

            Console.WriteLine($"  [Потік #{Thread.CurrentThread.ManagedThreadId}] Файл успішно зчитано. Виконуємо розрахунок метрик...");

            // Обчислення аналітики на основі зчитаних даних:
            string[] lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            long fileSizeBytes = new FileInfo(filePath).Length;

            // Формуємо підсумковий результат (Task<string>)
            string result = $"  - Назва файлу: {Path.GetFileName(filePath)}\n" +
                            $"  - Загальний обсяг даних: {fileSizeBytes} байт\n" +
                            $"  - Загальна кількість рядків: {lines.Length}\n" +
                            $"  - Перший рядок звіту: '{lines[0]}'\n" +
                            $"  - Останній рядок звіту: '{lines[^1]}'";

            return result;
        }
    }
}
