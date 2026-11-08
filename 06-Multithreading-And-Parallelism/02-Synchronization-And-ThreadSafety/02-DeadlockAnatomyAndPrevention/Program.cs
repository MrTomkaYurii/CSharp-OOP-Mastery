using System.Threading;

namespace DeadlockAnatomyAndPrevention;

/// <summary>
/// Банківський рахунок із власним об'єктом блокування та унікальним числовим Id.
/// </summary>
public sealed class BankAccount
{
    public int Id { get; }
    public string Owner { get; }
    public decimal Balance { get; private set; }
    public object SyncRoot { get; } = new();

    public BankAccount(int id, string owner, decimal initialBalance)
    {
        Id = id;
        Owner = owner;
        Balance = initialBalance;
    }

    public void Withdraw(decimal amount) => Balance -= amount;
    public void Deposit(decimal amount) => Balance += amount;
}

/// <summary>
/// Демонструє причину виникнення взаємного блокування (Deadlock AB-BA)
/// та канонічний спосіб його усунення через впорядкування ресурсів (Lock Ordering).
/// </summary>
public static class BankTransferService
{
    /// <summary>
    /// НЕБЕЗПЕЧНИЙ МЕТОД (Bad): Якщо Потік 1 викликає Transfer(A, B), а Потік 2 одночасно
    /// викликає Transfer(B, A), Потік 1 захопить A і чекатиме на B, а Потік 2 захопить B і чекатиме на A!
    /// Щоб програма не зависла назавжди під час демонстрації, ми використовуємо Monitor.TryEnter для виявлення клінчу.
    /// </summary>
    public static bool TryTransferDeadlockProne(BankAccount from, BankAccount to, decimal amount)
    {
        lock (from.SyncRoot)
        {
            // Штучна затримка, щоб обидва потоки гарантовано захопили свій перший рахунок
            Thread.Sleep(40);

            // Спроба захопити другий рахунок: якщо інший потік уже тримає його — це класичний Deadlock!
            if (!Monitor.TryEnter(to.SyncRoot, TimeSpan.FromMilliseconds(150)))
            {
                Console.WriteLine($"  [DEADLOCK ВИЯВЛЕНО!] Потік #{Environment.CurrentManagedThreadId} тримає Рахунок #{from.Id}, але не може захопити Рахунок #{to.Id}!");
                return false;
            }

            try
            {
                from.Withdraw(amount);
                to.Deposit(amount);
                return true;
            }
            finally
            {
                Monitor.Exit(to.SyncRoot);
            }
        }
    }

    /// <summary>
    /// ЕТАЛОННИЙ МЕТОД (Good): Впорядкування блокувань (Lock Ordering) за зростанням Id рахунку.
    /// Незалежно від того, хто кому переказує кошти (A -> B чи B -> A), обидва потоки
    /// ЗАВЖДИ блокують спочатку рахунок із меншим Id, а потім із більшим Id. Цикл очікування неможливий!
    /// </summary>
    public static void TransferSafeOrdered(BankAccount from, BankAccount to, decimal amount)
    {
        BankAccount firstLock = from.Id < to.Id ? from : to;
        BankAccount secondLock = from.Id < to.Id ? to : from;

        lock (firstLock.SyncRoot)
        {
            Thread.Sleep(20);
            lock (secondLock.SyncRoot)
            {
                from.Withdraw(amount);
                to.Deposit(amount);
                Console.WriteLine($"  [УСПІШНИЙ ПЕРЕКАЗ] {from.Owner} (#{from.Id}) -> {to.Owner} (#{to.Id}): {amount:N0} грн");
            }
        }
    }
}

internal class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("==========================================================================");
        Console.WriteLine(" 02. АНАТОМІЯ ВЗАЄМНОГО БЛОКУВАННЯ (DEADLOCK) ТА МЕТОДИ ЗАПОБІГАННЯ");
        Console.WriteLine(" Чому виникає перехресний клінч AB-BA і як його усуває Lock Ordering");
        Console.WriteLine("==========================================================================\n");

        var accountA = new BankAccount(id: 101, owner: "Олександр", initialBalance: 5_000m);
        var accountB = new BankAccount(id: 202, owner: "Вікторія", initialBalance: 5_000m);

        // 1. Відтворення класичного перехресного блокування (AB-BA Deadlock)
        Console.WriteLine("--- 1. Антипатерн: Перехресне захоплення ресурсів (A -> B одночасно з B -> A) ---");
        var t1 = new Thread(() => BankTransferService.TryTransferDeadlockProne(accountA, accountB, 500m));
        var t2 = new Thread(() => BankTransferService.TryTransferDeadlockProne(accountB, accountA, 300m));

        t1.Start();
        t2.Start();
        t1.Join();
        t2.Join();
        Console.WriteLine("Пояснення: Без таймауту Monitor.TryEnter обидва потоки зависли б назавжди!\n");

        // 2. Вирішення через суворий порядок захоплення блокувань (Lock Ordering)
        Console.WriteLine("--- 2. Еталон: Впорядкування блокувань за зростанням Account.Id (Lock Ordering) ---");
        var safeT1 = new Thread(() => BankTransferService.TransferSafeOrdered(accountA, accountB, 500m));
        var safeT2 = new Thread(() => BankTransferService.TransferSafeOrdered(accountB, accountA, 300m));

        safeT1.Start();
        safeT2.Start();
        safeT1.Join();
        safeT2.Join();

        Console.WriteLine($"\n  Підсумковий баланс {accountA.Owner} (#101): {accountA.Balance:N0} грн (Очікувано: 4 800 грн)");
        Console.WriteLine($"  Підсумковий баланс {accountB.Owner} (#202): {accountB.Balance:N0} грн (Очікувано: 5 200 грн)");
    }
}
