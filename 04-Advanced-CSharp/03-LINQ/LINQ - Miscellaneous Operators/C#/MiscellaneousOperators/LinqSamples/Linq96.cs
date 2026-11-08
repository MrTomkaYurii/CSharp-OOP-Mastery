using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MiscellaneousOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор SequenceEqual (Поелементне порівняння послідовностей)
    // ==============================================================================================
    // Категорія: Miscellaneous Operators (Різні допоміжні оператори)
    // Призначення: Перевіряє, чи дві послідовності є ідентичними за складом елементів та їхнім порядком.
    //
    // Принцип роботи:
    //   - Одночасно перебирає обидві послідовності за допомогою двох ітераторів IEnumerator.
    //   - Порівнює відповідні пари елементів за допомогою стандартного або переданого IEqualityComparer<T>.
    //   - Повертає true тоді й тільки тоді, коли обидві колекції мають однакову довжину і всі відповідні елементи рівні.
    //   - Зупиняє перевірку на першій же невідповідності (short-circuiting).
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesMiscellaneousOperators
    {
        /// <summary>
        /// Приклад 96: Порівняння двох ідентичних масивів рядків за допомогою SequenceEqual (результат true).
        /// </summary>
        public void Linq96()
        {
            var wordsA = new string[] { "cherry", "apple", "blueberry" };
            var wordsB = new string[] { "cherry", "apple", "blueberry" };

            #region Make Sure to try yourself before looking at the code

            // Послідовності однакові за елементами та порядком розташування -> true
            bool match = wordsA.SequenceEqual(wordsB);

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            Console.WriteLine("The sequences match: {0}", match);
        }
    }
}
