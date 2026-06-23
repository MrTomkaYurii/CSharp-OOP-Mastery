# Модуль 07: Перелічувачі та ітератори в C# (.NET 10)

Цей модуль присвячений глибокому аналізу внутрішніх механізмів обходу колекцій у .NET: від базового циклу `foreach` та ручної реалізації інтерфейсів `IEnumerable` / `IEnumerator` до патернного ітерування (Duck Typing) та компіляторного кінцевого автомата `yield return`.

---

## 🗺️ Карта підручника (Навчальні гайди)

1. **[01. Протоколи ітерування: IEnumerable та IEnumerator](Docs/01_IEnumerable_And_IEnumerator_Protocols.md)**
   - Ролі постачальника та курсора, ручна реалізація `MoveNext()`, `Current`, `Reset()`.
2. **[02. Механіка циклу foreach та Duck Typing у C#](Docs/02_Foreach_Mechanics_And_Duck_Typing.md)**
   - Декомпіляція `foreach` у блок `try/finally`, патернне ітерування без інтерфейсів для `ref struct` (`Span<T>`).
3. **[03. Ітератори, yield return та кінцевий автомат (State Machine)](Docs/03_Yield_Return_And_State_Machine.md)**
   - Відкладене виконання (lazy evaluation), генерація класу-автомата компілятором, `yield break` та множинні іменовані ітератори.

---

## 📂 Структура проектів модуля

| Проект | Опис теми | Ключові концепції |
| :--- | :--- | :--- |
| **[p486[SOLIS]](file:///03-Csharp-Mechanics/03-Enumerators-And-Iterators/p486[SOLIS]/Program.cs)** | Базовий цикл `foreach` | Синтаксис `foreach` по масиву |
| **[p490[SOLIS]](file:///03-Csharp-Mechanics/03-Enumerators-And-Iterators/p490[SOLIS]/Program.cs)** | Робота з `IEnumerator` напряму | `GetEnumerator()`, `while (ie.MoveNext())`, `(int)ie.Current` |
| **[p492[SOLIS]](file:///03-Csharp-Mechanics/03-Enumerators-And-Iterators/p492[SOLIS]/Program.cs)** | Власна реалізація `IEnumerator` | Клас `ColorEnumerator`, стан позиції `Position = -1` |
| **[ex-01](file:///03-Csharp-Mechanics/03-Enumerators-And-Iterators/ex-01/Program.cs)** | Об'єктна модель ітерування | `People : IEnumerable`, `PeopleEnum : IEnumerator` |
| **[ex-02](file:///03-Csharp-Mechanics/03-Enumerators-And-Iterators/ex-02/Program.cs)** | Узагальнене ітерування `IEnumerable<T>` | Делегування перебору внутрішній колекції `List<int>` |
| **[p498[SOLIS]](file:///03-Csharp-Mechanics/03-Enumerators-And-Iterators/p498[SOLIS]/Program.cs)** | Базовий `yield return` | Повернення `IEnumerator<string>` з методу-ітератора |
| **[p500[SOLIS]](file:///03-Csharp-Mechanics/03-Enumerators-And-Iterators/p500[SOLIS]/Program.cs)** | `yield return` з `IEnumerable<T>` | Повернення `IEnumerable<string>` та багаторазовий перебір |
| **[p503[SOLIS]](file:///03-Csharp-Mechanics/03-Enumerators-And-Iterators/p503[SOLIS]/Program.cs)** | Множинні ітератори | Методи `UVtoIR()` та `IRtoUV()` в одному класі |
| **[p504[SOLIS]](file:///03-Csharp-Mechanics/03-Enumerators-And-Iterators/p504[SOLIS]/Program.cs)** | Параметризований ітератор | Вибір напрямку обходу на базі конструктора |
| **[ex-03-Yield](file:///03-Csharp-Mechanics/03-Enumerators-And-Iterators/ex-03-Yield/Program.cs)** | Генерація послідовностей | `RandomNumberGenerator`, `yield break` та нескінченні потоки |

---

## 🛠️ Перевірка збірки (Zero-Error Verification)

Усі 10 проектів модуля оновлені до **.NET 10** та компілюються через централізоване рішення:
```powershell
dotnet build "03-Csharp-Mechanics/03-Enumerators-And-Iterators/OOP - Enumerators.sln" -c Release
```
