namespace XmlDeserializationDemo;

/// <summary>
/// Цільова модель конфігурації сервера, яку ми відновлюємо з XML-файлу.
/// </summary>
[XmlRootTag("ServerConfig")]
public class ServerConfiguration
{
    [XmlAttr("environment")]
    public string Environment { get; set; } = string.Empty;

    [XmlAttr("version")]
    public int ConfigVersion { get; set; }

    [XmlTag("HostName")]
    public string Host { get; set; } = string.Empty;

    [XmlTag("PortNumber")]
    public int Port { get; set; }

    [XmlTag("TimeoutSeconds")]
    public double TimeoutSeconds { get; set; }

    [XmlTag("UseHttps")]
    public bool UseSsl { get; set; }

    // Поле захищене від зовнішньої ін'єкції з XML
    [XmlExclude]
    public string MasterEncryptionKey { get; set; } = "DEFAULT-INTERNAL-KEY";
}
