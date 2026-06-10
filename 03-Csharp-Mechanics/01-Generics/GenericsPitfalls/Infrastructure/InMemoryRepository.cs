using GenericsPitfalls.Interfaces;
using GenericsPitfalls.Models;

namespace GenericsPitfalls.Infrastructure;

// Узагальнений репозиторій у пам'яті.
// Обмеження: T — посилальний тип (class) і має Id (IEntity).
//  - class   -> дозволяє T? як "може бути null" та повернення null
//  - IEntity -> дозволяє звертатися до entity.Id всередині класу
public class InMemoryRepository<T> : IRepository<T> where T : class, IEntity
{
    private readonly Dictionary<int, T> _items = new Dictionary<int, T>();

    public void Add(T entity) => _items[entity.Id] = entity;

    public bool Remove(int id) => _items.Remove(id);

    public T? FindById(int id)
    {
        // default(T) для посилального типу — це null
        return _items.TryGetValue(id, out T? found) ? found : default;
    }

    public IEnumerable<T> GetAll() => _items.Values;
}
