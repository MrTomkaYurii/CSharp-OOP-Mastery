using System.Linq.Expressions;
using System.Reflection;
using Shared.Common;

Console.OutputEncoding = System.Text.Encoding.UTF8;

Console.WriteLine("==========================================================================");
Console.WriteLine(" 15.2. ЕКСПРЕСІЇ ДЛЯ РЕФЛЕКСІЇ: ДОСТУП ДО ЧЛЕНІВ ТА ВИКЛИКІВ");
Console.WriteLine(" Заміна повільних GetValue/Invoke/CreateInstance на скомпільовані делегати");
Console.WriteLine("==========================================================================\n");

var myCar = new Car("Синій", 50);

// -----------------------------------------------------------------------
// Крок 1: Швидке читання властивостей (Заміна PropertyInfo.GetValue)
// -----------------------------------------------------------------------
Console.WriteLine("--- Крок 1: Генерація швидкого геттера властивості (Property Access) ---\n");

PropertyInfo licensePlateProp = typeof(Car).GetProperty("LicensePlate")!;

// Класичний шлях рефлексії:
// string slowPlate = (string)licensePlateProp.GetValue(myCar)!;

// Шлях Expression Trees:
// Будуємо вираз виду: (Car car) => car.LicensePlate
ParameterExpression carInstanceParam = Expression.Parameter(typeof(Car), "car");
MemberExpression propertyAccess = Expression.Property(carInstanceParam, licensePlateProp);
var getterLambda = Expression.Lambda<Func<Car, string>>(propertyAccess, carInstanceParam);

Console.WriteLine($"Сконструйований вираз геттера: {getterLambda}");

// Компіляція у високоефективний делегат:
Func<Car, string> fastLicensePlateGetter = getterLambda.Compile();

string readPlate = fastLicensePlateGetter(myCar);
Console.WriteLine($"Прочитано номер через скомпільований геттер: '{readPlate}' (швидкість прямого звернення!)\n");

// -----------------------------------------------------------------------
// Крок 2: Швидкий виклик методу екземпляра (Заміна MethodInfo.Invoke)
// -----------------------------------------------------------------------
Console.WriteLine("--------------------------------------------------------------------------");
Console.WriteLine("--- Крок 2: Генерація швидкого виклику методу (Method Call) ---");
Console.WriteLine("--------------------------------------------------------------------------\n");

MethodInfo accelerateMethod = typeof(Car).GetMethod("Accelerate", new[] { typeof(int) })!;

// Класичний шлях рефлексії (повільний, пакує параметри в object[]):
// accelerateMethod.Invoke(myCar, new object[] { 60 });

// Шлях Expression Trees:
// Будуємо вираз виду: (Car car, int delta) => car.Accelerate(delta)
ParameterExpression deltaParam = Expression.Parameter(typeof(int), "delta");
MethodCallExpression methodCall = Expression.Call(carInstanceParam, accelerateMethod, deltaParam);
var callLambda = Expression.Lambda<Action<Car, int>>(methodCall, carInstanceParam, deltaParam);

Console.WriteLine($"Сконструйований вираз виклику методу: {callLambda}");

Action<Car, int> fastAccelerateInvoker = callLambda.Compile();

Console.WriteLine($"Поточна швидкість авто до виклику: {myCar.Speed} км/год");
fastAccelerateInvoker(myCar, 60);
Console.WriteLine($"Поточна швидкість авто після fastAccelerateInvoker(60): {myCar.Speed} км/год\n");

// -----------------------------------------------------------------------
// Крок 3: Швидка фабрика об'єктів (Заміна Activator.CreateInstance)
// -----------------------------------------------------------------------
Console.WriteLine("--------------------------------------------------------------------------");
Console.WriteLine("--- Крок 3: Генерація фабрики об'єктів (Заміна Activator.CreateInstance) ---");
Console.WriteLine("--------------------------------------------------------------------------\n");

ConstructorInfo sportsCarCtor = typeof(SportsCar).GetConstructor(new[] { typeof(string), typeof(int), typeof(bool) })!;

// Класичний шлях Activator:
// var slowCar = (SportsCar)Activator.CreateInstance(typeof(SportsCar), "Червоний", 120, true)!;

// Шлях Expression Trees:
// Будуємо вираз виду: (string color, int speed, bool hasTurbo) => new SportsCar(color, speed, hasTurbo)
ParameterExpression colorArg = Expression.Parameter(typeof(string), "color");
ParameterExpression speedArg = Expression.Parameter(typeof(int), "speed");
ParameterExpression turboArg = Expression.Parameter(typeof(bool), "hasTurbo");
NewExpression newInstanceExpr = Expression.New(sportsCarCtor, colorArg, speedArg, turboArg);

var factoryLambda = Expression.Lambda<Func<string, int, bool, SportsCar>>(newInstanceExpr, colorArg, speedArg, turboArg);

Console.WriteLine($"Сконструйований вираз фабрики: {factoryLambda}");

Func<string, int, bool, SportsCar> fastSportsCarFactory = factoryLambda.Compile();

SportsCar ferrari = fastSportsCarFactory("Червоний", 120, true);
Console.WriteLine($"Створено об'єкт через швидку фабрику: Колір = {ferrari.Color}, Швидкість = {ferrari.Speed} км/год, Турбо = {ferrari.HasTurbo}");

Console.WriteLine("\n==========================================================================");
Console.WriteLine(" Підсумок: Завдяки генерації делегатів через Expression Trees ми повністю");
Console.WriteLine(" усуваємо оверхед рефлексії на виклики GetValue, Invoke та CreateInstance.");
Console.WriteLine(" Саме це забезпечує багаторазовий бустінг швидкодії, який ми виміряємо в наступному прикладі!");
Console.WriteLine("==========================================================================");
