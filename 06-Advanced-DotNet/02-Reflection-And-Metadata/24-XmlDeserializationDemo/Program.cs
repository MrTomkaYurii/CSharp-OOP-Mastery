using XmlDeserializationDemo;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 24. ДЕСЕРІАЛІЗАЦІЯ З XML ЧЕРЕЗ РЕФЛЕКСІЮ (ВЛАСНИЙ РУШІЙ)");
Console.WriteLine(" Як рефлексія перетворює атрибути та теги XML у властивості об'єкта C#");
Console.WriteLine("==========================================================================\n");

string incomingXml = """
<?xml version="1.0" encoding="utf-8"?>
<ServerConfig environment="Production" version="4">
  <HostName>api.mastery-cloud.ua</HostName>
  <PortNumber>8443</PortNumber>
  <TimeoutSeconds>30.5</TimeoutSeconds>
  <UseHttps>true</UseHttps>
  <MasterEncryptionKey>HACKED-KEY-FROM-XML</MasterEncryptionKey>
  <LegacyUnusedSetting>123</LegacyUnusedSetting>
</ServerConfig>
""";

Console.WriteLine("--- Крок 1: Вхідний XML-документ ---");
Console.WriteLine(incomingXml);

Console.WriteLine("\n--- Крок 2: Покрокова траса роботи рефлексивного XML-десеріалізатора ---");
ServerConfiguration config = ReflectionXmlDeserializer.Deserialize<ServerConfiguration>(incomingXml, traceSteps: true);

Console.WriteLine("\n--- Крок 3: Перевірка відновленого об'єкта ServerConfiguration у пам'яті ---");
Console.WriteLine($"  Environment (з @environment): {config.Environment}");
Console.WriteLine($"  ConfigVersion (з @version):   {config.ConfigVersion}");
Console.WriteLine($"  Host (з <HostName>):          {config.Host}");
Console.WriteLine($"  Port (з <PortNumber>):        {config.Port}");
Console.WriteLine($"  TimeoutSeconds:               {config.TimeoutSeconds}");
Console.WriteLine($"  UseSsl (з <UseHttps>):        {config.UseSsl}");
Console.WriteLine($"  MasterEncryptionKey:          {config.MasterEncryptionKey} (захищено через [XmlExclude])");
