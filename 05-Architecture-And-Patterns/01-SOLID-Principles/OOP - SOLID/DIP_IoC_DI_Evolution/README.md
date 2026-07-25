# Архітектурний практикум: Еволюція від DIP до IoC, DI та сучасних контейнерів

> **"DIP визначає мету: залежати від абстракцій. IoC інвертує контроль. DI надає механізм. Контейнер автоматизує рутину."**  
> — *Марк Сіман (Mark Seemann), автор фундаментальної праці «Dependency Injection in .NET»*

---

## 🧭 Швидка навігація до коду проекту

Усі посилання оптимізовані для середовища **Visual Studio 2022** та **VS Code** (натисніть на посилання або відкрийте файл клавішею `F12`):

| Етап еволюції | Клас / Компонент | Роль в архітектурі | Посилання на вихідний код |
| :--- | :--- | :--- | :---: |
| 🎮 **Головний запуск** | **`DipEvolutionCommand`** | Інтерактивне консольне меню для тестування кроків | [👉 Відкрити `DipEvolutionCommand.cs`](DipEvolutionCommand.cs) |
| **Крок 1: DIP** | `IUserRepository` | Контракт доступу до бази даних користувачів | [👉 Відкрити `IUserRepository.cs`](Step1_DIP/IUserRepository.cs) |
| **Крок 1: DIP** | `INotificationSender` | Контракт сповіщення клієнтів | [👉 Відкрити `INotificationSender.cs`](Step1_DIP/INotificationSender.cs) |
| **Крок 1: DIP** | `ILogger` | Контракт системного логування та аудиту | [👉 Відкрити `ILogger.cs`](Step1_DIP/ILogger.cs) |
| **Крок 1: DIP** | `MySqlUserRepository` | Конкретна низькорівнева реалізація сховища MySQL | [👉 Відкрити `MySqlUserRepository.cs`](Step1_DIP/MySqlUserRepository.cs) |
| **Крок 1: DIP** | `SmtpNotificationSender` | Конкретна низькорівнева реалізація відправки Email | [👉 Відкрити `SmtpNotificationSender.cs`](Step1_DIP/SmtpNotificationSender.cs) |
| **Крок 1: DIP** | `ConsoleLogger` | Конкретна реалізація виводу логів у термінал | [👉 Відкрити `ConsoleLogger.cs`](Step1_DIP/ConsoleLogger.cs) |
| **Крок 1: DIP** | `DipPrincipleDemo` | Демонстрація розвороту стрілок залежностей | [👉 Відкрити `DipPrincipleDemo.cs`](Step1_DIP/DipPrincipleDemo.cs) |
| **Крок 2: IoC** | `IInfrastructureFactory` | Інтерфейс патерна GoF Abstract Factory | [👉 Відкрити `IInfrastructureFactory.cs`](Step2_AbstractFactory_IoC/IInfrastructureFactory.cs) |
| **Крок 2: IoC** | `MySqlInfrastructureFactory` | Конкретна фабрика сімейства MySQL + SMTP | [👉 Відкрити `ConcreteFactories.cs`](Step2_AbstractFactory_IoC/ConcreteFactories.cs) |
| **Крок 2: IoC** | `PostgresInfrastructureFactory`| Конкретна фабрика сімейства PostgreSQL + Push | [👉 Відкрити `ConcreteFactories.cs`](Step2_AbstractFactory_IoC/ConcreteFactories.cs) |
| **Крок 2: IoC** | `AlternativeInfrastructure` | Драйвери PostgreSQL, Push-сповіщень та FileLogger | [👉 Відкрити `AlternativeInfrastructure.cs`](Step2_AbstractFactory_IoC/AlternativeInfrastructure.cs) |
| **Крок 2: IoC** | `UserServiceWithFactory` | Бізнес-сервіс із Pull-моделлю отримання залежностей | [👉 Відкрити `UserServiceWithFactory.cs`](Step2_AbstractFactory_IoC/UserServiceWithFactory.cs) |
| **Крок 2: IoC** | `FactoryIoCDemo` | Демонстрація підміни всього стека одним рядком коду | [👉 Відкрити `FactoryIoCDemo.cs`](Step2_AbstractFactory_IoC/FactoryIoCDemo.cs) |
| **Крок 3: DI** | `UserServiceWithDI` | Еталонний Constructor Injection (Push-модель) | [👉 Відкрити `UserServiceWithDI.cs`](Step3_DependencyInjection/UserServiceWithDI.cs) |
| **Крок 3: DI** | `InvoiceService` | Демонстрація Method Injection та Property Injection | [👉 Відкрити `MethodAndPropertyInjectionDemo.cs`](Step3_DependencyInjection/MethodAndPropertyInjectionDemo.cs) |
| **Крок 3: DI** | `FakeUserRepository` | In-memory Mock/Stub для миттєвих модульних тестів | [👉 Відкрити `FakeUserRepository.cs`](Step3_DependencyInjection/FakeUserRepository.cs) |
| **Крок 3: DI** | `PureDiDemo` | Точка збирання (Composition Root), тести, Pure DI | [👉 Відкрити `PureDiDemo.cs`](Step3_DependencyInjection/PureDiDemo.cs) |
| **Крок 4.1: Mini-IoC**| `SimpleContainer` | Власний контейнер із нуля на ~35 рядках Reflection | [👉 Відкрити `SimpleContainer.cs`](Step4_IoCContainer/MiniContainer/SimpleContainer.cs) |
| **Крок 4.1: Mini-IoC**| `MiniContainerDemo` | Автоматичне рекурсивне збирання графу залежностей | [👉 Відкрити `MiniContainerDemo.cs`](Step4_IoCContainer/MiniContainer/MiniContainerDemo.cs) |
| **Крок 4.2: Lifetimes**| `LifetimeContracts` | Контракти для аналізу Transient, Scoped, Singleton | [👉 Відкрити `LifetimeContracts.cs`](Step4_IoCContainer/DotNetContainer/LifetimeContracts.cs) |
| **Крок 4.2: Lifetimes**| `LifetimesDemo` | Порівняння поведінки Scope 1 vs Scope 2 через Guid | [👉 Відкрити `LifetimesDemo.cs`](Step4_IoCContainer/DotNetContainer/LifetimesDemo.cs) |
| **Крок 4.3: .NET DI** | `DotNetContainerDemo` | Промислове налаштування `IServiceCollection` у .NET | [👉 Відкрити `DotNetContainerDemo.cs`](Step4_IoCContainer/DotNetContainer/DotNetContainerDemo.cs) |
| 📚 **Повний посібник** | `06_DeepDive.md` | Академічна лекція з архітектури та теорії | [👉 Читати повну лекцію](../Docs/06_DIP_IoC_DI_Containers_DeepDive.md) |

