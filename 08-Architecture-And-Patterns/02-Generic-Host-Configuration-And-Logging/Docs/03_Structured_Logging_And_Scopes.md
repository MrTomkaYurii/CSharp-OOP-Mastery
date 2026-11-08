# 03. Структуроване Логування, Scopes та Високопродуктивний аудит

## 1. Парадигма Structured Logging проти рядкової конкатенації

Традиційне логування записувало в журнал плоскі тексти: `log.Info("Користувач " + userId + " купив товар " + productId)`. У такому вигляді лог майже неможливо автоматизовано парсити та фільтрувати у великих системах (ELK, Seq, Datadog).

**Структуроване логування (Semantic / Structured Logging)** зберігає повідомлення як шаблон із пойменованими властивостями:
```csharp
_logger.LogInformation("Користувач {UserId} оформив замовлення {OrderId} на суму {TotalAmount:C}", 
    userId, orderId, totalAmount);
```

Компілятор та провайдер логування не склеюють текст через `string.Format`, а передають у підсистему структурований JSON-об'єкт із полями `UserId`, `OrderId`, `TotalAmount`. Це дозволяє виконувати пошукові SQL-подібні запити типу: `OrderId = 42 AND TotalAmount > 1000`.

---

## 2. Діаграма конвеєра логування та контекстних областей (Scopes)

```mermaid
flowchart TD
    classDef call fill:#0284c7,stroke:#38bdf8,color:#ffffff,stroke-width:2px;
    classDef scope fill:#7e22ce,stroke:#c084fc,color:#ffffff,stroke-width:2px;
    classDef prov fill:#0d9488,stroke:#2dd4bf,color:#ffffff,stroke-width:2px;

    Call["Виклик: _logger.LogInformation(...)"]:::call --> Check{"Рівень повідомлення >= LogLevel фільтра?"}
    Check -->|Ні| Drop["Повідомлення ігнорується (нульові витрати)"]
    Check -->|Так| ScopeCheck{"Чи активний BeginScope?"}:::scope

    ScopeCheck -->|Так| Enrich["Збагатити повідомлення метаданими Scope<br/>(TraceId, CorrelationId, CustomerId)"]:::scope
    ScopeCheck -->|Ні| Pipeline["Передача в зареєстровані провайдери"]:::prov

    Enrich --> Pipeline
    Pipeline --> Cons["Console Logger"]:::prov
    Pipeline --> File["File / JSON Logger"]:::prov
    Pipeline --> OTel["OpenTelemetry / Seq / ELK"]:::prov
```

---

## 3. Діагностичні області `BeginScope`: Трасування операцій

Метод `logger.BeginScope` дозволяє "обернути" блок коду логічним контекстом. Усі логи, згенеровані всередині цього блоку (навіть у викликаних підсервісах), автоматично міститимуть параметри цієї області:

```csharp
public async Task ProcessOrderBatchAsync(Guid batchId, List<Order> orders)
{
    // Створюємо контекстну область для всієї пачки:
    using (_logger.BeginScope(new Dictionary<string, object>
    {
        ["BatchId"] = batchId,
        ["TotalOrders"] = orders.Count
    }))
    {
        _logger.LogInformation("Початок обробки пакету замовлень.");

        foreach (var order in orders)
        {
            // Вкладена область конкретного замовлення:
            using (_logger.BeginScope("OrderId: {OrderId}", order.Id))
            {
                _logger.LogInformation("Валідація замовлення...");
                await ProcessSingleOrderAsync(order);
                _logger.LogInformation("Замовлення успішно оброблено.");
            }
        }

        _logger.LogInformation("Обробку пакету завершено.");
    }
}
```

---

## 4. Високопродуктивне логування: `[LoggerMessage]` Source Generators

При мільйонах операцій на секунду стандартний виклик `_logger.LogInformation(...)` створює навантаження на збирач сміття (GC) через пакування типів-значень (`boxing`) та виділення пам'яті під масиви аргументів `params object[]`.

У сучасному C# використовується **Source Generator `[LoggerMessage]`**:

```csharp
public static partial class LogExtensions
{
    // Нульовий allocation: компілятор створює оптимізований код без боксингу аргументів
    [LoggerMessage(
        EventId = 101,
        Level = LogLevel.Information,
        Message = "Користувач {UserId} успішно авторизувався з IP-адреси {IpAddress}")]
    public static partial void LogUserLoggedIn(this ILogger logger, int userId, string ipAddress);
}

// Виклик:
_logger.LogUserLoggedIn(42, "192.168.1.1");
```

---

## 5. Порівняння рівнів логування (`LogLevel`)

| Рівень | Enum | Коли використовувати | Типова реакція інженера |
|:---|:---:|:---|:---|
| **Trace** | 0 | Найдетальніший дамп байтів, сокетів, внутрішніх структур | Лише для локального відлагодження критичних багів |
| **Debug** | 1 | Поточні змінні, внутрішній стан методів | Середовище тестування та Dev |
| **Information** | 2 | Ключові бізнес-події (запуск сервісу, оформлення чека) | Продакшн-моніторинг |
| **Warning** | 3 | Неочікувані ситуації, відновлені автоматично (ретраї мережі) | Аналіз черги технічного боргу |
| **Error** | 4 | Помилки, через які конкретна операція перервалася | Сповіщення в чат чергових інженерів |
| **Critical** | 5 | Аварія застосунку (відсутній доступ до БД, OutOfMemory) | Негайна тривога, автоперезапуск подика |

---

## 6. Матриця проєкту цієї секції

| Проєкт | Посилання на вихідний код | Що демонструє під капотом |
|:---|:---|:---|
| [`03-StructuredLoggingAndScopes`](../03-StructuredLoggingAndScopes/Program.cs) | [Program.cs](../03-StructuredLoggingAndScopes/Program.cs) | Налаштування рівнів фільтрації логів, семантична передача параметрів у шаблони та використання `BeginScope` для трекінгу кореляційних ідентифікаторів. |
