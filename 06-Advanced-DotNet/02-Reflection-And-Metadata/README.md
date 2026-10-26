# Модуль: Рефлексія та метадані (Reflection & Metadata in .NET)

Цей навчальний модуль є повним практичним і теоретичним керівництвом із дослідження системи типів CLR, пізнього зв'язування, метаданих атрибутів, оптимізації продуктивності та створення власного повнофункціонального IoC/DI контейнера на базі рефлексії.

Всі матеріали та проєкти структуровані у логічній послідовності — від базових понять метаданих до розробки складного архітектурного каркаса.

---

## 1. Навігація по теоретичних та практичних керівництвах (Docs)

| Розділ | Тематика та ключові питання | Документ |
| :---: | :--- | :--- |
| **00** | **Навчальний план, послідовність викладу, понятійний апарат і дорожня карта тем** | [Docs/00_Curriculum_And_Roadmap.md](Docs/00_Curriculum_And_Roadmap.md) |
| **01** | Дослідження системи типів (`Type`, `Assembly`, `MemberInfo`, 3 шляхи отримання `Type`) | [Docs/01_Type_System_And_Metadata_Inspection.md](Docs/01_Type_System_And_Metadata_Inspection.md) |
| **02** | Члени типу, параметри та бітові прапорці (`BindingFlags`, `FieldInfo`, `PropertyInfo`, `MethodInfo`, `ConstructorInfo`, `ParameterInfo`) | [Docs/02_Members_And_BindingFlags.md](Docs/02_Members_And_BindingFlags.md) |
| **03** | Пізнє зв'язування, динамічна активація та дженерики (`Activator`, `MakeGenericType`, `MakeGenericMethod`) | [Docs/03_Late_Binding_And_Generics.md](Docs/03_Late_Binding_And_Generics.md) |
| **04** | Метадані, атрибути та валідаційні рушії (`AttributeUsage`, `CustomAttributeData`, XML-серіалізація) | [Docs/04_Attributes_And_Metadata_Validation.md](Docs/04_Attributes_And_Metadata_Validation.md) |
| **05** | **Дерева виразів (Expression Trees) та динамічна компіляція (AST, Code as Data, бустінг рефлексії)** | [Docs/05_Expression_Trees_Deep_Dive.md](Docs/05_Expression_Trees_Deep_Dive.md) |
| **06** | Продуктивність рефлексії, UnsafeAccessor та Native AOT (`MethodInfo.Invoke` vs `dynamic` vs `[UnsafeAccessor]`) | [Docs/06_Performance_Optimization_And_Native_AOT.md](Docs/06_Performance_Optimization_And_Native_AOT.md) |
| **07** | **Фінальний проєкт: Власний IoC/DI Контейнер (Lifetimes, Cycles, Greedy Ctor, Service Resolution)** | [Docs/07_IoC_Container_Deep_Dive.md](Docs/07_IoC_Container_Deep_Dive.md) |

---

## 2. Архітектура та структура Solution-тегів у рішенні `OOP - Reflection.sln`

Рішення містить **23 пронумерованих проєкти**, які рознесені по **6 тематичних Solution-тегах (Solution Folders)** для логічного структурування парадигм використання рефлексії:

```mermaid
graph TD
    subgraph S1["01. Type System & Metadata Inspection (Система типів та метадані)"]
        P0["00-Shared"]
        P1["01-TypeofDemo"]
        P2["02-ObjectGetTypeDemo"]
        P3["03-TypeGetTypeDemo"]
        P4["04-TypePropertiesDemo"]
        P5["05-AssemblyDemo"]
        P6["06-GetInterfacesDemo"]
    end

    subgraph S2["02. Members & Encapsulation (Члени типу та інкапсуляція)"]
        P7["07-GetFieldsPropertiesDemo"]
        P8["08-GetMethodsDemo"]
        P9["09-GetParameterInfoDemo"]
        P10["10-GetConstructorInfoDemo"]
        P11["11-GetEventsDemo"]
    end

    subgraph S3["03. Generics Reflection (Рефлексія дженериків)"]
        P12["12-GenericsReflectionDemo"]
        P13["13-OpenGenericsAndVarianceDemo"]
    end

    subgraph S4["04. Dynamic Execution & Performance (Динамічне виконання та оптимізація)"]
        P14["14-LateBindingDemo"]
        P15_0["15.0-ExpressionTreesBasicsDemo"]
        P15_1["15.1-ExpressionTreesBuildingDemo"]
        P15_2["15.2-ExpressionTreesMemberAccessDemo"]
        P15_3["15.3-ReflectionPerformanceDemo"]
        P16["16-UnsafeAccessorDemo"]
    end

    subgraph S5["05. Attributes & Serialization (Атрибути та серіалізація)"]
        P17["17-CreateCustomAttributes"]
        P18["18-SerializationDeserialization"]
    end

    subgraph S6["06. Frameworks & Dependency Injection (Фреймворки та IoC-контейнер)"]
        P19["19-DependencyInjection"]
    end

    P0 --> P1
    P1 --> P2
    P2 --> P3
    P3 --> P4
    P4 --> P5
    P5 --> P6
    P6 --> P7
    P7 --> P8
    P8 --> P9
    P9 --> P10
    P10 --> P11
    P11 --> P12
    P12 --> P13
    P13 --> P14
    P14 --> P15_0
    P15_0 --> P15_1
    P15_1 --> P15_2
    P15_2 --> P15_3
    P15_3 --> P16
    P16 --> P17
    P17 --> P18
    P18 --> P19
```

