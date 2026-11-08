# Клас System.Reflection.MethodInfo: Інспекція та виклик методів

> **Простір імен**: `System.Reflection`  
> **Збірка**: `System.Private.CoreLib.dll`  
> **Базовий тип**: `System.Reflection.MethodBase : System.Reflection.MemberInfo`  
> **Офіційна документація**: [MethodInfo Class (Microsoft Learn)](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodinfo)

Клас `MethodInfo` представляє метадані методу програми в .NET. Успадковуючи проміжний клас `MethodBase` (який також є базовим для `ConstructorInfo`), він містить інформацію про сигнатуру, модифікатори доступу, повернене значення та дозволяє виконувати динамічний виклик коду.

---

## 1. Динамічний виклик методу (Method Invocation)

```csharp
MethodInfo? method = typeof(Car).GetMethod("Accelerate", new[] { typeof(int) });

// Виклик методу: екземпляр передається першим аргументом,
// параметри методу пакуються в масив object[]:
object? result = method?.Invoke(myCar, new object[] { 40 });
```

---

## 2. Властивості класу MethodInfo (Properties)

| Властивість | Сигнатура | Опис призначення | Статус у курсі | Приклад у коді | Документація MSDN |
| :--- | :--- | :--- | :---: | :--- | :--- |
| **`ReturnType`** | `Type ReturnType { get; }` | Тип значення, що повертає метод (або `System.Void` для процедур) | Використано | [08-GetMethodsDemo/Program.cs](../../08-GetMethodsDemo/Program.cs#L50) | [MethodInfo.ReturnType](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodinfo.returntype) |
| **`IsStatic`** | `bool IsStatic { get; }` | Визначає, чи є метод статичним (`MethodBase`) | Використано | [08-GetMethodsDemo/Program.cs](../../08-GetMethodsDemo/Program.cs#L60) | [MethodBase.IsStatic](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.isstatic) |
| **`IsVirtual`** | `bool IsVirtual { get; }` | Визначає, чи є метод віртуальним та доступним для перевизначення | Використано | [08-GetMethodsDemo/Program.cs](../../08-GetMethodsDemo/Program.cs#L62) | [MethodBase.IsVirtual](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.isvirtual) |
| **`IsAbstract`** | `bool IsAbstract { get; }` | Визначає, чи є метод абстрактним (без реалізації тіла) | Загальний BCL | Довідник | [MethodBase.IsAbstract](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.isabstract) |

---

## 3. Методи класу MethodInfo (Methods)

| Метод | Сигнатура | Опис призначення | Статус у курсі | Приклад у коді | Документація MSDN |
| :--- | :--- | :--- | :---: | :--- | :--- |
| **`Invoke`** | `object? Invoke(object? obj, object?[]? parameters)` | Динамічно виконує метод на цільовому об'єкті з передачею аргументів | Використано | [08-GetMethodsDemo/Program.cs](../../08-GetMethodsDemo/Program.cs#L40) | [MethodBase.Invoke](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.invoke) |
| **`GetParameters`** | `ParameterInfo[] GetParameters()` | Повертає масив інформації про вхідні та вихідні параметри методу | Використано | [08-GetMethodsDemo/Program.cs](../../08-GetMethodsDemo/Program.cs#L32) | [MethodBase.GetParameters](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.getparameters) |
| **`GetBaseDefinition`** | `MethodInfo GetBaseDefinition()` | Повертає дескриптор методу з вихідного кореневого базового класу | Загальний BCL | Довідник | [MethodInfo.GetBaseDefinition](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodinfo.getbasedefinition) |
| **`CreateDelegate`** | `Delegate CreateDelegate(Type delegateType, object? target)` | Створює типізований делегат до методу для високошвидкісного повторного виклику | Загальний BCL | Довідник | [MethodInfo.CreateDelegate](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodinfo.createdelegate) |
