GenericsPitfalls — типові помилки з узагальненнями (generics)
=============================================================

Консольний проєкт із меню. Кожен пункт показує ОДНУ типову помилку та її виправлення.
Делегати, лямбди та LINQ навмисно НЕ використовуються.

Докладні пояснення до кожного пункту меню: PITFALLS.md

Запуск:   dotnet run --project GenericsPitfalls
          (або F5 у Visual Studio, стартовий проєкт — GenericsPitfalls)


МОДЕЛІ ТА ЗВ'ЯЗКИ
-----------------

                    +-----------+
                    | IEntity   |  { int Id }
                    +-----------+
                      ^        ^
                      |        |
   +---------+   +---------+  |
   | Animal  |   | Owner   |--+  також реалізує IEquatable<Owner>
   +---------+   +---------+
    ^       ^     | Pets: List<Animal>   (Owner 1 --- * Animal)
    |       |
 +-----+ +-----+
 | Dog | | Cat |     Animal.OwnerId : int?  (Nullable<int>)
 +-----+ +-----+

   IReadOnlyRepository<out T>          -- лише читає T  -> КОВАРІАНТНИЙ
            ^
            |
   IRepository<T>                      -- ще й приймає T (Add) -> ІНВАРІАНТНИЙ
            ^
            |
   InMemoryRepository<T>               -- where T : class, IEntity

   IValidator<in T>                    -- лише приймає T -> КОНТРАВАРІАНТНИЙ
        ^                 ^
        |                 |
   AnimalValidator   DogValidator      (IValidator<Animal>, IValidator<Dog>)

   EntityCounterBase  <-  EntityCounter<T>     (статичні поля; демо №6)


ПОМИЛКИ ЧАСУ ВИКОНАННЯ (пункти 1–9)  — збирається, але ламається/дає хибний результат
-----------------------------------------------------------------------------------
  1. Коваріантність масивів          ArrayTypeMismatchException
  2. ArrayList замість List<T>       InvalidCastException
  3. Boxing / unboxing               InvalidCastException
  4. default(T) = null               NullReferenceException
  5. Nullable<T>.Value               InvalidOperationException
  6. Статичне поле в generic-класі   логічна помилка (окрема копія на кожен T)
  7. Dictionary                      ArgumentException, KeyNotFoundException
  8. Зміна списку в foreach          InvalidOperationException
  9. Ключ без Equals/GetHashCode     логічна помилка (посилальна рівність)


ПОМИЛКИ КОМПІЛЯЦІЇ (пункти 10–18)  — див. Demos\CompileErrors.cs
---------------------------------------------------------------
Хибний код захований за #if, тому проєкт збирається. Щоб побачити помилку:
розкоментуйте ОДИН рядок  // #define ERR_...  на початку Demos\CompileErrors.cs
(після перегляду закоментуйте назад).

  Пункт  Символ                    Код       Суть
  -----  ------------------------  --------  ------------------------------------
  10     ERR_INVARIANT_LIST        CS0029    List<Dog> -> List<Animal>
  11     ERR_CONSTRAINT            CS0311    тип не відповідає where (string; для int — CS0452)
  12     ERR_NO_MEMBER             CS1061    член T без обмеження
  13     ERR_NEW_T                 CS0304    new T() без where T : new()
  14     ERR_INFERENCE             CS0411    T не виводиться з аргументів
  15     ERR_COMPARE               CS0019    оператор > для T
  16     ERR_VARIANCE_DECL         CS1961    out T у параметрі методу
  17     ERR_VARIANCE_DIRECTION    CS0266    IValidator<Dog> -> IValidator<Animal>
  18     ERR_REPO_INVARIANT        CS1503    IRepository<Dog> -> IRepository<Animal>

Усі коди перевірено збіркою з відповідним символом.

Також можна з командного рядка, без правки файлу:
  dotnet build GenericsPitfalls -p:DefineConstants=ERR_CONSTRAINT
