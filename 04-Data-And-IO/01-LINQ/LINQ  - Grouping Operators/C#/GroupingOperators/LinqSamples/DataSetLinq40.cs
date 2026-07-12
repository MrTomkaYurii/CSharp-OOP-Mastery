using LinqSamplesCommon.LinqSamplesHelpers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace GroupingOperators
{
    // ==============================================================================================
    // ТЕОРЕТИЧНА ДОВІДКА: Оператор GroupBy (Групування елементів)
    // ==============================================================================================
    // Категорія: Grouping Operators (Оператори групування)
    // Призначення: Розподіляє елементи вхідної послідовності на групи за спільним значенням ключа (Key).
    //
    // Принцип роботи:
    //   - Повертає послідовність груп типу IEnumerable<IGrouping<TKey, TElement>>.
    //   - Інтерфейс IGrouping<TKey, TElement> розширює IEnumerable<TElement> і містить властивість Key.
    //   - У Query Syntax використовується конструкція:
    //       group element by key into g
    //   - Де 'into g' створює змінну діапазону для отриманої групи.
    //
    // Тип виконання:
    //   - Відкладене виконання з повною буферизацією (Deferred Execution, Non-streaming / Buffering).
    //     Для формування груп LINQ повинен переглянути всю вихідну колекцію.
    // ==============================================================================================

    public partial class LinqSamplesGroupingOperators
    {
        public DataSet testDS;

        public LinqSamplesGroupingOperators()
        {
            testDS = TestHelper.CreateTestDataset();
        }

        /// <summary>
        /// Допоміжний компаратор для виявлення анаграм (слів, що складаються з однакових літер).
        /// </summary>
        private class AnagramEqualityComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return getCanonicalString(x) == getCanonicalString(y);
            }

            public int GetHashCode(string obj)
            {
                return getCanonicalString(obj).GetHashCode();
            }

            private string getCanonicalString(string word)
            {
                char[] wordChars = word.ToCharArray();
                Array.Sort(wordChars);
                return new string(wordChars);
            }
        }

        /// <summary>
        /// Приклад 40: Групування чисел за остачею від їхнього ділення на 5 (Query Syntax).
        /// </summary>
        public void DataSetLinq40()
        {
            EnumerableRowCollection<DataRow> numbers = testDS.Tables["Numbers"].AsEnumerable();

            #region LINQ Query Syntax (group by % 5)

            var numberGroups =
                from n in numbers
                group n by n.Field<int>("number") % 5 into g
                select new { Remainder = g.Key, Numbers = g };

            Console.WriteLine("Query Syntax — Числа, згруповані за остачею від ділення на 5:");
            foreach (var g in numberGroups)
            {
                Console.WriteLine("Числа з остачею {0} при діленні на 5:", g.Remainder);
                foreach (var n in g.Numbers)
                {
                    Console.WriteLine("  " + n.Field<int>("number"));
                }
            }

            #endregion
        }

        /// <summary>
        /// Приклад 40A: Групування чисел за остачею від ділення на 5 за допомогою Method Syntax (GroupBy).
        /// </summary>
        public void DataSetLinq40A()
        {
            EnumerableRowCollection<DataRow> numbers = testDS.Tables["Numbers"].AsEnumerable();

            #region LINQ Method Syntax (GroupBy + Select)

            var numberGroups = numbers
                .GroupBy(n => n.Field<int>("number") % 5)
                .Select(g => new { Remainder = g.Key, Numbers = g });

            Console.WriteLine();
            Console.WriteLine("***********************************");
            Console.WriteLine("Method Syntax — Числа, згруповані за остачею від ділення на 5:");
            foreach (var g in numberGroups)
            {
                Console.WriteLine("Числа з остачею {0} при діленні на 5:", g.Remainder);
                foreach (var n in g.Numbers)
                {
                    Console.WriteLine("  " + n.Field<int>("number"));
                }
            }

            #endregion
        }
    }
}
