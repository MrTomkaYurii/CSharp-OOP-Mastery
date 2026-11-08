# 04. Декоратори потоків: Стиснення (GZip / Brotli), ZIP-архіви та реактивний FileSystemWatcher

Цей розділ показує, як архітектура потоків `System.IO.Stream` реалізує класичний патерн **Decorator (Обгортка)** для стиснення даних на льоту, пакетування у ZIP-архіви та реактивної обробки файлів.

---

## 1. Патерн Decorator у потоках: `GZipStream` та `BrotliStream`

Оскільки і `FileStream`, і `MemoryStream`, і `GZipStream`, і `BrotliStream` успадковують спільний абстрактний клас `Stream`, їх можна вкладати один в одного як матрьошки:

`JsonSerializer` -> пише байти у `BrotliStream` -> який стискає їх і передає у `FileStream` або `MemoryStream`.

- **`GZipStream`** — універсальний стандарт RFC 1952, сумісний із усіма утилітами (`gzip`, браузери, HTTP).
- **`BrotliStream`** — сучасний алгоритм стиснення тексту та JSON із відчутно вищим коефіцієнтом стиснення.

---

## 2. Багатофайлові архіви `ZipArchive`

Клас `ZipArchive` із простору імен `System.IO.Compression` дозволяє створювати, модифікувати та інспектувати `.zip`-архіви без проміжних тимчасових папок на диску — кожен запис `ZipArchiveEntry` відкриває власний потік `entry.Open()`.

---

## 3. Реактивний моніторинг директорії (`FileSystemWatcher`) та лінивий конвеєр CSV

- **`FileSystemWatcher`** підписується на системні сповіщення ОС (`Created`, `Changed`, `Renamed`, `Deleted`) для автоматичного запуску імпорту щойно файл з'являється у папці.
- Поєднання `StreamReader` + ітератора `yield return` дозволяє парсити гігабайтні CSV-файли по одному рядку (`O(1)` пам'яті) і фільтрувати їх через LINQ.

---

## Матриця проєктів

| Проєкт | Призначення та ключові концепції | Вихідний код |
| :--- | :--- | :--- |
| **`25-CompressionAndZipArchivesDemo`** | Стиснення JSON-потоків через `GZipStream` та `BrotliStream`, створення та читання `ZipArchive` | [`Program.cs`](../25-CompressionAndZipArchivesDemo/Program.cs) |
| **`26-FileSystemWatcherAndCsvPipelineDemo`** | Відстеження нових файлів через `FileSystemWatcher` та потоковий парсинг CSV через `yield return` + LINQ | [`Program.cs`](../26-FileSystemWatcherAndCsvPipelineDemo/Program.cs) | 