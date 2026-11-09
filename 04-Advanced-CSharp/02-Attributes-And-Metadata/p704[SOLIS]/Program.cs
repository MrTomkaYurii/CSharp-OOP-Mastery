using System;
using System.Runtime.CompilerServices;

/// <summary>
/// =========================================================================================
/// СИСТЕМНІ АТРИБУТИ: Caller Info Attributes (Деніел Соліс, с. 704)
/// =========================================================================================
/// Демонструє приклад зі сторінки 704 книги Daniel Solis "Illustrated C#".
/// Робота атрибутів [CallerFilePath], [CallerLineNumber] та [CallerMemberName].
/// 
/// ТЕОРЕТИЧНІ АСПЕКТИ:
/// 1. Ця група атрибутів із простору імен System.Runtime.CompilerServices використовується
///    для автоматичного трасування, логування та реалізації інтерфейсу INotifyPropertyChanged.
/// 2. Механізм підстановки:
///    - Значення підставляються КОМПІЛЯТОРОМ Roslyn безпосередньо у виклик (call-site) під час збірки!
///    - Рантайм не використовує важку рефлексію або аналіз стеку (StackFrame), тому накладні витрати дорівнюють 0.
/// 3. Обов'язкова умова:
///    Параметри з цими атрибутами обов'язково повинні бути необов'язковими (мати дефолтні значення).
/// 4. C# 10 також додав [CallerArgumentExpression], що захоплює точний вираз коду, переданий в інший аргумент.
/// </summary>
public static class Program
{
    public static void MyTrace(string message,
                               [CallerFilePath] string fileName = "",
                               [CallerLineNumber] int lineNumber = 0,
                               [CallerMemberName] string callingMember = "")
    {
        Console.WriteLine($"File: {fileName}");
        Console.WriteLine($"Line: {lineNumber}");
        Console.WriteLine($"Called From: {callingMember}");
        Console.WriteLine($"Message: {message}");
    }

    public static void Main()
    {
        // Компілятор автоматично підставить ім'я поточного файлу, номер рядка та ім'я методу "Main":
        MyTrace("Simple message");
    }
}
