using System;
using System.Threading;

// ============================================================================
// КАСТОМНИЙ ПРИКЛАД 6: Бізнес-процес та Слабке зв'язування (Loose Coupling)
// ============================================================================
// НАВІЩО ЦЕЙ ПРИКЛАД:
// Це вершина ООП у темі подій:
// 1. Як бізнес-сервіс (обробка замовлення) відокремлюється від деталей сповіщень.
// 2. Клас OrderProcessor НЕ ЗНАЄ, хто його слухає (Email, SMS, файл аудиту).
//    Він просто робить свою роботу і генерує дві події: ProgressChanged та OrderCompleted.
// 3. Додавання нового сервісу (наприклад, Telegram чи Viber) НЕ ПОТРЕБУЄ
//    зміни жодного рядка коду всередині OrderProcessor!
// ============================================================================

namespace ex06_BusinessProcessLogic;

// ============================================================================
// [КРОК 1] КЛАС ДАНИХ ПОДІЇ (EventArgs)
// ============================================================================
public class OrderEventArgs : EventArgs
{
    public int OrderId { get; }
    public bool IsSuccessful { get; }
    public decimal TotalAmount { get; }
    public string? ErrorMessage { get; }
    public DateTime CompletionTime { get; }

    public OrderEventArgs(int orderId, bool isSuccessful, decimal totalAmount, string? errorMessage = null)
    {
        OrderId = orderId;
        IsSuccessful = isSuccessful;
        TotalAmount = totalAmount;
        ErrorMessage = errorMessage;
        CompletionTime = DateTime.Now;
    }
}

// ============================================================================
// [КРОК 2] БІЗНЕС-СЕРВІС ВИДАВЕЦЬ (Publisher): OrderProcessor
// ============================================================================
public class OrderProcessor
{
    // Подія прогресу виконання етапів (відсотки 0..100)
    public event EventHandler<int>? ProgressChanged;

    // Подія фінального результату
    public event EventHandler<OrderEventArgs>? OrderCompleted;

    public void ProcessOrder(int orderId, decimal amount, bool simulateFailure = false)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n[OrderProcessor] Початок обробки замовлення #{orderId} на суму {amount:C}...");
        Console.ResetColor();

        try
        {
            // Етап 1: Валідація замовлення
            ReportProgress(25);
            Thread.Sleep(200);

            // Етап 2: Списання коштів
            ReportProgress(50);
            Thread.Sleep(200);

            if (simulateFailure)
            {
                throw new InvalidOperationException("Банк відхилив транзакцію (недостатньо коштів)!");
            }

            // Етап 3: Резервування на складі
            ReportProgress(75);
            Thread.Sleep(200);

            // Етап 4: Замовлення спаковано
            ReportProgress(100);

            // Генеруємо подію успіху:
            OnOrderCompleted(new OrderEventArgs(orderId, isSuccessful: true, amount));
        }
        catch (Exception ex)
        {
            // Генеруємо подію помилки з описом:
            OnOrderCompleted(new OrderEventArgs(orderId, isSuccessful: false, amount, ex.Message));
        }
    }

    protected virtual void ReportProgress(int percentage)
    {
        ProgressChanged?.Invoke(this, percentage);
    }

    protected virtual void OnOrderCompleted(OrderEventArgs e)
    {
        OrderCompleted?.Invoke(this, e);
    }
}

// ============================================================================
// [КРОК 3] НЕЗАЛЕЖНІ СЕРВІСИ-ПІДПИСНИКИ (Subscribers)
// ============================================================================

// Підписник 1: Сервіс надсилання електронних листів
public class EmailNotificationService
{
    public void OnOrderCompleted(object? sender, OrderEventArgs e)
    {
        if (e.IsSuccessful)
            Console.WriteLine($"   📧 [EmailService] Лист покупцю: Замовлення #{e.OrderId} на {e.TotalAmount:C} оплачено!");
        else
            Console.WriteLine($"   📧 [EmailService] Лист покупцю: Замовлення #{e.OrderId} відхилено. Причина: {e.ErrorMessage}");
    }
}

// Підписник 2: Сервіс SMS-повідомлень
public class SmsNotificationService
{
    public void OnOrderCompleted(object? sender, OrderEventArgs e)
    {
        if (e.IsSuccessful)
            Console.WriteLine($"   📱 [SmsService] SMS клієнту: Замовлення #{e.OrderId} передано кур'єру.");
    }
}

// Підписник 3: Журнал аудиту та фінансової безпеки
public class AuditLogService
{
    public void OnOrderCompleted(object? sender, OrderEventArgs e)
    {
        string status = e.IsSuccessful ? "SUCCESS" : "FAILED";
        Console.WriteLine($"   📝 [AuditLog] [{e.CompletionTime:HH:mm:ss}] Замовлення #{e.OrderId} | Статус: {status}");
    }
}

// ============================================================================
// [КРОК 4] СЦЕНАРІЙ У MAIN (Покроковий показ для викладача)
// ============================================================================
class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Приклад 6: Бізнес-процес зі слабким зв'язуванням (Loose Coupling) ===\n");

        // --------------------------------------------------------------------
        // ЕТАП 1: Створюємо видавця та підписників
        // --------------------------------------------------------------------
        OrderProcessor processor = new();
        EmailNotificationService emailService = new();
        SmsNotificationService smsService = new();
        AuditLogService auditService = new();

        // --------------------------------------------------------------------
        // ЕТАП 2: Підписка на прогрес (через лямбду для консольного індикатора)
        // --------------------------------------------------------------------
        processor.ProgressChanged += (sender, percent) =>
        {
            Console.WriteLine($"   -> прогрес виконання: {percent}%");
        };

        // --------------------------------------------------------------------
        // ЕТАП 3: Підписка незалежних підсистем на фінал замовлення
        // --------------------------------------------------------------------
        processor.OrderCompleted += emailService.OnOrderCompleted;
        processor.OrderCompleted += smsService.OnOrderCompleted;
        processor.OrderCompleted += auditService.OnOrderCompleted;

        // --------------------------------------------------------------------
        // ЕТАП 4: Сценарій 1 — Успішна обробка замовлення
        // --------------------------------------------------------------------
        Console.WriteLine("========================================================");
        Console.WriteLine("СЦЕНАРІЙ 1: Успішна оплата та обробка замовлення");
        Console.WriteLine("========================================================");
        processor.ProcessOrder(orderId: 101, amount: 1850.00m, simulateFailure: false);

        // --------------------------------------------------------------------
        // ЕТАП 5: Сценарій 2 — Обробка з помилкою банку
        // --------------------------------------------------------------------
        Console.WriteLine("\n========================================================");
        Console.WriteLine("СЦЕНАРІЙ 2: Банк відхилив оплату (помилка в try-catch)");
        Console.WriteLine("========================================================");
        processor.ProcessOrder(orderId: 102, amount: 4500.00m, simulateFailure: true);

        // --------------------------------------------------------------------
        // ГОЛОВНИЙ ВИСНОВОК ДЛЯ СТУДЕНТІВ:
        // Клас OrderProcessor абсолютно автономний.
        // Він знає ЛИШЕ про свій бізнес-процес.
        // Це і є Loose Coupling (слабке зв'язування) — основа масштабованої архітектури!
        // --------------------------------------------------------------------
    }
}
