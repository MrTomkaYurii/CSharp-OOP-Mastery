using CreateCustomAttributes;

Console.OutputEncoding = System.Text.Encoding.UTF8;

//створюємо екземпляр користувача
var user = new User() { Age = 15, Name = "John" };

Console.WriteLine("Користувач повинен бути старшим за 18 років. Дане число встановлюється через атрибут");
Console.WriteLine(" ----------------------------- ");
Console.WriteLine("Користувач повнолітній: " + MinValueValidator.Validate(user));