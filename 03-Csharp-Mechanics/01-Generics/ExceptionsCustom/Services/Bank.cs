using ExceptionsCustom.Exceptions;
using ExceptionsCustom.Models;

namespace ExceptionsCustom.Services;

public static class Bank
{
    // Переказ між рахунками. Усі помилки нижчого рівня (BankException) обгортаються
    // у TransferFailedException, а першопричина лишається в InnerException.
    public static void Transfer(BankAccount from, BankAccount to, decimal amount)
    {
        try
        {
            from.Withdraw(amount);                   // 1) зняли з першого

            try
            {
                to.Deposit(amount);                  // 2) поклали на другий
            }
            catch
            {
                from.Deposit(amount);                // відкат: повертаємо гроші, щоб вони не зникли
                throw;                               // і кидаємо ТОЙ САМИЙ виняток далі
            }
        }
        catch (BankException ex)
        {
            throw new TransferFailedException(from.Number, to.Number, amount, ex);
        }
    }
}
