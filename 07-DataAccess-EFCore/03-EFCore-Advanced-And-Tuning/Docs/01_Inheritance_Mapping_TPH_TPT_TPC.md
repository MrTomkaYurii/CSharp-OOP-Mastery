# Розділ 1: Поглиблене Мапування в EF Core (Стратегії Успадкування, Owned Types, JSON Columns)

## 1. Стратегії мапування ієрархій успадкування (Inheritance Mapping)

В об'єктно-орієнтованому програмуванні спадкування є базовим інструментом моделювання. Реляційні бази даних не мають нативної підтримки ООП-спадкування. EF Core пропонує три фундаментальні стратегії проектування спадкування на реляційні таблиці:

### А. Table-per-Hierarchy (TPH) — Стратегія за замовчуванням
- **Принцип:** Вся ієрархія класів зберігається в **одній спільній таблиці**.
- **Дискримінатор (Discriminator):** Спеціальний стовпець (зазвичай рядковий або числовий), що вказує тип конкретного екземпляра.
- **Переваги:** Максимальна швидкість виконання поліморфних запитів (жодних SQL `JOIN` або `UNION`).
- **Недоліки:** Стовпці, специфічні для нащадків, повинні бути `NULLABLE` на рівні БД.

```csharp
modelBuilder.Entity<Payment>()
    .ToTable("Payments")
    .HasDiscriminator<string>("PaymentType")
    .HasValue<CreditCardPayment>("CreditCard")
    .HasValue<CryptoPayment>("Crypto")
    .HasValue<BankTransferPayment>("BankTransfer");
```

### Б. Table-per-Type (TPT)
- **Принцип:** Кожен клас ієрархії мапується на **окрему таблицю**. Базова таблиця містить спільні поля, таблиці нащадків містять лише специфічні поля та Foreign Key на первинний ключ базової таблиці.
- **Переваги:** Нормалізована схема БД, поля нащадків можуть бути `NOT NULL`.
- **Недоліки:** Поліморфні вибірки потребують множинних операцій `LEFT JOIN`, що суттєво уповільнює запити при великій кількості спадкоємців.

### В. Table-per-Concrete-Type (TPC) (.NET 7+)
- **Принцип:** Кожен конкретний клас мапується на повністю окрему таблицю, що дублює спільні стовпці. Базовий клас не має власної таблиці.
- **Переваги:** Жодних `JOIN` для вибірки конкретного типу; первинні ключі генеруються через Hi/Lo або Sequence.
- **Недоліки:** Поліморфні запити до базового типу виконуються через `UNION ALL`.

---

## 2. Вбудовані типи (Owned Entity Types / Value Objects)

У Domain-Driven Design (DDD) об'єкти-значення (**Value Objects**) не мають власного глобального ідентифікатора (`Id`), а визначаються сукупністю своїх значень.

В EF Core для цього використовуються **Owned Entity Types**:
```csharp
modelBuilder.Entity<Customer>(entity =>
{
    entity.OwnsOne(c => c.ShippingAddress, addr =>
    {
        addr.Property(a => a.Street).HasColumnName("Shipping_Street");
        addr.Property(a => a.City).HasColumnName("Shipping_City");
        addr.Property(a => a.PostalCode).HasColumnName("Shipping_PostalCode");
    });
});
```
За замовчуванням поля вбудованого об'єкта зберігаються як стовпці тієї самої таблиці (`Customers`).

---

## 3. Зберігання та фільтрація в JSON-стовпцях (.NET 8+)

EF Core 8+ дозволяє мапувати складні вкладені структури, масиви та словники в нативні JSON-стовпці БД за допомогою виклику `.ToJson()`:

```csharp
modelBuilder.Entity<CatalogProduct>(entity =>
{
    entity.OwnsOne(p => p.Specifications, spec =>
    {
        spec.ToJson();
    });
});
```

### Запити всередину JSON через LINQ:
EF Core автоматично транслює LINQ-вирази у нативні JSON-функції реляційної СУБД (наприклад, `json_extract` в SQLite або `JSON_VALUE` / `->>` в SQL Server/PostgreSQL):

```csharp
var ultraLightLaptops = await db.Products
    .Where(p => p.Specifications.WeightKg < 1.5)
    .ToListAsync();
```
Це поєднує гнучкість документоорієнтованих NoSQL сховищ зі строгістю та цілісністю реляційних баз даних.
