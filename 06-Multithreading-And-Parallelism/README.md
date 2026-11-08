# Глава 06: Багатопотокове, Асинхронне та Паралельне програмування в .NET 10

Ця глава об'єднує всі рівні конкурентного виконання коду на платформі .NET — від низькорівневих потоків ОС (`Thread`) та примітивів синхронізації (`Lock`, `Monitor`, `SemaphoreSlim`) до кінцевих автоматів `async/await`, паралельної обробки даних (`Parallel.ForEachAsync`, `PLINQ`) і високопродуктивних черг `System.Threading.Channels`.

---

## Логічна послідовність підмодулів

| № | Підмодуль | Фокус вивчення | Рішення Visual Studio |
|:--:|:---|:---|:---|
| **01** | **[01-Multithreading-Basics](01-Multithreading-Basics/)** | Клас `Thread`, `Foreground` vs `Background`, `ThreadPool`, стан гонитви (`Race Condition`), атомарний `Interlocked`, `ThreadLocal<T>` та `AsyncLocal<T>` | `OOP - Multithreading.sln` |
| **02** | **[02-Synchronization-And-ThreadSafety](02-Synchronization-And-ThreadSafety/)** | Критичні секції `lock`, `Monitor.TryEnter`, новий `.NET 9/10 System.Threading.Lock`, анатомія `Deadlock` (Lock Ordering), `SemaphoreSlim`, `ReaderWriterLockSlim`, `ConcurrentDictionary` | `OOP - Synchronization.sln` |
| **03** | **[03-Asynchronous-Programming](03-Asynchronous-Programming/)** | Механіка `async`/`await`, `Task<T>`, `ValueTask<T>`, комбінатори `Task.WhenAll`/`WhenAny`, кооперативне скасування `CancellationToken`, розгортання `AggregateException` | `OOP - Asynchronous Programming.sln` |
| **04** | **[04-Parallel-And-Channels](04-Parallel-And-Channels/)** | Контрольований паралелізм `Parallel.ForEachAsync`, `PLINQ` (`AsParallel`, `AsOrdered`), потокова передача даних `System.Threading.Channels` (Producer-Consumer) | `OOP - Parallel and Channels.sln` |
