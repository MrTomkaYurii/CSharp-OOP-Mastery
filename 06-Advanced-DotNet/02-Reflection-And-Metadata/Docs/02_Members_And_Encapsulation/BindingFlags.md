# Перерахування System.Reflection.BindingFlags

> **Простір імен**: `System.Reflection`  
> **Збірка**: `System.Private.CoreLib.dll`  
> **Офіційна документація**: [BindingFlags Enum (Microsoft Learn)](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.bindingflags)

`BindingFlags` — це бітова маска (`[Flags] enum`), що контролює процес пошуку та фільтрації членів типу (`GetMembers`, `GetMethods`, `GetProperties`, `GetFields`, `GetEvents`, `GetConstructors`) у рефлексії .NET.

---

## 1. Головне правило вибору прапорців пошуку

> [!IMPORTANT]
> Для успішного пошуку будь-якого члена обов'язково необхідно комбінувати **видимість** (`Public` або `NonPublic`) із **приналежністю** (`Instance` або `Static`).  
> Виклик `type.GetMethod("MyMethod", BindingFlags.Public)` поверне `null`, оскільки не вказано `Instance` чи `Static`!

```csharp
// Правильна комбінація для публічних методів екземпляра:
BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;

// Пошук усіх приватних та внутрішніх полів екземпляра:
BindingFlags privateFlags = BindingFlags.NonPublic | BindingFlags.Instance;

// Пошук абсолютно всіх членів (публічних, приватних, екземплярних і статичних):
BindingFlags allMembers = BindingFlags.Public | BindingFlags.NonPublic | 
                          BindingFlags.Instance | BindingFlags.Static;
```

---

## 2. Таблиця значень BindingFlags

| Прапорець | Числове значення | Опис призначення | Статус у курсі | Посилання на приклад | Посилання на MSDN |
| :--- | :---: | :--- | :---: | :--- | :--- |
| **`Default`** | `0` | Пошук без прапорців (за замовчуванням) | Загальний BCL | Довідник | [BindingFlags.Default](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.bindingflags) |
| **`IgnoreCase`** | `1` | Ігнорування регістру символів у назві члена | Загальний BCL | Довідник | [BindingFlags.IgnoreCase](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.bindingflags) |
| **`DeclaredOnly`** | `2` | Пошук тільки членів, оголошених на рівні поточного типу (ігнорує успадковані) | Використано | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L22) | [BindingFlags.DeclaredOnly](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.bindingflags) |
| **`Instance`** | `4` | Включає в пошук нестатичні члени екземпляра | Використано | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L16) | [BindingFlags.Instance](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.bindingflags) |
| **`Static`** | `8` | Включає в пошук статичні члени класу | Використано | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L16) | [BindingFlags.Static](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.bindingflags) |
| **`Public`** | `16` | Включає в пошук відкриті (`public`) члени | Використано | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L16) | [BindingFlags.Public](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.bindingflags) |
| **`NonPublic`** | `32` | Включає в пошук закриті (`private`), захищені (`protected`) та внутрішні (`internal`) члени | Використано | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L16) | [BindingFlags.NonPublic](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.bindingflags) |
| **`FlattenHierarchy`** | `64` | Включає в пошук публічні та захищені статичні члени по всьому дереву успадкування | Загальний BCL | Довідник | [BindingFlags.FlattenHierarchy](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.bindingflags) |
