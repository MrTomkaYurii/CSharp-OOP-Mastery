# Модуль 06: Колекції в C# та .NET (.NET 10)

Ласкаво просимо до навчального модуля, присвяченого колекціям та структурам даних у C#. Тут ви опануєте еволюцію від класичних масивів та legacy-колекцій (`ArrayList`, `Hashtable`) до сучасних узагальнених структур (`List<T>`, `Dictionary<TKey, TValue>`, `SortedList<TKey, TValue>`).

---

## 🗺️ Карта підручника (Навчальні гайди)

1. **[01. Масиви: одновимірні, багатовимірні та зубчасті (Jagged)](Docs/01_Arrays_Multidimensional_And_Jagged.md)**
   - Базовий устрій масивів у пам'яті, порівняння прямокутних `[,]` та зубчастих `[][]` масивів.
2. **[02. Неузагальнені колекції та накладні витрати Boxing/Unboxing](Docs/02_NonGeneric_Legacy_Collections.md)**
   - `ArrayList`, `Hashtable`, тиск на GC та втрата компіляторної безпеки типів.
3. **[03. Узагальнений список List<T> та стратегія розширення](Docs/03_Generic_List_And_Dynamic_Resizing.md)**
   - Внутрішній буфер `_items`, динамічне подвоєння ємності (`Capacity`), методи швидкого пошуку.
4. **[04. Словники та сортовані структури: Dictionary та SortedList](Docs/04_Dictionaries_And_Sorted_Collections.md)**
   - Кошики хеш-таблиць, бінарний пошук у впорядкованих колекціях, оцінка асимптотичної складності Big-O.

---

## 📂 Структура проектів модуля

| Проект | Опис теми | Ключові концепції |
| :--- | :--- | :--- |
| **[01-01-Array](file:///03-Csharp-Mechanics/02-Collections/01-01-Array/Program.cs)** | Одновимірні масиви | `Array`, індексація, властивість `Length` |
| **[01-02-2DArray](file:///03-Csharp-Mechanics/02-Collections/01-02-2DArray/Program.cs)** | Двовимірні прямокутні масиви | `int[,]`, `GetLength(0)`, `GetLength(1)` |
| **[01-03-JaggedArray](file:///03-Csharp-Mechanics/02-Collections/01-03-JaggedArray/Program.cs)** | Зубчасті масиви (масиви масивів) | `int[][]`, неоднорідна довжина рядків |
| **[02-ArrayList](file:///03-Csharp-Mechanics/02-Collections/02-ArrayList/Program.cs)** | Базовий `ArrayList` | Додавання різнорідних типів, boxing |
| **[02-01-ArrayList-Access](file:///03-Csharp-Mechanics/02-Collections/02-01-ArrayList-Access/Program.cs)** | Доступ до елементів `ArrayList` | Необхідність явного приведення типів `(int)` |
| **[02-03-ArrayList-Iterate](file:///03-Csharp-Mechanics/02-Collections/02-03-ArrayList-Iterate/Program.cs)** | Ітерація по `ArrayList` | Обхід через `foreach` та `IEnumerator` |
| **[02-04-ArrayList-CloneTheNon-GenericArrayList](file:///03-Csharp-Mechanics/02-Collections/02-04-ArrayList-CloneTheNon-GenericArrayList/Program.cs)** | Клонування колекції | Метод `Clone()` (shallow copy) |
| **[03-01-HashTable](file:///03-Csharp-Mechanics/02-Collections/03-01-HashTable/Program.cs)** | Базова `Hashtable` | Па́ри ключ-значення на базі `object` |
| **[03-02-HashTable-Add](file:///03-Csharp-Mechanics/02-Collections/03-02-HashTable-Add/Program.cs)** | Додавання пар у `Hashtable` | Методи `.Add()` та індексатор `table[key]` |
| **[03-03-HashTable-CloneHashtable](file:///03-Csharp-Mechanics/02-Collections/03-03-HashTable-CloneHashtable/Program.cs)** | Поверхнева копія `Hashtable` | Метод `Clone()` |
| **[03-04-HashTable-CopyHashtableToArray](file:///03-Csharp-Mechanics/02-Collections/03-04-HashTable-CopyHashtableToArray/Program.cs)** | Експорт пар у масив | Метод `CopyTo()` та тип `DictionaryEntry` |
| **[04-01-GenericList-Create](file:///03-Csharp-Mechanics/02-Collections/04-01-GenericList-Create/Program.cs)** | Створення `List<T>` | Взаємозв'язок `Count` та `Capacity` |
| **[04-02-GenericList-Insert](file:///03-Csharp-Mechanics/02-Collections/04-02-GenericList-Insert/Program.cs)** | Модифікація `List<T>` | `Insert`, `InsertRange`, `Remove`, `RemoveAt` |
| **[04-03-GenericList-FindElement](file:///03-Csharp-Mechanics/02-Collections/04-03-GenericList-FindElement/Program.cs)** | Пошук у `List<T>` | `Find`, `FindAll`, `Exists`, `TrueForAll` |
| **[05-01-SortedList-Сreate](file:///03-Csharp-Mechanics/02-Collections/05-01-SortedList-Сreate/Program.cs)** | Створення `SortedList<TKey, TValue>` | Автоматичне впорядкування за ключем |
| **[05-02-SortedList-Add](file:///03-Csharp-Mechanics/02-Collections/05-02-SortedList-Add/Program.cs)** | Додавання ключів | Заборона дублікатів ключів |
| **[05-03-SortedList-Remove](file:///03-Csharp-Mechanics/02-Collections/05-03-SortedList-Remove/Program.cs)** | Видалення елементів | Видалення за ключем та за індексом |
| **[05-04-SortedList-CheckTheAvailability](file:///03-Csharp-Mechanics/02-Collections/05-04-SortedList-CheckTheAvailability/Program.cs)** | Перевірка наявності | `ContainsKey`, `ContainsValue` |
| **[GenericDictionary-06-01](file:///03-Csharp-Mechanics/02-Collections/GenericDictionary-06-01/Program.cs)** | Сучасний `Dictionary<TKey, TValue>` | `TryGetValue`, швидкий $O(1)$ доступ |

---

## 🛠️ Перевірка збірки (Zero-Error Verification)

Усі проекти оновлені до **.NET 10** з увімкненим `Nullable: enable` та `ImplicitUsings: enable`.

Збірка здійснюється командою:
```powershell
dotnet build "03-Csharp-Mechanics/02-Collections/OOP-CollectionsInC#.slnx" -c Release
```
