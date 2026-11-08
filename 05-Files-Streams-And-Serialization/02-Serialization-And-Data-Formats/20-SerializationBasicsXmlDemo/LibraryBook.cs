using System.Xml.Serialization;

namespace SerializationBasicsXmlDemo;

/// <summary>
/// Навчальна модель книги в бібліотеці для демонстрації можливостей XML-розмітки.
/// </summary>
[XmlRoot("LibraryBook")]
public class LibraryBook
{
    // Записується як атрибут відкриваючого тегу: <LibraryBook isbn="978-966-..." year="2025">
    [XmlAttribute("isbn")]
    public string Isbn { get; set; } = string.Empty;

    [XmlAttribute("year")]
    public int PublishedYear { get; set; }

    // Записується як вкладений дочірній тег: <BookTitle>...</BookTitle>
    [XmlElement("BookTitle")]
    public string Title { get; set; } = string.Empty;

    [XmlElement("MainAuthor")]
    public string Author { get; set; } = string.Empty;

    [XmlElement("PriceUah")]
    public decimal Price { get; set; }

    // Вкладений масив тегів: <Categories><Category>...</Category></Categories>
    [XmlArray("Categories")]
    [XmlArrayItem("Category")]
    public List<string> Categories { get; set; } = new();

    // Пропускається при генерації XML
    [XmlIgnore]
    public string InternalStorageShelfCode { get; set; } = string.Empty;
}
