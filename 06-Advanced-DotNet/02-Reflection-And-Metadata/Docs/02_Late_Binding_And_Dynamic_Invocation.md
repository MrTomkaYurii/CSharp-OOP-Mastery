# Пізнє зв'язування та динамічний виклик (Late Binding & Dynamic Invocation)

**Пізнє зв'язування (Late Binding)** — це створення екземплярів типів та виклик їхніх методів/властивостей у runtime без прямого посилання на тип під час компіляції.

---

## 1. Створення екземплярів: `Activator.CreateInstance`

Клас `System.Activator` надає методи для створення екземплярів типів на основі їхнього `Type` або рядкового імені:

```csharp
Type? carType = Type.GetType("Shared.Common.SportsCar, Shared");
if (carType != null)
{
    // Створення екземпляра конструктором за замовчуванням
    object? carInstance = Activator.CreateInstance(carType);

    // Створення екземпляра з передачею аргументів у конструктор
    object? parameterizedCar = Activator.CreateInstance(carType, "Ferrari", 320);
}
```

---

## 2. Динамічний виклик методів (`MethodInfo.Invoke`)

Після отримання екземпляра можна знайти метод та викликати його, передавши аргументи як масив `object[]`:

```csharp
MethodInfo? accelerateMethod = carType?.GetMethod("Accelerate");
if (accelerateMethod != null && carInstance != null)
{
    // Виклик методу: Accelerate(50)
    accelerateMethod.Invoke(carInstance, new object[] { 50 });
}
```

### Виклик методів з поверненням результату:
```csharp
MethodInfo? getSpeedMethod = carType?.GetMethod("GetCurrentSpeed");
if (getSpeedMethod != null && carInstance != null)
{
    object? result = getSpeedMethod.Invoke(carInstance, null);
    int speed = (int)(result ?? 0);
    Console.WriteLine($"Поточна швидкість: {speed}");
}
```

---

## 3. Робота з динамічними збірками (`Assembly.Load`)

Рефлексія дозволяє завантажувати збірки (.dll) з диску та виконувати їхній код ізольовано:

```csharp
Assembly assembly = Assembly.LoadFrom("Plugins/CustomLogger.dll");
Type? pluginType = assembly.GetType("CustomLogger.FileLogger");

if (pluginType != null)
{
    object? logger = Activator.CreateInstance(pluginType);
    MethodInfo? logMethod = pluginType.GetMethod("Log");
    logMethod?.Invoke(logger, new object[] { "Повідомлення із плагіна" });
}
```

---

## 4. Продуктивність та оптимізації

| Механізм | Відносна швидкість | Зауваження |
| :--- | :--- | :--- |
| Прямий виклик | $1\times$ (еталон) | Повна перевірка компілятором |
| `dynamic` (DLR) | $\approx 2-5\times$ повільніше | Кешує CallSite під капотом |
| `Compiled Expression Trees` | $\approx 1.1-1.5\times$ повільніше | Майже як прямий виклик після прогріву |
| `MethodInfo.Invoke` | $\approx 20-50\times$ повільніше | Виконує перевірки аргументів та boxing при кожному виклику |
