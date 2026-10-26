# 07. Фінальний проєкт: Власний IoC/DI Контейнер (IoC Container Deep Dive & Architecture)

> **Solution-тег (Solution Folder)**: `06. Frameworks & Dependency Injection`  
> **Проєкти у тегу**:
> - [19.1-DependencyInjectionDemo](../19.1-DependencyInjectionDemo/Program.cs) — Навчальний мінімальний IoC-контейнер для студентів (прозоре створення екземплярів через `GetConstructors`, `GetParameters`, `Invoke`).
> - [19.2-DependencyInjectionRealWorld](../19.2-DependencyInjectionRealWorld/Program.cs) — Промислова модульна архітектура: In-Memory база даних, репозиторії, сервіси, валідація циклів, Greedy Ctor, Property Injection.

Цей розділ є вершиною навчального модуля. Він демонструє практичне поєднання всіх вивчених концепцій рефлексії: інспекції типів, дослідження конструкторів і параметрів, аналізу атрибутів, роботи з інтерфейсами та динамічної активації об'єктів для побудови власного Dependency Injection контейнера.

Матеріал розбито на два дидактичні рівні:
1. **Рівень 1 (Проєкт 19.1)**: Прозорий мінімальний контейнер (~70 рядків), призначений для першого знайомства студентів із механізмом конструювання об'єктів через рефлексію.
2. **Рівень 2 (Проєкт 19.2)**: Повнофункціональна модульна система з реальними бізнес-сервісами, контекстом бази даних, виявленням циклічних залежностей та жадібним вибором конструкторів.

---

## 1. Понятійний апарат: Архітектурні принципи IoC та DI

### 1.1. Що таке Inversion of Control (IoC)?
**Інверсія керування (Inversion of Control — IoC)** — це архітектурний принцип проєктування програмного забезпечення, за якого традиційний потік керування змінюється на протилежний: замість того, щоб користувацький код самостійно створював залежні компоненти та викликав бібліотечні функції, зовнішній каркас (фреймворк) бере на себе керування життєвим циклом об'єктів і викликає користувацький код.

- **Голлівудський принцип (Hollywood Principle)**: *"Не дзвоніть нам, ми самі вам зателефонуємо"*. Клас більше не пише `new SqlDatabase()`, а пасивно очікує, поки контейнер створить і надасть йому необхідний ресурс.

### 1.2. Що таке Dependency Inversion Principle (DIP)?
**Принцип інверсії залежностей (DIP)** — це літера "D" у принципах SOLID:
1. Модулі високих рівнів не повинні залежати від модулів низьких рівнів. Обидва типи мають залежати від абстракцій (інтерфейсів).
2. Абстракції не повинні залежати від деталей. Деталі повинні залежати від абстракцій.

### 1.3. Що таке Dependency Injection (DI)?
**Впровадження залежностей (Dependency Injection — DI)** — це конкретний патерн проектування, що реалізує принцип IoC. Він полягає в тому, що залежності передаються в об'єкт ззовні (найчастіше через параметри конструктора — Constructor Injection або через властивості — Property Injection), а не створюються об'єктом самостійно.

### 1.4. Що таке IoC-контейнер?
**IoC-контейнер (DI-контейнер)** — це програмний модуль (фреймворк), який автоматизує процес конструювання об'єктів. Він слугує централізованим реєстром відповідностей між абстракціями (інтерфейсами) та їхніми конкретними реалізаціями, рекурсивно розв'язує граф залежностей та керує життєвими циклами створених екземплярів.

---

## 2. Рівень 1: Прозорий навчальний IoC-контейнер (Проєкт 19.1)

Для первинного ознайомлення студентів створено проєкт [19.1-DependencyInjectionDemo](../19.1-DependencyInjectionDemo/Program.cs). Він демонструє, що під капотом будь-якого IoC-контейнера лежать лише **три базові методи рефлексії**:

```mermaid
flowchart TD
    ResolveCall["1. container.Resolve(serviceType)"] --> FindReg["2. Пошук дескриптора реєстрації<br/>(Interface -> Implementation)"]
    FindReg --> CheckSingleton{"Це Singleton і екземпляр уже створено?"}
    CheckSingleton -- Так --> ReturnCached["Повернути кешований об'єкт"]
    CheckSingleton -- Ні --> GetCtors["3. РЕФЛЕКСІЯ: targetType.GetConstructors()"]
    GetCtors --> GetParams["4. РЕФЛЕКСІЯ: ctor.GetParameters()"]
    GetParams --> LoopParams["5. Рекурсивне розв'язання кожного аргументу:<br/>Resolve(param.ParameterType)"]
    LoopParams --> Invoke["6. РЕФЛЕКСІЯ: ctor.Invoke(resolvedArgs)"]
    Invoke --> SaveSingleton{"Це Singleton?"}
    SaveSingleton -- Так --> CacheObj["Зберегти у дескриптор"]
    SaveSingleton -- Ні --> ReturnObj["Повернути новий екземпляр"]
    CacheObj --> ReturnObj
```

### 2.1. Чотири кроки рефлексії в `SimpleContainer`

