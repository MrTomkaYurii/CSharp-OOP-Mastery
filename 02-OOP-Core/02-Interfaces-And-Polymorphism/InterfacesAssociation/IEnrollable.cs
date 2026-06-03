// Контракт для сутності, на яку можна записатися (курс, гурток, захід тощо).
// Student працює саме через цей інтерфейс, а не через клас Course напряму -
// в цьому й суть асоціації: два незалежні об'єкти "знають" одне про одного
// лише в межах контракту, без жорсткої прив'язки до конкретної реалізації.
namespace AssociationDemo;

public interface IEnrollable
{
    string GetInfo();
    int GetCapacity();
    bool HasAvailableSpots();
}
