// Course реалізує IEnrollable - для студента це просто "щось, на що можна
// записатися". АСОЦІАЦІЯ: Course і Student - два рівноправні незалежні
// об'єкти. Жоден з них не володіє іншим; кожен лише зберігає посилання
// (список) на пов'язані об'єкти протилежного боку.
namespace AssociationDemo;

public class Course : IEnrollable
{
    public string Name { get; }
    public string Code { get; }
    public int Capacity { get; }
    private readonly List<IStudent> enrolledStudents = new();

    public Course(string name, string code, int capacity)
    {
        Name = name;
        Code = code;
        Capacity = capacity;
    }

    public string GetInfo() => $"{Code}: {Name}";
    public int GetCapacity() => Capacity;
    public bool HasAvailableSpots() => enrolledStudents.Count < Capacity;

    public void AddStudent(IStudent student)
    {
        if (HasAvailableSpots())
        {
            enrolledStudents.Add(student);
            Console.WriteLine($"  Зараховано: {student.Name} на курс {Name}");
        }
        else
        {
            Console.WriteLine($"  Немає вільних місць на курсі {Name}");
        }
    }

    public void RemoveStudent(IStudent student)
    {
        if (enrolledStudents.Remove(student))
            Console.WriteLine($"  Відраховано: {student.Name} з курсу {Name}");
    }

    public int GetEnrolledCount() => enrolledStudents.Count;
}
