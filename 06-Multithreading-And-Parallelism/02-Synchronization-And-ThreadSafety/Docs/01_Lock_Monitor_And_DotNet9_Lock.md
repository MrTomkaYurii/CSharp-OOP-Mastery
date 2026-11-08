# 01. Критичні секції: `lock`, `Monitor` та `.NET 10 System.Threading.Lock`

## 1. Як влаштований `lock` під капотом CLR

Конструкція `lock (syncObject) { ... }` є синтаксичним цукром над статичним класом `System.Threading.Monitor`:

```csharp
bool lockTaken = false;
try
{
    Monitor.Enter(syncObject, ref lockTaken);
    // Критична секція (лише один потік одночасно)
}
finally
{
    if (lockTaken) Monitor.Exit(syncObject);
}
```

### Заголовок об'єкта та SyncBlockIndex
Кожен посилальний об'єкт у купі .NET має заголовок із двох службових полів:
1. **MethodTable Pointer**: вказівник на таблицю методів типу.
2. **SyncBlockIndex**: 4-байтне поле, яке спочатку зберігає хеш-код об'єкта. При першому виклику `Monitor.Enter` CLR виділяє запис у глобальній таблиці синхроблоків і записує його індекс у це поле.

---

## 2. Новий тип `.NET 9 / .NET 10`: `System.Threading.Lock`

Починаючи з C# 13 та .NET 9+, якщо об'єктом блокування виступає екземпляр нового типу `System.Threading.Lock`, компілятор генерує набагато швидший код через структуру `Lock.Scope`:

```csharp
// Сучасний стандарт (.NET 9 / .NET 10):
private readonly Lock _gate = new();

public void ExecuteCriticalWork()
{
    lock (_gate) // Автоматично використовує ref struct Scope!
    {
        // Відсутній оверхед на SyncBlockIndex у купі!
    }
}
```

---

## 3. Запобігання зависанню через `Monitor.TryEnter`

Якщо потік не може отримати блокування протягом визначеного часу, замість нескінченного очікування слід вийти з таймаутом:

```csharp
if (Monitor.TryEnter(_syncLock, TimeSpan.FromSeconds(2)))
{
    try
    {
        // Робота з ресурсом
    }
    finally
    {
        Monitor.Exit(_syncLock);
    }
}
else
{
    Console.WriteLine("[Warning] Не вдалося отримати блокування за 2 секунди. Пропускаємо.");
}
```

---

## 4. Матриця проєкту цієї секції

| Проєкт | Посилання на вихідний код | Що демонструє під капотом |
|:---|:---|:---|
| [`01-LockAndMonitor`](../01-LockAndMonitorDemo/Program.cs) | [Program.cs](../01-LockAndMonitorDemo/Program.cs) | Порівняння класичного `lock`, класу `Monitor.TryEnter` із таймаутом та нового типу `.NET 10 System.Threading.Lock`. |
