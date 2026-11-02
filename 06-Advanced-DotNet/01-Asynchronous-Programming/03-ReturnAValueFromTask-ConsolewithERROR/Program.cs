using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AsynchronousProgramming
{
    /// <summary>
    /// =========================================================================================
    /// ОБРОБКА ПОМИЛОК В АСИНХРОННОМУ I/O: Відловлювання винятків у Task<T>
    /// =========================================================================================
    /// Демонструє виникнення помилок при асинхронному мережевому запиті (невірний ендпоінт,
    /// недоступний сервер, 404/500 статуси через EnsureSuccessStatusCode).
    /// Показує, як блок try/catch коректно перехоплює виняток, розгорнутий оператором 'await'.
    /// </summary>
    internal class Program
    {
        // =====================================================================================
        // БЛОК 1: Точка входу в програму
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
        // БЛОК 2: Асинхронний метод із безпечною обробкою винятків (try/catch)
        // Коли виникає HttpRequestException, оператор await розгортає його,
        // дозволяючи зловити помилку класичним блоком catch.
        // =====================================================================================
        public async static void SomeMethod(string name)
        {
            Console.WriteLine("Some Method Started......");

            try
            {
                // Спроба виконати асинхронний запит до неіснуючого маршруту api/greetings2/...
                var greetingsMessage = await Greetings(name);
                Console.WriteLine($"\n{greetingsMessage}");
            }
            catch (HttpRequestException ex)
            {
                // Безпечне перехоплення винятку мережевого рівня
                Console.WriteLine($"\n[Помилка мережі]: Запит не вдався. Деталі: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Загальний перехоплювач непередбачених збоїв
                Console.WriteLine($"\n[Непередбачена помилка]: {ex.Message}");
            }

            Console.WriteLine("Some Method End");
        }

        // =====================================================================================
        // БЛОК 3: Мережевий метод із перевіркою валідності HTTP-статусу
        // Метод звертається до невірного маршруту ("api/greetings2/{name}").
        // EnsureSuccessStatusCode() гарантує викидання HttpRequestException при HTTP 404.
        // =====================================================================================
        public static async Task<string> Greetings(string name)
        {
            string message = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7118/");

                // Запит до невірного ендпоінту (викликає 404 Not Found)
                HttpResponseMessage response = await client.GetAsync($"api/greetings2/{name}");
                
                // Якщо статус не 2xx (Success), генерується HttpRequestException
                response.EnsureSuccessStatusCode();

                message = await response.Content.ReadAsStringAsync();
            }
            return message;
        }
    }
}
