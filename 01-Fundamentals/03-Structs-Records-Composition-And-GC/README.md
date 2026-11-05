# Модуль 01.3: Взаємодія Класів, `struct` vs `record` та Життєвий Цикл Об'єкта в Пам'яті

Цей підмодуль завершує першу главу (`01-Fundamentals`), підсумовуючи знання про класи та готуючи фундамент перед переходом до Успадкування (`02-OOP-Core`):
1. Як будувати складні моделі з простих класів через **Композицію та Агрегацію (`Has-A`)**.
2. Чим значущі типи **`struct`** відрізняються від **`class`** у пам'яті та що таке **Boxing / Unboxing**.
3. Як працюють сучасні незмінні моделі **`record`** (`Value Equality`, оператор `with`, `Deconstruct`).
4. Як **Garbage Collector (GC)** керує поколіннями об'єктів (`Gen 0 / 1 / 2`) у Купі та навіщо потрібен **`IDisposable` (`using`)**.

---

## Матриця проєктів та документації

| Розділ | Проєкт | Посібник у Docs | Ключові концепції |
|:---|:---|:---|:---|
| **01** | [01-CompositionVsAggregation](01-CompositionVsAggregation/Program.cs) | [01_Composition_And_Aggregation.md](Docs/01_Composition_And_Aggregation.md) | Зв'язок `Has-A`, життєвий цикл частин у Композиції та Агрегації |
| **02** | [02-StructVsClassAndBoxing](02-StructVsClassAndBoxing/Program.cs) | [02_Structs_And_Boxing.md](Docs/02_Structs_And_Boxing.md) | `readonly struct` vs `class`, копіювання за значенням, `Boxing`/`Unboxing` |
| **03** | [03-RecordEqualityAndWith](03-RecordEqualityAndWith/Program.cs) | [03_Records_And_Value_Equality.md](Docs/03_Records_And_Value_Equality.md) | `Reference Equality` проти `Value Equality`, клонування `with`, `Deconstruct` |
| **04** | [04-GarbageCollectorAndIDisposable](04-GarbageCollectorAndIDisposable/Program.cs) | [04_Garbage_Collector_And_IDisposable.md](Docs/04_Garbage_Collector_And_IDisposable.md) | Покоління `Gen 0/1/2`, `GC.GetGeneration`, `WeakReference`, `IDisposable` | 