---

## 3. Зведена таблиця проєктів у рішенні

| № | Solution-тег | Назва проєкту | Призначення та ключові API | Посилання на вихідний код |
| :-: | :--- | :--- | :--- | :--- |
| 00 | `01. Type System & Metadata Inspection` | `00-Shared` | Доменні класи (`Car`, `SportsCar`, `ICar`), об'єкти для інспекції | [00-Shared/Common/Car.cs](00-Shared/Common/Car.cs) |
| 01 | `01. Type System & Metadata Inspection` | `01-TypeofDemo` | Отримання `Type` на етапі компіляції (`typeof(T)`) | [01-TypeofDemo/Program.cs](01-TypeofDemo/Program.cs) |
| 02 | `01. Type System & Metadata Inspection` | `02-ObjectGetTypeDemo` | Отримання точного рантайм-типу через невіртуальний `GetType()` | [02-ObjectGetTypeDemo/Program.cs](02-ObjectGetTypeDemo/Program.cs) |
| 03 | `01. Type System & Metadata Inspection` | `03-TypeGetTypeDemo` | Динамічне завантаження типу за текстовим ім'ям (`Type.GetType`) | [03-TypeGetTypeDemo/Program.cs](03-TypeGetTypeDemo/Program.cs) |
| 04 | `01. Type System & Metadata Inspection` | `04-TypePropertiesDemo` | Загальні характеристики: `IsClass`, `IsValueType`, `IsAbstract`, `IsSealed` | [04-TypePropertiesDemo/Program.cs](04-TypePropertiesDemo/Program.cs) |
| 05 | `01. Type System & Metadata Inspection` | `05-AssemblyDemo` | Завантаження збірки, перелік її типів та інспекція ресурсів | [05-AssemblyDemo/Program.cs](05-AssemblyDemo/Program.cs) |
| 06 | `01. Type System & Metadata Inspection` | `06-GetInterfacesDemo` | Інтерфейси типу та карта реалізації методів (`GetInterfaceMap`) | [06-GetInterfacesDemo/Program.cs](06-GetInterfacesDemo/Program.cs) |
| 07 | `02. Members & Encapsulation` | `07-GetFieldsPropertiesDemo` | Фільтрація полів (`FieldInfo`) та властивостей (`PropertyInfo`) | [07-GetFieldsPropertiesDemo/Program.cs](07-GetFieldsPropertiesDemo/Program.cs) |
| 08 | `02. Members & Encapsulation` | `08-GetMethodsDemo` | Інспекція відкритих та закритих методів (`MethodInfo`, `Invoke`) | [08-GetMethodsDemo/Program.cs](08-GetMethodsDemo/Program.cs) |
| 09 | `02. Members & Encapsulation` | `09-GetParameterInfoDemo` | Інспекція параметрів: типи, `ref`/`out`, значення за замовчуванням | [09-GetParameterInfoDemo/Program.cs](09-GetParameterInfoDemo/Program.cs) |
| 10 | `02. Members & Encapsulation` | `10-GetConstructorInfoDemo` | Аналіз конструкторів (`ConstructorInfo`), створення об'єктів | [10-GetConstructorInfoDemo/Program.cs](10-GetConstructorInfoDemo/Program.cs) |
| 11 | `02. Members & Encapsulation` | `11-GetEventsDemo` | Інспекція `EventInfo`, динамічна підписка делегатів (`AddEventHandler`) | [11-GetEventsDemo/Program.cs](11-GetEventsDemo/Program.cs) |
| 12 | `03. Generics Reflection` | `12-GenericsReflectionDemo` | Рефлексія дженериків: `MakeGenericType`, `MakeGenericMethod` | [12-GenericsReflectionDemo/Program.cs](12-GenericsReflectionDemo/Program.cs) |
| 13 | `03. Generics Reflection` | `13-OpenGenericsAndVarianceDemo` | Відкриті дженерики (`IRepository<>`), коваріантність (`out`), контраваріантність (`in`) | [13-OpenGenericsAndVarianceDemo/Program.cs](13-OpenGenericsAndVarianceDemo/Program.cs) |
| 14 | `04. Dynamic Execution & Performance` | `14-LateBindingDemo` | Створення об'єкта (`Activator.CreateInstance`, `GetUninitializedObject`) | [14-LateBindingDemo/Program.cs](14-LateBindingDemo/Program.cs) |
| 15.0 | `04. Dynamic Execution & Performance` | `15.0-ExpressionTreesBasicsDemo` | Дерева виразів (AST): різниця між `Func` та `Expression`, анатомія вузлів, `.Compile()` | [15.0-ExpressionTreesBasicsDemo/Program.cs](15.0-ExpressionTreesBasicsDemo/Program.cs) |
| 15.1 | `04. Dynamic Execution & Performance` | `15.1-ExpressionTreesBuildingDemo` | Ручне конструювання AST: `Expression.Parameter`, `Multiply`, `Add`, `Condition` (if-else) | [15.1-ExpressionTreesBuildingDemo/Program.cs](15.1-ExpressionTreesBuildingDemo/Program.cs) |
| 15.2 | `04. Dynamic Execution & Performance` | `15.2-ExpressionTreesMemberAccessDemo` | Бустінг рефлексії: `Expression.Property` (геттери), `Call` (методи), `New` (фабрики) | [15.2-ExpressionTreesMemberAccessDemo/Program.cs](15.2-ExpressionTreesMemberAccessDemo/Program.cs) |
| 15.3 | `04. Dynamic Execution & Performance` | `15.3-ReflectionPerformanceDemo` | Бенчмарк швидкодії: Direct Call vs Delegates vs Expression Trees vs Invoke | [15.3-ReflectionPerformanceDemo/Program.cs](15.3-ReflectionPerformanceDemo/Program.cs) |
| 16 | `04. Dynamic Execution & Performance` | `16-UnsafeAccessorDemo` | JIT Compiler Intrinsic `[UnsafeAccessor]` (.NET 8+): доступ до private без оверхеду | [16-UnsafeAccessorDemo/Program.cs](16-UnsafeAccessorDemo/Program.cs) |
| 17 | `05. Attributes & Serialization` | `17-CreateCustomAttributes` | Створення користувацьких атрибутів та декларативна валідація | [17-CreateCustomAttributes/Program.cs](17-CreateCustomAttributes/Program.cs) |
| 18 | `05. Attributes & Serialization` | `18-SerializationDeserialization` | Рефлексивний XML-серіалізатор і десеріалізатор | [18-SerializationDeserialization/Program.cs](18-SerializationDeserialization/Program.cs) |
| 19 | `06. Frameworks & Dependency Injection` | `19-DependencyInjection` | **Фінальний проєкт**: повноцінний рефлексивний IoC/DI контейнер | [19-DependencyInjection/Program.cs](19-DependencyInjection/Program.cs) |

---

## 4. Як відкрити та запустити в Visual Studio

1. Відкрийте файл рішення `OOP - Reflection.sln` у Visual Studio (2022 або новішій).
2. У вікні **Solution Explorer** ви побачите:
   - Папку **`Docs`** з усіма інтерактивними теоретичними markdown-файлами.
   - Тематичні **Solution-папки** (`01. Type System & Metadata Inspection`, `02. Members & Encapsulation`, `03. Generics Reflection`, `04. Dynamic Execution & Performance`, `05. Attributes & Serialization`, `06. Frameworks & Dependency Injection`).
   - Пронумеровані проєкти від `00-Shared` до `19-DependencyInjection`.
3. Щоб запустити будь-який приклад, натисніть правою кнопкою миші на відповідний проєкт і виберіть **Set as Startup Project** (або призначте активним у випадаючому списку панелі інструментів), після чого натисніть `F5` або `Ctrl+F5`.
