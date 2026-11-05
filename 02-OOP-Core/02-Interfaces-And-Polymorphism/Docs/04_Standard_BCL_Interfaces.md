# 04. Стандартні інтерфейси .NET BCL (`IComparable`, `IComparer`, `IEnumerable`, `ICloneable`, `IDisposable`)

Стандартна бібліотека базових класів (.NET Base Class Library — BCL) побудована навколо канонічних інтерфейсів, реалізація яких інтегрує власні класи у вбудовані механізми мови C# та платформи .NET.

---

## 1. Головні контракти BCL

| Інтерфейс | Яку проблему вирішує | Де використовується в .NET |
|:---|:---|:---|
| **`IComparable` / `IComparable<T>`** | Задає «природний» (основний) порядок сортування об'єктів (`CompareTo`). | `Array.Sort()`, `List<T>.Sort()`, `SortedSet<T>` |
| **`IComparer` / `IComparer<T>`** | Зовнішні стратегії сортування за різними критеріями (за віком, ціною, у зворотному порядку). | `Array.Sort(arr, comparer)`, `OrderBy(..., comparer)` |
| **`IEnumerable` / `IEnumerator`** | Перебір елементів колекції за допомогою курсора (`MoveNext()`, `Current`) або генератора `yield return`. | Цикл `foreach`, усі оператори LINQ |
| **`ICloneable`** | Створення поверхневої (`MemberwiseClone`) або глибокої копії об'єкта. | Дублювання графів об'єктів без мутації оригіналу |
| **`IDisposable`** | Детерміноване звільнення некерованих ресурсів (файлів, сокетів, дескрипторів ОС). | Конструкція `using (...)` та `using var` |

---

## Матриця проєктів цієї секції

| Проєкт | Інтерфейс / Сторінка | Що демонструє |
|:---|:---|:---|
| [`p390[SOLIS]`](../p390[SOLIS]/Program.cs) | Solis, с. 390 (`IComparable`) | Як `Array.Sort(myInt)` використовує вбудований інтерфейс `IComparable` у типі `int`. |
| [`p393[SOLIS]`](../p393[SOLIS]/Program.cs) | Solis, с. 393 (`IComparable`) | Реалізація `IComparable.CompareTo` у власному класі `MyClass` для роботи `Array.Sort(mcArr)`. |
| [`ex-Icomparable01`](../ex-Icomparable/Program.cs) | `IComparable` | Сортування масиву співробітників `Employee[]` за зростанням `ID`. |
| [`ex-IComparable02`](../ex-IComparable02/Program.cs) | `IComparable<T>` | Типізоване порівняння об'єктів без боксингу та кастингу з `object`. |
| [`ex-IComparer01`](../ex-IComparer01/Program.cs) | `IComparer<T>` | Окремі зовнішні компаратори для сортування колекції за різними полями. |
| [`ex-Icomparer02`](../ex-Icomparer02/Program.cs) | `IComparer` | Зворотне регістронезалежне сортування рядків через власний `ReverserClass`. |
| [`ex-IEnumerable01`](../ex-IEnumerable01/Program.cs) | `IEnumerable` + `IEnumerator` | Повна ручна реалізація колекції `Playlist` та курсора `PlaylistEnumerator` (`MoveNext`, `Current`, `Reset`). |
| [`ex-IEnumerable02`](../ex-IEnumerable02/Program.cs) | `IEnumerable` + `yield return` | Автоматична генерація кінцевого автомата перебирача компілятором через лінивий `yield return`. |
| [`ex-IClonable01`](../ex-IClonable01/Program.cs) | `ICloneable` (Shallow vs Deep) | Різниця між поверхневим копіюванням (`MemberwiseClone`) та глибоким клонуванням вкладених об'єктів. |
| [`ex-IClonable02`](../ex-IClonable02/Program.cs) | `ICloneable` | Практичний приклад клонування складної доменної моделі. |
| [`ex-IDisposable01`](../ex-IDisposable01/Program.cs) | `IDisposable` | Базовий контракт очищення ресурсів та виклик `Dispose()` через `using`. |
| [`ex-IDisposable02`](../ex-IDisposable02/Program.cs) | `IDisposable` (`LogWriter`) | Повноцінний захист від повторного `Dispose()` та генерація `ObjectDisposedException` при спробі запису у закритий файл. | 