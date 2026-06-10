using ExceptionsCustom.Exceptions;
using ExceptionsCustom.Models;
using ExceptionsCustom.Services;

namespace ExceptionsCustom;

// Кожен метод — окремий маленький приклад для одного пункту меню.
static class Demos
{
    // 1. Винятки, які кидає CLR ----------------------------------------------------------
    public static void SystemThrown()
    {
        Ui.Title("1. Винятки, які кидає САМЕ СЕРЕДОВИЩЕ ВИКОНАННЯ (CLR)");
        Ui.Info("Це помилки в логіці програми. Їх не ловлять, а ВИПРАВЛЯЮТЬ у коді.");
        Ui.Info("");

        try
        {
            string? text = GetNothing();
            Ui.Info($"  {text!.Length}");                        // звертання до null
        }
        catch (NullReferenceException ex) { Ui.Bad($"  NullReferenceException     : {ex.Message}"); }

        try
        {
            int[] numbers = new int[3];
            int index = 5;
            Ui.Info($"  {numbers[index]}");                      // індекс за межами масиву
        }
        catch (IndexOutOfRangeException ex) { Ui.Bad($"  IndexOutOfRangeException   : {ex.Message}"); }

        try
        {
            object box = "текст";
            int number = (int)box;                               // у коробці рядок, а не int
            Ui.Info($"  {number}");
        }
        catch (InvalidCastException ex) { Ui.Bad($"  InvalidCastException       : {ex.Message}"); }

        try
        {
            int zero = 0;
            Ui.Info($"  {10 / zero}");                           // ділення на нуль
        }
        catch (DivideByZeroException ex) { Ui.Bad($"  DivideByZeroException      : {ex.Message}"); }

        Ui.Info("");
        Ui.Note("Ці типи належить кидати ТІЛЬКИ середовищу. Самому писати");
        Ui.Note("throw new NullReferenceException() / IndexOutOfRangeException() / InvalidCastException() не варто.");
        Ui.Note("Є ще OutOfMemoryException і StackOverflowException — з ними зазвичай уже нічого не зробити.");
    }

    static string? GetNothing() => null;

    // 2. Вбудовані винятки, які кидаємо МИ -----------------------------------------------
    public static void SystemGuards()
    {
        Ui.Title("2. Вбудовані винятки, які кидає ПРОГРАМІСТ (перевірка вхідних даних)");
        Ui.Info("Метод має захистити себе від неправильного використання і сказати ЧОМУ саме.");
        Ui.Info("");

        Guard1();
        Guard2();
        Guard3();
        Guard4();

        Ui.Info("");
        Ui.Note("Який вбудований виняток обрати:");
        Ui.Info("  ArgumentNullException         — аргумент null, а він обов'язковий");
        Ui.Info("  ArgumentOutOfRangeException   — значення поза допустимим діапазоном");
        Ui.Info("  ArgumentException             — аргумент некоректний з іншої причини");
        Ui.Info("  InvalidOperationException     — об'єкт зараз у такому стані, що операція неможлива");
        Ui.Info("  NotSupportedException         — операція взагалі не підтримується цим типом");
        Ui.Info("  NotImplementedException       — 'ще не написано' (тимчасова заглушка)");
    }

    static void Guard1()
    {
        try { new BankAccount(null!, 100); }
        catch (ArgumentNullException ex)
        {
            Ui.Bad($"  ArgumentNullException       : параметр '{ex.ParamName}' не може бути null");
        }
    }

    static void Guard2()
    {
        try { new BankAccount("   ", 100); }
        catch (ArgumentException ex)
        {
            Ui.Bad($"  ArgumentException           : параметр '{ex.ParamName}' — {ShortMessage(ex)}");
        }
    }

    static void Guard3()
    {
        try { new BankAccount("UA001", -50); }
        catch (ArgumentOutOfRangeException ex)
        {
            Ui.Bad($"  ArgumentOutOfRangeException : параметр '{ex.ParamName}', отримано {ex.ActualValue}");
        }
    }

    static void Guard4()
    {
        BankAccount account = new BankAccount("UA001", 100);
        account.Close();
        try { account.Deposit(10); }
        catch (InvalidOperationException ex)
        {
            Ui.Bad($"  InvalidOperationException   : {ex.Message}");
        }
    }

    // Повідомлення без службового суфікса " (Parameter '...')", який додає сама .NET
    static string ShortMessage(Exception ex)
    {
        string message = ex.Message;
        int cut = message.IndexOf(" (Parameter");
        return cut > 0 ? message[..cut] : message;
    }

    // 3. Ієрархія винятків ---------------------------------------------------------------
    public static void Hierarchy()
    {
        Ui.Title("3. Ієрархія: усі винятки — нащадки System.Exception");
        Ui.Info("Кожен рядок: тип -> його батько -> ... -> Object.");
        Ui.Info("catch (БАТЬКО) ловить і всіх його нащадків.");
        Ui.Info("");

        Ui.Note("Вбудовані:");
        PrintChain(typeof(DivideByZeroException));
        PrintChain(typeof(ArgumentNullException));
        PrintChain(typeof(ArgumentOutOfRangeException));
        PrintChain(typeof(FormatException));
        PrintChain(typeof(KeyNotFoundException));

        Ui.Info("");
        Ui.Note("Власні (з цього проєкту):");
        PrintChain(typeof(InsufficientFundsException));
        PrintChain(typeof(AccountFrozenException));
        PrintChain(typeof(TransferFailedException));

        Ui.Info("");
        Ui.Info("Наприклад, catch (ArgumentException) зловить і ArgumentNullException,");
        Ui.Info("і ArgumentOutOfRangeException; а catch (BankException) — усі три власні.");
    }

