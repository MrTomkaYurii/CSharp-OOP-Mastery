using System;
using System.Reflection;

namespace DynamicMethodExample
{
    // Клас Robot
    public class Robot
    {
        public string Name { get; set; }

        // Поле для зберігання динамічних команд
        public Action DynamicCommand;

        public Robot(string name)
        {
            Name = name;
        }

        public void ExistingCommand()
        {
            Console.WriteLine($"{Name} виконує стандартну команду.");
        }
    }

    class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 1️⃣ Створюємо об’єкт Robot динамічно
            Type robotType = typeof(Robot);
            object robot = Activator.CreateInstance(robotType, new object[] { "Robo1" });

            // 2️⃣ Викликаємо існуючу команду через рефлексію
            MethodInfo existingMethod = robotType.GetMethod("ExistingCommand");
            Console.WriteLine("Викликаємо існуючу команду:");
            existingMethod.Invoke(robot, null);

            // 3️⃣ Додаємо динамічну команду через делегат
            Action newCommand = () =>
            {
                Console.WriteLine("Виконується динамічна команда: обхід перешкод!");
            };

            FieldInfo dynamicField = robotType.GetField("DynamicCommand");
            dynamicField.SetValue(robot, newCommand);

            // 4️⃣ Викликаємо динамічну команду через поле
            Console.WriteLine("Викликаємо динамічну команду:");
            Action storedCommand = (Action)dynamicField.GetValue(robot);
            storedCommand();
        }
    }
}
