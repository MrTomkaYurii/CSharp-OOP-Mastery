# Підмодуль 01: Рефлексія, інспекція метаданих та власний IoC-контейнер (`OOP - Reflection.sln`)

Цей підмодуль присвячений дослідженню системи типів CLR у рантаймі (`System.Reflection`), рефлексії узагальнень, пізньому зв'язуванню (`Activator`, `MethodInfo.Invoke`), динамічній генерації IL-коду (`Reflection.Emit`), високопродуктивному доступу через `[UnsafeAccessor]` (.NET 8+) та побудові власного повнофункціонального **IoC / Dependency Injection контейнера**.

---

## Навігація по теоретичних та практичних посібниках (`Docs`)

| Розділ | Тематика та ключові питання | Посилання |
| :---: | :--- | :--- |
| **00** | Навчальний план і дорожня карта рефлексії | [`Docs/00_Curriculum_And_Roadmap.md`](Docs/00_Curriculum_And_Roadmap.md) |
| **01** | Система типів (`Type`, `Assembly`, `typeof`, `GetType`, `Type.GetType`) | [`Docs/01_Type_System_And_Metadata_Inspection.md`](Docs/01_Type_System_And_Metadata_Inspection.md) |
| **02** | Члени типу та прапорці пошуку (`MemberInfo`, `BindingFlags`, `FieldInfo`, `PropertyInfo`, `MethodInfo`, `ConstructorInfo`) | [`Docs/02_Members_And_BindingFlags.md`](Docs/02_Members_And_BindingFlags.md) |
| **03** | Рефлексія дженериків та варіантності (`MakeGenericType`, `MakeGenericMethod`, `GenericParameterAttributes`) | [`Docs/03_Late_Binding_And_Generics.md`](Docs/03_Late_Binding_And_Generics.md) |
| **04** | Генерація проміжного коду CIL на льоту (`System.Reflection.Emit`, `DynamicMethod`, `ILGenerator`) | [`Docs/04_IL_Generation_And_Reflection_Emit.md`](Docs/04_IL_Generation_And_Reflection_Emit.md) |
| **05** | Продуктивність рефлексії, `[UnsafeAccessor]` та Native AOT | [`Docs/06_Performance_Optimization_And_Native_AOT.md`](Docs/06_Performance_Optimization_And_Native_AOT.md) |
| **06** | Архітектура власного IoC / Dependency Injection контейнера | [`Docs/07_IoC_Container_Deep_Dive.md`](Docs/07_IoC_Container_Deep_Dive.md) |

---

## Структура Solution-тек у рішенні `OOP - Reflection.sln` (33 проєкти)

| Solution-тека | Проєкти | Ключові концепції |
| :--- | :--- | :--- |
| **`01. Система типів та інспекція метаданих`** | `00-Shared`, `p696[SOLIS]`, `p698[SOLIS]`, `reflection-example-intro-01`, `01-TypeofDemo`..`06-GetInterfacesDemo` | Отримання `Type` трьома способами, властивості типу, дослідження збірок `Assembly` та карти інтерфейсів `GetInterfaceMap` |
| **`02. Члени типу, інкапсуляція та прапорці пошуку`** | `reflection-ex-04`..`10`, `07-GetFieldsPropertiesDemo`..`11-GetEventsDemo` | Пошук відкритих і приватних членів через бітові маски `BindingFlags`, інспекція полів, властивостей, методів, параметрів, конструкторів та подій |
| **`03. Рефлексія узагальнень та варіантності`** | `12-GenericsReflectionDemo`, `13.1-CovarianceAndContravarianceDemo`, `13.2-VarianceReflectionInspectionDemo` | Конструювання закритих generic-типів у рантаймі (`MakeGenericType`), аналіз коваріантності (`out`) та контраваріантності (`in`) |
| **`04. Пізнє зв'язування, Reflection.Emit та UnsafeAccessor`** | `14-LateBindingDemo`, `reflection-example-late-binding-01`..`02`, `reflection-example-intro-02`..`03`, `16-UnsafeAccessorDemo` | Динамічне створення екземплярів (`Activator`), плагінна архітектура, генерація IL-інструкцій (`ILGenerator`) та нульовий оверхед `[UnsafeAccessor]` |
| **`05. Побудова власного IoC та DI контейнера`** | `25.1-DependencyInjectionDemo`, `25.2-DependencyInjectionRealWorld` | Автоматичне розв'язання графа залежностей конструкторів, життєві цикли `Transient`/`Scoped`/`Singleton` та виявлення циклічних залежностей |

---

## Збірка та запуск

```powershell
dotnet build "04-Advanced-CSharp/01-Reflection/OOP - Reflection.sln" -c Release
```
