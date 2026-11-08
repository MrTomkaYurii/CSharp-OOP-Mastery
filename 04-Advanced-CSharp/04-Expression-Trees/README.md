# Підмодуль 04: Дерева виразів (Expression Trees) та Динамічний LINQ

Цей підмодуль логічно завершує розділ **`04-Advanced-CSharp`**, поєднуючи **Рефлексію** (`01-Reflection`) та **LINQ** (`03-LINQ`). Тут розглядається концепція **«Код як дані» (Code as Data)** через абстрактні синтаксичні дерева (`System.Linq.Expressions`).

---

## Навігація по навчальних матеріалах (`Docs`)

1. **[01. Анатомія дерев виразів (AST) та конструювання виразів у Runtime](Docs/01_Expression_Trees_AST_And_Dynamic_Building.md)**
2. **[02. Компіляція швидких делегатів та прискорення рефлексії (Expression Trees vs Invoke)](Docs/02_Compiled_Delegates_And_Reflection_Boosting.md)**
3. **[03. Динамічні LINQ-запити, комбінування предикатів та ExpressionVisitor (Місток до IQueryable)](Docs/03_Dynamic_LINQ_Predicates_And_IQueryable_Translation.md)**

---

## Структура Solution-тек у рішенні `OOP - Expression Trees.sln`

| Solution-тека | Проєкт | Ключові теми та API | Вихідний код |
| :--- | :--- | :--- | :--- |
| **`01. Анатомія дерев виразів (AST) та ручне конструювання`** | `15.0-ExpressionTreesBasicsDemo` | `Func<T>` проти `Expression<Func<T>>`, інспекція вузлів `NodeType`, `Parameters`, `Body`, метод `.Compile()` | [`Program.cs`](15.0-ExpressionTreesBasicsDemo/Program.cs) |
| **`01. Анатомія дерев виразів (AST) та ручне конструювання`** | `15.1-ExpressionTreesBuildingDemo` | Побудова AST з нуля: `Parameter`, `Constant`, `Multiply`, `Add`, `Condition` (`if-else`) | [`Program.cs`](15.1-ExpressionTreesBuildingDemo/Program.cs) |
| **`02. Компіляція швидких делегатів та бустінг рефлексії`** | `00-Shared` | Спільні доменні моделі (`Car`, `SportsCar`, `ICar`) | [`Car.cs`](00-Shared/Common/Car.cs) |
| **`02. Компіляція швидких делегатів та бустінг рефлексії`** | `15.2-ExpressionTreesMemberAccessDemo` | Генерація швидких геттерів/сеттерів `Expression.Property`, викликів методів `Expression.Call` та фабрик `Expression.New` | [`Program.cs`](15.2-ExpressionTreesMemberAccessDemo/Program.cs) |
| **`02. Компіляція швидких делегатів та бустінг рефлексії`** | `15.3-ReflectionPerformanceDemo` | Бенчмарк швидкодії: прямий виклик vs делегат vs скомпільований `Expression` vs `MethodInfo.Invoke` | [`Program.cs`](15.3-ReflectionPerformanceDemo/Program.cs) |
| **`03. Динамічні LINQ-предикати та трансляція IQueryable`** | `15.4-DynamicLinqPredicateBuilderDemo` | Динамічний фільтр для `IQueryable<T>`, об'єднання предикатів (`And`/`Or`) через `ExpressionVisitor` та власний транслятор у SQL `WHERE` | [`Program.cs`](15.4-DynamicLinqPredicateBuilderDemo/Program.cs) |

---

## Збірка та запуск

```powershell
dotnet build "04-Advanced-CSharp/04-Expression-Trees/OOP - Expression Trees.sln" -c Release
dotnet run --project "04-Advanced-CSharp/04-Expression-Trees/15.4-DynamicLinqPredicateBuilderDemo/15.4-DynamicLinqPredicateBuilderDemo.csproj"
```
