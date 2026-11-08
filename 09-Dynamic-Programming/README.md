# Модуль 09: Динамічне програмування ( від простого до складного ) та динамічні можливості C# (DLR)

Цей розділ побудований так, щоб студент міг **максимально легко і наочно** опанувати дві важливі теми:
1. **Алгоритмічне динамічне програмування «на пальцях»**: що це таке без складної математики, чому звичайна рекурсія «зависає» на мільйонах однакових викликів, як **Мемоізація** (Рекурсія + Блокнот/Кеш) та **Табуляція** (заповнення таблички у циклі `for`) прискорюють програму в мільйони разів, та як розв'язувати три класичні життєві задачі (Решта в кавомашині, Туристичний рюкзак, Виправлення одруківок у слові).
2. **Динамічні можливості мови C# та метапрограмування**: ключове слово `dynamic`, середа виконання Dynamic Language Runtime (DLR), динамічні об'єкти `ExpandoObject` і `DynamicObject`, генерація байт-коду IL через `DynamicMethod` та компіляція дерев виразів (`Expression Trees`).

---

## 1. Навігація по теоретичних матеріалах (папка `Docs`)

| Розділ | Тематика та ключові питання | Посилання на документ |
| :---: | :--- | :--- |
| **00** | **Навчальний план, послідовність викладу, глосарій та дорожня карта** | [Docs/00_Curriculum_And_Roadmap.md](Docs/00_Curriculum_And_Roadmap.md) |
| **01** | **Динамічне програмування простими словами** (Top-Down vs Bottom-Up, сходинки, робот-збирач монет, кавомашина, рюкзак, одруківки) | [Docs/01_Algorithmic_Dynamic_Programming.md](Docs/01_Algorithmic_Dynamic_Programming.md) |
| **02** | Динамічні можливості C# та DLR (`dynamic`, DLR CallSite кеш, `ExpandoObject`, `DynamicObject`) | [Docs/02_CSharp_Dynamic_And_DLR_Internals.md](Docs/02_CSharp_Dynamic_And_DLR_Internals.md) |
| **03** | Динамічне метапрограмування та кодогенерація (`DynamicMethod`, емісія IL, Expression Trees) | [Docs/03_Dynamic_Metaprogramming_And_Code_Generation.md](Docs/03_Dynamic_Metaprogramming_And_Code_Generation.md) |
| **04** | **Фінальний проєкт: Динамічний конвеєр обробки замовлень (Dynamic Pipeline Engine)** | [Docs/04_Capstone_Pipeline_Deep_Dive.md](Docs/04_Capstone_Pipeline_Deep_Dive.md) |

---

## 2. Структура рішення `DynamicProgramming.sln` (4 тематичні теки)

У вікні **Solution Explorer** усі 9 проєктів згруповані у **4 пронумеровані навчальні розділи**:

* **`01. Основи динамічного програмування: Мемоізація та Табуляція`**
  * `01-Memoization-Basics` — підхід Згори-Вниз (Top-Down = Рекурсія + Словник-блокнот) на задачі про сходинки та числах Фібоначчі.
  * `02-Tabulation-Basics` — підхід Знизу-Вгору (Bottom-Up = заповнення таблички `dp[]` у циклі без рекурсії) на сходинках, роботі-збирачі монет та оптимізації пам'яті до $O(1)$ (лише 2 змінні!).
  * `03-Classic-DP-Problems` — покрокова візуалізація таблиць у консолі для трьох класичних задач: **Кавомашина та решта монетами (Coin Change)**, **Туристичний рюкзак (0/1 Knapsack)** та **Виправлення одруківок у слові (Відстань Левенштейна)**.
  * `04-Generic-Memoizer-Pattern` — універсальний метод розширення `.Memoize()` для автоматичного кешування будь-яких функцій у C#.
* **`02. Динамічна типізація C# та середа DLR (dynamic, ExpandoObject)`**
  * `05-Dynamic-Keyword-And-DLR` — порівняння `dynamic`, `object` та `var`, кешування `CallSite`, мультидиспетчеризація.
  * `06-ExpandoObject-And-DynamicObject` — додавання властивостей і методів «на льоту» через `ExpandoObject` та власний `DynamicObject`.
