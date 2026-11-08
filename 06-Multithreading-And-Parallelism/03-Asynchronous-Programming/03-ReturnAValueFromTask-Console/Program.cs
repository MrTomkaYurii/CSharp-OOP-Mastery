using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsynchronousProgramming
{
    /// <summary>
    /// =========================================================================================
    /// КЛІЄНТСЬКА АСИНХРОННІСТЬ: Отримання значення з Task<T> через ключове слово await
    /// =========================================================================================
    /// Демонструє, як асинхронний метод повертає значення Task<string>.
    /// Ключове слово 'await' розгортає Task<T> і витягує чисте значення типу T (string)
    /// без блокування потоку, на відміну від блокуючих викликів .Result або .Wait().
    /// </summary>
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("======================================================================");
            Console.WriteLine(" ОТРИМАННЯ ЗНАЧЕННЯ З Task<T>: Розгортання результату через await");
            Console.WriteLine("======================================================================\n");

            Console.WriteLine($"[Головний потік Main #{Thread.CurrentThread.ManagedThreadId}] Метод розпочато.");
            Console.Write("Введіть ім'я студента для генерації персонального сертифіката: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                name = "Олександр Коваленко";
            }

            // Виклик та неблокуюче очікування асинхронного методу через await
            await ProcessCertificateAsync(name);

            Console.WriteLine($"\n[Головний потік Main #{Thread.CurrentThread.ManagedThreadId}] Метод успішно завершено.");
        }

        public static async Task ProcessCertificateAsync(string studentName)
        {
            Console.WriteLine($"\n[Потік #{Thread.CurrentThread.ManagedThreadId}] Виклик ProcessCertificateAsync розпочато...");

            // 'await' розпаковує Task<string> у звичайний string без заморожування викликаючого потоку
            string certificateMessage = await GenerateCertificateAsync(studentName);

            Console.WriteLine($"[Потік #{Thread.CurrentThread.ManagedThreadId}] Отримано результат після await:");
            Console.WriteLine($"--------------------------------------------------");
            Console.WriteLine(certificateMessage);
            Console.WriteLine($"--------------------------------------------------");
            Console.WriteLine($"[Потік #{Thread.CurrentThread.ManagedThreadId}] ProcessCertificateAsync завершено.\n");
        }

        // =====================================================================================
        // Метод повертає Task<string>: імітує асинхронну перевірку реєстру та генерацію сертифіката
        // =====================================================================================
        public static async Task<string> GenerateCertificateAsync(string studentName)
        {
            Console.WriteLine($"  [Потік #{Thread.CurrentThread.ManagedThreadId}] Початок асинхронної генерації сертифіката...");

            // Неблокуюча асинхронна пауза: імітація звернення до бази даних чи дискового сховища (300 мс)
            // Потік звільняється і повертається в пул потоків!
            await Task.Delay(300);

            string certificateId = $"CERT-{Guid.NewGuid().ToString()[..8].ToUpper()}";
            string date = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

            return $"СЕРТИФІКАТ ПРО УСПІШНЕ ПРОХОДЖЕННЯ КУРСУ C# OOP MASTERY\n" +
                   $"Студент:     {studentName}\n" +
                   $"Серійний №:  {certificateId}\n" +
                   $"Дата видачі: {date}\n" +
                   $"Статус:      Верифіковано в електронному реєстрі";
        }
    }
}
