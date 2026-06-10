// Приклад 367: узагальнена (generic) структура.
// T — параметр типу; конкретний тип підставляється під час створення екземпляра.

struct PieceOfData<T>                           // Узагальнена структура
{
    public PieceOfData(T value) { Data = value; }

    // Автовластивість замість ручного поля _data + get/set
    public T Data { get; set; }
}

class Program
{
    static void Main()
    {
        // Аргумент типу вказуємо явно: int та string — два різні закриті типи
        var intData = new PieceOfData<int>(10);
        var stringData = new PieceOfData<string>("Hi there.");

        Console.WriteLine($"intData    = {intData.Data}");
        Console.WriteLine($"stringData = {stringData.Data}");
    }
}
