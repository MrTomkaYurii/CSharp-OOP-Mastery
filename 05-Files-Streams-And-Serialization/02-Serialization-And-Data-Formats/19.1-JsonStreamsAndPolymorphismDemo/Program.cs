using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 19.1. ПРОСУНУТА РОБОТА З JSON: ПОТОКИ (STREAMS), ПОЛІМОРФІЗМ ТА UTF8WRITER");
Console.WriteLine(" Серіалізація ієрархій ООП напряму у FileStream без алокації великих рядків");
Console.WriteLine("==========================================================================\n");

var options = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic)
};

// -----------------------------------------------------------------------
// Крок 1: Поліморфна серіалізація ієрархії ООП ([JsonPolymorphic], [JsonDerivedType])
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 1: Поліморфна серіалізація колекції базового типу PaymentMethod ---");

var invoice = new OrderInvoice(
    InvoiceNumber: "INV-2026-0042",
    CreatedAtUtc: new DateTime(2026, 11, 8, 18, 30, 0, DateTimeKind.Utc),
    Payments:
    [
        new BankCardPayment(12500.00m, "UAH", "4149-****-****-8821", "Monobank"),
        new CryptoTransferPayment(350.00m, "USDT", "TRC20", "TX99a8b7c6d5e4f3"),
        new WireTransferPayment(48000.00m, "UAH", "UA213223130000026007233566001", "ТОВ ІТ-Системи")
    ]);

// -----------------------------------------------------------------------
// Крок 2: Прямий асинхронний запис та читання через FileStream (Zero-String Allocation)
// -----------------------------------------------------------------------
string tempJsonPath = Path.Combine(Path.GetTempPath(), "invoice_polymorphic_demo.json");

await using (FileStream writeStream = File.Create(tempJsonPath))
{
    await JsonSerializer.SerializeAsync(writeStream, invoice, options);
}

Console.WriteLine($"JSON успішно записано напряму у файловий потік: {tempJsonPath}\n");
Console.WriteLine(await File.ReadAllTextAsync(tempJsonPath));

Console.WriteLine("\n--- Крок 2: Десеріалізація з FileStream зі збереженням конкретних підкласів ---");
await using (FileStream readStream = File.OpenRead(tempJsonPath))
{
    OrderInvoice? restoredInvoice = await JsonSerializer.DeserializeAsync<OrderInvoice>(readStream, options);
    if (restoredInvoice is not null)
    {
        Console.WriteLine($"Накладна: {restoredInvoice.InvoiceNumber}");
        foreach (PaymentMethod payment in restoredInvoice.Payments)
        {
            Console.WriteLine($"  -> Відновлено тип [{payment.GetType().Name}]: {payment.Describe()}");
        }
    }
}
Console.WriteLine();

// -----------------------------------------------------------------------
// Крок 3: Високопродуктивний низькорівневий запис через Utf8JsonWriter
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 3: Низькорівнева потокова генерація JSON через Utf8JsonWriter ---");
using var memoryStream = new MemoryStream();
await using (var writer = new Utf8JsonWriter(memoryStream, new JsonWriterOptions { Indented = true }))
{
    writer.WriteStartObject();
    writer.WriteString("service", "TelemetryGateway");
    writer.WriteNumber("activeConnections", 128);
    writer.WriteStartArray("metrics");
    writer.WriteNumberValue(12.4);
    writer.WriteNumberValue(15.8);
    writer.WriteNumberValue(11.9);
    writer.WriteEndArray();
    writer.WriteEndObject();
}

string fastJson = Encoding.UTF8.GetString(memoryStream.ToArray());
Console.WriteLine(fastJson);

File.Delete(tempJsonPath);

// =======================================================================
// Поліморфна модель даних ООП з атрибутами System.Text.Json
// =======================================================================
public record OrderInvoice(
    string InvoiceNumber,
    DateTime CreatedAtUtc,
    List<PaymentMethod> Payments);

[JsonPolymorphic(TypeDiscriminatorPropertyName = "$paymentType")]
[JsonDerivedType(typeof(BankCardPayment), typeDiscriminator: "card")]
[JsonDerivedType(typeof(CryptoTransferPayment), typeDiscriminator: "crypto")]
[JsonDerivedType(typeof(WireTransferPayment), typeDiscriminator: "iban")]
public abstract record PaymentMethod(decimal Amount, string Currency)
{
    public abstract string Describe();
}

public sealed record BankCardPayment(decimal Amount, string Currency, string MaskedPan, string BankName)
    : PaymentMethod(Amount, Currency)
{
    public override string Describe() => $"Картка {MaskedPan} ({BankName}) на суму {Amount} {Currency}";
}

public sealed record CryptoTransferPayment(decimal Amount, string Currency, string Network, string TxHash)
    : PaymentMethod(Amount, Currency)
{
    public override string Describe() => $"Крипто-переказ {Amount} {Currency} у мережі {Network} (Tx: {TxHash})";
}

public sealed record WireTransferPayment(decimal Amount, string Currency, string Iban, string Counterparty)
    : PaymentMethod(Amount, Currency)
{
    public override string Describe() => $"Банківський переказ IBAN {Iban} ({Counterparty}) на суму {Amount} {Currency}";
}
