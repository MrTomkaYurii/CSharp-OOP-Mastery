# Модуль 06.1: Основи багатопотоковості (Multithreading Basics) у .NET 10

Цей підмодуль знайомить із фундаментальними механізмами багатопотокового програмування на платформі .NET: від створення фізичних потоків ОС (`System.Threading.Thread`) та роботи системного пулу потоків (`ThreadPool`) до природи стану гонитви (`Race Condition`), апаратної атомарності (`Interlocked`) та ізоляції контексту виконання (`ThreadLocal<T>` і `AsyncLocal<T>`).

---

## Структура проєктів

| Проєкт | Файл коду | Ключові класи та концепції |
|:---|:---|:---|
| **01-ThreadCreationAndLifecycle** | [Program.cs](01-ThreadCreationAndLifecycle/Program.cs) | `Thread`, `ThreadStart`, `IsBackground`, `ThreadState`, `Join()` |
| **02-ThreadPoolAndWorkItems** | [Program.cs](02-ThreadPoolAndWorkItems/Program.cs) | `ThreadPool.QueueUserWorkItem`, повторне використання потоків, `CountdownEvent` |
| **03-RaceConditionAndInterlocked** | [Program.cs](03-RaceConditionAndInterlocked/Program.cs) | Чому `x++` неатомарний, `Interlocked.Increment`, `Interlocked.Add`, `CompareExchange` |
| **04-ThreadLocalAndAsyncLocal** | [Program.cs](04-ThreadLocalAndAsyncLocal/Program.cs) | `ThreadLocal<T>` проти `AsyncLocal<T>`, збереження `CorrelationId` через `await` |

---

## Навчальні посібники (Docs)

1. [01. Потоки ОС (Thread) та Пул потоків (ThreadPool)](Docs/01_Threads_And_ThreadPool.md)
2. [02. Стан гонитви (Race Condition) та атомарні операції Interlocked](Docs/02_Race_Conditions_And_Interlocked.md)
3. [03. Ізоляція стану потоку: ThreadLocal проти AsyncLocal](Docs/03_ThreadLocal_And_AsyncLocal.md)
