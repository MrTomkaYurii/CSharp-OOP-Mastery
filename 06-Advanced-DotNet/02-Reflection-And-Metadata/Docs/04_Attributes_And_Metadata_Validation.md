# 04. Метадані, атрибути та валідаційні рушії (Attributes & Metadata Validation)

**Атрибути (Attributes)** у .NET — це спеціальні класи-маркери, які додають декларативні метадані до коду: збірок, класів, методів, властивостей, полів та параметрів. Вони не впливають на логіку методу напряму, але можуть бути зчитані рефлексією для побудови бізнес-правил, серіалізації або валідації.

---

## 🏷️ Створення власного атрибута: `System.Attribute`

Будь-який атрибут є похідним від базового класу `System.Attribute`. Для керування його поведінкою застосовується системний мета-атрибут `[AttributeUsage]`:

```csharp
[AttributeUsage(
    AttributeTargets.Property | AttributeTargets.Field, // Де можна застосовувати
    AllowMultiple = false,                              // Чи дозволено повторювати кілька разів на одному члені
    Inherited = true)]                                  // Чи успадковується в похідних класах
public class MinValueAttribute : Attribute
{
    public int Value { get; }
    public string? ErrorMessage { get; set; }

    public MinValueAttribute(int value) => Value = value;
}
```

🔗 Переглянути код визначення атрибутів можна в [13-CreateCustomAttributes/MinValueAttribute.cs](../13-CreateCustomAttributes/MinValueAttribute.cs#L1-L38).

---

## 📋 Таблиця способів зчитування атрибутів через рефлексію

| Метод | Клас | Особливість роботи | Посилання на приклад |
| :--- | :--- | :--- | :---: |
| `member.GetCustomAttribute<T>()` | `CustomAttributeExtensions` | Повертає типізований атрибут або `null`. Інстанціює об'єкт атрибута. | [![Код](https://img.shields.io/badge/Код-13_GetCustomAttribute-green?style=flat-square)](../13-CreateCustomAttributes/MinValueValidator.cs#L22-L35) |
| `member.GetCustomAttributes<T>()` | `CustomAttributeExtensions` | Повертає всі атрибути типу `T` (якщо `AllowMultiple = true`). | [![Код](https://img.shields.io/badge/Код-13_GetAllAttributes-green?style=flat-square)](../13-CreateCustomAttributes/MinValueValidator.cs) |
| `Attribute.IsDefined(member, typeof(T))` | `Attribute` | Швидка перевірка наявності атрибута без створення його екземпляра. | [![Код](https://img.shields.io/badge/Код-14_IsDefined-green?style=flat-square)](../14-SerializationDeserialization/XmlConverter.cs#L28-L34) |
| `member.GetCustomAttributesData()` | `MemberInfo` | **Інспекція без виклику конструктора!** Повертає сирі метадані `CustomAttributeData`. | [![Код](https://img.shields.io/badge/Код-13_InspectRawData-green?style=flat-square)](../13-CreateCustomAttributes/MinValueValidator.cs#L45-L65) |

---

## ⚙️ Практичні сценарії застосування атрибутів

### 1. Декларативна валідація сутностей (Validation Engine)
Замість розпилення перевірок `if (user.Age < 18)` по всьому коду, ми помічаємо модель атрибутами `[Required]`, `[MinValue(18)]`, `[StringLength(2, 30)]`. Універсальний валідатор `MinValueValidator.Validate(object, out errors)` через рефлексію проходить по властивостях та автоматично генерує список помилок.

🔗 Переглянути валідатор: [13-CreateCustomAttributes/MinValueValidator.cs](../13-CreateCustomAttributes/MinValueValidator.cs#L15-L42).

### 2. Керування серіалізацією (Custom XML/JSON Engine)
Атрибути дозволяють керувати процесом зіставлення полів при збереженні даних у зовнішні формати:
- `[XmlElementName("FullName")]` — змінює назву XML-тегу.
- `[XmlIgnoreProperty]` — виключає чутливі дані (наприклад, паролі, секретні ключі) із результуючого файлу.

🔗 Переглянути серіалізатор: [14-SerializationDeserialization/XmlConverter.cs](../14-SerializationDeserialization/XmlConverter.cs#L20-L48).
