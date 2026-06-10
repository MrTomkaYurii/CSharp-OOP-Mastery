// ============================================================================
//  ЯК ВІДТВОРИТИ ПОМИЛКУ КОМПІЛЯЦІЇ:
//  розкоментуйте ОДИН рядок нижче (прибрати "// ") і зберіть проєкт.
//  Після перегляду помилки закоментуйте рядок назад.
// ============================================================================
// #define ERR_INVARIANT_LIST
// #define ERR_CONSTRAINT
// #define ERR_NO_MEMBER
// #define ERR_NEW_T
// #define ERR_COMPARE
// #define ERR_VARIANCE_DECL
// #define ERR_VARIANCE_DIRECTION
// #define ERR_INFERENCE
// #define ERR_REPO_INVARIANT

using GenericsPitfalls.Infrastructure;
using GenericsPitfalls.Interfaces;
using GenericsPitfalls.Models;

namespace GenericsPitfalls.Demos;

// Помилки, які компілятор ловить сам. Правильний код тут працює;
// хибний захований за #if і вмикається символом зверху файлу.
static class CompileErrors
{
    // 10. Списки інваріантні -----------------------------------------------------------
    public static void ListInvariance()
    {
        Ui.Title("10. List<Dog> не є List<Animal>");
        List<Dog> dogs = new List<Dog>();
        dogs.Add(new Dog { Id = 1, Name = "Рекс" });
        dogs.Add(new Dog { Id = 2, Name = "Бім" });

#if ERR_INVARIANT_LIST
        // Чому так: якби це присвоєння дозволили, крізь змінну animals у "список
        // собак" вдалося б додати Cat, а зчитування dogs[0] могло б повернути кота.
        // List<T> водночас і приймає T (Add), і віддає його (індексатор), тому
        // безпечним є лише точний збіг типу — список інваріантний.
        List<Animal> animals = dogs;                        // CS0029
#endif
        Ui.CompileError("ERR_INVARIANT_LIST",
            "List<Animal> animals = dogs;",
            "CS0029: Cannot implicitly convert type 'List<Dog>' to 'List<Animal>'");
        Ui.Info("  Чому: якби це було дозволено, у animals можна було б додати Cat — і 'список собак' містив би кота.");

        Ui.Fix("IEnumerable<T> тільки віддає елементи (out T), тому тут коваріантність безпечна.");
        IEnumerable<Animal> readOnlyAnimals = dogs;
        foreach (Animal animal in readOnlyAnimals)
            Ui.Good($"  {animal}");
    }

    // 11. Порушення обмеження ---------------------------------------------------------
    public static void ConstraintViolation()
    {
        Ui.Title("11. Аргумент типу не відповідає обмеженню where");

#if ERR_CONSTRAINT
        // Чому так: where T : class, IEntity — це договір, який дозволяє компілятору
        // перевірити, що код усередині InMemoryRepository<T> використовує лише
        // доступні можливості T. string є class, але не реалізує IEntity — звідси
        // CS0311; для int (значущий тип) буде CS0452, бо не виконано where T : class.
        InMemoryRepository<string> broken = new InMemoryRepository<string>();  // CS0311
#endif
        Ui.CompileError("ERR_CONSTRAINT",
            "new InMemoryRepository<string>();",
            "CS0311: The type 'string' cannot be used as type parameter 'T' ... There is no implicit reference conversion from 'string' to 'IEntity'");
        Ui.Info("  Для int замість string буде CS0452 (int не є посилальним типом, а вимагається class).");

        Ui.Fix("підставляти тип, що відповідає where T : class, IEntity.");
        InMemoryRepository<Dog> repo = new InMemoryRepository<Dog>();
        repo.Add(new Dog { Id = 1, Name = "Рекс" });
        Ui.Good($"  Репозиторій собак створено, у ньому {new List<Dog>(repo.GetAll()).Count} запис");
    }

    // 12. Звернення до члена T без обмеження ---------------------------------------------
#if ERR_NO_MEMBER
    static string BrokenName<T>(T item)
    {
        // Чому так: без обмежень T для компілятора — це "будь-що", про яке відомо
        // лише те, що є в object (ToString, Equals, GetHashCode, GetType). Замість T
        // можуть підставити, наприклад, int, у якого властивості Name немає, тож
        // звернення item.Name компілятор відхиляє одразу, ще на етапі компіляції.
        return item.Name;                                   // CS1061
    }
#endif

    static string GetName<T>(T item) where T : Animal       // обмеження відкриває члени Animal
    {
        return item.Name;
    }

