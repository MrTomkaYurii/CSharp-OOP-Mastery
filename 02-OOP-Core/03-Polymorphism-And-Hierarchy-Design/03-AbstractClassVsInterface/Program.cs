using System;
using System.Collections.Generic;

namespace AbstractClassVsInterface;

/*
 * ── 03. Абстрактний клас проти Інтерфейсу та патерн Template Method ─────
 *
 * Коли проектувати `abstract class`, а коли `interface`?
 *
 *   1. `abstract class` ("ХТО ТИ Є" / IS-A + спільний стан + каркас алгоритму):
 *      Використовується, коли класи мають спільну природу, спільні поля стану
 *      та єдиний незмінний конвеєр виконання (патерн Template Method).
 *
 *   2. `interface` ("ЩО ТИ ВМІЄШ РОБИТИ" / CAN-DO роль):
 *      Використовується для надання ортогональних здібностей (наприклад,
 *      `IEncryptable`, `ICompressible`), які можуть підтримувати лише окремі
 *      експортери або взагалі класи з інших ієрархій.
 */

// Інтерфейси описують додаткові РОЛІ (здібності CAN-DO):
public interface ICompressible
{
    string CompressPayload(string payload);
}

public interface IEncryptable
{
    string EncryptPayload(string payload, string keyId);
}

// Абстрактний клас задає БАЗОВИЙ СТАН і КАРКАС АЛГОРИТМУ (Template Method):
public abstract class ReportExporter
{
    public string ExporterName { get; }
    public int ExportCount { get; private set; }

    protected ReportExporter(string exporterName)
    {
        ExporterName = exporterName;
    }

    // Патерн Template Method: метод НЕ є віртуальним (його кроки зафіксовані!),
    // але всередині він викликає абстрактні/віртуальні кроки похідних класів.
    public string ExportReport(string title, IReadOnlyList<string> rows)
    {
        ValidateInput(title, rows);
        string body = FormatDocument(title, rows);

        // Якщо конкретний спадкоємець реалізує роль ICompressible або IEncryptable — застосовуємо:
        if (this is ICompressible compressible)
            body = compressible.CompressPayload(body);

        if (this is IEncryptable encryptable)
            body = encryptable.EncryptPayload(body, "KMS-KEY-01");

        ExportCount++;
        return body;
    }

    protected virtual void ValidateInput(string title, IReadOnlyList<string> rows)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Заголовок звіту порожній.");
    }

    // Абстрактний крок — кожен формат звіту формує розмітку по-своєму:
    protected abstract string FormatDocument(string title, IReadOnlyList<string> rows);
}

// 1. Звичайний CSV-експортер: лише спадкує абстрактний каркас
public sealed class CsvReportExporter : ReportExporter
{
    public CsvReportExporter() : base("CSV Exporter") { }

    protected override string FormatDocument(string title, IReadOnlyList<string> rows)
        => $"# {title}\n" + string.Join("\n", rows);
}

// 2. Захищений архівний JSON-експортер: спадкує каркас ReportExporter + реалізує 2 інтерфейсні ролі!
public sealed class SecureArchiveJsonExporter : ReportExporter, ICompressible, IEncryptable
{
    public SecureArchiveJsonExporter() : base("Secure JSON Exporter") { }

    protected override string FormatDocument(string title, IReadOnlyList<string> rows)
        => $"{{ \"title\": \"{title}\", \"items\": [\"{string.Join("\", \"", rows)}\"] }}";

    public string CompressPayload(string payload)
        => $"GZIP({payload})";

    public string EncryptPayload(string payload, string keyId)
        => $"AES256[{keyId}]<{payload}>";
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var data = new[] { "Order #101: $250", "Order #102: $480" };
        ReportExporter[] exporters =
        {
            new CsvReportExporter(),
            new SecureArchiveJsonExporter()
        };

        foreach (ReportExporter exporter in exporters)
        {
            Console.WriteLine($"=== {exporter.ExporterName} ===");
            string result = exporter.ExportReport("Фінансовий звіт Q4", data);
            Console.WriteLine(result);
            Console.WriteLine($"Виконано експортів: {exporter.ExportCount}\n");
        }
    }
}
