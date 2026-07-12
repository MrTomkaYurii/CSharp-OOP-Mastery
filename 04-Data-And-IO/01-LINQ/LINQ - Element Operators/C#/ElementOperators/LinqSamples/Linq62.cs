using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElementOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор FirstOrDefault з предикатом для посилальних типів
    // ==============================================================================================
    // Категорія: Element Operators (Оператори елементів)
    // Призначення: Знаходить перший елемент за предикатом або повертає null для посилального типу (class).
    //
    // Принцип роботи:
    //   - Здійснює пошук першого збігу. Якщо знайдено — повертає посилання на екземпляр.
    //   - Якщо збігу немає — повертає null без викидання InvalidOperationException.
    //   - Це є ідіоматичним і безпечним патерном перевірки наявності об'єкта перед подальшим використанням:
    //     var item = collection.FirstOrDefault(x => x.Id == id);
    //     if (item != null) { ... }
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesElementOperators
    {
        /// <summary>
        /// Приклад 62: Пошук товару з ProductID == 789 за допомогою FirstOrDefault; перевірка на null за відсутності збігу.
        /// </summary>
        public void Linq62()
        {
            List<Products.Product> products = Products.GetProductList();

            #region Make Sure to try yourself before looking at the code

            // Шукаємо товар із неіснуючим ID; оскільки це class (reference type), отримуємо null
            Products.Product product789 = products.FirstOrDefault(p => p.ProductID == 789);

            Console.WriteLine("Product 789 exists: {0}", product789 != null);

            #endregion
        }
    }
}
