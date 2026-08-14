using SerializationDeserialization;

// Serialize() method
Console.WriteLine("---------------------------");
Console.WriteLine("СЕРІАЛІЗАЦІЯ");
Console.WriteLine("---------------------------");
var obj = new User { Name = "John Doe", Age = 30 };
var s = XmlConverter.Serialize(obj);
Console.WriteLine(s);

Console.WriteLine("---------------------------");
Console.WriteLine("ДЕ -> СЕРІАЛІЗАЦІЯ");
Console.WriteLine("---------------------------");

// DeSerialize() method
var stringToDecode = "<User><Name>John Doe</Name><Age>30</Age></User>";
var decodedUser = XmlConverter.Deserialize<User>(stringToDecode);
Console.WriteLine("User name: " + decodedUser.Name);
Console.WriteLine("User age: " + decodedUser.Age);