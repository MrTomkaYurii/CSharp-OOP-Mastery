# 02. Члени типу, параметри та бітові прапорці BindingFlags (Members, BindingFlags & Encapsulation)

Будь-який тип у .NET складається з членів: конструкторів, методів, властивостей, полів та подій. Усі вони походять від спільного абстрактного предка — **`System.Reflection.MemberInfo`**.

---

## 🏛️ 1. Ієрархія класів `MemberInfo` в CLR

```mermaid
classDiagram
    MemberInfo <|-- MethodBase
    MemberInfo <|-- PropertyInfo
    MemberInfo <|-- FieldInfo
    MemberInfo <|-- EventInfo
    MethodBase <|-- MethodInfo
    MethodBase <|-- ConstructorInfo

    class MemberInfo {
        <<abstract>>
        +string Name
        +Type DeclaringType
        +Type ReflectedType
        +MemberTypes MemberType
        +Module Module
        +int MetadataToken
        +CustomAttributeData[] CustomAttributes
    }

    class MethodBase {
        <<abstract>>
        +MethodAttributes Attributes
        +CallingConventions CallingConvention
        +bool IsStatic
        +bool IsPublic
        +bool IsPrivate
        +bool IsVirtual
        +ParameterInfo[] GetParameters()
        +MethodBody GetMethodBody()
    }

    class MethodInfo {
        +Type ReturnType
        +ParameterInfo ReturnParameter
        +MethodInfo MakeGenericMethod(Type[] typeArgs)
        +object? Invoke(object? obj, object?[]? parameters)
    }

    class ConstructorInfo {
        +object Invoke(object?[]? parameters)
    }

    class FieldInfo {
        +Type FieldType
        +FieldAttributes Attributes
        +bool IsInitOnly
        +object? GetValue(object? obj)
        +void SetValue(object? obj, object? value)
    }

    class PropertyInfo {
        +Type PropertyType
        +PropertyAttributes Attributes
        +bool CanRead
        +bool CanWrite
        +MethodInfo? GetMethod
        +MethodInfo? SetMethod
        +object? GetValue(object? obj)
        +void SetValue(object? obj, object? value)
    }
```

---

## 🎛️ 2. Бітова алгебра `System.Reflection.BindingFlags`

Методи пошуку членів (`GetMethod`, `GetProperty`, `GetField`, `GetConstructor`, `GetMembers`) використовують перерахування `BindingFlags`, позначене атрибутом `[Flags]`. Це означає, що значення комбінуються за допомогою бітового оператора `|` (побітове АБО).

### 2.1. Чому звичайний пошук може повернути `null`?
За замовчуванням методи без передачі `BindingFlags` використовують внутрішню маску:
$$\text{DefaultMask} = \text{BindingFlags.Public} \mid \text{BindingFlags.Instance} \mid \text{BindingFlags.Static}$$
Якщо шуканий член є приватним (`private`), захищеним (`protected`) або внутрішнім (`internal`), він **ніколи не буде знайдений** без явної передачі `BindingFlags.NonPublic`!

### 2.2. Повний реєстр прапорців `BindingFlags`

