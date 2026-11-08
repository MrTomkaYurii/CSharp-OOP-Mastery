namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step1_DIP
{
    // Абстракція каналу сповіщень.
    // Дозволяє системі надсилати повідомлення через будь-який транспорт:
    // електронну пошту, SMS, Telegram, Push-повідомлення тощо.
    public interface INotificationSender
    {
        void Send(string recipient, string message);
    }
}
