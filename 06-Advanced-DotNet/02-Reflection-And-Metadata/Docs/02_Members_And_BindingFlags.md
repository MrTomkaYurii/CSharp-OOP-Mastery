# 02. Члени типу, параметри та бітові прапорці BindingFlags (Members & BindingFlags)

Усі складові елементи типу в .NET є похідними від абстрактного класу `System.Reflection.MemberInfo`. Щоб отримати доступ до потрібного члена, необхідно чітко розуміти роботу фільтруючої маски `BindingFlags`.

---

## 🎛️ Бітові прапорці `System.Reflection.BindingFlags`

Методи пошуку членів (`GetMethod`, `GetProperty`, `GetField`, `GetConstructor`) за замовчуванням повертають тільки **публічні екземплярні члени** (`BindingFlags.Public | BindingFlags.Instance`). Якщо вам потрібно знайти статичний, приватний або захищений член — обов'язково треба вказати відповідну комбінацію прапорців.

### Ключові прапорці та їхнє значення:

| Прапорець | Бітове значення | Призначення |
| :--- | :---: | :--- |
| `BindingFlags.Public` | `0x10` | Включає у пошук відкриті (`public`) члени. |
| `BindingFlags.NonPublic` | `0x20` | Включає у пошук закриті (`private`, `protected`, `internal`) члени. |
| `BindingFlags.Instance` | `0x04` | Включає члени екземпляра (потребують створеного об'єкта для виклику). |
| `BindingFlags.Static` | `0x08` | Включає статичні члени (не прив'язані до екземпляра). |
| `BindingFlags.DeclaredOnly` | `0x02` | Шукає члени, оголошені **виключно в цьому типі**, ігноруючи наслідування (наприклад, відкидає методи `ToString()`, `GetHashCode()` від `object`). |
| `BindingFlags.FlattenHierarchy` | `0x40` | Включає відкриті та захищені статичні члени вгору по ієрархії наслідування. |
| `BindingFlags.IgnoreCase` | `0x01` | Ігнорує регістр символів у назві шуканого члена. |

> [!IMPORTANT]
> Для успішного пошуку завжди комбінуйте прапорець видимості (`Public` або `NonPublic`) із прапорцем контексту (`Instance` або `Static`). Наприклад: `BindingFlags.NonPublic | BindingFlags.Instance`.

---

## 🏗️ 1. Конструктори (`System.Reflection.ConstructorInfo`)

Клас `ConstructorInfo` надає доступ до сигнатур конструкторів та дозволяє динамічно інстанціювати об'єкти.

| Метод / Властивість | Опис | Приклад у коді |
| :--- | :--- | :---: |
| `type.GetConstructors()` | Повертає відкриті конструктори класу | [![Код](https://img.shields.io/badge/Код-GetConstructors-blue?style=flat-square)](../GetConstractorInfoDemo/Program.cs#L10-L16) |
| `type.GetConstructor(Type[])` | Шукає конструктор з точним набором типів аргументів | [![Код](https://img.shields.io/badge/Код-FindCtor-blue?style=flat-square)](../GetConstractorInfoDemo/Program.cs#L24-L28) |
| `ctor.Invoke(object?[] args)` | Викликає конструктор і повертає новий екземпляр | [![Код](https://img.shields.io/badge/Код-InvokeCtor-blue?style=flat-square)](../GetConstractorInfoDemo/Program.cs#L27-L31) |
| `ctor.IsPublic` / `IsPrivate` | Перевіряє модифікатор доступу конструктора | [![Код](https://img.shields.io/badge/Код-CtorVisibility-blue?style=flat-square)](../GetConstractorInfoDemo/Program.cs#L43-L49) |

---

## 🏷️ 2. Поля та властивості (`FieldInfo` та `PropertyInfo`)

Поля представляють фізичні комірки пам'яті, а властивості — пару методів-аксесорів (`get_Name` / `set_Name`).

| Член | Метод / Властивість | Призначення | Посилання на приклад |
| :--- | :--- | :--- | :---: |
| **FieldInfo** | `field.GetValue(object? obj)` | Зчитує значення поля з об'єкта | [![Код](https://img.shields.io/badge/Код-GetField-blue?style=flat-square)](../GetFieldsPropertiesDemo/Program.cs#L14-L19) |
| **FieldInfo** | `field.SetValue(obj, value)` | Записує нове значення у поле (включно з приватними полями) | [![Код](https://img.shields.io/badge/Код-SetField-blue?style=flat-square)](../GetFieldsPropertiesDemo/Program.cs#L27-L36) |
| **FieldInfo** | `field.FieldType` | Тип даних, що зберігається в полі | [![Код](https://img.shields.io/badge/Код-FieldType-blue?style=flat-square)](../GetFieldsPropertiesDemo/Program.cs#L16-L18) |
| **PropertyInfo**| `prop.CanRead` / `CanWrite` | Чи доступні операції читання та запису | [![Код](https://img.shields.io/badge/Код-CanReadWrite-blue?style=flat-square)](../GetFieldsPropertiesDemo/Program.cs#L38-L46) |
| **PropertyInfo**| `prop.GetValue(obj)` | Викликає прихований метод геттера `get_PropertyName()` | [![Код](https://img.shields.io/badge/Код-GetProp-blue?style=flat-square)](../GetFieldsPropertiesDemo/Program.cs#L41-L45) |
| **PropertyInfo**| `prop.SetValue(obj, val)` | Викликає прихований метод сеттера `set_PropertyName(val)` | [![Код](https://img.shields.io/badge/Код-SetProp-blue?style=flat-square)](../SerializationDeserialization/XmlConverter.cs#L45-L51) |
| **PropertyInfo**| `prop.GetGetMethod(bool)` | Отримує безпосередній `MethodInfo` геттера (навіть непублічного) | [![Код](https://img.shields.io/badge/Код-GetAccessor-blue?style=flat-square)](../GenericsReflectionDemo/Program.cs#L30-L33) |

---

## ⚡ 3. Методи (`System.Reflection.MethodInfo`)

`MethodInfo` дозволяє досліджувати сигнатуру будь-якого методу та динамічно виконувати його через `Invoke()`.

| Метод / Властивість | Призначення | Посилання на приклад |
| :--- | :--- | :---: |
| `type.GetMethods(...)` | Повертає список методів відповідно до маски `BindingFlags` | [![Код](https://img.shields.io/badge/Код-GetMethods-blue?style=flat-square)](../GetMethodsDemo/Program.cs#L11-L17) |
| `type.GetMethod(name, Type[])` | Знаходить точне перевантаження методу за типами аргументів | [![Код](https://img.shields.io/badge/Код-FindOverload-blue?style=flat-square)](../GetMethodsDemo/Program.cs#L19-L27) |
| `method.Invoke(target, args)` | Викликає метод на цільовому об'єкті (або `null` для static) | [![Код](https://img.shields.io/badge/Код-InvokeMethod-blue?style=flat-square)](../GetMethodsDemo/Program.cs#L22-L35) |
| `method.ReturnType` | Тип значення, що повертає метод (або `typeof(void)`) | [![Код](https://img.shields.io/badge/Код-ReturnType-blue?style=flat-square)](../GetParameterInfoDemo/Program.cs#L10-L16) |
| `method.IsStatic` | `true`, якщо метод є статичним | [![Код](https://img.shields.io/badge/Код-IsStatic-blue?style=flat-square)](../GetMethodsDemo/Program.cs#L29-L35) |

---

## 📥 4. Параметри методів (`System.Reflection.ParameterInfo`)

Клас `ParameterInfo` дозволяє з'ясувати все про аргументи, що передаються у конструктор або метод:

| Властивість `ParameterInfo` | Опис | Посилання на приклад |
| :--- | :--- | :---: |
| `ParameterType` | Тип даних параметра (наприклад, `typeof(int)`) | [![Код](https://img.shields.io/badge/Код-ParamType-blue?style=flat-square)](../GetParameterInfoDemo/Program.cs#L18-L25) |
| `Name` | Назва аргументу в коді (наприклад, `"orderId"`) | [![Код](https://img.shields.io/badge/Код-ParamName-blue?style=flat-square)](../GetParameterInfoDemo/Program.cs#L22-L28) |
| `Position` | Порядковий індекс у списку аргументів (починаючи з 0) | [![Код](https://img.shields.io/badge/Код-ParamPos-blue?style=flat-square)](../GetParameterInfoDemo/Program.cs#L24-L29) |
| `IsOptional` | Чи є аргумент необов'язковим (має значення за замовчуванням) | [![Код](https://img.shields.io/badge/Код-IsOptional-blue?style=flat-square)](../GetParameterInfoDemo/Program.cs#L22-L27) |
| `DefaultValue` | Конкретне дефолтне значення аргументу | [![Код](https://img.shields.io/badge/Код-DefaultVal-blue?style=flat-square)](../GetParameterInfoDemo/Program.cs#L23-L27) |
| `ParameterType.IsByRef` | Чи передається параметр за посиланням (`ref`, `out`, `in`) | [![Код](https://img.shields.io/badge/Код-ByRef-blue?style=flat-square)](../GetParameterInfoDemo/Program.cs#L18-L23) |
| `IsOut` / `IsIn` | Уточнює напрямок передачі посилання (`out` чи `in`) | [![Код](https://img.shields.io/badge/Код-OutIn-blue?style=flat-square)](../GetParameterInfoDemo/Program.cs#L19-L22) |