* **`03. Динамічна кодогенерація (DynamicMethod IL Emit та Expression Trees)`**
  * `07-DynamicMethod-Emit` — низькорівнева генерація інструкцій CIL (`Reflection.Emit`) для надшвидкого доступу до властивостей.
  * `08-Expression-Trees-Dynamic-Compilation` — конструювання та компіляція дерев виразів (`Expression<Func<T, bool>>`) під час виконання.
* **`04. Практичний конвеєр правил (Dynamic Pipeline Engine)`**
  * `09-Dynamic-Pipeline-Engine` — комплексний проєкт, що об'єднує всі розглянуті техніки в єдиному бізнес-конвеєрі.

---

## 3. Зведена таблиця проєктів

| № | Назва проєкту | Що демонструє простими словами | Вихідний код |
| :-: | :--- | :--- | :--- |
| 01 | `01-Memoization-Basics` | Як словник `Dictionary<int, long>` скорочує кількість викликів рекурсії з 331 мільйона до 79. | [01-Memoization-Basics/Program.cs](01-Memoization-Basics/Program.cs) |
| 02 | `02-Tabulation-Basics` | Покрокове заповнення масиву `dp[]` у циклі (сходинки, робот і монети) та заміна масиву на 2 змінні. | [02-Tabulation-Basics/Program.cs](02-Tabulation-Basics/Program.cs) |
| 03 | `03-Classic-DP-Problems` | Кавомашина (решта 11 грн), Туристичний рюкзак (7 кг) та Виправлення слова `"КІТ" -> "КОД"` з таблицями в консолі. | [03-Classic-DP-Problems/Program.cs](03-Classic-DP-Problems/Program.cs) |
| 04 | `04-Generic-Memoizer-Pattern` | Універсальна обгортка `Memoize()` на базі замикань і `ConcurrentDictionary`. | [04-Generic-Memoizer-Pattern/Program.cs](04-Generic-Memoizer-Pattern/Program.cs) |
| 05 | `05-Dynamic-Keyword-And-DLR` | Робота ключового слова `dynamic` та кешування місць виклику (`CallSite`) у DLR. | [05-Dynamic-Keyword-And-DLR/Program.cs](05-Dynamic-Keyword-And-DLR/Program.cs) |
| 06 | `06-ExpandoObject-And-DynamicObject` | Гнучкі динамічні об'єкти `ExpandoObject` та перехоплення викликів через `DynamicObject`. | [06-ExpandoObject-And-DynamicObject/Program.cs](06-ExpandoObject-And-DynamicObject/Program.cs) |
| 07 | `07-DynamicMethod-Emit` | Генерація байт-коду IL на льоту через `DynamicMethod` та `ILGenerator`. | [07-DynamicMethod-Emit/Program.cs](07-DynamicMethod-Emit/Program.cs) |
| 08 | `08-Expression-Trees-Dynamic-Compilation` | Побудова та компіляція дерев виразів (`Expression`) для динамічних фільтрів і фабрик. | [08-Expression-Trees-Dynamic-Compilation/Program.cs](08-Expression-Trees-Dynamic-Compilation/Program.cs) |
| 09 | `09-Dynamic-Pipeline-Engine` | **Фінальний проєкт**: конвеєр обробки замовлень із DP-оптимізацією доставки та динамічними правилами. | [09-Dynamic-Pipeline-Engine/Program.cs](09-Dynamic-Pipeline-Engine/Program.cs) |

---

## 4. Як відкрити та запустити

1. Відкрийте `DynamicProgramming.sln` у Visual Studio або запустіть збірку через консоль:
   ```bash
   dotnet build DynamicProgramming.sln
   ```
2. Щоб запустити будь-який приклад із консолі (наприклад, перші три уроки з динамічного програмування):
   ```bash
   dotnet run --project 01-Memoization-Basics
   dotnet run --project 02-Tabulation-Basics
   dotnet run --project 03-Classic-DP-Problems
   ```
