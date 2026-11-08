using System;
using System.Linq;

namespace AggregateOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Aggregate (Згортка послідовності / Fold / Reduce)
    // ==============================================================================================
    // Категорія: Aggregate Operators (Агрегатні оператори)
    // Призначення: Виконує користувацьку операцію накопичення (акумуляції) над послідовністю.
    //
    // Принцип роботи:
    //   - Реалізує фундаментальний патерн функціонального програмування Fold/Reduce.
    //   - У формі без початкового значення (seed) першим значенням акумулятора стає перший елемент колекції,
    //     а функція викликається послідовно для всіх наступних елементів:
    //     acc = f(acc, next).
    //   - Якщо послідовність порожня, викидає InvalidOperationException.
    //   - Дозволяє реалізувати будь-яку операцію агрегації (добуток чисел, конкатенацію рядків тощо).
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesAggregateOperators
    {
        /// <summary>
        /// Приклад 92: Обчислення загального добутку всіх дійсних чисел у масиві за допомогою Aggregate.
        /// </summary>
        public void Linq92()
        {
            double[] doubles = { 1.7, 2.3, 1.9, 4.1, 2.9 };

            #region Make Sure to try yourself before looking at the code

            // Послідовно перемножуємо елементи: ((1.7 * 2.3) * 1.9) * ...
            double product = doubles.Aggregate((runningProduct, nextFactor) => runningProduct * nextFactor);

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            Console.WriteLine("Total product of all numbers: {0}", product);
        }
    }
}
