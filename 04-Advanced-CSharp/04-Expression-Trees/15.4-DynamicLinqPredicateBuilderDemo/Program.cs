using System.Linq.Expressions;
using System.Text;

Console.OutputEncoding = Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 15.4. МІСТОК МІЖ LINQ ТА EXPRESSION TREES: ДИНАМІЧНІ ЗАПИТИ ТА IQUERYABLE");
Console.WriteLine(" Побудова предикатів на льоту, ExpressionVisitor та трансляція у SQL");
Console.WriteLine("==========================================================================\n");

var catalog = new List<Product>
{
    new(1, "Ноутбук ProBook 16", "Electronics", 42000m, true, 4.8),
    new(2, "Механічна клавіатура Keychron", "Electronics", 4500m, true, 4.9),
    new(3, "Монітор UltraWide 34\"", "Electronics", 19500m, false, 4.7),
    new(4, "Ергономічне крісло Aero", "Furniture", 14200m, true, 4.6),
    new(5, "Стіл з регулюванням висоти", "Furniture", 18000m, true, 4.9),
    new(6, "Настільна лампа LED", "Home", 1200m, true, 4.3)
};

// -----------------------------------------------------------------------
// Крок 1: IEnumerable<T> (Func) проти IQueryable<T> (Expression<Func>)
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 1: Чому IQueryable<T> використовує Expression<Func<T, bool>> ---");

Func<Product, bool> inMemoryDelegate = p => p.Price > 10000m && p.IsInStock;
Expression<Func<Product, bool>> queryExpression = p => p.Price > 10000m && p.IsInStock;

Console.WriteLine($"1. Делегат Func<Product, bool> (для IEnumerable): {inMemoryDelegate}");
Console.WriteLine("   -> Це скомпільований чорний ящик IL-коду. Ми можемо лише викликати його в RAM.");
Console.WriteLine($"2. Дерево виразів Expression<Func<Product, bool>> (для IQueryable): {queryExpression}");
Console.WriteLine($"   -> Це прозора структура даних (AST) з кореневим вузлом '{queryExpression.Body.NodeType}', яку можна розібрати!\n");

// -----------------------------------------------------------------------
// Крок 2: Динамічний конструктор фільтрів для LINQ (Dynamic Filter Builder)
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 2: Динамічна побудова фільтра за критеріями користувача ---");

var filterCriteria = new ProductSearchFilter(
    Category: "Electronics",
    MinPrice: 3000m,
    MaxPrice: 50000m,
    OnlyInStock: true);

Expression<Func<Product, bool>> dynamicPredicate = DynamicProductFilter.BuildPredicate(filterCriteria);
Console.WriteLine($"Згенероване дерево виразів AST:\n  {dynamicPredicate}\n");

// Застосування до IQueryable / IEnumerable
List<Product> filteredProducts = catalog
    .AsQueryable()
    .Where(dynamicPredicate)
    .ToList();

Console.WriteLine("Результати виконання динамічного LINQ-запиту:");
foreach (var item in filteredProducts)
{
    Console.WriteLine($"  - [{item.Category}] {item.Name} | Ціна: {item.Price} грн | В наявності: {item.IsInStock}");
}
Console.WriteLine();

// -----------------------------------------------------------------------
// Крок 3: Комбінування двох виразів (AndAlso / OrElse) через ExpressionVisitor
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 3: Комбінування незалежних предикатів через ParameterRebinder ---");

Expression<Func<Product, bool>> highRatedSpec = x => x.Rating >= 4.8;
Expression<Func<Product, bool>> affordableFurnitureSpec = y => y.Category == "Furniture" && y.Price <= 20000m;

// Об'єднуємо два предикати з різними параметрами (x та y) в єдине дерево за допомогою OrElse
Expression<Func<Product, bool>> combinedSpec = dynamicPredicate
    .And(highRatedSpec)
    .Or(affordableFurnitureSpec);

Console.WriteLine($"Об'єднаний предикат (Specification Pattern):\n  {combinedSpec}\n");

foreach (var item in catalog.AsQueryable().Where(combinedSpec))
{
    Console.WriteLine($"  * Підходить під комбіновану специфікацію: {item.Name} ({item.Category}, ★ {item.Rating}, {item.Price} грн)");
}
Console.WriteLine();

// -----------------------------------------------------------------------
// Крок 4: Як працює ORM (EF Core): Трансляція Expression Tree у SQL WHERE
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 4: Власний транслятор Expression Tree -> SQL WHERE ---");
string sqlWhere = SqlWhereTranslator.Translate(combinedSpec);
Console.WriteLine("Трансльований SQL-запит:");
Console.WriteLine($"  SELECT Id, Name, Category, Price, IsInStock, Rating");
Console.WriteLine($"  FROM Products");
Console.WriteLine($"  WHERE {sqlWhere};");

