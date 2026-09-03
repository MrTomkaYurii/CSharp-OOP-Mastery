# Конфігурація схем за допомогою Fluent API (`IEntityTypeConfiguration`)

У сучасній розробці на EF Core конфігурація через **Fluent API** є рекомендованим стандартом у порівнянні з Data Annotations, оскільки зберігає чисті POCO-моделі безпосередньо в доменному шарі.

---

## 1. Патерн `IEntityTypeConfiguration<T>`

Замість перевантаження одного методу `OnModelCreating` сотнями рядків коду, кожна сутність має окремий конфігураційний клас:

```csharp
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using CodeFirst.Models.Models;

public class MoviesConfiguration : IEntityTypeConfiguration<Movies>
{
    public void Configure(EntityTypeBuilder<Movies> builder)
    {
        // Назва таблиці
        builder.ToTable("Movies");

        // Первинний ключ
        builder.HasKey(m => m.MovieId);

        // Обмеження колонок
        builder.Property(m => m.Title)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(m => m.Director)
            .HasMaxLength(100);

        // Налаштування зв'язку 1-to-Many з Copies
        builder.HasMany(m => m.Copies)
            .WithOne(c => c.Movie)
            .HasForeignKey(c => c.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
```

---

## 2. Налаштування складеного первинного ключа

Приклад конфігурації проміжної сутності `Starring`:

```csharp
public class StarringConfiguration : IEntityTypeConfiguration<Starring>
{
    public void Configure(EntityTypeBuilder<Starring> builder)
    {
        builder.ToTable("Starring");

        // Складений ключ з двох полів
        builder.HasKey(s => new { s.ActorId, s.MovieId });

        // Зв'язок з Actor
        builder.HasOne(s => s.Actor)
            .WithMany(a => a.Starrings)
            .HasForeignKey(s => s.ActorId);

        // Зв'язок з Movie
        builder.HasOne(s => s.Movie)
            .WithMany(m => m.Starrings)
            .HasForeignKey(s => s.MovieId);
    }
}
```

---

## 3. Автоматична реєстрація всіх конфігурацій

У класі `DbContext` достатньо викликати метод `ApplyConfigurationsFromAssembly`:

```csharp
protected override void OnModelCreating(ModelBuilder modelBuilder)
{
    base.OnModelCreating(modelBuilder);
    
    // Автоматично знаходить і застосовує всі класи IEntityTypeConfiguration у збірці
    modelBuilder.ApplyConfigurationsFromAssembly(typeof(MoviesConfiguration).Assembly);
}
```
