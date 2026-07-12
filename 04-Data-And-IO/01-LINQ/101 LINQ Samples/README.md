# 🚀 101 LINQ Samples Runner (.NET 10)

> **Головний інтерактивний консольний раннер та навігатор для всіх 101 прикладів LINQ у .NET 10**

---

## 🎯 Призначення Проєкту

Цей проєкт є консольною точкою входу (`p_101 LINQ Samples`), яка поєднує 15 модульних бібліотек операторів LINQ та надає зручне консольне меню для виконання будь-якого прикладу в реальному часі.

* 📜 **Точка входу та меню:** [`Program.cs`](Program.cs)
* ⚙️ **Файл проєкту:** [`p_101 LINQ Samples.csproj`](p_101%20LINQ%20Samples.csproj) (налаштований на `net10.0`)

---

## 📚 Навчальні Матеріали та Презентація в Папці Проєкту

У цій директорії розміщено повний комплект документації та презентацій:

| Матеріал | Локальний шлях у проєкті | Опис |
| :--- | :--- | :--- |
| 🎓 **Навчальний Хаб** | 👉 [docs/README.md](docs/README.md) | Матриця всіх 101 прикладів та класифікація |
| 🎬 **Інтерактивна Презентація** | 👉 [docs/LINQ-Lecture-Presentation.md](docs/LINQ-Lecture-Presentation.md) | 25 слайдів з Mermaid-діаграмами та посиланнями на код |
| 📊 **PowerPoint Презентація** | 👉 [pptx/101-LINQ-Architecture-and-Operators.pptx](pptx/101-LINQ-Architecture-and-Operators.pptx) | Widescreen 16:9 лекційна презентація (30 слайдів) |
| 📘 **Розділ 1: Архітектура** | 👉 [docs/01-LINQ-Architecture-and-Execution.md](docs/01-LINQ-Architecture-and-Execution.md) | Моделі виконання (Deferred / Immediate), `yield return` |
| 📗 **Розділ 2: Фільтрація & Проєкції** | 👉 [docs/02-Filtering-and-Projections.md](docs/02-Filtering-and-Projections.md) | `Where`, `Select`, `SelectMany`, анонімні типи |
| 📙 **Розділ 3: Секціонування & Сортування** | 👉 [docs/03-Partitioning-and-Ordering.md](docs/03-Partitioning-and-Ordering.md) | `Take`, `Skip`, `OrderBy`, `ThenBy`, компаратори |
| 📕 **Розділ 4: Групування & Множини** | 👉 [docs/04-Grouping-and-Set-Operators.md](docs/04-Grouping-and-Set-Operators.md) | `GroupBy`, `Distinct`, `Union`, `Intersect`, `Except` |
| 📔 **Розділ 5: З'єднання & Агрегація** | 👉 [docs/05-Joins-and-Aggregate-Operators.md](docs/05-Joins-and-Aggregate-Operators.md) | `Join`, `GroupJoin`, `Count`, `Sum`, `Aggregate` |
| 📓 **Розділ 6: Квантори & Елементи** | 👉 [docs/06-Quantifiers-Elements-Conversion-and-Custom.md](docs/06-Quantifiers-Elements-Conversion-and-Custom.md) | `Any`, `All`, `First`, `ToList`, кастомні оператори |

---

## 🏃 Запуск з Консолі

З поточної директорії:
```bash
dotnet run
```

Або з кореня рішення:
```bash
dotnet run --project "101 LINQ Samples/p_101 LINQ Samples.csproj"
```

Користувач може ввести номер категорії (1–15), після чого відкриється підменю з номерами прикладів, або ввести `0` для виходу.
