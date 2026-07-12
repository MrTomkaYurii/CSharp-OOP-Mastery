using System;
using System.Linq;

namespace OrderingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Reverse (Інвертування порядку елементів)
    // ==============================================================================================
    // Категорія: Ordering Operators (Оператори впорядкування / сортування)
    // Призначення: Перевертає порядок слідування елементів у послідовності на протилежний (від кінця до початку).
    //
    // Принцип роботи:
    //   - Reverse() НЕ порівнює значення між собою (на відміну від OrderBy).
    //   - Він просто інвертує фізичний порядок надходження елементів.
    //   - Для виконання операції необхідна буферизація всієї колекції у пам'яті (щоб повернути останній
    //     елемент першим).
    //
    // Тип виконання:
    //   - Відкладене виконання з буферизацією (Deferred Execution, Non-streaming / Buffering).
    // ==============================================================================================

    public partial class LinqSamplesOrderingOperators
    {
        /// <summary>
        /// Приклад 39: Вибірка цифр, друга літера в назві яких 'i', та інвертування їхнього порядку (Query Syntax + Reverse).
        /// </summary>
        public void Linq39()
        {
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            #region LINQ Query Syntax + Reverse()

            // Фільтруємо цифри з другою літерою 'i' ("five", "six", "nine") та розвертаємо список
            var reversedIDigits = (
                from digit in digits
                where digit[1] == 'i'
                select digit)
                .Reverse();

            Console.WriteLine("Query Syntax — Слова з другою літерою 'i' у зворотному порядку:");
            foreach (var d in reversedIDigits)
            {
                Console.WriteLine(d);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 39A: Інвертування порядку слів за допомогою Method Syntax (Where + Reverse).
        /// </summary>
        public void Linq39A()
        {
            string[] digits = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" };

            #region LINQ Method Syntax (Where + Reverse)

            var reversedIDigits = digits
                .Where(digit => digit[1] == 'i')
                .Reverse();

            Console.WriteLine();
            Console.WriteLine("*****************************");
            Console.WriteLine("Method Syntax — Слова з другою літерою 'i' у зворотному порядку:");
            foreach (var d in reversedIDigits)
            {
                Console.WriteLine(d);
            }

            #endregion
        }
    }
}
