using System;
using System.Reflection;
using System.Reflection.Emit;

class Program
{
    static void Main()
    {
        // Ми створюємо нову збірку в пам’яті, в якій можна динамічно визначати класи і методи.
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Стоврення нової динамічної збірки
        AssemblyName assemblyName = new AssemblyName("DynamicAssembly");
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.Run);
        ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("DynamicModule");
        //Тут ми створюємо новий клас MyDynamicClass, який ще не існує фізично.
        TypeBuilder typeBuilder = moduleBuilder.DefineType("MyDynamicClass", TypeAttributes.Public);

        // Додавання нового методу
        MethodBuilder methodBuilder = typeBuilder.DefineMethod(
            "DynamicMethod",
            MethodAttributes.Public | MethodAttributes.Static,
            typeof(void),
            null
        );

        //EmitWriteLine генерує код для виводу рядка на консоль.
        //Ret — повернення з методу.
        //Тобто фактично ми “записуємо тіло методу” динамічно.
        ILGenerator ilGenerator = methodBuilder.GetILGenerator();
        ilGenerator.EmitWriteLine("Це новий динамічний метод!");
        ilGenerator.Emit(OpCodes.Ret);

        // Завершення визначення класу
        Type dynamicType = typeBuilder.CreateType();

        // Створення екземпляру динамічного класу та виклик нового методу
        object dynamicInstance = Activator.CreateInstance(dynamicType);
        MethodInfo dynamicMethod = dynamicType.GetMethod("DynamicMethod");
        dynamicMethod.Invoke(null, null);//public object Invoke(object obj, object[] parameters); null тому що метод статичний
    }
}