```csharp
// Крок 1: Отримуємо конструктор класу
ConstructorInfo constructor = targetType.GetConstructors()[0];

// Крок 2: Читаємо список необхідних параметрів
ParameterInfo[] parameters = constructor.GetParameters();
object[] parameterInstances = new object[parameters.Length];

// Крок 3: Рекурсивно розв'язуємо кожен параметр через контейнер
for (int i = 0; i < parameters.Length; i++)
{
    parameterInstances[i] = Resolve(parameters[i].ParameterType);
}

// Крок 4: Створюємо готовий екземпляр передачею параметрів
object createdInstance = constructor.Invoke(parameterInstances);
```

- Посилання на реалізацію простого контейнера: [19.1-DependencyInjectionDemo/SimpleContainer.cs](../19.1-DependencyInjectionDemo/SimpleContainer.cs).
- Посилання на консольну демонстрацію зі студентоорієнтованим виводом: [19.1-DependencyInjectionDemo/Program.cs](../19.1-DependencyInjectionDemo/Program.cs).

---

## 3. Рівень 2: Реальний промисловий DI-контейнер (Проєкт 19.2)

Після засвоєння базового механізму у проєкті 19.1, проєкт [19.2-DependencyInjectionRealWorld](../19.2-DependencyInjectionRealWorld/Program.cs) демонструє повномасштабну промислову модульну архітектуру.

### 3.1. Математична модель: Орієнтований ациклічний граф (DAG)

Процес розв'язання залежностей у контейнері формалізується як побудова та топологічний обхід **орієнтованого ациклічного графу (Directed Acyclic Graph — DAG)**:
- **Вершини ($V$)**: зареєстровані типи сервісів (`OrderService`, `IOrderRepository`, `ILogger`).
- **Орієнтовані ребра ($E$)**: конструкторні залежності між типами ($A \to B$ означає, що клас $A$ вимагає у своєму конструкторі тип $B$).

```mermaid
graph TD
    OrderService["OrderProcessingService"]
    OrderService --> IDatabaseContext["IDatabaseContext (Singleton)"]
    OrderService --> IProductRepository["IProductRepository (Transient)"]
    OrderService --> IOrderRepository["IOrderRepository (Transient)"]
    OrderService --> IPaymentService["IPaymentService (Transient)"]
    OrderService --> ILogger["ILogger (Singleton)"]
    OrderService -.->|Property Injection| INotificationService["INotificationService (Transient)"]

    IProductRepository --> IDatabaseContext
    IProductRepository --> ILogger

    IOrderRepository --> IDatabaseContext
    IOrderRepository --> ILogger

    IPaymentService --> ILogger
    INotificationService --> ILogger
```

Граф розв'язання обов'язково повинен бути **ациклічним**. Поява будь-якого циклу робить конструювання об'єктів неможливим.

---

## 3. Захист від циклічних залежностей (Circular Dependency Detection)

Якщо розробники припустилися архітектурної помилки і створили взаємну залежність класів:
$$\text{ServiceA} \longrightarrow \text{ServiceB} \longrightarrow \text{ServiceC} \longrightarrow \text{ServiceA}$$

Наївний рекурсивний алгоритм увійде в нескінченну рекурсію. У середовищі CLR це гарантовано призведе до вичерпання пам'яті стека потоку та фатального аварійного завершення процесу з помилкою **`StackOverflowException`**, яку в .NET неможливо перехопити через блок `try-catch`!

### 3.1. Алгоритм виявлення циклів за допомогою стеку
Контейнер підтримує внутрішній контекстний стек поточного ланцюжка резолюції `Stack<Type> _resolutionStack`:
1. Перед початком створення будь-якого типу перевіряється умова: `_resolutionStack.Contains(serviceType)`.
2. Якщо тип уже є у стеку — зафіксовано наявність зворотного ребра в графі (цикл!).
3. Контейнер негайно припиняє обхід і викидає виняток `CircularDependencyException`, формуючи зрозумілий ланцюжок помилки для діагностики:

```csharp
if (_resolutionStack.Contains(serviceType))
{
    var cycleChain = _resolutionStack.Reverse().Concat(new[] { serviceType }).Select(t => t.Name);
    string formattedChain = string.Join(" -> ", cycleChain);
    throw new CircularDependencyException($"Виявлено циклічну залежність! Ланцюжок: {formattedChain}");
}
```

