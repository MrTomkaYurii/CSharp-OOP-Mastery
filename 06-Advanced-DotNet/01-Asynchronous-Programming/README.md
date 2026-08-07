# Модуль 14: Асинхронне програмування в C# (.NET 10)

Цей модуль присвячений поглибленому вивченню асинхронності в сучасній екосистемі .NET: від базових ключових слів `async` / `await` та кінцевого автомата задач до координації паралельних потоків даних (`Task.WhenAll`), троттлінгу запитів через `SemaphoreSlim` та кооперативного скасування операцій (`CancellationToken`).

---

## 🗺️ Карта підручника (Навчальні гайди)

1. **[01. Архітектура Async / Await та кінцевий автомат Task](Docs/01_Async_Await_Architecture_And_State_Machine.md)**
   - I/O-Bound проти CPU-Bound, звільнення потоків, внутрішня структура `IAsyncStateMachine`, небезпека `async void`.
2. **[02. Координація завдань, комбінатори Task.WhenAll та троттлінг через SemaphoreSlim](Docs/02_Task_Coordination_And_Combinators.md)**
   - Паралельне очікування завдань `Task.WhenAll` / `Task.WhenAny`, запобігання вичерпанню сокетів за допомогою `SemaphoreSlim`.
3. **[03. Кооперативне скасування операцій: CancellationToken та CancellationTokenSource](Docs/03_Cancellation_Tokens_And_Cooperative_Cancellation.md)**
   - Двостороння модель скасування, безпечне завершення без витоків ресурсів, автоматичні таймаути.
4. **[04. Обробка виключень в асинхронному коді: Розгортання AggregateException](Docs/04_Exception_Handling_In_Async_Code.md)**
   - Як `await` розгортає перше виключення, отримання повного списку помилок з `allTasks.Exception.InnerExceptions`.

---

## 📂 Структура проектів модуля (16 проектів)

| Проект | Опис теми | Ключові концепції |
| :--- | :--- | :--- |
| **[01-AsyncAndAwait](file:///06-Advanced-DotNet/01-Asynchronous-Programming/01-AsyncAndAwait/Program.cs)** | Перший крок у TAP | Базовий виклик `async Task` |
| **[02-Tasks](file:///06-Advanced-DotNet/01-Asynchronous-Programming/02-Tasks/Program.cs)** | Створення задач | Використання `Task.Run` для фонової роботи |
| **[03-ReturnAValueFromTask-API](file:///06-Advanced-DotNet/01-Asynchronous-Programming/03-ReturnAValueFromTask-API/Controllers/GreetingsController.cs)** | Web API контролер | Асинхронний контролер ASP.NET Core `Task<ActionResult>` |
| **[03-ReturnAValueFromTask-Console](file:///06-Advanced-DotNet/01-Asynchronous-Programming/03-ReturnAValueFromTask-Console/Program.cs)** | Повернення значень | Використання `Task<string>` та очікування результату |
| **[03-ReturnAValueFromTask-ConsolewithERROR](file:///06-Advanced-DotNet/01-Asynchronous-Programming/03-ReturnAValueFromTask-ConsolewithERROR/Program.cs)** | Асинхронні помилки | Аналіз виключень всередині `Task<T>` |
| **[03-ReturnComplexTypeValueFromАTask](file:///06-Advanced-DotNet/01-Asynchronous-Programming/03-ReturnComplexTypeValueFromАTask/Program.cs)** | Складні сутності | Повернення об'єктів користувацьких класів |
| **[04-ExecuteMultipleTasks](file:///06-Advanced-DotNet/01-Asynchronous-Programming/04-ExecuteMultipleTasks/Program.cs)** | Множинні задачі | Розвантаження поточного потоку (Offloading) |
| **[04-ExecuteMultipleTasks-Task.WhenAll](file:///06-Advanced-DotNet/01-Asynchronous-Programming/04-ExecuteMultipleTasks-Task.WhenAll/Program.cs)** | Комбінатор WhenAll | Синхронізація групи паралельних задач |
| **[05-LimitTheNumberOfConcurrentTasks](file:///06-Advanced-DotNet/01-Asynchronous-Programming/05-LimitTheNumberOfConcurrentTasks/Program.cs)** | Троттлінг пулу | Обмеження паралелізму через `SemaphoreSlim` |
| **[06-HandleResponseWhenExecutingMultipleTasks](file:///06-Advanced-DotNet/01-Asynchronous-Programming/06-HandleResponseWhenExecutingMultipleTasks/Program.cs)** | Обробка відповідей | Почерговий збір відповідей від множини задач |
| **[CancellationTokenToCancelATask](file:///06-Advanced-DotNet/01-Asynchronous-Programming/CancellationTokenToCancelATask/Program.cs)** | Скасування задач | Використання `CancellationTokenSource` |
| **[EX_01_async_await](file:///06-Advanced-DotNet/01-Asynchronous-Programming/EX_01_async_await/Program.cs)** | Поведінковий експеримент 1 | Синхронний vs асинхронний потік виконання |
| **[EX_02_async_await](file:///06-Advanced-DotNet/01-Asynchronous-Programming/EX_02_async_await/Program.cs)** | Асинхронний сніданок | Класична метафора паралельної підготовки страв |
| **[EX_03_async_await](file:///06-Advanced-DotNet/01-Asynchronous-Programming/EX_03_async_await/Program.cs)** | Оптимізація сніданку | Одночасне приготування яєчні, бекону, тостів та кави |
| **[EX_04_async_await](file:///06-Advanced-DotNet/01-Asynchronous-Programming/EX_04_async_await/Program.cs)** | Об'єднання задач | Компонування операцій через `Task.WhenAll` |
| **[EX_05-exceptions](file:///06-Advanced-DotNet/01-Asynchronous-Programming/EX_05-exceptions/Program.cs)** | Виключення у WhenAll | Перехоплення всіх виключень групи задач |

---

## 🛠️ Перевірка збірки (Zero-Error Verification)

Усі 16 проектів модуля оновлені до **.NET 10** та компілюються через центральне рішення:
```powershell
dotnet build "06-Advanced-DotNet/01-Asynchronous-Programming/OOP - Asynchronous Programming.sln" -c Release
```
