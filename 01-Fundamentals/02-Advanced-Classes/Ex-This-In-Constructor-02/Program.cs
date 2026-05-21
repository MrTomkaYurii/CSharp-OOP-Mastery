namespace Ex_This_In_Constructor_02
{
    using System;
    using System.Text;

    /// <summary>
    /// Простий клас Студент для демонстрації роботи з оператором this
    /// </summary>
    public class Student
    {
        // Поля класу
        private string name;
        private int age;
        private string group;

        /// <summary>
        /// ОСНОВНИЙ конструктор - тут вся робота з ініціалізації
        /// </summary>
        public Student(string name, int age, string group)
        {
            Console.WriteLine("ВИКЛИКАВСЯ ОСНОВНИЙ КОНСТРУКТОР");

            // ОСОБЛИВІСТЬ 1: this розрізняє параметри і поля класу
            this.name = name;     // this.name - це поле класу, name - це параметр
            this.age = age;       // Без this компілятор заплутається
            this.group = group;

            // Вся складна логіка тільки тут (без дублювання!)
            Console.WriteLine($"Ініціалізовано: {this.name}, {this.age} років, група {this.group}");
            Console.WriteLine("Проведено валідацію даних");
            Console.WriteLine("Записано в базу даних");
        }

        /// <summary>
        /// Конструктор без групи - НЕ ДУБЛЮЄ КОД!
        /// </summary>
        public Student(string name, int age) : this(name, age, "Не вказано")
        {
            Console.WriteLine(" ВИКЛИКАВСЯ КОНСТРУКТОР БЕЗ ГРУПИ");
            Console.WriteLine(" Встановлено групу за замовчуванням");
            // Тут немає дублювання! Основну роботу зробив інший конструктор
        }

        /// <summary>
        /// Конструктор тільки з ім'ям - теж НЕ ДУБЛЮЄ КОД!
        /// </summary>
        public Student(string name) : this(name, 18)
        {
            Console.WriteLine(" ВИКЛИКАВСЯ КОНСТРУКТОР ТІЛЬКИ З ІМ'ЯМ");
            Console.WriteLine(" Встановлено вік за замовчуванням: 18");
            // І тут немає дублювання!
        }

        /// <summary>
        /// Метод для виведення інформації
        /// </summary>
        public void ShowInfo()
        {
            // this тут необов'язковий, але показує, що працюємо з полями класу
            Console.WriteLine($"Студент: {this.name}, Вік: {this.age}, Група: {this.group}");
        }

        /// <summary>
        /// Метод, що повертає this для демонстрації method chaining
        /// </summary>
        public Student SetGroup(string newGroup)
        {
            this.group = newGroup;
            Console.WriteLine($" Змінено групу на: {this.group}");
            return this; // Повертаємо this для ланцюжка викликів
        }

        /// <summary>
        /// Ще один метод для method chaining
        /// </summary>
        public Student SetAge(int newAge)
        {
            this.age = newAge;
            Console.WriteLine($" Змінено вік на: {this.age}");
            return this; // Повертаємо this
        }
    }

    /// <summary>
    /// Демонстрація роботи
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            // Налаштування української мови для консолі
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("=== ПРОСТА ДЕМОНСТРАЦІЯ ОПЕРАТОРА this ===\n");

            // 1. Основний конструктор - вся робота тут
            Console.WriteLine("1️. СТВОРЕННЯ ЧЕРЕЗ ОСНОВНИЙ КОНСТРУКТОР:");
            var student1 = new Student("Іван", 20, "ПІ-21");
            student1.ShowInfo();
            Console.WriteLine();

            // 2. Конструктор без групи - делегує роботу основному
            Console.WriteLine("2️. СТВОРЕННЯ БЕЗ ГРУПИ (делегування):");
            Console.WriteLine("   Послідовність: Основний конструктор → Конструктор без групи");
            var student2 = new Student("Марія", 19);
            student2.ShowInfo();
            Console.WriteLine();

            // 3. Конструктор тільки з ім'ям - подвійне делегування
            Console.WriteLine("3️. СТВОРЕННЯ ТІЛЬКИ З ІМ'ЯМ (подвійне делегування):");
            Console.WriteLine("   Послідовність: Основний → Без групи → Тільки ім'я");
            var student3 = new Student("Олександр");
            student3.ShowInfo();
            Console.WriteLine();

            // 4. Method chaining завдяки поверненню this
            Console.WriteLine("4️. METHOD CHAINING (ланцюжок методів):");
            student3.SetGroup("КН-22").SetAge(21);
            student3.ShowInfo();
            Console.WriteLine();

            Console.WriteLine("=== ВИСНОВКИ ===");
            Console.WriteLine("Код ініціалізації написаний тільки ОДИН раз");
            Console.WriteLine("Кожен конструктор додає тільки свою специфічну логіку");
            Console.WriteLine("Немає дублювання коду!");
            Console.WriteLine("this дозволяє створювати зручні ланцюжки методів");
        }
    }

    /*
    ОСНОВНІ ОСОБЛИВОСТІ this У КОНСТРУКТОРАХ:

    1️. РОЗРІЗНЕННЯ ПАРАМЕТРІВ І ПОЛІВ:
       this.поле = параметр;
       Коли назви однакові, this показує, що звертаємось до поля класу

    2. ДЕЛЕГУВАННЯ РОБОТИ ІНШИМ КОНСТРУКТОРАМ:
       : this(параметри) - "зроби основну роботу за мене"
       Виконується ПЕРЕД тілом поточного конструктора

    3️. УНИКНЕННЯ ДУБЛЮВАННЯ:
       Код ініціалізації пишеться тільки в одному місці
       Інші конструктори тільки додають специфічну логіку

    4️. ЛАНЦЮЖКИ КОНСТРУКТОРІВ:
       Один → Другий → Третій
       Кожен передає роботу далі

    5️. METHOD CHAINING:
       return this; дозволяє obj.Method1().Method2()

    ❗ ВАЖЛИВО:
    - this() виконується ДО тіла конструктора
    - Не можна викликати this() з середини конструктора (лише в списку ініціалізації)
    - this в статичних методах заборонений (немає поточного об'єкта)

    ЩО МОЖНА ДОДАТИ:
    - Валідацію: кинути ArgumentException при порожньому name чи від'ємному age
      ОДИН раз в основному конструкторі — усі інші отримають перевірку безкоштовно.
    - Властивості Name/Age/Group замість прямого доступу до полів.
    - Порівняти з варіантом БЕЗ ": this(...)", де код ініціалізації дублюється
      в кожному конструкторі, — щоб побачити різницю в супроводі коду.
    */
}
