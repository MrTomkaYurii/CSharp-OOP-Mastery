using System;
namespace AttributesDemo
{
    class Example
    {

        // [Obsolete] — це атрибут, який позначає методи, класи, властивості або інші елементи 
        // коду як застарілі(obsolete = застарілий, застарілий).Він попереджає розробників, 
        // що цей код не рекомендується використовувати і, можливо, буде видалений в майбутніх версіях.

        static void Main(string[] args)
        {
            Console.WriteLine("Program for Attributes");
            DemoMethod();
        }

        [Obsolete("Don't use the DemoMethod()", false)]
        public static void DemoMethod()
        {
            Console.WriteLine("Inside DemoMethod()");
        }
    }
}