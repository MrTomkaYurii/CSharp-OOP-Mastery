using System;
using System.Linq;

namespace AggregateOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор Aggregate з початковим значенням (Seed)
    // ==============================================================================================
    // Категорія: Aggregate Operators (Агрегатні оператори)
    // Призначення: Виконує користувацьку агрегацію з явно заданим початковим станом акумулятора (seed).
    //
    // Принцип роботи:
    //   - Приймає початкове значення seed (тип якого може відрізнятися від типу елементів послідовності).
    //   - Послідовно передає поточний акумулятор та наступний елемент у функцію агрегації:
    //     TAccumulate Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, TAccumulate> func).
    //   - Якщо послідовність порожня, безпечно повертає початкове значення seed без викидання винятків.
    //   - Ідеально підходить для моделювання бізнес-логіки з накопичувальним станом (баланси, транзакції тощо).
    //
    // Тип виконання:
    //   - Негайне виконання (Immediate Execution).
    // ==============================================================================================

    public partial class LinqSamplesAggregateOperators
    {
        /// <summary>
        /// Приклад 93: Моделювання зняття коштів з банківського рахунку (стартовий баланс 100) за допомогою Aggregate з seed.
        /// </summary>
        public void Linq93()
        {
            double startBalance = 100.0;

            int[] attemptedWithdrawals = { 20, 10, 40, 50, 10, 70, 30 };

            #region Make Sure to try yourself before looking at the code

            // Послідовно списуємо кошти, якщо залишок на балансі дозволяє операцію
            double endBalance =
                attemptedWithdrawals.Aggregate(startBalance,
                    (balance, nextWithdrawal) =>
                        ((nextWithdrawal <= balance) ? (balance - nextWithdrawal) : balance));

            #endregion

            Console.WriteLine("Original 101 Linq examples.");
            Console.WriteLine("Ending balance: {0}", endBalance);
        }
    }
}
