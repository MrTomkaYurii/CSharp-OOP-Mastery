# 04. Dynamic Execution & Performance: Загальний огляд та зріз методів

> **Solution-тег (Solution Folder)**: `04. Dynamic Execution & Performance`  
> **Проєкти у тегу**:
> - [14-LateBindingDemo/Program.cs](../../14-LateBindingDemo/Program.cs) — Пізнє зв'язування: `Activator.CreateInstance`, `RuntimeHelpers.GetUninitializedObject`, dynamic DLR.
> - [15.0-ExpressionTreesBasicsDemo/Program.cs](../../15.0-ExpressionTreesBasicsDemo/Program.cs) — Вступ до дерев виразів: код як дані, анатомія AST, метод `.Compile()`.
> - [15.1-ExpressionTreesBuildingDemo/Program.cs](../../15.1-ExpressionTreesBuildingDemo/Program.cs) — Програмне конструювання AST через фабричні методи: математика, if-else умови (`Expression.Condition`).
> - [15.2-ExpressionTreesMemberAccessDemo/Program.cs](../../15.2-ExpressionTreesMemberAccessDemo/Program.cs) — Бустінг рефлексії: швидкі геттери (`Expression.Property`), виклики (`Expression.Call`) та фабрики (`Expression.New`).
> - [15.3-ReflectionPerformanceDemo/Program.cs](../../15.3-ReflectionPerformanceDemo/Program.cs) — Бенчмарк: Direct Call vs Compiled Expression vs Delegate vs dynamic vs `MethodInfo.Invoke`.
> - [16-UnsafeAccessorDemo/Program.cs](../../16-UnsafeAccessorDemo/Program.cs) — JIT Compiler Intrinsic доступ до private (`[UnsafeAccessor]`, .NET 8+).

---

## 1. Концептуальний зріз розділу

Цей розділ фокусується на переході від повільної інтерпретованої рефлексії до високопродуктивних технологій динамічного виконання:
1. **Динамічна активація об'єктів**: використання `Activator` та `RuntimeHelpers`.
2. **Дерева виразів (Expression Trees)**: генерація коду в оперативній пам'яті через AST та компіляція у швидкі типізовані делегати (`DynamicMethod`).
3. **JIT Intrinsics**: асемблерна адресація закритих членів на нульових накладних витратах через `[UnsafeAccessor]`.

### Охоплені ключові класи:
- [Activator.md](Activator.md) — динамічне створення об'єктів (`System.Activator`, `System.Runtime.CompilerServices.RuntimeHelpers`).
- [Expression.md](Expression.md) — дерева виразів, фабричні методи AST та динамічна компіляція (`System.Linq.Expressions.Expression`).
- [UnsafeAccessor.md](UnsafeAccessor.md) — JIT-інтринсики для обходу інкапсуляції в .NET 8+.

---

## 2. Зведена матриця методів та механізмів виклику

