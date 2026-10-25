# 01. Дослідження системи типів та інспекція метаданих (Type System & Metadata)

Рефлексія (**Reflection**) в .NET — це здатність керуючого середовища (CLR) досліджувати структуру збірок, модулів, типів та їхніх членів під час виконання (runtime), а також створювати нові сутності й динамічно викликати їхній код.

---

## 🏛️ Ключові класи інспекції системи типів

1. **`System.Type`** — центральний хаб доступу до метаданих типу. Представляє оголошення класів, структур, інтерфейсів, перерахувань, делегатів і масивів.
2. **`System.Reflection.Assembly`** — представляє скомпільовану збірку (.dll / .exe). Дозволяє перелічувати типи, шукати експортовані ресурси та завантажувати залежності.
3. **`System.Reflection.MemberInfo`** — абстрактний базовий клас для всіх членів типу (`MethodInfo`, `PropertyInfo`, `FieldInfo`, `ConstructorInfo`, `EventInfo`).

---

## 🔍 Три шляхи отримання екземпляра `System.Type`

| Спосіб | Час виконання | Коли використовувати | Особливості | Приклад у коді |
| :--- | :--- | :--- | :--- | :---: |
| **`typeof(T)`** | Compile-time | Тип відомий на етапі написання коду | Найшвидший, 100% типобезпечний, підтримує відкриті дженерики `typeof(List<>)` | [![Перейти](https://img.shields.io/badge/Код-TypeofDemo-blue?style=flat-square)](../TypeofDemo/Program.cs#L8-L23) |
| **`obj.GetType()`** | Runtime | Є екземпляр об'єкта у пам'яті | Поліморфний: повертає фактичний конкретний тип навіть за інтерфейсним посиланням | [![Перейти](https://img.shields.io/badge/Код-ObjectGetType-blue?style=flat-square)](../ObjectGetTypeDemo/Program.cs#L7-L25) |
| **`Type.GetType(string)`** | Runtime | Відоме лише текстове ім'я типу | Завантаження плагінів, конфігураційні файли, AssemblyQualifiedName | [![Перейти](https://img.shields.io/badge/Код-TypeGetType-blue?style=flat-square)](../TypeGetTypeDemo/Program.cs#L8-L31) |

---

## 📋 Узагальнююча таблиця: Основні властивості та методи `System.Type`

| Метод / Властивість | Тип повернення | Призначення та поведінка | Посилання на приклад |
| :--- | :--- | :--- | :---: |
| `Name` | `string` | Коротка назва типу (без простору імен, наприклад `"Car"`) | [![Код](https://img.shields.io/badge/Код-TypeProperties-blue?style=flat-square)](../TypePropertiesDemo/Program.cs#L18-L22) |
| `FullName` | `string?` | Повна назва включно з простором імен (наприклад `"Shared.Common.Car"`) | [![Код](https://img.shields.io/badge/Код-TypeProperties-blue?style=flat-square)](../TypePropertiesDemo/Program.cs#L19-L21) |
| `AssemblyQualifiedName` | `string?` | Повна назва з версією, культурою та токеном публічного ключа збірки | [![Код](https://img.shields.io/badge/Код-TypeGetType-blue?style=flat-square)](../TypeGetTypeDemo/Program.cs#L13-L17) |
| `BaseType` | `Type?` | Безпосередній базовий клас у ланцюжку наслідування | [![Код](https://img.shields.io/badge/Код-TypeProperties-blue?style=flat-square)](../TypePropertiesDemo/Program.cs#L22-L23) |
| `IsClass` / `IsValueType` | `bool` | Класифікує тип: посилальний тип (class) чи тип значення (struct) | [![Код](https://img.shields.io/badge/Код-TypeProperties-blue?style=flat-square)](../TypePropertiesDemo/Program.cs#L25-L27) |
| `IsInterface` / `IsEnum` | `bool` | Визначає, чи є тип інтерфейсом або перерахуванням | [![Код](https://img.shields.io/badge/Код-TypeProperties-blue?style=flat-square)](../TypePropertiesDemo/Program.cs#L25-L27) |
| `IsAbstract` / `IsSealed` | `bool` | Перевіряє наявність модифікаторів `abstract` або `sealed` | [![Код](https://img.shields.io/badge/Код-TypeProperties-blue?style=flat-square)](../TypePropertiesDemo/Program.cs#L26-L28) |
| `IsGenericType` | `bool` | Чи є тип дженериком (як відкритим `List<>`, так і закритим `List<int>`) | [![Код](https://img.shields.io/badge/Код-GenericsDemo-green?style=flat-square)](../GenericsReflectionDemo/Program.cs#L8-L18) |
| `GetInterfaces()` | `Type[]` | Повертає масив усіх інтерфейсів, які реалізує даний тип | [![Код](https://img.shields.io/badge/Код-GetInterfaces-blue?style=flat-square)](../GetInterfacesDemo/Program.cs#L10-L16) |
| `IsAssignableFrom(Type)` | `bool` | Чи можна екземпляр переданого типу присвоїти змінній поточного типу | [![Код](https://img.shields.io/badge/Код-GetInterfaces-blue?style=flat-square)](../GetInterfacesDemo/Program.cs#L18-L22) |
| `GetInterfaceMap(Type)` | `InterfaceMapping` | Повертає карту відповідності між методами інтерфейсу та методами класу | [![Код](https://img.shields.io/badge/Код-GetInterfaces-blue?style=flat-square)](../GetInterfacesDemo/Program.cs#L24-L33) |
| `GetMembers(...)` | `MemberInfo[]` | Отримує список усіх членів (методів, властивостей, полів, подій) | [![Код](https://img.shields.io/badge/Код-TypeProperties-blue?style=flat-square)](../TypePropertiesDemo/Program.cs#L35-L42) |

---

## 📦 Дослідження збірок (`System.Reflection.Assembly`)

Збірка (.dll) є фізичною одиницею розгортання та збереження метаданих.

| Метод `Assembly` | Призначення | Посилання на приклад |
| :--- | :--- | :---: |
| `Assembly.GetExecutingAssembly()` | Отримує збірку, яка містить код поточного методу | [![Код](https://img.shields.io/badge/Код-AssemblyDemo-blue?style=flat-square)](../AssemblyDemo/Program.cs#L8-L10) |
| `Assembly.GetEntryAssembly()` | Отримує збірку вхідної точки процесу (виконуваний .exe файл) | [![Код](https://img.shields.io/badge/Код-AssemblyDemo-blue?style=flat-square)](../AssemblyDemo/Program.cs) |
| `Assembly.LoadFrom(string path)` | Завантажує збірку з файлової системи за вказаним шляхом | [![Код](https://img.shields.io/badge/Код-TypeGetType-blue?style=flat-square)](../TypeGetTypeDemo/Program.cs#L24-L31) |
| `assembly.GetTypes()` | Повертає абсолютно всі типи збірки (включно з `internal` та `private`) | [![Код](https://img.shields.io/badge/Код-AssemblyDemo-blue?style=flat-square)](../AssemblyDemo/Program.cs#L20-L25) |
| `assembly.GetExportedTypes()` | Повертає тільки відкриті (`public`) типи, видимі за межами збірки | [![Код](https://img.shields.io/badge/Код-AssemblyDemo-blue?style=flat-square)](../AssemblyDemo/Program.cs) |
| `assembly.GetCustomAttributes()` | Повертає глобальні метадані рівня збірки (версія, автор, копірайт) | [![Код](https://img.shields.io/badge/Код-AssemblyDemo-blue?style=flat-square)](../AssemblyDemo/Program.cs#L35-L40) |

---

## 💡 Практичний сценарій: Пошук плагінів у збірці

Одним із найпоширеніших застосувань `Assembly.GetTypes()` є динамічне виявлення сервісів, які реалізують певний спільний контракт, без жорсткої компіляційної прив'язки:

```csharp
// Пошук усіх некореневих конкретних реалізацій контракту ICar у збірці Shared.dll:
var carTypes = sharedAssembly.GetTypes()
    .Where(t => typeof(ICar).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

foreach (Type carType in carTypes)
{
    Console.WriteLine($"Знайдено доступний плагін: {carType.FullName}");
}
```

🔗 Переглянути повний працюючий код можна в [AssemblyDemo/Program.cs](../AssemblyDemo/Program.cs#L27-L35).
