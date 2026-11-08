# 01. Критичні секції: `lock`, `Monitor` та новий `System.Threading.Lock` (.NET 9/10)

## 1. Як працює класичний `lock (object)`?
Коли необхідно виконати декілька операцій як єдину неподільну транзакцію (наприклад, перевірити залишок на складі і списати товар), атомарного `Interlocked` уже недостатньо — потрібна **критична секція**.

Конструкція `lock (_syncRoot)` є синтаксичним цукром компілятора C# над класом `System.Threading.Monitor`:

```csharp
bool lockTaken = false;
try
{
    Monitor.Enter(_syncRoot, ref lockTaken);
    // Критична секція: одночасно тут перебуває лише ОДИН потік
}
finally
{
    if (lockTaken)
        Monitor.Exit(_syncRoot);
}
```

---

## 2. Новий тип `System.Threading.Lock` у .NET 9 та .NET 10
Починаючи з .NET 9, у BCL з'явився спеціалізований тип `System.Threading.Lock`. Якщо змінна оголошена як `private readonly Lock _lock = new();`, конструкція `lock (_lock)` автоматично використовує новий швидкий механізм `Lock.EnterScope()` замість застарілого заголовка об'єкта `SyncBlock`.

---

## 3. Методи класів `Monitor` та `Lock`

| Метод | Опис | Приклад у коді | Документація MSDN |
|:---|:---|:---|:---|
| `Monitor.Enter(object, ref bool)` | Захоплює ексклюзивне блокування на об'єкті | [Program.cs](../01-LockAndMonitorDemo/Program.cs#L28) | [Monitor.Enter](https://learn.microsoft.com/en-us/dotnet/api/system.threading.monitor.enter) |
| `Monitor.TryEnter(object, TimeSpan, ref bool)` | Намагається увійти в критичну секцію протягом заданого таймауту | [Program.cs](../01-LockAndMonitorDemo/Program.cs#L56) | [Monitor.TryEnter](https://learn.microsoft.com/en-us/dotnet/api/system.threading.monitor.tryenter) |
| `Monitor.Exit(object)` | Звільняє блокування об'єкта у блоці `finally` | [Program.cs](../01-LockAndMonitorDemo/Program.cs#L69) | [Monitor.Exit](https://learn.microsoft.com/en-us/dotnet/api/system.threading.monitor.exit) |
| `Lock.EnterScope()` | Повертає `ref struct Scope`, що автоматично знімає блокування при виході з `lock` | [Program.cs](../01-LockAndMonitorDemo/Program.cs#L43) | [Lock.EnterScope](https://learn.microsoft.com/en-us/dotnet/api/system.threading.lock.enterscope) |
