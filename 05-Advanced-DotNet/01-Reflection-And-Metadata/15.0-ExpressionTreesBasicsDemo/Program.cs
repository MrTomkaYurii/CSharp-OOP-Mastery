using System.Linq.Expressions;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 15.0. ВСТУП ДО ДЕРЕВ ВИРАЗІВ (EXPRESSION TREES): КОД ЯК ДАНІ");
Console.WriteLine(" Базові концепції, анатомія синтаксичного дерева та компіляція у рантаймі");
Console.WriteLine("==========================================================================\n");

// -----------------------------------------------------------------------
// Крок 1: Що таке дерево виразів? Делегат проти Expression Tree
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 1: Делегат (чорна скринька) vs Expression Tree (код як дані) ---\n");

// А. Звичайний делегат (Delegate)
// Компілятор C# одразу перетворює лямбду в скомпільований IL-байткод.
// Ми НЕ МОЖЕМО дослідити, що написано всередині addDelegate під час виконання програми.
Func<int, int, int> addDelegate = (a, b) => a + b;
Console.WriteLine($"1. Звичайний делегат: addDelegate(10, 20) = {addDelegate(10, 20)}");
Console.WriteLine($"   Тип об'єкта: {addDelegate.GetType().FullName}");
Console.WriteLine("   Неможливо дослідити внутрішню структуру: це скомпільований виконуваний код.\n");

// Б. Дерево виразів (Expression Tree)
// Компілятор C# бачить обгортку Expression<TDelegate> і замість генерації IL-коду
// створює в оперативній пам'яті структуру даних (граф об'єктів), що описує код.
Expression<Func<int, int, int>> addExpression = (a, b) => a + b;
Console.WriteLine($"2. Дерево виразів: addExpression = {addExpression}");
Console.WriteLine($"   Тип об'єкта: {addExpression.GetType().FullName}");
Console.WriteLine("   Це структура даних (AST - Abstract Syntax Tree), яку можна читати, змінювати або компілювати!\n");

// -----------------------------------------------------------------------
// Крок 2: Анатомія дерева виразів (розбір вузлів)
// -----------------------------------------------------------------------
Console.WriteLine("--------------------------------------------------------------------------");
Console.WriteLine("--- Крок 2: Анатомія дерева виразів (дослідження властивостей вузлів) ---");
Console.WriteLine("--------------------------------------------------------------------------\n");

Console.WriteLine($"Кореневий вузол (NodeType):       {addExpression.NodeType}");
Console.WriteLine($"Тип значення, що повертається:    {addExpression.ReturnType.Name}");
Console.WriteLine($"Кількість вхідних параметрів:     {addExpression.Parameters.Count}");

for (int i = 0; i < addExpression.Parameters.Count; i++)
{
    ParameterExpression param = addExpression.Parameters[i];
    Console.WriteLine($"  Параметр [{i}]: Назва = '{param.Name}', Тип = {param.Type.Name}, Вузол = {param.NodeType}");
}

// Тіло лямбда-виразу (Body)
BinaryExpression body = (BinaryExpression)addExpression.Body;
Console.WriteLine($"\nТіло виразу (Body):               {body}");
Console.WriteLine($"  Тип операції (NodeType):        {body.NodeType} (Додавання)");
Console.WriteLine($"  Лівий операнд (Left):           {body.Left} (Параметр: '{((ParameterExpression)body.Left).Name}')");
Console.WriteLine($"  Правий операнд (Right):         {body.Right} (Параметр: '{((ParameterExpression)body.Right).Name}')");

// -----------------------------------------------------------------------
// Крок 3: Візуалізація синтаксичного дерева в консолі
// -----------------------------------------------------------------------
Console.WriteLine("\n--------------------------------------------------------------------------");
Console.WriteLine("--- Крок 3: Графічна візуалізація структури синтаксичного дерева ---");
Console.WriteLine("--------------------------------------------------------------------------\n");

Console.WriteLine("  LambdaExpression: (a, b) => a + b");
Console.WriteLine("  |");
Console.WriteLine("  +-- Parameters: [a: int, b: int]");
Console.WriteLine("  |");
Console.WriteLine("  +-- Body: BinaryExpression (NodeType = Add)");
Console.WriteLine("        |");
Console.WriteLine("        +-- Left:  ParameterExpression (Name = 'a')");
Console.WriteLine("        |");
Console.WriteLine("        +-- Right: ParameterExpression (Name = 'b')\n");

// -----------------------------------------------------------------------
// Крок 4: Метод .Compile() — перетворення даних назад у виконуваний код
// -----------------------------------------------------------------------
Console.WriteLine("--------------------------------------------------------------------------");
Console.WriteLine("--- Крок 4: Метод .Compile() (трансляція дерева в нативний машинний код) ---");
Console.WriteLine("--------------------------------------------------------------------------\n");

Console.WriteLine("Викликаємо метод addExpression.Compile()...");
Console.WriteLine("У цей момент середовище CoreCLR через механізм DynamicMethod генерує");
Console.WriteLine("свіжий блок IL-інструкцій безпосередньо в оперативній пам'яті та компілює його JIT-ом.\n");

Func<int, int, int> compiledFunc = addExpression.Compile();

int result1 = compiledFunc(15, 25);
int result2 = compiledFunc(100, 250);

Console.WriteLine($"Результат виклику скомпільованого дерева compiledFunc(15, 25)  = {result1}");
Console.WriteLine($"Результат виклику скомпільованого дерева compiledFunc(100, 250) = {result2}");

Console.WriteLine("\n==========================================================================");
Console.WriteLine(" Підсумок: Дерево виразів дозволяє маніпулювати кодом як даними,");
Console.WriteLine(" аналізувати його складові та за потреби компілювати у високоефективний делегат.");
Console.WriteLine("==========================================================================");
