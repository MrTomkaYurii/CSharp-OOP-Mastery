using DependencyInjection;


Console.OutputEncoding = System.Text.Encoding.UTF8;

// Ініціалізуємо контейнер залежностей
var container = new Container();

            // Реєструємо залежності
            container.Register<IService, Service>();
            // Друкуємо список реєстрацій
            container.PrintRegistrations();

// Отримуємо клієнта з усіма залежностями
var client = container.GetInstance<Client>();
// Запускаємо процес
client.RunProcess();