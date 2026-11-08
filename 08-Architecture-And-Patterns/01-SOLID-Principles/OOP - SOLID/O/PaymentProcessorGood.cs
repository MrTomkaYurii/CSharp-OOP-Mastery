using OOP___SOLID.O.OCPGoodExample;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.O
{
    // Процесор платежів - НЕ потребує змін при додаванні нових методів оплати (OCP)
    public class PaymentProcessorGood
    {
        private readonly List<IPaymentMethod> _availablePayments;

        // Реєстрація методів за замовчуванням
        public PaymentProcessorGood()
            : this(new List<IPaymentMethod>
            {
                new CreditCardPayment(),
                new PayPalPayment(),
                new CashPayment(),
                new CryptoPayment(),
                new GooglePayPayment(),
                new ApplePayPayment()
            })
        {
        }

        // Конструктор із передачею списку методів (відкрито для розширення через конфігурацію)
        public PaymentProcessorGood(IEnumerable<IPaymentMethod> paymentMethods)
        {
            _availablePayments = new List<IPaymentMethod>(paymentMethods);
        }

        // Метод для динамічної реєстрації нових платіжних провайдерів без зміни коду класу (OCP)
        public void RegisterPaymentMethod(IPaymentMethod paymentMethod)
        {
            if (paymentMethod == null) throw new ArgumentNullException(nameof(paymentMethod));
            if (!_availablePayments.Any(p => p.Name.Equals(paymentMethod.Name, StringComparison.OrdinalIgnoreCase)))
            {
                _availablePayments.Add(paymentMethod);
            }
        }

        public void ProcessPayment(string paymentMethodName, decimal amount)
        {
            // Знаходимо потрібний метод оплати
            var paymentMethod = _availablePayments
                .FirstOrDefault(p => p.Name.Equals(paymentMethodName, StringComparison.OrdinalIgnoreCase));

            if (paymentMethod == null)
            {
                throw new NotSupportedException($"Метод оплати '{paymentMethodName}' не підтримується");
            }

            // Обробка платежу
            Console.WriteLine($"\n╔═══════════════════════════════════════════╗");
            Console.WriteLine($"║  Обробка платежу: {paymentMethod.Name,-24}║");
            Console.WriteLine($"╚═══════════════════════════════════════════╝");

            paymentMethod.Process(amount);

            var fee = paymentMethod.CalculateFee(amount);
            var total = amount + fee;

            Console.WriteLine($"\n[ПІДСУМОК] Сума: {amount} грн");
            Console.WriteLine($"[ПІДСУМОК] Комісія: {fee} грн ({(fee / amount * 100):F2}%)");
            Console.WriteLine($"[ПІДСУМОК] До сплати: {total} грн");
        }

        public void ShowAvailablePayments()
        {
            Console.WriteLine("\nДоступні методи оплати:");
            Console.WriteLine(new string('─', 60));
            foreach (var payment in _availablePayments)
            {
                var sampleFee = payment.CalculateFee(100m);
                Console.WriteLine($"  • {payment.Name,-20} (комісія: {(sampleFee / 100 * 100):F2}%)");
            }
            Console.WriteLine(new string('─', 60));
        }
    }
}
