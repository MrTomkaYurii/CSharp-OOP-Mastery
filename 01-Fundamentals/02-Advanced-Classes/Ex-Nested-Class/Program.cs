// ============================================================================
// Тема: Вкладені класи (nested types)
// ----------------------------------------------------------------------------
// Клас можна оголосити ВСЕРЕДИНІ іншого класу. Навіщо:
//   * вкладений тип — деталь реалізації зовнішнього (напр. вузол списку),
//     його не варто показувати всьому проєкту;
//   * вкладений тип має доступ до ПРИВАТНИХ членів зовнішнього класу
//     (це виняток із загального правила інкапсуляції);
//   * логічне групування: тип існує тільки в контексті зовнішнього.
//
// Важливі правила:
//   * для вкладених типів доступний модифікатор private (для звичайних —
//     тільки internal/public); за замовчуванням вкладений тип — private;
//   * вкладений тип НЕ отримує автоматично посилання на екземпляр зовнішнього
//     класу — якщо треба, його передають явно (тут — параметром owner);
//   * звертання ззовні: Outer.Inner (якщо доступ дозволяє).
// ============================================================================

using System;
using System.Collections;
using System.Collections.Generic;

namespace Ex_Nested_Class
{
    /// <summary>
    /// Однозв'язний список. Клас вузла Node прихований усередині —
    /// користувачам списку про нього знати не треба.
    /// </summary>
    public class SinglyLinkedList<T> : IEnumerable<T>
    {
        // PRIVATE вкладений клас — видно тільки всередині SinglyLinkedList<T>
        private sealed class Node
        {
            public T Value;
            public Node? Next;

            public Node(T value) => Value = value;
        }

        private Node? _head;
        private Node? _tail;
        public int Count { get; private set; }

        public void Add(T value)
        {
            var node = new Node(value);
            if (_head is null)
                _head = _tail = node;
            else
            {
                _tail!.Next = node;
                _tail = node;
            }
            Count++;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (Node? current = _head; current is not null; current = current.Next)
                yield return current.Value;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        // PUBLIC вкладений клас — доступний ззовні як SinglyLinkedList<T>.Statistics.
        // Демонструє доступ вкладеного типу до ПРИВАТНИХ полів зовнішнього об'єкта.
        public sealed class Statistics
        {
            private readonly SinglyLinkedList<T> _owner;   // посилання передаємо явно

            public Statistics(SinglyLinkedList<T> owner) => _owner = owner;

            public bool IsEmpty => _owner._head is null;   // читаємо приватне поле _head!

            public T? First => _owner._head is null ? default : _owner._head.Value;

            public T? Last => _owner._tail is null ? default : _owner._tail.Value;
        }

        public Statistics GetStatistics() => new Statistics(this);
    }

    internal static class Program
    {
        private static void Main()
        {
            var list = new SinglyLinkedList<string>();
            list.Add("альфа");
            list.Add("бета");
            list.Add("гамма");

            Console.WriteLine($"Елементів: {list.Count}");
            foreach (var item in list)          // працює завдяки IEnumerable<T>
                Console.WriteLine($"  {item}");

            // Використовуємо ПУБЛІЧНИЙ вкладений тип
            SinglyLinkedList<string>.Statistics stats = list.GetStatistics();
            Console.WriteLine($"\nПорожній? {stats.IsEmpty}");
            Console.WriteLine($"Перший:   {stats.First}");
            Console.WriteLine($"Останній: {stats.Last}");

            // new SinglyLinkedList<string>.Node("x");  // ПОМИЛКА: Node приватний
        }
    }
}

// ----------------------------------------------------------------------------
// Що можна додати:
//  * Патерн «Будівельник» як вкладений клас: Pizza.Builder.
//  * Вкладений enum або struct (напр. Color усередині Palette).
//  * Приватний вкладений клас-компаратор для сортування.
//  * Показати різницю: вкладений тип НЕ має неявного this зовнішнього класу
//    (на відміну від внутрішніх класів у Java).
// ----------------------------------------------------------------------------
