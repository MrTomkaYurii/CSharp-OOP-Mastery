using System.Collections;
using GenericsPitfalls.Infrastructure;
using GenericsPitfalls.Models;

namespace GenericsPitfalls.Demos;

// Помилки, які компілятор НЕ ловить: програма збирається, а ламається (або тихо
// дає неправильний результат) під час виконання.
static class RuntimeErrors
{
    // 1. Коваріантність масивів ------------------------------------------------------
    // Чому так: масиви посилальних типів у .NET коваріантні ще з часів до generics,
    // тому Dog[] неявно приводиться до Animal[]. Компілятор бачить лише статичний
    // тип змінної (Animal[]) і дозволяє покласти туди Cat. Через це середовище
    // виконання змушене при КОЖНОМУ записі в масив перевіряти, чи справжній тип
    // масиву (Dog[]) сумісний зі значенням — не підходить, кидає виняток.
    public static void ArrayCovariance()
    {
        Ui.Title("1. Коваріантність масивів -> ArrayTypeMismatchException");
        Ui.Info("Dog[] можна присвоїти змінній Animal[] — компілятор не заперечує.");

        Animal[] animals = new Dog[2];                       // реально це масив собак
        animals[0] = new Dog { Id = 1, Name = "Рекс", Breed = "Вівчарка" };

        try
        {
            animals[1] = new Cat { Id = 2, Name = "Мурка" }; // за типом змінної — можна, за фактом — ні
            Ui.Good("  Кота додано (цього рядка ми не побачимо)");
        }
        catch (ArrayTypeMismatchException ex)
        {
            Ui.Bad("  Спроба покласти Cat у масив Dog[]:");
            Ui.Exception(ex);
        }

        Ui.Fix("List<Animal> приймає і собак, і котів; List<T> інваріантний, тож підміни немає.");
        List<Animal> list = new List<Animal>();
        list.Add(new Dog { Id = 1, Name = "Рекс" });
        list.Add(new Cat { Id = 2, Name = "Мурка" });
        Ui.Good($"  У списку {list.Count} тварини — усе гаразд");
    }

    // 2. Негенеричні колекції --------------------------------------------------------
    // Чому так: ArrayList з'явився до generics і зберігає елементи як object, тому
    // приймає будь-що без перевірки типів на компіляції. Відповідальність за
    // коректність приведення ((Dog)item) лягає повністю на програміста, і помилка
    // проявляється не там, де додали хибний елемент, а лише там, де його читають.
    public static void NonGenericList()
    {
        Ui.Title("2. ArrayList (object) -> InvalidCastException");
        Ui.Info("ArrayList зберігає object, тому прийме що завгодно.");

        ArrayList pets = new ArrayList();
        pets.Add(new Dog { Id = 1, Name = "Рекс" });
        pets.Add("Мурка");                                   // рядок замість тварини — компілятор мовчить

        try
        {
            foreach (object item in pets)
            {
                Dog dog = (Dog)item;                         // явне приведення: на рядку впаде
                Ui.Info($"  Собака: {dog.Name}");
            }
        }
        catch (InvalidCastException ex)
        {
            Ui.Exception(ex);
        }

        Ui.Fix("List<Dog>: рядок навіть не додасться — помилка CS1503 з'явиться ще на компіляції.");
        List<Dog> dogs = new List<Dog>();
        dogs.Add(new Dog { Id = 1, Name = "Рекс" });
        foreach (Dog dog in dogs)                            // приведення типів більше не потрібне
            Ui.Good($"  Собака: {dog.Name}");
    }

    // 3. Boxing / unboxing -----------------------------------------------------------
    // Чому так: boxing пакує значущий тип у об'єкт на купі, а unboxing розпаковує
    // назад — але СТРОГО у той самий тип, який було запаковано. Усередині boxedId
    // лежить саме int, і вимагати з нього long напряму не можна, хоча як звичайні
    // змінні int у long перетворюється неявно: між упакованим int і long зв'язку немає.
    public static void BoxingUnboxing()
    {
        Ui.Title("3. Unboxing не того типу -> InvalidCastException");
        Ui.Info("int, покладений в object, можна дістати назад ТІЛЬКИ як int.");

        object boxedId = 5;                                  // boxing: int -> object

        try
        {
            long id = (long)boxedId;                         // int -> long напряму не розпаковується
            Ui.Info($"  id = {id}");
        }
        catch (InvalidCastException ex)
        {
            Ui.Exception(ex);
        }

        Ui.Fix("спершу розпакувати як int, потім розширити до long: (long)(int)boxedId.");
        long fixedId = (long)(int)boxedId;
        Ui.Good($"  id = {fixedId}");
        Ui.Info("  Ще краще — не пакувати взагалі: List<int> замість ArrayList/object.");
    }

