using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AsynchronousProgramming
{
    /// <summary>
    /// =========================================================================================
    /// КЛІЄНТСЬКА АСИНХРОННІСТЬ: Отримання значення з Task<T> через HttpClient
    /// =========================================================================================
    /// Демонструє споживання зовнішнього REST API асинхронним методом із поверненням Task<string>.
    /// Ключове слово 'await' розгортає Task<T> і витягує чисте значення типу T (string).
    /// </summary>
    internal class Program
    {
        // =====================================================================================
        // БЛОК 1: Точка входу в програму (Консольний клієнт)
        // Зчитує ім'я користувача та передає в асинхронний ланцюжок викликів.
        // =====================================================================================
        public static void Main(string[] args)
        {
            Console.WriteLine("Main Method Started......");
            Console.WriteLine("Enter the Name: ");
            string name = Console.ReadLine() ?? string.Empty;

            SomeMethod(name);

            Console.WriteLine("Main Method End");
            Console.ReadKey();
        }

        // =====================================================================================
        // БЛОК 2: Метод-споживач асинхронного результату
        // Очікує завершення Greetings(name) через 'await' і виводить результат на консоль.
        // =====================================================================================
        public async static void SomeMethod(string name)
        {
            Console.WriteLine("Some Method Started......");

            // 'await' розпаковує Task<string> у звичайний string без блокування потоку
            var greetingsMessage = await Greetings(name);

            Console.WriteLine($"\n{greetingsMessage}");
            Console.WriteLine("Some Method End");
        }

        // =====================================================================================
        // БЛОК 3: Мережевий I/O запит через HttpClient
        // Виконує асинхронний GET-запит та асинхронне читання тіла відповіді.
        // =====================================================================================
        public static async Task<string> Greetings(string name)
        {
            string message = string.Empty;

            // Навчальний приклад локального HttpClient
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7118/");

                // Неблокуючий мережевий виклик до Web API
                HttpResponseMessage response = await client.GetAsync($"api/greetings/{name}");

                // Неблокуюче читання потоку даних тіла відповіді
                message = await response.Content.ReadAsStringAsync();
            }

            return message;
        }
    }
}
