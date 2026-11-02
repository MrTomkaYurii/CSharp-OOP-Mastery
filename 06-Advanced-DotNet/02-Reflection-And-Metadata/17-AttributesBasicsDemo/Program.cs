using System.Reflection;
using AttributesBasicsDemo;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 17. ВСТУП ДО АТРИБУТІВ: ДЛЯ ЧОГО ВОНИ ПОТРІБНІ ТА ЯК ПРАЦЮЮТЬ");
Console.WriteLine(" Від базової концепції міток метаданих до практичних сценаріїв");
Console.WriteLine("==========================================================================\n");

// -----------------------------------------------------------------------
// Крок 1: Суть атрибутів та вбудовані атрибути компілятора
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 1: Що таке атрибут та як працюють вбудовані атрибути C# ---");
Console.WriteLine("Атрибут — це декларативний ярлик (стікер з метаданими), прикріплений до коду.");
Console.WriteLine("Сам по собі атрибут нічого не виконує, доки його не прочитає компілятор або рефлексія.\n");

var service = new PaymentReportService();

// Виклик застарілого методу з [Obsolete]
service.PrintOldFormatReport();

// Виклик методу з атрибутами [CallerMemberName] та [CallerLineNumber]
service.LogExecutionStep("Перевірка автоматичної підстановки метаданих компілятором");
Console.WriteLine();

// -----------------------------------------------------------------------
// Крок 2: Сценарій 1 — Документування та історія змін ([AuthorInfo])
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 2: Сценарій 1 — Зчитування паспорта класу ([AuthorInfo]) ---");
Type serviceType = typeof(PaymentReportService);

// Оскільки AllowMultiple = true, використовуємо GetCustomAttributes<T>() (у множині)
IEnumerable<AuthorInfoAttribute> authors = serviceType.GetCustomAttributes<AuthorInfoAttribute>();

Console.WriteLine($"Клас '{serviceType.Name}' містить такі записи про авторів у метаданих:");
foreach (AuthorInfoAttribute info in authors)
{
    Console.WriteLine($"  - Автор: {info.Author,-22} | Версія: {info.Version} | Дата: {info.LastModified}");
}
Console.WriteLine();

// -----------------------------------------------------------------------
// Крок 3: Сценарій 2 — Автоматична побудова меню команд ([ConsoleCommand])
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 3: Сценарій 2 — Автоматичне виявлення команд через [ConsoleCommand] ---");
Console.WriteLine("Замість ручного switch-case рефлексія сканує всі методи класу та шукає атрибут:\n");

MethodInfo[] methods = serviceType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

Console.WriteLine("Знайдені консольні команди:");
foreach (MethodInfo method in methods)
{
    // Швидка перевірка наявності атрибута без створення його екземпляра
    if (method.IsDefined(typeof(ConsoleCommandAttribute), inherit: false))
    {
        var cmdAttr = method.GetCustomAttribute<ConsoleCommandAttribute>()!;
        Console.WriteLine($"  /{cmdAttr.CommandName,-14} -> {cmdAttr.Description} (Метод: {method.Name})");
    }
}
Console.WriteLine();

// -----------------------------------------------------------------------
// Крок 4: Сценарій 3 — Перевірка прав доступу ([RequiresPermission])
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 4: Сценарій 3 — Контроль доступу до методів ([RequiresPermission]) ---");

ExecuteCommandWithSecurity(service, "status", currentUserRole: "Guest");
ExecuteCommandWithSecurity(service, "reset-cache", currentUserRole: "Guest");
ExecuteCommandWithSecurity(service, "reset-cache", currentUserRole: "Admin");
Console.WriteLine();

// -----------------------------------------------------------------------
// Крок 5: Безпечна інспекція сирих метаданих через CustomAttributeData
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 5: Інспекція сирих метаданих (#Blob) без створення об'єктів атрибутів ---");
foreach (CustomAttributeData rawAttr in serviceType.GetCustomAttributesData())
{
    Console.WriteLine($"  Атрибут: {rawAttr.AttributeType.Name}");
    foreach (CustomAttributeTypedArgument ctorArg in rawAttr.ConstructorArguments)
    {
        Console.WriteLine($"    Позиційний аргумент конструктора ({ctorArg.ArgumentType.Name}): {ctorArg.Value}");
    }
    foreach (CustomAttributeNamedArgument namedArg in rawAttr.NamedArguments)
    {
        Console.WriteLine($"    Іменована властивість '{namedArg.MemberName}': {namedArg.TypedValue.Value}");
    }
}

static void ExecuteCommandWithSecurity(PaymentReportService target, string commandToRun, string currentUserRole)
{
    Console.WriteLine($"\n[Запит]: Користувач із роллю '{currentUserRole}' викликає команду '/{commandToRun}'");

    MethodInfo? targetMethod = target.GetType()
        .GetMethods(BindingFlags.Public | BindingFlags.Instance)
        .FirstOrDefault(m => m.GetCustomAttribute<ConsoleCommandAttribute>()?.CommandName == commandToRun);

    if (targetMethod == null)
    {
        Console.WriteLine("  [Помилка]: Команду не знайдено.");
        return;
    }

    // Перевіряємо, чи вимагає цей метод спеціальних прав доступу
    var permissionAttr = targetMethod.GetCustomAttribute<RequiresPermissionAttribute>();
    if (permissionAttr != null && !string.Equals(permissionAttr.RequiredRole, currentUserRole, StringComparison.OrdinalIgnoreCase))
    {
        Console.WriteLine($"  [ВІДМОВА В ДОСТУПІ]: Метод '{targetMethod.Name}' вимагає роль '{permissionAttr.RequiredRole}', а поточна роль — '{currentUserRole}'.");
        return;
    }

    // Якщо перевірку пройдено — викликаємо метод
    targetMethod.Invoke(target, null);
}
