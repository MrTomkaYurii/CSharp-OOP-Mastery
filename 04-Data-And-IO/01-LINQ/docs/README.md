# 📚 Language Integrated Query (LINQ) у .NET 10: Повний Академічний та Архітектурний Довідник

> **Стек:** .NET 10 LTS | C# 13 | LINQ to Objects | Functional Programming  
> **Базовий репозиторій:** `101 LINQ Samples (.NET 10 Modernized)`  
> **Призначення:** Навчальний посібник, лекційний конспект та інтерактивна шпаргалка для студентів, викладачів та .NET-інженерів.

---

<a id="toc"></a>
## 📑 Навігатор Навчального Комплексу

Цей довідник складається з 6 тематичних підручників та повної лекційної презентації-шпаргалки на 25 слайдів:

| Модуль | Тематика та Зміст | Посилання на Підручник |
| :--- | :--- | :--- |
| **Вступ та Архітектура** | Архітектура LINQ, `IEnumerable<T>`, `IEnumerator<T>`, механізм `yield return`, конвеєризація, відкладене (Deferred) проти негайного (Immediate) виконання, потоковість (Streaming) та буферизація (Buffering). | 📖 [01-LINQ-Architecture-and-Execution.md](./01-LINQ-Architecture-and-Execution.md) |
| **Фільтрація та Проекція** | Категорії 1 та 2: Restriction (`Where`, indexed `Where`, `OfType`) та Projection (`Select`, indexed `Select`, анонімні типи, `SelectMany`, сплющення ієрархій, `let`). | 📖 [02-Filtering-and-Projections.md](./02-Filtering-and-Projections.md) |
| **Секціонування та Сортування** | Категорії 3 та 4: Partitioning (`Take`, `Skip`, `TakeWhile`, `SkipWhile`, діапазони) та Ordering (`OrderBy`, `OrderByDescending`, `ThenBy`, `ThenByDescending`, `Reverse`, компаратори). | 📖 [03-Partitioning-and-Ordering.md](./03-Partitioning-and-Ordering.md) |
| **Групування та Множини** | Категорії 5 та 6: Grouping (`GroupBy`, `IGrouping<K, V>`, складні ключі, агрегація груп) та Set Operators (`Distinct`, `Union`, `Intersect`, `Except`, хеш-множини). | 📖 [04-Grouping-and-Set-Operators.md](./04-Grouping-and-Set-Operators.md) |
| **З'єднання та Агрегація** | Категорії 11 та 15: Join Operators (Inner Join, Group Join, Cross Join, Left Outer Join, складені ключі) та Aggregate Operators (`Count`, `Sum`, `Min`, `Max`, `Average`, `Aggregate`). | 📖 [05-Joins-and-Aggregate-Operators.md](./05-Joins-and-Aggregate-Operators.md) |
| **Квантори, Елементи, Конверсія** | Категорії 7, 8, 9, 10, 12, 13: Conversion (`ToArray`, `ToList`, `ToDictionary`), Element (`First`, `FirstOrDefault`, `ElementAt`), Generation (`Range`, `Repeat`), Quantifiers (`Any`, `All`), Misc (`Concat`, `SequenceEqual`), Custom (`Combine`/`Zip`). | 📖 [06-Quantifiers-Elements-Conversion-and-Custom.md](./06-Quantifiers-Elements-Conversion-and-Custom.md) |
| **Лекційна Презентація** | Готова 25-слайдова інтерактивна презентація для аудиторних лекцій та експрес-повторення з візуальними схемами та життєвими аналогіями. | 🏛️ [LINQ-Lecture-Presentation.md](./LINQ-Lecture-Presentation.md) |

---

## 🏛️ Загальна Архітектура LINQ (Language Integrated Query)

LINQ інтегрує можливості декларативних запитів безпосередньо в мову C#, поєднуючи перевірку типів на етапі компіляції (Compile-Time Type Safety) з оптимізованими функціональними конвеєрами.

```mermaid
flowchart TD
    subgraph Language["Рівень Мови C#"]
        QS["Query Syntax\n(from ... where ... select)"]
        MS["Method Syntax\n(.Where(...).Select(...))"]
    end

    subgraph Compiler["C# Roslyn Compiler"]
        QS -->|"Синтаксичне розгортання (Syntactic Desugaring)"| MS
        MS --> Expr["Лямбда-вирази / Дерева виразів\n(Expression Trees)"]
    end

    subgraph Providers["Рівень Провайдерів LINQ"]
        Expr -->|"IEnumerable<T>"| L2O["LINQ to Objects\n(In-Memory Ітератори)"]
        Expr -->|"IQueryable<T>"| EF["LINQ to Entities / Database\n(SQL Translation)"]
        Expr -->|"XDocument / XElement"| L2X["LINQ to XML\n(DOM Навігація)"]
    end

    subgraph Execution["Фізичне Виконання"]
        L2O --> Pull["Pull-модель (GetEnumerator / MoveNext)\nЛінива генерація елементів"]
        EF --> SQL["Згенерований оптимізований SQL\n(SELECT ... FROM ...)"]
    end
```

