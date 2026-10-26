# Клас System.Reflection.ParameterInfo: Інспекція параметрів

> **Простір імен**: `System.Reflection`  
> **Збірка**: `System.Private.CoreLib.dll`  
> **Базовий тип**: `System.Object`  
> **Офіційна документація**: [ParameterInfo Class (Microsoft Learn)](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo)

Клас `ParameterInfo` містить вичерпні метадані про один вхідний або вихідний аргумент методу, конструктора чи властивості-індексатора (тип, назву, позицію, модифікатори `ref`/`out`/`in` та значення за замовчуванням).

---

## 1. Роль ParameterInfo в Dependency Injection

Під час автоматичного розв'язання залежностей контейнер інспектує кожен параметр конструктора:

```csharp
foreach (ParameterInfo param in constructor.GetParameters())
{
    Type dependencyType = param.ParameterType;
    object resolvedInstance = container.Resolve(dependencyType);
}
```

---

## 2. Властивості класу ParameterInfo (Properties)

| Властивість | Сигнатура | Опис призначення | Статус у курсі | Приклад у коді | Документація MSDN |
| :--- | :--- | :--- | :---: | :--- | :--- |
| **`Name`** | `string? Name { get; }` | Назва параметра, як вона визначена у вихідному коді | Використано | [09-GetParameterInfoDemo/Program.cs](../../09-GetParameterInfoDemo/Program.cs#L26) | [ParameterInfo.Name](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.name) |
| **`ParameterType`** | `Type ParameterType { get; }` | Тип даних параметра (для `ref/out` містить `&`, наприклад `int&`) | Використано | [09-GetParameterInfoDemo/Program.cs](../../09-GetParameterInfoDemo/Program.cs#L28) | [ParameterInfo.ParameterType](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.parametertype) |
| **`Position`** | `int Position { get; }` | Нульова позиція параметра у сигнатурі методу | Використано | [09-GetParameterInfoDemo/Program.cs](../../09-GetParameterInfoDemo/Program.cs#L30) | [ParameterInfo.Position](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.position) |
| **`IsIn`** | `bool IsIn { get; }` | Визначає, чи передається параметр лише для читання з модифікатором `in` | Використано | [09-GetParameterInfoDemo/Program.cs](../../09-GetParameterInfoDemo/Program.cs#L38) | [ParameterInfo.IsIn](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.isin) |
| **`IsOut`** | `bool IsOut { get; }` | Визначає, чи є параметр вихідним з модифікатором `out` | Використано | [09-GetParameterInfoDemo/Program.cs](../../09-GetParameterInfoDemo/Program.cs#L40) | [ParameterInfo.IsOut](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.isout) |
| **`HasDefaultValue`** | `bool HasDefaultValue { get; }` | Чи має параметр опціональне значення за замовчуванням | Використано | [09-GetParameterInfoDemo/Program.cs](../../09-GetParameterInfoDemo/Program.cs#L48) | [ParameterInfo.HasDefaultValue](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.hasdefaultvalue) |
| **`DefaultValue`** | `object? DefaultValue { get; }` | Літеральне значення за замовчуванням для опціонального параметра | Використано | [09-GetParameterInfoDemo/Program.cs](../../09-GetParameterInfoDemo/Program.cs#L50) | [ParameterInfo.DefaultValue](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.defaultvalue) |
| **`Member`** | `MemberInfo Member { get; }` | Батьківський метод або конструктор, якому належить цей параметр | Загальний BCL | Довідник | [ParameterInfo.Member](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.member) |
| **`IsOptional`** | `bool IsOptional { get; }` | Визначає, чи є параметр опціональним у виклику | Загальний BCL | Довідник | [ParameterInfo.IsOptional](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.isoptional) |

---

## 3. Методи класу ParameterInfo (Methods)

| Метод | Сигнатура | Опис призначення | Статус у курсі | Приклад у коді | Документація MSDN |
| :--- | :--- | :--- | :---: | :--- | :--- |
| **`GetCustomAttributes`** | `object[] GetCustomAttributes(bool inherit)` | Повертає масив атрибутів, застосованих безпосередньо до цього параметра | Загальний BCL | Довідник | [ParameterInfo.GetCustomAttributes](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.getcustomattributes) |
| **`GetCustomAttributesData`** | `IList<CustomAttributeData> GetCustomAttributesData()` | Повертає метадані атрибутів параметра без інстанціювання їхніх класів | Загальний BCL | Довідник | [ParameterInfo.GetCustomAttributesData](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.getcustomattributesdata) |
| **`IsDefined`** | `bool IsDefined(Type attributeType, bool inherit)` | Перевіряє наявність заданого типу атрибута на параметрі | Загальний BCL | Довідник | [ParameterInfo.IsDefined](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.isdefined) |
