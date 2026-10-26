namespace DependencyInjection.Container;

/// <summary>
/// Атрибут для явного вибору цільового конструктора під час впровадження залежностей.
/// Якщо клас містить декілька публічних конструкторів, IoC-контейнер
/// обирає конструктор, помічений цим атрибутом, ігноруючи правило жадібного вибору.
/// </summary>
[AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false, Inherited = false)]
public sealed class InjectConstructorAttribute : Attribute
{
}

/// <summary>
/// Атрибут для автоматичного впровадження залежності у відкриту властивість об'єкта (Property Injection).
/// Після виклику конструктора контейнер сканує всі публічні властивості з цим атрибутом і встановлює їх значення.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public sealed class InjectPropertyAttribute : Attribute
{
}
