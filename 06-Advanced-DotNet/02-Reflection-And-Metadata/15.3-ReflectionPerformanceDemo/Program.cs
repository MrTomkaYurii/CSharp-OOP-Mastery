using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 15.3. БЕНЧМАРК ШВИДКОДІЇ: ДЕМОНСТРАЦІЯ БУСТІНГУ ЧЕРЕЗ EXPRESSION TREES");
Console.WriteLine(" Порівняння: Прямий виклик vs Expression Tree vs Delegate vs dynamic vs Invoke");
Console.WriteLine("==========================================================================\n");

const int iterations = 1_000_000;
var car = new Car("Тестове авто", 2024);
Type carType = typeof(Car);
MethodInfo accelerateMethod = carType.GetMethod("Accelerate", new[] { typeof(int) })!;

// -----------------------------------------------------------------------
// Підготовка механізмів виклику на основі знань з 15.0 - 15.2
// -----------------------------------------------------------------------

// 1. dynamic (DLR)
dynamic dynamicCar = car;

// 2. Delegate.CreateDelegate (типізований делегат до відкритого методу)
var typedDelegate = (Action<Car, int>)Delegate.CreateDelegate(typeof(Action<Car, int>), accelerateMethod);

// 3. Compiled Expression Tree (побудовано як у прикладі 15.2)
var instanceParam = Expression.Parameter(typeof(Car), "target");
var argParam = Expression.Parameter(typeof(int), "delta");
var callExpr = Expression.Call(instanceParam, accelerateMethod, argParam);
var compiledExpression = Expression.Lambda<Action<Car, int>>(callExpr, instanceParam, argParam).Compile();

// Прогрів JIT-компілятора (Warmup), щоб виключити час первинної компіляції з вимірів
car.Accelerate(1);
accelerateMethod.Invoke(car, new object[] { 1 });
dynamicCar.Accelerate(1);
typedDelegate(car, 1);
compiledExpression(car, 1);

Console.WriteLine($"Кількість викликів методу Accelerate: {iterations:N0} ітерацій\n");
Console.WriteLine("--------------------------------------------------------------------------");
Console.WriteLine("Результати замірів часу виконання (Stopwatch):");
Console.WriteLine("--------------------------------------------------------------------------");

// 1. Прямий компіляційний виклик (Direct Call)
var sw = Stopwatch.StartNew();
for (int i = 0; i < iterations; i++)
{
    car.Accelerate(1);
}
sw.Stop();
long directMs = sw.ElapsedMilliseconds;
Console.WriteLine($"1. Прямий виклик (Direct Call):              {directMs,5} ms | 1.0x (Базовий еталон)");

// 2. Скомпільоване дерево виразів (Compiled Expression Tree)
sw.Restart();
for (int i = 0; i < iterations; i++)
{
    compiledExpression(car, 1);
}
sw.Stop();
long exprMs = sw.ElapsedMilliseconds;
double exprRatio = (double)exprMs / Math.Max(directMs, 1);
Console.WriteLine($"2. Compiled Expression Tree:                 {exprMs,5} ms | {exprRatio:F1}x (Бустінг рефлексії)");

// 3. Delegate.CreateDelegate
sw.Restart();
for (int i = 0; i < iterations; i++)
{
    typedDelegate(car, 1);
}
sw.Stop();
long delegateMs = sw.ElapsedMilliseconds;
double delegateRatio = (double)delegateMs / Math.Max(directMs, 1);
Console.WriteLine($"3. Delegate.CreateDelegate:                  {delegateMs,5} ms | {delegateRatio:F1}x");

// 4. dynamic (DLR з кешуванням CallSite)
sw.Restart();
for (int i = 0; i < iterations; i++)
{
    dynamicCar.Accelerate(1);
}
sw.Stop();
long dynamicMs = sw.ElapsedMilliseconds;
double dynamicRatio = (double)dynamicMs / Math.Max(directMs, 1);
Console.WriteLine($"4. dynamic (DLR CallSite):                   {dynamicMs,5} ms | {dynamicRatio:F1}x");

// 5. Класична рефлексія (MethodInfo.Invoke)
sw.Restart();
object[] invokeParams = new object[] { 1 };
for (int i = 0; i < iterations; i++)
{
    accelerateMethod.Invoke(car, invokeParams);
}
sw.Stop();
long invokeMs = sw.ElapsedMilliseconds;
double invokeRatio = (double)invokeMs / Math.Max(directMs, 1);
Console.WriteLine($"5. MethodInfo.Invoke (Рефлексія):            {invokeMs,5} ms | {invokeRatio:F1}x (Найповільніший)");

// -----------------------------------------------------------------------
// Теоретичне резюме для студентів
// -----------------------------------------------------------------------
Console.WriteLine("\n--------------------------------------------------------------------------");
Console.WriteLine("Чому скомпільоване Expression Tree забезпечує такий колосальний бустінг?");
Console.WriteLine("--------------------------------------------------------------------------");
Console.WriteLine("1. При класичному MethodInfo.Invoke середовище CoreCLR під час КОЖНОГО виклику:");
Console.WriteLine("   - Перевіряє сумісність типів та кількість аргументів.");
Console.WriteLine("   - Виконує boxing для примітивних типів значень у масив object[].");
Console.WriteLine("   - Здійснює перехід через нативний диспетчерський перехідник (CallDescrWorker).");
Console.WriteLine("   - Унеможливлює інлайнінг коду JIT-компілятором.");
Console.WriteLine();
Console.WriteLine("2. При використанні Expression Trees (метод Compile()):");
Console.WriteLine("   - Дерево один раз перекладається в прямі інструкції проміжної мови (IL).");
Console.WriteLine("   - Отриманий делегат викликається як прямий машинний код процесора.");
Console.WriteLine("   - Повністю відсутні зайві виділення пам'яті (Heap Allocations) та перевірки.");
Console.WriteLine();
Console.WriteLine("Саме тому високонавантажені бібліотеки (.NET ORM, серіалізатори, IoC-контейнери)");
Console.WriteLine("використовують Expression Trees для динамічних операцій замість класичного Invoke.");
Console.WriteLine("==========================================================================");
