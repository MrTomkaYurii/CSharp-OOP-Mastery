# 02. Конфігураційні файли та Патерн Options (`IConfiguration`, `appsettings.json`, `IOptions`)

Жоден промисловий застосунок не зберігає рядки підключення, ліміти пакетної обробки чи адреси серверів безпосередньо у скомпільованому коді C# (Hardcoding). Платформа .NET надає потужну багаторівневу систему конфігурації `Microsoft.Extensions.Configuration` та патерн строгої типізації **Options Pattern** (`Microsoft.Extensions.Options`).

---

## 1. Ієрархія та пріоритет джерел у `Host.CreateApplicationBuilder`

Коли ми створюємо `Host.CreateApplicationBuilder(args)`, хост автоматично налаштовує конвеєр провайдерів конфігурації у чіткому порядку зростання пріоритету (кожен наступний рівень **перекриває** значення попереднього з тим самим ключем):

```mermaid
flowchart LR
    P1["1. appsettings.json<br/>(Базові значення)"] --> P2["2. appsettings.{Environment}.json<br/>(Налаштування середовища)"]
    P2 --> P3["3. User Secrets<br/>(Локальні секрети у Development)"]
    P3 --> P4["4. Змінні середовища ОС<br/>(Environment Variables)"]
    P4 --> P5["5. Аргументи командного рядка<br/>(Command-Line Args — найвищий пріоритет)"]
```

### Як об'єднуються ключі?
Використовується двокрапка `:` як роздільник рівнів вкладеності JSON (а у змінних середовища ОС двокрапку замінює подвійне підкреслення `__`):
- У `appsettings.json`: `"DatabaseSettings": { "MaxRetryCount": 3 }`
- Через змінну середовища: `DatabaseSettings__MaxRetryCount=5`
- Через аргументи командного рядка: `--OrderProcessing:MaxDiscountPercent=20.5`

---

## 2. Чому `IConfiguration` у бізнес-сервісах — це антипатерн?

Якщо інжектувати `IConfiguration` напряму у бізнес-класи (`config.GetValue<int>("OrderProcessing:BatchSize")`), виникають три серйозні проблеми:
1. **Порушення принципу ISP (Interface Segregation)**: сервіс потребує лише двох параметрів, але отримує доступ до всього дерева налаштувань програми.
2. **Втрата типобезпеки (Magic Strings)**: помилка в одній літері рядкового ключа виявиться лише під час виконання конкретного сценарію.
3. **Складність модульного тестування**: у Unit-тестах набагато простіше створити POCO-об'єкт налаштувань, ніж конструювати фейкове дерево `IConfiguration`.

Саме цю проблему вирішує **Патерн Options**.

---

## 3. Три інтерфейси патерну Options

Після прив'язки секції JSON до C#-класу через `builder.Services.AddOptions<T>().Bind(...)` сервіси можуть запитувати три різні обгортки залежно від потреб:

| Інтерфейс | Час життя в DI | Чи бачить зміни файлу без перезапуску? | Коли використовувати? |
|:---|:---|:---|:---|
| **`IOptions<T>`** | `Singleton` | Ні (читається один раз) | Для статичних налаштувань, які не змінюються на льоту (шляхи, базові ліміти). Найшвидший варіант. |
| **`IOptionsSnapshot<T>`** | `Scoped` | Так (оновлюється на початку кожного нового `IServiceScope`) | Коли кожна окрема бізнес-операція (Scope) має працювати з узгодженим знімком актуальних налаштувань. |
| **`IOptionsMonitor<T>`** | `Singleton` | Так (`CurrentValue` + подія `OnChange`) | У довгоживучих `Singleton`-сервісах та `BackgroundService`, яким потрібно миттєво реагувати на правки конфігурації. |

---

## 4. Декларативна валідація та принцип Fail-Fast (`ValidateOnStart`)

За замовчуванням помилка в `appsettings.json` (наприклад, від'ємний таймаут або порожній рядок підключення) проявиться лише тоді, коли якийсь сервіс вперше звернеться до `options.Value`. У виробничому середовищі це неприпустимо: застосунок має впасти **миттєво під час старту** з чітким переліком помилок конфігурації.

```csharp
builder.Services
    .AddOptions<DatabaseSettings>()
    .Bind(builder.Configuration.GetSection(DatabaseSettings.SectionName))
    .ValidateDataAnnotations() // Перевіряє атрибути [Required], [Range], [EmailAddress]
    .Validate(s => !string.IsNullOrWhiteSpace(s.ConnectionString), "Порожній ConnectionString!")
    .ValidateOnStart();        // Примусова перевірка на етапі host.StartAsync()!
```

Практичний приклад із перевіркою ієрархії конфігурації та перехопленням `OptionsValidationException` наведено у проєкті [`02-ConfigurationAndOptionsPattern`](../02-ConfigurationAndOptionsPattern/Program.cs).
