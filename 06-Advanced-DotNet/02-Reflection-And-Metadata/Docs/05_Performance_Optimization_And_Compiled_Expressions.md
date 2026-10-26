# 05. Продуктивність рефлексії та скомпільовані вирази (Performance & Expression Trees)

Рефлексія — надзвичайно гнучкий інструмент, але за цю гнучкість доводиться платити продуктивністю. Розуміння того, **чому** вона повільніша і **як** її прискорити, відрізняє розробника рівня Senior.

---

## 🛑 Чому класична рефлексія (`MethodInfo.Invoke`) повільна?

При кожному виклику `method.Invoke(instance, args)` середовище CLR змушене виконувати серію важких операцій:
1. **Перевірка безпеки (Security Checks)** — чи має викликаючий код доступ до закритого або внутрішнього методу.
2. **Валідація аргументів** — перевірка кількості аргументів та їхньої сумісності з типами параметрів.
3. **Упаковка та розпаковка (Boxing/Unboxing)** — оскільки параметри передаються як `object[]`, будь-який тип значення (`int`, `double`, `struct`) пакується в купу (Heap allocation), створюючи навантаження на Garbage Collector.
4. **Непрямий перехід (Indirect Call)** — CLR не може заінлайнити (inline) метод під час JIT-компіляції.

---

## 📊 Порівняльний бенчмарк (Результати вимірювання 1,000,000 викликів)

| Метод виклику | Механізм | Час виконання | Відносне уповільнення | Примітка | Посилання на приклад |
| :--- | :--- | :---: | :---: | :--- | :---: |
| **Direct Call** | Прямий компіляційний виклик | **18 ms** | **1.0x** (Еталон) | Повна типізація, можливий інлайнінг JIT | [![Код](https://img.shields.io/badge/Код-15_DirectCall-green?style=flat-square)](../15-ReflectionPerformanceDemo/Program.cs#L35-L43) |
| **Delegate.CreateDelegate** | Типізований вказівник на функцію | **2 ms** | **~0.1x** | Швидше або на рівні прямого виклику завдяки інлайнінгу | [![Код](https://img.shields.io/badge/Код-15_DelegateCall-green?style=flat-square)](../15-ReflectionPerformanceDemo/Program.cs#L45-L53) |
| **Compiled Expression Tree** | Скомпільоване дерево виразів | **1 ms** | **~0.1x** | Генерує чистий IL-байткод у рантаймі | [![Код](https://img.shields.io/badge/Код-15_ExpressionTree-green?style=flat-square)](../15-ReflectionPerformanceDemo/Program.cs#L55-L63) |
| **dynamic (DLR)** | Кешований CallSite DLR | **3 ms** | **~0.2x** | Кешує згенерований IL після першого «прогріву» | [![Код](https://img.shields.io/badge/Код-15_DynamicCall-green?style=flat-square)](../15-ReflectionPerformanceDemo/Program.cs#L65-L73) |
| **MethodInfo.Invoke** | Класична рефлексія | **27 ms** | **~1.5x** | Валідація масиву об'єктів та boxing при кожному виклику | [![Код](https://img.shields.io/badge/Код-15_MethodInvoke-green?style=flat-square)](../15-ReflectionPerformanceDemo/Program.cs#L75-L84) |

> [!NOTE]
> Заміри проведено на .NET 10. У складніших сценаріях з багатьма аргументами типів значень розрив між `MethodInfo.Invoke` та компільованими виразами сягає **20x - 50x**!

---

## ⚡ Як сучасні .NET-бібліотеки оптимізують рефлексію

1. **Кешування метаданих**:
   Ніколи не викликайте `type.GetProperties()` або `type.GetMethod()` у циклах. Знайдіть їх один раз під час старту програми і збережіть у `ConcurrentDictionary<Type, ...>`.

2. **Компіляція дерев виразів (`Expression Trees`)**:
   Замість того, щоб кожного разу викликати метод через рефлексію, бібліотеки на зразок **AutoMapper** або **Entity Framework** генерують дерево виразів і компілюють його у справжній швидкий делегат:

   ```csharp
   // Генерація делегата Action<Car, int> під час старту
   var instanceParam = Expression.Parameter(typeof(Car), "target");
   var argParam = Expression.Parameter(typeof(int), "delta");
   var callExpr = Expression.Call(instanceParam, accelerateMethod, argParam);

   // Компіляція дерева у машинний код
   Action<Car, int> fastCompiledDelegate = Expression.Lambda<Action<Car, int>>(
       callExpr, instanceParam, argParam).Compile();

   // Далі викликаємо з швидкістю прямого методу:
   fastCompiledDelegate(car, 10);
   ```

3. **C# Source Generators (.NET 6+)**:
   Сучасний золотий стандарт — заміна рантайм-рефлексії генерацією коду під час компіляції (AOT-friendly, нульовий оверхед у runtime).
