using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JoinOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Сплощення групового з'єднання (Flattening Group Join / Join + SelectMany)
    // ==============================================================================================
    // Категорія: Join Operators (Оператори з'єднання даних)
    // Призначення: Поєднує групове з'єднання (into) із наступним розгортанням вкладеної групи через другий from.
    //
    // Принцип роботи:
    //   - Спочатку виконується group join: join prod in products ... into ps.
    //   - Потім вкладена колекція ps розгортається оператором: from p in ps.
    //   - У результаті отримується плаский список пар (Category, ProductName).
    //   - Якщо для певної категорії немає жодного товару, ps буде порожньою, і `from p in ps` пропустить цю категорію.
    //   - У ламбда-синтаксисі це відповідає комбінації GroupJoin(...).SelectMany(...).
    //
    // Тип виконання:
    //   - Відкладене виконання (Deferred Execution, Streaming).
    // ==============================================================================================

    public partial class LinqSamplesJoinOperators
    {
        /// <summary>
        /// Приклад 104: З'єднання масиву категорій зі списком товарів із розгортанням результатів через group join та from (Query Syntax).
        /// </summary>
        public void Linq104()
        {
            string[] categories = new string[]{
                "Beverages",
                "Condiments",
                "Vegetables",
                "Dairy Products",
                "Seafood" };

            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            // Спочатку групуємо товари за категорією into ps, а потім розгортаємо через from p in ps
            var prodByCategory =
                from cat in categories
                join prod in products on cat equals prod.Category into ps
                from p in ps
                select new { Category = cat, p.ProductName };

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            foreach (var item in prodByCategory.Take(10))
            {
                Console.WriteLine(item.ProductName + ": " + item.Category);
            }
        }

        /// <summary>
        /// Приклад 104A: Сплощення групового з'єднання за допомогою GroupJoin(...).SelectMany(...) (Method Syntax).
        /// </summary>
        public void Linq104A()
        {
            string[] categories = new string[]{
                "Beverages",
                "Condiments",
                "Vegetables",
                "Dairy Products",
                "Seafood" };

            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            var prodByCategory = categories
                .GroupJoin(products, 
                    cat => cat, 
                    prod => prod.Category,
                    (cat, ps) => new { Category = cat, ps })
                .SelectMany(x => x.ps, (cat, ps) => new { cat.Category, ps.ProductName });

            #endregion

            Console.WriteLine();
            Console.WriteLine("************************************************");
            Console.WriteLine("Linq with Lambda.");
            foreach (var item in prodByCategory.Take(10))
            {
                Console.WriteLine(item.ProductName + ": " + item.Category);
            }
        }
    }
}
