# Модуль 13: AutoMapper, патерн DTO та серіалізація в C# (.NET 10)

Цей модуль присвячений архітектурним патернам передачі та трансформації даних у корпоративних системах на базі .NET: від концепції Data Transfer Object (DTO) та автоматизованого мапінгу складних графів об'єктів (AutoMapper) до високопродуктивної серіалізації в JSON (`System.Text.Json`) та XML (`XmlSerializer`).

---

## 🗺️ Карта підручника (Навчальні гайди)

1. **[01. Патерн DTO (Data Transfer Object) та трансформація об'єктів](Docs/01_DTO_Pattern_And_Object_Mapping.md)**
   - Навіщо потрібен DTO, ізоляція доменного шару, захист від Over-Posting, вирішення проблеми циклічних посилань.
2. **[02. AutoMapper: Профілі, Конвенції та Розширена Конфігурація](Docs/02_AutoMapper_Profiles_And_Advanced_Config.md)**
   - Профілі `Profile`, автоматичне сплющення ієрархій (Flattening), `ForMember`, `Ignore`, `MapFrom`, кастомні резолвери `IValueResolver`, інтеграція з DI.
3. **[03. Серіалізація даних у .NET: System.Text.Json та XmlSerializer](Docs/03_Serialization_Patterns_JSON_XML.md)**
   - Конфігурація `JsonSerializerOptions`, запобігання циклам через `ReferenceHandler`, атрибути `[XmlRoot]`/`[XmlElement]`, порівняння форматів.

---

## 📂 Структура проектів модуля

### 1. AutoMapperDemo (Трансформація та мапінг об'єктів)
| Компонент | Опис теми | Ключові файли |
| :--- | :--- | :--- |
| **Центральний профіль** | Налаштування правил мапінгу | [MappingProfile.cs](AutoMapperDemo/Mapping/MappingProfile.cs) |
| **Базовий мапінг** | Простий мапінг властивостей за назвами | [BasicMappingFeature.cs](AutoMapperDemo/Features/BasicMappingFeature.cs) |
| **Колекції** | Мапінг списків `List<T>`, масивів | [CollectionsFeature.cs](AutoMapperDemo/Features/CollectionsFeature.cs) |
| **Складний мапінг** | Сплющення вкладених сутностей | [ComplexMappingFeature.cs](AutoMapperDemo/Features/ComplexMappingFeature.cs) |
| **Умовний мапінг** | Мапінг за предикатом `Condition` | [ConditionalMappingFeature.cs](AutoMapperDemo/Features/ConditionalMappingFeature.cs) |
| **Ігнорування полів** | Виключення чутливих властивостей | [IgnoreMappingFeature.cs](AutoMapperDemo/Features/IgnoreMappingFeature.cs) |
| **Зворотний мапінг** | Двонаправлений зв'язок `ReverseMap` | [ReverseMappingFeature.cs](AutoMapperDemo/Features/ReverseMappingFeature.cs) |
| **Користувацькі резолвери** | Обчислення значень через `IValueResolver` | [AdvancedFeature.cs](AutoMapperDemo/Features/Advanced/AdvancedFeature.cs) |
| **Ручний мапінг** | Еталонний код без бібліотек | [ManualMappingFeature.cs](AutoMapperDemo/Features/ManualMappingFeature.cs) |

### 2. SerializationDemo (Серіалізація та архітектура додатку)
| Компонент | Опис теми | Ключові файли |
| :--- | :--- | :--- |
| **Сервіс серіалізації** | Уніфікована робота з JSON та XML | [SerializationService.cs](SerializationDemo/SerializationService.cs) |
| **Генератор даних** | Фабрика тестових сутностей | [DataGenerator.cs](SerializationDemo/DataGenerator.cs) |
| **Демонстратор** | Покрокове виконання тестів серіалізації | [DemoRunner.cs](SerializationDemo/DemoRunner.cs) |
| **Хост додатку** | Інверсія керування (DI) та запуск | [AppHost.cs](SerializationDemo/AppHost.cs) |
| **Доменні моделі** | Сутності `Customer`, `Order`, `Address` | [Models/](SerializationDemo/Models/) |

---

## 🛠️ Перевірка збірки (Zero-Error Verification)

Обидва проекти налаштовані на роботу з **.NET 10** та компілюються через центральне рішення:
```powershell
dotnet build "05-Architecture-And-Patterns/02-AutoMapper-And-DTO/OOP - Serialization-AutoMapper.slnx" -c Release
```
