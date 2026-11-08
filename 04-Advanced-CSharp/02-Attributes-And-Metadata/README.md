# Підмодуль 02: Метадані та Користувацькі Атрибути (`OOP-Attributes-And-Metadata.sln`)

Цей підмодуль охоплює декларативне програмування у C# за допомогою системних та користувацьких атрибутів (`System.Attribute`), таблиць метаданих збірки (`#Blob`) та побудови власного рушія валідації моделей у Runtime.

---

## Навігація по навчальних матеріалах (`Docs`)

1. **[01. Системні атрибути компілятора та основи створення власних атрибутів](Docs/01_Custom_Attributes_Fundamentals.md)**
2. **[02. Зчитування та обробка метаданих атрибутів у Runtime](Docs/02_Metadata_Querying_And_Runtime_Processing.md)**
3. **[03. Метадані та декларативна валідація моделей](Docs/03_Attributes_And_Metadata_Validation.md)**
4. **[04. Поглиблений розбір кастомних атрибутів та потоку `#Blob`](Docs/04_Custom_Attributes_Deep_Dive.md)**
5. **[05. Архітектура рушія валідації на рівні класу та властивостей](Docs/05_Class_And_Property_Validation_Engine.md)**

---

## Структура Solution-тек у рішенні `OOP-Attributes-And-Metadata.sln` (12 проєктів)

| Solution-тека | Проєкти | Ключові теми та API |
| :--- | :--- | :--- |
| **`01. Системні атрибути компілятора та Caller Info`** | `p701[SOLIS]`, `p703[SOLIS]`, `p704[SOLIS]`, `attribute-example-02` | Атрибути `[Obsolete]`, умовна компіляція `[Conditional]`, трасування викликів `[CallerFilePath]`, `[CallerLineNumber]`, `[CallerMemberName]` |
| **`02. Проєктування власних атрибутів та таблиці метаданих`** | `00-Shared`, `p717[SOLIS]`, `p718[SOLIS]`, `attribute-example-01`, `attribute-example-03`, `attribute-example-04`, `17-AttributesBasicsDemo` | Успадкування від `Attribute`, конфігурація `[AttributeUsage]`, позиційні та іменовані параметри, перевірка `IsDefined`, читання `GetCustomAttributes<T>()` та `CustomAttributeData` |
| **`03. Декларативна валідація моделей та правил у Runtime`** | `18-AttributesValidationDemo` | Розробка власних валідаційних атрибутів на рівні властивостей (`Property`) і класу (`Class`) та універсального рефлексивного валідатора |

---

## Збірка та запуск

```powershell
dotnet build "04-Advanced-CSharp/02-Attributes-And-Metadata/OOP-Attributes-And-Metadata.sln" -c Release
```