    // 4. default(T) = null ------------------------------------------------------------
    // Чому так: для посилальних типів (class) default(T) дорівнює null, тож метод
    // пошуку чесно повертає "нічого не знайдено" замість винятку — перевірка
    // результату лягає на виклик. Оператор '!' лише вимикає попередження
    // компілятора про можливий null, але нічого не перевіряє, тому розіменування
    // missing!.Name все одно впаде, якщо запис справді не знайдено.
    public static void NullFromRepository()
    {
        Ui.Title("4. FindById повертає default(T) = null -> NullReferenceException");

        InMemoryRepository<Dog> repo = new InMemoryRepository<Dog>();
        repo.Add(new Dog { Id = 1, Name = "Рекс", Breed = "Вівчарка" });

        Ui.Info("У репозиторії лише собака з Id = 1. Шукаємо Id = 42.");
        Dog? missing = repo.FindById(42);                    // для class default(T) — це null

        try
        {
            Ui.Info($"  Знайдено: {missing!.Name}");         // '!' глушить попередження, але не помилку
        }
        catch (NullReferenceException ex)
        {
            Ui.Exception(ex);
        }

        Ui.Fix("перевіряти результат на null (або використовувати ?. та ??).");
        Ui.Good($"  Знайдено: {missing?.Name ?? "(такої тварини немає)"}");
    }

    // 5. Nullable<T>.Value ------------------------------------------------------------
    // Чому так: int? — це скорочення для узагальненої структури Nullable<int>, яка
    // складається зі значення і прапорця HasValue. Коли HasValue == false, звернення
    // до .Value кидає виняток, бо там за фактом немає що повертати — так само
    // впаде і явне приведення (int)stray.OwnerId.
    public static void NullableValue()
    {
        Ui.Title("5. Nullable<int>.Value без значення -> InvalidOperationException");
        Ui.Info("int? — це узагальнена структура Nullable<int>. У бездомного пса OwnerId = null.");

        Dog stray = new Dog { Id = 7, Name = "Бобік" };      // OwnerId не задано -> null

        try
        {
#pragma warning disable CS8629   // навмисно ігноруємо попередження компілятора
            int ownerId = stray.OwnerId.Value;
#pragma warning restore CS8629
            Ui.Info($"  OwnerId = {ownerId}");
        }
        catch (InvalidOperationException ex)
        {
            Ui.Exception(ex);
        }

        Ui.Fix("перевірити HasValue або взяти GetValueOrDefault / оператор ??.");
        Ui.Good($"  OwnerId = {(stray.OwnerId.HasValue ? stray.OwnerId.Value.ToString() : "немає власника")}");
        Ui.Good($"  Через ??: {stray.OwnerId ?? -1}");
    }

    // 6. Статичні поля в generic-класі -------------------------------------------------
    // Чому так: EntityCounter<T> — це відкритий тип-шаблон, а EntityCounter<Dog>,
    // EntityCounter<Cat> і EntityCounter<Animal> — три РІЗНІ закриті типи, кожен зі
    // своєю власною копією статичного поля Count. Спадкування Dog від Animal тут
    // не допомагає: EntityCounter<Dog> не є нащадком EntityCounter<Animal>, це два
    // самостійні типи зі своєю статикою.
    public static void StaticPerClosedType()
    {
        Ui.Title("6. Статичне поле в EntityCounter<T> — окреме для кожного T (логічна помилка)");

        EntityCounter<Dog>.Count = 0;                        // скидаємо, якщо демо запускають повторно
        EntityCounter<Cat>.Count = 0;
        EntityCounterBase.Total = 0;

        new EntityCounter<Dog>();
        new EntityCounter<Dog>();
        new EntityCounter<Cat>();

        Ui.Info("Створили 2 лічильники для Dog і 1 для Cat. Очікуємо разом 3?");
        Ui.Bad($"  EntityCounter<Dog>.Count    = {EntityCounter<Dog>.Count}");
        Ui.Bad($"  EntityCounter<Cat>.Count    = {EntityCounter<Cat>.Count}");
        Ui.Bad($"  EntityCounter<Animal>.Count = {EntityCounter<Animal>.Count}  (а не 3!)");
        Ui.Info("  EntityCounter<Dog>, <Cat> і <Animal> — три РІЗНІ типи, кожен зі своїм статичним полем.");

        Ui.Fix("спільний стан тримати в неузагальненому базовому класі.");
        Ui.Good($"  EntityCounterBase.Total = {EntityCounterBase.Total}");
    }

