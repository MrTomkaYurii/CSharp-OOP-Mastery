using LinqSamplesCommon.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AggregateOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Count у комбінації з Distinct
    // ==============================================================================================
    // Категорія: Aggregate Operators (Агрегатні оператори)
    // Призначення: Обчислює кількість унікальних елементів у послідовності.
    //
    // Принцип роботи:
    //   - Distinct() усуває дублікати на основі хеш-множини (Set).
    //   - Count() ітерує результуючу послідовність і підраховує кількість унікальних значень.
    //   - Агрегатні методи згортають послідовність до одного скалярного значення.
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesAggregateOperators
    {
        /// <summary>
        /// Приклад 73: Підрахунок кількості унікальних простих множників числа 300 за допомогою Distinct().Count().
        /// </summary>
        public void Linq73()
        {
            int[] primeFactorsOf300 = { 2, 2, 3, 5, 5 };

            #region Make Sure to try yourself before looking at the code

            // Видаляємо дублікати та рахуємо кількість унікальних множників
            int uniqueFactors = primeFactorsOf300.Distinct().Count();

            Console.WriteLine("There are {0} unique prime factors of 300.", uniqueFactors);

            #endregion
        }
    }
}