    public static void NoMemberWithoutConstraint()
    {
        Ui.Title("12. Немає обмеження -> немає доступу до членів T");
        Ui.CompileError("ERR_NO_MEMBER",
            "static string BrokenName<T>(T item) { return item.Name; }",
            "CS1061: 'T' does not contain a definition for 'Name'");
        Ui.Info("  Для компілятора T без обмежень — це майже object: він знає про T лише те, що є в object.");

        Ui.Fix("додати обмеження: where T : Animal.");
        Ui.Good($"  GetName(dog) = {GetName(new Dog { Name = "Рекс" })}");
        Ui.Good($"  GetName(cat) = {GetName(new Cat { Name = "Мурка" })}");
    }

    // 13 + 18. new T() та виведення типу ---------------------------------------------------
#if ERR_NEW_T
    static T BrokenCreate<T>()
    {
        // Чому так: компілятор не знає, чи має конкретний T публічний конструктор
        // без параметрів — у string чи абстрактного класу такого може не бути.
        // Тому new T() дозволено лише з явним обмеженням where T : new(), яке
        // гарантує наявність такого конструктора.
        return new T();                                     // CS0304
    }
#endif

    static T Create<T>() where T : new()
    {
        return new T();
    }

    public static void NewOfT()
    {
        Ui.Title("13. new T() без обмеження new()");
        Ui.CompileError("ERR_NEW_T",
            "static T BrokenCreate<T>() { return new T(); }",
            "CS0304: Cannot create an instance of the variable type 'T' because it does not have the new() constraint");
        Ui.Info("  Компілятор не знає, чи є у T публічний конструктор без параметрів.");

        Ui.Fix("where T : new().  Тоді Create<Dog>() працює, а Create<Owner>() теж (має конструктор за замовчуванням).");
        Dog dog = Create<Dog>();
        dog.Name = "Новий пес";
        Ui.Good($"  Створено: {dog}");
    }

    public static void TypeInference()
    {
        Ui.Title("14. Компілятор не може вивести T");

#if ERR_INFERENCE
        // Чому так: виведення типу (type inference) працює тільки за аргументами
        // виклику методу. У Create<T>() параметрів немає, тому компілятору нізвідки
        // взяти "підказку" про T — тип, у який присвоюють результат (Animal a = ...),
        // у виведенні участі не бере.
        Animal a = Create();                                // CS0411
#endif
        Ui.CompileError("ERR_INFERENCE",
            "Animal a = Create();",
            "CS0411: The type arguments for method 'Create<T>()' cannot be inferred from the usage");
        Ui.Info("  T не зустрічається серед параметрів методу, а тип результату у виведенні НЕ бере участі.");

        Ui.Fix("вказати аргумент типу явно: Create<Dog>().");
        Animal a2 = Create<Dog>();
        Ui.Good($"  Створено: {a2.GetType().Name}");
    }

    // 15. Порівняння T -----------------------------------------------------------------
#if ERR_COMPARE
    static T BrokenMax<T>(T a, T b)
    {
        // Чому так: оператори (>, <, ==) визначені для конкретних типів, і
        // компілятор не може гарантувати їх існування для довільного T без
        // обмежень (T може виявитися типом, для якого > взагалі не має сенсу).
        // Замість оператора для порівняння T використовують IComparable<T>.
        return a > b ? a : b;                               // CS0019
    }
#endif

    static T Max<T>(T a, T b) where T : IComparable<T>
    {
        return a.CompareTo(b) >= 0 ? a : b;
    }

    public static void CompareGeneric()
    {
        Ui.Title("15. Оператори (>, <) для T");
        Ui.CompileError("ERR_COMPARE",
            "static T BrokenMax<T>(T a, T b) { return a > b ? a : b; }",
            "CS0019: Operator '>' cannot be applied to operands of type 'T' and 'T'");
        Ui.Info("  Оператори не можна вимагати через where (до C# 11); замість них — інтерфейс IComparable<T>.");

        Ui.Fix("where T : IComparable<T> і виклик a.CompareTo(b).");
        Ui.Good($"  Max(3, 7)          = {Max(3, 7)}");
        Ui.Good($"  Max(\"аб\", \"аа\")    = {Max("аб", "аа")}");
    }

    // 16. Неправильне оголошення variance ------------------------------------------------
#if ERR_VARIANCE_DECL
    interface IBadProducer<out T>
    {
        T Get();
        // Чому так: out T дозволяє T лише в позиції РЕЗУЛЬТАТУ (T "виходить" з
        // інтерфейсу), а Set(T value) вимагає, щоб T "заходив" усередину як
        // параметр. Якби такий інтерфейс дозволили, через IBadProducer<Animal>
        // можна було б викликати Set(cat) на насправді IBadProducer<Dog> —
        // компілятор блокує таке оголошення ще тут, до будь-якого використання.
        void Set(T value);                                  // CS1961
    }
#endif

