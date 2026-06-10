namespace GenericsPitfalls.Models;

// Спільний контракт для всіх "сутностей" предметної області.
// Саме на нього спираються обмеження (where T : IEntity) у репозиторії.
public interface IEntity
{
    int Id { get; }
}
