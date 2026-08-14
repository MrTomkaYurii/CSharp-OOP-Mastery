using System;
using System.Collections.Generic;
using System.Reflection;

namespace DependencyInjection
{
    public class Container
    {
        // Зберігаємо залежності: ключ — інтерфейс/базовий клас, значення — реалізація
        private readonly Dictionary<Type, Type> _containerData = new Dictionary<Type, Type>();

        /// <summary>
        /// Реєстрація залежності: інтерфейс/базовий клас → конкретний клас
        /// </summary>
        public void Register<TType, TImplementation>()
        {
            var type = typeof(TType);
            var impl = typeof(TImplementation);

            // Перевірка для інтерфейсів та абстрактних класів
            if ((type.IsInterface || type.IsAbstract) && !type.IsAssignableFrom(impl))
                throw new InvalidOperationException($"{impl.Name} не реалізує {type.Name}");

            _containerData[type] = impl;

            Console.WriteLine($"✅ Зареєстровано: {type.Name} → {impl.Name}");
        }

        /// <summary>
        /// Отримати екземпляр TType з усіма залежностями
        /// </summary>
        public TType GetInstance<TType>()
        {
            return (TType)GetInstance(typeof(TType));
        }

        /// <summary>
        /// Рекурсивне створення екземпляра
        /// </summary>
        private object GetInstance(Type type)
        {
            Type implementationType;

            // Якщо тип зареєстрований, беремо реалізацію
            if (_containerData.ContainsKey(type))
            {
                implementationType = _containerData[type];
            }
            else
            {
                // Якщо це клас, який не зареєстрований, беремо його як сам себе
                if (type.IsClass)
                    implementationType = type;
                else
                    throw new InvalidOperationException($"Тип {type.FullName} не зареєстрований у контейнері.");
            }

            // Беремо єдиний конструктор
            ConstructorInfo[] constructors = implementationType.GetConstructors();
            if (constructors.Length != 1)
                throw new InvalidOperationException($"Тип {implementationType.FullName} повинен мати лише один конструктор.");

            ConstructorInfo ctor = constructors[0];
            ParameterInfo[] parameters = ctor.GetParameters();
            object[] args = new object[parameters.Length];

            // Рекурсивно створюємо залежності
            for (int i = 0; i < parameters.Length; i++)
            {
                Type paramType = parameters[i].ParameterType;
                args[i] = GetInstance(paramType);
            }

            // Створюємо екземпляр
            return ctor.Invoke(args);
        }

        /// <summary>
        /// Вивід усіх зареєстрованих залежностей
        /// </summary>
        public void PrintRegistrations()
        {
            Console.WriteLine("📦 Зареєстровані залежності:");
            foreach (var pair in _containerData)
                Console.WriteLine($"  {pair.Key.Name} → {pair.Value.Name}");
        }
    }
}
