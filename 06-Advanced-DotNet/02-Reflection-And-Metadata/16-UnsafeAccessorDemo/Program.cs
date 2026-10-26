using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using Shared.Common;

namespace UnsafeAccessorDemo;

internal class Program
{
    #region Визначення UnsafeAccessor інтринсиків (.NET 8+)

    // 1. Прямий доступ до приватного поля _licensePlate (тип string)
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_licensePlate")]
    extern static ref string GetPrivateLicensePlate(Car car);

    // 2. Прямий доступ до приватного поля _odometer (тип int)
    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_odometer")]
    extern static ref int GetPrivateOdometer(Car car);

    // 3. Прямий виклик приватного методу ResetOdometer()
    [UnsafeAccessor(UnsafeAccessorKind.Method, Name = "ResetOdometer")]
    extern static void CallPrivateResetOdometer(Car car);

    // 4. Прямий виклик приватного конструктора: Car(string licensePlate, double maxSpeed, int odometer)
    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    extern static Car CallPrivateConstructor(string licensePlate, double maxSpeed, int odometer);

    #endregion

    static void Main()
    {
        Console.WriteLine("=== 16. Сучасний доступ до непублічних членів: [UnsafeAccessor] (.NET 8+) ===\n");

        // 1. Інстанціювання через приватний конструктор БЕЗ рефлексії
        Console.WriteLine("--- 1. Створення об'єкта через закритий приватний конструктор ---");
        Car car = CallPrivateConstructor("AX 7777 KX", 280.0, 95000);
        Console.WriteLine($"Створено авто з номерами: {car.LicensePlate}");
        Console.WriteLine($"Одометр (початковий):     {GetPrivateOdometer(car)} км\n");

        // 2. Читання та зміна приватних полів за посиланням (ref)
        Console.WriteLine("--- 2. Модифікація приватного поля _licensePlate через ref повернення ---");
        ref string plateRef = ref GetPrivateLicensePlate(car);
        Console.WriteLine($"Поточний номер через UnsafeAccessor: {plateRef}");
        plateRef = "KA 9999 II"; // Змінюємо значення поля напряму в пам'яті!
        Console.WriteLine($"Оновлений номер:                     {car.LicensePlate}\n");

        // 3. Прямий виклик приватного методу
        Console.WriteLine("--- 3. Виклик приватного методу ResetOdometer() ---");
        Console.WriteLine($"Одометр до скидання:    {GetPrivateOdometer(car)} км");
        CallPrivateResetOdometer(car);
        Console.WriteLine($"Одометр після скидання: {GetPrivateOdometer(car)} км\n");

        // 4. Бенчмарк швидкодії: Reflection (FieldInfo) проти [UnsafeAccessor]
        Console.WriteLine("--- 4. Порівняння швидкодії: FieldInfo vs [UnsafeAccessor] (1,000,000 операцій) ---");
        FieldInfo fieldInfo = typeof(Car).GetField("_odometer", BindingFlags.NonPublic | BindingFlags.Instance)!;
        int iterations = 1_000_000;

        // Тест класичної рефлексії
        var sw = Stopwatch.StartNew();
        for (int i = 0; i < iterations; i++)
        {
            fieldInfo.SetValue(car, i);
        }
        sw.Stop();
        long reflectionMs = sw.ElapsedMilliseconds;

        // Тест UnsafeAccessor
        sw.Restart();
        for (int i = 0; i < iterations; i++)
        {
            GetPrivateOdometer(car) = i;
        }
        sw.Stop();
        long unsafeMs = sw.ElapsedMilliseconds;

        Console.WriteLine($"Класична рефлексія (FieldInfo.SetValue): {reflectionMs} ms");
        Console.WriteLine($"Сучасний [UnsafeAccessor] (Compiler Intrinsic): {unsafeMs} ms");
        Console.WriteLine($"Прискорення: у {((double)reflectionMs / Math.Max(unsafeMs, 1)):F1} разів швидше!");
        Console.WriteLine("Висновок: [UnsafeAccessor] генерує прямі асемблерні інструкції з нульовим оверхедом і 100% підтримкою AOT/Trimming.");
    }
}
