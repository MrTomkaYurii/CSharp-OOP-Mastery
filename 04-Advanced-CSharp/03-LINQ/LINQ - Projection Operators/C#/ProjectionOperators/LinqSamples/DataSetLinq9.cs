using System;
using System.Data;
using System.Linq;

namespace ProjectionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Select — Проєкція в анонімні типи (Anonymous Types)
    // ==============================================================================================
    // Категорія: Projection Operators (Оператори проєкції)
    // Призначення: Створення на льоту нових неіменованих об'єктів (new { Prop1 = ..., Prop2 = ... })
    //              без необхідності окремо оголошувати клас чи структуру.
    //
    // Принцип роботи:
    //   - Компілятор автоматично генерує незмінний (immutable) клас із властивостями, перевантаженими
    //     методами Equals(), GetHashCode() та ToString().
    //   - Дуже корисно для формування DTO або проміжних результатів обробки.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesProjectionOperators
    {
        /// <summary>
        /// Приклад 9: Створення послідовності слів у верхньому та нижньому регістрі за допомогою анонімних типів.
        /// </summary>
        public void DataSetLinq9()
        {
            var words = testDS.Tables["Words"].AsEnumerable();

            #region LINQ Method Syntax (Anonymous Types)

            // Проєктуємо кожен рядок у новий анонімний об'єкт із полями Upper та Lower
            var upperLowerWords = words.Select(p => new
            {
                Upper = p.Field<string>(0).ToUpper(),
                Lower = p.Field<string>(0).ToLower()
            });

            Console.WriteLine("Слова у верхньому та нижньому регістрі (Lambda):");
            foreach (var ul in upperLowerWords)
            {
                Console.WriteLine("Верхній: {0}, Нижній: {1}", ul.Upper, ul.Lower);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 9A: Аналогічна проєкція в анонімний тип із використанням Query Syntax.
        /// </summary>
        public void DataSetLinq9A()
        {
            var words = testDS.Tables["Words"].AsEnumerable();

            #region LINQ Query Syntax (Anonymous Types)

            var upperLowerWords =
                from w in words
                select new
                {
                    Upper = w.Field<string>(0).ToUpper(),
                    Lower = w.Field<string>(0).ToLower()
                };

            Console.WriteLine("************************************************");
            Console.WriteLine("Слова у верхньому та нижньому регістрі (Query Syntax):");
            foreach (var ul in upperLowerWords)
            {
                Console.WriteLine("Верхній: {0}, Нижній: {1}", ul.Upper, ul.Lower);
            }

            #endregion
        }
    }
}
