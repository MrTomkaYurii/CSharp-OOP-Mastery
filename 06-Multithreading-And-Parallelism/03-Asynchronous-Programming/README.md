# Модуль 14: Асинхронне програмування в C# (.NET 10)

Цей модуль присвячений поглибленому вивченню асинхронності в сучасній екосистемі .NET: від базових ключових слів `async` / `await` та кінцевого автомата задач до координації паралельних потоків даних (`Task.WhenAll`), троттлінгу запитів через `SemaphoreSlim` та кооперативного скасування операцій (`CancellationToken`).

Кожен із 16 проектів модуля забезпечений власним детальним навчальним посібником (`README.md`) з візуальними Mermaid-діаграмами послідовності та розбором архітектурних концепцій.

---

## Карта підручника (Центральні гайди у Docs/)

1. **[01. Архітектура Async / Await та кінцевий автомат Task](Docs/01_Async_Await_Architecture_And_State_Machine.md)**
   - I/O-Bound проти CPU-Bound, звільнення потоків, внутрішня структура `IAsyncStateMachine`, небезпека `async void`.
2. **[02. Координація завдань, комбінатори Task.WhenAll та троттлінг через SemaphoreSlim](Docs/02_Task_Coordination_And_Combinators.md)**
   - Паралельне очікування завдань `Task.WhenAll` / `Task.WhenAny`, запобігання вичерпанню сокетів за допомогою `SemaphoreSlim`.
3. **[03. Кооперативне скасування операцій: CancellationToken та CancellationTokenSource](Docs/03_Cancellation_Tokens_And_Cooperative_Cancellation.md)**
   - Двостороння модель скасування, безпечне завершення без витоків ресурсів, автоматичні таймаути.
4. **[04. Обробка виключень в асинхронному коді: Розгортання AggregateException](Docs/04_Exception_Handling_In_Async_Code.md)**
   - Як `await` розгортає перше виключення, отримання повного списку помилок з `allTasks.Exception.InnerExceptions`.

---

## Структура проектів модуля (16 проектів з інтегрованими README)

| Проект | Документація проекту | Ключові концепції та візуалізації |
| :--- | :--- | :--- |
| **01-AsyncAndAwait** | [ README проекту](01-AsyncAndAwait/Program.cs) | Синхронне блокування потоку (`Thread.Sleep`), Thread Pool Starvation. |
| **02-Tasks** | [ README проекту](02-Tasks/Program.cs) | Неблокуюче очікування (`Task.Delay`), життєвий цикл таймера, пастка `async void`. |
| **03-ReturnAValueFromTask-API** | [ README проекту](03-ReturnAValueFromTask-FileProcessing/Program.cs) | Асинхронний Web API контролер ASP.NET Core, обробка Kestrel. |
| **03-ReturnAValueFromTask-Console** | [ README проекту](03-ReturnAValueFromTask-Console/Program.cs) | Отримання результату `Task<T>`, `HttpClient` запити, розгортання `await`. |
| **03-ReturnAValueFromTask-ConsolewithERROR** | [ README проекту](03-ReturnAValueFromTask-ConsolewithERROR/Program.cs) | Асинхронні помилки I/O, `EnsureSuccessStatusCode`, `HttpRequestException`. |
| **03-ReturnComplexTypeValueFromАTask** | [ README проекту](03-ReturnComplexTypeValueFromАTask/Program.cs) | Повернення складних сутностей `Task<Employee>`, типізація та DTO. |
| **04-ExecuteMultipleTasks** | [ README проекту](04-ExecuteMultipleTasks/Program.cs) | Розвантаження потоку (Offloading через `Task.Run`), 100k операцій. |
| **04-ExecuteMultipleTasks-Task.WhenAll** | [ README проекту](04-ExecuteMultipleTasks-Task.WhenAll/Program.cs) | Комбінатор `Task.WhenAll`: послідовний await (10с) vs паралельний (1с). |
| **05-LimitTheNumberOfConcurrentTasks** | [ README проекту](05-LimitTheNumberOfConcurrentTasks/Program.cs) | Троттлінг через `SemaphoreSlim(3)`, захист сокетів і бази даних. |
| **06-HandleResponseWhenExecutingMultipleTasks** | [ README проекту](06-HandleResponseWhenExecutingMultipleTasks/Program.cs) | Агрегація відповідей, десеріалізація JSON у DTO, LINQ-фільтрація. |
| **CancellationTokenToCancelATask** | [ README проекту](CancellationTokenToCancelATask/Program.cs) | Кооперативне скасування, `CancellationTokenSource`, таймаут 5с. |
| **EX_01_async_await** | [ README проекту](EX_01_async_await/Program.cs) | Інтерактивна лабораторія: CPU vs I/O bound, конкурентний старт. |
| **EX_02_async_await** | [ README проекту](EX_02_async_await/Program.cs) | Хрестоматійний «Асинхронний сніданок», черга подій `Task.WhenAny`. |
| **EX_03_async_await** | [ README проекту](EX_03_async_await/Program.cs) | Типи повернення: `void` (події), `Task`, `Task<T>`, `ValueTask<T>`. |
| **EX_04_async_await** | [ README проекту](EX_04_async_await/Program.cs) | Порівняння `WhenAll` vs `WhenAny`, гонка задач, доступ до `.Result`. |
| **EX_05-exceptions** | [ README проекту](EX_05-exceptions/Program.cs) | Повна анатомія винятків, `AggregateException`, `IsFaulted`, `InnerExceptions`. |

---

## Перевірка збірки (Zero-Error Verification)

Усі 16 проектів модуля націлені на **.NET 10** та компілюються через центральне рішення:
```powershell
dotnet build "06-Advanced-DotNet/01-Asynchronous-Programming/OOP - Asynchronous Programming.sln" -c Release
```
- **Errors**: `0`
- **Warnings**: `34` (лише стиль найменування просторів імен у проектах-вправах)
