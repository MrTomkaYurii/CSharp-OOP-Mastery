namespace AttributesBasicsDemo;

/// <summary>
/// 1. Атрибут документування та версіонування коду.
/// Демонструє:
/// - AllowMultiple = true (можна вказати декілька авторів для одного класу або методу).
/// - Позиційні параметри (передаються через конструктор: author).
/// - Іменовані параметри (передаються через публічні властивості: Version, LastModified).
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
public class AuthorInfoAttribute : Attribute
{
    // Позиційний параметр (обов'язковий при застосуванні атрибута)
    public string Author { get; }

    // Іменовані параметри (опціональні, задаються за іменем властивості)
    public string Version { get; set; } = "1.0";
    public string LastModified { get; set; } = "Не вказано";

    public AuthorInfoAttribute(string author)
    {
        Author = author;
    }
}

/// <summary>
/// 2. Атрибут для автоматичної реєстрації консольних команд (маршрутизація).
/// Саме так працюють контролери в ASP.NET Core ([HttpGet("users")]) або CLI-фреймворки.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class ConsoleCommandAttribute : Attribute
{
    public string CommandName { get; }
    public string Description { get; }

    public ConsoleCommandAttribute(string commandName, string description)
    {
        CommandName = commandName;
        Description = description;
    }
}

/// <summary>
/// 3. Атрибут контролю доступу (авторизації).
/// Дозволяє декларативно вказати, яка роль потрібна для виклику методу.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class RequiresPermissionAttribute : Attribute
{
    public string RequiredRole { get; }

    public RequiresPermissionAttribute(string requiredRole)
    {
        RequiredRole = requiredRole;
    }
}
