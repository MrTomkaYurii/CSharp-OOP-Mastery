using System.Reflection;

namespace VarianceReflectionInspectionDemo;

#region Доменна ієрархія та інтерфейси для тестування варіантності

public class Animal { public string Name { get; set; } = "Unknown"; }
public class Dog : Animal { public string Breed { get; set; } = "Retriever"; }
public class Cat : Animal { public bool IsLazy { get; set; } = true; }

// 1. Коваріантний інтерфейс (out T)
public interface IProducer<out T>
{
    T Produce();
}

// 2. Контраваріантний інтерфейс (in T)
public interface IConsumer<in T>
{
    void Consume(T item);
}

// 3. Інваріантний інтерфейс (без модифікаторів)
public interface IStorage<T>
{
    T Value { get; set; }
}

// 4. Відкритий тип з обмеженнями (Constraints)
public interface IRepository<T> where T : class, new()
{
    T GetById(int id);
}

#endregion

#region Практичний кейс: Диспетчеризація подій (Event Bus) через контраваріантність

public abstract class DomainEvent { public DateTime OccurredOn { get; } = DateTime.UtcNow; }
public class OrderCreatedEvent : DomainEvent { public int OrderId { get; set; } = 42; }

// Контраваріантний приймач/обробник подій (in TEvent)
public interface IEventReceiver<in TEvent> where TEvent : DomainEvent
{
    void Handle(TEvent @event);
}

// Спеціалізований обробник для конкретної події створення замовлення
public class OrderCreatedHandler : IEventReceiver<OrderCreatedEvent>
{
    public void Handle(OrderCreatedEvent @event) =>
        Console.WriteLine($"[OrderCreatedHandler]: Оброблено подію для замовлення #{@event.OrderId}");
}

// Універсальний аудиторський обробник для БУДЬ-ЯКОЇ доменної події
public class AuditLogHandler : IEventReceiver<DomainEvent>
{
    public void Handle(DomainEvent @event) =>
        Console.WriteLine($"[AuditLogHandler]: Зафіксовано подію типу {@event.GetType().Name} о {@event.OccurredOn:HH:mm:ss}");
}

#endregion

internal class Program
{
    static void Main()
    {
        Console.WriteLine("=== 13.2. Інспекція коваріантності та контраваріантності через рефлексію ===\n");

        // -------------------------------------------------------------
        // СЕКЦІЯ 1: Зчитування прапорців варіантності з метаданих CLR
        // -------------------------------------------------------------
        Console.WriteLine("--- СЕКЦІЯ 1: Аналіз GenericParameterAttributes для узагальнених типів ---");
        InspectVarianceAttributes(typeof(IProducer<>));
        InspectVarianceAttributes(typeof(IConsumer<>));
        InspectVarianceAttributes(typeof(IStorage<>));
        InspectVarianceAttributes(typeof(IRepository<>));

        // -------------------------------------------------------------
        // СЕКЦІЯ 2: Перевірка динамічної сумісності типів через IsAssignableFrom
        // -------------------------------------------------------------
        Console.WriteLine("--- СЕКЦІЯ 2: Динамічна перевірка сумісності (Type.IsAssignableFrom) ---");
        CheckCovarianceAssignment();
        CheckContravarianceAssignment();
        CheckInvarianceAssignment();

        // -------------------------------------------------------------
        // СЕКЦІЯ 3: Практичний кейс рефлексії - Диспетчеризація контраваріантних обробників
        // -------------------------------------------------------------
        Console.WriteLine("--- СЕКЦІЯ 3: Пошук контраваріантних обробників подій через рефлексію ---");
        DispatchOrderEvent(new OrderCreatedEvent { OrderId = 1001 });
    }

    /// <summary>
    /// Інспектує параметр узагальнення, зчитуючи атрибути коваріантності, контраваріантності та обмежень.
    /// </summary>
    private static void InspectVarianceAttributes(Type genericTypeDefinition)
    {
        Console.WriteLine($"Аналіз типу: {genericTypeDefinition.Name}");

        Type[] typeParameters = genericTypeDefinition.GetGenericArguments();
        foreach (Type param in typeParameters)
        {
            GenericParameterAttributes attrs = param.GenericParameterAttributes;

            // Виділяємо бітову маску варіантності (перші 2 біти)
            GenericParameterAttributes varianceMask = attrs & GenericParameterAttributes.VarianceMask;

            string varianceDescription = varianceMask switch
            {
                GenericParameterAttributes.Covariant => "Коваріантний (Covariant: out T)",
                GenericParameterAttributes.Contravariant => "Контраваріантний (Contravariant: in T)",
                _ => "Інваріантний (Invariant: без out/in)"
            };

            Console.WriteLine($"  Параметр:          {param.Name}");
            Console.WriteLine($"  Сирі атрибути:     {attrs} (Числове значення: {(int)attrs})");
            Console.WriteLine($"  Тип варіантності:  {varianceDescription}");

            // Перевірка додаткових обмежень (Special Constraints)
            if ((attrs & GenericParameterAttributes.ReferenceTypeConstraint) != 0)
            {
                Console.WriteLine("  Обмеження:         where T : class");
            }
            if ((attrs & GenericParameterAttributes.DefaultConstructorConstraint) != 0)
            {
                Console.WriteLine("  Обмеження:         where T : new()");
            }

            // Інспекція обмежень на базові типи та інтерфейси
            Type[] constraints = param.GetGenericParameterConstraints();
            if (constraints.Length > 0)
            {
                string constraintNames = string.Join(", ", constraints.Select(c => c.Name));
                Console.WriteLine($"  Типові обмеження:  {constraintNames}");
            }
        }
        Console.WriteLine();
    }

