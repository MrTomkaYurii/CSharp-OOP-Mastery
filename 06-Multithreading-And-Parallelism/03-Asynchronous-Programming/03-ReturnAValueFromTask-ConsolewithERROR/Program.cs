using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AsynchronousProgramming
{
    /// <summary>
    /// =========================================================================================
    /// ОБРОБКА ПОМИЛОК В АСИНХРОННОМУ I/O: Відловлювання винятків у Task<T>
    /// =========================================================================================
    /// Демонструє виникнення помилок при асинхронному мережевому запиті.
    /// Показує, як класичний блок try/catch перехоплює виняток, розгорнутий оператором 'await'.
    /// </summary>
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Main розпочато...");
            Console.Write("Введіть ім'я для запиту з помилкою: ");
            string name = Console.ReadLine() ?? "Студент";

            await SomeMethodAsync(name);

            Console.WriteLine("Main успішно завершено.");
        }

        public static async Task SomeMethodAsync(string name)
        {
            Console.WriteLine("Спроба виконати асинхронний запит із потенційною помилкою...");

            try
            {
                var greetingsMessage = await GreetingsWithError(name);
                Console.WriteLine($"\n{greetingsMessage}");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"\n[Очікувана помилка мережі]: Запит завершився з кодом збою. Деталі: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[Загальний виняток]: {ex.Message}");
            }
        }

        public static async Task<string> GreetingsWithError(string name)
        {
            using (HttpClient client = new HttpClient())
            {
                // Запит до ендпоінту, що гарантовано повертає статус помилки 404
                client.BaseAddress = new Uri("https://httpbin.org/");
                client.Timeout = TimeSpan.FromSeconds(5);

                HttpResponseMessage response = await client.GetAsync("status/404");
                response.EnsureSuccessStatusCode(); // Викидає HttpRequestException для 4xx/5xx статусів
                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
