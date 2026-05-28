using System;

namespace Ex_ThisInConstructor;

/*
 * ── Ключове слово `this` у конструкторі ─────────────────────────────
 *
 * `this` — це посилання на поточний об'єкт (той, який зараз створюється
 * або з яким працює метод).
 *
 * У конструкторі `this` потрібне у двох типових ситуаціях:
 *
 *   1. Параметр конструктора має таке саме ім'я, що й поле класу.
 *      Тоді `this.name` — це поле, а просто `name` — це параметр.
 *
 *   2. Один конструктор викликає інший конструктор цього ж класу
 *      через запис `: this(...)`. Так не доводиться дублювати код.
 *
 * Очікуваний вивід:
 *   Студент: Іван, вік: 20, група: КН-11
 *   Студент: Марія, вік: 17, група: КН-11
 *   Студент: Петро, вік: 17, група: без групи
 */

class Student
{
    // Поля об'єкта.
    private string name;
    private int age;
    private string group;

    // Головний конструктор: задає всі три поля.
    public Student(string name, int age, string group)
    {
        // Ліворуч — поле об'єкта (this.), праворуч — параметр конструктора.
        this.name = name;
        this.age = age;
        this.group = group;
    }

    // Конструктор без віку: викликає головний конструктор і передає вік 17.
    public Student(string name, string group)
        : this(name, 17, group)
    {
        // Тіло може бути порожнім — усю роботу вже зробив головний конструктор.
    }

    // Конструктор лише з іменем: теж перенаправляє роботу в головний конструктор.
    public Student(string name)
        : this(name, 17, "без групи")
    {
    }

    public void Print()
    {
        Console.WriteLine($"Студент: {name}, вік: {age}, група: {group}");
    }
}

class Program
{
    static void Main()
    {
        Student first = new Student("Іван", 20, "КН-11");
        Student second = new Student("Марія", "КН-11");   // вік візьметься за замовчуванням
        Student third = new Student("Петро");             // вік і група — за замовчуванням

        first.Print();
        second.Print();
        third.Print();
    }
}
