// Приклад демонструє реалізацію ОДНИМ класом ОДРАЗУ ДВОХ різних
// інтерфейсів, кожен з яких описує окрему частину поведінки:
// IDataRetrieve відповідає за читання даних, IDataStore - за запис.
// Такий поділ дозволяє розділяти "контракти" за призначенням,
// а клас MyData поєднує обидві можливості в одній реалізації.
using System;

interface IDataRetrieve { int GetData(); }            // Інтерфейс "читання" - лише отримання значення.
interface IDataStore { void SetData(int x); }    // Інтерфейс "запис" - лише встановлення значення.

class MyData : IDataRetrieve, IDataStore              // Клас реалізує обидва інтерфейси одночасно.
{
    int Mem1;                                          // Приватне поле, що зберігає дані.

    public int GetData()
    {
        return Mem1;
    }

    public void SetData(int x)
    {
        Mem1 = x;
    }
}

class Program
{
    static void Main()                                 // Main
    {
        MyData data = new MyData();
        data.SetData(5);
        Console.WriteLine($"Value = {data.GetData()}");
    }
}