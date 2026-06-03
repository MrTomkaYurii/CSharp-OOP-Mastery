// АСОЦІАЦІЯ: Student реалізує IStudent і зберігає список IEnrollable
// (курсів), на які записаний. Він не створює ці курси і не керує їхнім
// життєвим циклом - лише посилається на них. Видалення студента ніяк не
// впливає на існування курсів, і навпаки.
namespace AssociationDemo;

public class Student : IStudent
{
    public string Name { get; }
    public string StudentId { get; }
    private readonly List<IEnrollable> enrolledCourses = new();

    public Student(string name, string studentId)
    {
        Name = name;
        StudentId = studentId;
    }

    public void EnrollInCourse(IEnrollable course)
    {
        if (!enrolledCourses.Contains(course) && course.HasAvailableSpots())
        {
            enrolledCourses.Add(course);
            // Двосторонній зв'язок, але об'єкти лишаються повністю
            // незалежними одне від одного - типова ознака асоціації.
            if (course is Course c)
                c.AddStudent(this);
        }
    }

    public void DropCourse(IEnrollable course)
    {
        if (enrolledCourses.Remove(course))
        {
            if (course is Course c)
                c.RemoveStudent(this);
        }
    }

    public List<IEnrollable> GetEnrolledCourses() => enrolledCourses;
}