---

## 🧭 Матриця 15 Категорій Операторів LINQ

| № | Категорія | Ключові Оператори | Тип Виконання | Алгоритмічна Модель | Приклади в Репозиторії |
| :-: | :--- | :--- | :--- | :--- | :--- |
| **1** | **Restriction** | `Where`, `OfType` | Відкладене (Deferred) | Потокова фільтрація (Streaming) | 👉 [Linq1.cs](../LINQ%20-%20Restriction%20Operators/C%23/RestrictionOperators/LinqSamples/Linq1.cs) – [OfTypeLinq5A.cs](../LINQ%20-%20Restriction%20Operators/C%23/RestrictionOperators/LinqSamples/OfTypeLinq5A.cs) |
| **2** | **Projection** | `Select`, `SelectMany` | Відкладене (Deferred) | Потокова трансформація (Streaming) | 👉 [DataSetLinq6.cs](../LINQ%20-%20Projection%20Operators/C%23/ProjectionOperators/LinqSamples/DataSetLinq6.cs) – [DataSetLinq19.cs](../LINQ%20-%20Projection%20Operators/C%23/ProjectionOperators/LinqSamples/DataSetLinq19.cs) |
| **3** | **Partitioning** | `Take`, `Skip`, `TakeWhile`, `SkipWhile` | Відкладене (Deferred) | Раннє переривання / зсув (Streaming) | 👉 [Linq20.cs](../LINQ%20-%20Partitioning%20Operators/C%23/PartitioningOperators/LinqSamples/Linq20.cs) – [Linq27.cs](../LINQ%20-%20Partitioning%20Operators/C%23/PartitioningOperators/LinqSamples/Linq27.cs) |
| **4** | **Ordering** | `OrderBy`, `OrderByDescending`, `ThenBy`, `Reverse` | Відкладене (Deferred) | **Буферизація** (Повне завантаження в пам'ять) | 👉 [Linq28.cs](../LINQ%20-%20Ordering%20Operators/C%23/OrderingOperators/LinqSamples/Linq28.cs) – [Linq39.cs](../LINQ%20-%20Ordering%20Operators/C%23/OrderingOperators/LinqSamples/Linq39.cs) |
| **5** | **Grouping** | `GroupBy`, `ToLookup` | Відкладене (Deferred) | **Буферизація** (Побудова Lookup-структури) | 👉 [DataSetLinq40.cs](..%5CLINQ%20%20-%20Grouping%20Operators%5CC%23%5CGroupingOperators%5CLinqSamples%5CDataSetLinq40.cs) – [DataSetLinq45.cs](..%5CLINQ%20%20-%20Grouping%20Operators%5CC%23%5CGroupingOperators%5CLinqSamples%5CDataSetLinq45.cs) |
| **6** | **Set Operators** | `Distinct`, `Union`, `Intersect`, `Except` | Відкладене (Deferred) | Хеш-множина (Set-based Streaming) | 👉 [Linq46.cs](../LINQ%20-%20Set%20Operators/C%23/SetOperators/LinqSamples/Linq46.cs) – [Linq53.cs](../LINQ%20-%20Set%20Operators/C%23/SetOperators/LinqSamples/Linq53.cs) |
| **7** | **Conversion** | `ToArray`, `ToList`, `ToDictionary`, `OfType` | **Негайне** (Immediate) | Матеріалізація в оперативну пам'ять | 👉 [Linq54.cs](../LINQ%20-%20Conversion%20Operators/C%23/ConversionOperators/LinqSamples/Linq54.cs) – [Linq57.cs](../LINQ%20-%20Conversion%20Operators/C%23/ConversionOperators/LinqSamples/Linq57.cs) |
| **8** | **Element** | `First`, `FirstOrDefault`, `ElementAt` | **Негайне** (Immediate) | Скалярний пошук (зупинка на першому збігу) | 👉 [Linq58.cs](../LINQ%20-%20Element%20Operators/C%23/ElementOperators/LinqSamples/Linq58.cs) – [Linq64.cs](../LINQ%20-%20Element%20Operators/C%23/ElementOperators/LinqSamples/Linq64.cs) |
| **9** | **Generation** | `Range`, `Repeat`, `Empty` | Відкладене (Deferred) | Генератор чисел/об'єктів на льоту | 👉 [Linq65.cs](../LINQ%20-%20Generation%20Operators/C%23/GenerationOperators/LinqSamples/Linq65.cs) – [Linq66.cs](../LINQ%20-%20Generation%20Operators/C%23/GenerationOperators/LinqSamples/Linq66.cs) |
| **10** | **Quantifiers** | `Any`, `All`, `Contains` | **Негайне** (Immediate) | Булева перевірка з коротким замиканням | 👉 [Linq67.cs](../LINQ%20-%20Quantifiers/C%23/Quantifiers/LinqSamples/Linq67.cs) – [Linq72.cs](../LINQ%20-%20Quantifiers/C%23/Quantifiers/LinqSamples/Linq72.cs) |
| **11** | **Aggregate** | `Count`, `Sum`, `Min`, `Max`, `Average`, `Aggregate` | **Негайне** (Immediate) | Згортка послідовності в одне число (Fold) | 👉 [Linq73.cs](../LINQ%20-%20Aggregate%20Operators/C%23/AggregateOperators/LinqSamples/Linq73.cs) – [Linq93.cs](../LINQ%20-%20Aggregate%20Operators/C%23/AggregateOperators/LinqSamples/Linq93.cs) |
| **12** | **Miscellaneous** | `Concat`, `SequenceEqual` | Відкладене (`Concat`) / Негайне (`SequenceEqual`) | Послідовне об'єднання / попарне порівняння | 👉 [Linq94.cs](../LINQ%20-%20Miscellaneous%20Operators/C%23/MiscellaneousOperators/LinqSamples/Linq94.cs) – [Linq97.cs](../LINQ%20-%20Miscellaneous%20Operators/C%23/MiscellaneousOperators/LinqSamples/Linq97.cs) |
| **13** | **Custom Sequence**| `Combine` (користувацький аналог `Zip`) | Відкладене (Deferred) | Паралельна ітерація двох перелічувачів | 👉 [DataSetLinq98.cs](../LINQ%20-%20Custom%20Sequence%20Operators/C%23/CustomSequenceOperators/LinqSamples/DataSetLinq98.cs) |
| **14** | **Query Execution**| Deferred vs Immediate, Query Re-evaluation | Архітектурна модель | Життєвий цикл стану запиту | 👉 [Linq99.cs](../LINQ%20-%20Query%20Execution/C%23/QueryExecution/LinqSamples/Linq99.cs) – [Linq101.cs](../LINQ%20-%20Query%20Execution/C%23/QueryExecution/LinqSamples/Linq101.cs) |
| **15** | **Join Operators** | Inner `Join`, `GroupJoin`, Left Outer Join | Відкладене (Deferred) | Hash Join / Хеш-таблиця правої послідовності | 👉 [Linq102.cs](../LINQ%20-%20Join%20Operators/C%23/JoinOperators/LinqSamples/Linq102.cs) – [Linq107.cs](../LINQ%20-%20Join%20Operators/C%23/JoinOperators/LinqSamples/Linq107.cs) |

---

## ⚡ Query Syntax проти Method Syntax: Правило Архітектора

| Аспект | Query Syntax (Синтаксис запитів) | Method Syntax (Методний / Лямбда синтаксис) |
| :--- | :--- | :--- |
| **Форма запису** | Декларативна, нагадує SQL (`from ... where ... select`) | Функціональний ланцюжок методів розширення (`.Where().Select()`) |
| **Компіляція** | Транслюється компілятором Roslyn у виклики методів розширення | Компілюється безпосередньо у виклики методів |
| **Коли використовувати** | Складні `join`, множинні `group by`, використання `let` та кількох джерел `from` | Більшість операцій, оператори без аналогів у Query Syntax (`Take`, `Skip`, `Distinct`, `Count`, `Any`) |
| **Приклади в коді** | 👉 [Linq102.cs (Query Syntax)](../LINQ%20-%20Join%20Operators/C%23/JoinOperators/LinqSamples/Linq102.cs) | 👉 [Linq102A.cs (Method Syntax)](../LINQ%20-%20Join%20Operators/C%23/JoinOperators/LinqSamples/Linq102.cs#L39-L62) |

---

## 🛠️ Як Запускати та Експериментувати з Проєктом

Усі проєкти переведені на сучасний SDK-style формат **.NET 10 (`net10.0`)**.

```bash
# 1. Клонувати репозиторій або відкрити термінал у кореневій папці
cd "c:\Users\Yurii\source\repos\- Examples OOP\OOP - LINQ-Samples"

# 2. Збірка всього рішення
dotnet build "101 LINQ Samples.sln"

# 3. Запуск головної консольної програми
dotnet run --project "101 LINQ Samples\p_101 LINQ Samples.csproj"
```

> [!TIP]
> Щоб запустити конкретну категорію прикладів, відкрийте [Program.cs](../101%20LINQ%20Samples/Program.cs), розкоментуйте виклик потрібного класу (наприклад, `samples_a.Linq1()`) та запустіть проєкт.
