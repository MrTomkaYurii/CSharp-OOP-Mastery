using GenericsPitfalls.Interfaces;
using GenericsPitfalls.Models;

namespace GenericsPitfalls.Infrastructure;

// Валідатор для будь-якої тварини: перевіряє лише ім'я
public class AnimalValidator : IValidator<Animal>
{
    public bool IsValid(Animal item) => !string.IsNullOrWhiteSpace(item.Name);
}

// Валідатор саме для собак: використовує Breed, якого немає в Animal
public class DogValidator : IValidator<Dog>
{
    public bool IsValid(Dog item) =>
        !string.IsNullOrWhiteSpace(item.Name) && !string.IsNullOrWhiteSpace(item.Breed);
}