    // 7. Dictionary: дублікат та відсутній ключ ------------------------------------------
    // Чому так: Add явно означає "додай новий елемент", тому кидає ArgumentException,
    // якщо ключ уже зайнятий — це захист від випадкової підміни. Індексатор при
    // читанні відсутнього ключа кидає KeyNotFoundException, бо йому немає що
    // повернути. TryAdd/TryGetValue існують саме для випадків, коли відсутність чи
    // наявність ключа — звичайна ситуація, а не виняткова.
    public static void DictionaryKeys()
    {
        Ui.Title("7. Dictionary: дублікат ключа та відсутній ключ");

        Dictionary<int, Owner> owners = new Dictionary<int, Owner>();
        owners.Add(1, new Owner { Id = 1, Name = "Олена" });

        try
        {
            owners.Add(1, new Owner { Id = 1, Name = "Ігор" });
        }
        catch (ArgumentException ex)
        {
            Ui.Bad("  Add з уже існуючим ключем:");
            Ui.Exception(ex);
        }

        try
        {
            Owner owner = owners[99];
            Ui.Info($"  {owner.Name}");
        }
        catch (KeyNotFoundException ex)
        {
            Ui.Bad("  Індексатор з неіснуючим ключем:");
            Ui.Exception(ex);
        }

        Ui.Fix("TryAdd та TryGetValue повертають bool і не кидають винятків.");
        bool added = owners.TryAdd(1, new Owner { Id = 1, Name = "Ігор" });
        Ui.Good($"  TryAdd повернув {added}, у словнику лишився: {owners[1].Name}");
        if (owners.TryGetValue(99, out Owner? found))
            Ui.Info($"  {found.Name}");
        else
            Ui.Good("  TryGetValue: ключа 99 немає, і це не помилка");
    }

    // 8. Зміна колекції під час foreach ------------------------------------------------
    // Чому так: foreach працює через перелічувач (enumerator), який запам'ятовує
    // "версію" колекції на старті обходу. Remove змінює цю внутрішню версію, і на
    // наступному кроці перелічувач помічає розбіжність і кидає виняток — це захист
    // від того, щоб не пропустити елемент або не вийти за межі після зсуву індексів.
    public static void ModifyDuringForeach()
    {
        Ui.Title("8. Зміна List<T> під час foreach -> InvalidOperationException");

        Owner owner = new Owner { Id = 1, Name = "Олена" };
        owner.Pets.Add(new Dog { Id = 1, Name = "Рекс" });
        owner.Pets.Add(new Cat { Id = 2, Name = "Мурка" });
        owner.Pets.Add(new Dog { Id = 3, Name = "Бім" });

        Ui.Info("Хочемо видалити всіх котів зі списку улюбленців.");
        try
        {
            foreach (Animal pet in owner.Pets)
            {
                if (pet is Cat)
                    owner.Pets.Remove(pet);                  // змінюємо колекцію, яку перебираємо
            }
        }
        catch (InvalidOperationException ex)
        {
            Ui.Exception(ex);
        }

        Ui.Fix("іти індексом з кінця (або перебирати копію списку).");
        owner.Pets.Clear();
        owner.Pets.Add(new Dog { Id = 1, Name = "Рекс" });
        owner.Pets.Add(new Cat { Id = 2, Name = "Мурка" });
        owner.Pets.Add(new Dog { Id = 3, Name = "Бім" });
        for (int i = owner.Pets.Count - 1; i >= 0; i--)
        {
            if (owner.Pets[i] is Cat)
                owner.Pets.RemoveAt(i);
        }
        Ui.Good($"  Лишилось улюбленців: {owner.Pets.Count}");
    }

    // 9. Ключ без Equals/GetHashCode ----------------------------------------------------
    // Чому так: Dog не перевизначає Equals/GetHashCode, тому діє успадкована від
    // object поведінка — рівність за посиланням. rex1 і rex2 мають однакові дані,
    // але це два різні об'єкти з різними хеш-кодами, тож Dictionary/HashSet навіть
    // не заглядають у потрібний "кошик" і вважають їх різними ключами.
    public static void KeysWithoutEquality()
    {
        Ui.Title("9. Ключ Dictionary/HashSet без Equals/GetHashCode (логічна помилка)");
        Ui.Info("Два об'єкти з однаковим Id — це одна й та сама тварина?");

        Dog rex1 = new Dog { Id = 1, Name = "Рекс" };
        Dog rex2 = new Dog { Id = 1, Name = "Рекс" };

        Dictionary<Dog, int> ages = new Dictionary<Dog, int>();
        ages[rex1] = 3;
        HashSet<Dog> dogSet = new HashSet<Dog> { rex1, rex2 };

        Ui.Info("Dog НЕ перевизначає Equals -> порівнюється за посиланням:");
        Ui.Bad($"  ages.ContainsKey(rex2) = {ages.ContainsKey(rex2)}  (очікували True)");
        Ui.Bad($"  HashSet<Dog>.Count     = {dogSet.Count}  (очікували 1)");

        Ui.Fix("реалізувати IEquatable<T> + Equals + GetHashCode (як у Owner) або брати ключем Id.");
        Owner olena1 = new Owner { Id = 1, Name = "Олена" };
        Owner olena2 = new Owner { Id = 1, Name = "Олена" };
        Dictionary<Owner, int> visits = new Dictionary<Owner, int>();
        visits[olena1] = 5;
        HashSet<Owner> ownerSet = new HashSet<Owner> { olena1, olena2 };
        Ui.Good($"  visits.ContainsKey(olena2) = {visits.ContainsKey(olena2)}");
        Ui.Good($"  HashSet<Owner>.Count       = {ownerSet.Count}");
    }
}
