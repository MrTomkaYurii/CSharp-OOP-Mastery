# 02. Моделювання сутностей: Data Annotations проти Fluent API

## 1. Три рівні конфігурації моделі у EF Core

Entity Framework Core будує метамодель бази даних, комбінуючи три підходи (у порядку зростання пріоритету):
1. **За угодою (Conventions)** — автоматичні правила: клас стає таблицею, властивість `Id` або `<ClassName>Id` стає первинним ключем (`PRIMARY KEY`), reference-типи без `?` (за увімкненого `<Nullable>enable</Nullable>`) стають `NOT NULL`.
2. **Атрибути (Data Annotations)** — декларативні атрибути з просторів імен `System.ComponentModel.DataAnnotations` та `...Schema` (`[Table]`, `[Column]`, `[Key]`, `[Required]`, `[MaxLength]`, `[NotMapped]`), які розміщуються безпосередньо над класами та властивостями.
3. **Fluent API (`OnModelCreating`)** — програмна конфігурація через `ModelBuilder`. Має **найвищий пріоритет** і перекриває як угоди, так і атрибути. Дозволяє налаштовувати можливості, недоступні в атрибутах (Check-обмеження, значення за замовчуванням, складені ключі та фільтри).

---

## 2. Чиста архітектура: `IEntityTypeConfiguration<T>`

Щоб метод `OnModelCreating` у `DbContext` не розростався на сотні рядків, конфігурацію кожної сутності виносять в окремий клас, що реалізує інтерфейс **`IEntityTypeConfiguration<TEntity>`**:

```csharp
public class ProductConfiguration : IEntityTypeConfiguration<Product> {
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("CatalogProducts");
        builder.HasKey(p => p.Id);
        // ...
    }
}
```
А у `OnModelCreating` достатньо одного рядка для автоматичного сканування та застосування всіх конфігурацій зі збірки:
```csharp
modelBuilder.ApplyConfigurationsFromAssembly(typeof(ModelingDbContext).Assembly);
```

---

## 3. Порівняльна таблиця Data Annotations та Fluent API

| Завдання моделювання | Data Annotations | Fluent API (`EntityTypeBuilder<T>`) |
| :--- | :--- | :--- |
| Зміна імені таблиці | `[Table("CatalogProducts")]` | `builder.ToTable("CatalogProducts")` |
| Зміна імені та типу стовпця | `[Column("ProductId", TypeName = "decimal(18,2)")]` | `builder.Property(p => p.UnitPrice).HasColumnName("...").HasColumnType("decimal(18,2)")` |
| Первинний ключ | `[Key]` | `builder.HasKey(p => p.Id)` |
| Складений первинний ключ | `[PrimaryKey(nameof(WarehouseCode), nameof(ProductId))]` | `builder.HasKey(w => new { w.WarehouseCode, w.ProductId })` |
| Альтернативний ключ (`UNIQUE NOT NULL`) | *Не підтримується* | `builder.HasAlternateKey(p => p.Sku)` |
| Обмеження `CHECK` у таблиці | *Не підтримується* | `builder.ToTable(t => t.HasCheckConstraint("CK_Price", "UnitPrice >= 0"))` |
| Значення за замовчуванням (`DEFAULT`) | *Не підтримується* | `builder.Property(p => p.StockQuantity).HasDefaultValue(0)` |
| Виключення властивості з таблиці БД | `[NotMapped]` | `builder.Ignore(w => w.TemporaryInspectorNote)` |

---

## 4. Початкове заповнення даних (`HasData`)

Метод `builder.HasData(...)` дозволяє зафіксувати довідкові або стартові записи прямо в метамоделі EF Core (вони автоматично потрапляють у DDL/міграції та створюються під час `EnsureCreatedAsync()`).
- При використанні `HasData` необхідно **явно вказувати значення первинного ключа (`Id = 1`, `Id = 2`)**, навіть якщо у БД ключ є автоінкрементним.
