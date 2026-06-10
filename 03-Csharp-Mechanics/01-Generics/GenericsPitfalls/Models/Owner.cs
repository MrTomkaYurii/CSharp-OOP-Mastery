namespace GenericsPitfalls.Models;

// Власник тварин: зв'язок "один до багатьох" (Owner 1 --- * Animal).
// На відміну від Animal, Owner ПРАВИЛЬНО реалізує рівність через IEquatable<Owner>:
// два об'єкти з однаковим Id вважаються одним власником.
public class Owner : IEntity, IEquatable<Owner>
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public List<Animal> Pets { get; } = new List<Animal>();

    public bool Equals(Owner? other) => other is not null && Id == other.Id;

    public override bool Equals(object? obj) => Equals(obj as Owner);

    // Правило: об'єкти, що рівні за Equals, МУСЯТЬ мати однаковий GetHashCode
    public override int GetHashCode() => Id.GetHashCode();
}
