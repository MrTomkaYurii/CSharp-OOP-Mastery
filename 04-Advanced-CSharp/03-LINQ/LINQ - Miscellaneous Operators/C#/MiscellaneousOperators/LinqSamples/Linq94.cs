using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MiscellaneousOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Concat (Конкатенація двох послідовностей)
    // ==============================================================================================
    // Категорія: Miscellaneous Operators (Різні допоміжні оператори)
    // Призначення: Поєднує дві послідовності в одну, розташовуючи елементи другої послідовності слідом за першою.
    //
    // Принцип роботи:
    //   - На відміну від Union, Concat НЕ перевіряє дублікати і НЕ використовує Set для хешування.
    //   - Повністю зберігає порядок елементів і всі повтори.
    //   - Працює максимально швидко у потоковому режимі: спочатку перебирає першу колекцію, потім другу.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesMiscellaneousOperators
    {
        /// <summary>
        /// Приклад 94: Об'єднання двох числових масивів в одну послідовність за допомогою Concat.
        /// </summary>
        public void Linq94()
        {
            int[] numbersA = { 0, 2, 4, 5, 6, 8, 9 };
            int[] numbersB = { 1, 3, 5, 7, 8 };

            #region Make Sure to try yourself before looking at the code

            // Поєднуємо масиви один за одним із збереженням дублікатів (числа 5 та 8 повторюються)
            var allNumbers = numbersA.Concat(numbersB);

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            Console.WriteLine("All numbers from both arrays:");
            foreach (var n in allNumbers)
            {
                Console.WriteLine(n);
            }
        }
    }
}
