# 03. `SemaphoreSlim` та `ReaderWriterLockSlim`

## 1. Коли звичайного `lock` недостатньо?
- Усередині класичного блоку `lock (...)` **заборонено використовувати `await`**, оскільки продовження після `await` може відновитися на іншому потоці, тоді як `Monitor` прив'язаний до конкретного потоку ОС.
- Для асинхронної синхронізації або обмеження кількості одночасних запитів до зовнішнього API чи БД використовується **`SemaphoreSlim`**.
- Коли дані читаються 99% часу і змінюються лише 1% часу, ексклюзивний `lock` штучно гальмує читачів. **`ReaderWriterLockSlim`** дозволяє будь-якій кількості потоків читати паралельно (`EnterReadLock`), блокуючи їх лише на мить оновлення (`EnterWriteLock`).

---

## 2. Властивості та методи API

| Клас / Член API | Опис | Приклад у коді | Документація MSDN |
|:---|:---|:---|:---|
| `SemaphoreSlim.CurrentCount` | Кількість вільних слотів семафора в поточний момент | [Program.cs](../03-SemaphoreSlimAndReaderWriterLock/Program.cs#L70) | [SemaphoreSlim.CurrentCount](https://learn.microsoft.com/en-us/dotnet/api/system.threading.semaphoreslim.currentcount) |
| `SemaphoreSlim.WaitAsync()` | Асинхронно очікує на звільнення слота семафора | [Program.cs](../03-SemaphoreSlimAndReaderWriterLock/Program.cs#L67) | [SemaphoreSlim.WaitAsync](https://learn.microsoft.com/en-us/dotnet/api/system.threading.semaphoreslim.waitasync) |
| `SemaphoreSlim.Release()` | Повертає слот у семафор, пробуджуючи наступне завдання в черзі | [Program.cs](../03-SemaphoreSlimAndReaderWriterLock/Program.cs#L75) | [SemaphoreSlim.Release](https://learn.microsoft.com/en-us/dotnet/api/system.threading.semaphoreslim.release) |
| `ReaderWriterLockSlim.EnterReadLock()` | Дозволяє спільний вхід для паралельного читання | [Program.cs](../03-SemaphoreSlimAndReaderWriterLock/Program.cs#L22) | [EnterReadLock](https://learn.microsoft.com/en-us/dotnet/api/system.threading.readerwriterlockslim.enterreadlock) |
| `ReaderWriterLockSlim.EnterWriteLock()` | Захоплює ексклюзивне блокування для модифікації стану | [Program.cs](../03-SemaphoreSlimAndReaderWriterLock/Program.cs#L35) | [EnterWriteLock](https://learn.microsoft.com/en-us/dotnet/api/system.threading.readerwriterlockslim.enterwritelock) | 