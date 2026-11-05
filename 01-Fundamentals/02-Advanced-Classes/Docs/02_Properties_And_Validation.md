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
var user = new User { Email = "admin@example.com" }; //  Коректно
// user.Email = "hacker@example.com";                 //  Помилка компіляції!
```
Це забезпечує надійну незмінність (`immutability`) без потреби створювати довгі ланцюжки конструкторів.



---



## Матриця проєктів цієї секції (`02. Властивості та Валідація`)



| Проєкт у Solution Explorer | Файл коду | Що реалізовано в прикладі |
|:---|:---|:---|
| **SOLIS-p145** | [Program.cs](../SOLIS-p145/Program.cs) | Властивість із явним приватним backing-полем і перевіркою діапазону в `set` |
| **SOLIS-p149** | [Program.cs](../SOLIS-p149/Program.cs) | Обчислювана властивість лише для читання (гіпотенуза трикутника з катетів) |
| **SOLIS-p150** | [Program.cs](../SOLIS-p150/Program.cs) | Автоматично реалізовані властивості (`Auto-Implemented Properties { get; set; }`) |
| **SOLIS-p151** | [Program.cs](../SOLIS-p151/Program.cs) | Статичні властивості (`static property`) та підключення через `using static` |
| **SOLIS-p170** | [Program.cs](../SOLIS-p170/Program.cs) | Асиметричні модифікатори доступу для аксесорів (`public get; private set;`) |
| **Ex-Property-Validation** | [Program.cs](../Ex-Property-Validation/Program.cs) | Комплексна валідація вхідних даних та захист інваріантів моделі через властивості | 