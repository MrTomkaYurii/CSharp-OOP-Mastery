// ============================================================================
// Тема: Звільнення ресурсів — IDisposable, фіналізатор (~Class) і using
// ----------------------------------------------------------------------------
// Пам'ять у .NET звільняє збирач сміття (GC) автоматично. Але є ресурси, ПРО
// ЯКІ GC НЕ ЗНАЄ: відкриті файли, з'єднання з БД, сокети, дескриптори ОС.
// Їх треба закривати ЯВНО і вчасно.
//
// Стандартні механізми:
//   * інтерфейс IDisposable з методом Dispose() — «звільни ресурси зараз»;
//   * оператор using — гарантовано викликає Dispose() при виході з блоку
//     (навіть якщо стався виняток);
//   * фіналізатор ~Class() — «остання надія»: GC викличе його, якщо про
//     Dispose() забули. Момент виклику НЕ визначений, тож це лише запобіжник.
//
// Канонічний «Dispose pattern» (реалізований нижче):
//   Dispose()            -> Dispose(true) + GC.SuppressFinalize(this)
//   ~Class()             -> Dispose(false)
//   Dispose(bool)        -> спільна логіка, розрізняє два сценарії
// ============================================================================

using System;

namespace Ex_IDisposable_Finalizer
{
    /// <summary>
    /// Навчальний «логер у файл». Реальний FileStream замінено імітацією,
    /// щоб зосередитись на самому патерні звільнення ресурсів.
    /// </summary>
    public sealed class FileLogger : IDisposable
    {
        private readonly string _fileName;
        private bool _disposed;          // захист від повторного звільнення

        public FileLogger(string fileName)
        {
            _fileName = fileName;
            Console.WriteLine($"  [ВІДКРИТО]  ресурс для '{_fileName}'");
        }

        public void Write(string message)
        {
            // Після звільнення користуватись об'єктом не можна
            ObjectDisposedException.ThrowIf(_disposed, this);
            Console.WriteLine($"  [ЗАПИС]     '{message}' -> {_fileName}");
        }

        // --- Реалізація Dispose pattern ------------------------------------

        // Публічний метод інтерфейсу IDisposable. Його викликає using або код вручну.
        public void Dispose()
        {
            Dispose(true);
            // Ресурси вже звільнені явно => фіналізатор не потрібен,
            // прибираємо об'єкт із черги фіналізації (це прискорює GC).
            GC.SuppressFinalize(this);
        }

        // Фіналізатор. Синтаксис ~ІмʼяКласу(). Викликається збирачем сміття,
        // якщо Dispose() так і не викликали. Не можна кидати винятки, не можна
        // звертатись до інших керованих об'єктів (вони можуть бути вже зібрані).
        ~FileLogger()
        {
            Dispose(false);
        }

        /// <param name="disposing">
        /// true  — виклик з Dispose(): можна чіпати й керовані, і некеровані ресурси;
        /// false — виклик з фіналізатора: звільняємо ЛИШЕ некеровані ресурси.
        /// </param>
        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                // Тут звільняли б вкладені керовані IDisposable-поля (StreamWriter тощо)
                Console.WriteLine($"  [ЗАКРИТО]   ресурс '{_fileName}' через Dispose()");
            }
            else
            {
                // Сюди потрапляємо лише з фіналізатора — про Dispose() забули
                Console.WriteLine($"  [ЗАКРИТО]   ресурс '{_fileName}' через ФІНАЛІЗАТОР (Dispose не викликали!)");
            }

            _disposed = true;
        }
    }

    internal static class Program
    {
        private static void Main()
        {
            Console.WriteLine("1) Класичний блок using { } — Dispose гарантовано в кінці блоку:");
            using (var log = new FileLogger("app.log"))
            {
                log.Write("Програма запущена");
                log.Write("Виконано роботу");
            } // <-- саме тут неявно викликається log.Dispose()

            Console.WriteLine("\n2) using-оголошення (C# 8) — Dispose у кінці ПОТОЧНОГО методу:");
            UsingDeclarationDemo();

            Console.WriteLine("\n3) Dispose спрацьовує навіть при винятку:");
            try
            {
                using var log = new FileLogger("crash.log");
                log.Write("Перед помилкою");
                throw new InvalidOperationException("Щось пішло не так");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"  Спіймано виняток: {ex.Message} (ресурс усе одно закрито)");
            }

            Console.WriteLine("\n4) 'Забули' Dispose — рятує фіналізатор (колись, під час збирання сміття):");
            _ = new FileLogger("leaked.log");   // посилання не зберігаємо
            GC.Collect();                       // навчально примушуємо GC
            GC.WaitForPendingFinalizers();

            Console.WriteLine("\nГотово.");
        }

        private static void UsingDeclarationDemo()
        {
            using var log = new FileLogger("declaration.log");
            log.Write("Працюємо без вкладеного блоку");
            // Dispose буде викликано автоматично при виході з UsingDeclarationDemo()
        }
    }
}

// ----------------------------------------------------------------------------
// Що можна додати:
//  * Реальний FileStream/StreamWriter замість імітації.
//  * Успадкування: базовий клас із virtual void Dispose(bool) і override у нащадку.
//  * IAsyncDisposable + 'await using' для асинхронного звільнення.
//  * SafeHandle як сучасну заміну «сирих» фіналізаторів.
//  * Показати, що подвійний виклик Dispose() безпечний (прапорець _disposed).
// ----------------------------------------------------------------------------
