using System;
using System.Collections.Generic;
using System.Linq;

namespace ConversionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор ToList (Примусова матеріалізація в динамічний список List<T>)
    // ==============================================================================================
    // Категорія: Conversion Operators (Оператори перетворення типів / Матеріалізації)
    // Призначення: Примусово ітерує вихідну послідовність та зберігає результати в екземплярі List<T>.
    //
    // Принцип роботи:
    //   - Перериває відкладеність запиту (Immediate Execution).
    //   - Створює динамічний список у пам'яті (Heap allocation), до якого можна додавати (Add)
    //     чи видаляти (Remove) елементи.
    //   - Кешує «знімок даних» (snapshot), запобігаючи повторним обчисленням при подальшому читанні.
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesConversionOperators
    {
        /// <summary>
        /// Приклад 55: Негайне виконання запиту сортування слів та створення списку List<string> (Query Syntax).
        /// </summary>
        public void Linq55()
        {
            string[] words = { "cherry", "apple", "blueberry" };

            #region LINQ Query Syntax + ToList()

            var sortedWords =
                from w in words
                orderby w
                select w;

            // Матеріалізація в динамічний список List<string>
            List<string> wordList = sortedWords.ToList();

            Console.WriteLine("Query Syntax + ToList — Відсортований список:");
            foreach (var w in wordList)
            {
                Console.WriteLine("  " + w);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 55A: Матеріалізація результатів у список за допомогою Method Syntax (OrderBy + ToList).
        /// </summary>
        public void Linq55A()
        {
            string[] words = { "cherry", "apple", "blueberry" };

            #region LINQ Method Syntax + ToList()

            var sortedWords = words.OrderBy(w => w).ToList();

            Console.WriteLine();
            Console.WriteLine("*****************************************");
            Console.WriteLine("Method Syntax + ToList — Відсортований список:");
            foreach (var w in sortedWords)
            {
                Console.WriteLine("  " + w);
            }

            #endregion
        }
    }
}
