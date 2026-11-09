# Зчитування та обробка метаданих атрибутів у Runtime (Attribute Inspection)

Після декорування коду атрибутами їх можна аналізувати під час виконання програми за допомогою методів рефлексії: `System.Type`, `System.Reflection.MemberInfo` та `System.Attribute`.

---

## 1. Методи пошуку та отримання атрибутів

### 1.1. `Attribute.IsDefined` / `MemberInfo.IsDefined`
Швидка перевірка наявності атрибута **без створення його екземпляра в пам'яті (Heap)**:
```csharp
MemberInfo member = typeof(Employee).GetMethod("PrintDetails")!;
bool isAudited = Attribute.IsDefined(member, typeof(MethodInfoAttribute));
```

### 1.2. `MemberInfo.GetCustomAttribute<T>()`
Отримання одиничного екземпляра атрибута (найчастіше використовуваний у сучасному .NET):
```csharp
var methodAttr = member.GetCustomAttribute<MethodInfoAttribute>();
if (methodAttr != null)
{
    Console.WriteLine($"Метод: {member.Name}, Опис: {methodAttr.Description}");
}
```

### 1.3. `MemberInfo.GetCustomAttributes<T>()`
Отримання колекції атрибутів, якщо атрибут підтримує `AllowMultiple = true`:
```csharp
IEnumerable<RoleAttribute> roles = typeof(AdminController).GetCustomAttributes<RoleAttribute>();
foreach (var role in roles)
{
    Console.WriteLine($"Дозволена роль: {role.RoleName}");
}
```

---

## 2. Практичний патерн: Маркування методів для динамічного виклику

Атрибути дозволяють створити механізм декларативного запуску тестових методів або обробників подій:

```mermaid
sequenceDiagram
    autonumber
    participant Runner as Test / Task Runner
    participant Type as Type Definition
    participant Method as MethodInfo
    participant Attr as RunnableAttribute
    
    Runner->>Type: GetMethods()
    loop Для кожного методу
        Type-->>Runner: MethodInfo
        Runner->>Method: GetCustomAttribute<RunnableAttribute>()
        alt Атрибут присутній
            Method-->>Runner: RunnableAttribute(Priority = 1)
            Runner->>Method: Invoke(instance, null)
        else Атрибут відсутній
            Method-->>Runner: null (пропуск)
        end
    end
```

### Приклад коду:
```csharp
public class TestRunner
{
    public static void RunTests(object testClassInstance)
    {
        Type type = testClassInstance.GetType();
        foreach (MethodInfo method in type.GetMethods())
        {
            var attr = method.GetCustomAttribute<TestMethodAttribute>();
            if (attr != null)
            {
                Console.WriteLine($"[RUN] Виконується тест: {method.Name} (Опис: {attr.Description})");
                method.Invoke(testClassInstance, null);
            }
        }
    }
}
```

---

## 3. Оптимізація продуктивності: Кешування метаданих

> [!WARNING]
> Виклик `GetCustomAttributes()` у гарячих циклах (наприклад, при обробці кожного запиту в ORM чи серіалізаторі) є **антипатерном**, оскільки кожен виклик повторно десеріалізує потік `#Blob` та виділяє новий об'єкт у керованій купі (Heap Allocation).

Для досягнення максимальної швидкодії фреймворки (Entity Framework Core, Dapper, ASP.NET Core) використовують потокобезпечне кешування через `ConcurrentDictionary`:

```csharp
public static class AttributeCache<TAttribute> where TAttribute : Attribute
{
    private static readonly ConcurrentDictionary<MemberInfo, TAttribute?> _cache = new();

    public static TAttribute? Get(MemberInfo member)
    {
        return _cache.GetOrAdd(member, m => m.GetCustomAttribute<TAttribute>());
    }
}
```

### Порівняльна таблиця стратегій доступу до метаданих:

| Підхід | Накладні витрати на CPU | Виділення пам'яті (GC) | Рекомендоване застосування |
|:---|:---:|:---:|:---|
| **`MemberInfo.IsDefined`** | Мінімальні | **0 байт** | Перевірка прапорців (наприклад, `[IgnoreField]`) |
| **`GetCustomAttribute<T>()`** | Середні | Виділення нового об'єкта | Одноразова ініціалізація або рідкісні виклики |
| **Кешування у `ConcurrentDictionary`** | Нульові (після прогріву) | 0 байт (після прогріву) | Високонавантажені сервери, ORM, парсери JSON |
| **Source Generators (C# 9+)** | **0% (Compile-Time)** | **0 байт** | Сучасний підхід AOT/.NET без використання рефлексії |
