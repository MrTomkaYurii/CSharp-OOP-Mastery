# 05. Attributes & Serialization: Загальний огляд та зріз методів

> **Solution-тег (Solution Folder)**: `05. Attributes & Serialization`  
> **Проєкти у тегу**:
> - [17-CreateCustomAttributes/Program.cs](../../17-CreateCustomAttributes/Program.cs) — Створення власних атрибутів (`AttributeUsage`), декларативна валідація, інспекція `#Blob`.
> - [18-SerializationDeserialization/Program.cs](../../18-SerializationDeserialization/Program.cs) — Рефлексивний XML-серіалізатор і десеріалізатор, динамічний обхід графу властивостей.

---

## 1. Концептуальний зріз розділу

Цей розділ розглядає декларативне метапрограмування та автоматизацію обробки даних за допомогою атрибутів:
1. **Користувацькі атрибути (Custom Attributes)**: створення класів-спадкоємців `Attribute`, обмеження цілей застосування через `[AttributeUsage]`.
2. **Два рівні зчитування атрибутів**:
   - `GetCustomAttribute<T>()` — створює готовий екземпляр класу атрибута.
   - `GetCustomAttributesData()` — зчитує сирі байти без створення екземплярів (безпечно для інструментів аналізу та AOT).
3. **Рефлексивна серіалізація**: автоматизоване зіставлення об'єктної моделі з текстовими форматами (XML/JSON) на основі зіставлення властивостей.

### Охоплені ключові класи:
- [CustomAttributes.md](CustomAttributes.md) — робота з класами `Attribute`, `AttributeUsageAttribute` та `CustomAttributeData`.
- [ReflectionSerialization.md](ReflectionSerialization.md) — патерни обходу властивостей об'єкта для серіалізації/десеріалізації.

---

## 2. Зведена матриця методів атрибутів та серіалізації

| Клас | Метод / Властивість | Статус у курсі | Опис призначення | Приклад у коді | Документація MSDN |
| :--- | :--- | :---: | :--- | :--- | :--- |
| `MemberInfo` | `GetCustomAttribute<T>()` | Використано | Повертає типізований екземпляр атрибута, викликаючи його конструктор | [17-CreateCustomAttributes/Program.cs](../../17-CreateCustomAttributes/Program.cs#L22) | [CustomAttributeExtensions.GetCustomAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.customattributeextensions.getcustomattribute) |
| `MemberInfo` | `GetCustomAttributesData()` | Використано | Повертає список дескрипторів `CustomAttributeData` без виконання користувацького коду | [17-CreateCustomAttributes/Program.cs](../../17-CreateCustomAttributes/Program.cs#L45) | [MemberInfo.GetCustomAttributesData](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.memberinfo.getcustomattributesdata) |
| `MemberInfo` | `IsDefined(Type, bool)` | Використано | Перевіряє наявність атрибута заданого типу на елементі коду | [17-CreateCustomAttributes/Program.cs](../../17-CreateCustomAttributes/Program.cs#L30) | [MemberInfo.IsDefined](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.memberinfo.isdefined) |
| `CustomAttributeData` | `AttributeType` | Використано | Тип атрибута, збережений у метаданих | [17-CreateCustomAttributes/Program.cs](../../17-CreateCustomAttributes/Program.cs#L52) | [CustomAttributeData.AttributeType](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.customattributedata.attributetype) |
| `CustomAttributeData` | `ConstructorArguments` | Використано | Позиційні аргументи, передані в конструктор атрибута | [17-CreateCustomAttributes/Program.cs](../../17-CreateCustomAttributes/Program.cs#L56) | [CustomAttributeData.ConstructorArguments](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.customattributedata.constructorarguments) |
| `CustomAttributeData` | `NamedArguments` | Використано | Іменовані властивості або поля, передані атрибуту | [17-CreateCustomAttributes/Program.cs](../../17-CreateCustomAttributes/Program.cs#L60) | [CustomAttributeData.NamedArguments](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.customattributedata.namedarguments) |
| `AttributeUsageAttribute` | `ValidOn` | Використано | Бітова маска цілей застосування атрибута (`AttributeTargets`) | [17-CreateCustomAttributes/Program.cs](../../17-CreateCustomAttributes/Program.cs#L72) | [AttributeUsageAttribute.ValidOn](https://learn.microsoft.com/en-us/dotnet/api/system.attributeusageattribute.validon) |
| `AttributeUsageAttribute` | `AllowMultiple` | Використано | Чи дозволено застосовувати атрибут кілька разів до одного елемента | [17-CreateCustomAttributes/Program.cs](../../17-CreateCustomAttributes/Program.cs#L74) | [AttributeUsageAttribute.AllowMultiple](https://learn.microsoft.com/en-us/dotnet/api/system.attributeusageattribute.allowmultiple) |
| `AttributeUsageAttribute` | `Inherited` | Використано | Чи успадковується атрибут похідними класами та перевизначеними членами | [17-CreateCustomAttributes/Program.cs](../../17-CreateCustomAttributes/Program.cs#L76) | [AttributeUsageAttribute.Inherited](https://learn.microsoft.com/en-us/dotnet/api/system.attributeusageattribute.inherited) |
| `Type` | `GetProperties()` | Використано | Рефлексивний збір властивостей для генерації XML/JSON тегів | [18-SerializationDeserialization/Program.cs](../../18-SerializationDeserialization/Program.cs#L24) | [Type.GetProperties](https://learn.microsoft.com/en-us/dotnet/api/system.type.getproperties) |
| `PropertyInfo` | `GetValue(object)` | Використано | Отримання значення властивості під час серіалізації | [18-SerializationDeserialization/Program.cs](../../18-SerializationDeserialization/Program.cs#L32) | [PropertyInfo.GetValue](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.propertyinfo.getvalue) |
| `PropertyInfo` | `SetValue(object, value)` | Використано | Запис спарсеного значення під час десеріалізації | [18-SerializationDeserialization/Program.cs](../../18-SerializationDeserialization/Program.cs#L68) | [PropertyInfo.SetValue](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.propertyinfo.setvalue) |

---

## 3. Статистичний зріз охоплення API

- **Методів та властивостей досліджено в прикладах**: 12
- **Загальних методів API BCL задокументовано додатково**: 2
- **Сумарно в довіднику розділу**: 14 API елементів
- **Відсоток фокусного практичного охоплення**: ~86%
