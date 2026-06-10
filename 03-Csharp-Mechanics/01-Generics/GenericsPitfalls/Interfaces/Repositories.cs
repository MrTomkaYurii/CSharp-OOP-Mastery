using GenericsPitfalls.Models;

namespace GenericsPitfalls.Interfaces;

// Інтерфейс ТІЛЬКИ ДЛЯ ЧИТАННЯ: T з'являється лише у результатах (T?, IEnumerable<T>),
// тому можна поставити "out" (коваріантність):
// IReadOnlyRepository<Dog> можна передати туди, де чекають IReadOnlyRepository<Animal>.
public interface IReadOnlyRepository<out T> where T : class, IEntity
{
    T? FindById(int id);            // null, якщо сутність не знайдено
    IEnumerable<T> GetAll();
}

// Інтерфейс ДЛЯ ЗАПИСУ: T приймається як параметр (Add), тому "out" ставити НЕ МОЖНА.
// Якби IRepository<Dog> приймався як IRepository<Animal>, у сховище собак можна було б
// додати кота. Тому цей інтерфейс залишається інваріантним.
public interface IRepository<T> : IReadOnlyRepository<T> where T : class, IEntity
{
    void Add(T entity);
    bool Remove(int id);
}

// Валідатор ТІЛЬКИ ПРИЙМАЄ T -> "in" (контраваріантність):
// валідатор для Animal підходить там, де потрібен валідатор для Dog (кожен Dog — це Animal).
public interface IValidator<in T>
{
    bool IsValid(T item);
}
