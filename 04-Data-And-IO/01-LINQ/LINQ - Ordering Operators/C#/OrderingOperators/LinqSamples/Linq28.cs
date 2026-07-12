using System;
using System.Linq;

namespace OrderingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор OrderBy (Сортування за зростанням / Ascending)
    // ==============================================================================================
    // Категорія: Ordering Operators (Оператори впорядкування / сортування)
    // Призначення: Впорядковує елементи послідовності за зростанням на основі заданого ключа.
    //
    // Принцип роботи:
    //   - За замовчуванням використовується стандартний компаратор порівняння (Comparer<TKey>.Default).
    //   - Для рядків виконується лексикографічне сортування за зростанням.
    //   - Повертає типізовану послідовність IOrderedEnumerable<TElement>, що дозволяє застосовувати
    //     подальше вторинне сортування (ThenBy).
    //
    // Тип виконання:
    //   - Відкладене виконання з буферизацією (Deferred Execution, Non-streaming / Buffering).
    //     Для знаходження першого елемента LINQ зобов'язаний вичитати й відсортувати всю колекцію.
    //
    // Синтаксис:
    //   - Query Syntax: from x in source orderby x select x;
    //   - Method Syntax: source.OrderBy(x => x);
    // ==============================================================================================

    public partial class LinqSamplesOrderingOperators
    {
        /// <summary>
        /// Приклад 28: Сортування списку слів за алфавітом за допомогою Query Syntax.
        /// </summary>
        public void Linq28()
        {
            string[] words = { "cherry", "apple", "blueberry" };

            #region LINQ Query Syntax

            var sortedWords =
                from word in words
                orderby word
                select word;

            Console.WriteLine("Query Syntax — Відсортовані за алфавітом слова:");
            foreach (var w in sortedWords)
            {
                Console.WriteLine(w);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 28A: Сортування слів за алфавітом за допомогою Method Syntax (OrderBy).
        /// </summary>
        public void Linq28A()
        {
            string[] words = { "cherry", "apple", "blueberry" };

            #region LINQ Method Syntax (Lambda)

            var sortedWords = words.OrderBy(word => word);

            Console.WriteLine();
            Console.WriteLine("**********************************");
            Console.WriteLine("Lambda-вираз — Відсортовані за алфавітом слова:");
            foreach (var w in sortedWords)
            {
                Console.WriteLine(w);
            }

            #endregion
        }
    }
}
