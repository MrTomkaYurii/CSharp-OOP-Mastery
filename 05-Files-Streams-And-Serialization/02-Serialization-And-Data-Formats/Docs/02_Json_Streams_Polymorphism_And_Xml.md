# 02. Потокова серіалізація JSON, поліморфізм ООП та XML / LINQ to XML

Цей гайд доповнює базові можливості `System.Text.Json` та `XmlSerializer` просунутими сценаріями роботи з потоками (`Stream`), ієрархіями успадкування ООП та деревами `XDocument`.

---

## 1. Потокова серіалізація напряму у `FileStream` (Zero-String Allocation)

Виклик `JsonSerializer.Serialize(obj)` створює великий рядок `string` в оперативній пам'яті (який при розмірі понад 85 КБ потрапляє у Large Object Heap). У промислових сервісах об'єкти серіалізують та десеріалізують **напряму в потік байтів UTF-8**:

```csharp
await using FileStream writeStream = File.Create("invoice.json");
await JsonSerializer.SerializeAsync(writeStream, invoice, options);
```

Для максимальної швидкодії без рефлексії використовується низькорівневий `Utf8JsonWriter` та `Utf8JsonReader`.

---

## 2. Поліморфна серіалізація ієрархій класів (`[JsonPolymorphic]`, `[JsonDerivedType]`)

За замовчуванням `System.Text.Json` серіалізує лише властивості оголошеного базового типу. Щоб зберегти та відновити конкретні похідні класи колекції `List<PaymentMethod>`, базовий тип позначають атрибутами дискримінатора:

```csharp
[JsonPolymorphic(TypeDiscriminatorPropertyName = "$paymentType")]
[JsonDerivedType(typeof(BankCardPayment), typeDiscriminator: "card")]
[JsonDerivedType(typeof(CryptoTransferPayment), typeDiscriminator: "crypto")]
[JsonDerivedType(typeof(WireTransferPayment), typeDiscriminator: "iban")]
public abstract record PaymentMethod(decimal Amount, string Currency);
```

---

## 3. Серіалізація XML та LINQ to XML (`XDocument`, `XElement`)

- **`XmlSerializer`** керується атрибутами `[XmlRoot]`, `[XmlElement]`, `[XmlAttribute]`, `[XmlIgnore]`.
- **`XDocument` (LINQ to XML)** дозволяє працювати з XML-документом у пам'яті за допомогою знайомих операторів LINQ (`Elements()`, `Attributes()`, `Where()`, `Select()`).

---

## Матриця проєктів

| Проєкт | Призначення та ключові концепції | Вихідний код |
| :--- | :--- | :--- |
| **`19-SerializationBasicsJsonDemo`** | Основи формату JSON, `JsonSerializer`, `JsonDocument`, `[JsonPropertyName]`, `[JsonIgnore]` | [`Program.cs`](../19-SerializationBasicsJsonDemo/Program.cs) |
| **`19.1-JsonStreamsAndPolymorphismDemo`** | Асинхронний запис/читання у `FileStream`, поліморфна ієрархія `[JsonPolymorphic]` та `Utf8JsonWriter` | [`Program.cs`](../19.1-JsonStreamsAndPolymorphismDemo/Program.cs) |
| **`20-SerializationBasicsXmlDemo`** | `XmlSerializer`, атрибути XML, `XDocument` (LINQ to XML) та безпечний `XmlReader` | [`Program.cs`](../20-SerializationBasicsXmlDemo/Program.cs) | 