---

## 1. Концептуальна мапа: 4 сходинки еволюції

Часто поняття **DIP**, **IoC**, **DI** та **IoC-контейнер** вживаються як синоніми. Це груба інженерна помилка. Вони знаходяться на принципово різних рівнях абстракції:

```mermaid
flowchart TD
    classDef badStyle fill:#801313,stroke:#ef4444,color:#ffffff,stroke-width:2px;
    classDef dipStyle fill:#1e293b,stroke:#64748b,color:#ffffff,stroke-width:2px;
    classDef iocStyle fill:#854d0e,stroke:#eab308,color:#ffffff,stroke-width:2px;
    classDef diStyle fill:#1e40af,stroke:#3b82f6,color:#ffffff,stroke-width:2px;
    classDef contStyle fill:#065f46,stroke:#10b981,color:#ffffff,stroke-width:2px;

    Step0["0. Спагеті-код (Tight Coupling)<br/>new MySqlDatabase() всередині бізнес-логіки"]:::badStyle
    Step1["1. DIP (Принцип SOLID)<br/>Високорівневі й низькорівневі класи залежать від спільних інтерфейсів"]:::dipStyle
    Step2["2. IoC + Abstract Factory (Парадигма + GoF)<br/>Клас віддає контроль за створення об'єктів зовнішній Фабриці"]:::iocStyle
    Step3["3. DI: Constructor Injection (Шаблон проектування)<br/>Залежності насильно передаються у конструктор ззовні (PUSH)"]:::diStyle
    Step4["4. IoC Container (Інструмент автоматизації)<br/>Рефлексія автоматично будує граф об'єктів і керує Lifetimes"]:::contStyle

    Step0 -->|Вводимо інтерфейси| Step1
    Step1 -->|Інвертуємо керування створенням| Step2
    Step2 -->|Позбуваємося знання про фабрики (Push замість Pull)| Step3
    Step3 -->|Усуваємо рутинне ручне збирання об'єктів (Constructor Hell)| Step4
```

### Порівняльна таблиця сутностей:

| Поняття | Що це таке? | На яке питання відповідає? | Приклад у нашому коді |
| :--- | :--- | :--- | :--- |
| **DIP** | **Архітектурний принцип** проектування (SOLID) | *Від чого повинен залежати мій клас?* (Від абстракцій, а не від деталей) | [IUserRepository.cs](Step1_DIP/IUserRepository.cs) |
| **IoC** | **Парадигма / Концепція** («Голлівудський принцип») | *Хто керує життєвим циклом та створенням об'єктів?* (Не сам клас, а зовнішнє середовище) | [IInfrastructureFactory.cs](Step2_AbstractFactory_IoC/IInfrastructureFactory.cs) |
| **DI** | **Шаблон проектування** (Design Pattern) | *Як саме доставити залежність усередину класу?* (Через аргументи конструктора — Push) | [UserServiceWithDI.cs](Step3_DependencyInjection/UserServiceWithDI.cs) |
| **IoC Container** | **Програмний інструмент / Бібліотека** | *Як автоматизувати виклики `new` для сотень класів системи?* (Через Reflection/Source Gen) | [SimpleContainer.cs](Step4_IoCContainer/MiniContainer/SimpleContainer.cs) |

