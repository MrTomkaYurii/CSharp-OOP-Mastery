using System;

/*
 * ── Ініціалізатори конструкторів в ієрархії `: base(...)` (Solis, с. 196) ──
 *
 * Під час створення об'єкта похідного класу конструктори викликаються
 * від вершини ієрархії донизу:
 *   System.Object -> Базовий клас (MyBaseClass) -> Похідний клас (MyDerivedClass).
 *
 * За замовчуванням компілятор викликає конструктор базового класу без параметрів
 * `: base()`. Якщо потрібно передати аргументи у конкретний перевантажений
 * конструктор базового класу, використовують явний ініціалізатор `: base(x, y)`.
 *
 * Очікуваний вивід:
 *   1. Конструктор MyBaseClass(string): Ініціалізація базової частини [Робоча станція]
 *   2. Конструктор MyDerivedClass(string, int): Додано 16 ядер CPU
 *   Пристрій: Робоча станція, Ядер: 16
 */

class MyBaseClass
{
    public string DeviceName { get; }

    public MyBaseClass()
    {
        DeviceName = "Невідомий пристрій";
        Console.WriteLine("1. Конструктор MyBaseClass() без параметрів");
    }

    public MyBaseClass(string deviceName)
    {
        DeviceName = deviceName;
        Console.WriteLine($"1. Конструктор MyBaseClass(string): Ініціалізація базової частини [{DeviceName}]");
    }
}

class MyDerivedClass : MyBaseClass
{
    public int CpuCores { get; }

    // Явно передаємо deviceName у конструктор базового класу через `: base(deviceName)`
    public MyDerivedClass(string deviceName, int cpuCores)
        : base(deviceName)
    {
        CpuCores = cpuCores;
        Console.WriteLine($"2. Конструктор MyDerivedClass(string, int): Додано {CpuCores} ядер CPU");
    }

    public void PrintSummary()
    {
        Console.WriteLine($"Пристрій: {DeviceName}, Ядер: {CpuCores}");
    }
}

class Program
{
    static void Main()
    {
        MyDerivedClass workstation = new MyDerivedClass("Робоча станція", 16);
        workstation.PrintSummary();
    }
}
