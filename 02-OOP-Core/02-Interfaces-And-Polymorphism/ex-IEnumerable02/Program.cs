// IEnumerable + yield return - той самий плейліст, але значно коротше.
//
// У прикладі ex-IEnumerable01 довелося писати окремий клас-перебирач із
// полем position та методами MoveNext/Current/Reset. Ключове слово
// "yield return" перекладає цю роботу на компілятор: він сам генерує
// такий клас. Ми лише описуємо, які елементи і в якому порядку віддавати.
//
// Дві речі, які варто помітити:
//   1. GetEnumerator() перетворюється на кілька рядків.
//   2. Метод з yield return виконується ліниво: код у ньому "засинає" після
//      кожного yield return і продовжується лише тоді, коли foreach
//      запросить наступний елемент.
using System.Collections;

namespace EnumerableYieldDemo;

class Playlist : IEnumerable
{
    private readonly string[] songs;

    public Playlist(params string[] songs)
    {
        this.songs = songs;
    }

    // Порівняйте з ex-IEnumerable01: замість окремого класу-перебирача -
    // цикл з yield return. Компілятор сам зробить з цього IEnumerator.
    public IEnumerator GetEnumerator()
    {
        foreach (string song in songs)
            yield return song;
    }

    // Метод, що повертає IEnumerable, теж може бути написаний через yield return.
    // Так можна створювати "альтернативні способи обходу" однієї колекції.
    public IEnumerable InReverseOrder()
    {
        for (int i = songs.Length - 1; i >= 0; i--)
        {
            Console.WriteLine($"    (метод віддає: {songs[i]})");
            yield return songs[i];
        }
    }
}

class Program
{
    static void Main()
    {
        var playlist = new Playlist("Yesterday", "Imagine", "Hey Jude", "Let It Be");

        // 1. Звичайний foreach - працює так само, як і раніше.
        Console.WriteLine("=== 1. foreach ===");
        foreach (string song in playlist)
            Console.WriteLine($"  {song}");

        // 2. Інший порядок обходу через власний метод з yield return.
        //    Зверніть увагу на порядок рядків у виводі: повідомлення методу
        //    та повідомлення циклу ЧЕРГУЮТЬСЯ. Метод не готує всі елементи
        //    наперед - він віддає їх по одному, коли цикл про це просить.
        Console.WriteLine("\n=== 2. Зворотний порядок (ліниве виконання) ===");
        foreach (string song in playlist.InReverseOrder())
            Console.WriteLine($"  Граємо: {song}");

        // 3. Наслідок ліні: якщо вийти з циклу раніше, решта елементів
        //    взагалі не буде оброблена - повідомлень про них не з'явиться.
        Console.WriteLine("\n=== 3. Дві пісні і break ===");
        int count = 0;
        foreach (string song in playlist.InReverseOrder())
        {
            Console.WriteLine($"  Граємо: {song}");
            if (++count == 2)
                break;
        }
    }
}
