using System.Reflection;

Type myType = typeof(Person);
foreach (PropertyInfo prop in myType.GetProperties(
    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static))
{
    Console.Write($"{prop.PropertyType} {prop.Name} {{");

    // якщо властивість доступна для читання
    if (prop.CanRead) Console.Write("get;");
    // якщо властивість доступна до запису
    if (prop.CanWrite) Console.Write("set;");
    Console.WriteLine("}");
}

class Person
{
    public string Name { get; }
    public int Age { get; set; }
    public Person(string name, int age)
    {
        Name = name;
        Age = age;
    }
    public void Print() => Console.WriteLine($"{Name} - {Age}");
}