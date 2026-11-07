# Модуль 06.2: Синхронізація та Потокобезпека (Synchronization & Thread Safety) у .NET 10

Цей підмодуль охоплює механізми захисту спільних ресурсів та координації потоків у C#: від критичних секцій (`lock`, `Monitor`, новий `System.Threading.Lock`) та запобігання взаємним блокуванням (`Deadlock`) до гнучких примітивів (`SemaphoreSlim`, `ReaderWriterLockSlim`) та потокобезпечних колекцій (`ConcurrentDictionary`, `ConcurrentQueue`).

---

## Структура проєктів

| Проєкт | Файл коду | Ключові класи та концепції |
|:---|:---|:---|
| **01-LockAndMonitorDemo** | [Program.cs](01-LockAndMonitorDemo/Program.cs) | `lock`, `Monitor.TryEnter`, сучасний тип `System.Threading.Lock` (.NET 9/10) |
| **02-DeadlockAnatomyAndPrevention** | [Program.cs](02-DeadlockAnatomyAndPrevention/Program.cs) | Природа Deadlock (AB-BA), впорядкування блокувань (Lock Ordering) |
| **03-SemaphoreSlimAndReaderWriterLock** | [Program.cs](03-SemaphoreSlimAndReaderWriterLock/Program.cs) | `SemaphoreSlim.WaitAsync`, `ReaderWriterLockSlim` (`EnterReadLock` / `EnterWriteLock`) |
| **04-ConcurrentCollectionsDemo** | [Program.cs](04-ConcurrentCollectionsDemo/Program.cs) | `ConcurrentDictionary.AddOrUpdate`, `ConcurrentQueue.TryDequeue` |

---

## Навчальні посібники (Docs)

1. [01. Критичні секції: lock, Monitor та новий System.Threading.Lock](Docs/01_Lock_Monitor_And_DotNet9_Lock.md)
2. [02. Анатомія Deadlock та способи його уникнення](Docs/02_Deadlocks_And_Prevention.md)
3. [03. SemaphoreSlim та ReaderWriterLockSlim](Docs/03_SemaphoreSlim_And_ReaderWriterLock.md)
4. [04. Потокобезпечні колекції System.Collections.Concurrent](Docs/04_Concurrent_Collections.md)

