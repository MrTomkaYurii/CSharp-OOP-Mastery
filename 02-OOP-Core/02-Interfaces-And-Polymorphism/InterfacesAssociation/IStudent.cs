// Контракт студента з погляду курсу: курсу байдуже, як саме реалізований
// студент - головне, що він уміє записуватися/відписуватися від курсів
// і звітувати про свій список.
namespace AssociationDemo;

public interface IStudent
{
    string Name { get; }
    string StudentId { get; }
    void EnrollInCourse(IEnrollable course);
    void DropCourse(IEnrollable course);
    List<IEnrollable> GetEnrolledCourses();
}
