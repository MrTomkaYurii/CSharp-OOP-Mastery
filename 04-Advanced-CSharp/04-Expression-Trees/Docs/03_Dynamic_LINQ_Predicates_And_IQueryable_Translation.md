# 03. Динамічні LINQ-запити, комбінування предикатів та ExpressionVisitor (Місток до IQueryable)

Цей розділ з'єднує **LINQ** (підмодуль `03-LINQ`) та **Дерева виразів (Expression Trees)** у єдину архітектурну картину.

---

## 1. Головна відмінність: `IEnumerable<T>` проти `IQueryable<T>`

Коли ви пишете лямбда-вираз `p => p.Price > 10000 && p.IsInStock`, компілятор C# може перетворити його у дві абсолютно різні речі залежно від типу цільового параметра методу розширення:

| Характеристика | `IEnumerable<T>.Where(Func<T, bool>)` | `IQueryable<T>.Where(Expression<Func<T, bool>>)` |
| :--- | :--- | :--- |
| **Тип параметра** | Скомпільований делегат `Func<T, bool>` | Дерево виразів `Expression<Func<T, bool>>` |
| **Що генерує компілятор** | Метод із байт-кодом CIL (чорний ящик) | Граф об'єктів AST (`BinaryExpression`, `MemberExpression`, `ConstantExpression`) |
| **Де виконується запит** | У пам'яті процесу .NET (In-Memory / LINQ to Objects) | Транслюється провайдером (EF Core, LINQ to DB) у мову цільової СКБД (SQL) |

---

## 2. Динамічна побудова фільтрів пошуку у Runtime

У реальних системах (інтернет-магазини, CRM, адмін-панелі) користувач обирає довільну комбінацію фільтрів. Замість склеювання SQL-рядків (що призводить до SQL-ін'єкцій), ми конструюємо `Expression<Func<Product, bool>>` вузол за вузлом:

```csharp
ParameterExpression param = Expression.Parameter(typeof(Product), "p");
Expression body = Expression.Constant(true);

if (!string.IsNullOrWhiteSpace(filter.Category))
{
    MemberExpression prop = Expression.Property(param, nameof(Product.Category));
    ConstantExpression val = Expression.Constant(filter.Category);
    body = Expression.AndAlso(body, Expression.Equal(prop, val));
}

var lambda = Expression.Lambda<Func<Product, bool>>(body, param);
```

---

## 3. Патерн `ExpressionVisitor` та об'єднання предикатів (`ParameterRebinder`)

Оскільки дерева виразів у .NET є **незмінними (Immutable)**, для модифікації або комбінування двох виразів `expr1 = x => ...` та `expr2 = y => ...` використовується базовий клас `ExpressionVisitor`. Він рекурсивно обходить дерево і підміняє параметр `y` другого виразу на параметр `x` першого виразу перед викликом `Expression.AndAlso` або `Expression.OrElse`.

Саме за цим же принципом `SqlWhereTranslator : ExpressionVisitor` обходить AST-дерево C# і генерує валідний вираз `WHERE` для SQL!

---

## Матриця проєктів цієї секції

| Проєкт | Призначення та ключові концепції | Вихідний код |
| :--- | :--- | :--- |
| **`15.4-DynamicLinqPredicateBuilderDemo`** | Порівняння `Func` vs `Expression<Func>`, динамічний конструктор фільтрів `DynamicProductFilter`, комбінатор специфікацій `And`/`Or` через `ParameterRebinder` та власний транслятор `SqlWhereTranslator` у SQL | [`Program.cs`](../15.4-DynamicLinqPredicateBuilderDemo/Program.cs) | 