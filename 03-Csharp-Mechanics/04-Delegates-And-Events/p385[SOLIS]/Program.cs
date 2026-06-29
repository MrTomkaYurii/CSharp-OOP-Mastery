using System;

// ============================================================================
// ПРИКЛАД: p385[SOLIS] - Підписка (+ =) та відписка (- =) від подій
// ============================================================================
// Тема: Управління списком підписників події, використання стандартного нетипізованого
//       EventHandler, оператор відписки '-=' та важливість очищення ресурсів.
// ============================================================================

// 1. КЛАС-ВИДАВЕЦЬ (PUBLISHER):
class Publisher
{
    // Якщо подія не потребує передачі особливих даних, використовується
    // стандартний нетипізований делегат EventHandler.
    // Його сигнатура: void EventHandler(object? sender, EventArgs e);
    public event EventHandler SimpleEvent;

    public void RaiseTheEvent()
    {
        // За стандартом, якщо додаткових даних немає, замість null рекомендується
        // передавати EventArgs.Empty, хоча в навчальному прикладі передається null.
        if (SimpleEvent != null)
            SimpleEvent(this, EventArgs.Empty);
    }
}

// 2. КЛАС-ПІДПИСНИК (SUBSCRIBER):
class Subscriber
{
    public void MethodA(object o, EventArgs e)
    {
        Console.WriteLine("Method A() is called");
    }

    public void MethodB(object o, EventArgs e)
    {
        Console.WriteLine("Method B() is called");
    }
}

class Program
{
    static void Main()
    {
        Publisher p = new Publisher();
        Subscriber s = new Subscriber();

        // 1. ПІДПИСКА НА ПОДІЮ (РЕЄСТРАЦІЯ ОБРОБНИКІВ):
        // Додаємо MethodA і MethodB до списку підписників за допомогою '+='
        p.SimpleEvent += s.MethodA;
        p.SimpleEvent += s.MethodB;

        Console.WriteLine("--- Перший виклик події (підписано обидва методи) ---");
        // Будуть викликані по черзі MethodA та MethodB
        p.RaiseTheEvent();

        // 2. ВІДПИСКА ВІД ПОДІЇ (ЗНЯТТЯ З РЕЄСТРАЦІЇ):
        // За допомогою оператора '-=' видаляємо MethodB зі списку викликів.
        // Тепер подія вже не знає про існування MethodB.
        Console.WriteLine("\r\n--- Відписка MethodB через '-=' ---");
        p.SimpleEvent -= s.MethodB;

        Console.WriteLine("--- Другий виклик події (MethodB вже відписаний) ---");
        // Буде викликано тільки MethodA
        p.RaiseTheEvent();

        // ЧОМУ ВІДПИСКА (-=) ДУЖЕ ВАЖЛИВА У РЕАЛЬНОМУ ЖИТТІ?
        // Коли об'єкт підписується на подію довгоживучого об'єкта (наприклад, вікно UI або сервіс),
        // видавець зберігає сильне посилання (strong reference) на підписника.
        // Збирач сміття (Garbage Collector) НЕ зможе видалити підписника з пам'яті,
        // доки видавець живий, що призводить до витоку пам'яті (Memory Leak).
        // Завжди відписуйтеся від подій, коли об'єкт-підписник більше не потрібен!
    }
}
