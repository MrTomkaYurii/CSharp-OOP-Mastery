# Модуль: Advanced Concurrency, Parallelism & Channels у .NET 10

Цей модуль присвячений поглибленому вивченню асинхронного та паралельного програмування у сучасному .NET: контрольованому асинхронному паралелізму (`Parallel.ForEachAsync`), обчисленням з `PLINQ`, потоковій передачі даних через `System.Threading.Channels`, рейт-лімітингу з `SemaphoreSlim` та розширеному скасуванню через зв'язані `CancellationTokenSource`.

---

## Структура проєктів

```
05-Parallel-And-Channels/
├── ParallelAndChannels.slnx                        # Файл рішення .NET 10
├── Concurrency.ParallelAndPLINQ/                   # Асинхронний паралелізм та PLINQ
│   └── Program.cs                                  # Parallel.ForEachAsync, PLINQ AsOrdered, Interlocked
├── Concurrency.Channels.ProducerConsumer/          # Високопродуктивні черги Channels
│   └── Program.cs                                  # BoundedChannel, Backpressure, IAsyncEnumerable
├── Concurrency.SynchronizationAndCancellation/     # Синхронізація та скасування
│   └── Program.cs                                  # SemaphoreSlim, Linked CancellationTokenSource, Lock-Free
└── Docs/                                           # Посібники українською
    ├── 01_Parallel_ForEachAsync_And_PLINQ.md
    ├── 02_Channels_Producer_Consumer.md
    └── 03_Synchronization_And_Cancellation.md
```

---

## Навчальні матеріали
1. [Розділ 1: Parallel.ForEachAsync та PLINQ](file:///c:/Users/Yurii/source/repos/CSharp-OOP-Mastery/06-Advanced-DotNet/05-Parallel-And-Channels/Docs/01_Parallel_ForEachAsync_And_PLINQ.md)
2. [Розділ 2: Черги Channels та Producer-Consumer патерн](file:///c:/Users/Yurii/source/repos/CSharp-OOP-Mastery/06-Advanced-DotNet/05-Parallel-And-Channels/Docs/02_Channels_Producer_Consumer.md)
3. [Розділ 3: Асинхронна синхронізація, SemaphoreSlim та Linked Tokens](file:///c:/Users/Yurii/source/repos/CSharp-OOP-Mastery/06-Advanced-DotNet/05-Parallel-And-Channels/Docs/03_Synchronization_And_Cancellation.md)

---

## Як запустити проєкти

```powershell
# 1. Запуск Parallel.ForEachAsync та PLINQ:
dotnet run --project Concurrency.ParallelAndPLINQ/Concurrency.ParallelAndPLINQ.csproj

# 2. Запуск черг Channels (Producer-Consumer):
dotnet run --project Concurrency.Channels.ProducerConsumer/Concurrency.Channels.ProducerConsumer.csproj

# 3. Запуск SemaphoreSlim та розширеного скасування:
dotnet run --project Concurrency.SynchronizationAndCancellation/Concurrency.SynchronizationAndCancellation.csproj
```
