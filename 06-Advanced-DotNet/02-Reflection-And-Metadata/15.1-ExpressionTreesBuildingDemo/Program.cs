using System.Linq.Expressions;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 15.1. РУЧНЕ КОНСТРУЮВАННЯ ДЕРЕВ ВИРАЗІВ (MANUAL EXPRESSION BUILDING)");
Console.WriteLine(" Створення математичних формул, умовних виразів та методів через API Expression");
Console.WriteLine("==========================================================================\n");

// -----------------------------------------------------------------------
// Крок 1: Побудова математичної функції f(x) = (x * 5) + 10 без лямбда-синтаксису
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 1: Програмна побудова формули f(x) = (x * 5) + 10 ---\n");

// 1. Оголошуємо змінну-параметр: double x
ParameterExpression xParam = Expression.Parameter(typeof(double), "x");

// 2. Оголошуємо константи: 5.0 та 10.0
ConstantExpression fiveConst = Expression.Constant(5.0, typeof(double));
ConstantExpression tenConst = Expression.Constant(10.0, typeof(double));

// 3. Створюємо операцію множення: (x * 5.0)
BinaryExpression multiplyExpr = Expression.Multiply(xParam, fiveConst);

// 4. Створюємо операцію додавання: ((x * 5.0) + 10.0)
BinaryExpression addExpr = Expression.Add(multiplyExpr, tenConst);

// 5. Обгортаємо вираз у лямбду: (x) => ((x * 5.0) + 10.0)
Expression<Func<double, double>> linearFunctionExpr = Expression.Lambda<Func<double, double>>(addExpr, xParam);

Console.WriteLine($"Сконструйований вираз: {linearFunctionExpr}");
Console.WriteLine($"Тип тіла виразу:       {linearFunctionExpr.Body.GetType().Name}");

// 6. Компіляція в нативний машинний код
Func<double, double> linearFunc = linearFunctionExpr.Compile();

Console.WriteLine($"Розрахунок для x = 2.0:  f(2)  = {linearFunc(2.0)} (Очікувано: 20)");
Console.WriteLine($"Розрахунок для x = 6.0:  f(6)  = {linearFunc(6.0)} (Очікувано: 40)");
Console.WriteLine($"Розрахунок для x = 10.0: f(10) = {linearFunc(10.0)} (Очікувано: 60)\n");

// -----------------------------------------------------------------------
// Крок 2: Побудова логічних розгалужень через Expression.Condition (if-else)
// -----------------------------------------------------------------------
Console.WriteLine("--------------------------------------------------------------------------");
Console.WriteLine("--- Крок 2: Побудова логіки зі знижкою через Expression.Condition (if-else) ---");
Console.WriteLine("--------------------------------------------------------------------------\n");

// Задача: Розрахувати вартість замовлення.
// Якщо (price * quantity) >= 1000m, застосувати знижку 10% (помножити на 0.9m),
// інакше сплачується повна вартість.

// 1. Вхідні параметри: decimal price, int quantity
ParameterExpression priceParam = Expression.Parameter(typeof(decimal), "price");
ParameterExpression quantityParam = Expression.Parameter(typeof(int), "quantity");

// 2. Перетворення int quantity в decimal для коректної арифметики
UnaryExpression quantityDecimal = Expression.Convert(quantityParam, typeof(decimal));

// 3. Базова сума: price * (decimal)quantity
BinaryExpression rawTotal = Expression.Multiply(priceParam, quantityDecimal);

// 4. Умова: rawTotal >= 1000.0m
ConstantExpression threshold = Expression.Constant(1000.0m, typeof(decimal));
BinaryExpression condition = Expression.GreaterThanOrEqual(rawTotal, threshold);

// 5. Гілка True: rawTotal * 0.90m (знижка 10%)
ConstantExpression discountMultiplier = Expression.Constant(0.90m, typeof(decimal));
BinaryExpression discountedTotal = Expression.Multiply(rawTotal, discountMultiplier);

// 6. Тернарний вузол Expression.Condition(тест, якщо_true, якщо_false)
ConditionalExpression ifThenElseExpr = Expression.Condition(condition, discountedTotal, rawTotal);

// 7. Складання повної лямбди: (price, quantity) => if (price * qty >= 1000) ...
var orderCalcExpr = Expression.Lambda<Func<decimal, int, decimal>>(ifThenElseExpr, priceParam, quantityParam);

Console.WriteLine($"Сконструйований умовний вираз:\n  {orderCalcExpr}\n");

Func<decimal, int, decimal> calculateFinalPrice = orderCalcExpr.Compile();

// Тест 1: 5 одиниць по 100 грн = 500 грн (менше 1000 -> без знижки)
decimal order1 = calculateFinalPrice(100.0m, 5);
Console.WriteLine($"Замовлення 1 (100 грн x 5 шт = 500 грн):  До сплати = {order1:C} (без знижки)");

// Тест 2: 5 одиниць по 300 грн = 1500 грн (більше 1000 -> знижка 10% = 1350 грн)
decimal order2 = calculateFinalPrice(300.0m, 5);
Console.WriteLine($"Замовлення 2 (300 грн x 5 шт = 1500 грн): До сплати = {order2:C} (знижка 10% врахована!)");

Console.WriteLine("\n==========================================================================");
Console.WriteLine(" Підсумок: За допомогою статичних методів класу Expression можна динамічно");
Console.WriteLine(" конструювати будь-які алгоритми, бізнес-правила та фільтри прямо в рантаймі.");
Console.WriteLine("==========================================================================");
