# Клас System.Reflection.ConstructorInfo: Інспекція та виклик конструкторів

> **Простір імен**: `System.Reflection`  
> **Збірка**: `System.Private.CoreLib.dll`  
> **Базовий тип**: `System.Reflection.MethodBase : System.Reflection.MemberInfo`  
> **Офіційна документація**: [ConstructorInfo Class (Microsoft Learn)](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.constructorinfo)

Клас `ConstructorInfo` представляє метадані конструктора типу (`.ctor` для екземплярних або `.cctor` для статичних ініціалізаторів). Успадковуючи `MethodBase`, він слугує фундаментом для патернів фабрик об'єктів та інверсії керування (IoC/DI).

---

## 1. Створення об'єкта через ConstructorInfo.Invoke

```csharp
// Пошук конструктора: Car(string color, int speed)
ConstructorInfo? ctor = typeof(Car).GetConstructor(new[] { typeof(string), typeof(int) });

// Інстанціювання об'єкта:
object instance = ctor!.Invoke(new object[] { "Зелений", 80 });
Car greenCar = (Car)instance;
```

---

## 2. Властивості класу ConstructorInfo (Properties)

| Властивість | Сигнатура | Опис призначення | Статус у курсі | Приклад у коді | Документація MSDN |
| :--- | :--- | :--- | :---: | :--- | :--- |
| **`IsPublic`** | `bool IsPublic { get; }` | Визначає, чи є конструктор відкритим (`MethodBase`) | Використано | [10-GetConstructorInfoDemo/Program.cs](../../10-GetConstructorInfoDemo/Program.cs#L24) | [MethodBase.IsPublic](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.ispublic) |
| **`IsPrivate`** | `bool IsPrivate { get; }` | Визначає, чи є конструктор закритим (`MethodBase`) | Використано | [10-GetConstructorInfoDemo/Program.cs](../../10-GetConstructorInfoDemo/Program.cs#L26) | [MethodBase.IsPrivate](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.isprivate) |
| **`IsStatic`** | `bool IsStatic { get; }` | Визначає, чи є конструктор статичним конструктором типу (`.cctor`) | Загальний BCL | Довідник | [MethodBase.IsStatic](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.isstatic) |

---

## 3. Методи класу ConstructorInfo (Methods)

| Метод | Сигнатура | Опис призначення | Статус у курсі | Приклад у коді | Документація MSDN |
| :--- | :--- | :--- | :---: | :--- | :--- |
| **`Invoke`** | `object Invoke(object?[]? parameters)` | Створює та ініціалізує новий екземпляр типу, виконуючи цей конструктор з переданими аргументами | Використано | [10-GetConstructorInfoDemo/Program.cs](../../10-GetConstructorInfoDemo/Program.cs#L36) | [ConstructorInfo.Invoke](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.constructorinfo.invoke) |
| **`GetParameters`** | `ParameterInfo[] GetParameters()` | Повертає список типів та назв аргументів, необхідних для виклику конструктора | Використано | [10-GetConstructorInfoDemo/Program.cs](../../10-GetConstructorInfoDemo/Program.cs#L22) | [MethodBase.GetParameters](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.getparameters) |