    static void PrintChain(Type type)
    {
        string line = "";
        for (Type? current = type; current != null; current = current.BaseType)
        {
            line += current.Name;
            if (current.BaseType != null)
                line += " -> ";
        }
        Ui.Info("  " + line);
    }

    // 4. Власний виняток -----------------------------------------------------------------
    public static void CustomException()
    {
        Ui.Title("4. Власний виняток: навіщо він потрібен");
        BankAccount account = new BankAccount("UA001", 100m);

        Ui.Bad("Погано: кидаємо голий Exception — дані доводиться розбирати з тексту.");
        try { BadWithdraw(account, 250m); }
        catch (Exception ex)
        {
            Ui.Info($"  Message = \"{ex.Message}\"");
            Ui.Info("  Скільки не вистачає? З рядка не дістати без розбору тексту.");
        }

        Ui.Info("");
        Ui.Ok("Добре: власний InsufficientFundsException несе дані у властивостях.");
        try { account.Withdraw(250m); }
        catch (InsufficientFundsException ex)
        {
            Ui.Info($"  Message   = \"{ex.Message}\"");
            Ui.Info($"  Balance   = {ex.Balance}");
            Ui.Info($"  Requested = {ex.Requested}");
            Ui.Ok($"  Shortfall = {ex.Shortfall}   <- готове число, парсити нічого не треба");
        }
    }

    // Приклад "як не треба": загальний Exception без даних
    static void BadWithdraw(BankAccount account, decimal amount)
    {
        if (amount > account.Balance)
            throw new Exception("Недостатньо коштів");
    }

    // 5. Ловимо по ієрархії --------------------------------------------------------------
    public static void CatchByHierarchy()
    {
        Ui.Title("5. Catch за ієрархією: від конкретного до загального");
        Ui.Info("Один блок try з різними catch — реакція залежить від типу винятку.");
        Ui.Info("");

        for (int scenario = 1; scenario <= 4; scenario++)
            RunScenario(scenario);
    }

    static void RunScenario(int scenario)
    {
        BankAccount account = new BankAccount("UA001", 100m);

        try
        {
            switch (scenario)
            {
                case 1:
                    Ui.Info("Сценарій 1: зняти 500 при балансі 100");
                    account.Withdraw(500m);
                    break;
                case 2:
                    Ui.Info("Сценарій 2: покласти гроші на заблокований рахунок");
                    account.IsFrozen = true;
                    account.Deposit(10m);
                    break;
                case 3:
                    Ui.Info("Сценарій 3: закрити рахунок і спробувати зняти");
                    account.Close();
                    account.Withdraw(10m);
                    break;
                case 4:
                    Ui.Info("Сценарій 4: покласти від'ємну суму");
                    account.Deposit(-5m);
                    break;
            }
        }
        catch (InsufficientFundsException ex)                    // найконкретніший власний
        {
            Ui.Bad($"  [InsufficientFunds] не вистачає {ex.Shortfall}");
        }
        catch (BankException ex)                                 // будь-яка інша банківська помилка
        {
            Ui.Bad($"  [BankException] {ex.Message}   (справжній тип: {ex.GetType().Name})");
        }
        catch (ArgumentException ex)                             // вбудований: неправильний аргумент
        {
            Ui.Bad($"  [ArgumentException] помилка виклику: {ex.GetType().Name}, параметр '{ex.ParamName}'");
        }
        catch (Exception ex)                                     // усе решта
        {
            Ui.Bad($"  [Exception] {ex.GetType().Name}: {ex.Message}");
        }
    }

    // 6. InnerException ------------------------------------------------------------------
    public static void InnerExceptionDemo()
    {
        Ui.Title("6. InnerException: виняток усередині винятку");
        Ui.Info("Верхній рівень пояснює ЩО сталося, InnerException зберігає ЧОМУ.");

        // Сценарій А: у відправника не вистачає грошей
        Ui.Info("");
        Ui.Note("Сценарій А: у відправника мало грошей");
        BankAccount a1 = new BankAccount("UA001", 100m);
        BankAccount b1 = new BankAccount("UA002", 0m);
        TransferAndReport(a1, b1, 500m);

        // Сценарій Б: одержувач заблокований — гроші мають повернутись
        Ui.Info("");
        Ui.Note("Сценарій Б: рахунок одержувача заблоковано (є відкат)");
        BankAccount a2 = new BankAccount("UA003", 100m);
        BankAccount b2 = new BankAccount("UA004", 0m) { IsFrozen = true };
        Ui.Info($"  До:    {a2.Number} = {a2.Balance},  {b2.Number} = {b2.Balance}");
        TransferAndReport(a2, b2, 40m);
        Ui.Ok($"  Після: {a2.Number} = {a2.Balance},  {b2.Number} = {b2.Balance}   <- гроші повернулися");
    }

    static void TransferAndReport(BankAccount from, BankAccount to, decimal amount)
    {
        try
        {
            Bank.Transfer(from, to, amount);
            Ui.Ok("  Переказ виконано");
        }
        catch (TransferFailedException ex)
        {
            Ui.Bad($"  Верхній рівень: {ex.GetType().Name}: {ex.Message}");

            // Проходимо по ланцюжку InnerException до першопричини
            Exception? inner = ex.InnerException;
            int level = 1;
            while (inner != null)
            {
                Ui.Info($"  {new string(' ', level * 2)}└─ причина: {inner.GetType().Name}: {inner.Message}");
                inner = inner.InnerException;
                level++;
            }
        }
    }
}