---

## 2. Покроковий розбір еволюції

### Крок 1. Принцип інверсії залежностей (DIP)
- **Файли коду:** [`Step1_DIP/`](Step1_DIP/)
- **Проблема антипатерну:** Коли клас `UserService` всередині пише `new MySqlUserRepository()`, він намертво пришитий до конкретної бази даних. Його неможливо протестувати без підключення до реального сервера MySQL, і будь-яка зміна БД вимагає переписування бізнес-коду.
- **Рішення DIP:** Створюються чисті абстракції:
  - [`IUserRepository`](Step1_DIP/IUserRepository.cs)
  - [`INotificationSender`](Step1_DIP/INotificationSender.cs)
  - [`ILogger`](Step1_DIP/ILogger.cs)
- **Результат:** Напрямок залежності розгортається на 180 градусів. Високорівневий сервіс та низькорівневий репозиторій тепер обидва залежать від спільного контракту.

---

### Крок 2. Інверсія контролю через Абстрактну Фабрику (IoC & Abstract Factory)
- **Файли коду:** [`Step2_AbstractFactory_IoC/`](Step2_AbstractFactory_IoC/)
- **Ідея:** Застосувати патерн **Abstract Factory** (GoF) для демонстрації першого кроку до IoC (Hollywood Principle: *"Не дзвоніть нам, ми самі вам зателефонуємо"*).
- **Реалізація:** Інтерфейс [`IInfrastructureFactory`](Step2_AbstractFactory_IoC/IInfrastructureFactory.cs) та його дві конкретні реалізації:
  - `MySqlInfrastructureFactory` (повертає MySQL репозиторій, SMTP пошту, Console логер).
  - `PostgresInfrastructureFactory` (повертає PostgreSQL репозиторій, Push сповіщення, File логер).
- **Обмеження підходу (Pull-модель):** Сервіс [`UserServiceWithFactory`](Step2_AbstractFactory_IoC/UserServiceWithFactory.cs) знає про існування фабрики і змушений сам викликати методи `_factory.CreateUserRepository()`. Це створює непотрібну залежність від фабрики та ускладнює модульне тестування (наближається до антипатерна Service Locator).

---

### Крок 3. Впровадження залежностей (Dependency Injection - DI)
- **Файли коду:** [`Step3_DependencyInjection/`](Step3_DependencyInjection/)
- **Перехід від Pull до Push:** Ми забираємо у `UserService` будь-яку можливість керувати створенням залежностей!
- **Три види впровадження залежностей:**
  1. **Constructor Injection** (Еталонний та рекомендований підхід) — [`UserServiceWithDI.cs`](Step3_DependencyInjection/UserServiceWithDI.cs). Всі обов'язкові залежності передаються через конструктор і зберігаються у `readonly` полях.
  2. **Method Injection** — [`MethodAndPropertyInjectionDemo.cs`](Step3_DependencyInjection/MethodAndPropertyInjectionDemo.cs). Залежність потрібна лише для виконання однієї конкретної операції (наприклад, форматер чеку `IReceiptFormatter`).
  3. **Property (Setter) Injection** — Необов'язкові (опціональні) залежності з гарантованим значенням за замовчуванням (Null Object або Fallback).
- **Миттєве модульне тестування:** Завдяки Constructor DI ми передаємо фейковий репозиторій [`FakeUserRepository.cs`](Step3_DependencyInjection/FakeUserRepository.cs) у сервіс і тестуємо його без баз даних та мережі за 0.001 секунди (див. [`PureDiDemo.cs`](Step3_DependencyInjection/PureDiDemo.cs)).
- **Проблема «Pure DI» (Constructor Hell):** Якщо у додатку сотні сервісів із вкладеними залежностями, створення кореневого об'єкта вручну перетворюється на нескінченний спагеті-код:
  ```csharp
  var service = new OrderService(
      new UserRepository(new DatabaseConnection(new ConfigManager())),
      new EmailSender(new SmtpClient(new NetworkCredentials())),
      new PaymentGateway(new BankClient(new SecurityContext()))
  );
  ```

---

