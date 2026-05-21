# Лекція 2: Властивості, Аксесори та Захист Інваріантів

Властивості (`Properties`) у мові C# — це синтаксично елегантні методи-аксесори (`get` та `set`), які зовні маскуються під звичайні змінні.

---

## 1. Різновиди властивостей у C#

### 1. Властивість з явним Backing Field:
```csharp
private string _name = string.Empty;
public string Name
{
    get => _name;
    set
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Ім'я не може бути порожнім.", nameof(value));
        _name = value.Trim();
    }
}
```

### 2. Автоматична властивість (Auto-Property):
Компільована назва backing-поля генерується компілятором автоматично:
```csharp
public int Age { get; set; } = 18; // Зі значенням за замовчуванням
```

### 3. Обчислювана властивість лише для читання (Expression-bodied):
```csharp
public decimal FullPrice => Price * (1 + TaxRate); // Обчислюється під час кожного виклику
```

### 4. Асиметричний доступ:
```csharp
public Guid Id { get; private set; } // Читання відкрите для всіх, запис лише всередині класу
```

---

## 2. Модифікатор `init` у сучасному .NET

Модифікатор `init` (введений у C# 9+) дозволяє встановити значення властивості **лише під час ініціалізації об'єкта**:
```csharp
public class User
{
    public string Email { get; init; } = string.Empty;
}

// Виклик:
var user = new User { Email = "admin@example.com" }; // ✅ Коректно
// user.Email = "hacker@example.com";                 // ❌ Помилка компіляції!
```
Це забезпечує надійну незмінність (`immutability`) без потреби створювати довгі ланцюжки конструкторів.
