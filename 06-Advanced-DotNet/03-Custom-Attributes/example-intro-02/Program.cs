using System.Reflection;
using System;
using System.Linq;
using System.Reflection;

namespace StrategyWithReflection
{
    // Інтерфейс стратегії сортування
    public interface ISortStrategy
    {
        void Sort(int[] array);
    }

    // Реалізація 1: бульбашкове сортування
    public class BubbleSort : ISortStrategy
    {
        public void Sort(int[] array)
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - i - 1; j++)
                {
                    if (array[j] > array[j + 1])
                    {
                        int temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
            Console.WriteLine("Використано BubbleSort");
        }
    }

    // Реалізація 2: сортування через Array.Sort
    public class QuickSort : ISortStrategy
    {
        public void Sort(int[] array)
        {
            Array.Sort(array);
            Console.WriteLine("Використано QuickSort");
        }
    }

    // Контекст для використання стратегії
    public class SortContext
    {
        private ISortStrategy _strategy;

        // Встановлюємо стратегію через рефлексію по назві класу
        public void SetStrategy(string strategyName)
        {
            var strategyType = Assembly.GetExecutingAssembly()
                                       .GetTypes()
                                       .FirstOrDefault(t => t.Name.Equals(strategyName, StringComparison.OrdinalIgnoreCase)
                                                            && typeof(ISortStrategy).IsAssignableFrom(t));

            if (strategyType == null)
                throw new InvalidOperationException($"Стратегія {strategyName} не знайдена.");

            _strategy = (ISortStrategy)Activator.CreateInstance(strategyType);
        }

        public void Sort(int[] array)
        {
            if (_strategy == null)
                throw new InvalidOperationException("Стратегія не встановлена.");
            _strategy.Sort(array);
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            int[] numbers = { 5, 3, 8, 1, 2 };

            var context = new SortContext();

            // Користувач вибирає стратегію по назві
            Console.Write("Введіть стратегію сортування (BubbleSort/QuickSort): ");
            string input = Console.ReadLine();

            context.SetStrategy(input);
            context.Sort(numbers);

            Console.WriteLine("Результат: " + string.Join(", ", numbers));
        }
    }
}
