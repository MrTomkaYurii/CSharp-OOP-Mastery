namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP
{
    // Абстракція аудиту та логування.
    // Захищає бізнес-логіку від прив'язки до системного виводу, файлів чи сторонніх сервісів моніторингу.
    public interface ILogger
    {
        void LogInfo(string message);
        void LogError(string message);
    }
}
