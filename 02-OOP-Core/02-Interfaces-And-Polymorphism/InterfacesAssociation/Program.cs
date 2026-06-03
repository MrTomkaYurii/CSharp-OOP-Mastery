// АСОЦІАЦІЯ (Association) - найслабший тип зв'язку між об'єктами: вони
// просто "знають" одне про одного через посилання, але існують повністю
// незалежно. Якщо видалити студента - курси залишаться; якщо видалити
// курс - студенти залишаться. Жоден об'єкт не володіє іншим і не керує
// його життєвим циклом.
//
// НАВІЩО ТУТ ІНТЕРФЕЙСИ: Student працює з типом IEnrollable, а не з
// конкретним класом Course. Завтра можна додати інший тип "того, на що
// записуються" (гурток, тренінг, вебінар) - і Student продовжить
// працювати без жодної зміни свого коду, аби новий клас реалізовував
// IEnrollable.
namespace AssociationDemo;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== АСОЦІАЦІЯ: Студенти і курси ===\n");

        var csharpCourse = new Course("Основи C#", "CS101", 2);
        var pythonCourse = new Course("Python для початківців", "PY101", 3);
        var javaCourse = new Course("Java Enterprise", "JV201", 2);

        var student1 = new Student("Олена Коваль", "STD001");
        var student2 = new Student("Максим Петренко", "STD002");
        var student3 = new Student("Анна Шевченко", "STD003");

        Console.WriteLine("Реєстрація студентів на курси:");
        student1.EnrollInCourse(csharpCourse);
        student1.EnrollInCourse(pythonCourse);
        student2.EnrollInCourse(csharpCourse);
        student2.EnrollInCourse(javaCourse);
        student3.EnrollInCourse(pythonCourse);

        Console.WriteLine($"\nКурси студента {student1.Name}:");
        foreach (var course in student1.GetEnrolledCourses())
            Console.WriteLine($"  - {course.GetInfo()}");

        Console.WriteLine("\nВідрахування з курсу:");
        student1.DropCourse(pythonCourse);

        Console.WriteLine("\nСтатистика курсів:");
        Console.WriteLine($"  {csharpCourse.GetInfo()}: {csharpCourse.GetEnrolledCount()}/{csharpCourse.GetCapacity()}");
        Console.WriteLine($"  {pythonCourse.GetInfo()}: {pythonCourse.GetEnrolledCount()}/{pythonCourse.GetCapacity()}");
        Console.WriteLine($"  {javaCourse.GetInfo()}: {javaCourse.GetEnrolledCount()}/{javaCourse.GetCapacity()}");

        // Ключовий момент асоціації: жоден з об'єктів не знищується разом
        // з іншим - вони існують у програмі повністю незалежно.
        Console.WriteLine("\nЯкщо видалити студента, курси залишаться.");
        Console.WriteLine("Якщо видалити курс, студенти залишаться.");
    }
}
