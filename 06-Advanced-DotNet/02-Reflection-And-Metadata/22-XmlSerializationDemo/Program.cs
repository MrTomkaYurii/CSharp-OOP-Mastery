using XmlSerializationDemo;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 22. СЕРІАЛІЗАЦІЯ В XML ЧЕРЕЗ РЕФЛЕКСІЮ (ВЛАСНИЙ РУШІЙ)");
Console.WriteLine(" Побудова кореневого тегу, XML-атрибутів та дочірніх елементів через Type");
Console.WriteLine("==========================================================================\n");

var employee = new EmployeeRecord
{
    EmployeeId = 408,
    Department = "R&D Engineering",
    Name = "Тарас Кравченко",
    Position = "Senior .NET Developer",
    Salary = 5200.50,
    IsRemote = true,
    BankCardPin = "0000-DO-NOT-EXPORT"
};

Console.WriteLine("--- Крок 1: Покрокова траса роботи рефлексивного XML-серіалізатора ---");
string xmlDocument = ReflectionXmlSerializer.Serialize(employee, traceSteps: true);

Console.WriteLine("\n--- Крок 2: Згенерований XML-документ ---");
Console.WriteLine(xmlDocument);
