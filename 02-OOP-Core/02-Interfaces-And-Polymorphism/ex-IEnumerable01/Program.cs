// IEnumerable / IEnumerator - "ручна" реалізація.
//
// Цикл foreach уміє працювати з будь-яким типом, який реалізує інтерфейс
// IEnumerable. Інтерфейс містить єдиний метод:
//     IEnumerator GetEnumerator();
// а IEnumerator (перебирач, "курсор") - три члени:
//     bool MoveNext()   - зсунути курсор на наступний елемент; false, якщо елементи закінчились;
//     object Current    - поточний елемент;
//     void Reset()      - повернути курсор на початок.
//
// Тут ми реалізуємо обидва інтерфейси власноруч, щоб побачити, що саме
// робить foreach "під капотом".
using System.Collections;

namespace EnumerableManualDemo;

// Колекція: реалізує IEnumerable - тобто "мене можна перебирати".
class Playlist : IEnumerable
{
    private readonly string[] songs;

    public Playlist(params string[] songs)
    {
        this.songs = songs;
    }

    // Кожен виклик повертає НОВИЙ перебирач зі своєю власною позицією.
    public IEnumerator GetEnumerator()
    {
        return new PlaylistEnumerator(songs);
    }
}

// Перебирач: знає, як пройти по піснях, і пам'ятає, де він зупинився.
class PlaylistEnumerator : IEnumerator
{
    private readonly string[] songs;
    private int position = -1; // -1 = "перед першим елементом": Current ще недоступний

    public PlaylistEnumerator(string[] songs)
    {
        this.songs = songs;
    }

    public bool MoveNext()
    {
        position++;
        return position < songs.Length;
    }

    public object Current
    {
        get
        {
            if (position < 0 || position >= songs.Length)
                throw new InvalidOperationException("Курсор стоїть поза межами колекції.");
            return songs[position];
        }
    }

    public void Reset()
    {
        position = -1;
    }
}

class Program
{
    static void Main()
    {
        var playlist = new Playlist("Yesterday", "Imagine", "Hey Jude", "Let It Be");

        // 1. Звичайний foreach - працює, бо Playlist реалізує IEnumerable.
        Console.WriteLine("=== 1. foreach ===");
        foreach (string song in playlist)
            Console.WriteLine($"  {song}");

        // 2. Те саме, але без foreach: ось що компілятор робить за нас.
        Console.WriteLine("\n=== 2. Те саме вручну через IEnumerator ===");
        IEnumerator enumerator = playlist.GetEnumerator();
        while (enumerator.MoveNext())
        {
            string song = (string)enumerator.Current; // Current має тип object, потрібне приведення
            Console.WriteLine($"  {song}");
        }

        // 3. Чому перебирач - окремий об'єкт: кожен має власну позицію,
        //    тому по одній колекції можна йти двома "курсорами" незалежно.
        Console.WriteLine("\n=== 3. Два незалежні перебирачі ===");
        IEnumerator first = playlist.GetEnumerator();
        IEnumerator second = playlist.GetEnumerator();

        first.MoveNext();
        first.MoveNext();   // перший курсор - на другій пісні
        second.MoveNext();  // другий курсор - на першій пісні

        Console.WriteLine($"  first.Current  = {first.Current}");
        Console.WriteLine($"  second.Current = {second.Current}");

        // 4. Reset повертає курсор на початок.
        Console.WriteLine("\n=== 4. Reset ===");
        first.Reset();
        first.MoveNext();
        Console.WriteLine($"  Після Reset і MoveNext: first.Current = {first.Current}");
    }
}
