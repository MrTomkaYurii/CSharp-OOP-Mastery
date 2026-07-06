# Модуль 09: Обробка виключень у C# (.NET 10)

Цей модуль присвячений поглибленому вивченню моделі обробки виключень у платформі .NET: від базових блоків `try-catch-finally` до архітектури доменних виключень, збереження стеку викликів при перекиданні та уникнення типових антипатернів продуктивності.

---

## 🗺️ Карта підручника (Навчальні гайди)

1. **[01. Основи обробки виключень: try, catch, finally та порядок фільтрації](Docs/01_Exception_Handling_Fundamentals_Try_Catch_Finally.md)**
   - Розгортання стеку (Stack Unwinding), порядок перехоплення від специфічних типів до базового `Exception`, гарантії виконання блоку `finally`.
2. **[02. Повторне викидання (Rethrowing) та вкладені виключення (InnerException)](Docs/02_Rethrowing_And_Inner_Exceptions.md)**
   - Критична різниця між `throw;` та `throw ex;`, інкапсуляція технічних збоїв у доменні бізнес-помилки через `InnerException`.
3. **[03. Користувацькі виключення (Custom Exceptions) та Throw Expressions](Docs/03_Custom_Exceptions_And_Best_Practices.md)**
   - Три стандартні конструктори за угодами .NET, throw-вирази у тернарних операторах та перевірках на `null`.
4. **[04. Фільтри виключень (when) та проблема продуктивності (Performance Abuse)](Docs/04_Exception_Filters_And_Performance_Abuse.md)**
   - Обчислення фільтрів до розгортання стеку, висока вартість виключень, патерни `Tester-Doer` та `Try-Parse`.

---

## 📂 Структура проектів модуля

| Проект | Опис теми | Ключові концепції |
| :--- | :--- | :--- |
| **[p660[SOLIS]](file:///03-Csharp-Mechanics/05-Exceptions-Handling/p660[SOLIS]/Program.cs)** | Необроблене виключення | Аварійне завершення процесу при діленні на нуль |
| **[p662[SOLIS]](file:///03-Csharp-Mechanics/05-Exceptions-Handling/p662[SOLIS]/Program.cs)** | Базовий перехоплювач | Блок `catch` без вказання типу виключення |
| **[p666[SOLIS]](file:///03-Csharp-Mechanics/05-Exceptions-Handling/p666[SOLIS]/Program.cs)** | Типізоване перехоплення | Доступ до властивостей `Message` та `StackTrace` |
| **[example-Multiple-Catch-Blocks](file:///03-Csharp-Mechanics/05-Exceptions-Handling/example-Multiple-Catch-Blocks/Program.cs)** | Множинні блоки `catch` | Ієрархія фільтрації від часткового до загального |
| **[p669[SOLIS]](file:///03-Csharp-Mechanics/05-Exceptions-Handling/p669[SOLIS]/Program.cs)** | Гарантії `finally` | Спрацьовування `finally` навіть при явному `return` |
| **[p676[SOLIS]](file:///03-Csharp-Mechanics/05-Exceptions-Handling/p676[SOLIS]/Program.cs)** | Генерація виключення | Оператор `throw new ArgumentNullException()` |
| **[p677[SOLIS]](file:///03-Csharp-Mechanics/05-Exceptions-Handling/p677[SOLIS]/Program.cs)** | Вкладена обробка | Локальне логування та прокидання далі через `throw` |
| **[Example-Inner-Exception](file:///03-Csharp-Mechanics/05-Exceptions-Handling/Example-Inner-Exception/Program.cs)** | Властивість `InnerException` | Ланцюжок причин виникнення збою (Root Cause) |
| **[example-user-defined-exception](file:///03-Csharp-Mechanics/05-Exceptions-Handling/example-user-defined-exception/Program.cs)** | Власний клас помилки | Реалізація `TempIsZeroException` |
| **[p679[SOLIS]](file:///03-Csharp-Mechanics/05-Exceptions-Handling/p679[SOLIS]/Program.cs)** | `throw` як вираз | Використання `throw` всередині тернарного оператора |
| **[Exception-Handling-Abuse](file:///03-Csharp-Mechanics/05-Exceptions-Handling/Exception-Handling-Abuse/Program.cs)** | Антипатерн потоку керування | Порівняння `int.Parse` + `catch` проти `int.TryParse` |

---

## 🛠️ Перевірка збірки (Zero-Error Verification)

Усі 11 проектів модуля оновлені до **.NET 10** та компілюються через централізоване рішення:
```powershell
dotnet build "03-Csharp-Mechanics/05-Exceptions-Handling/OOP - Exception.sln" -c Release
```
