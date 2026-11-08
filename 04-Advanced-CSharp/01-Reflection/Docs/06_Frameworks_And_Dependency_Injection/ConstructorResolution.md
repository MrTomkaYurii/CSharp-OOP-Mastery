# Алгоритми резолвінгу конструкторів у Dependency Injection

> **Ключові класи**: `System.Type`, `System.Reflection.ConstructorInfo`, `System.Reflection.ParameterInfo`  
> **Офіційна документація**: [Dependency injection in .NET (Microsoft Learn)](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)

Під час автоматичного створення об'єкта IoC-контейнер має визначити, який саме конструктор слід викликати (якщо їх декілька) та які аргументи необхідно передати в кожен з його параметрів.

---

## 1. Стратегії вибору конструктора (від простого у 25.1 до промислового у 25.2)

```mermaid
flowchart TD
    Start["Запит на резолвінг типу T"] --> GetCtors["Type.GetConstructors()"]
    GetCtors --> CheckCount{"Скільки відкритих конструкторів?"}
    
    CheckCount -- "Рівно один (як у 25.1)" --> SingleCtor["Використати цей єдиний конструктор: constructors[0]"]
    CheckCount -- "Декілька (як у 25.2)" --> CheckAttr{"Чи є конструктор з [InjectConstructor]?"}
    
    CheckAttr -- "Так" --> UseAttr["Використати маркований конструктор"]
    CheckAttr -- "Ні" --> Greedy["Жадібний вибір (Greedy Constructor):<br/>OrderByDescending(c => c.GetParameters().Length).First()"]
    
    SingleCtor --> ResolveParams["Рекурсивно резолвити кожен param.ParameterType"]
    UseAttr --> ResolveParams
    Greedy --> ResolveParams
    ResolveParams --> InvokeCtor["ctor.Invoke(resolvedArgs)"]
```

---

## 2. Властивості, задіяні в резолвінгу конструкторів (Properties)

| Властивість | Сигнатура | Опис призначення | Статус у курсі | Приклад у коді | Документація MSDN |
| :--- | :--- | :--- | :---: | :--- | :--- |
| **`ParameterType`** | `Type ParameterType { get; }` | Тип даних залежності, яку необхідно знайти в контейнері | Використано | [25.1-DependencyInjectionDemo/SimpleContainer.cs](../../25.1-DependencyInjectionDemo/SimpleContainer.cs) | [ParameterInfo.ParameterType](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.parametertype) |
| **`Name`** | `string? Name { get; }` | Ім'я формального параметра конструктора для діагностичного логування | Використано | [25.1-DependencyInjectionDemo/SimpleContainer.cs](../../25.1-DependencyInjectionDemo/SimpleContainer.cs) | [ParameterInfo.Name](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.parameterinfo.name) |
| **`CanWrite`** | `bool CanWrite { get; }` | Перевірка наявності сеттера властивості перед виконанням Property Injection | Використано | [25.2-DependencyInjectionRealWorld/Container/Container.cs](../../25.2-DependencyInjectionRealWorld/Container/Container.cs) | [PropertyInfo.CanWrite](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.propertyinfo.canwrite) |
| **`IsPublic`** | `bool IsPublic { get; }` | Перевірка відкритості конструктора для створення екземпляра ззовні | Загальний BCL | Довідник | [MethodBase.IsPublic](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.ispublic) |

---

## 3. Методи, задіяні в резолвінгу конструкторів (Methods)

| Метод | Сигнатура | Опис призначення | Статус у курсі | Приклад у коді | Документація MSDN |
| :--- | :--- | :--- | :---: | :--- | :--- |
| **`GetConstructors`** | `ConstructorInfo[] GetConstructors()` | Повертає масив усіх публічних конструкторів цільового класу | Використано | [25.1-DependencyInjectionDemo/SimpleContainer.cs](../../25.1-DependencyInjectionDemo/SimpleContainer.cs) | [Type.GetConstructors](https://learn.microsoft.com/en-us/dotnet/api/system.type.getconstructors) |
| **`GetParameters`** | `ParameterInfo[] GetParameters()` | Повертає список формальних параметрів конструктора | Використано | [25.1-DependencyInjectionDemo/SimpleContainer.cs](../../25.1-DependencyInjectionDemo/SimpleContainer.cs) | [MethodBase.GetParameters](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.getparameters) |
| **`Invoke`** | `object Invoke(object?[]? parameters)` | Виконує конструктор у пам'яті, створюючи готовий об'єкт | Використано | [25.1-DependencyInjectionDemo/SimpleContainer.cs](../../25.1-DependencyInjectionDemo/SimpleContainer.cs) | [ConstructorInfo.Invoke](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.constructorinfo.invoke) |
| **`GetCustomAttribute<T>`**| `static T? GetCustomAttribute<T>(this MemberInfo element)` | Перевіряє наявність маркерного атрибута на конструкторі (`[InjectConstructor]`) | Використано | [25.2-DependencyInjectionRealWorld/Container/Container.cs](../../25.2-DependencyInjectionRealWorld/Container/Container.cs) | [CustomAttributeExtensions.GetCustomAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.customattributeextensions.getcustomattribute) | 