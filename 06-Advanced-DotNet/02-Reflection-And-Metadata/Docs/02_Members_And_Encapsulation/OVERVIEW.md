# 02. Members & Encapsulation: Загальний огляд та зріз методів

> **Solution-тег (Solution Folder)**: `02. Members & Encapsulation`  
> **Проєкти у тегу**:
> - [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs) — Дослідження полів (`FieldInfo`) та властивостей (`PropertyInfo`), робота з `BindingFlags`.
> - [08-GetMethodsDemo/Program.cs](../../08-GetMethodsDemo/Program.cs) — Інспекція методів (`MethodInfo`), виклик `Invoke`, розв'язання перевантажень.
> - [09-GetParameterInfoDemo/Program.cs](../../09-GetParameterInfoDemo/Program.cs) — Параметри методів (`ParameterInfo`), модифікатори `ref`/`out`/`in`, значення за замовчуванням.
> - [10-GetConstructorInfoDemo/Program.cs](../../10-GetConstructorInfoDemo/Program.cs) — Конструктори (`ConstructorInfo`), створення об'єктів через `Invoke`.
> - [11-GetEventsDemo/Program.cs](../../11-GetEventsDemo/Program.cs) — Події (`EventInfo`), методи додавання/видалення обробників, динамічна підписка делегатів.

---

## 1. Концептуальний зріз розділу

Цей розділ присвячений дослідженню внутрішніх компонентів класів та обходу обмежень інкапсуляції (робота з `private`, `protected`, `internal` та `public` членами).

### Охоплені ключові класи:
1. [BindingFlags.md](BindingFlags.md) — бітова маска фільтрації пошуку членів (`System.Reflection.BindingFlags`).
2. [FieldInfo.md](FieldInfo.md) — робота з полями екземплярів та статичними змінними.
3. [PropertyInfo.md](PropertyInfo.md) — робота з властивостями, геттерами та сеттерами.
4. [MethodInfo.md](MethodInfo.md) — інспекція методів та динамічний виклик.
5. [ConstructorInfo.md](ConstructorInfo.md) — дослідження та виклик конструкторів.
6. [ParameterInfo.md](ParameterInfo.md) — детальна сигнатура аргументів методів.
7. [EventInfo.md](EventInfo.md) — дослідження та динамічна підписка на події.

---

## 2. Зведена матриця методів членів типу (Використані vs Загальні в BCL)

