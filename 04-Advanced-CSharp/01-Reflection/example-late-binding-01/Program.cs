using System;
using System.Linq;
using System.Reflection;

// Інтерфейс плагіна
public interface IPlugin
{
    void Run();
}

// Плагін 1
public class HelloPlugin : IPlugin
{
    public void Run()
    {
        Console.WriteLine("Привіт від HelloPlugin!");
    }
}

// Плагін 2
public class ByePlugin : IPlugin
{
    public void Run()
    {
        Console.WriteLine("Бувай від ByePlugin!");
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("Введіть назву плагіну для виконання (HelloPlugin / ByePlugin):");
        string pluginName = Console.ReadLine();

        // Знаходимо клас плагіну через рефлексію
        Type pluginType = Assembly.GetExecutingAssembly()
                                  .GetTypes()
                                  .FirstOrDefault(t => t.Name.Equals(pluginName, StringComparison.OrdinalIgnoreCase)
                                                       && typeof(IPlugin).IsAssignableFrom(t));

        if (pluginType == null)
        {
            Console.WriteLine("Плагін не знайдено.");
            return;
        }

        // Створюємо екземпляр через late binding
        IPlugin pluginInstance = (IPlugin)Activator.CreateInstance(pluginType);

        // Якщо клас HelloPlugin — виконуємо метод
        if (pluginType == typeof(HelloPlugin))
        {
            pluginInstance.Run();
        }
        else
        {
            // Для інших класів попередження, але викликати можна
            Console.WriteLine("⚠️ Увага: виконання методу цього плагіну може бути катастрофічним!");
            pluginInstance.Run();
        }
    }
}

/*У цьому прикладі ми створюємо об'єкт obj, але ми не знаємо його тип на етапі компіляції. 
    Потім ми визначаємо метод і викликаємо його за допомогою рефлексії. 
    Це дозволяє викликати методи та отримувати доступ до членів об'єкта, навіть коли 
    типи визначені під час виконання програми.

    Late binding корисний в ситуаціях, коли типи визначені динамічно, наприклад, при роботі з 
    плагінами або зовнішніми збірками. Однак використання late binding може призвести до 
    збільшення ризику помилок на етапі виконання та зниження продуктивності, тому його 
    варто використовувати обережно.*/