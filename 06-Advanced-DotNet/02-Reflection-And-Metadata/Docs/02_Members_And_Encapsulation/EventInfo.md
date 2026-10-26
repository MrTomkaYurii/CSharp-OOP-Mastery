# Клас System.Reflection.EventInfo: Інспекція та підписка на події

> **Простір імен**: `System.Reflection`  
> **Збірка**: `System.Private.CoreLib.dll`  
> **Базовий тип**: `System.Reflection.MemberInfo`  
> **Офіційна документація**: [EventInfo Class (Microsoft Learn)](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.eventinfo)

Клас `EventInfo` представляє метадані події програми. У середовищі CLR подія реалізується парою методів для підписки (`add_EventName`) та відписки (`remove_EventName`) екземплярів делегатів. Він успадковує `MemberInfo` та надає засоби динамічної підписки під час виконання.

---

## 1. Динамічна підписка делегата через рефлексію

```csharp
EventInfo? eventInfo = typeof(Car).GetEvent("SpeedLimitExceeded");

// Створення сумісного делегата
Action<int> handler = speed => Console.WriteLine($"Перевищено швидкість: {speed} км/год");

// Динамічна реєстрація підписника:
eventInfo?.AddEventHandler(myCar, handler);
```

---

## 2. Властивості класу EventInfo (Properties)

| Властивість | Сигнатура | Опис призначення | Статус у курсі | Приклад у коді | Документація MSDN |
| :--- | :--- | :--- | :---: | :--- | :--- |
| **`EventHandlerType`** | `Type? EventHandlerType { get; }` | Тип делегата, який використовується подією для сповіщення підписників | Використано | [11-GetEventsDemo/Program.cs](../../11-GetEventsDemo/Program.cs#L26) | [EventInfo.EventHandlerType](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.eventinfo.eventhandlertype) |
| **`IsMulticast`** | `bool IsMulticast { get; }` | Визначає, чи є делегат події багатоадресним (`MulticastDelegate`) | Загальний BCL | Довідник | [EventInfo.IsMulticast](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.eventinfo.ismulticast) |

---

## 3. Методи класу EventInfo (Methods)

| Метод | Сигнатура | Опис призначення | Статус у курсі | Приклад у коді | Документація MSDN |
| :--- | :--- | :--- | :---: | :--- | :--- |
| **`AddEventHandler`** | `void AddEventHandler(object? target, Delegate? handler)` | Додає обробник до події на цільовому об'єкті | Використано | [11-GetEventsDemo/Program.cs](../../11-GetEventsDemo/Program.cs#L38) | [EventInfo.AddEventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.eventinfo.addeventhandler) |
| **`RemoveEventHandler`**| `void RemoveEventHandler(object? target, Delegate? handler)` | Відписує обробник від події | Використано | [11-GetEventsDemo/Program.cs](../../11-GetEventsDemo/Program.cs#L52) | [EventInfo.RemoveEventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.eventinfo.removeeventhandler) |
| **`GetAddMethod`** | `MethodInfo? GetAddMethod(bool nonPublic)` | Отримує дескриптор `MethodInfo` для методу підписки `add` | Використано | [11-GetEventsDemo/Program.cs](../../11-GetEventsDemo/Program.cs#L28) | [EventInfo.GetAddMethod](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.eventinfo.getaddmethod) |
| **`GetRemoveMethod`** | `MethodInfo? GetRemoveMethod(bool nonPublic)` | Отримує дескриптор `MethodInfo` для методу відписки `remove` | Використано | [11-GetEventsDemo/Program.cs](../../11-GetEventsDemo/Program.cs#L30) | [EventInfo.GetRemoveMethod](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.eventinfo.getremovemethod) |
