using System;
using System.Reflection;

class MyClass
{
    public int MyField;
    public void MyMethod()
    {
        Console.WriteLine("Це метод MyMethod");
    }
}

class Program
{
    static void Main()
    {
        // Створення екземпляра класу MyClass
        MyClass myInstance = new MyClass();

        // Отримання типу класу за допомогою рефлексії
        Type myType = myInstance.GetType();

        // Отримання інформації про поля класу
        FieldInfo[] fields = myType.GetFields();
        Console.WriteLine("Поля класу:");
        foreach (FieldInfo field in fields)
        {
            Console.WriteLine(field.Name);
        }

        // Отримання інформації про методи класу
        MethodInfo[] methods = myType.GetMethods();
        Console.WriteLine("Методи класу:");
        foreach (MethodInfo method in methods)
        {
            Console.WriteLine(method.Name);
        }

        // Виклик методу за допомогою рефлексії
        MethodInfo myMethod = myType.GetMethod("MyMethod");
        myMethod.Invoke(myInstance, null);
    }
}
