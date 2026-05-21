// ============================================================================
// Тема: Статичний клас (static class)
// ----------------------------------------------------------------------------
// static class — це клас, який:
//   * НЕ можна створити через new (немає екземплярів);
//   * НЕ можна успадкувати і від якого не можна успадкувати;
//   * містить ЛИШЕ статичні члени (компілятор це перевіряє);
//   * не має конструктора екземпляра, але може мати СТАТИЧНИЙ конструктор.
//
// Призначення: набір пов'язаних функцій і даних, для яких стан окремого
// об'єкта не має сенсу. Приклади з .NET: System.Math, System.Console,
// System.IO.File, System.Linq.Enumerable.
//
// Порівняйте з SOLIS-p137 (звичайний клас з окремими static-членами):
// static class гарантує, що НЕстатичних членів не буде взагалі.
// ============================================================================

using System;

namespace Ex_Static_Class
{
    /// <summary>
    /// Утиліта для перетворення температур. Стану немає — лише чисті функції.
    /// </summary>
    public static class TemperatureConverter
    {
        // Статичне поле-константа — спільне для всієї програми
        public const double AbsoluteZeroCelsius = -273.15;

        // Статичний лічильник викликів — приклад спільного стану на рівні класу
        public static int ConversionsCount { get; private set; }

        // Статичний конструктор: один раз перед першим зверненням до класу
        static TemperatureConverter()
        {
            Console.WriteLine("[TemperatureConverter] статичний конструктор виконано\n");
        }

        public static double CelsiusToFahrenheit(double celsius)
        {
            Guard(celsius);
            ConversionsCount++;
            return celsius * 9.0 / 5.0 + 32.0;
        }

        public static double FahrenheitToCelsius(double fahrenheit)
        {
            double celsius = (fahrenheit - 32.0) * 5.0 / 9.0;
            Guard(celsius);
            ConversionsCount++;
            return celsius;
        }

        public static double CelsiusToKelvin(double celsius)
        {
            Guard(celsius);
            ConversionsCount++;
            return celsius - AbsoluteZeroCelsius;
        }

        // Приватний статичний метод-помічник (деталь реалізації)
        private static void Guard(double celsius)
        {
            if (celsius < AbsoluteZeroCelsius)
                throw new ArgumentOutOfRangeException(nameof(celsius),
                    $"Температура не може бути нижчою за абсолютний нуль ({AbsoluteZeroCelsius} °C)");
        }
    }

    internal static class Program
    {
        private static void Main()
        {
            // Звертаємось через ім'я класу — об'єкт не потрібен і неможливий
            Console.WriteLine($"20 °C  = {TemperatureConverter.CelsiusToFahrenheit(20):F1} °F");
            Console.WriteLine($"98.6 °F = {TemperatureConverter.FahrenheitToCelsius(98.6):F1} °C");
            Console.WriteLine($"0 °C   = {TemperatureConverter.CelsiusToKelvin(0):F2} K");

            Console.WriteLine($"\nУсього перетворень: {TemperatureConverter.ConversionsCount}");

            // var x = new TemperatureConverter();  // ПОМИЛКА CS0712: не можна створити екземпляр статичного класу

            try
            {
                TemperatureConverter.CelsiusToKelvin(-300);   // нижче абсолютного нуля
            }
            catch (ArgumentOutOfRangeException ex)
            {
                Console.WriteLine($"\nОчікувана помилка: {ex.Message}");
            }
        }
    }
}

// ----------------------------------------------------------------------------
// Що можна додати:
//  * using static Ex_Static_Class.TemperatureConverter; => виклик без імені класу.
//  * Методи розширення в static class (див. Ex-Extension-Methods) — вони бувають
//    ЛИШЕ в статичних класах.
//  * Порівняти зі звичайним класом із приватним конструктором (теж «не створити»,
//    але дозволяє нестатичні члени й успадкування).
//  * Показати, що static class не реалізує інтерфейси (звідси — обмеження в DI).
// ----------------------------------------------------------------------------
