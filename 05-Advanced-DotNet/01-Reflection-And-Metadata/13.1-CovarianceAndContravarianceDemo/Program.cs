namespace CovarianceAndContravarianceDemo;

#region Доменна модель: Тварини (Animal Hierarchy)

public class Animal
{
    public string Name { get; }

    public Animal(string name)
    {
        Name = name;
    }

    public virtual void MakeSound() => Console.WriteLine($"[Animal] {Name} видає загальний звук");
}

public class Dog : Animal
{
    public Dog(string name) : base(name) { }

    public override void MakeSound() => Console.WriteLine($"[Dog] {Name} гавкає: Гав-гав!");

    public void FetchBall() => Console.WriteLine($"[Dog] {Name} приніс м'ячик");
}

public class Cat : Animal
{
    public Cat(string name) : base(name) { }

    public override void MakeSound() => Console.WriteLine($"[Cat] {Name} нявчить: Мяу-мяу!");
}

#endregion

#region Узагальнені інтерфейси: Варіантність (Variance Interfaces)

// 1. Коваріантний інтерфейс (out T): тільки ДЖЕРЕЛО даних (Producer/Output)
// Ключове слово 'out' гарантує компілятору, що тип T використовується ВИКЛЮЧНО як значення, що повертається.
public interface IAnimalShelter<out T>
{
    T AdoptAnimal();
}

// 2. Контраваріантний інтерфейс (in T): тільки СПОЖИВАЧ даних (Consumer/Input)
// Ключове слово 'in' гарантує компілятору, що тип T використовується ВИКЛЮЧНО як вхідний аргумент методів.
public interface IAnimalFeeder<in T>
{
    void Feed(T animal);
}

// 3. Інваріантний інтерфейс (без out/in): двосторонній обмін (Producer + Consumer)
// Оскільки тип T і повертається, і приймається, безпечна заміна типів неможлива.
public interface IAnimalCage<T>
{
    T Resident { get; set; }
}

#endregion

#region Реалізації інтерфейсів

public class DogShelter : IAnimalShelter<Dog>
{
    private readonly string _dogName;

    public DogShelter(string dogName) => _dogName = dogName;

    public Dog AdoptAnimal() => new Dog(_dogName);
}

public class GeneralAnimalFeeder : IAnimalFeeder<Animal>
{
    public void Feed(Animal animal)
    {
        Console.WriteLine($"[Feeder]: Годуємо тварину {animal.Name} універсальним кормом.");
        animal.MakeSound();
    }
}

public class DogCage : IAnimalCage<Dog>
{
    public Dog Resident { get; set; }

    public DogCage(Dog initialDog) => Resident = initialDog;
}

#endregion

internal class Program
{
    static void Main()
    {
        Console.WriteLine("=== 13.1. Коваріантність та контраваріантність у C# (OOP Variance) ===\n");

        // -------------------------------------------------------------
        // СЕКЦІЯ 1: Проблема інваріантності (Invariance)
        // -------------------------------------------------------------
        Console.WriteLine("--- СЕКЦІЯ 1: Чому List<Dog> НЕ є List<Animal> (Небезпека інваріантності) ---");
        List<Dog> dogs = new List<Dog> { new Dog("Бім"), new Dog("Рекс") };

        // Наступний рядок НЕ скомпілюється:
        // List<Animal> animals = dogs; // CS0029: Cannot implicitly convert type 'List<Dog>' to 'List<Animal>'
        
        Console.WriteLine("Пояснення небезпеки:");
        Console.WriteLine("  Якби C# дозволив: List<Animal> animals = dogs;");
        Console.WriteLine("  Тоді хтось міг би написати: animals.Add(new Cat(\"Мурка\"));");
        Console.WriteLine("  У результаті список List<Dog> містив би кота Cat!");
        Console.WriteLine("  Виклик dogs[0].FetchBall() на коті призвів би до фатального краху в пам'яті.");
        Console.WriteLine("  ВИСНОВОК: Класи та колекції, які і читають, і записують (Read-Write), ЗАВЖДИ інваріантні.\n");

        // -------------------------------------------------------------
        // СЕКЦІЯ 2: Коваріантність (Covariance: out T)
        // -------------------------------------------------------------
        Console.WriteLine("--- СЕКЦІЯ 2: Коваріантність (out T) - Прямий напрямок сумісності ---");
        Console.WriteLine("Правило: Якщо Dog є Animal, то IAnimalShelter<Dog> є IAnimalShelter<Animal>.");
        Console.WriteLine("Модифікатор 'out' повідомляє: тип T тільки ВИХОДИТЬ з методів (Output).\n");

        IAnimalShelter<Dog> dogShelter = new DogShelter("Барбос");

        // Завдяки 'out T' дозволено неявне приведення до базового типу:
        IAnimalShelter<Animal> animalShelter = dogShelter;

        Animal adopted = animalShelter.AdoptAnimal();
        Console.WriteLine($"Успішно забрали тварину з коваріантного притулку: {adopted.Name}");
        adopted.MakeSound();

        // Стандартний BCL приклад коваріантності: IEnumerable<out T>
        IEnumerable<Dog> dogCollection = dogs;
        IEnumerable<Animal> animalCollection = dogCollection; // Безпечно, бо з IEnumerable можна ТІЛЬКИ ЧИТАТИ!
        Console.WriteLine($"Коваріантний перелік IEnumerable<Animal>: {animalCollection.Count()} елементів.\n");

        // -------------------------------------------------------------
        // СЕКЦІЯ 3: Контраваріантність (Contravariance: in T)
        // -------------------------------------------------------------
        Console.WriteLine("--- СЕКЦІЯ 3: Контраваріантність (in T) - Зворотний напрямок сумісності ---");
        Console.WriteLine("Правило: Якщо метод вміє приймати БУДЬ-ЯКУ Animal, він гарантовано впорається з Dog.");
        Console.WriteLine("Модифікатор 'in' повідомляє: тип T тільки ВХОДИТЬ у методи як аргумент (Input).\n");

        IAnimalFeeder<Animal> universalFeeder = new GeneralAnimalFeeder();

        // Завдяки 'in T' ми можемо використати універсальну годівницю для конкретних собак:
        IAnimalFeeder<Dog> dogFeeder = universalFeeder;

        Dog myDog = new Dog("Хатіко");
        dogFeeder.Feed(myDog);

        // Стандартний BCL приклад контраваріантності: Action<in T>
        Action<Animal> inspectAnimal = a => Console.WriteLine($"[Ветеринарний огляд]: Пацієнт {a.Name}");
        Action<Dog> inspectDog = inspectAnimal; // Зворотне приведення від Animal до Dog!
        inspectDog(myDog);
        Console.WriteLine();

        // -------------------------------------------------------------
        // СЕКЦІЯ 4: Підсумкова таблиця правил варіантності
        // -------------------------------------------------------------
        Console.WriteLine("--- СЕКЦІЯ 4: Фундаментальні правила варіантності в .NET ---");
        Console.WriteLine("1. Застосовується ЛИШЕ до інтерфейсів та делегатів (не до класів чи структур).");
        Console.WriteLine("2. Застосовується ЛИШЕ до посилальних типів (Reference Types). Примітиви не підтримуються.");
        Console.WriteLine("3. 'out T' (Коваріантність): T повертається з методів, сумісність: Derived -> Base.");
        Console.WriteLine("4. 'in T'  (Контраваріантність): T приймається у параметри, сумісність: Base -> Derived.");
        Console.WriteLine("5. Відсутність out/in (Інваріантність): сумісність лише за точним збігом типів.");
    }
}
