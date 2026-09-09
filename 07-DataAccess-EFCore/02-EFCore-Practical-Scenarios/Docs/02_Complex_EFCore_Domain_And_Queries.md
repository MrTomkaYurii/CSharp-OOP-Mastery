# Комплексна доменна модель та практичні EF Core запити (Advanced Queries)

Доменна модель індустрії розваг охоплює фільми, серіали, продюсерські компанії, акторів та персонажів, демонструючи наслідування таблиць та складні багатотабличні зв'язки.

---

## 1. Схема сутностей (ERD)

```mermaid
erDiagram
    PRODUCTION ||--o{ MOVIE : "produces"
    PRODUCTION ||--o{ SERIES : "produces"
    MOVIE ||--o{ RATING : "has"
    SERIES ||--o{ RATING : "has"
    ACTOR ||--o{ CHARACTER : "plays"
    MOVIE ||--o{ CHARACTER : "features"
    SERIES ||--o{ CHARACTER : "features"

    PRODUCTION {
        int Id PK
        string Name
        string Country
    }
    ACTOR {
        int Id PK
        string FullName
        DateTime BirthDate
    }
    CHARACTER {
        int Id PK
        string Name
        int ActorId FK
        int? MovieId FK
        int? SeriesId FK
    }
    MOVIE {
        int Id PK
        string Title
        int ReleaseYear
        int ProductionId FK
    }
    SERIES {
        int Id PK
        string Title
        int SeasonsCount
        int ProductionId FK
    }
    RATING {
        int Id PK
        double Score
        string Source
    }
```

---

## 2. Комплексні LINQ-запити в `Queries.cs`

### 2.1. Жадібне завантаження (Eager Loading) з проекцією:
```csharp
public async Task<List<MovieDto>> GetMoviesWithFullCastAsync()
{
    return await _context.Movies
        .AsNoTracking()
        .Include(m => m.Production)
        .Include(m => m.Characters)
            .ThenInclude(c => c.Actor)
        .Include(m => m.Ratings)
        .Select(m => new MovieDto
        {
            Title = m.Title,
            Production = m.Production.Name,
            Actors = m.Characters.Select(c => $"{c.Actor.FullName} as {c.Name}").ToList(),
            AverageRating = m.Ratings.Average(r => r.Score)
        })
        .ToListAsync();
}
```

### 2.2. Фільтрація за складними агрегатними умовами:
```csharp
// Пошук акторів, які зіграли в більше ніж 3 фільмах із середнім рейтингом > 8.0
public async Task<List<TopActorDto>> GetTopActorsAsync()
{
    return await _context.Actors
        .AsNoTracking()
        .Where(a => a.Characters.Count(c => c.Movie != null && c.Movie.Ratings.Average(r => r.Score) >= 8.0) >= 3)
        .Select(a => new TopActorDto
        {
            Name = a.FullName,
            HitCount = a.Characters.Count
        })
        .ToListAsync();
}
```

---

## 3. Оптимізація продуктивності: `AsNoTracking`

Для операцій тільки для читання (Read-Only queries) завжди вмикається `.AsNoTracking()`. Це вимикає механізм відстеження змін у `ChangeTracker`, заощаджує до 50% оперативної пам'яті та значно прискорює матеріалізацію результатів.
