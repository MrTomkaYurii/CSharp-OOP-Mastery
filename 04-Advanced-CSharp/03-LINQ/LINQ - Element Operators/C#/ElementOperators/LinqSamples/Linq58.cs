using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElementOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор First (Отримання першого елемента)
    // ==============================================================================================
    // Категорія: Element Operators (Оператори елементів)
    // Призначення: Повертає перший елемент послідовності (або перший, що відповідає предикату).
    //
    // Принцип роботи:
    //   - Обчислює перший елемент і негайно зупиняє перебір послідовності.
    //   - Якщо послідовність порожня або жоден елемент не відповідає умові, викидає виняток:
    //     System.InvalidOperationException: Sequence contains no elements / no matching element.
    //   - Використовується, коли ми гарантовано знаємо, що шуканий елемент обов'язково присутній у колекції.
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesElementOperators
    {
        /// <summary>
        /// Приклад 58: Використання First для повернення єдиного об'єкта Product з ProductID == 12 замість послідовності (Query Syntax).
        /// </summary>
        public void Linq58()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            // Отримуємо перший знайдений товар із ProductID == 12
            Products.Product product12 = (
                from prod in products
                where prod.ProductID == 12
                select prod)
                .First();

            ObjectDumper.Write(product12);

            #endregion
        }

        /// <summary>
        /// Приклад 58A: Пошук першого елемента через ланцюжок методів Where(...).First() (Method Syntax).
        /// </summary>
        public void Linq58A()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            // Альтернативний ланцюжок методів розширення
            Products.Product product12 = products.Where(prod => prod.ProductID == 12).First();

            Console.WriteLine();
            Console.WriteLine("*****************************");
            Console.WriteLine("Linq with Linq");
            ObjectDumper.Write(product12);

            #endregion
        }
    }
}
