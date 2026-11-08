# Підмодуль 04: Патерн DTO та бібліотека AutoMapper (`OOP - AutoMapper and DTO.sln`)

Цей підмодуль розглядає практики розділення внутрішніх моделей збереження даних (Entities) та зовнішніх контрактів взаємодії (DTO — Data Transfer Objects), проектування профілів трансформації за допомогою популярної бібліотеки **AutoMapper**, а також шаблони серіалізації даних.

---

## Архітектурний розподіл рівнів: Entities проти DTO

```mermaid
flowchart LR
    classDef ent fill:#0284c7,stroke:#38bdf8,color:#ffffff,stroke-width:2px;
    classDef map fill:#0d9488,stroke:#2dd4bf,color:#ffffff,stroke-width:2px;
    classDef dto fill:#7e22ce,stroke:#c084fc,color:#ffffff,stroke-width:2px;

    DB[("База даних")] --> EF["Entity Framework Models<br/>(User, Order, PasswordHash)"]:::ent
    EF --> Mapper["AutoMapper Profile<br/>(Трансформація, маскування, сплющення)"]:::map
    Mapper --> DTO["Data Transfer Objects (DTO)<br/>(UserResponseDto, OrderSummaryDto)"]:::dto
    DTO --> API["UI / API / Зовнішні клієнти"]
```

---

## Навігація по навчальних посібниках (`Docs`)

| № | Посібник | Ключові теми та системні інструменти | Посилання на документ |
|:---|:---|:---|:---|
| 01 | **Патерн DTO та Об'єктний Мапінг** | Навіщо потрібні DTO, захист від Over-Posting, проектування чистих контрактів без витоку деталей БД | [Docs/01_DTO_Pattern_And_Object_Mapping.md](Docs/01_DTO_Pattern_And_Object_Mapping.md) |
| 02 | **Профілі AutoMapper та Розширена Конфігурація** | `Profile`, `CreateMap`, `ForMember`, сплющення ієрархій (`Flattening`), конвертери значень | [Docs/02_AutoMapper_Profiles_And_Advanced_Config.md](Docs/02_AutoMapper_Profiles_And_Advanced_Config.md) |
| 03 | **Шаблони Серіалізації JSON та XML** | Серіалізація DTO у JSON (`System.Text.Json`), налаштування CamelCase, атрибути ігнорування | [Docs/03_Serialization_Patterns_JSON_XML.md](Docs/03_Serialization_Patterns_JSON_XML.md) |

---

## Структура проєктів рішення `OOP - AutoMapper and DTO.sln`

* **`AutoMapperDemo`**:
  * Створення доменних сутностей `Customer`, `Order`, `Address`.
  * Оголошення профілів мапінгу `CustomerProfile` із кастомними правилами обчислення полів.
  * Тестування конфігурації мапінгу через `configuration.AssertConfigurationIsValid()`.
  * Посилання на вихідний код: [`Program.cs`](AutoMapperDemo/Program.cs).
* **`SerializationDemo`**:
  * Серіалізація та десеріалізація отриманих DTO-об'єктів за допомогою сучасного `System.Text.Json`.
  * Налаштування конвертерів та оптимізація розміру вихідного корисного навантаження.
  * Посилання на вихідний код: [`Program.cs`](SerializationDemo/Program.cs).
