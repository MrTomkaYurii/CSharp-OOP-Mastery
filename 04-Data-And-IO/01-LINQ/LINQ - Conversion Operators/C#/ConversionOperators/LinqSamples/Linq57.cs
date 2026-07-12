using System;
using System.Linq;

namespace ConversionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор OfType<T> (Фільтрація та приведення типів)
    // ==============================================================================================
    // Категорія: Conversion & Restriction Operators (Оператори перетворення та фільтрації за типом)
    // Призначення: Вибирає з гетерогенного масиву об'єктів (object[]) лише ті елементи,
    //              які сумісні із зазначеним типом T, та автоматично приводить їх до T.
    //
    // Відмінність від Cast<T>:
    //   - Cast<T>() намагається привести КОЖЕН елемент до типу T. Якщо хоча б один елемент іншого типу,
    //     виникає InvalidCastException.
    //   - OfType<T>() безпечно пропускає невідповідні елементи (а також null) без жодних винятків.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesConversionOperators
    {
        /// <summary>
        /// Приклад 57: Вибірка тільки дійсних чисел (тип double) зі змішаного масиву об'єктів object[].
        /// </summary>
        public void Linq57()
        {
            // Масив містить null, числа double, рядки, цілі числа int
            object[] numbers = { null, 1.0, "two", 3, "four", 5, "six", 7.0 };

            #region LINQ Method Syntax (OfType)

            // Відбираємо та приводимо тільки значення типу double
            var doubles = numbers.OfType<double>();

            Console.WriteLine("Числа, збережені як double (OfType<double>):");
            foreach (var d in doubles)
            {
                Console.WriteLine("  " + d);
            }

            #endregion
        }
    }
}
