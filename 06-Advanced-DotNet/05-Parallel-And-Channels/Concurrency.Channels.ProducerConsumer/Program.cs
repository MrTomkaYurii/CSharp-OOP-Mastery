using System.Diagnostics;
using System.Text;
using System.Threading.Channels;
using Concurrency.Channels.ProducerConsumer.Models;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("=================================================================");
Console.WriteLine("  .NET 10 CONCURRENCY: SYSTEM.THREADING.CHANNELS (PRODUCER-CONSUMER)");
Console.WriteLine("=================================================================\n");

// Створюємо обмежений канал (Bounded Channel) на 5 елементів з політикою Backpressure
var channelOptions = new BoundedChannelOptions(capacity: 5)
{
    FullMode = BoundedChannelFullMode.Wait, // Якщо черга заповнена — продюсер асинхронно чекає звільнення місця
    SingleWriter = false,
    SingleReader = false
};

var channel = Channel.CreateBounded<TelemetryPacket>(channelOptions);

Console.WriteLine("1. Запуск 2-х незалежних Продюсерів (генераторів пакетів телеметрії)...");
Console.WriteLine("2. Запуск 3-х незалежних Споживачів (обробників черги каналу)...\n");

var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));

// Запускаємо двох продюсерів
var producer1 = ProduceTelemetryAsync(channel.Writer, producerName: "IoT-Sensor-Alpha", packetCount: 6, cts.Token);
var producer2 = ProduceTelemetryAsync(channel.Writer, producerName: "IoT-Sensor-Beta",  packetCount: 6, cts.Token);

// Коли всі продюсери завершать запис, ми завершуємо канал (Writer.Complete)
var producersCompletion = Task.WhenAll(producer1, producer2).ContinueWith(_ =>
{
    channel.Writer.Complete();
    Console.WriteLine("\n[Канал закрито]: Всі продюсери завершили генерацію. Writer.Complete() викликано.\n");
});

// Запускаємо трьох паралельних споживачів
var consumers = new List<Task>
{
    ConsumeTelemetryAsync(channel.Reader, consumerId: 1, cts.Token),
    ConsumeTelemetryAsync(channel.Reader, consumerId: 2, cts.Token),
    ConsumeTelemetryAsync(channel.Reader, consumerId: 3, cts.Token)
};

// Очікуємо завершення обробки всіх повідомлень споживачами
await Task.WhenAll(consumers);

Console.WriteLine("\n=================================================================");
Console.WriteLine("  Concurrency.Channels успішно завершив виконання!");
Console.WriteLine("=================================================================");

static async Task ProduceTelemetryAsync(ChannelWriter<TelemetryPacket> writer, string producerName, int packetCount, CancellationToken ct)
{
    for (int i = 1; i <= packetCount; i++)
    {
        var packet = new TelemetryPacket(Guid.NewGuid(), producerName, Temperature: 20.0 + (i * 0.5), Timestamp: DateTime.UtcNow);

        // Якщо канал переповнений, WriteAsync асинхронно призупиняється без блокування потоку (Backpressure)
        await writer.WriteAsync(packet, ct);
        Console.WriteLine($"[+] {producerName} відправив пакет #{i} ({packet.Temperature:F1}°C)");
        await Task.Delay(100, ct); // Пауза між замірами сенсора
    }
}

static async Task ConsumeTelemetryAsync(ChannelReader<TelemetryPacket> reader, int consumerId, CancellationToken ct)
{
    // Читаємо через IAsyncEnumerable доки канал відкритий та не спорожнілий
    await foreach (var packet in reader.ReadAllAsync(ct))
    {
        // Імітуємо збереження в аналітичне сховище
        await Task.Delay(150, ct);
        Console.WriteLine($"      [-] Споживач #{consumerId} успішно обробив пакет від [{packet.SensorName}] ({packet.Temperature:F1}°C)");
    }
}

namespace Concurrency.Channels.ProducerConsumer.Models
{
    public record TelemetryPacket(Guid Id, string SensorName, double Temperature, DateTime Timestamp);
}
