// ============================================================================
//  Помилка компіляції для пункту 2 (порядок catch).
//  Щоб побачити її — розкоментуйте рядок нижче і зберіть проєкт.
// ============================================================================
// #define ERR_CATCH_ORDER

using System.Diagnostics;

namespace ExceptionsBasics;

// Кожен метод — окремий маленький приклад для одного пункту меню.
static class Demos
{
    // 1. Найпростіший try / catch -------------------------------------------------------
    public static void BasicTryCatch()
    {
        Ui.Title("1. try / catch — перехоплення винятку");
        Ui.Info("Виняток — це ОБ'ЄКТ, що описує помилку. Коли він виникає, виконання");
        Ui.Info("стрибає з місця помилки прямо в підходящий catch.");

        try
        {
            Ui.Info("  [try]   перед діленням 10 / 0");
            int result = Divide(10, 0);                          // тут виникає виняток
            Ui.Info($"  [try]   результат = {result}   <- цей рядок НЕ виконається");
        }
        catch (DivideByZeroException ex)
        {
            Ui.Bad("  [catch] виняток перехоплено. Що лежить в об'єкті:");
            Ui.Info($"            Тип:        {ex.GetType().FullName}");
            Ui.Info($"            Message:    {ex.Message}");
            Ui.Info($"            Де сталося: метод {ex.TargetSite?.Name}");
        }

        Ui.Ok("  Після catch програма спокійно працює далі.");
    }

    static int Divide(int a, int b) => a / b;

    // 2. Кілька catch -------------------------------------------------------------------
    public static void MultipleCatch()
    {
        Ui.Title("2. Кілька catch: перший підходящий виграє");
        Ui.Info("int.Parse може кинути різні винятки — реагуємо на кожен по-своєму.");

        ParseAndReport("42");
        ParseAndReport("abc");
        ParseAndReport("99999999999");

        Ui.Note("Правило: від КОНКРЕТНОГО до ЗАГАЛЬНОГО. catch (Exception) — завжди останній.");

#if ERR_CATCH_ORDER
        try { Divide(1, 0); }
        catch (Exception) { }
        catch (DivideByZeroException) { }                        // CS0160
#endif
        Ui.CompileError("ERR_CATCH_ORDER",
            "catch (Exception) { }  catch (DivideByZeroException) { }",
            "CS0160: A previous catch clause already catches all exceptions of this or of a super type ('Exception')");
    }

    static void ParseAndReport(string text)
    {
        try
        {
            int number = int.Parse(text);
            Ui.Ok($"  \"{text}\" -> {number}");
        }
        catch (FormatException)                                  // це не число
        {
            Ui.Bad($"  \"{text}\" -> FormatException: це не число");
        }
        catch (OverflowException)                                // число не влізло в int
        {
            Ui.Bad($"  \"{text}\" -> OverflowException: число завелике для int");
        }
        catch (Exception ex)                                     // "сітка безпеки" для всього іншого
        {
            Ui.Bad($"  \"{text}\" -> несподіваний виняток {ex.GetType().Name}");
        }
    }

    // 3. finally ------------------------------------------------------------------------
    public static void FinallyAlways()
    {
        Ui.Title("3. finally виконується ЗАВЖДИ");

        for (int mode = 0; mode <= 2; mode++)
        {
            string what = mode == 0 ? "без помилок"
                        : mode == 1 ? "з винятком"
                        : "з return усередині try";
            Ui.Info($"Сценарій {mode}: {what}");
            int returned = TryFinally(mode);
            Ui.Info($"  метод повернув {returned}");
            Ui.Info("");
        }

        Ui.Note("Використання: закрити файл / з'єднання / розблокувати ресурс — незалежно від результату.");
    }

    static int TryFinally(int mode)
    {
        try
        {
            Ui.Info("  [try]     початок");
            if (mode == 1)
                throw new InvalidOperationException("помилка в try");
            if (mode == 2)
                return 2;                                        // вихід з методу... але finally все одно спрацює
            Ui.Info("  [try]     кінець без помилок");
            return 0;
        }
        catch (InvalidOperationException ex)
        {
            Ui.Bad($"  [catch]   {ex.Message}");
            return 1;
        }
        finally
        {
            Ui.Note("  [finally] виконався!");
        }
    }

    // 4. Розкрутка стека ----------------------------------------------------------------
    public static void StackUnwinding()
    {
        Ui.Title("4. Розкрутка стека: виняток летить ВГОРУ по викликах");
        Ui.Info("Ланцюжок: StackUnwinding -> Level1 -> Level2 -> Level3.");
        Ui.Info("Помилка виникає в Level3, catch є тільки в Level1.");
        Ui.Info("");

        Level1();
    }

    static void Level1()
    {
        Ui.Info("Level1: вхід");
        try
        {
            Level2();
            Ui.Info("Level1: після Level2   <- НЕ виконається");
        }
        catch (InvalidOperationException ex)
        {
            Ui.Bad($"Level1: [catch] спіймав виняток: {ex.Message}");
            Ui.Info("Стек викликів (зверху — де кинуто, знизу — метод, що спіймав; вище стек не показується):");
            PrintTrace(ex);
        }
    }

    static void Level2()
    {
        Ui.Info("  Level2: вхід");
        try
        {
            Level3();
            Ui.Info("  Level2: після Level3   <- НЕ виконається");
        }
        finally
        {
            Ui.Note("  Level2: [finally] виконався під час розкрутки, хоча catch тут немає");
        }
    }

    static void Level3()
    {
        Ui.Info("    Level3: вхід");
        throw new InvalidOperationException("щось пішло не так у Level3");
    }

