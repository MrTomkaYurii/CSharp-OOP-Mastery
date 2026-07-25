namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP
{
    // Абстракція репозиторію користувачів.
    // Високорівневий модуль не знає про SQL, MongoDB чи файлову систему,
    // він знає лише про контракт збереження та пошуку даних.
    public interface IUserRepository
    {
        void Save(string userId, string userName, string email);
        string? GetById(string userId);
    }
}
