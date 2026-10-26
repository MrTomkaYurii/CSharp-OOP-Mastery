using System.Reflection;
using System.Reflection.Emit;

namespace DynamicMethodEmit;

internal class Program
{
    static void Main()
    {
        Console.WriteLine("=== 07. Динамічне метапрограмування: Reflection.Emit та DynamicMethod ===\n");

        // 1. Створення швидкого математичного методу на льоту: f(a, b) = a * a + b * b
        Console.WriteLine("--- 1. Генерація математичного методу через DynamicMethod ---");
        var mathDelegate = BuildSumOfSquaresMethod();

        int a = 3, b = 4;
        int result = mathDelegate(a, b);
        Console.WriteLine($"Скомпільовано динамічний метод f(a, b) = a^2 + b^2");
        Console.WriteLine($"f({a}, {b}) = {result} (Очікувано: 3^2 + 4^2 = 9 + 16 = 25)\n");

        // 2. Створення високошвидкісного динамічного гетера властивості
        Console.WriteLine("--- 2. Генерація нативного гетера властивості (швидка альтернатива PropertyInfo.GetValue) ---");
        var product = new Product { Title = "Gaming Laptop", Price = 1499.99m };

        // Звичайний PropertyInfo для порівняння
        PropertyInfo prop = typeof(Product).GetProperty(nameof(Product.Price))!;

        // Динамічно згенерований IL-гетер
        Func<object, object> fastGetter = BuildDynamicPropertyGetter(prop);

        object normalValue = prop.GetValue(product)!;
        object fastValue = fastGetter(product);

        Console.WriteLine($"Читання через звичайний PropertyInfo:   ${normalValue}");
        Console.WriteLine($"Читання через скомпільований IL-гетер: ${fastValue}");
        Console.WriteLine("Висновок: DynamicMethod компілюється в машинний код JIT і виконується зі швидкістю прямого C# коду.");
    }

    /// <summary>
    /// Генерує динамічний метод: (int a, int b) => a * a + b * b
    /// </summary>
    private static Func<int, int, int> BuildSumOfSquaresMethod()
    {
        var dynamicMethod = new DynamicMethod(
            name: "SumOfSquares",
            returnType: typeof(int),
            parameterTypes: new[] { typeof(int), typeof(int) });

        ILGenerator il = dynamicMethod.GetILGenerator();

        // Обчислення a * a:
        il.Emit(OpCodes.Ldarg_0); // Завантажити аргумент 0 (a) у стек
        il.Emit(OpCodes.Dup);     // Продублювати верхнє значення стека (a)
        il.Emit(OpCodes.Mul);     // Помножити a * a (результат у стеку)

        // Обчислення b * b:
        il.Emit(OpCodes.Ldarg_1); // Завантажити аргумент 1 (b) у стек
        il.Emit(OpCodes.Dup);     // Продублювати b
        il.Emit(OpCodes.Mul);     // Помножити b * b (результат у стеку)

        // Додавання результатів:
        il.Emit(OpCodes.Add);     // (a*a) + (b*b)
        il.Emit(OpCodes.Ret);     // Повернути результат

        return (Func<int, int, int>)dynamicMethod.CreateDelegate(typeof(Func<int, int, int>));
    }

    /// <summary>
    /// Генерує типізований IL-делегат для читання властивості об'єкта без накладних витрат рефлексії:
    /// (object target) => (object)((Product)target).get_Price()
    /// </summary>
    private static Func<object, object> BuildDynamicPropertyGetter(PropertyInfo property)
    {
        MethodInfo getMethod = property.GetGetMethod()!;
        Type targetType = property.DeclaringType!;

        var dynamicMethod = new DynamicMethod(
            name: $"DynamicGet_{property.Name}",
            returnType: typeof(object),
            parameterTypes: new[] { typeof(object) });

        ILGenerator il = dynamicMethod.GetILGenerator();

        il.Emit(OpCodes.Ldarg_0);                   // Завантажити екземпляр типу object у стек
        il.Emit(OpCodes.Castclass, targetType);     // Привести тип object до конкретного класу (Product)
        il.Emit(OpCodes.Callvirt, getMethod);       // Викликати метод get_Price()

        // Якщо тип властивості є ValueType (decimal, int), упаковуємо його для повернення як object
        if (property.PropertyType.IsValueType)
        {
            il.Emit(OpCodes.Box, property.PropertyType);
        }

        il.Emit(OpCodes.Ret);                       // Повернути значення

        return (Func<object, object>)dynamicMethod.CreateDelegate(typeof(Func<object, object>));
    }
}

public class Product
{
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