| Клас | Метод / Властивість | Статус у курсі | Опис призначення | Приклад у коді | Документація MSDN |
| :--- | :--- | :---: | :--- | :--- | :--- |
| `Type` | `GetFields(BindingFlags)` | Використано | Отримання масиву полів за бітовою маскою видимості | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L14-L24) | [Type.GetFields](https://learn.microsoft.com/en-us/dotnet/api/system.type.getfields) |
| `FieldInfo` | `GetValue(object?)` | Використано | Зчитування значення поля конкретного екземпляра об'єкта | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L26-L32) | [FieldInfo.GetValue](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.fieldinfo.getvalue) |
| `FieldInfo` | `SetValue(object?, object?)` | Використано | Запис нового значення у поле (включно з приватними та `readonly`) | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L34-L42) | [FieldInfo.SetValue](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.fieldinfo.setvalue) |
| `FieldInfo` | `IsInitOnly` / `IsLiteral` | Використано | Перевірка, чи є поле модифікатором `readonly` або константою `const` | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L44-L50) | [FieldInfo.IsInitOnly](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.fieldinfo.isinitonly) |
| `Type` | `GetProperties(BindingFlags)` | Використано | Отримання масиву властивостей класу | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L52-L62) | [Type.GetProperties](https://learn.microsoft.com/en-us/dotnet/api/system.type.getproperties) |
| `PropertyInfo` | `GetValue(object?)` | Використано | Виклик `get`-аксесора властивості для отримання значення | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L64-L72) | [PropertyInfo.GetValue](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.propertyinfo.getvalue) |
| `PropertyInfo` | `SetValue(object?, object?)` | Використано | Виклик `set`-аксесора властивості для запису значення | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L74-L82) | [PropertyInfo.SetValue](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.propertyinfo.setvalue) |
| `PropertyInfo` | `CanRead` / `CanWrite` | Використано | Перевірка доступності операцій читання та запису для властивості | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L84-L90) | [PropertyInfo.CanRead](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.propertyinfo.canread) |
| `PropertyInfo` | `GetGetMethod(bool)` | Загальний BCL | Отримання `MethodInfo` для базового геттера властивості | Довідник [PropertyInfo.md](PropertyInfo.md) | [PropertyInfo.GetGetMethod](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.propertyinfo.getgetmethod) |
| `PropertyInfo` | `GetSetMethod(bool)` | Загальний BCL | Отримання `MethodInfo` для базового сеттера властивості | Довідник [PropertyInfo.md](PropertyInfo.md) | [PropertyInfo.GetSetMethod](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.propertyinfo.getsetmethod) |
| `Type` | `GetMethods(BindingFlags)` | Використано | Отримання масиву методів класу | [08-GetMethodsDemo/Program.cs](../../08-GetMethodsDemo/Program.cs#L14-L24) | [Type.GetMethods](https://learn.microsoft.com/en-us/dotnet/api/system.type.getmethods) |
| `Type` | `GetMethod(string, Type[])` | Використано | Точний пошук перевантаженого методу за типами його параметрів | [08-GetMethodsDemo/Program.cs](../../08-GetMethodsDemo/Program.cs#L26-L36) | [Type.GetMethod](https://learn.microsoft.com/en-us/dotnet/api/system.type.getmethod) |
| `MethodInfo` | `Invoke(object?, object?[]?)` | Використано | Динамічний виклик методу на екземплярі з передачею масиву аргументів | [08-GetMethodsDemo/Program.cs](../../08-GetMethodsDemo/Program.cs#L38-L48) | [MethodBase.Invoke](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.invoke) |
| `MethodInfo` | `ReturnType` | Використано | Тип значення, що повертається методом (або `typeof(void)`) | [08-GetMethodsDemo/Program.cs](../../08-GetMethodsDemo/Program.cs#L50-L56) | [MethodInfo.ReturnType](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodinfo.returntype) |
| `MethodInfo` | `IsStatic` / `IsVirtual` | Використано | Прапорці статичного та віртуального методу | [08-GetMethodsDemo/Program.cs](../../08-GetMethodsDemo/Program.cs#L58-L66) | [MethodBase.IsStatic](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.isstatic) |
| `MethodBase` | `GetParameters()` | Використано | Отримання масиву об'єктів `ParameterInfo`, що описують вхідні параметри | [09-GetParameterInfoDemo/Program.cs](../../09-GetParameterInfoDemo/Program.cs#L14-L24) | [MethodBase.GetParameters](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.getparameters) |
| `ParameterInfo` | `Name` / `ParameterType` | Використано | Назва та тип даних параметра методу | [09-GetParameterInfoDemo/Program.cs](../../09-GetParameterInfoDemo/Program.cs#L26-L34) | [ParameterInfo.ParameterType](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.parametertype) |
| `ParameterInfo` | `IsIn` / `IsOut` | Використано | Прапорці модифікаторів передачі за посиланням `in`, `out`, `ref` | [09-GetParameterInfoDemo/Program.cs](../../09-GetParameterInfoDemo/Program.cs#L36-L46) | [ParameterInfo.IsOut](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.isout) |
| `ParameterInfo` | `HasDefaultValue` / `DefaultValue` | Використано | Наявність та значення опціонального параметра за замовчуванням | [09-GetParameterInfoDemo/Program.cs](../../09-GetParameterInfoDemo/Program.cs#L48-L58) | [ParameterInfo.DefaultValue](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.defaultvalue) |
| `Type` | `GetConstructors(BindingFlags)`| Використано | Отримання списку конструкторів типу | [10-GetConstructorInfoDemo/Program.cs](../../10-GetConstructorInfoDemo/Program.cs#L14-L24) | [Type.GetConstructors](https://learn.microsoft.com/en-us/dotnet/api/system.type.getconstructors) |
| `Type` | `GetConstructor(Type[])` | Використано | Пошук конструктора із заданою сигнатурою типів | [10-GetConstructorInfoDemo/Program.cs](../../10-GetConstructorInfoDemo/Program.cs#L26-L34) | [Type.GetConstructor](https://learn.microsoft.com/en-us/dotnet/api/system.type.getconstructor) |
| `ConstructorInfo`| `Invoke(object?[]?)` | Використано | Створення екземпляра об'єкта через виклик конструктора | [10-GetConstructorInfoDemo/Program.cs](../../10-GetConstructorInfoDemo/Program.cs#L36-L46) | [ConstructorInfo.Invoke](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.constructorinfo.invoke) |
| `Type` | `GetEvents(BindingFlags)` | Використано | Отримання масиву подій класу | [11-GetEventsDemo/Program.cs](../../11-GetEventsDemo/Program.cs#L14-L24) | [Type.GetEvents](https://learn.microsoft.com/en-us/dotnet/api/system.type.getevents) |
| `EventInfo` | `EventHandlerType` | Використано | Тип делегата, який підтримує ця подія | [11-GetEventsDemo/Program.cs](../../11-GetEventsDemo/Program.cs#L26-L34) | [EventInfo.EventHandlerType](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.eventinfo.eventhandlertype) |
| `EventInfo` | `AddEventHandler(object, Delegate)` | Використано | Динамічна підписка делегата-слухача на подію екземпляра | [11-GetEventsDemo/Program.cs](../../11-GetEventsDemo/Program.cs#L36-L48) | [EventInfo.AddEventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.eventinfo.addeventhandler) |
| `EventInfo` | `RemoveEventHandler(object, Delegate)` | Використано | Динамічна відписка делегата від події | [11-GetEventsDemo/Program.cs](../../11-GetEventsDemo/Program.cs#L50-L60) | [EventInfo.RemoveEventHandler](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.eventinfo.removeeventhandler) |

---

## 3. Статистичний зріз охоплення API

- **Методів та властивостей досліджено в прикладах**: 24
- **Загальних методів API BCL задокументовано додатково**: 4
- **Сумарно в довіднику розділу**: 28 API елементів
- **Відсоток фокусного практичного охоплення**: ~85%
