using System.IO.Compression;
using System.Text;
using System.Text.Json;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 25. ДЕКОРАТОРИ ПОТОКІВ: СТИСНЕННЯ (GZIP / BROTLI) ТА ZIP-АРХІВИ");
Console.WriteLine(" Комбінування FileStream, MemoryStream, GZipStream, BrotliStream і ZipArchive");
Console.WriteLine("==========================================================================\n");

// Генеруємо тестовий масив телеметрії для серіалізації та стиснення
var logs = Enumerable.Range(1, 500)
    .Select(i => new AuditLogEntry(
        Id: i,
        TimestampUtc: DateTime.UtcNow.AddMinutes(-i),
        Level: i % 10 == 0 ? "WARNING" : "INFO",
        Service: "OrderProcessingService",
        Message: $"Оброблено транзакцію #{10000 + i} у кластері EU-CENTRAL-1 без затримок."))
    .ToList();

byte[] rawJsonBytes = JsonSerializer.SerializeToUtf8Bytes(logs, new JsonSerializerOptions { WriteIndented = true });

// -----------------------------------------------------------------------
// Крок 1: Патерн Decorator — стиснення потоку через GZipStream та BrotliStream
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 1: Порівняння GZipStream та BrotliStream поверх MemoryStream ---");

byte[] gzipBytes = CompressWithGZip(rawJsonBytes);
byte[] brotliBytes = CompressWithBrotli(rawJsonBytes);

Console.WriteLine($"  Оригінальний розмір JSON: {rawJsonBytes.Length,8:N0} байтів (100.0%)");
Console.WriteLine($"  Стиснений GZipStream:     {gzipBytes.Length,8:N0} байтів ({gzipBytes.Length * 100.0 / rawJsonBytes.Length:F1}%)");
Console.WriteLine($"  Стиснений BrotliStream:   {brotliBytes.Length,8:N0} байтів ({brotliBytes.Length * 100.0 / rawJsonBytes.Length:F1}%)\n");

// Перевірка розпакування напряму через JsonSerializer
using (var compressedInput = new MemoryStream(brotliBytes))
using (var brotliDecompressor = new BrotliStream(compressedInput, CompressionMode.Decompress))
{
    var restoredLogs = JsonSerializer.Deserialize<List<AuditLogEntry>>(brotliDecompressor);
    Console.WriteLine($"  Успішно десеріалізовано з Brotli-потоку записів: {restoredLogs?.Count}\n");
}

// -----------------------------------------------------------------------
// Крок 2: Створення та інспекція багатофайлового ZIP-архіву (ZipArchive)
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 2: Пакетування звітів у ZIP-архів на льоту (ZipArchive) ---");
string zipFilePath = Path.Combine(Path.GetTempPath(), $"backup_bundle_{Guid.NewGuid():N}.zip");

using (FileStream zipFileStream = File.Create(zipFilePath))
using (var archive = new ZipArchive(zipFileStream, ZipArchiveMode.Create))
{
    // 1. Записуємо JSON-звіт усередину архіву
    ZipArchiveEntry jsonEntry = archive.CreateEntry("data/audit_logs.json", CompressionLevel.Optimal);
    using (Stream entryStream = jsonEntry.Open())
    {
        entryStream.Write(rawJsonBytes, 0, rawJsonBytes.Length);
    }

    // 2. Записуємо текстовий маніфест
    ZipArchiveEntry manifestEntry = archive.CreateEntry("manifest.txt", CompressionLevel.Fastest);
    using (var writer = new StreamWriter(manifestEntry.Open(), Encoding.UTF8))
    {
        writer.WriteLine("Backup Bundle v1.0");
        writer.WriteLine($"CreatedUtc: {DateTime.UtcNow:O}");
        writer.WriteLine($"TotalRecords: {logs.Count}");
    }
}

Console.WriteLine($"Створено архів: {Path.GetFileName(zipFilePath)} ({new FileInfo(zipFilePath).Length:N0} байтів)");

using (ZipArchive readArchive = ZipFile.OpenRead(zipFilePath))
{
    Console.WriteLine("Вміст ZIP-архіву:");
    foreach (ZipArchiveEntry entry in readArchive.Entries)
    {
        Console.WriteLine($"  - {entry.FullName,-25} | Оригінал: {entry.Length,8:N0} Б | У стисненому вигляді: {entry.CompressedLength,8:N0} Б");
    }
}

File.Delete(zipFilePath);

static byte[] CompressWithGZip(byte[] data)
{
    using var output = new MemoryStream();
    using (var gzip = new GZipStream(output, CompressionLevel.Optimal, leaveOpen: true))
    {
        gzip.Write(data, 0, data.Length);
    }
    return output.ToArray();
}

static byte[] CompressWithBrotli(byte[] data)
{
    using var output = new MemoryStream();
    using (var brotli = new BrotliStream(output, CompressionLevel.Optimal, leaveOpen: true))
    {
        brotli.Write(data, 0, data.Length);
    }
    return output.ToArray();
}

public record AuditLogEntry(int Id, DateTime TimestampUtc, string Level, string Service, string Message);
