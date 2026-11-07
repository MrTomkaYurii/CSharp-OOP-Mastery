# 04. Потокобезпечні колекції (`System.Collections.Concurrent`)

Стандартні колекції (`List<T>`, `Dictionary<TKey, TValue>`, `Queue<T>`) не є потокобезпечними: одночасний запис із двох потоків може пошкодити внутрішній масив бакетів і викликати `IndexOutOfRangeException` або нескінченний цикл.

Простір імен `System.Collections.Concurrent` надає високопродуктивні структури даних, які використовують дрібнозернисте блокування (Fine-Grained Locking на рівні окремих кошиків) або Lock-Free алгоритми (`CAS`).

---

## Основні методи API

| Клас / Метод | Опис | Приклад у коді | Документація MSDN |
|:---|:---|:---|:---|
| `ConcurrentDictionary.AddOrUpdate` | Атомарно додає ключ або оновлює існуюче значення через фабрику | [Program.cs](../04-ConcurrentCollectionsDemo/Program.cs#L18) | [ConcurrentDictionary.AddOrUpdate](https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentdictionary-2.addorupdate) |
| `ConcurrentQueue.Enqueue(T)` | Безпечно додає елемент у кінець черги FIFO без блокування | [Program.cs](../04-ConcurrentCollectionsDemo/Program.cs#L58) | [ConcurrentQueue.Enqueue](https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentqueue-1.enqueue) |
| `ConcurrentQueue.TryDequeue(out T)` | Безпечно вилучає перший елемент із черги, якщо вона не порожня | [Program.cs](../04-ConcurrentCollectionsDemo/Program.cs#L62) | [ConcurrentQueue.TryDequeue](https://learn.microsoft.com/en-us/dotnet/api/system.collections.concurrent.concurrentqueue-1.trydequeue) | 