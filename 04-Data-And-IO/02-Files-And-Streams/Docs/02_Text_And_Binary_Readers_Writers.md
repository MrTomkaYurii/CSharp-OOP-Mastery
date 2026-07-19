# Текстові та бінарні адаптери: StreamReader, StreamWriter, BinaryReader та BinaryWriter

## Вступ
Низькорівнева робота з потоками через масиви байтів `byte[]` є незручною, коли потрібно читати або записувати текст чи строго типізовані примітиви C#. Для цього .NET надає адаптери (декоратори), які загортають довільний `Stream` і надають високорівневі методи.

---

## 1. Текстові потоки: StreamReader та StreamWriter

`StreamReader` та `StreamWriter` працюють на рівні символів `char` та рядків `string`, автоматично конвертуючи їх у байти за допомогою заданого кодування (`Encoding.UTF8`).

```mermaid
graph LR
    String["Рядок C# (UTF-16 char)"] --> Writer["StreamWriter (AutoFlush, Encoding)"]
    Writer --> Buffer["Внутрішній буфер"]
    Buffer --> Stream["FileStream (масив байтів)"]
    Stream --> Disk[("Фізичний файл")]
```

### Важливі нюанси:
1. **Буферизація:** `StreamWriter` накопичує символи у внутрішньому буфері (за замовчуванням 4 КБ) і скидає їх на диск лише тоді, коли буфер заповниться або викликано `Flush()` / `Dispose()`.
2. **`AutoFlush`:** Якщо встановити `writer.AutoFlush = true`, кожен виклик `Write`/`WriteLine` негайно скидає дані в базовий потік.
3. **Патерн читання до кінця файлу:**
   ```csharp
   using var reader = new StreamReader("notes.txt", Encoding.UTF8);
   string? line;
   while ((line = reader.ReadLine()) != null)
   {
       Console.WriteLine(line);
   }
   ```

---

## 2. Бінарні потоки: BinaryReader та BinaryWriter

`BinaryReader` та `BinaryWriter` записують примітивні типи даних C# (`int`, `double`, `bool`, `DateTime`) у їхньому компактному двійковому представленні без перетворення на текст.

- Наприклад, число `123456789` у тексті займає 9 байтів ASCII (або 18 байтів у UTF-16), тоді як у бінарному вигляді це рівно 4 байти (32 біти).
- Рядки записуються з префіксом довжини (7-bit encoded integer).

---

## 3. Операції в пам'яті: StringReader та StringWriter

`StringWriter` використовує `StringBuilder` як сховище, дозволяючи формувати текст через потоковий інтерфейс `TextWriter`. `StringReader` дозволяє зчитувати звичайний рядок порядково, як із файлу.

---

## 4. Проекти для практики

- [02-01-StreamReader](file:///04-Data-And-IO/02-Files-And-Streams/02-01-StreamReader/Program.cs) — Створення та читання через `StreamReader`.
- [02-02-StreamReader-StreamWriter](file:///04-Data-And-IO/02-Files-And-Streams/02-02-StreamReader-StreamWriter/Program.cs) — Запис рядків через `StreamWriter` з `AutoFlush`.
- [02-03-StreamReader-StreamWriter](file:///04-Data-And-IO/02-Files-And-Streams/02-03-StreamReader-StreamWriter/Program.cs) — Дописування тексту в кінець файлу (Append).
- [02-04-StreamReader-StreamWriter](file:///04-Data-And-IO/02-Files-And-Streams/02-04-StreamReader-StreamWriter/Program.cs) — Робота з кодуваннями (UTF-8, ASCII, Unicode).
- [04-01-TextWriter-TextReader](file:///04-Data-And-IO/02-Files-And-Streams/04-01-TextWriter-TextReader/Program.cs) — Поліморфне використання абстрактних класів `TextWriter` та `TextReader`.
- [05-01-BinaryWriter-BinaryReader](file:///04-Data-And-IO/02-Files-And-Streams/05-01-BinaryWriter-BinaryReader/Program.cs) — Серіалізація примітивних типів через `BinaryWriter`.
- [05-02-BinaryWriter-BinaryReader](file:///04-Data-And-IO/02-Files-And-Streams/05-02-BinaryWriter-BinaryReader/Program.cs) — Десеріалізація та відновлення структурних даних через `BinaryReader`.
- [06-01-StringWriter-StringReader](file:///04-Data-And-IO/02-Files-And-Streams/06-01-StringWriter-StringReader/Program.cs) — Потокова генерація рядків у пам'яті.
