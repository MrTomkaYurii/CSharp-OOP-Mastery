using System.Text;
using DI.CoreConcepts.Services;
using Microsoft.Extensions.DependencyInjection;

Console.OutputEncoding = Encoding.UTF8;
Console.WriteLine("=================================================================");
Console.WriteLine("  .NET 10 DEPENDENCY INJECTION: ПОГЛИБЛЕНИЙ ПРАКТИКУМ");
Console.WriteLine("=================================================================\n");

// -------------------------------------------------------------
// 1. ДЕМОНСТРАЦІЯ ЖИТТЄВИХ ЦИКЛІВ (LIFETIMES)
// -------------------------------------------------------------
Console.WriteLine("--- 1. Життєві цикли сервісів (Transient, Scoped, Singleton) ---");
var services = new ServiceCollection();

services.AddTransient<ITransientOperation, TransientOperation>();
services.AddScoped<IScopedOperation, ScopedOperation>();
services.AddSingleton<ISingletonOperation, SingletonOperation>();
services.AddTransient<OperationReporter>();

var rootProvider = services.BuildServiceProvider();

Console.WriteLine("\n[Scope #1 - Початок запиту]:");
using (var scope1 = rootProvider.CreateScope())
{
    var reporter1 = scope1.ServiceProvider.GetRequiredService<OperationReporter>();
    reporter1.Report("Виклик 1 всередині Scope 1");

    var reporter2 = scope1.ServiceProvider.GetRequiredService<OperationReporter>();
    reporter2.Report("Виклик 2 всередині Scope 1 (Scoped ID не змінюється, Transient змінюється)");
}

Console.WriteLine("\n[Scope #2 - Новий незалежний запит]:");
using (var scope2 = rootProvider.CreateScope())
{
    var reporter3 = scope2.ServiceProvider.GetRequiredService<OperationReporter>();
    reporter3.Report("Виклик 1 всередині Scope 2 (новий Scoped ID, той самий Singleton)");
}

// -------------------------------------------------------------
// 2. ДЕМОНСТРАЦІЯ KEYED SERVICES (.NET 8/9/10)
// -------------------------------------------------------------
Console.WriteLine("\n-------------------------------------------------------------");
Console.WriteLine("--- 2. Іменовані / Ключові сервіси (Keyed Services) ---");
var keyedServices = new ServiceCollection();

// Реєструємо сервіси за різними ключами
keyedServices.AddKeyedSingleton<INotificationSender, EmailNotificationSender>("email");
keyedServices.AddKeyedSingleton<INotificationSender, SmsNotificationSender>("sms");
keyedServices.AddKeyedSingleton<INotificationSender, TelegramNotificationSender>("telegram");
keyedServices.AddTransient<AlertDispatchService>();

var keyedProvider = keyedServices.BuildServiceProvider();

// Пряме отримання за ключем
var smsSender = keyedProvider.GetRequiredKeyedService<INotificationSender>("sms");
Console.WriteLine($"\nПряме отримання з провайдера за ключем 'sms':");
smsSender.Send("+380991234567", "Код підтвердження: 482910");

// Отримання через клас з атрибутами [FromKeyedServices]
var dispatcher = keyedProvider.GetRequiredService<AlertDispatchService>();
Console.WriteLine("\nОтримання через клас AlertDispatchService (ін'єкція через атрибути):");
dispatcher.DispatchUrgentAlert("alex_dev", "Критична помилка на сервері БД!");
dispatcher.DispatchStandardDigest("team@company.ua", "Щотижневий звіт продуктивності готовий.");

// -------------------------------------------------------------
// 3. CAPTIVE DEPENDENCY ТА ВАЛІДАЦІЯ КОНТЕЙНЕРА
// -------------------------------------------------------------
Console.WriteLine("\n-------------------------------------------------------------");
Console.WriteLine("--- 3. Проблема Captive Dependency та ValidateScopes ---");

var captiveServices = new ServiceCollection();
captiveServices.AddScoped<IUserSessionScope, UserSessionScope>();
captiveServices.AddSingleton<BadCaptiveSingleton>();
captiveServices.AddSingleton<SafeScopeAwareSingleton>();

Console.WriteLine("Спроба збудувати ServiceProvider із увімкненим ValidateScopes = true:");
try
{
    var options = new ServiceProviderOptions
    {
        ValidateScopes = true,
        ValidateOnBuild = true
    };

    var strictlyValidatedProvider = captiveServices.BuildServiceProvider(options);
    Console.WriteLine("Помилка не виникла (неочікувано).");
}
catch (AggregateException ex)
{
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine($"[ПЕРЕХОПЛЕНО ПРАВИЛОМ ВАЛІДАЦІЇ]: {ex.InnerExceptions[0].Message}");
    Console.ResetColor();
}

Console.WriteLine("\nБезпечний патерн: SafeScopeAwareSingleton використовує IServiceScopeFactory:");
var safeServices = new ServiceCollection();
safeServices.AddScoped<IUserSessionScope, UserSessionScope>();
safeServices.AddSingleton<SafeScopeAwareSingleton>();
var safeProvider = safeServices.BuildServiceProvider(new ServiceProviderOptions { ValidateScopes = true });

var safeSingleton = safeProvider.GetRequiredService<SafeScopeAwareSingleton>();
safeSingleton.ExecuteInNewScope(session =>
{
    session.CurrentUser = "Оксана (Scope A)";
    Console.WriteLine($"  Виконання в Scope A: Session ID = {session.SessionId:N}, User = {session.CurrentUser}");
});

safeSingleton.ExecuteInNewScope(session =>
{
    session.CurrentUser = "Богдан (Scope B)";
    Console.WriteLine($"  Виконання в Scope B: Session ID = {session.SessionId:N}, User = {session.CurrentUser}");
});

// -------------------------------------------------------------
// 4. ОЧИЩЕННЯ РЕСУРСІВ (IDISPOSABLE / IASYNCDISPOSABLE)
// -------------------------------------------------------------
Console.WriteLine("\n-------------------------------------------------------------");
Console.WriteLine("--- 4. Автоматичне очищення ресурсів контейнером (IDisposable) ---");

var disposalServices = new ServiceCollection();
disposalServices.AddTransient<ManagedResource>(_ => new ManagedResource("Transient Resource"));
disposalServices.AddScoped<ManagedResource>(_ => new ManagedResource("Scoped Resource"));

var disposalProvider = disposalServices.BuildServiceProvider();

Console.WriteLine("Створюємо scope та отримуємо сервіси:");
using (var scope = disposalProvider.CreateScope())
{
    var r1 = scope.ServiceProvider.GetRequiredService<ManagedResource>();
    var r2 = scope.ServiceProvider.GetRequiredService<ManagedResource>();
    Console.WriteLine("Виходимо з блоку using (scope)...");
}
Console.WriteLine("Блок using (scope) завершено: контейнер автоматично викликав Dispose().\n");

Console.WriteLine("=================================================================");
Console.WriteLine("  DI.CoreConcepts успішно завершив виконання!");
Console.WriteLine("=================================================================");
