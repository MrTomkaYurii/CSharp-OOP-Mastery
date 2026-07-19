# Модуль 11: Робота з файлами, потоками та даними в C# (.NET 10)

Цей модуль є комплексним посібником з введення/виведення (I/O) у .NET: від байтових потоків `FileStream` та адаптерів `StreamReader`/`BinaryReader` до високорівневих класів `File`/`FileInfo` та генерації звітів Microsoft Excel за допомогою бібліотеки NPOI.

---

## 🗺️ Карта підручника (Навчальні гайди)

1. **[01. Архітектура потоків (Streams) та робота з FileStream](Docs/01_Streams_Architecture_And_FileStream.md)**
   - Базовий клас `Stream`, буферизація, режими `FileMode`, `FileAccess`, `FileShare`, патерн безпечного вивільнення ресурсів.
2. **[02. Текстові та бінарні адаптери: StreamReader, StreamWriter, BinaryReader та BinaryWriter](Docs/02_Text_And_Binary_Readers_Writers.md)**
   - Кодування символів, властивість `AutoFlush`, бінарна серіалізація примітивів, потоки в пам'яті `StringWriter`.
3. **[03. Файлова система: File vs FileInfo, Directory vs DirectoryInfo](Docs/03_File_And_FileInfo_Directory_And_DirectoryInfo.md)**
   - Порівняння статичних класів з екземплярними, кешування метаданих, перевірка прав ОС, рекурсивний пошук файлів.
4. **[04. Експорт та імпорт табличних даних: Робота з Excel (NPOI)](Docs/04_Excel_Data_Import_Export.md)**
   - Робота з форматом `.xlsx`, мапінг сутностей C# на рядки та клітинки, стилізація та валідація даних.

---

## 📂 Структура проектів модуля (36 проектів)

