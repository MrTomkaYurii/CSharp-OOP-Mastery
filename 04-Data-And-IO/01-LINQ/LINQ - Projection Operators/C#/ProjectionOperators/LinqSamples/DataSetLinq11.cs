using System;
using System.Data;
using System.Linq;

namespace ProjectionOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Select — Проєкція з перейменуванням властивостей
    // ==============================================================================================
    // Категорія: Projection Operators (Оператори проєкції)
    // Призначення: Формування скороченої моделі даних (DTO) з одночасним перейменуванням властивостей
    //              (наприклад, UnitPrice перейменовується на Price).
    //
    // Принцип роботи:
    //   - В анонімному типі задається новий ідентифікатор: new { Price = prod.UnitPrice }.
    //   - Це дозволяє адаптувати формат даних під вимоги інтерфейсу користувача або зовнішнього API.
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesProjectionOperators
    {
        /// <summary>
        /// Приклад 11: Проєкція властивостей товару в анонімний тип із перейменуванням UnitPrice на Price (Method Syntax).
        /// </summary>
        public void DataSetLinq11()
        {
            var products = testDS.Tables["Products"].AsEnumerable();

            #region LINQ Method Syntax (Lambda)

            var productInfos = products.Select(n => new
            {
                ProductName = n.Field<string>("ProductName"),
                Category = n.Field<string>("Category"),
                Price = n.Field<decimal>("UnitPrice")
            });

            Console.WriteLine("Інформація про товари (Lambda):");
            foreach (var productInfo in productInfos)
            {
                Console.WriteLine("{0} належить до категорії {1} та коштує {2:C} за одиницю.",
                    productInfo.ProductName, productInfo.Category, productInfo.Price);
            }

            #endregion
        }

        /// <summary>
        /// Приклад 11A: Проєкція властивостей товару за допомогою Query Syntax.
        /// </summary>
        public void DataSetLinq11A()
        {
            var products = testDS.Tables["Products"].AsEnumerable();

            #region LINQ Query Syntax

            var productInfos =
                from n in products
                select new
                {
                    ProductName = n.Field<string>("ProductName"),
                    Category = n.Field<string>("Category"),
                    Price = n.Field<decimal>("UnitPrice")
                };

            Console.WriteLine();
            Console.WriteLine("******************************");
            Console.WriteLine("Інформація про товари (Query Syntax):");
            foreach (var productInfo in productInfos)
            {
                Console.WriteLine("{0} належить до категорії {1} та коштує {2:C} за одиницю.",
                    productInfo.ProductName, productInfo.Category, productInfo.Price);
            }

            #endregion
        }
    }
}
