using System.Diagnostics;
using System.Linq.Expressions;
using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("=== Порівняння продуктивності: Прямий виклик vs Рефлексія vs Expression Trees ===\n");

const int iterations = 1_000_000;
var car = new Car();
Type carType = typeof(Car);
MethodInfo accelerateMethod = carType.GetMethod("Accelerate", new[] { typeof(int) })!;

// Підготовка альтернативних способів виклику
// 1. DLR (dynamic)
dynamic dynamicCar = car;

// 2. Delegate.CreateDelegate (найшвидший динамічний делегат для відкритих методів)
var typedDelegate = (Action<Car, int>)Delegate.CreateDelegate(typeof(Action<Car, int>), accelerateMethod);

// 3. Compiled Expression Tree (універсальний спосіб побудови делегатів у runtime)
var instanceParam = Expression.Parameter(typeof(Car), "target");
var argParam = Expression.Parameter(typeof(int), "delta");
var callExpr = Expression.Call(instanceParam, accelerateMethod, argParam);
var compiledExpression = Expression.Lambda<Action<Car, int>>(callExpr, instanceParam, argParam).Compile();

// Прогрів JIT (warmup)
car.Accelerate(1);
accelerateMethod.Invoke(car, new object[] { 1 });
dynamicCar.Accelerate(1);
typedDelegate(car, 1);
compiledExpression(car, 1);

Console.WriteLine($"Кількість ітерацій: {iterations:N0}\n");

// 1. Прямий виклик
var sw = Stopwatch.StartNew();
for (int i = 0; i < iterations; i++)
{
    car.Accelerate(1);
}
sw.Stop();
long directMs = sw.ElapsedMilliseconds;
Console.WriteLine($"1. Прямий виклик (Direct Call):              {directMs,5} ms (1.0x - Базовий еталон)");

// 2. Delegate.CreateDelegate
sw.Restart();
for (int i = 0; i < iterations; i++)
{
    typedDelegate(car, 1);
}
sw.Stop();
long delegateMs = sw.ElapsedMilliseconds;
Console.WriteLine($"2. Delegate.CreateDelegate:                  {delegateMs,5} ms ({(double)delegateMs / Math.Max(directMs, 1):F1}x)");

// 3. Compiled Expression Tree
sw.Restart();
for (int i = 0; i < iterations; i++)
{
    compiledExpression(car, 1);
}
sw.Stop();
long exprMs = sw.ElapsedMilliseconds;
Console.WriteLine($"3. Compiled Expression Tree:                 {exprMs,5} ms ({(double)exprMs / Math.Max(directMs, 1):F1}x)");

// 4. dynamic (DLR CallSite Cache)
sw.Restart();
for (int i = 0; i < iterations; i++)
{
    dynamicCar.Accelerate(1);
}
sw.Stop();
long dynamicMs = sw.ElapsedMilliseconds;
Console.WriteLine($"4. Dynamic (DLR CallSite):                   {dynamicMs,5} ms ({(double)dynamicMs / Math.Max(directMs, 1):F1}x)");

// 5. Чистий MethodInfo.Invoke
sw.Restart();
object[] invokeParams = new object[] { 1 };
for (int i = 0; i < iterations; i++)
{
    accelerateMethod.Invoke(car, invokeParams);
}
sw.Stop();
long invokeMs = sw.ElapsedMilliseconds;
Console.WriteLine($"5. Чистий MethodInfo.Invoke (Reflection):    {invokeMs,5} ms ({(double)invokeMs / Math.Max(directMs, 1):F1}x)");

Console.WriteLine("\n💡 Висновок: Чиста рефлексія (MethodInfo.Invoke) має відчутний оверхед на перевірку аргументів та boxing.");
Console.WriteLine("Сучасні високонавантажені .NET-фреймворки (EF Core, MediatR, ASP.NET Core) компілюють виклики через Expression Trees або генератори вихідного коду (Source Generators).");
