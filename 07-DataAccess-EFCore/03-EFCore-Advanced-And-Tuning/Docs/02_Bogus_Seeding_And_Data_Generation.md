# Розділ 2: Генерація Тестових Даних та Сидинг за допомогою Bogus

## 1. Навіщо потрібен професійний сидинг?
Ручне створення тестових сутностей у базі даних ("User1", "User2", "Test test") призводить до:
- Неможливості виявити проблеми з реальними форматами даних (довгі імена, специфічні символи, локалізація).
- Відсутності навантажувальних даних для перевірки індексів та швидкодії запитів.
- Труднощів при моделюванні складних зв'язків (один-до-багатьох, багато-до-багатьох).

Бібліотека **Bogus** є золотим стандартом у світі .NET для автоматизованої генерації правдоподібних, багатих та локалізованих тестових датасетів.

---

## 2. Налаштування Bogus Faker

```csharp
// Налаштування локалізації ("uk" для української мови)
var authorFaker = new Faker<Author>("uk")
    .RuleFor(a => a.FirstName, f => f.Name.FirstName())
    .RuleFor(a => a.LastName, f => f.Name.LastName())
    .RuleFor(a => a.Email, (f, a) => f.Internet.Email(a.FirstName, a.LastName))
    .RuleFor(a => a.Biography, f => f.Lorem.Paragraph());
```

### Відтворюваність генерації (Seed)
Для автоматизованих тестів критично мати детермінований набір даних:
```csharp
// Однаковий Seed гарантує генерацію абсолютно однакових даних на кожному запуску
Randomizer.Seed = new Random(42);
```

---

## 3. Генерація зв'язаних графів об'єктів

Bogus дозволяє вкладати генератори один в один для створення реляційних деревоподібних структур:

```csharp
var reviewFaker = new Faker<Review>("uk")
    .RuleFor(r => r.ReviewerName, f => f.Name.FullName())
    .RuleFor(r => r.Rating, f => f.Random.Int(1, 5))
    .RuleFor(r => r.Comment, f => f.Rant.Review("книга"));

var bookFaker = new Faker<Book>("uk")
    .RuleFor(b => b.Title, f => f.Commerce.ProductName())
    .RuleFor(b => b.Price, f => Math.Round(f.Random.Decimal(100, 1000), 2))
    .RuleFor(b => b.Reviews, f => reviewFaker.Generate(f.Random.Int(1, 5))); // 1-5 відгуків на книгу

var authorFaker = new Faker<Author>("uk")
    // ...
    .RuleFor(a => a.Books, f => bookFaker.Generate(f.Random.Int(2, 4))); // 2-4 книги на автора
```

---

## 4. Патерн ідемпотентного сидингу (Idempotent Seeder)

Сидер повинен безпечно викликатися як при першому старті додатку, так і при наступних перезапусках без дублювання записів:

```csharp
public static async Task SeedAsync(BookstoreDbContext context)
{
    // Ідемпотентність: якщо в таблиці вже є записи — виходимо
    if (await context.Authors.AnyAsync())
    {
        return;
    }

    var dataset = authorFaker.Generate(100);
    await context.Authors.AddRangeAsync(dataset);
    await context.SaveChangesAsync();
}
```
У фонових службах або при старті Web Host сідинг викликається через `IHostApplicationLifetime` або окрему команду міграцій.
