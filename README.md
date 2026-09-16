# C# & OOP Mastery: Від Основ до Архітектури в .NET 10

[![.NET 10](https://img.shields.io/badge/.NET-10.0-512bd4.svg?style=flat-square&logo=dotnet)](https://dotnet.microsoft.com/)
[![C# 13/14](https://img.shields.io/badge/C%23-13%20%2F%2014-239120.svg?style=flat-square&logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![Architecture](https://img.shields.io/badge/Architecture-SOLID%20%26%20Clean%20Code-blue.svg?style=flat-square)](https://github.com/)

Ласкаво просимо до повного практичного курсу та живої бази знань з **об'єктно-орієнтованого програмування, інженерних практик та архітектури на платформі .NET 10**.

Цей репозиторій є результатом консолідації, модернізації та систематизації розрізнених навчальних матеріалів в єдину цілісну екосистему. Кожен модуль спроектовано за принципом **«Теорія ➔ Антипатерн (Bad) ➔ Еталонне рішення (Good)»**.

---

## 🗺️ Дорожня карта курсу (Roadmap)

```mermaid
flowchart TD
    classDef fundClass fill:#0284c7,stroke:#38bdf8,color:#ffffff,stroke-width:2px;
    classDef oopClass fill:#0d9488,stroke:#2dd4bf,color:#ffffff,stroke-width:2px;
    classDef mechClass fill:#d97706,stroke:#fcd34d,color:#ffffff,stroke-width:2px;
    classDef dataClass fill:#4f46e5,stroke:#818cf8,color:#ffffff,stroke-width:2px;
    classDef archClass fill:#be123c,stroke:#fb7185,color:#ffffff,stroke-width:2px;
    classDef advClass fill:#7e22ce,stroke:#c084fc,color:#ffffff,stroke-width:2px;
    classDef efClass fill:#15803d,stroke:#4ade80,color:#ffffff,stroke-width:2px;

    M1["01. Fundamentals<br/>Класи, інкапсуляція, records, конструктори"]:::fundClass
    M2["02. OOP Core<br/>Успадкування, поліморфізм, інтерфейси"]:::oopClass
    M3["03. C# Mechanics<br/>Generics, колекції, ітератори, делегати, події, exceptions"]:::mechClass
    M4["04. Data & I/O<br/>Глибокий LINQ, FileStream, серіалізація"]:::dataClass
    M5["05. Architecture & Patterns<br/>SOLID, IoC/DI, AutoMapper"]:::archClass
    M6["06. Advanced .NET<br/>Async/Await, Reflection, кастомні атрибути, Spectre.Console"]:::advClass
    M7["07. Data Access (EF Core)<br/>Code First, DbContext, міграції"]:::efClass

    M1 --> M2 --> M3 --> M4 --> M5 --> M6 --> M7
```

---

## 📚 Структура модулів курсу

| Модуль | Тематика | Ключові концепції | Статус |
|:---|:---|:---|:---:|
| **[01-Fundamentals](01-Fundamentals/)** | Основи класів та типи даних | Поля, властивості, інваріанти, конструктори, `record`, `static`, `IDisposable` | ✅ Завершено |
| **[02-OOP-Core](02-OOP-Core/)** | Ядро ООП | Наслідування, `override`/`new`, явна реалізація інтерфейсів, поліморфізм | ✅ Завершено |
| **[03-Csharp-Mechanics](03-Csharp-Mechanics/)** | Механізми платформи C# | `Generics`, внутрішня будова колекцій, `IEnumerator`, лямбди, події, фільтри помилок | ✅ Завершено |
| **[04-Data-And-IO](04-Data-And-IO/)** | Дані та потоки введення/виведення | LINQ-оператори, ліниві обчислення, `FileStream`, бінарне/текстове читання | ✅ Завершено |
| **[05-Architecture-And-Patterns](05-Architecture-And-Patterns/)** | Архітектурне проектування | SOLID-практикум, еволюція DI-контейнерів, DTO-патерни | ✅ Завершено |
| **[06-Advanced-DotNet](06-Advanced-DotNet/)** | Просунутий .NET | Асинхронність (`Task`, `async/await`), рефлексія типів, атрибути, CLI Spectre.Console | ✅ Завершено |
| **[07-DataAccess-EFCore](07-DataAccess-EFCore/)** | Доступ до реляційних БД | ORM Entity Framework Core, Code-First, міграції, Multi-Provider, Generic Host | ✅ Завершено |

---

## 🛠️ Вимоги до оточення та швидкий старт

- **SDK:** [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) або новіший
- **IDE:** Visual Studio 2026 / Visual Studio Code / JetBrains Rider

### Клонування та збірка
```bash
git clone <URL_REPOSITORIA>
cd CSharp-OOP-Mastery
dotnet build
```

---

## 📋 Регламент проекту
- [RULES.md](RULES.md) — Повний регламент міграції, 7-кроковий чекліст переносу кожного модуля та стандарти документації.
- [MIGRATION_TRACKER.md](MIGRATION_TRACKER.md) — Живий статус міграції модулів.
