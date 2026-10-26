# 03. Generics Reflection: Загальний огляд та зріз методів

> **Solution-тег (Solution Folder)**: `03. Generics Reflection`  
> **Проєкти у тегу**:
> - [12-GenericsReflectionDemo/Program.cs](../../12-GenericsReflectionDemo/Program.cs) — Дослідження узагальнених класів та методів, закриття типів через `MakeGenericType` та `MakeGenericMethod`.
> - [13-OpenGenericsAndVarianceDemo/Program.cs](../../13-OpenGenericsAndVarianceDemo/Program.cs) — Відкриті дженерики у DI (`IRepository<>`), перевірка обмежень типів, коваріантність (`out`) та контраваріантність (`in`).

---

## 1. Концептуальний зріз розділу

Цей розділ розкриває специфіку роботи рефлексії з узагальненнями (дженериками) в .NET:
1. **Відкриті типи (Open Generics / Generic Type Definitions)**: шаблонні описи типу (наприклад, `List<>` або `Dictionary<,>`), екземпляр яких створити неможливо.
2. **Закриті зконструйовані типи (Constructed Closed Generics)**: типи, в яких усі параметри типізації замінені реальними класами (наприклад, `List<int>`), готові до інстанціювання.
3. **Варіативність (Variance)**: коваріантність (`out`) та контраваріантність (`in`) в інтерфейсах та делегатах.

### Охоплені ключові теми та класи:
- [GenericTypeReflection.md](GenericTypeReflection.md) — робота з узагальненими класами та інтерфейсами (`System.Type`).
- [GenericMethodReflection.md](GenericMethodReflection.md) — динамічне конструювання та виклик узагальнених методів (`System.Reflection.MethodInfo`).

---

## 2. Зведена матриця методів узагальнень (Використані vs Загальні в BCL)

| Клас | Метод / Властивість | Статус у курсі | Опис призначення | Приклад у коді | Документація MSDN |
| :--- | :--- | :---: | :--- | :--- | :--- |
| `Type` | `IsGenericType` | Використано | Визначає, чи є тип узагальненим (відкритим чи закритим) | [12-GenericsReflectionDemo/Program.cs](../../12-GenericsReflectionDemo/Program.cs#L14) | [Type.IsGenericType](https://learn.microsoft.com/en-us/dotnet/api/system.type.isgenerictype) |
| `Type` | `IsGenericTypeDefinition` | Використано | Визначає, чи є тип відкритим шаблоном (наприклад, `Repository<>`) | [12-GenericsReflectionDemo/Program.cs](../../12-GenericsReflectionDemo/Program.cs#L16) | [Type.IsGenericTypeDefinition](https://learn.microsoft.com/en-us/dotnet/api/system.type.isgenerictypedefinition) |
| `Type` | `GetGenericTypeDefinition()` | Використано | Отримує дескриптор відкритого типу з закритого типу | [12-GenericsReflectionDemo/Program.cs](../../12-GenericsReflectionDemo/Program.cs#L22) | [Type.GetGenericTypeDefinition](https://learn.microsoft.com/en-us/dotnet/api/system.type.getgenerictypedefinition) |
| `Type` | `GetGenericArguments()` | Використано | Повертає масив аргументів або параметрів типу | [12-GenericsReflectionDemo/Program.cs](../../12-GenericsReflectionDemo/Program.cs#L28) | [Type.GetGenericArguments](https://learn.microsoft.com/en-us/dotnet/api/system.type.getgenericarguments) |
| `Type` | `MakeGenericType(Type[])` | Використано | Динамічно конструює закритий тип, підставляючи конкретні класи | [12-GenericsReflectionDemo/Program.cs](../../12-GenericsReflectionDemo/Program.cs#L36) | [Type.MakeGenericType](https://learn.microsoft.com/en-us/dotnet/api/system.type.makegenerictype) |
| `Type` | `ContainsGenericParameters` | Використано | Визначає, чи містить тип хоча б один незакритий параметр типу | [13-OpenGenericsAndVarianceDemo/Program.cs](../../13-OpenGenericsAndVarianceDemo/Program.cs#L22) | [Type.ContainsGenericParameters](https://learn.microsoft.com/en-us/dotnet/api/system.type.containsgenericparameters) |
| `Type` | `GenericParameterAttributes` | Використано | Бітові прапорці коваріантності (`Covariant`), контраваріантності (`Contravariant`) та обмежень | [13-OpenGenericsAndVarianceDemo/Program.cs](../../13-OpenGenericsAndVarianceDemo/Program.cs#L45) | [Type.GenericParameterAttributes](https://learn.microsoft.com/en-us/dotnet/api/system.type.genericparameterattributes) |
| `Type` | `GetGenericParameterConstraints()`| Використано| Повертає масив обмежень на параметр типу (`where T : class, new()`) | [13-OpenGenericsAndVarianceDemo/Program.cs](../../13-OpenGenericsAndVarianceDemo/Program.cs#L52) | [Type.GetGenericParameterConstraints](https://learn.microsoft.com/en-us/dotnet/api/system.type.getgenericparameterconstraints) |
| `MethodInfo` | `IsGenericMethod` | Використано | Визначає, чи є метод узагальненим | [12-GenericsReflectionDemo/Program.cs](../../12-GenericsReflectionDemo/Program.cs#L48) | [MethodBase.IsGenericMethod](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.isgenericmethod) |
| `MethodInfo` | `IsGenericMethodDefinition` | Використано | Визначає, чи є метод відкритим визначенням узагальненого методу | [12-GenericsReflectionDemo/Program.cs](../../12-GenericsReflectionDemo/Program.cs#L50) | [MethodBase.IsGenericMethodDefinition](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodbase.isgenericmethoddefinition) |
| `MethodInfo` | `MakeGenericMethod(Type[])`| Використано | Підставляє конкретні типи замість `T` і повертає готовий до виклику метод | [12-GenericsReflectionDemo/Program.cs](../../12-GenericsReflectionDemo/Program.cs#L58) | [MethodInfo.MakeGenericMethod](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodinfo.makegenericmethod) |
| `MethodInfo` | `GetGenericMethodDefinition()`| Загальний BCL | Отримує вихідне відкрите визначення з раніше закритого методу | Довідник [GenericMethodReflection.md](GenericMethodReflection.md) | [MethodInfo.GetGenericMethodDefinition](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.methodinfo.getgenericmethoddefinition) |

---

## 3. Статистичний зріз охоплення API

- **Методів та властивостей досліджено в прикладах**: 11
- **Загальних методів API BCL задокументовано додатково**: 1
- **Сумарно в довіднику розділу**: 12 API елементів
- **Відсоток фокусного практичного охоплення**: ~92%
