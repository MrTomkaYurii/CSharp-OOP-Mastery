ExceptionsCustom — вбудовані та власні винятки
================================================

Консольний проєкт із меню. Предметна область — банківський рахунок.
Делегати та лямбди не використовуються.

Запуск:  dotnet run --project ExceptionsCustom

Структура:
  Exceptions\BankExceptions.cs   власні винятки (ієрархія нижче)
  Models\BankAccount.cs          рахунок: кидає і вбудовані, і власні винятки
  Services\Bank.cs               переказ з відкатом та обгорткою TransferFailedException
  Demos.cs                       приклади для пунктів меню

Ієрархія власних винятків:

  Exception
    └── BankException                     спільна база всіх банківських помилок
          ├── InsufficientFundsException  Balance, Requested, Shortfall
          ├── AccountFrozenException      AccountNumber
          └── TransferFailedException     From, To, Amount + InnerException

Пункти меню:
  Вбудовані ("зарезервовані" в .NET)
    1. Що кидає сам CLR        NullReference, IndexOutOfRange, InvalidCast, DivideByZero
                               — це помилки в коді, їх виправляють, а не ловлять
    2. Що кидаємо ми           ArgumentNull / ArgumentOutOfRange / Argument /
                               InvalidOperation — таблиця "який обрати"
    3. Ієрархія                ланцюжки типів до System.Exception (вбудовані й власні)
  Власні
    4. Навіщо власний виняток  голий Exception проти InsufficientFundsException з даними
    5. Catch за ієрархією      конкретний власний -> BankException -> ArgumentException -> Exception
    6. InnerException          обгортка TransferFailedException + відкат при збої

Правила власного винятку (коротко):
  - ім'я закінчується на "Exception";
  - успадковується від Exception (не від ApplicationException);
  - несе дані у властивостях, а не лише в тексті повідомлення;
  - має три конструктори: (), (message), (message, inner).

Спершу варто пройти проєкт ExceptionsBasics (try / catch / finally).
