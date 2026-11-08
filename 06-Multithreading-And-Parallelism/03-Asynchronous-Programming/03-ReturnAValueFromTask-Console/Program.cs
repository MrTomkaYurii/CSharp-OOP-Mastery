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
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Головний метод Main розпочато...");
            Console.Write("Введіть ім'я: ");
            string name = Console.ReadLine() ?? "Студент";

            // Виклик та очікування асинхронного методу через await (без блокування потоку)
            await SomeMethodAsync(name);

            Console.WriteLine("Головний метод Main успішно завершено.");
        }

        public static async Task SomeMethodAsync(string name)
        {
            Console.WriteLine("Виклик SomeMethodAsync розпочато...");

            // 'await' розпаковує Task<string> у звичайний string без блокування потоку
            var greetingsMessage = await Greetings(name);

            Console.WriteLine($"\n{greetingsMessage}");
            Console.WriteLine("SomeMethodAsync завершено.\n");
        }

        public static async Task<string> Greetings(string name)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://httpbin.org/");
                client.Timeout = TimeSpan.FromSeconds(5);

                try
                {
                    HttpResponseMessage response = await client.GetAsync($"base64/{Uri.EscapeDataString(name)}");
                    response.EnsureSuccessStatusCode();
                    string message = await response.Content.ReadAsStringAsync();
                    return $"Привіт, {name}! Відповідь сервера: {message}";
                }
                catch (Exception)
                {
                    // Fallback для демонстрації офлайн:
                    await Task.Delay(200);
                    return $"Привіт, {name}! (Локальна асинхронна генерація вітання)";
                }
            }
        }
    }
}
