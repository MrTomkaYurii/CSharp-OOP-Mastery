using System.Globalization;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 26. РЕАКТИВНИЙ МОНІТОРИНГ ФАЙЛІВ (FILESYSTEMWATCHER) ТА CSV-КОНВЕЄР");
Console.WriteLine(" Відстеження подій ОС у реальному часі та ліниве потокове читання CSV");
Console.WriteLine("==========================================================================\n");

string watchDir = Path.Combine(Path.GetTempPath(), $"CsvInbox_{Guid.NewGuid():N}");
Directory.CreateDirectory(watchDir);

var detectedFiles = new List<string>();

// -----------------------------------------------------------------------
// Крок 1: Налаштування FileSystemWatcher для моніторингу папки Inbox
// -----------------------------------------------------------------------
Console.WriteLine($"--- Крок 1: Запуск FileSystemWatcher для директорії {Path.GetFileName(watchDir)} ---");

using var watcher = new FileSystemWatcher(watchDir, "*.csv")
{
    NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.Size,
    EnableRaisingEvents = true
};

watcher.Created += (_, e) =>
{
    Console.WriteLine($"  [Подія ОС Created] Виявлено новий файл: {e.Name}");
    lock (detectedFiles)
    {
        detectedFiles.Add(e.FullPath);
    }
};

watcher.Renamed += (_, e) =>
{
    Console.WriteLine($"  [Подія ОС Renamed] Файл перейменовано: {e.OldName} -> {e.Name}");
};

// -----------------------------------------------------------------------
// Крок 2: Генерація CSV-файлу транзакцій та спрацювання події
// -----------------------------------------------------------------------
string csvPath = Path.Combine(watchDir, "transactions_batch_01.csv");
await WriteSampleCsvAsync(csvPath);
await Task.Delay(150); // Даємо змогу потоку подій ОС доставити сповіщення

// -----------------------------------------------------------------------
// Крок 3: Потоковий конвеєр читання CSV (StreamReader + yield return)
// -----------------------------------------------------------------------
Console.WriteLine("\n--- Крок 2: Лінивий потоковий парсинг CSV без завантаження файлу в RAM ---");

var highValueTransactions = ReadCsvLazily(csvPath)
    .Where(t => t.Amount >= 5000m && t.Status == "COMPLETED")
    .OrderByDescending(t => t.Amount)
    .ToList();

Console.WriteLine($"Знайдено великих завершених транзакцій: {highValueTransactions.Count}");
foreach (var tx in highValueTransactions)
{
    Console.WriteLine($"  - [{tx.TransactionId}] Клієнт: {tx.CustomerName,-18} | Сума: {tx.Amount,10:N2} грн | Категорія: {tx.Category}");
}

Directory.Delete(watchDir, recursive: true);

static async Task WriteSampleCsvAsync(string path)
{
    await using var fs = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.Read);
    await using var writer = new StreamWriter(fs, Encoding.UTF8);

    await writer.WriteLineAsync("TransactionId;CustomerName;Category;Amount;Status");
    await writer.WriteLineAsync("TX-1001;Олександр Коваль;Hardware;18500.50;COMPLETED");
    await writer.WriteLineAsync("TX-1002;Марія Бондаренко;Books;650.00;COMPLETED");
    await writer.WriteLineAsync("TX-1003;Дмитро Савченко;CloudServices;7400.00;PENDING");
    await writer.WriteLineAsync("TX-1004;Ірина Ткаченко;SoftwareLicenses;12900.00;COMPLETED");
    await writer.WriteLineAsync("TX-1005;Андрій Лисенко;Consulting;31000.00;COMPLETED");
}

static IEnumerable<CsvTransaction> ReadCsvLazily(string path)
{
    using var reader = new StreamReader(path, Encoding.UTF8);
    string? header = reader.ReadLine(); // Пропускаємо заголовок

    while (!reader.EndOfStream)
    {
        string? line = reader.ReadLine();
        if (string.IsNullOrWhiteSpace(line))
            continue;

        string[] parts = line.Split(';');
        if (parts.Length == 5 &&
            decimal.TryParse(parts[3], NumberStyles.Number, CultureInfo.InvariantCulture, out decimal amount))
        {
            yield return new CsvTransaction(parts[0], parts[1], parts[2], amount, parts[4]);
        }
    }
}

public record CsvTransaction(string TransactionId, string CustomerName, string Category, decimal Amount, string Status);