    // Виводить лише назви методів зі StackTrace (без шляхів до файлів)
    static void PrintTrace(Exception ex)
    {
        string[] lines = ex.StackTrace!.Split('\n');
        foreach (string line in lines)
        {
            string text = line.Trim();
            int cut = text.IndexOf(" in ");
            if (cut > 0)
                text = text[..cut];
            Ui.Info("    " + text);
        }
    }

    // 5. throw та throw ex --------------------------------------------------------------
    public static void ThrowVsThrowEx()
    {
        Ui.Title("5. throw; проти throw ex; (повторне кидання)");
        Ui.Info("Обидва способи кидають виняток далі, але по-різному впливають на StackTrace.");

        Ui.Info("");
        Ui.Ok("Варіант 'throw;' — стек зберігається повністю:");
        try { RethrowWithThrow(); }
        catch (InvalidOperationException ex) { PrintTrace(ex); }

        Ui.Info("");
        Ui.Bad("Варіант 'throw ex;' — стек починається заново, справжнє місце помилки ВТРАЧЕНО:");
        try { RethrowWithThrowEx(); }
        catch (InvalidOperationException ex) { PrintTrace(ex); }

        Ui.Info("");
        Ui.Note("Правило: щоб перекинути виняток далі — пишіть просто 'throw;'.");
    }

    static void ThrowingMethod()
    {
        throw new InvalidOperationException("помилка в ThrowingMethod");
    }

    static void RethrowWithThrow()
    {
        try
        {
            ThrowingMethod();
        }
        catch (InvalidOperationException)
        {
            Ui.Info("  [catch] щось залогували...");
            throw;                                               // ПРАВИЛЬНО
        }
    }

    static void RethrowWithThrowEx()
    {
        try
        {
            ThrowingMethod();
        }
        catch (InvalidOperationException ex)
        {
            Ui.Info("  [catch] щось залогували...");
#pragma warning disable CA2200   // навмисно: показуємо, чому так робити не можна
            throw ex;                                            // НЕПРАВИЛЬНО
#pragma warning restore CA2200
        }
    }

    // 6. using = try/finally ------------------------------------------------------------
    public static void UsingStatement()
    {
        Ui.Title("6. using: скорочений запис try / finally");
        Ui.Info("Ресурс має бути закритий, навіть якщо посередині впав виняток.");
        Ui.Info("");

        try
        {
            using (Resource file = new Resource("data.txt"))
            {
                Ui.Info("    [робота] читаємо дані...");
                throw new IOException("збій під час читання");
            }
        }
        catch (IOException ex)
        {
            Ui.Bad($"  [catch] {ex.Message}");
        }

        Ui.Info("");
        Ui.Note("Зверніть увагу: Dispose() спрацював ДО catch. using — це те саме, що:");
        Ui.Info("    Resource r = new Resource(...);");
        Ui.Info("    try { ... }");
        Ui.Info("    finally { r.Dispose(); }");
    }

    // 7. Порожній catch — антипатерн ------------------------------------------------------
    public static void EmptyCatch()
    {
        Ui.Title("7. Антипатерн: порожній catch ховає помилки");
        string[] input = { "10", "abc", "30" };

        Ui.Bad("Погано: catch без реакції.");
        int silentSum = 0;
        foreach (string s in input)
        {
            try { silentSum += int.Parse(s); }
            catch { }                                            // помилку проковтнули — ніхто не дізнається
        }
        Ui.Info($"  Сума = {silentSum}. Правильна? Ми навіть не знаємо, що \"abc\" було пропущено.");

        Ui.Info("");
        Ui.Ok("Краще: ловимо КОНКРЕТНИЙ виняток і явно повідомляємо про проблему.");
        int sum = 0;
        int skipped = 0;
        foreach (string s in input)
        {
            try { sum += int.Parse(s); }
            catch (FormatException)
            {
                skipped++;
                Ui.Note($"  пропущено некоректне значення \"{s}\"");
            }
        }
        Ui.Info($"  Сума = {sum}, пропущено значень: {skipped}");
    }

    // 8. Виняток чи TryParse ------------------------------------------------------------
    public static void ExceptionOrTryParse()
    {
        Ui.Title("8. Виняток — для виняткових ситуацій, а не для звичайної логіки");
        Ui.Info("Введення користувача, що не є числом — це НОРМАЛЬНО, а не виняток.");
        Ui.Info("Для такого є TryParse: повертає bool, без винятку.");

        const int Count = 2000;

        Stopwatch timer = Stopwatch.StartNew();
        for (int i = 0; i < Count; i++)
        {
            try { _ = int.Parse("abc"); }
            catch (FormatException) { }
        }
        timer.Stop();
        Ui.Bad($"  int.Parse + catch : {timer.Elapsed.TotalMilliseconds:F1} мс на {Count} невдалих спроб");

        timer.Restart();
        for (int i = 0; i < Count; i++)
        {
            _ = int.TryParse("abc", out _);
        }
        timer.Stop();
        Ui.Ok($"  int.TryParse      : {timer.Elapsed.TotalMilliseconds:F1} мс на {Count} невдалих спроб");

        Ui.Note("Кидання винятку на порядки повільніше за звичайну перевірку.");
        Ui.Info("");
        Ui.Info("Правильне використання TryParse:");
        if (int.TryParse("123", out int value))
            Ui.Ok($"  \"123\" -> {value}");
        if (!int.TryParse("xyz", out _))
            Ui.Info("  \"xyz\" -> не число (без жодних винятків)");
    }
}
