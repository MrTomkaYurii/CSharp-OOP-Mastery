# Розділ 3: Асинхронна Синхронізація (SemaphoreSlim) та Розширене Скасування

## 1. Чому `lock` не працює з `await`?
Класична конструкція C# `lock (syncRoot)` блокує конкретний потік операційної системи за допомогою `Monitor.Enter`.
Якщо всередині критичної секції викликати `await`, метод відпустить поточний потік, а після завершення асинхронної операції виконання може відновитися вже в **іншому** потоці з пулу. Це викликає помилку компілятора:
`CS1996: Cannot await in the body of a lock statement`.

### Вирішення: `SemaphoreSlim`
`SemaphoreSlim` — це єдиний стандартний примітив синхронізації в .NET, що підтримує асинхронне очікування через `await WaitAsync()`:

```csharp
private readonly SemaphoreSlim _semaphore = new(initialCount: 1, maxCount: 1);

public async Task ExecuteProtectedOperationAsync()
{
    await _semaphore.WaitAsync(); // Асинхронне очікування без блокування потоку
    try
    {
        await DoWorkAsync();
    }
    finally
    {
        _semaphore.Release(); // Обов'язкове звільнення у блоці finally
    }
}
```

---

## 2. Rate Limiting (Обмеження частоти запитів)
Встановивши `initialCount` більше одиниці, можна обмежити максимальну кількість одночасних звернень до зовнішнього шлюзу (наприклад, не більше 5 одночасних запитів):
```csharp
var rateLimiter = new SemaphoreSlim(initialCount: 5, maxCount: 5);
```

---

## 3. Розширене використання `CancellationTokenSource`

### А. Автоматичне скасування за таймаутом
```csharp
// Токен автоматично скасується через 5 секунд
using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
// або
cts.CancelAfter(TimeSpan.FromSeconds(5));
```

### Б. Пов'язані токени (Linked Tokens)
Часто операція повинна зупинитися, якщо трапилася **будь-яка** з кількох подій:
1. Користувач натиснув кнопку "Скасувати".
2. Минув локальний таймаут запиту.
3. Зупиняється весь веб-хост (`IHostApplicationLifetime.ApplicationStopping`).

Для цього створюється зв'язка:
```csharp
using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
    userToken, 
    timeoutCts.Token, 
    hostLifetimeToken);

await httpClient.GetAsync(url, linkedCts.Token);
```

---

## 4. Неблокуюча атомарна синхронізація (Lock-Free Interlocked)

Коли необхідно оновити лічильник, прапорець або посилання між десятками паралельних потоків, використання будь-яких локів є надлишковим.
Клас `Interlocked` використовує апаратні атомарні інструкції процесора (CPU atomic instructions):

```csharp
long totalProcessed = 0;

// Атомарний інкремент (потокобезпечно без lock):
Interlocked.Increment(ref totalProcessed);

// Атомарне додавання:
Interlocked.Add(ref totalBytes, 1024);

// Compare-And-Swap (CAS): замінити значення 0 на 1 лише якщо воно дорівнювало 0
long original = Interlocked.CompareExchange(ref status, 1, 0);
```
Операції `Interlocked` у десятки разів швидші за будь-які м'ютекси або семафори.
