using System;
using System.Data;
using System.Linq;

namespace ProjectionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Select — Вибірка окремого поля / властивості
    // ==============================================================================================
    // Категорія: Projection Operators (Оператори проєкції)
    // Призначення: Витягує тільки одне конкретне поле чи властивість зі складної сутності,
    //              звужуючи потік даних до простої послідовності значень (наприклад, IEnumerable<string>).
    //
    // Принцип роботи:
    //   - Замість повернення всього об'єкта повертається лише потрібне значення (наприклад, ProductName).
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesProjectionOperators
    {
        /// <summary>
        /// Приклад 7: Вибірка тільки назв товарів (ProductName) за допомогою Query Syntax.
        /// </summary>
        public void DataSetLinq7()
        {
            // Вхідні дані: таблиця продуктів
            var products = testDS.Tables["Products"].AsEnumerable();

            #region LINQ Query Syntax

            // Проєктуємо сутності продуктів у послідовність рядків із назвами
            var productNames =
                from p in products
                select p.Field<string>("ProductName");

            Console.WriteLine("Query Syntax — Назви продуктів:");
            foreach (var productName in productNames)
            {
                Console.WriteLine(productName);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 7A: Вибірка назв товарів за допомогою Method Syntax (Select).
        /// </summary>
        public void DataSetLinq7A()
        {
            var products = testDS.Tables["Products"].AsEnumerable();

            #region LINQ Method Syntax (Lambda)

            // Використання методу Select для вилучення тільки поля ProductName
            var productNames = products.Select(name => name.Field<string>("ProductName"));

            Console.WriteLine("************************************************");
            Console.WriteLine("Lambda-вираз — Назви продуктів:");
            foreach (var productName in productNames)
            {
                Console.WriteLine(productName);
            }

            #endregion
        }
    }
}
