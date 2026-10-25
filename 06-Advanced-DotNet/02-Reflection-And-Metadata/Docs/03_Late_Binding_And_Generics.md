# 03. Пізнє зв'язування, динамічна активація та дженерики (Late Binding & Generics)

**Пізнє зв'язування (Late Binding)** — це техніка створення екземплярів об'єктів та виклику методів у рантаймі, коли типи або методи не були відомі на етапі компіляції (наприклад, типи завантажуються з динамічних бібліотек чи плагінів).

---

## 🚀 Динамічне створення екземплярів: `System.Activator`

Клас `Activator` містить статичні фабричні методи для створення об'єктів за їхнім `Type`.

| Метод `Activator` | Призначення | Посилання на приклад |
| :--- | :--- | :---: |
| `Activator.CreateInstance(Type)` | Викликає безпараметричний конструктор за замовчуванням | [![Код](https://img.shields.io/badge/Код-CreateInstance-blue?style=flat-square)](../LateBindingDemo/Program.cs#L14-L17) |
| `Activator.CreateInstance(Type, object?[])` | Знаходить і викликає відповідний конструктор з аргументами | [![Код](https://img.shields.io/badge/Код-CreateWithArgs-blue?style=flat-square)](../LateBindingDemo/Program.cs#L18-L22) |
| `Activator.CreateInstance<T>()` | Типобезпечна дженерик-версія для відомого інтерфейсу чи базового типу | [![Код](https://img.shields.io/badge/Код-GenericCreate-blue?style=flat-square)](../LateBindingDemo/Program.cs) |

> [!TIP]
> Під капотом `Activator.CreateInstance` кешує конструктори та є швидшим за прямий пошук `type.GetConstructors()` для простих сценаріїв без кастомного інжектування параметрів.

---

## 🧬 Рефлексія дженериків (Generics in Reflection)

У .NET типи-дженерики мають два стани:
1. **Відкритий тип (Open Generic / Generic Type Definition)** — шаблон типу, де аргументи типів ще не вказані (наприклад, `List<>` або `Dictionary<,>`). Не може бути інстанційованим напряму!
2. **Закритий тип (Closed / Constructed Generic Type)** — конкретний тип, де всі аргументи типів замінено на реальні типи (наприклад, `List<Car>` або `Dictionary<string, int>`).

### Таблиця ключових API для роботи з дженериками:

| Метод / Властивість | Опис | Посилання на приклад |
| :--- | :--- | :---: |
| `type.IsGenericType` | Перевіряє, чи є тип дженериком (відкритим або закритим) | [![Код](https://img.shields.io/badge/Код-IsGeneric-green?style=flat-square)](../GenericsReflectionDemo/Program.cs#L8-L17) |
| `type.IsGenericTypeDefinition` | `true`, якщо це незакритий шаблон (наприклад, `typeof(List<>)`) | [![Код](https://img.shields.io/badge/Код-GenericDef-green?style=flat-square)](../GenericsReflectionDemo/Program.cs#L10-L15) |
| `type.GetGenericTypeDefinition()` | Отримує відкритий шаблон із закритого типу | [![Код](https://img.shields.io/badge/Код-GetTypeDef-green?style=flat-square)](../GenericsReflectionDemo/Program.cs) |
| `type.GetGenericArguments()` | Отримує масив аргументів типу (або параметрів типу для відкритого) | [![Код](https://img.shields.io/badge/Код-GetArgs-green?style=flat-square)](../GenericsReflectionDemo/Program.cs#L16-L19) |
| `type.MakeGenericType(Type[])` | **Конструює закритий тип у runtime!** Наприклад: `typeof(Dictionary<,>).MakeGenericType(typeof(string), typeof(Car))` | [![Код](https://img.shields.io/badge/Код-MakeGenericType-green?style=flat-square)](../GenericsReflectionDemo/Program.cs#L20-L28) |
| `method.MakeGenericMethod(Type[])` | **Конструює закритий метод у runtime!** Дозволяє викликати відкриті узагальнені методи | [![Код](https://img.shields.io/badge/Код-MakeGenericMethod-green?style=flat-square)](../GenericsReflectionDemo/Program.cs#L35-L42) |
| `param.GetGenericParameterConstraints()` | Отримує обмеження узагальненого параметра (`where T : class, ICar`) | [![Код](https://img.shields.io/badge/Код-Constraints-green?style=flat-square)](../GenericsReflectionDemo/Program.cs#L45-L55) |

---

## 🛡️ Безпека та виняток `TargetInvocationException`

Коли метод, викликаний через `MethodInfo.Invoke()`, викидає виняток, CLR не прокидає оригінальну помилку напряму. Замість цього вона загортає її у **`System.Reflection.TargetInvocationException`**.

Щоб дізнатися справжню причину помилки, необхідно звертатися до властивості **`InnerException`**:

```csharp
try
{
    method.Invoke(instance, args);
}
catch (TargetInvocationException ex)
{
    // Справжня бізнес-помилка знаходиться тут:
    Exception actualException = ex.InnerException!;
    Console.WriteLine($"Помилка бізнес-логіки: {actualException.Message}");
}
```

🔗 Переглянути демонстрацію обробки помилок можна в [LateBindingDemo/Program.cs](../LateBindingDemo/Program.cs#L37-L49).
