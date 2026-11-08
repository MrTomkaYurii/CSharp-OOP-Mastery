using System;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step2_AbstractFactory_IoC;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step3_DependencyInjection;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step4_IoCContainer.DotNetContainer;
using OOP___SOLID.DIP_IoC_DI_Evolution.Step4_IoCContainer.MiniContainer;

namespace OOP___SOLID.DIP_IoC_DI_Evolution
{
    // Інтерактивна команда меню: Повний еволюційний практикум зв'язку
    // DIP (Принцип) -> IoC (Парадигма / Abstract Factory) -> DI (Шаблон) -> IoC Container (Інструмент).
    public class DipEvolutionCommand : IMenuCommand
    {
        public string Title => "DIP - ЕВОЛЮЦІЯ: DIP -> IoC (Abstract Factory) -> DI -> IoC Container";

        public void Execute()
        {
            while (true)
            {
                if (!Console.IsOutputRedirected) { try { Console.Clear(); } catch { } }
                Console.WriteLine("╔══════════════════════════════════════════════════════════════════════════╗");
                Console.WriteLine("║      ЕВОЛЮЦІЙНИЙ ПРАКТИКУМ: ВІД ПРИНЦИПУ ДО СУЧАСНОГО DI-КОНТЕЙНЕРА      ║");
                Console.WriteLine("║            DIP  --->  IoC  --->  Abstract Factory  --->  DI  --->  IoC  ║");
                Console.WriteLine("╚══════════════════════════════════════════════════════════════════════════╝\n");

                Console.WriteLine("Оберіть крок еволюції для вивчення:");
                Console.WriteLine("1. [DIP]       Крок 1. Принцип інверсії залежностей (Контракти замість деталей)");
                Console.WriteLine("2. [IoC]       Крок 2. Інверсія контролю через Абстрактну Фабрику (GoF Pattern)");
                Console.WriteLine("3. [DI]        Крок 3. Впровадження залежностей (Constructor, Method, Property DI)");
                Console.WriteLine("4. [Mini-IoC]  Крок 4.1. Власний IoC-контейнер 'під капотом' (Рефлексія з нуля)");
                Console.WriteLine("5. [Lifetimes] Крок 4.2. Життєві цикли .NET: Transient vs Scoped vs Singleton");
                Console.WriteLine("6. [.NET DI]   Крок 4.3. Промисловий Microsoft.Extensions.DependencyInjection");
                Console.WriteLine("7. [ALL]       Запустити повний ланцюжок по черзі (1 -> 6)");
                Console.WriteLine("0. [Назад]     Повернутися до головного меню SOLID\n");

                Console.Write("Ваш вибір: ");
                string? input = Console.ReadLine();

                if (input == "0") break;

                if (!Console.IsOutputRedirected) { try { Console.Clear(); } catch { } }

                switch (input)
                {
                    case "1":
                        DipPrincipleDemo.Run();
                        break;
                    case "2":
                        FactoryIoCDemo.Run();
                        break;
                    case "3":
                        PureDiDemo.Run();
                        break;
                    case "4":
                        MiniContainerDemo.Run();
                        break;
                    case "5":
                        LifetimesDemo.Run();
                        break;
                    case "6":
                        DotNetContainerDemo.Run();
                        break;
                    case "7":
                        RunFullEvolution();
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                        break;
                }

                Console.WriteLine("\n" + new string('-', 70));
                Console.WriteLine("Натисніть будь-яку клавішу для повернення до меню еволюції...");
                Console.ReadKey();
            }
        }

        private static void RunFullEvolution()
        {
            Console.WriteLine("==========================================================================");
            Console.WriteLine("          ЗАПУСК ПОВНОГО ЕВОЛЮЦІЙНОГО ЛАНЦЮЖКА (КРОКИ 1 - 4.3)            ");
            Console.WriteLine("==========================================================================\n");

            DipPrincipleDemo.Run();
            Console.WriteLine("\n" + new string('=', 70) + "\n");

            FactoryIoCDemo.Run();
            Console.WriteLine("\n" + new string('=', 70) + "\n");

            PureDiDemo.Run();
            Console.WriteLine("\n" + new string('=', 70) + "\n");

            MiniContainerDemo.Run();
            Console.WriteLine("\n" + new string('=', 70) + "\n");

            LifetimesDemo.Run();
            Console.WriteLine("\n" + new string('=', 70) + "\n");

            DotNetContainerDemo.Run();
        }
    }
}
