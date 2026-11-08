# Клас System.Reflection.FieldInfo: Дослідження полів

> **Простір імен**: `System.Reflection`  
> **Збірка**: `System.Private.CoreLib.dll`  
> **Базовий тип**: `System.Reflection.MemberInfo`  
> **Офіційна документація**: [FieldInfo Class (Microsoft Learn)](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.fieldinfo)

Клас `FieldInfo` представляє метадані поля класу або структури (як екземплярного, так і статичного), успадковуючи базовий функціонал `MemberInfo` (`Name`, `DeclaringType`, тощо) та додаючи специфічні операції для роботи з пам'яттю полів.

---

## 1. Обхід інкапсуляції полів (Private Reflection)

Рефлексія дозволяє зчитувати та змінювати значення навіть закритих (`private`) та незмінних (`readonly`) полів, використовуючи методи `GetValue` та `SetValue`:

```csharp
FieldInfo? speedField = typeof(Car).GetField("_speed", BindingFlags.NonPublic | BindingFlags.Instance);
int currentSpeed = (int)speedField!.GetValue(myCar)!;

// Зміна значення закритого поля:
speedField.SetValue(myCar, 180);
```

---

## 2. Властивості класу FieldInfo (Properties)

| Властивість | Сигнатура | Опис призначення | Статус у курсі | Приклад у коді | Документація MSDN |
| :--- | :--- | :--- | :---: | :--- | :--- |
| **`FieldType`** | `Type FieldType { get; }` | Тип даних, що зберігається в полі (наприклад, `int`, `string`) | Використано | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L28) | [FieldInfo.FieldType](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.fieldinfo.fieldtype) |
| **`IsPublic`** | `bool IsPublic { get; }` | Визначає, чи є поле відкритим (`public`) | Використано | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L45) | [FieldInfo.IsPublic](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.fieldinfo.ispublic) |
| **`IsPrivate`** | `bool IsPrivate { get; }` | Визначає, чи є поле закритим (`private`) | Використано | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L47) | [FieldInfo.IsPrivate](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.fieldinfo.isprivate) |
| **`IsInitOnly`** | `bool IsInitOnly { get; }` | Визначає, чи є поле незмінним після ініціалізації (`readonly`) | Використано | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L49) | [FieldInfo.IsInitOnly](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.fieldinfo.isinitonly) |
| **`IsLiteral`** | `bool IsLiteral { get; }` | Визначає, чи є поле компіляційною константою (`const`) | Використано | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L50) | [FieldInfo.IsLiteral](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.fieldinfo.isliteral) |
| **`IsStatic`** | `bool IsStatic { get; }` | Визначає, чи є поле статичним (`static`) | Загальний BCL | Довідник | [FieldInfo.IsStatic](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.fieldinfo.isstatic) |

---

## 3. Методи класу FieldInfo (Methods)

| Метод | Сигнатура | Опис призначення | Статус у курсі | Приклад у коді | Документація MSDN |
| :--- | :--- | :--- | :---: | :--- | :--- |
| **`GetValue`** | `object? GetValue(object? obj)` | Повертає значення поля для заданого екземпляра об'єкта (для статичних полів передається `null`) | Використано | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L30) | [FieldInfo.GetValue](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.fieldinfo.getvalue) |
| **`SetValue`** | `void SetValue(object? obj, object? value)` | Встановлює нове значення поля для вказаного об'єкта | Використано | [07-GetFieldsPropertiesDemo/Program.cs](../../07-GetFieldsPropertiesDemo/Program.cs#L38) | [FieldInfo.SetValue](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.fieldinfo.setvalue) |
| **`GetRawConstantValue`**| `object? GetRawConstantValue()` | Повертає літеральне значення константи безпосередньо з метаданих | Загальний BCL | Довідник | [FieldInfo.GetRawConstantValue](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.fieldinfo.getrawconstantvalue) |