    public static void VarianceDeclaration()
    {
        Ui.Title("16. 'out T' у позиції параметра -> неправильна variance");
        Ui.CompileError("ERR_VARIANCE_DECL",
            "interface IBadProducer<out T> { T Get(); void Set(T value); }",
            "CS1961: Invalid variance: The type parameter 'T' must be invariantly valid on 'Set(T)'. 'T' is covariant.");
        Ui.Info("  out — T лише у результатах; in — T лише у параметрах. Обидва варіанти разом неможливі.");

        Ui.Fix("розділити інтерфейс: читання (out T) і запис (in T / інваріантний). Саме так зроблено в проєкті:");
        Ui.Good("  IReadOnlyRepository<out T>  — тільки віддає T");
        Ui.Good("  IRepository<T>              — приймає T (Add), тому інваріантний");
    }

    // 17. Variance у зворотний бік ----------------------------------------------------------
    public static void VarianceDirection()
    {
        Ui.Title("17. Контраваріантність працює лише в один бік");

        IValidator<Animal> animalValidator = new AnimalValidator();
        IValidator<Dog> asDogValidator = animalValidator;   // OK: "in" — валідатор тварин перевірить і собаку
        Ui.Good($"  IValidator<Animal> як IValidator<Dog>: OK, IsValid(Рекс) = {asDogValidator.IsValid(new Dog { Name = "Рекс" })}");

#if ERR_VARIANCE_DIRECTION
        IValidator<Dog> dogValidator = new DogValidator();
        // Чому так: IValidator<in T> контраваріантний — валідатор ЗАГАЛЬНІШОГО типу
        // (Animal) можна використати там, де очікують валідатор конкретнішого (Dog),
        // бо він вміє перевірити і Dog. Зворотне неможливе: DogValidator читає
        // item.Breed, якого немає в Cat, тому передати його як IValidator<Animal>
        // компілятор забороняє — це б означало попросити валідатор зробити неможливе.
        IValidator<Animal> broken = dogValidator;           // CS0266
#endif
        Ui.CompileError("ERR_VARIANCE_DIRECTION",
            "IValidator<Animal> broken = dogValidator;  // dogValidator має тип IValidator<Dog>",
            "CS0266: Cannot implicitly convert type 'IValidator<Dog>' to 'IValidator<Animal>'. An explicit conversion exists");
        Ui.Info("  Чому: DogValidator читає item.Breed. Кота (Cat) у нього просто нема чого перевіряти.");
    }

    // 18. Інваріантність IRepository ----------------------------------------------------------
    static void PrintAll(IReadOnlyRepository<Animal> repo)
    {
        foreach (Animal animal in repo.GetAll())
            Ui.Good($"  {animal}");
    }

#if ERR_REPO_INVARIANT
    static void FillWithCats(IRepository<Animal> repo)
    {
        repo.Add(new Cat { Id = 99, Name = "Підкинутий кіт" });
    }
#endif

    public static void RepositoryInvariance()
    {
        Ui.Title("18. IRepository<Dog> не передати як IRepository<Animal>");

        InMemoryRepository<Dog> dogRepo = new InMemoryRepository<Dog>();
        dogRepo.Add(new Dog { Id = 1, Name = "Рекс" });

        Ui.Info("Читання: IReadOnlyRepository<out T> — передається без проблем.");
        PrintAll(dogRepo);

#if ERR_REPO_INVARIANT
        // Чому так: FillWithCats записує (Add) елемент у репозиторій. Якби
        // IRepository<Dog> можна було передати як IRepository<Animal>, метод міг би
        // покласти туди Cat — і "сховище собак" отримало б кота. IRepository<T>
        // приймає T, тому лишається інваріантним (на відміну від PrintAll вище,
        // який приймає лише-читання IReadOnlyRepository<out T> і працює без проблем).
        FillWithCats(dogRepo);                              // CS1503
#endif
        Ui.CompileError("ERR_REPO_INVARIANT",
            "FillWithCats(dogRepo);   // метод приймає IRepository<Animal>",
            "CS1503: Argument 1: cannot convert from 'InMemoryRepository<Dog>' to 'IRepository<Animal>'");
        Ui.Info("  Чому: FillWithCats додав би Cat у сховище собак — саме від цього захищає інваріантність.");
        Ui.Fix("для запису передавати IRepository<Dog>; для читання — IReadOnlyRepository<Animal>.");
    }
}
