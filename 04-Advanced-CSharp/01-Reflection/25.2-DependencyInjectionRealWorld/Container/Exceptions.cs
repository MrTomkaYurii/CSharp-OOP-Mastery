namespace DependencyInjection.Container;

/// <summary>
/// Виняток, що сигналізує про виявлення циклічної залежності в графі об'єктів.
/// Запобігає фатальному вичерпанню пам'яті стека потоку та падінню процесу через StackOverflowException.
/// </summary>
public sealed class CircularDependencyException : Exception
{
    public CircularDependencyException(string message) : base(message)
    {
    }
}
