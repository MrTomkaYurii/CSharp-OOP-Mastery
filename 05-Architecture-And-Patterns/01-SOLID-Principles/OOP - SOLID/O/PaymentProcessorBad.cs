using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___SOLID.O
{
    // ПОГАНИЙ ПРИКЛАД: Порушення OCP (Open/Closed Principle)
    // Будь-яке додавання нового платіжного методу змушує змінювати наявний код класу
    public class PaymentProcessorBad
    {
        // Кожен раз при додаванні нового способу оплати треба змінювати цей метод
        public void ProcessPayment(string paymentType, decimal amount)
        {
            if (paymentType == "CreditCard")
            {
                Console.WriteLine($"[ОПЛАТА] Обробка оплати кредитною карткою на суму {amount} грн");
                Console.WriteLine("[ОПЛАТА] Підключення до банківського API...");
                Console.WriteLine("[ОПЛАТА] Перевірка CVV коду...");
                Console.WriteLine("[ОПЛАТА] Списання коштів...");
                Console.WriteLine("[ОПЛАТА] [ОК] Оплата кредитною карткою успішна");
            }
            else if (paymentType == "PayPal")
            {
                Console.WriteLine($"[ОПЛАТА] Обробка оплати через PayPal на суму {amount} грн");
                Console.WriteLine("[ОПЛАТА] Перенаправлення на PayPal...");
                Console.WriteLine("[ОПЛАТА] Авторизація користувача...");
                Console.WriteLine("[ОПЛАТА] Підтвердження платежу...");
                Console.WriteLine("[ОПЛАТА] [ОК] Оплата через PayPal успішна");
            }
            else if (paymentType == "Cash")
            {
                Console.WriteLine($"[ОПЛАТА] Обробка готівкової оплати на суму {amount} грн");
                Console.WriteLine("[ОПЛАТА] Генерація рахунку для оплати...");
                Console.WriteLine("[ОПЛАТА] Очікування підтвердження оплати...");
                Console.WriteLine("[ОПЛАТА] [ОК] Готівкова оплата підтверджена");
            }
            // Щоб додати криптовалюту, довелося модифікувати цей метод (порушення OCP)
            else if (paymentType == "Crypto")
            {
                Console.WriteLine($"[ОПЛАТА] Обробка оплати криптовалютою на суму {amount} грн");
                Console.WriteLine("[ОПЛАТА] Генерація Bitcoin адреси...");
                Console.WriteLine("[ОПЛАТА] Очікування підтвердження транзакції...");
                Console.WriteLine("[ОПЛАТА] [ОК] Криптоплатіж підтверджено в блокчейні");
            }
            else
            {
                throw new NotSupportedException($"Тип оплати '{paymentType}' не підтримується");
            }
        }

        // При додаванні нового типу треба також додавати нові методи
        public decimal CalculateFee(string paymentType, decimal amount)
        {
            if (paymentType == "CreditCard")
                return amount * 0.025m; // 2.5% комісія
            else if (paymentType == "PayPal")
                return amount * 0.035m; // 3.5% комісія
            else if (paymentType == "Cash")
                return 0; // Без комісії
            else if (paymentType == "Crypto")
                return amount * 0.01m; // 1% комісія
            else
                return 0;
        }
    }
}
