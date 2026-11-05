using System;
using System.Collections.Generic;

namespace HierarchyEncapsulationAndProtected;

/*
 * ── 02. Інкапсуляція в ієрархіях (`protected`, `internal`, `private protected`) ──
 *
 * Коли ми проектуємо базовий клас для спадкування, у нього з'являється ДВА
 * незалежні інтерфейси:
 *   1. `public` API — для зовнішніх клієнтів класу;
 *   2. `protected` API — спеціально для класів-спадкоємців.
 *
 * Правило хорошого ООП:
 *   Поля базового класу мають залишатися `private` (щоб похідний клас не міг
 *   зламати інваріант напряму), а для спадкоємців відкривають контрольовані
 *   `protected`-методи або `protected`-властивості.
 */

public abstract class BankAccount
{
    // Поле бачить ЛИШЕ сам BankAccount (навіть спадкоємці не можуть записати туди -99999 напряму!)
    private decimal _balance;

    // `private protected` — доступно спадкоємцям ЛИШЕ всередині поточної збірки (.dll/.exe)
    private protected List<string> AuditTrail { get; } = new();

    public string Owner { get; }

    // `protected set` дозволяє змінювати статус рахунку лише базовому класу та спадкоємцям
    public string Status { get; protected set; } = "Active";

    public decimal Balance => _balance;

    protected BankAccount(string owner, decimal initialBalance)
    {
        if (initialBalance < 0)
            throw new ArgumentOutOfRangeException(nameof(initialBalance));

        Owner = owner;
        _balance = initialBalance;
        RecordAudit($"Відкрито рахунок із балансом {initialBalance:C}");
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0) throw new ArgumentOutOfRangeException(nameof(amount));
        _balance += amount;
        RecordAudit($"Поповнення +{amount:C}. Баланс: {_balance:C}");
    }

    // Контрольована точка розширення для спадкоємців:
    // похідний клас визначає правило комісії та ліміту, але саме списання виконує базовий клас!
    public bool TryWithdraw(decimal amount)
    {
        if (amount <= 0) return false;

        decimal fee = CalculateWithdrawalFee(amount);
        decimal totalDeduction = amount + fee;

        if (!CanDebit(_balance, totalDeduction))
        {
            RecordAudit($"Відхилено зняття {amount:C} (комісія {fee:C})");
            return false;
        }

        _balance -= totalDeduction;
        RecordAudit($"Знято {amount:C} (комісія {fee:C}). Залишок: {_balance:C}");
        return true;
    }

    // `protected virtual` та `protected abstract` — внутрішні гачки (hooks) для підкласів
    protected virtual decimal CalculateWithdrawalFee(decimal amount) => 0m;
    protected abstract bool CanDebit(decimal currentBalance, decimal totalDeduction);

    protected void RecordAudit(string entry)
        => AuditTrail.Add($"[{DateTime.Now:HH:mm:ss}] {Owner}: {entry}");

    public void PrintAudit()
    {
        Console.WriteLine($"--- Аудит рахунку {Owner} ({Status}, Баланс: {Balance:C}) ---");
        foreach (string line in AuditTrail)
            Console.WriteLine($"  {line}");
    }
}

public sealed class SavingsAccount : BankAccount
{
    public SavingsAccount(string owner, decimal initialBalance)
        : base(owner, initialBalance)
    {
    }

    // Накопичувальний рахунок: комісія 1.5% за зняття, баланс не може піти в мінус
    protected override decimal CalculateWithdrawalFee(decimal amount)
        => Math.Round(amount * 0.015m, 2);

    protected override bool CanDebit(decimal currentBalance, decimal totalDeduction)
        => currentBalance >= totalDeduction;
}

public sealed class CreditLineAccount : BankAccount
{
    public decimal CreditLimit { get; }

    public CreditLineAccount(string owner, decimal initialBalance, decimal creditLimit)
        : base(owner, initialBalance)
    {
        CreditLimit = creditLimit;
    }

    // Кредитна лінія дозволяє йти в мінус у межах CreditLimit
    protected override bool CanDebit(decimal currentBalance, decimal totalDeduction)
    {
        bool allowed = (currentBalance - totalDeduction) >= -CreditLimit;
        if (allowed && (currentBalance - totalDeduction) < 0)
        {
            Status = "Overdraft"; // використовуємо protected set!
        }
        return allowed;
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        BankAccount savings = new SavingsAccount("Олена (Накопичувальний)", 1000m);
        savings.TryWithdraw(500m);
        savings.TryWithdraw(600m); // перевищує залишок
        savings.PrintAudit();

        Console.WriteLine();

        BankAccount credit = new CreditLineAccount("Максим (Кредитний)", 200m, creditLimit: 1000m);
        credit.TryWithdraw(800m); // іде в овердрафт до -600
        credit.PrintAudit();
    }
}