| Прапорець | Бітове значення | Призначення та поведінка |
| :--- | :---: | :--- |
| `BindingFlags.Public` | `0x10` | Включає у пошук відкриті члени. |
| `BindingFlags.NonPublic` | `0x20` | Включає закриті члени (`private`, `protected`, `internal`, `private protected`). |
| `BindingFlags.Instance` | `0x04` | Включає члени екземпляра (потребують конкретного створеного об'єкта для виклику чи зчитування). |
| `BindingFlags.Static` | `0x08` | Включає статичні члени класу (не прив'язані до конкретного об'єкта). |
| `BindingFlags.DeclaredOnly` | `0x02` | **Обмежує пошук членами поточного типу.** Відкидає всі члени, успадковані від предків (наприклад, методи `ToString()`, `Equals()`, `GetHashCode()` від `object`). |
| `BindingFlags.FlattenHierarchy` | `0x40` | Включає статичні члени вгору по дереву наслідування (за замовчуванням статичні члени предків не повертаються). |
| `BindingFlags.IgnoreCase` | `0x01` | Ігнорує регістр літер під час пошуку за назвою (наприклад, `"speed"` знайде `"Speed"`). |

> [!IMPORTANT]
> **Золоте правило BindingFlags**: завжди обов'язково комбінуйте прапорець видимості (`Public` або `NonPublic`) з прапорцем контексту (`Instance` або `Static`). Наприклад:
> ```csharp
> BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
> ```

---

## 🏗️ 3. Дослідження та виклик конструкторів (`ConstructorInfo`)

У метаданих CLR конструктори позначаються спеціальними назвами методів:
- `.ctor` — конструктор екземпляра (Instance Constructor).
- `.cctor` — статичний конструктор типу (Class Constructor / Type Initializer).

### 3.1. Довідник методів `ConstructorInfo`

| Метод / Властивість | Сигнатура | Опис | Посилання на приклад |
| :--- | :--- | :--- | :---: |
| `type.GetConstructors(...)` | `ConstructorInfo[] GetConstructors(BindingFlags flags)` | Повертає список конструкторів відповідно до переданої бітової маски. | [![Код](https://img.shields.io/badge/Код-10_GetCtors-blue?style=flat-square)](../10-GetConstructorInfoDemo/Program.cs#L10-L22) |
| `type.GetConstructor(Type[])` | `ConstructorInfo? GetConstructor(Type[] types)` | Знаходить перевантаження публічного конструктора з точним переліком типів параметрів. | [![Код](https://img.shields.io/badge/Код-10_FindCtor-blue?style=flat-square)](../10-GetConstructorInfoDemo/Program.cs#L24-L31) |
| `ctor.Invoke(object?[] args)` | `object Invoke(object?[]? parameters)` | Інстанціює об'єкт через знайдений конструктор. Якщо передати невідповідну кількість аргументів — кидає `TargetParameterCountException`. | [![Код](https://img.shields.io/badge/Код-10_InvokeCtor-blue?style=flat-square)](../10-GetConstructorInfoDemo/Program.cs#L27-L31) |
| `ctor.IsPublic` / `IsPrivate` | `bool IsPublic { get; }` | Визначає модифікатор доступу конструктора. | [![Код](https://img.shields.io/badge/Код-10_CtorVisibility-blue?style=flat-square)](../10-GetConstructorInfoDemo/Program.cs#L43-L49) |

---

## 🏷️ 4. Поля та властивості: `FieldInfo` проти `PropertyInfo`

У CLR **поля** і **властивості** кардинально відрізняються:
- **Поле (`FieldInfo`)** — фізична комірка пам'яті всередині об'єкта або пам'яті типу.
- **Властивість (`PropertyInfo`)** — синтаксичний цукор, що складається з рядка в таблиці метаданих `PropertyDef` та одного або двох згенерованих методів: `get_PropertyName()` та `set_PropertyName(value)`.

### 4.1. Злам інкапсуляції приватних полів
Рефлексія дозволяє читати та модифікувати навіть закриті `private` та `readonly` поля, отримуючи прямий доступ до внутрішнього стану об'єкта:

```csharp
FieldInfo? plateField = carType.GetField("_licensePlate", BindingFlags.NonPublic | BindingFlags.Instance);
plateField?.SetValue(car, "HACKED-99");
```

> [!TIP]
> У сучасному **.NET 8/9/10** для надшвидкого доступу до приватних полів без оверхеду рефлексії додано системний механізм **`[UnsafeAccessor]`**:
> ```csharp
> [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_licensePlate")]
> static extern ref string GetLicensePlate(Car car);
> ```

### 4.2. Довідник методів `FieldInfo` та `PropertyInfo`

| Клас | Метод / Властивість | Опис | Посилання на приклад |
| :--- | :--- | :--- | :---: |
| `FieldInfo` | `field.GetValue(object? obj)` | Повертає значення поля з об'єкта (для статичного — передається `null`). | [![Код](https://img.shields.io/badge/Код-07_GetField-blue?style=flat-square)](../07-GetFieldsPropertiesDemo/Program.cs#L14-L19) |
| `FieldInfo` | `field.SetValue(object? obj, object? val)` | Записує нове значення у поле в обхід інкапсуляції. | [![Код](https://img.shields.io/badge/Код-07_SetField-blue?style=flat-square)](../07-GetFieldsPropertiesDemo/Program.cs#L27-L36) |
| `FieldInfo` | `field.IsInitOnly` | `true`, якщо поле позначено ключовим словом `readonly`. | [![Код](https://img.shields.io/badge/Код-07_FieldProps-blue?style=flat-square)](../07-GetFieldsPropertiesDemo/Program.cs) |
| `PropertyInfo` | `prop.CanRead` / `CanWrite` | Визначає наявність відкритих або закритих методів `get` / `set`. | [![Код](https://img.shields.io/badge/Код-07_CanReadWrite-blue?style=flat-square)](../07-GetFieldsPropertiesDemo/Program.cs#L38-L46) |
| `PropertyInfo` | `prop.GetValue(obj)` | Викликає прихований метод геттера `get_PropertyName()`. | [![Код](https://img.shields.io/badge/Код-07_GetProp-blue?style=flat-square)](../07-GetFieldsPropertiesDemo/Program.cs#L41-L45) |
| `PropertyInfo` | `prop.SetValue(obj, val)` | Викликає прихований метод сеттера `set_PropertyName(val)`. | [![Код](https://img.shields.io/badge/Код-14_SetProp-blue?style=flat-square)](../14-SerializationDeserialization/XmlConverter.cs#L45-L51) |
| `PropertyInfo` | `prop.GetGetMethod(bool nonPublic)` | Отримує безпосередній об'єкт `MethodInfo` для геттера. | [![Код](https://img.shields.io/badge/Код-12_GetAccessor-blue?style=flat-square)](../12-GenericsReflectionDemo/Program.cs#L30-L33) |

---

## ⚡ 5. Інспекція та динамічний виклик методів (`MethodInfo`)

Клас `MethodInfo` представляє скомпільовану точку входу в метод у таблиці `MethodDef` або `MemberRef`.

### 5.1. Розв'язання перевантажень (Overload Resolution)
Якщо клас містить кілька методів з однаковим ім'ям, виклик `type.GetMethod("Name")` кине виняток **`AmbiguousMatchException`**! Для точного пошуку необхідно передавати сигнатуру типів параметрів:

```csharp
// Пошук конкретного перевантаження: Accelerate(int)
MethodInfo method = type.GetMethod("Accelerate", new[] { typeof(int) })!;
```

### 5.2. Довідник методів `MethodInfo`

| Метод / Властивість | Опис | Посилання на приклад |
| :--- | :--- | :---: |
| `type.GetMethods(...)` | Повертає список методів за прапорцями `BindingFlags`. | [![Код](https://img.shields.io/badge/Код-08_GetMethods-blue?style=flat-square)](../08-GetMethodsDemo/Program.cs#L11-L17) |
| `method.Invoke(target, args)` | Викликає метод на екземплярі `target` (або `null` для статичного). Аргументи передаються як `object[]`. | [![Код](https://img.shields.io/badge/Код-08_InvokeMethod-blue?style=flat-square)](../08-GetMethodsDemo/Program.cs#L22-L35) |
| `method.ReturnType` | Тип значення, що повертається методом (або `typeof(void)`). | [![Код](https://img.shields.io/badge/Код-09_ReturnType-blue?style=flat-square)](../09-GetParameterInfoDemo/Program.cs#L10-L16) |
| `method.IsStatic` | `true`, якщо метод є статичним і не вимагає екземпляра об'єкта. | [![Код](https://img.shields.io/badge/Код-08_IsStatic-blue?style=flat-square)](../08-GetMethodsDemo/Program.cs#L29-L35) |
| `method.IsVirtual` | `true`, якщо виклик методу диспетчеризується через таблицю віртуальних методів (`VTable`). | [![Код](https://img.shields.io/badge/Код-08_MethodsDemo-blue?style=flat-square)](../08-GetMethodsDemo/Program.cs) |

---

## 📥 6. Параметри методів: `System.Reflection.ParameterInfo`

Параметри описуються в таблиці метаданих `Param` (`0x08`). Клас `ParameterInfo` дозволяє дослідити кожен вхідний або вихідний аргумент.

| Властивість `ParameterInfo` | Опис та внутрішня семантика CLR | Посилання на приклад |
| :--- | :--- | :---: |
| `ParameterType` | Тип даних параметра. Для параметрів за посиланням повертає `ByRef`-тип (`T&`). | [![Код](https://img.shields.io/badge/Код-09_ParamType-blue?style=flat-square)](../09-GetParameterInfoDemo/Program.cs#L18-L25) |
| `Name` | Назва параметра в коді (наприклад, `"customerEmail"`). | [![Код](https://img.shields.io/badge/Код-09_ParamName-blue?style=flat-square)](../09-GetParameterInfoDemo/Program.cs#L22-L28) |
| `Position` | Порядковий індекс аргументу в сигнатурі (0-based). | [![Код](https://img.shields.io/badge/Код-09_ParamPos-blue?style=flat-square)](../09-GetParameterInfoDemo/Program.cs#L24-L29) |
| `IsOptional` | `true`, якщо параметр має значення за замовчуванням (може бути опущений при виклику). | [![Код](https://img.shields.io/badge/Код-09_IsOptional-blue?style=flat-square)](../09-GetParameterInfoDemo/Program.cs#L22-L27) |
| `DefaultValue` | Фізичне константне значення аргументу за замовчуванням, збережене в метаданих. | [![Код](https://img.shields.io/badge/Код-09_DefaultVal-blue?style=flat-square)](../09-GetParameterInfoDemo/Program.cs#L23-L27) |
| `ParameterType.IsByRef` | `true`, якщо параметр передається за посиланням (`ref`, `out`, `in`). | [![Код](https://img.shields.io/badge/Код-09_ByRef-blue?style=flat-square)](../09-GetParameterInfoDemo/Program.cs#L18-L23) |
| `IsOut` / `IsIn` | Уточнює семантику посилання: вихідний параметр (`out`) або вхідний тільки для читання (`in`). | [![Код](https://img.shields.io/badge/Код-09_OutIn-blue?style=flat-square)](../09-GetParameterInfoDemo/Program.cs#L19-L22) |
