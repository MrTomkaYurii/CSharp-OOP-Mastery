# Серіалізація даних у .NET: System.Text.Json та XmlSerializer

## Вступ
Серіалізація — це процес перетворення графа об'єктів у послідовність байтів або текст для збереження на диск або передачі по мережі. Десеріалізація — зворотний процес відновлення об'єкта в пам'яті.

---

## 1. System.Text.Json: Сучасний стандарт .NET

Починаючи з .NET Core 3.0 та суттєво розширений у .NET 8–10, `System.Text.Json` замінив застарілий `Newtonsoft.Json`. Він розроблений з акцентом на нульове зайве виділення пам'яті (Zero Allocation) завдяки використанню `Span<byte>` та `Utf8JsonReader/Writer`.

### Налаштування JsonSerializerOptions:
```csharp
var options = new JsonSerializerOptions
{
    WriteIndented = true, // Гарне форматування з відступами
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // camelCase для веб-стандарту
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull, // Пропуск null значень
    ReferenceHandler = ReferenceHandler.IgnoreCycles // Запобігання падінню при циклічних посиланнях
};

string json = JsonSerializer.Serialize(customer, options);
Customer? restored = JsonSerializer.Deserialize<Customer>(json, options);
```

---

## 2. XmlSerializer: Класичний контрактний XML

`XmlSerializer` з простору імен `System.Xml.Serialization` дозволяє формувати XML-документи зі строгим контролем тегів та атрибутів.

### Основні вимоги:
1. Клас обов'язково повинен мати відкритий конструктор без параметрів (parameterless constructor).
2. Серіалізуються лише відкриті поля та властивості з геттером і сеттером.

```csharp
[XmlRoot("CompanyCustomer")]
public class Customer
{
    [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlElement("FullName")]
    public string Name { get; set; } = string.Empty;

    [XmlIgnore] // Це поле не потрапить до XML-документа
    public string InternalSecret { get; set; } = string.Empty;
}
```

---

## 3. Порівняння форматів

| Критерій | JSON (`System.Text.Json`) | XML (`XmlSerializer`) |
| :--- | :--- | :--- |
| **Розмір даних** | Компактний, мінімальний оверхед | Більший через парні закриваючі теги |
| **Швидкість парсингу**| Надзвичайно висока (векторизація, SIMD) | Середня |
| **Сфера застосування** | REST API, мікросервіси, веб-клієнти | Банківські протоколи, SOAP, корпоративний обмін |
| **Підтримка схем** | JSON Schema (опціонально) | XSD (строга валідація схем) |

---

## 4. Проекти для практики

- [SerializationDemo - SerializationService](file:///05-Architecture-And-Patterns/02-AutoMapper-And-DTO/SerializationDemo/SerializationService.cs) — Уніфікований сервіс серіалізації в JSON та XML.
- [SerializationDemo - DemoRunner](file:///05-Architecture-And-Patterns/02-AutoMapper-And-DTO/SerializationDemo/DemoRunner.cs) — Практичне порівняння результатів серіалізації бізнес-моделей `Order`, `Customer`, `Address`.
- [SerializationDemo - AppHost](file:///05-Architecture-And-Patterns/02-AutoMapper-And-DTO/SerializationDemo/AppHost.cs) — Архітектура консольного хоста з контейнером інверсії керування (DI).