### 1. Низькорівневі файлові потоки (FileStream)
| Проект | Опис | Ключові концепції |
| :--- | :--- | :--- |
| **[01-01-FileStream](file:///04-Data-And-IO/02-Files-And-Streams/01-01-FileStream/Program.cs)** | Створення та запис | `FileStream`, `FileMode.Create`, запис байтів |
| **[01-02-FileStream](file:///04-Data-And-IO/02-Files-And-Streams/01-02-FileStream/Program.cs)** | Побайтове читання | `fs.ReadByte()`, перевірка кінця файлу |
| **[01-03-FileStream](file:///04-Data-And-IO/02-Files-And-Streams/01-03-FileStream/Program.cs)** | Блокове читання | `fs.Read(buffer, 0, count)`, буферизація |

### 2. Текстові адаптери (StreamReader / StreamWriter)
| Проект | Опис | Ключові концепції |
| :--- | :--- | :--- |
| **[02-01-StreamReader](file:///04-Data-And-IO/02-Files-And-Streams/02-01-StreamReader/Program.cs)** | Читання тексту | `StreamReader.ReadLine()`, `ReadToEnd()` |
| **[02-02-StreamReader-StreamWriter](file:///04-Data-And-IO/02-Files-And-Streams/02-02-StreamReader-StreamWriter/Program.cs)** | Запис тексту | `StreamWriter.WriteLine()`, `Flush()` |
| **[02-03-StreamReader-StreamWriter](file:///04-Data-And-IO/02-Files-And-Streams/02-03-StreamReader-StreamWriter/Program.cs)** | Дописування у файл | Конструктор з прапорцем `append: true` |
| **[02-04-StreamReader-StreamWriter](file:///04-Data-And-IO/02-Files-And-Streams/02-04-StreamReader-StreamWriter/Program.cs)** | Кодування (Encoding) | `Encoding.UTF8`, `Encoding.ASCII`, `Encoding.Unicode` |
| **[02-05-StreamReader-StreamWriter](file:///04-Data-And-IO/02-Files-And-Streams/02-05-StreamReader-StreamWriter/Program.cs)** | Порядкове читання | Цикл `while (!reader.EndOfStream)` |
| **[02-06-StreamReader-StreamWriter](file:///04-Data-And-IO/02-Files-And-Streams/02-06-StreamReader-StreamWriter/Program.cs)** | Позиціювання потоку | Властивість `BaseStream.Position` |

### 3. Статичний клас File
| Проект | Опис | Ключові концепції |
| :--- | :--- | :--- |
| **[03-01-FileClass](file:///04-Data-And-IO/02-Files-And-Streams/03-01-FileClass/Program.cs)** | Створення та видалення | `File.Create()`, `File.Delete()` |
| **[03-02-FileClass](file:///04-Data-And-IO/02-Files-And-Streams/03-02-FileClass/Program.cs)** | Перевірка існування | `File.Exists()` |
| **[03-03-FileClass](file:///04-Data-And-IO/02-Files-And-Streams/03-03-FileClass/Program.cs)** | Швидкий запис/читання | `File.WriteAllText()`, `File.ReadAllText()` |
| **[03-04-FileClass](file:///04-Data-And-IO/02-Files-And-Streams/03-04-FileClass/Program.cs)** | Порядковий масив | `File.WriteAllLines()`, `File.ReadAllLines()` |
| **[03-05-FileClass](file:///04-Data-And-IO/02-Files-And-Streams/03-05-FileClass/Program.cs)** | Копіювання та переміщення | `File.Copy()`, `File.Move()` |
| **[03-06-FileClass](file:///04-Data-And-IO/02-Files-And-Streams/03-06-FileClass/Program.cs)** | Дописування рядків | `File.AppendAllText()`, `File.AppendAllLines()` |

### 4. Поліморфні класи TextWriter та TextReader
| Проект | Опис | Ключові концепції |
| :--- | :--- | :--- |
| **[04-01-TextWriter-TextReader](file:///04-Data-And-IO/02-Files-And-Streams/04-01-TextWriter-TextReader/Program.cs)** | Абстрактний TextWriter | Запис у файл через поліморфний інтерфейс |
| **[04-02-TextWriter-TextReader](file:///04-Data-And-IO/02-Files-And-Streams/04-02-TextWriter-TextReader/Program.cs)** | Абстрактний TextReader | Посимвольне читання `reader.Read()` |

### 5. Бінарні потоки (BinaryWriter / BinaryReader)
| Проект | Опис | Ключові концепції |
| :--- | :--- | :--- |
| **[05-01-BinaryWriter-BinaryReader](file:///04-Data-And-IO/02-Files-And-Streams/05-01-BinaryWriter-BinaryReader/Program.cs)** | Бінарний запис | Запис `int`, `double`, `bool` безпосередньо в байти |
| **[05-02-BinaryWriter-BinaryReader](file:///04-Data-And-IO/02-Files-And-Streams/05-02-BinaryWriter-BinaryReader/Program.cs)** | Бінарне відновлення | Читання строго типізованих примітивів |

### 6. Потоки в оперативній пам'яті (StringWriter / StringReader)
| Проект | Опис | Ключові концепції |
| :--- | :--- | :--- |
| **[06-01-StringWriter-StringReader](file:///04-Data-And-IO/02-Files-And-Streams/06-01-StringWriter-StringReader/Program.cs)** | Формування тексту в пам'яті | Використання `StringBuilder` через `StringWriter` |
| **[06-02-StringWriter-StringReader](file:///04-Data-And-IO/02-Files-And-Streams/06-02-StringWriter-StringReader/Program.cs)** | Читання рядка як потоку | Порядковий парсинг `StringReader` |

### 7. Екземплярний клас FileInfo
| Проект | Опис | Ключові концепції |
| :--- | :--- | :--- |
| **[07-01-FileInfoClass](file:///04-Data-And-IO/02-Files-And-Streams/07-01-FileInfoClass/Program.cs)** | Створення екземпляра | Метод `fi.Create()` |
| **[07-02-FileInfoClass](file:///04-Data-And-IO/02-Files-And-Streams/07-02-FileInfoClass/Program.cs)** | Метадані файлу | `fi.Length`, `fi.Extension`, `fi.CreationTime` |
| **[07-03-FileInfoClass](file:///04-Data-And-IO/02-Files-And-Streams/07-03-FileInfoClass/Program.cs)** | Видалення | Метод `fi.Delete()` |
| **[07-04-FileInfoClass](file:///04-Data-And-IO/02-Files-And-Streams/07-04-FileInfoClass/Program.cs)** | Копіювання | Метод `fi.CopyTo()` |
| **[07-05-FileInfoClass](file:///04-Data-And-IO/02-Files-And-Streams/07-05-FileInfoClass/Program.cs)** | Переміщення | Метод `fi.MoveTo()` |
| **[07-06-FileInfoClass](file:///04-Data-And-IO/02-Files-And-Streams/07-06-FileInfoClass/Program.cs)** | Відкриття текстового потоку | `fi.OpenText()`, `fi.CreateText()` |
| **[07-07-FileInfoClass](file:///04-Data-And-IO/02-Files-And-Streams/07-07-FileInfoClass/Program.cs)** | Дописування тексту | Метод `fi.AppendText()` |
| **[07-08-FileInfoClass](file:///04-Data-And-IO/02-Files-And-Streams/07-08-FileInfoClass/Program.cs)** | Атрибути файлу | `fi.Attributes` (ReadOnly, Hidden) |

### 8. Робота з папками (DirectoryInfo)
| Проект | Опис | Ключові концепції |
| :--- | :--- | :--- |
| **[08-01-08-DirectoryInfoClass](file:///04-Data-And-IO/02-Files-And-Streams/08-01-08-DirectoryInfoClass/Program.cs)** | Створення папки | `dir.Create()`, `dir.CreateSubdirectory()` |
| **[08-02-DirectoryInfoClass](file:///04-Data-And-IO/02-Files-And-Streams/08-02-DirectoryInfoClass/Program.cs)** | Властивості каталогу | `dir.FullName`, `dir.Parent`, `dir.Root` |
| **[08-03-DirectoryInfoClass](file:///04-Data-And-IO/02-Files-And-Streams/08-03-DirectoryInfoClass/Program.cs)** | Видалення папки | Рекурсивне видалення `dir.Delete(true)` |
| **[08-04-DirectoryInfoClass](file:///04-Data-And-IO/02-Files-And-Streams/08-04-DirectoryInfoClass/Program.cs)** | Переміщення папки | Метод `dir.MoveTo()` |
| **[08-05-DirectoryInfoClass](file:///04-Data-And-IO/02-Files-And-Streams/08-05-DirectoryInfoClass/Program.cs)** | Отримання списку файлів | `dir.GetFiles("*.txt")`, фільтрація |

### 9. Робота з Excel (NPOI)
| Проект | Опис | Ключові концепції |
| :--- | :--- | :--- |
| **[09-01-ExportAndImportExcelData](file:///04-Data-And-IO/02-Files-And-Streams/09-01-ExportAndImportExcelData/Program.cs)** | Експорт та імпорт сутностей | `XSSFWorkbook`, створення аркушів, стилізація клітинок |
| **[09-02-ExportAndImportExcelData](file:///04-Data-And-IO/02-Files-And-Streams/09-02-ExportAndImportExcelData/Program.cs)** | Читання та парсинг таблиць | Обхід рядків, валідація числових та строкових клітинок |

---

## 🛠️ Перевірка збірки (Zero-Error Verification)

Усі 36 проектів модуля оновлені до **.NET 10** та компілюються через сучасне Visual Studio рішення:
```powershell
dotnet build "04-Data-And-IO/02-Files-And-Streams/OOP-WorkWithFilesInC#.slnx" -c Release
```