// =======================================================================
// Доменні моделі та інфраструктура Expression Trees
// =======================================================================
public record Product(int Id, string Name, string Category, decimal Price, bool IsInStock, double Rating);

public record ProductSearchFilter(
    string? Category = null,
    decimal? MinPrice = null,
    decimal? MaxPrice = null,
    bool OnlyInStock = false);

public static class DynamicProductFilter
{
    public static Expression<Func<Product, bool>> BuildPredicate(ProductSearchFilter filter)
    {
        ParameterExpression param = Expression.Parameter(typeof(Product), "p");
        Expression body = Expression.Constant(true);

        if (!string.IsNullOrWhiteSpace(filter.Category))
        {
            MemberExpression categoryProp = Expression.Property(param, nameof(Product.Category));
            ConstantExpression categoryVal = Expression.Constant(filter.Category, typeof(string));
            body = Expression.AndAlso(body, Expression.Equal(categoryProp, categoryVal));
        }

        if (filter.MinPrice.HasValue)
        {
            MemberExpression priceProp = Expression.Property(param, nameof(Product.Price));
            ConstantExpression minVal = Expression.Constant(filter.MinPrice.Value, typeof(decimal));
            body = Expression.AndAlso(body, Expression.GreaterThanOrEqual(priceProp, minVal));
        }

        if (filter.MaxPrice.HasValue)
        {
            MemberExpression priceProp = Expression.Property(param, nameof(Product.Price));
            ConstantExpression maxVal = Expression.Constant(filter.MaxPrice.Value, typeof(decimal));
            body = Expression.AndAlso(body, Expression.LessThanOrEqual(priceProp, maxVal));
        }

        if (filter.OnlyInStock)
        {
            MemberExpression stockProp = Expression.Property(param, nameof(Product.IsInStock));
            body = Expression.AndAlso(body, stockProp);
        }

        return Expression.Lambda<Func<Product, bool>>(body, param);
    }
}

public static class PredicateBuilderExtensions
{
    public static Expression<Func<T, bool>> And<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        ParameterExpression parameter = left.Parameters[0];
        Expression rewrittenRight = new ParameterRebinder(right.Parameters[0], parameter).Visit(right.Body);
        return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left.Body, rewrittenRight), parameter);
    }

    public static Expression<Func<T, bool>> Or<T>(
        this Expression<Func<T, bool>> left,
        Expression<Func<T, bool>> right)
    {
        ParameterExpression parameter = left.Parameters[0];
        Expression rewrittenRight = new ParameterRebinder(right.Parameters[0], parameter).Visit(right.Body);
        return Expression.Lambda<Func<T, bool>>(Expression.OrElse(left.Body, rewrittenRight), parameter);
    }

    private sealed class ParameterRebinder(ParameterExpression from, ParameterExpression to) : ExpressionVisitor
    {
        protected override Expression VisitParameter(ParameterExpression node)
            => node == from ? to : base.VisitParameter(node);
    }
}

public sealed class SqlWhereTranslator : ExpressionVisitor
{
    private readonly StringBuilder _sb = new();

    public static string Translate<T>(Expression<Func<T, bool>> predicate)
    {
        var translator = new SqlWhereTranslator();
        translator.Visit(predicate.Body);
        return translator._sb.ToString();
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        _sb.Append('(');
        Visit(node.Left);
        _sb.Append(node.NodeType switch
        {
            ExpressionType.AndAlso => " AND ",
            ExpressionType.OrElse => " OR ",
            ExpressionType.Equal => " = ",
            ExpressionType.NotEqual => " <> ",
            ExpressionType.GreaterThan => " > ",
            ExpressionType.GreaterThanOrEqual => " >= ",
            ExpressionType.LessThan => " < ",
            ExpressionType.LessThanOrEqual => " <= ",
            _ => throw new NotSupportedException($"Оператор {node.NodeType} не підтримується")
        });
        Visit(node.Right);
        _sb.Append(')');
        return node;
    }

    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Type == typeof(bool) && node.Expression is ParameterExpression)
        {
            _sb.Append($"[{node.Member.Name}] = 1");
            return node;
        }
        _sb.Append($"[{node.Member.Name}]");
        return node;
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        if (node.Value is string s)
            _sb.Append($"'{s}'");
        else if (node.Value is bool b)
            _sb.Append(b ? "1" : "0");
        else if (node.Value is IFormattable formattable)
            _sb.Append(formattable.ToString(null, System.Globalization.CultureInfo.InvariantCulture));
        else
            _sb.Append(node.Value ?? "NULL");
        return node;
    }
}