### Крок 4. IoC-контейнери: від рефлексії до Microsoft.Extensions.DependencyInjection
- **Файли коду:** [`Step4_IoCContainer/`](Step4_IoCContainer/)

#### 4.1. Власний міні-контейнер "під капотом" ([`SimpleContainer.cs`](Step4_IoCContainer/MiniContainer/SimpleContainer.cs))
- Щоб розвінчати міф про «магію» IoC-контейнерів, ми написали повноцінний контейнер усього на **35 рядках чистого C#**:
  - `Register<TService, TImplementation>()` — зберігає відображення `Type -> Type` у словнику `Dictionary<Type, Type>`.
  - `Resolve<TService>()` — знаходить конструктор із найбільшою кількістю параметрів через рефлексію (`GetConstructors()`), рекурсивно резолвить кожен аргумент конструктора і створює об'єкт через `Activator.CreateInstance()`.
- Демонстрація запуску: [`MiniContainerDemo.cs`](Step4_IoCContainer/MiniContainer/MiniContainerDemo.cs).

#### 4.2. Життєві цикли об'єктів (Lifetimes) у .NET ([`LifetimesDemo.cs`](Step4_IoCContainer/DotNetContainer/LifetimesDemo.cs))
- **Transient (`AddTransient`)**: Новий екземпляр створюється **кожного разу** при запиті. Ідеально для легких об'єктів без збереження стану.
- **Scoped (`AddScoped`)**: **Один екземпляр на область видимості** (Scope). В ASP.NET Core один Scope створюється на кожен окремий вхідний HTTP-запит (наприклад, `DbContext`).
- **Singleton (`AddSingleton`)**: **Один-єдиний екземпляр** на весь час життя додатка (process lifetime). Спільний для всіх потоків і скоупів (наприклад, кеш у пам'яті).
- Доказ поведінки реалізовано через унікальні `Guid` операцій:
  - У межах одного Scope скоупед-об'єкти мають однаковий Guid.
  - При переході до Scope 2 скоупед-об'єкти отримують абсолютно новий Guid.
  - Синглтон зберігає однаковий Guid в обох скоупах.

#### 4.3. Промисловий IoC у .NET ([`DotNetContainerDemo.cs`](Step4_IoCContainer/DotNetContainer/DotNetContainerDemo.cs))
- Використання стандарту індустрії `Microsoft.Extensions.DependencyInjection`:
  ```csharp
  var services = new ServiceCollection();
  services.AddTransient<IUserRepository, MySqlUserRepository>();
  services.AddTransient<INotificationSender, SmtpNotificationSender>();
  services.AddSingleton<ILogger, ConsoleLogger>();
  services.AddTransient<UserServiceWithDI>();

  using var serviceProvider = services.BuildServiceProvider();
  var userService = serviceProvider.GetRequiredService<UserServiceWithDI>();
  ```

---

## 3. Як запустити та протестувати

1. Відкрийте рішення у **Visual Studio 2022** або **VS Code**.
2. Запустіть консольний застосунок (`F5` або `Ctrl+F5`).
3. У головному меню SOLID оберіть пункт:
   ```text
   DIP - ЕВОЛЮЦІЯ: DIP -> IoC (Abstract Factory) -> DI -> IoC Container
   ```
4. Відкриється внутрішнє інтерактивне підменю оркестратора [`DipEvolutionCommand.cs`](DipEvolutionCommand.cs):
   ```text
   ╔══════════════════════════════════════════════════════════════════════════╗
   ║      ЕВОЛЮЦІЙНИЙ ПРАКТИКУМ: ВІД ПРИНЦИПУ ДО СУЧАСНОГО DI-КОНТЕЙНЕРА      ║
   ║            DIP  --->  IoC  --->  Abstract Factory  --->  DI  --->  IoC  ║
   ╚══════════════════════════════════════════════════════════════════════════╝

   1. [DIP]       Крок 1. Принцип інверсії залежностей (Контракти замість деталей)
   2. [IoC]       Крок 2. Інверсія контролю через Абстрактну Фабрику (GoF Pattern)
   3. [DI]        Крок 3. Впровадження залежностей (Constructor, Method, Property DI)
   4. [Mini-IoC]  Крок 4.1. Власний IoC-контейнер 'під капотом' (Рефлексія з нуля)
   5. [Lifetimes] Крок 4.2. Життєві цикли .NET: Transient vs Scoped vs Singleton
   6. [.NET DI]   Крок 4.3. Промисловий Microsoft.Extensions.DependencyInjection
   7. [ALL]       Запустити повний ланцюжок по черзі (1 -> 6)
   0. [Назад]     Повернутися до головного меню SOLID
   ```
5. Ви можете перевірити кожен етап окремо або запустити варіант `7`, який продемонструє весь еволюційний ланцюжок послідовно!
