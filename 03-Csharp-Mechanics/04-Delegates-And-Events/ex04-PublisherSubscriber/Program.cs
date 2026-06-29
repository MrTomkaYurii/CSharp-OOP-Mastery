using System;

// ============================================================================
// КАСТОМНИЙ ПРИКЛАД 4: Патерн "Видавець-Підписник" (Publisher-Subscriber)
// ============================================================================
// НАВІЩО ЦЕЙ ПРИКЛАД:
// Це найнаочніша і найближча до реального життя метафора подій — YouTube-канал!
//
// 1. ВИДАВЕЦЬ (Publisher)   — YouTube-канал, який випускає контент.
// 2. ПОДІЯ (Event)          — момент виходу нового відео.
// 3. ДАНІ ПОДІЇ (EventArgs) — назва відео, тривалість та посилання.
// 4. ПІДПИСНИКИ (Subscribers)— глядачі та мобільні сервіси, які натиснули дзвіночок (+=).
// ============================================================================

namespace ex04_PublisherSubscriber;

// ============================================================================
// [КРОК 1] КЛАС ДАНИХ ПОДІЇ (EventArgs)
// ============================================================================
// Пакет інформації, який видавець передає всім підписникам у момент події.
public class VideoEventArgs : EventArgs
{
    public string VideoTitle { get; }
    public int DurationMinutes { get; }
    public DateTime ReleaseDate { get; }

    public VideoEventArgs(string title, int durationMinutes)
    {
        VideoTitle = title;
        DurationMinutes = durationMinutes;
        ReleaseDate = DateTime.Now;
    }
}

// ============================================================================
// [КРОК 2] КЛАС-ВИДАВЕЦЬ (Publisher)
// ============================================================================
// Клас YoutubeChannel нічого не знає про конкретних глядачів!
// Він лише надає подію VideoUploaded і викликає її при виході ролика.
public class YoutubeChannel
{
    public string ChannelName { get; }

    // Подія за стандартом .NET:
    //  - object? sender: посилання на сам канал
    //  - VideoEventArgs e: інформація про відео
    public event EventHandler<VideoEventArgs>? VideoUploaded;

    public YoutubeChannel(string channelName)
    {
        ChannelName = channelName;
    }

    // Метод, який ініціює подію (публікація відео)
    public void PublishVideo(string title, int durationMinutes)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n📢 [Канал '{ChannelName}'] Опублікував нове відео: «{title}» ({durationMinutes} хв.)");
        Console.ResetColor();

        // Формуємо дані про подію:
        VideoEventArgs args = new(title, durationMinutes);

        // Сповіщаємо всіх, хто підписався:
        OnVideoUploaded(args);
    }

    // Канонічний захищений віртуальний метод інвокації:
    protected virtual void OnVideoUploaded(VideoEventArgs e)
    {
        VideoUploaded?.Invoke(this, e);
    }
}

// ============================================================================
// [КРОК 3] КЛАСИ-ПІДПИСНИКИ (Subscribers)
// ============================================================================

// Підписник 1: Звичайний глядач
public class User
{
    public string Name { get; }

    public User(string name)
    {
        Name = name;
    }

    // Метод-обробник, який реагує на подію
    public void OnNewVideo(object? sender, VideoEventArgs e)
    {
        // Приводимо sender до типу YoutubeChannel, щоб дізнатися, який саме канал випустив ролик
        if (sender is YoutubeChannel channel)
        {
            Console.WriteLine($"   👤 [{Name}] отримав сповіщення: канал '{channel.ChannelName}' випустив «{e.VideoTitle}»! Біжу дивитися!");
        }
    }
}

// Підписник 2: Мобільний додаток (Push-сповіщення на смартфон)
public class MobilePushNotificationService
{
    public void SendPush(object? sender, VideoEventArgs e)
    {
        Console.WriteLine($"   🔔 [Мобільний Push] Спливаюче вікно: «{e.VideoTitle}» вже на YouTube ({e.DurationMinutes} хв.)");
    }
}

// ============================================================================
// [КРОК 4] ДЕМОНСТРАЦІЯ ФЛОУ В MAIN (Покроковий сценарій для пояснення)
// ============================================================================
class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Приклад 4: Патерн Publisher-Subscriber (YouTube Канал) ===\n");

        // --------------------------------------------------------------------
        // ЕТАП 1: Створення видавця
        // --------------------------------------------------------------------
        YoutubeChannel channel = new("C# Академія");
        Console.WriteLine($"Створено YouTube-канал: '{channel.ChannelName}'");

        // --------------------------------------------------------------------
        // ЕТАП 2: Створення підписників
        // --------------------------------------------------------------------
        User alex = new("Олексій");
        User maria = new("Марія");
        MobilePushNotificationService pushService = new();

        // --------------------------------------------------------------------
        // ЕТАП 3: Підписка на подію через '+='
        // --------------------------------------------------------------------
        Console.WriteLine("\n[Підписка на канал (+ =)]");
        Console.WriteLine(" -> Олексій підписався (+дзвіночок)");
        channel.VideoUploaded += alex.OnNewVideo;

        Console.WriteLine(" -> Марія підписалася (+дзвіночок)");
        channel.VideoUploaded += maria.OnNewVideo;

        Console.WriteLine(" -> Мобільний додаток підписався на Push-сповіщення");
        channel.VideoUploaded += pushService.SendPush;

        // --------------------------------------------------------------------
        // ЕТАП 4: Публікація першого відео (Всі підписники отримують сповіщення)
        // --------------------------------------------------------------------
        Console.WriteLine("\n========================================================");
        Console.WriteLine("ПУБЛІКАЦІЯ ВІДЕО №1 (Підписано 3 слухачі):");
        Console.WriteLine("========================================================");
        channel.PublishVideo("Урок 1: Що таке делегати?", durationMinutes: 15);

        // --------------------------------------------------------------------
        // ЕТАП 5: Відписка одного з користувачів через '-='
        // --------------------------------------------------------------------
        Console.WriteLine("\n========================================================");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("[ВІДПИСКА (- =)]: Марія вирішила відписатися від каналу...");
        Console.ResetColor();
        Console.WriteLine("========================================================");
        channel.VideoUploaded -= maria.OnNewVideo;

        // --------------------------------------------------------------------
        // ЕТАП 6: Публікація другого відео (Марія більше нічого не отримає)
        // --------------------------------------------------------------------
        channel.PublishVideo("Урок 2: Події та патерн Publisher-Subscriber", durationMinutes: 22);

        // --------------------------------------------------------------------
        // ПІДСУМОК ДЛЯ СТУДЕНТІВ:
        // 1. Канал (Publisher) викликає подію VideoUploaded.
        // 2. Він не знає, скільки підписників і хто вони: люди, пуш-сервери чи боти.
        // 3. Підписка робиться через +=, відписка через -=.
        // 4. Дані передаються безпечно через VideoEventArgs.
        // --------------------------------------------------------------------
    }
}
