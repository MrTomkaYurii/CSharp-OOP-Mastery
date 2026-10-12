# Розділ 1: Паралелізм у .NET: Parallel.ForEachAsync та PLINQ

## 1. I/O-Bound Concurrency vs CPU-Bound Parallelism

У сучасній багатопоточній розробці критично розрізняти два типи навантаження:

| Характеристика | I/O-Bound операції (Мережа, Диск, БД) | CPU-Bound операції (Обчислення, Криптографія) |
| :--- | :--- | :--- |
| **Ресурсне обмеження** | Очікування відповіді зовнішнього сервісу або контролера диску. Потік процесора простоює. | Постійна 100% завантаженість ядер центрального процесора. |
| **Оптимальний інструмент** | `async` / `await`, `Task.WhenAll`, `Parallel.ForEachAsync`. | `Parallel.For`, `Parallel.ForEach`, `PLINQ` (`AsParallel()`), `Task.Run`. |
| **Кількість задач** | Може бути сотні й тисячі одночасно (не споживаючи потоки ОС). | Обмежена кількістю фізичних/логічних ядер (`Environment.ProcessorCount`). |

---

## 2. Асинхронний паралелізм через `Parallel.ForEachAsync` (.NET 6+)

До появи .NET 6 для асинхронної обробки списку використовували конструкцію `Task.WhenAll(items.Select(ProcessAsync))`.
**Проблема `Task.WhenAll`:** якщо в списку 10 000 елементів, вона запустить 10 000 запитів одночасно, що гарантовано призведе до вичерпання пулу сокетів, перевантаження зовнішнього API або винятку `OutOfMemoryException`.

### Вирішення: `Parallel.ForEachAsync`
Метод дозволяє контролювати максимальну кількість одночасних асинхронних операцій (**Throttle / Throttling**):

```csharp
var options = new ParallelOptions
{
    MaxDegreeOfParallelism = 4, // Одночасно виконується не більше 4-х асинхронних операцій
    CancellationToken = ct
};

await Parallel.ForEachAsync(urls, options, async (url, token) =>
{
    var response = await httpClient.GetStringAsync(url, token);
    await ProcessDataAsync(response, token);
});
```

---

## 3. Паралельний LINQ (PLINQ)

PLINQ автоматично розділяє колекцію вхідних даних на сегменти (partioning), розподіляє їх між ядрами процесора і збирає результат в єдиний потік:

```csharp
var heavyResults = sourceData.AsParallel()
    .WithDegreeOfParallelism(Environment.ProcessorCount)
    .Where(ComputeHeavyFilter)
    .Select(TransformData)
    .ToList();
```

### Збереження черговості через `.AsOrdered()`
Оскільки паралельні потоки завершують обробку своїх сегментів у довільному темпі, вихідний порядок елементів втрачається. Якщо збереження початкового порядку обов'язкове:
```csharp
var ordered = sourceData.AsParallel()
    .AsOrdered() // Гарантує відповідність порядку виходу до порядку входу
    .Select(Compute)
    .ToList();
```
*Примітка:* Використання `.AsOrdered()` має незначний оверхед на буферизацію та сортування результатів.
