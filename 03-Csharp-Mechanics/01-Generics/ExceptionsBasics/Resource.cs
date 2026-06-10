namespace ExceptionsBasics;

// Імітація ресурсу, який обов'язково треба закрити (файл, з'єднання з БД, сокет).
// IDisposable — контракт: "у мене є Dispose(), викличте його, коли закінчите".
class Resource : IDisposable
{
    private readonly string _name;

    public Resource(string name)
    {
        _name = name;
        Ui.Info($"    [ресурс] \"{_name}\" відкрито");
    }

    public void Dispose()
    {
        Ui.Note($"    [ресурс] \"{_name}\" закрито (Dispose)");
    }
}
