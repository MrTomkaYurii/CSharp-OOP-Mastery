using System.Reflection;

namespace DependencyInjection
{
    /// <summary>
    /// Життєвий цикл сервісу в DI-контейнері.
    /// </summary>
    public enum ServiceLifetime
    {
        /// <summary>
        /// Новий екземпляр створюється при кожному запиті.
        /// </summary>
        Transient,

        /// <summary>
        /// Єдиний екземпляр створюється один раз і перевикористовується для всіх наступних запитів.
        /// </summary>
        Singleton
    }

    /// <summary>
    /// Атрибут для маркування пріоритетного конструктора при наявності кількох перевантажень.
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor)]
    public sealed class InjectConstructorAttribute : Attribute { }

    /// <summary>
    /// Атрибут для ін'єкції залежності у властивість (Property Injection).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class InjectPropertyAttribute : Attribute { }

    /// <summary>
    /// Виняток, що викидається при виявленні циклічних залежностей (запобігає StackOverflowException).
    /// </summary>
    public sealed class CircularDependencyException : Exception
    {
        public CircularDependencyException(string message) : base(message) { }
    }

    /// <summary>
    /// Опис зареєстрованого сервісу.
    /// </summary>
    internal sealed class ServiceDescriptor
    {
        public Type ServiceType { get; }
        public Type? ImplementationType { get; }
        public ServiceLifetime Lifetime { get; }
        public Func<Container, object>? Factory { get; }
        public object? SingletonInstance { get; set; }

        public ServiceDescriptor(Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            ServiceType = serviceType;
            ImplementationType = implementationType;
            Lifetime = lifetime;
        }

        public ServiceDescriptor(Type serviceType, Func<Container, object> factory, ServiceLifetime lifetime)
        {
            ServiceType = serviceType;
            Factory = factory;
            Lifetime = lifetime;
        }

        public ServiceDescriptor(Type serviceType, object instance)
        {
            ServiceType = serviceType;
            SingletonInstance = instance;
            Lifetime = ServiceLifetime.Singleton;
        }
    }

    /// <summary>
    /// Повнофункціональний навчальний IoC/DI контейнер на базі рефлексії.
    /// Демонструє аналіз конструкторів, розв'язання параметрів, життєві цикли,
    /// property injection та захист від циклічних залежностей.
    /// </summary>
    public class Container
    {
        private readonly Dictionary<Type, ServiceDescriptor> _registrations = new();
        private readonly Stack<Type> _resolutionStack = new();
        private int _treeDepth;

        #region Registration API

        public void RegisterTransient<TService, TImplementation>() where TImplementation : class, TService
        {
            Register(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient);
        }

        public void RegisterTransient<TService>() where TService : class
        {
            Register(typeof(TService), typeof(TService), ServiceLifetime.Transient);
        }

        public void RegisterSingleton<TService, TImplementation>() where TImplementation : class, TService
        {
            Register(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton);
        }

        public void RegisterSingleton<TService>() where TService : class
        {
            Register(typeof(TService), typeof(TService), ServiceLifetime.Singleton);
        }

        public void RegisterInstance<TService>(TService instance) where TService : class
        {
            ArgumentNullException.ThrowIfNull(instance);
            _registrations[typeof(TService)] = new ServiceDescriptor(typeof(TService), instance);
            Console.WriteLine($"  [Container] Зареєстровано екземпляр: {typeof(TService).Name} (Вже створений Singleton)");
        }

        public void RegisterFactory<TService>(Func<Container, TService> factory, ServiceLifetime lifetime = ServiceLifetime.Transient) where TService : class
        {
            ArgumentNullException.ThrowIfNull(factory);
            _registrations[typeof(TService)] = new ServiceDescriptor(typeof(TService), c => factory(c), lifetime);
            Console.WriteLine($"  [Container] Зареєстровано фабрику: {typeof(TService).Name} ({lifetime})");
        }

        private void Register(Type serviceType, Type implementationType, ServiceLifetime lifetime)
        {
            if ((serviceType.IsInterface || serviceType.IsAbstract) && !serviceType.IsAssignableFrom(implementationType))
            {
                throw new InvalidOperationException($"Тип {implementationType.Name} не реалізує {serviceType.Name}");
            }

            if (implementationType.IsInterface || implementationType.IsAbstract)
            {
                throw new InvalidOperationException($"Тип реалізації {implementationType.Name} не може бути інтерфейсом чи абстрактним класом.");
            }

            _registrations[serviceType] = new ServiceDescriptor(serviceType, implementationType, lifetime);
            Console.WriteLine($"  [Container] Зареєстровано: {serviceType.Name} -> {implementationType.Name} [{lifetime}]");
        }

        #endregion

        #region Resolution API

        public TService Resolve<TService>()
        {
            return (TService)Resolve(typeof(TService));
        }

        public object Resolve(Type serviceType)
        {
            // 1. Перевірка на циклічну залежність
            if (_resolutionStack.Contains(serviceType))
            {
                var cycleList = _resolutionStack.Reverse().Concat(new[] { serviceType }).Select(t => t.Name);
                string chain = string.Join(" ──> ", cycleList);
                throw new CircularDependencyException($"Виявлено циклічну залежність! Ланцюжок: {chain}");
            }

            // 2. Отримання дескриптора або реєстрація конкретного класу on-the-fly
            if (!_registrations.TryGetValue(serviceType, out ServiceDescriptor? descriptor))
            {
                if (serviceType.IsClass && !serviceType.IsAbstract)
                {
                    descriptor = new ServiceDescriptor(serviceType, serviceType, ServiceLifetime.Transient);
                }
                else
                {
                    throw new InvalidOperationException($"Сервіс типу '{serviceType.FullName}' не зареєстровано у контейнері.");
                }
            }

            // 3. Якщо це Singleton і екземпляр уже існує — повертаємо кешований
            if (descriptor.Lifetime == ServiceLifetime.Singleton && descriptor.SingletonInstance != null)
            {
                PrintResolutionLog(serviceType, descriptor.SingletonInstance.GetType(), isCached: true);
                return descriptor.SingletonInstance;
            }

            _resolutionStack.Push(serviceType);
            _treeDepth++;

            try
            {
                object instance;

                // 4. Створення через фабрику або через рефлексію
                if (descriptor.Factory != null)
                {
                    PrintResolutionLog(serviceType, null, isFactory: true);
                    instance = descriptor.Factory(this);
                }
                else
                {
                    Type implType = descriptor.ImplementationType!;
                    PrintResolutionLog(serviceType, implType, isCached: false);

                    // Створення екземпляра через конструктор з розв'язанням залежностей
                    instance = CreateInstanceViaReflection(implType);

                    // 5. Property Injection для властивостей з [InjectProperty]
                    InjectAnnotatedProperties(instance, implType);
                }

                // 6. Якщо це Singleton — зберігаємо в кеш
                if (descriptor.Lifetime == ServiceLifetime.Singleton)
                {
                    descriptor.SingletonInstance = instance;
                }

                return instance;
            }
            finally
            {
                _treeDepth--;
                _resolutionStack.Pop();
            }
        }

        #endregion

        #region Reflection Engine Internals

        private object CreateInstanceViaReflection(Type implType)
        {
            ConstructorInfo[] constructors = implType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);
            if (constructors.Length == 0)
            {
                throw new InvalidOperationException($"Клас '{implType.FullName}' не має жодного публічного конструктора.");
            }

            // Вибір конструктора:
            // А. Якщо є конструктор з атрибутом [InjectConstructor] - обираємо його
            // Б. Інакше обираємо конструктор з найбільшою кількістю параметрів (жадібний вибір)
            ConstructorInfo selectedCtor = constructors.FirstOrDefault(c => c.GetCustomAttribute<InjectConstructorAttribute>() != null)
                                          ?? constructors.OrderByDescending(c => c.GetParameters().Length).First();

            ParameterInfo[] parameters = selectedCtor.GetParameters();
            object[] ctorArgs = new object[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                ParameterInfo param = parameters[i];
                Type paramType = param.ParameterType;

                if (!_registrations.ContainsKey(paramType) && param.HasDefaultValue)
                {
                    // Якщо параметр має значення за замовчуванням і явно не зареєстрований — використовуємо default
                    ctorArgs[i] = param.DefaultValue!;
                }
                else
                {
                    try
                    {
                        // Рекурсивне розв'язання параметра
                        ctorArgs[i] = Resolve(paramType);
                    }
                    catch when (param.HasDefaultValue)
                    {
                        // Якщо виникла помилка, але параметр необов'язковий — підставляємо default
                        ctorArgs[i] = param.DefaultValue!;
                    }
                }
            }

            return selectedCtor.Invoke(ctorArgs);
        }

        private void InjectAnnotatedProperties(object instance, Type implType)
        {
            PropertyInfo[] properties = implType.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in properties)
            {
                if (prop.GetCustomAttribute<InjectPropertyAttribute>() != null && prop.CanWrite)
                {
                    object dependency = Resolve(prop.PropertyType);
                    prop.SetValue(instance, dependency);
                }
            }
        }

        private void PrintResolutionLog(Type serviceType, Type? implType, bool isCached = false, bool isFactory = false)
        {
            int depth = Math.Max(0, _treeDepth - 1);
            string indent = new string(' ', depth * 3);
            string marker = _treeDepth <= 1 ? "📦 " : "└── ";

            if (isCached)
            {
                Console.WriteLine($"{indent}{marker}[КЕШ] {serviceType.Name} (Singleton повторно використано)");
            }
            else if (isFactory)
            {
                Console.WriteLine($"{indent}{marker}[ФАБРИКА] {serviceType.Name}");
            }
            else
            {
                string implInfo = implType != null && implType != serviceType ? $" -> {implType.Name}" : "";
                Console.WriteLine($"{indent}{marker}[СТВОРЕННЯ] {serviceType.Name}{implInfo}");
            }
        }

        #endregion
    }
}
