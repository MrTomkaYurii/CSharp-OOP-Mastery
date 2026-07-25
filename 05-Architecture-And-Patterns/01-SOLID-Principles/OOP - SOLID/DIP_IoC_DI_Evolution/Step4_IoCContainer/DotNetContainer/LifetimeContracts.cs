using System;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step4_IoCContainer.DotNetContainer
{
    // Базовий інтерфейс операції з унікальним ідентифікатором екземпляра
    public interface IOperation
    {
        Guid Id { get; }
        string LifetimeName { get; }
    }

    // Інтерфейс сервісу з часом життя Transient (створюється новий щоразу)
    public interface ITransientOperation : IOperation { }

    // Інтерфейс сервісу з часом життя Scoped (один екземпляр на Scope/Запит)
    public interface IScopedOperation : IOperation { }

    // Інтерфейс сервісу з часом життя Singleton (один екземпляр на весь додаток)
    public interface ISingletonOperation : IOperation { }

    // Універсальна реалізація для демонстрації життєвих циклів
    public class DefaultOperation : ITransientOperation, IScopedOperation, ISingletonOperation
    {
        public Guid Id { get; }
        public string LifetimeName { get; }

        public DefaultOperation(string lifetimeName)
        {
            Id = Guid.NewGuid();
            LifetimeName = lifetimeName;
        }

        public DefaultOperation() : this("Unknown")
        {
        }
    }
}
