# 03. Множинні реалізації інтерфейсів та Keyed Services (`.NET 8+`)

У реальних корпоративних системах один і той самий контракт часто має кілька реалізацій (наприклад, `INotificationChannel` реалізовано класами `EmailNotificationChannel`, `SmsNotificationChannel` та `TelegramNotificationChannel`). Розглянемо, як контейнер `Microsoft.Extensions.DependencyInjection` дозволяє керувати такими колекціями та точково обирати потрібну стратегію.

---

## 1. Реєстрація кількох реалізацій та `IEnumerable<T>`

Якщо зареєструвати кілька класів під одним інтерфейсом:
```csharp
builder.Services.AddScoped<INotificationChannel, EmailNotificationChannel>();
builder.Services.AddScoped<INotificationChannel, SmsNotificationChannel>();
builder.Services.AddScoped<INotificationChannel, TelegramNotificationChannel>();
```

Контейнер підтримує два сценарії впровадження:
1. **Запит колекції `IEnumerable<INotificationChannel>`**: контейнер створить і передасть **усі три реалізації** у порядку їх реєстрації. Це ідеальна основа для патернів **Composite**, **Chain of Responsibility** або **Pipeline** (наприклад, прогін замовлення через список усіх правил валідації `IEnumerable<IOrderRule>`).
2. **Запит одиничного `INotificationChannel`**: контейнер поверне **останню зареєстровану реалізацію** (`TelegramNotificationChannel`), не викидаючи помилки.

### Запобігання дублікатам: сімейство методів `TryAdd*`
Простір імен `Microsoft.Extensions.DependencyInjection.Extensions` містить захисні методи:
- `TryAddScoped<TService, TImpl>()` — реєструє сервіс **лише тоді**, коли для `TService` ще не зареєстровано жодної реалізації.
- `TryAddEnumerable(ServiceDescriptor.Scoped<TService, TImpl>())` — додає нову реалізацію в колекцію `IEnumerable<TService>`, але захищає від випадкової повторної реєстрації того самого класу `TImpl`.

---

## 2. Іменовані сервіси — Keyed Services (`.NET 8+` / `.NET 10`)

Раніше, коли одному класу був потрібен саме `SmsNotificationChannel`, а іншому — `EmailNotificationChannel`, розробникам доводилося писати власні фабрики або словники стратегій. Починаючи з .NET 8, контейнер має вбудовану підтримку **Keyed Services** (реєстрація за ключем будь-якого типу: `string` або `enum`):

```csharp
builder.Services.AddKeyedScoped<INotificationChannel, EmailNotificationChannel>("email");
builder.Services.AddKeyedScoped<INotificationChannel, SmsNotificationChannel>("sms");
builder.Services.AddKeyedScoped<INotificationChannel, TelegramNotificationChannel>("telegram");
```

Для отримання потрібної реалізації прямо в конструкторі використовується атрибут **`[FromKeyedServices("...")]`**:

```csharp
public sealed class SecurityAlertService
{
    private readonly INotificationChannel _smsChannel;

    public SecurityAlertService(
        [FromKeyedServices("sms")] INotificationChannel smsChannel)
    {
        _smsChannel = smsChannel;
    }
}
```

---

## 3. Фабричні делегати реєстрації (`sp => ...`)

Коли створення об'єкта потребує додаткових параметрів конструктора (наприклад, рядкового префікса) або обгортання одного сервісу в інший (патерн **Decorator**), використовується перевантаження реєстрації з фабричним делегатом:

```csharp
builder.Services.AddKeyedScoped<INotificationChannel>("urgent-fallback", (sp, key) => {
    var innerSms = sp.GetRequiredKeyedService<INotificationChannel>("sms");
    return new UrgentPrefixDecoratorChannel(innerSms, prefix: $"[КЛЮЧ:{key}] ТЕРМІНОВО");
});
```

Повний робочий приклад можна запустити у проєкті [`03-MultipleAndKeyedServices`](../03-MultipleAndKeyedServices/Program.cs).