    private static void CheckCovarianceAssignment()
    {
        Type producerDog = typeof(IProducer<Dog>);
        Type producerAnimal = typeof(IProducer<Animal>);

        bool canAssignDirect = producerAnimal.IsAssignableFrom(producerDog);
        bool canAssignReverse = producerDog.IsAssignableFrom(producerAnimal);

        Console.WriteLine("Коваріантність (IProducer<out T>):");
        Console.WriteLine($"  typeof(IProducer<Animal>).IsAssignableFrom(typeof(IProducer<Dog>))   -> {canAssignDirect}  [Пряма сумісність]");
        Console.WriteLine($"  typeof(IProducer<Dog>).IsAssignableFrom(typeof(IProducer<Animal>))   -> {canAssignReverse} [Зворотна заборонена]\n");
    }

    private static void CheckContravarianceAssignment()
    {
        Type consumerDog = typeof(IConsumer<Dog>);
        Type consumerAnimal = typeof(IConsumer<Animal>);

        bool canAssignDirect = consumerAnimal.IsAssignableFrom(consumerDog);
        bool canAssignReverse = consumerDog.IsAssignableFrom(consumerAnimal);

        Console.WriteLine("Контраваріантність (IConsumer<in T>):");
        Console.WriteLine($"  typeof(IConsumer<Dog>).IsAssignableFrom(typeof(IConsumer<Animal>))   -> {canAssignReverse} [Зворотна сумісність: загальний обробник підходить для вузького типу]");
        Console.WriteLine($"  typeof(IConsumer<Animal>).IsAssignableFrom(typeof(IConsumer<Dog>))   -> {canAssignDirect} [Пряма заборонена: вузький обробник не знає інших тварин]\n");
    }

    private static void CheckInvarianceAssignment()
    {
        Type storageDog = typeof(IStorage<Dog>);
        Type storageAnimal = typeof(IStorage<Animal>);

        bool canAssign = storageAnimal.IsAssignableFrom(storageDog);

        Console.WriteLine("Інваріантність (IStorage<T>):");
        Console.WriteLine($"  typeof(IStorage<Animal>).IsAssignableFrom(typeof(IStorage<Dog>))     -> {canAssign} [Інваріантність блокує будь-яке непряме приведення]\n");
    }

    /// <summary>
    /// Демонструє, як архітектурний Event Bus за допомогою рефлексії знаходить
    /// усі контраваріантні обробники для конкретної події.
    /// </summary>
    private static void DispatchOrderEvent(OrderCreatedEvent orderEvent)
    {
        Console.WriteLine($"Диспетчеризація події: {orderEvent.GetType().Name} (OrderId = {orderEvent.OrderId})");

        // Список усіх реалізованих типів обробників у системі
        Type[] handlerTypes = new[]
        {
            typeof(OrderCreatedHandler),
            typeof(AuditLogHandler)
        };

        // Цільовий інтерфейс обробника для поточної події:
        Type targetHandlerInterface = typeof(IEventReceiver<OrderCreatedEvent>);

        Console.WriteLine($"Шукаємо типи, сумісні з інтерфейсом: {targetHandlerInterface.Name}<{typeof(OrderCreatedEvent).Name}>");

        foreach (Type handlerType in handlerTypes)
        {
            // Завдяки контраваріантності (in TEvent), тип AuditLogHandler (який реалізує IEventReceiver<DomainEvent>)
            // є assignable до IEventReceiver<OrderCreatedEvent>!
            bool isCompatible = targetHandlerInterface.IsAssignableFrom(handlerType);

            Console.WriteLine($"  Перевірка {handlerType.Name}: IsAssignableFrom = {isCompatible}");

            if (isCompatible)
            {
                // Створюємо екземпляр обробника через рефлексію
                object handlerInstance = Activator.CreateInstance(handlerType)!;

                // Знаходимо та викликаємо метод Handle через рефлексію
                MethodInfo handleMethod = handlerType.GetMethod("Handle")!;
                handleMethod.Invoke(handlerInstance, new object[] { orderEvent });
            }
        }
        Console.WriteLine();
    }
}
