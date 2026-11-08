using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OOP___SOLID.DIP_IoC_DI_Evolution.Step4_IoCContainer.MiniContainer
{
    // Навчальний міні-контейнер інверсії контролю (Educational Mini IoC Container).
    // Покликаний показати студенту, що IoC-контейнер — це НЕ "магія",
    // а всього близько 40 рядків коду на базі C# Reflection (Рефлексії):
    // 1. Словник реєстрації типів (Type -> Type).
    // 2. Словник збережених сінглтонів (Type -> object).
    // 3. Рекурсивний обхід параметрів конструктора (Constructor Injection Resolver).
    public class SimpleContainer
    {
        private readonly Dictionary<Type, Type> _registrations = new();
        private readonly Dictionary<Type, object> _singletons = new();
        private readonly HashSet<Type> _singletonTypes = new();

        // Реєстрація звичайного сервісу (Transient: створюється новий об'єкт при кожному запиті)
        public void Register<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _registrations[typeof(TInterface)] = typeof(TImplementation);
        }

        // Реєстрація сінглтона (Singleton: один об'єкт створюється один раз і кешується)
        public void RegisterSingleton<TInterface, TImplementation>() where TImplementation : TInterface
        {
            _registrations[typeof(TInterface)] = typeof(TImplementation);
            _singletonTypes.Add(typeof(TInterface));
        }

        // Автоматичне вирішення (Resolve) графа залежностей за типом T
        public T Resolve<T>()
        {
            return (T)Resolve(typeof(T));
        }

        // Рекурсивний резолвер типів
        public object Resolve(Type serviceType)
        {
            // Якщо це сінглтон і він вже створений — повертаємо готовий екземпляр
            if (_singletonTypes.Contains(serviceType) && _singletons.TryGetValue(serviceType, out var existingSingleton))
            {
                return existingSingleton;
            }

            // Знаходимо тип реалізації (якщо зареєстровано інтерфейс — беремо клас, якщо передано сам клас — беремо його)
            Type implementationType = _registrations.TryGetValue(serviceType, out var registeredType)
                ? registeredType
                : serviceType;

            if (implementationType.IsInterface || implementationType.IsAbstract)
            {
                throw new InvalidOperationException($"Не вдалося створити екземпляр абстракції або незареєстрованого інтерфейсу: {implementationType.Name}");
            }

            // Отримуємо перший публічний конструктор через рефлексію
            ConstructorInfo constructor = implementationType.GetConstructors()
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault()
                ?? throw new InvalidOperationException($"Клас {implementationType.Name} не має доступних публічних конструкторів");

            // Рекурсивно вирішуємо залежності для кожного параметра конструктора
            ParameterInfo[] parameters = constructor.GetParameters();
            object[] parameterInstances = parameters
                .Select(p => Resolve(p.ParameterType))
                .ToArray();

            // Створюємо екземпляр об'єкта, передаючи resolved-параметри
            object createdInstance = Activator.CreateInstance(implementationType, parameterInstances)!;

            // Якщо зареєстровано як сінглтон — кешуємо
            if (_singletonTypes.Contains(serviceType))
            {
                _singletons[serviceType] = createdInstance;
            }

            return createdInstance;
        }
    }
}
