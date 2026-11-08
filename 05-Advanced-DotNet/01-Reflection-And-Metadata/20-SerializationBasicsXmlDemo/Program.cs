using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using SerializationBasicsXmlDemo;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 20. СЕРІАЛІЗАЦІЯ ТА ДЕСЕРІАЛІЗАЦІЯ: БАЗОВЕ РОЗУМІННЯ ФОРМАТУ XML");
Console.WriteLine(" Кореневий елемент, теги проти атрибутів та ієрархічне дерево DOM");
Console.WriteLine("==========================================================================\n");

// -----------------------------------------------------------------------
// Крок 1: Особливості формату XML порівняно з JSON
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 1: Анатомія формату XML (eXtensible Markup Language) ---");
Console.WriteLine("1. Кожен XML-документ зобов'язаний мати рівно ОДИН кореневий тег (Root Element).");
Console.WriteLine("2. Дані в XML можна передавати двома способами:");
Console.WriteLine("   - Через вкладені теги (Elements):   <BookTitle>Кобзар</BookTitle>");
Console.WriteLine("   - Через атрибути тегу (Attributes): <LibraryBook isbn=\"978-966\" year=\"2025\">");
Console.WriteLine("3. XML активно використовується у файлах проєктів (.csproj), банківських шлюзах, SOAP та конфігураціях.\n");

// -----------------------------------------------------------------------
// Крок 2: Серіалізація C#-об'єкта в XML
// -----------------------------------------------------------------------
var book = new LibraryBook
{
    Isbn = "978-617-12-8941-3",
    PublishedYear = 2024,
    Title = "Архітектура платформи .NET та C#",
    Author = "Андрій Ковальчук",
    Price = 850.50m,
    Categories = new List<string> { "Програмування", "Комп'ютерні науки", "C#" },
    InternalStorageShelfCode = "SHELF-B12-HIDDEN"
};

var xmlSerializer = new XmlSerializer(typeof(LibraryBook));
string xmlOutput;

using (var stringWriter = new StringWriter())
{
    xmlSerializer.Serialize(stringWriter, book);
    xmlOutput = stringWriter.ToString();
}

Console.WriteLine("--- Крок 2: Як об'єкт LibraryBook виглядає у форматі XML ---");
Console.WriteLine(xmlOutput);
Console.WriteLine("\nЗверніть увагу:");
Console.WriteLine("  - Властивості з [XmlAttribute] стали атрибутами всередині кореневого тегу <LibraryBook ...>.");
Console.WriteLine("  - Властивості з [XmlElement] стали дочірніми тегами.");
Console.WriteLine("  - Властивість з [XmlIgnore] (InternalStorageShelfCode) не потрапила до документа.\n");

// -----------------------------------------------------------------------
// Крок 3: Дослідження XML як дерева об'єктів (XDocument / LINQ to XML)
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 3: Розбір структури XML-дерева через XDocument ---");
XDocument doc = XDocument.Parse(xmlOutput);
XElement root = doc.Root!;

Console.WriteLine($"Кореневий тег: <{root.Name.LocalName}>");
Console.WriteLine("  Атрибути кореневого тегу:");
foreach (XAttribute attr in root.Attributes())
{
    if (!attr.IsNamespaceDeclaration)
    {
        Console.WriteLine($"    @{attr.Name.LocalName} = \"{attr.Value}\"");
    }
}

Console.WriteLine("  Дочірні елементи (теги):");
foreach (XElement element in root.Elements())
{
    if (element.HasElements)
    {
        Console.WriteLine($"    <{element.Name.LocalName}> (містить вкладених елементів: {element.Elements().Count()})");
    }
    else
    {
        Console.WriteLine($"    <{element.Name.LocalName}> -> {element.Value}");
    }
}
Console.WriteLine();

// -----------------------------------------------------------------------
// Крок 4: Десеріалізація XML назад у екземпляр класу LibraryBook
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 4: Відновлення об'єкта LibraryBook з XML-рядка ---");
var readerSettings = new XmlReaderSettings
{
    DtdProcessing = DtdProcessing.Prohibit,
    XmlResolver = null
};
using (var stringReader = new StringReader(xmlOutput))
using (var xmlReader = XmlReader.Create(stringReader, readerSettings))
{
    var restoredBook = (LibraryBook?)xmlSerializer.Deserialize(xmlReader);
    if (restoredBook != null)
    {
        Console.WriteLine($"  Відновлено ISBN:      {restoredBook.Isbn}");
        Console.WriteLine($"  Відновлено Рік:       {restoredBook.PublishedYear}");
        Console.WriteLine($"  Відновлено Назву:     {restoredBook.Title}");
        Console.WriteLine($"  Відновлено Автора:    {restoredBook.Author}");
        Console.WriteLine($"  Відновлено Категорії: {string.Join(", ", restoredBook.Categories)}");
    }
}
