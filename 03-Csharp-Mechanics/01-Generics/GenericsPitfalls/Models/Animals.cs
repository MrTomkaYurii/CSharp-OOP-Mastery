namespace GenericsPitfalls.Models;

// Ієрархія: Animal (база) -> Dog, Cat.
// Animal реалізує IEntity, тому тварин можна зберігати в репозиторії.
// ВАЖЛИВО: Animal НЕ перевизначає Equals/GetHashCode — порівнюється за посиланням.
// Це навмисно: на цьому побудовано демо №9.
public class Animal : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int? OwnerId { get; set; }          // Nullable<int>: у тварини може не бути власника

    public override string ToString() => $"{GetType().Name} #{Id} \"{Name}\"";
}

public class Dog : Animal
{
    public string Breed { get; set; } = "";    // Порода — є лише у собак
}

public class Cat : Animal
{
    public bool IsIndoor { get; set; }         // Домашня кішка — є лише у котів
}
