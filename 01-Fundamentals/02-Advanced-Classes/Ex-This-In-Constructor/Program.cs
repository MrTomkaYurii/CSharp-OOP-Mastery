namespace Ex_This_In_Constructor
{
    using System;
    using System.Text;

    /// <summary>
    /// Клас, який демонструє різні способи використання оператора this у конструкторах
    /// </summary>
    public class BankAccount
    {
        // Приватні поля класу
        private string accountNumber;
        private string ownerName;
        private decimal balance;
        private string bankName;
        private DateTime createdDate;

        /// <summary>
        /// Основний конструктор з усіма параметрами
        /// Демонструє використання this для розрізнення параметрів і полів
        /// </summary>
        public BankAccount(string accountNumber, string ownerName, decimal balance, string bankName)
        {
            // ОСОБЛИВІСТЬ 1: Використання this для розрізнення параметрів конструктора і полів класу
            // Коли назва параметра збігається з назвою поля, this вказує на поле класу
            this.accountNumber = accountNumber;  // this.accountNumber - поле класу, accountNumber - параметр
            this.ownerName = ownerName;         // Без this компілятор не зрозуміє, що ми звертаємось до поля
            this.balance = balance;
            this.bankName = bankName;
            this.createdDate = DateTime.Now;

            Console.WriteLine("Викликаний основний конструктор з 4 параметрами");
        }

        /// <summary>
        /// Конструктор з 3 параметрами, використовує this() для виклику основного конструктора
        /// </summary>
        public BankAccount(string accountNumber, string ownerName, decimal balance)
            : this(accountNumber, ownerName, balance, "Приват Банк") // ОСОБЛИВІСТЬ 2: this() викликає інший конструктор
        {
            // Цей конструктор викликає основний конструктор з параметром bankName = "Приват Банк"
            // this() ЗАВЖДИ виконується ПЕРЕД тілом поточного конструктора
            Console.WriteLine("Викликаний конструктор з 3 параметрами, банк встановлено за замовчуванням");

            // Тут можна додати специфічну логіку для цього конструктора
            // Наприклад, логування або додаткові перевірки
        }

        /// <summary>
        /// Конструктор з 2 параметрами, демонструює ланцюжок викликів конструкторів
        /// </summary>
        public BankAccount(string accountNumber, string ownerName)
            : this(accountNumber, ownerName, 0.00m) // ОСОБЛИВІСТЬ 3: Ланцюжок викликів this()
        {
            // Цей конструктор викликає конструктор з 3 параметрами,
            // який у свою чергу викликає основний конструктор з 4 параметрами
            // Порядок виконання: основний конструктор → 3-параметрний → поточний
            Console.WriteLine("Викликаний конструктор з 2 параметрами, баланс встановлено 0");
        }

        /// <summary>
        /// Конструктор копіювання, демонструє передачу this як параметра
        /// </summary>
        public BankAccount(BankAccount other)
            : this(other.accountNumber + "_COPY", other.ownerName, other.balance, other.bankName)
        {
            Console.WriteLine("Створено копію рахунку");
        }

        /// <summary>
        /// Метод для демонстрації повернення this з методу
        /// </summary>
        public BankAccount Deposit(decimal amount)
        {
            if (amount > 0)
            {
                this.balance += amount; // this необов'язковий, але підкреслює роботу з полем класу
                Console.WriteLine($"Поповнено на {amount:C}. Новий баланс: {this.balance:C}");
            }

            // ОСОБЛИВІСТЬ 4: Повернення this дозволяє створювати ланцюжки методів (method chaining)
            return this;
        }

        /// <summary>
        /// Ще один метод, що повертає this для демонстрації method chaining
        /// </summary>
        public BankAccount Withdraw(decimal amount)
        {
            if (amount > 0 && this.balance >= amount)
            {
                this.balance -= amount;
                Console.WriteLine($"Знято {amount:C}. Залишок: {this.balance:C}");
            }
            else
            {
                Console.WriteLine("Недостатньо коштів або некоректна сума");
            }

            return this; // Повертаємо this для method chaining
        }

        /// <summary>
        /// Метод, що демонструює передачу this як параметра іншому методу
        /// </summary>
        public void TransferTo(BankAccount targetAccount, decimal amount)
        {
            if (this.balance >= amount && amount > 0)
            {
                this.balance -= amount;
                targetAccount.balance += amount;

                // ОСОБЛИВІСТЬ 5: Передача this як параметра
                LogTransaction(this, targetAccount, amount);
            }
        }

        /// <summary>
        /// Статичний метод для логування транзакції
        /// </summary>
        private static void LogTransaction(BankAccount from, BankAccount to, decimal amount)
        {
            Console.WriteLine($"Переказ {amount:C} з рахунку {from.accountNumber} на {to.accountNumber}");
        }

        /// <summary>
        /// Приватний конструктор для внутрішнього використання
        /// </summary>
        private BankAccount() : this("TEMP_000", "Тимчасовий", 0, "Невідомий банк")
        {
            // ОСОБЛИВІСТЬ 6: Приватні конструктори також можуть використовувати this()
            Console.WriteLine("Викликаний приватний конструктор");
        }

        /// <summary>
        /// Фабричний метод, що використовує приватний конструктор
        /// </summary>
        public static BankAccount CreateTemporary()
        {
            return new BankAccount(); // Викликає приватний конструктор
        }

        /// <summary>
        /// Демонстрація equals з використанням this
        /// </summary>
        public override bool Equals(object? obj)
        {
            if (obj is not BankAccount other)
                return false;

            // ОСОБЛИВІСТЬ 7: this у порівнянні об'єктів
            // this представляє поточний екземпляр класу
            return this.accountNumber == other.accountNumber;
        }

        /// <summary>
        /// GetHashCode з використанням this
        /// </summary>
        public override int GetHashCode()
        {
            return this.accountNumber?.GetHashCode() ?? 0;
        }

        /// <summary>
        /// ToString для виведення інформації про рахунок
        /// </summary>
        public override string ToString()
        {
            return $"Рахунок: {this.accountNumber}, Власник: {this.ownerName}, " +
                   $"Баланс: {this.balance:C}, Банк: {this.bankName}, Створено: {this.createdDate:d}";
        }

        // Властивості тільки для читання для доступу до полів
        public string AccountNumber => this.accountNumber;
        public string OwnerName => this.ownerName;
        public decimal Balance => this.balance;
    }

    /// <summary>
    /// Демонстраційний клас для тестування функціональності
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            // Налаштування української мови для консолі
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            Console.WriteLine("=== ДЕМОНСТРАЦІЯ РОБОТИ З ОПЕРАТОРОМ THIS У КОНСТРУКТОРАХ ===\n");

            // 1. Виклик різних конструкторів
            Console.WriteLine("1. Створення рахунків різними конструкторами:");

            var account1 = new BankAccount("001", "Іван Петренко", 1000.00m, "ПриватБанк");
            Console.WriteLine(account1 + "\n");

            var account2 = new BankAccount("002", "Марія Коваленко", 500.00m);
            Console.WriteLine(account2 + "\n");

            var account3 = new BankAccount("003", "Олександр Шевченко");
            Console.WriteLine(account3 + "\n");

            // 2. Конструктор копіювання
            Console.WriteLine("2. Копіювання рахунку:");
            var accountCopy = new BankAccount(account1);
            Console.WriteLine(accountCopy + "\n");

            // 3. Фабричний метод з приватним конструктором
            Console.WriteLine("3. Створення тимчасового рахунку:");
            var tempAccount = BankAccount.CreateTemporary();
            Console.WriteLine(tempAccount + "\n");

            // 4. Method chaining (ланцюжки методів) завдяки поверненню this
            Console.WriteLine("4. Демонстрація method chaining:");
            account1.Deposit(200).Withdraw(50).Deposit(100);
            Console.WriteLine(account1 + "\n");

            // 5. Переказ між рахунками (передача this як параметра)
            Console.WriteLine("5. Переказ між рахунками:");
            account1.TransferTo(account2, 300);
            Console.WriteLine($"Рахунок 1 після переказу: {account1.Balance:C}");
            Console.WriteLine($"Рахунок 2 після переказу: {account2.Balance:C}\n");

            // 6. Порівняння об'єктів з використанням this
            Console.WriteLine("6. Порівняння рахунків:");
            Console.WriteLine($"account1 == accountCopy: {account1.Equals(accountCopy)}");

            Console.WriteLine("\n=== КІНЕЦЬ ДЕМОНСТРАЦІЇ ===");
        }
    }

    /*
    КЛЮЧОВІ ОСОБЛИВОСТІ РОБОТИ З this У КОНСТРУКТОРАХ:

    1. РОЗРІЗНЕННЯ ПАРАМЕТРІВ І ПОЛІВ:
       - Коли назви параметрів конструктора збігаються з назвами полів класу
       - this.поле = параметр; - this вказує на поле класу

    2. ВИКЛИК ІНШИХ КОНСТРУКТОРІВ:
       - : this(параметри) - викликає інший конструктор ТОГО Ж класу
       - Виконується ДО тіла поточного конструктора
       - Дозволяє уникнути дублювання коду

    3. ЛАНЦЮЖОК КОНСТРУКТОРІВ:
       - Можна створювати ланцюжки викликів: конструктор1 → конструктор2 → конструктор3
       - Порядок виконання: останній у ланцюжку → ... → перший

    4. ПОВЕРНЕННЯ this З МЕТОДІВ:
       - Дозволяє створювати method chaining: obj.Method1().Method2().Method3()
       - Корисно для fluent API

    5. ПЕРЕДАЧА this ЯК ПАРАМЕТРА:
       - this представляє посилання на поточний об'єкт
       - Можна передавати в інші методи для обробки

    6. this У ПРИВАТНИХ КОНСТРУКТОРАХ:
       - Приватні конструктори також можуть використовувати this()
       - Корисно для фабричних методів і singleton патернів

    7. this У ПОРІВНЯННЯХ:
       - this представляє поточний екземпляр при порівнянні з іншими об'єктами
       - Використовується в Equals, GetHashCode тощо

    ВАЖЛИВО ПАМ'ЯТАТИ:
    - this() завжди виконується ПЕРЕД тілом конструктора
    - this не можна використовувати в статичних методах (немає поточного об'єкта)
    - this необов'язковий у більшості випадків, але покращує читабельність коду
    - Неможливо викликати this() з тіла конструктора - тільки в списку ініціалізації
    - Ланцюжок : this(...) не можна замкнути на самого себе (циклічний виклик -
      помилка компіляції CS0768)

    ЩО МОЖНА ДОДАТИ ДО ПРИКЛАДУ:
    - Валідацію у полях/параметрах основного конструктора (кинути виняток при
      порожньому accountNumber чи від'ємному balance).
    - Первинний конструктор класу (C# 12): public class BankAccount(string accountNumber, ...).
    - Реалізацію IEquatable<BankAccount> замість override Equals(object).
    - readonly для полів, які не змінюються після створення (accountNumber, createdDate).
    */
}
