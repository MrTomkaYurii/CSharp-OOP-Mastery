# 03. Ізоляція стану потоку: `ThreadLocal<T>` проти `AsyncLocal<T>`

Найкращий спосіб уникнути проблем синхронізації — **не використовувати спільний мутований стан**. У .NET є два спеціалізовані контейнери для контекстних даних:

## 1. Порівняння `ThreadLocal<T>` та `AsyncLocal<T>`

| Критерій | `ThreadLocal<T>` | `AsyncLocal<T>` |
|:---|:---|:---|
| **До чого прив'язане значення?** | До конкретного фізичного потоку ОС (`ManagedThreadId`) | До логічного контексту виконання (`ExecutionContext`) |
| **Чи зберігається після `await`?** | **Ні** (після `await` продовження може виконатися на іншому потоці пулу) | **Так** (автоматично копіюється вздовж усього дерева `Task` / `await`) |
| **Типовий сценарій використання** | Екземпляри парсерів, буферів пам'яті, криптографічних генераторів на потік | `CorrelationId`, `TraceId`, ідентифікатор поточного користувача або тенанта |

---

## 2. Властивості API

| Клас / Властивість | Опис | Приклад у коді | Документація MSDN |
|:---|:---|:---|:---|
| `ThreadLocal<T>.Value` | Читає або записує ізольоване значення для поточного фізичного потоку | [Program.cs](../04-ThreadLocalAndAsyncLocal/Program.cs#L37) | [ThreadLocal.Value](https://learn.microsoft.com/en-us/dotnet/api/system.threading.threadlocal-1.value) |
| `AsyncLocal<T>.Value` | Читає або записує значення, що передається вниз по дереву асинхронних викликів | [Program.cs](../04-ThreadLocalAndAsyncLocal/Program.cs#L68) | [AsyncLocal.Value](https://learn.microsoft.com/en-us/dotnet/api/system.threading.asynclocal-1.value) | 