| Клас / Атрибут | Метод / Властивість | Статус у курсі | Опис призначення | Приклад у коді | Документація MSDN |
| :--- | :--- | :---: | :--- | :--- | :--- |
| `Activator` | `CreateInstance(Type, object[])` | Використано | Динамічне створення екземпляра за типом та аргументами конструктора | [14-LateBindingDemo/Program.cs](../../14-LateBindingDemo/Program.cs#L14) | [Activator.CreateInstance](https://learn.microsoft.com/en-us/dotnet/api/system.activator.createinstance) |
| `Activator` | `CreateInstance<T>()` | Використано | Узагальнене створення екземпляра без перевантаження типізації | [14-LateBindingDemo/Program.cs](../../14-LateBindingDemo/Program.cs#L22) | [Activator.CreateInstance](https://learn.microsoft.com/en-us/dotnet/api/system.activator.createinstance) |
| `RuntimeHelpers` | `GetUninitializedObject(Type)` | Використано | Аллокація об'єкта в пам'яті БЕЗ виклику його конструктора (для серіалізаторів) | [14-LateBindingDemo/Program.cs](../../14-LateBindingDemo/Program.cs#L36) | [RuntimeHelpers.GetUninitializedObject](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.runtimehelpers.getuninitializedobject) |
| `Expression` | `Parameter(Type, string)` | Використано | Створення вузла вхідного параметра виразу чи лямбди | [15.1-ExpressionTreesBuildingDemo/Program.cs](../../15.1-ExpressionTreesBuildingDemo/Program.cs#L16) | [Expression.Parameter](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression.parameter) |
| `Expression` | `Constant(object, Type)` | Використано | Створення вузла літеральної константи | [15.1-ExpressionTreesBuildingDemo/Program.cs](../../15.1-ExpressionTreesBuildingDemo/Program.cs#L19) | [Expression.Constant](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression.constant) |
| `Expression` | `Multiply(left, right)` | Використано | Бінарна арифметична операція множення | [15.1-ExpressionTreesBuildingDemo/Program.cs](../../15.1-ExpressionTreesBuildingDemo/Program.cs#L23) | [Expression.Multiply](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression.multiply) |
| `Expression` | `Add(left, right)` | Використано | Бінарна арифметична операція додавання | [15.1-ExpressionTreesBuildingDemo/Program.cs](../../15.1-ExpressionTreesBuildingDemo/Program.cs#L26) | [Expression.Add](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression.add) |
| `Expression` | `Condition(test, ifTrue, ifFalse)`| Використано | Вузол умовного розгалуження (if-then-else / тернарний оператор) | [15.1-ExpressionTreesBuildingDemo/Program.cs](../../15.1-ExpressionTreesBuildingDemo/Program.cs#L71) | [Expression.Condition](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression.condition) |
| `Expression` | `Property(instance, PropertyInfo)`| Використано | Створення вузла доступу до властивості об'єкта | [15.2-ExpressionTreesMemberAccessDemo/Program.cs](../../15.2-ExpressionTreesMemberAccessDemo/Program.cs#L27) | [Expression.Property](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression.property) |
| `Expression` | `Call(instance, MethodInfo, args)`| Використано | Створення вузла виклику методу | [15.2-ExpressionTreesMemberAccessDemo/Program.cs](../../15.2-ExpressionTreesMemberAccessDemo/Program.cs#L53) | [Expression.Call](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression.call) |
| `Expression` | `New(ConstructorInfo, args)` | Використано | Створення вузла виклику конструктора (`new T(...)`) | [15.2-ExpressionTreesMemberAccessDemo/Program.cs](../../15.2-ExpressionTreesMemberAccessDemo/Program.cs#L81) | [Expression.New](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression.new) |
| `Expression` | `Lambda<TDelegate>(body, params)`| Використано | Створення кореневого лямбда-виразу зі списком параметрів | [15.0-ExpressionTreesBasicsDemo/Program.cs](../../15.0-ExpressionTreesBasicsDemo/Program.cs#L26) | [Expression.Lambda](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.expression.lambda) |
| `LambdaExpression`| `Compile()` | Використано | JIT-компіляція дерева виразів у виконуваний машинний код делегата | [15.0-ExpressionTreesBasicsDemo/Program.cs](../../15.0-ExpressionTreesBasicsDemo/Program.cs#L83) | [LambdaExpression.Compile](https://learn.microsoft.com/en-us/dotnet/api/system.linq.expressions.lambdaexpression.compile) |
| `Delegate` | `CreateDelegate(Type, MethodInfo)`| Використано | Створення прямого типізованого покажчика на метод | [15.3-ReflectionPerformanceDemo/Program.cs](../../15.3-ReflectionPerformanceDemo/Program.cs#L26) | [Delegate.CreateDelegate](https://learn.microsoft.com/en-us/dotnet/api/system.delegate.createdelegate) |
| `[UnsafeAccessor]` | `UnsafeAccessorKind.Field` | Використано | JIT-інтринсик для прямого доступу до приватного поля за посиланням `ref` | [16-UnsafeAccessorDemo/Program.cs](../../16-UnsafeAccessorDemo/Program.cs#L22) | [UnsafeAccessorAttribute](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.unsafeaccessorattribute) |
| `[UnsafeAccessor]` | `UnsafeAccessorKind.Method`| Використано | JIT-інтринсик для прямого виклику приватного методу | [16-UnsafeAccessorDemo/Program.cs](../../16-UnsafeAccessorDemo/Program.cs#L30) | [UnsafeAccessorKind](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.unsafeaccessorkind) |
| `[UnsafeAccessor]` | `UnsafeAccessorKind.Constructor`| Використано | JIT-інтринсик для інстанціювання через приватний конструктор | [16-UnsafeAccessorDemo/Program.cs](../../16-UnsafeAccessorDemo/Program.cs#L38) | [UnsafeAccessorKind](https://learn.microsoft.com/en-us/dotnet/api/system.runtime.compilerservices.unsafeaccessorkind) |

---

## 3. Статистичний зріз охоплення API

- **Методів та механізмів досліджено в прикладах**: 17
- **Загальних методів API BCL задокументовано додатково**: 3
- **Сумарно в довіднику розділу**: 20 API елементів
- **Відсоток фокусного практичного охоплення**: ~85%
