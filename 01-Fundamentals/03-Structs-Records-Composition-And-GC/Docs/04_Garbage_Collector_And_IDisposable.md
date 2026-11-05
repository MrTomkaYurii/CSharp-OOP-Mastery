# 04. Життєвий цикл об'єкта: Garbage Collector (`Gen 0/1/2`) та `IDisposable`

## 1. Покоління збирача сміття (Generational GC)
Купа .NET поділена на 3 покоління за віком об'єктів:
- **Generation 0**: усі щойно створені малі об'єкти (`new`). Очищується найчастіше та найшвидше.
- **Generation 1**: буфер для об'єктів, які пережили одне очищення `Gen 0`.
- **Generation 2**: довгоживучі об'єкти (кеші, Singleton-сервіси) та об'єкти з Large Object Heap (`>= 85 000` байт).

## 2. Методи класів `GC` та `IDisposable`

| Клас / Метод | Опис | Приклад у коді | Документація MSDN |
|:---|:---|:---|:---|
| `GC.GetGeneration(object)` | Повертає поточне покоління об'єкта в Купі (`0`, `1` або `2`) | [Program.cs](../04-GarbageCollectorAndIDisposable/Program.cs#L51) | [GC.GetGeneration](https://learn.microsoft.com/en-us/dotnet/api/system.gc.getgeneration) |
| `GC.Collect()` | Примусово запускає збирання сміття (використовується лише для діагностики) | [Program.cs](../04-GarbageCollectorAndIDisposable/Program.cs#L53) | [GC.Collect](https://learn.microsoft.com/en-us/dotnet/api/system.gc.collect) |
| `GC.SuppressFinalize(object)` | Скасовує виклик фіналізатора, оскільки ресурси вже звільнені в `Dispose()` | [Program.cs](../04-GarbageCollectorAndIDisposable/Program.cs#L34) | [GC.SuppressFinalize](https://learn.microsoft.com/en-us/dotnet/api/system.gc.suppressfinalize) |
| `IDisposable.Dispose()` | Детерміновано звільняє зовнішні ресурси (файли, сокети, з'єднання з БД) | [Program.cs](../04-GarbageCollectorAndIDisposable/Program.cs#L25) | [IDisposable.Dispose](https://learn.microsoft.com/en-us/dotnet/api/system.idisposable.dispose) | 