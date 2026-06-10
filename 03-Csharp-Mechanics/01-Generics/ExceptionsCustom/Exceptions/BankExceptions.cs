namespace ExceptionsCustom.Exceptions;

// ============================================================================
//  ВЛАСНІ ВИНЯТКИ. Ієрархія:
//
//      Exception
//        └── BankException                      (спільна база: "щось не так у банку")
//              ├── InsufficientFundsException   (мало грошей; несе Balance і Requested)
//              ├── AccountFrozenException       (рахунок заблоковано; несе номер рахунку)
//              └── TransferFailedException      (переказ не вдався; несе InnerException)
//
//  Правила для власного винятку:
//   1. Ім'я закінчується на "Exception".
//   2. Успадковуємо від Exception (не від ApplicationException — це застаріла порада).
//   3. Додаємо властивості з ДАНИМИ про помилку, щоб не розбирати текст повідомлення.
//   4. Робимо три стандартні конструктори: (), (message), (message, inner).
// ============================================================================

// Базовий виняток предметної області. Дозволяє зловити ВСІ банківські помилки одним catch.
public class BankException : Exception
{
    public BankException() { }

    public BankException(string message) : base(message) { }

    public BankException(string message, Exception inner) : base(message, inner) { }
}

// Виняток з додатковими даними: баланс і скільки запросили
public class InsufficientFundsException : BankException
{
    public decimal Balance { get; }          // скільки є
    public decimal Requested { get; }        // скільки хотіли зняти
    public decimal Shortfall => Requested - Balance;   // скількох не вистачає

    public InsufficientFundsException(decimal balance, decimal requested)
        : base($"Недостатньо коштів: на рахунку {balance}, потрібно {requested}.")
    {
        Balance = balance;
        Requested = requested;
    }
}

// Виняток з номером рахунку
public class AccountFrozenException : BankException
{
    public string AccountNumber { get; }

    public AccountFrozenException(string accountNumber)
        : base($"Рахунок {accountNumber} заблоковано.")
    {
        AccountNumber = accountNumber;
    }
}

// Виняток-"обгортка": описує помилку високого рівня і зберігає першопричину в InnerException
public class TransferFailedException : BankException
{
    public string From { get; }
    public string To { get; }
    public decimal Amount { get; }

    public TransferFailedException(string from, string to, decimal amount, Exception inner)
        : base($"Переказ {amount} з {from} на {to} не вдався.", inner)
    {
        From = from;
        To = to;
        Amount = amount;
    }
}
