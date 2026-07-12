using LinqSamplesCommon.LinqSamplesHelpers;
using System;
using System.Data;
using System.Linq;

namespace ProjectionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Select (Проєкція елементів)
    // ==============================================================================================
    // Категорія: Projection Operators (Оператори проєкції / трансформації)
    // Призначення: Перетворює (проєктує) кожен елемент вихідної послідовності в нову форму за допомогою
    //              функції трансформації (мапінгу). Відношення: 1 до 1 (1 вхідний -> 1 вихідний).
    //
    // Принцип роботи:
    //   - Приймає делегат Func<TSource, TResult>.
    //   - Застосовує функцію до кожного елемента колекції і повертає трансформований результат.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming). Обчислення виконуються по мірі запиту елементів.
    //
    // Синтаксис:
    //   - Query Syntax: from x in source select transform(x);
    //   - Method Syntax: source.Select(x => transform(x));
    // ==============================================================================================

    public partial class LinqSamplesProjectionOperators
    {
        public DataSet testDS;

        public LinqSamplesProjectionOperators()
        {
            testDS = TestHelper.CreateTestDataset();
        }

        /// <summary>
        /// Приклад 6: Використання select для створення послідовності цілих чисел, збільшених на 1,
        /// за допомогою Query Syntax.
        /// </summary>
        public void DataSetLinq6()
        {
            // Вхідні дані: таблиця "Numbers" у форматі DataTable
            var numbers = testDS.Tables["Numbers"].AsEnumerable();

            #region LINQ Query Syntax

            // Трансформація: кожне число з поля таблиці збільшуємо на 1
            var numsPlusOne =
                from n in numbers
                select n.Field<int>(0) + 1;

            Console.WriteLine("Query Syntax — Числа + 1:");
            foreach (var i in numsPlusOne)
            {
                Console.WriteLine(i);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 6A: Збільшення чисел на 1 за допомогою Method Syntax (метод Select).
        /// </summary>
        public void DataSetLinq6A()
        {
            var numbers = testDS.Tables["Numbers"].AsEnumerable();

            #region LINQ Method Syntax (Lambda)

            // Використання методу розширення Select() для проєкції значень
            var numsPlusOne = numbers.Select(n => n.Field<int>(0) + 1);

            Console.WriteLine();
            Console.WriteLine("Lambda-вираз — Числа + 1:");
            foreach (var i in numsPlusOne)
            {
                Console.WriteLine(i);
            }

            #endregion
        }
    }
}