- Посилання на реалізацію алгоритму виявлення циклів: [19.2-DependencyInjectionRealWorld/Container/Container.cs](../19.2-DependencyInjectionRealWorld/Container/Container.cs#L85-L95).

---

## 4. Алгоритм розв'язання конструкторів (Constructor Resolution Engine)

Клас може мати декілька відкритих конструкторів. Контейнер використовує такий детермінований алгоритм вибору:

```mermaid
flowchart TD
    Start["Запит на створення типу T"] --> GetCtors["type.GetConstructors(Public | Instance)"]
    GetCtors --> HasAttr{"Чи є конструктор з атрибутом [InjectConstructor]?"}
    HasAttr -- Так --> PickAttr["Обрати конструктор з [InjectConstructor]"]
    HasAttr -- Ні --> SortGreedy["Відсортувати за кількістю параметрів (OrderByDescending)"]
    PickAttr --> ResolveParams["Аналіз кожного параметра (ParameterInfo)"]
    SortGreedy --> ResolveParams
    ResolveParams --> CheckReg{"Тип зареєстрований або HasDefaultValue == true?"}
    CheckReg -- Зареєстрований --> Recurse["Рекурсивний Resolve(param.ParameterType)"]
    CheckReg -- Має дефолтне значення --> UseDef["Підставити param.DefaultValue"]
    Recurse --> InvokeCtor["ctor.Invoke(resolvedArgs)"]
    UseDef --> InvokeCtor
    InvokeCtor --> CheckProp["Пошук властивостей з [InjectProperty]"]
    CheckProp --> Finish["Повернення повністю зібраного об'єкта"]
```

### 4.1. Етапи конструювання:
1. **Явна вказівка**: якщо присутній конструктор із міткою `[InjectConstructor]`, контейнер обирає його беззастережно.
2. **Жадібний вибір (Greedy Constructor Selection)**: якщо мітки немає, конструктори сортуються за спаданням кількості параметрів (`c.GetParameters().Length`). Контейнер намагається задовольнити конструктор із найбільшою кількістю залежностей.
3. **Параметри за замовчуванням**: якщо залежність не зареєстрована в контейнері, але параметр конструктора має значення за замовчуванням (`param.HasDefaultValue == true`), контейнер безпечно підставляє `param.DefaultValue`.
4. **Впровадження у властивості (Property Injection)**: після створення екземпляра контейнер знаходить усі відкриті властивості з атрибутом `[InjectProperty]` і викликає для них `prop.SetValue(instance, resolvedDependency)`.

- Посилання на реалізацію механізму інстанціювання: [19.2-DependencyInjectionRealWorld/Container/Container.cs](../19.2-DependencyInjectionRealWorld/Container/Container.cs#L165-L215).

---

## 5. Керування життєвими циклами (Service Lifetimes)

Контейнер підтримує дві фундаментальні моделі життєвого циклу:

| Життєвий цикл | Поведінка в пам'яті | Збереження посилання | Сценарії застосування |
| :--- | :--- | :--- | :--- |
| **`Transient`** | Новий об'єкт створюється при кожному зверненні до `Resolve<T>()`. | Посилання не зберігається контейнером. Об'єкт знищується збирачем сміття (GC), коли його перестає використовувати клієнтський код. | Легковагові сервіси та репозиторії без внутрішнього стану. |
| **`Singleton`** | Створюється рівно один екземпляр при першому зверненні. | Контейнер кешує посилання у внутрішньому потокобезпечному словнику `_registrations` на весь час життя додатка. | Контексти баз даних (`IDatabaseContext`), логери (`ILogger`), кеші. |

---

## 6. Демонстраційний запуск та візуалізація дерева розв'язання

Проєкт `19.2-DependencyInjectionRealWorld` містить повнофункціональну консольну демонстрацію інтернет-магазину зі структурованою емуляцією бази даних:

```csharp
var container = new Container();

// 1. Реєстрація компонентів
container.RegisterSingleton<ILogger, ConsoleLogger>();
container.RegisterSingleton<IDatabaseContext, InMemoryDatabaseContext>();
container.RegisterTransient<IProductRepository, ProductRepository>();
container.RegisterTransient<IOrderRepository, OrderRepository>();
container.RegisterTransient<IPaymentService, CreditCardPaymentService>();
container.RegisterTransient<INotificationService, EmailNotificationService>();
container.RegisterTransient<IOrderProcessingService, OrderProcessingService>();

// 2. Рекурсивне розв'язання зі створенням повного графа об'єктів
var orderService = container.Resolve<IOrderProcessingService>();

// 3. Виконання бізнес-операцій: списання оплати, оновлення складу, збереження в БД, надсилання сповіщення
orderService.PlaceOrder(customerId: 1, productId: 1, quantity: 2);
```

- Посилання на вихідний код контейнера: [19.2-DependencyInjectionRealWorld/Container/Container.cs](../19.2-DependencyInjectionRealWorld/Container/Container.cs).
- Посилання на контракт контейнера: [19.2-DependencyInjectionRealWorld/Container/IContainer.cs](../19.2-DependencyInjectionRealWorld/Container/IContainer.cs).
- Посилання на емуляцію бази даних: [19.2-DependencyInjectionRealWorld/Data/InMemoryDatabaseContext.cs](../19.2-DependencyInjectionRealWorld/Data/InMemoryDatabaseContext.cs).
- Посилання на бізнес-сервіс замовлень: [19.2-DependencyInjectionRealWorld/Services/Orders/OrderProcessingService.cs](../19.2-DependencyInjectionRealWorld/Services/Orders/OrderProcessingService.cs).
- Посилання на консольну демонстраційну програму: [19.2-DependencyInjectionRealWorld/Program.cs](../19.2-DependencyInjectionRealWorld/Program.cs).
