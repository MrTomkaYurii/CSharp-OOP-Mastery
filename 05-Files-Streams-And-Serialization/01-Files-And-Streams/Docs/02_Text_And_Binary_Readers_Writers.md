# 02. Текстові та бінарні адаптери: `StreamReader`, `StreamWriter`, `BinaryReader` та `BinaryWriter`

## 1. Концепція адаптерів потоків (Патерн Decorator)

Низькорівневий клас `Stream` оперує виключно сирими байтами `byte[]`. Ручна конвертація символів, рядків і чисел у байти та навпаки є джерелом частих помилок кодування та виділення пам'яті.

Платформа .NET вирішує це за допомогою патерну **Декоратор (Decorator / Adapter)**: високорівневі читачі та письменники загортають базовий потік `Stream`, надаючи типізовані методи читання та запису:

```mermaid
flowchart LR
    classDef obj fill:#0284c7,stroke:#38bdf8,color:#ffffff,stroke-width:2px;
    classDef dec fill:#0d9488,stroke:#2dd4bf,color:#ffffff,stroke-width:2px;
    classDef raw fill:#7e22ce,stroke:#c084fc,color:#ffffff,stroke-width:2px;

    App["C# Код: writer.WriteLine('Привіт!')"]:::obj --> Dec["StreamWriter / BinaryWriter<br/>(Буферизація, кодування UTF-8)"]:::dec
    Dec --> Stream["FileStream / NetworkStream<br/>(Потік сирих байтів)"]:::raw
    Stream --> Storage[("Фізичний файл / Сокет")]
```

---

## 2. Текстові адаптери: `StreamReader` та `StreamWriter`

`StreamReader` та `StreamWriter` працюють із символами `char` та рядками `string`, використовуючи вказане текстове кодування (`Encoding.UTF8` за замовчуванням).

### Механіка буферизації та властивість `AutoFlush`

За замовчуванням `StreamWriter` зберігає дані у внутрішньому буфері розміром 4 КБ. Фізичний запис на диск відбувається лише коли:
1. Буфер повністю заповнюється.
2. Явно викликається метод `writer.Flush()` або `await writer.FlushAsync()`.
3. Об'єкт закривається через `writer.Dispose()`.

Якщо програма аварійно завершиться до скидання буфера, нескинуті дані будуть втрачені! Для негайного скидання встановлюють:
```csharp
writer.AutoFlush = true;
```

### Критичний параметр `leaveOpen`: запобігання передчасному закриттю потоку

За замовчуванням виклик `reader.Dispose()` автоматично закриває базовий `Stream`. Якщо цей самий потік потрібно прочитати повторно або передати в інший сервіс, необхідно вказати `leaveOpen: true`:

```csharp
using (var stream = new MemoryStream())
{
    // Вказуємо leaveOpen: true, щоб stream не закрився після блоку using письменника:
    using (var writer = new StreamWriter(stream, Encoding.UTF8, bufferSize: 1024, leaveOpen: true))
    {
        writer.WriteLine("Потокові дані");
    }

    // Потік залишається відкритим і доступним:
    stream.Position = 0;
    using (var reader = new StreamReader(stream, Encoding.UTF8))
    {
        Console.WriteLine(reader.ReadToEnd());
    }
}
```

---

## 3. Бінарні адаптери: `BinaryReader` та `BinaryWriter`

На відміну від текстових адаптерів, що перетворюють числа на рядки символів, бінарні класи зберігають дані у їхньому нативному двійковому форматі:
* Число `int value = 123456789;` у тексті займає 9 байтів ASCII. У бінарному потоці `BinaryWriter.Write(value)` воно записується рівно у **4 байти** (32 біти Little-Endian).
* Рядки записуються з префіксом довжини у форматі **7-bit Encoded Integer**, що усуває потребу в символах перенесення рядка `\r\n`.

```csharp
// Запис структурованого бінарного пакета:
using (var bw = new BinaryWriter(File.OpenWrite("player.dat")))
{
    bw.Write((int)42);               // 4 байти
    bw.Write("Олександр");           // 1 байт довжини + UTF-8 байти
    bw.Write((double)99.5);          // 8 байтів IEEE 754
    bw.Write(true);                  // 1 байт
}
```

---

## 4. Порівняльна таблиця адаптерів потоків

| Клас адаптера | Що читає/записує | Формат збереження | Буферизація |
|:---|:---|:---|:---|
| **`StreamReader / StreamWriter`** | Текстові символи та рядки | Текст у кодуванні (UTF-8, UTF-16) | Внутрішній буфер 4 КБ |
| **`BinaryReader / BinaryWriter`** | Нативні примітиви C# (`int`, `bool`, `float`) | Двійкові байти CLR (Little-Endian) | Мінімальна / відсутня |
| **`StringReader / StringWriter`** | Текст у пам'яті | `StringBuilder` (без реального I/O) | Оперативна пам'ять процесу |

---

## 5. Матриця проєктів цієї секції

| Проєкт | Посилання на вихідний код | Що демонструє під капотом |
|:---|:---|:---|
| [`02-01-StreamReader`](../02-01-StreamReader/Program.cs) | [Program.cs](../02-01-StreamReader/Program.cs) | Базове читання рядків через `StreamReader` до кінця потоку (`EndOfStream`). |
| [`02-02-StreamReader-StreamWriter`](../02-02-StreamReader-StreamWriter/Program.cs) | [Program.cs](../02-02-StreamReader-StreamWriter/Program.cs) | Робота з `StreamWriter`, конфігурація `AutoFlush` та керування кодуванням. |
| [`05-01-BinaryWriter-BinaryReader`](../05-01-BinaryWriter-BinaryReader/Program.cs) | [Program.cs](../05-01-BinaryWriter-BinaryReader/Program.cs) | Запис і відновлення строго типізованих примітивів через `BinaryWriter` та `BinaryReader`. |
