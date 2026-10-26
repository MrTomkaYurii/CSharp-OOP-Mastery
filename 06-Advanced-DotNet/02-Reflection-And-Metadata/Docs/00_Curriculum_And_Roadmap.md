# 🧭 Навчальний план і дорожня карта (Curriculum & Roadmap): Рефлексія та метадані в .NET

Цей документ формує **послідовність викладу матеріалу** від фундаментальних концепцій рантайм-метаданих до розробки власного контейнера інверсії керування (IoC/DI).

---

## 🎯 Педагогічна мета модуля

1. Сформувати глибоке розуміння того, як **Common Language Runtime (CLR)** зберігає та надає інформацію про типи, збірки, методи та поля.
2. Навчити впевнено здійснювати інспекцію, пізнє зв'язування та модифікацію стану об'єктів під час виконання.
3. Опанувати роботу з **генериками**, **користувацькими атрибутами** та **бітовими масками (`BindingFlags`)**.
4. Зрозуміти ціну продуктивності рефлексії та техніки оптимізації (делегати, дерева виразів).
5. Створити завершений, архітектурно зрілий **IoC/DI-контейнер**, який узагальнює всі вивчені техніки.

---

## 📊 Узагальнююча таблиця навчального плану

| № | Етап / Тема | Ключові концепції та класи | Папка / Проєкт у рішенні | Посилання на код | Рівень |
| :-: | :--- | :--- | :--- | :-: | :-: |
| **00** | **Спільні доменні типи** | Класи `Car`, `SportsCar`, інтерфейс `ICar` для інспекції | `00-Shared` | [![Код](https://img.shields.io/badge/Код-00_Shared-blue?style=flat-square)](../00-Shared/Common/Car.cs) | Базовий |
| **01** | **Компіляційний Type** | Оператор `typeof`, відкриті/закриті дженерики, метадані CLR | `01-TypeofDemo` | [![Код](https://img.shields.io/badge/Код-01_TypeofDemo-blue?style=flat-square)](../01-TypeofDemo/Program.cs) | Базовий |
| **02** | **Поліморфний рантайм-тип** | `object.GetType()`, точний тип vs інтерфейс/базовий клас | `02-ObjectGetTypeDemo` | [![Код](https://img.shields.io/badge/Код-02_GetTypeDemo-blue?style=flat-square)](../02-ObjectGetTypeDemo/Program.cs) | Базовий |
| **03** | **Динамічний пошук за іменем** | `Type.GetType(string)`, AssemblyQualifiedName | `03-TypeGetTypeDemo` | [![Код](https://img.shields.io/badge/Код-03_TypeGetType-blue?style=flat-square)](../03-TypeGetTypeDemo/Program.cs) | Базовий |
| **04** | **Анатомія та прапорці Type** | `IsClass`, `IsValueType`, `IsInterface`, `IsAbstract`, `BaseType` | `04-TypePropertiesDemo` | [![Код](https://img.shields.io/badge/Код-04_TypeProps-blue?style=flat-square)](../04-TypePropertiesDemo/Program.cs) | Базовий |
| **05** | **Інспекція збірок (.dll)** | `Assembly`, `GetExecutingAssembly()`, `GetTypes()`, ресурси | `05-AssemblyDemo` | [![Код](https://img.shields.io/badge/Код-05_AssemblyDemo-blue?style=flat-square)](../05-AssemblyDemo/Program.cs) | Базовий |
| **06** | **Інтерфейси та диспетчеризація** | `Type.GetInterfaces()`, `IsAssignableFrom()`, `GetInterfaceMap()` | `06-GetInterfacesDemo` | [![Код](https://img.shields.io/badge/Код-06_GetInterfaces-blue?style=flat-square)](../06-GetInterfacesDemo/Program.cs) | Середній |
| **07** | **Поля, властивості та інкапсуляція** | `FieldInfo`, `PropertyInfo`, `GetValue`, `SetValue`, `BindingFlags` | `07-GetFieldsPropertiesDemo` | [![Код](https://img.shields.io/badge/Код-07_FieldsProps-blue?style=flat-square)](../07-GetFieldsPropertiesDemo/Program.cs) | Середній |
| **08** | **Методи та їх перевантаження** | `MethodInfo`, `Invoke`, пошук перевантажень за типами аргументів | `08-GetMethodsDemo` | [![Код](https://img.shields.io/badge/Код-08_MethodsDemo-blue?style=flat-square)](../08-GetMethodsDemo/Program.cs) | Середній |
| **09** | **Параметри методів** | `ParameterInfo`, `ref`/`out`/`in`, опціональні значення `DefaultValue` | `09-GetParameterInfoDemo` | [![Код](https://img.shields.io/badge/Код-09_ParamsDemo-blue?style=flat-square)](../09-GetParameterInfoDemo/Program.cs) | Середній |
| **10** | **Аналіз та виклик конструкторів** | `ConstructorInfo`, параметри ctor, вибір конструктора для DI | `10-GetConstructorInfoDemo` | [![Код](https://img.shields.io/badge/Код-10_CtorDemo-blue?style=flat-square)](../10-GetConstructorInfoDemo/Program.cs) | Середній |
| **11** | **Пізнє зв'язування та активація** | `Activator.CreateInstance`, динамічний виклик, перехоплення помилок | `11-LateBindingDemo` | [![Код](https://img.shields.io/badge/Код-11_LateBinding-blue?style=flat-square)](../11-LateBindingDemo/Program.cs) | Середній |
| **12** | **Рефлексія дженериків** | `MakeGenericType`, `MakeGenericMethod`, обмеження типів | `12-GenericsReflectionDemo` | [![Код](https://img.shields.io/badge/Код-12_GenericsDemo-green?style=flat-square)](../12-GenericsReflectionDemo/Program.cs) | Просунутий |
| **13** | **Користувацькі атрибути** | `AttributeUsage`, валідація сутностей, `CustomAttributeData` | `13-CreateCustomAttributes` | [![Код](https://img.shields.io/badge/Код-13_Attributes-green?style=flat-square)](../13-CreateCustomAttributes/Program.cs) | Просунутий |
| **14** | **Рефлексивна серіалізація** | Обхід графу властивостей, зіставлення XML/JSON, ігнорування | `14-SerializationDeserialization` | [![Код](https://img.shields.io/badge/Код-14_Serialization-green?style=flat-square)](../14-SerializationDeserialization/Program.cs) | Просунутий |
| **15** | **Продуктивність та оптимізація** | `MethodInfo.Invoke` vs `dynamic` vs `Delegate` vs `Expression Trees` | `15-ReflectionPerformanceDemo` | [![Код](https://img.shields.io/badge/Код-15_Performance-green?style=flat-square)](../15-ReflectionPerformanceDemo/Program.cs) | Експертний |
| **16** | **Фінальний проєкт: IoC/DI Контейнер** | Життєві цикли (Transient/Singleton), Constructor & Property Injection, Цикли | `16-DependencyInjection` | [![Код](https://img.shields.io/badge/Код-16_IoC_Container-red?style=flat-square)](../16-DependencyInjection/Program.cs) | Експертний |

---

## 🗺️ Графічна схема послідовності вивчення

```mermaid
flowchart TD
    subgraph M1["Модуль 1: Фундамент метаданих (System.Type)"]
        S0["00-Shared<br/>Доменні моделі"] --> S1["01-TypeofDemo<br/>Compile-time точність"]
        S1 --> S2["02-ObjectGetTypeDemo<br/>Runtime поліморфізм"]
        S2 --> S3["03-TypeGetTypeDemo<br/>Динамічне завантаження"]
        S3 --> S4["04-TypePropertiesDemo<br/>IsClass, IsValueType..."]
        S4 --> S5["05-AssemblyDemo<br/>Пошук типів у .dll"]
        S5 --> S6["06-GetInterfacesDemo<br/>GetInterfaceMap, IsAssignableFrom"]
    end

    subgraph M2["Модуль 2: Анатомія членів (MemberInfo & BindingFlags)"]
        S6 --> S7["07-GetFieldsPropertiesDemo<br/>FieldInfo, PropertyInfo, інкапсуляція"]
        S7 --> S8["08-GetMethodsDemo<br/>MethodInfo, пошук перевантажень"]
        S8 --> S9["09-GetParameterInfoDemo<br/>ParameterInfo, ref/out, defaults"]
        S9 --> S10["10-GetConstructorInfoDemo<br/>ConstructorInfo, сигнатури для DI"]
    end

    subgraph M3["Модуль 3: Динамічне створення та Дженерики"]
        S10 --> S11["11-LateBindingDemo<br/>Activator.CreateInstance, Invoke"]
        S11 --> S12["12-GenericsReflectionDemo<br/>MakeGenericType, MakeGenericMethod"]
    end

    subgraph M4["Модуль 4: Метадані та Практичні патерни"]
        S12 --> S13["13-CreateCustomAttributes<br/>AttributeUsage, рушій валідації"]
        S13 --> S14["14-SerializationDeserialization<br/>XML конвертер об'єктів"]
    end

    subgraph M5["Модуль 5: Продуктивність та оптимізація"]
        S14 --> S15["15-ReflectionPerformanceDemo<br/>Invoke vs Dynamic vs Expression Trees"]
    end

    subgraph M6["Модуль 6: Кульмінаційний проєкт"]
        S15 --> S16["16-DependencyInjection<br/>Синтез усіх знань: Lifetimes, Ctors, Circular Detection"]
    end

    style M1 fill:#e1f5fe,stroke:#0288d1,stroke-width:2px
    style M2 fill:#e8f5e9,stroke:#388e3c,stroke-width:2px
    style M3 fill:#fff3e0,stroke:#f57c00,stroke-width:2px
    style M4 fill:#f3e5f5,stroke:#7b1fa2,stroke-width:2px
    style M5 fill:#fffde7,stroke:#fbc02d,stroke-width:2px
    style M6 fill:#ffebee,stroke:#d32f2f,stroke-width:2px
```

---

## 📑 Опис модулів документації

1. [01. Дослідження системи типів та інспекція метаданих](01_Type_System_And_Metadata_Inspection.md) — Докладний розбір класів `Type`, `Assembly`, `MemberInfo` з таблицями методів.
2. [02. Члени типу, параметри та бітові прапорці BindingFlags](02_Members_And_BindingFlags.md) — Дослідження `FieldInfo`, `PropertyInfo`, `MethodInfo`, `ConstructorInfo`, `ParameterInfo`.
3. [03. Пізнє зв'язування, активація та дженерики](03_Late_Binding_And_Generics.md) — `Activator`, dynamic invoke, динамічне конструювання узагальнених типів та методів.
4. [04. Метадані, атрибути та валідаційні рушії](04_Attributes_And_Metadata_Validation.md) — Декларативне програмування, користувацькі атрибути, серіалізація.
5. [05. Продуктивність рефлексії та скомпільовані дерева виразів](05_Performance_Optimization_And_Compiled_Expressions.md) — Аналіз вузьких місць, порівняння швидкості, оптимізація через Expression Trees.
6. [06. Власний IoC/DI Контейнер: Повний розбір архітектури](06_IoC_Container_Deep_Dive.md) — Повне керівництво з побудови промислового міні-IoC контейнера.
