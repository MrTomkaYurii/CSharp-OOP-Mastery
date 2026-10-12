# Розділ 2: Високопродуктивні Черги з System.Threading.Channels

## 1. Архітектура Channels vs BlockingCollection
У старих версіях .NET для патерну "Виробник-Споживач" (Producer-Consumer) використовували `BlockingCollection<T>`.
Однак `BlockingCollection<T>` блокувала потік операційної системи (`Thread.Sleep` / `Monitor.Wait`), що неприпустимо у високонавантажених асинхронних додатках.

Бібліотека **`System.Threading.Channels`** створена спеціально для асинхронного неблокуючого обміну даними між потоками:
- Повністю оптимізована під `async/await` та `ValueTask`.
- Працює практично без виділення пам'яті в купі (Zero Allocation).
- Має чіткий поділ на інтерфейси читання (`ChannelReader<T>`) та запису (`ChannelWriter<T>`).

---

## 2. Bounded vs Unbounded канали та проблема Backpressure

| Тип каналу | Створення | Поведінка при переповненні |
| :--- | :--- | :--- |
| **Unbounded** | `Channel.CreateUnbounded<T>()` | Розмір буфера необмежений. Якщо продюсери генерують дані швидше, ніж споживачі встигають їх обробляти, черга розростається до переповнення оперативної пам'яті (`OutOfMemoryException`). |
| **Bounded** | `Channel.CreateBounded<T>(capacity)` | Буфер має фіксовану ємність. При заповненні буфера спрацьовує механізм **Backpressure** (зворотного тиску). |

### Режими FullMode для Bounded Channel:
```csharp
var options = new BoundedChannelOptions(capacity: 100)
{
    // Wait: Продюсер асинхронно призупиняється (await WriteAsync) до появи вільного місця
    FullMode = BoundedChannelFullMode.Wait,
    
    // Альтернативи: DropOldest, DropNewest, DropWrite
    SingleWriter = false,
    SingleReader = false
};
```

---

## 3. Читання через `IAsyncEnumerable<T>` та `await foreach`

Споживач підписується на потік повідомлень за допомогою сучасної конструкції `ReadAllAsync()`:

```csharp
static async Task ConsumeMessagesAsync(ChannelReader<OrderEvent> reader, CancellationToken ct)
{
    // Цикл автоматично завершиться, коли продюсер викличе writer.Complete()
    await foreach (var order in reader.ReadAllAsync(ct))
    {
        await ProcessOrderAsync(order, ct);
    }
}
```

---

## 4. Завершення передачі даних (`Complete`)

Коли продюсер (або всі продюсери) завершили роботу, обов'язково викликається:
```csharp
channel.Writer.Complete();
```
Це сигналізує всім очікуючим читачам, що нових даних не буде, і дозволяє споживачам коректно вийти з циклу `await foreach` без помилок.
