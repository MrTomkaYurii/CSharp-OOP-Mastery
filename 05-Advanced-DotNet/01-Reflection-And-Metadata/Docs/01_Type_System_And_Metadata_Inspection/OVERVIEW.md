# 01. Type System & Metadata Inspection: Загальний огляд та зріз методів

> **Solution-тег (Solution Folder)**: `01. Type System & Metadata Inspection`  
> **Проєкти у тегу**:
> - [00-Shared/Common/Car.cs](../../00-Shared/Common/Car.cs) — Базові доменні сутності (`Car`, `SportsCar`, `ICar`).
> - [01-TypeofDemo/Program.cs](../../01-TypeofDemo/Program.cs) — Отримання метаданих під час компіляції (`typeof`), токени типів.
> - [02-ObjectGetTypeDemo/Program.cs](../../02-ObjectGetTypeDemo/Program.cs) — Отримання точного рантайм-типу (`GetType()`), поліморфізм, boxing.
> - [03-TypeGetTypeDemo/Program.cs](../../03-TypeGetTypeDemo/Program.cs) — Динамічний пошук за рядковим ім'ям (`Type.GetType`), `AssemblyQualifiedName`.
> - [04-TypePropertiesDemo/Program.cs](../../04-TypePropertiesDemo/Program.cs) — Характеристики та прапорці типу (`IsClass`, `IsValueType`, `IsInterface`, `BaseType`).
> - [05-AssemblyDemo/Program.cs](../../05-AssemblyDemo/Program.cs) — Дослідження фізичних збірок (`Assembly.Load`, маніфест, перелік типів).
> - [06-GetInterfacesDemo/Program.cs](../../06-GetInterfacesDemo/Program.cs) — Дослідження контрактів (`GetInterfaces`, `IsAssignableFrom`, `GetInterfaceMap`).

---

## 1. Концептуальний зріз розділу

Перший розділ курсу присвячений фундаментальному входу в рефлексію .NET — дослідженню системи типів та структури двійкових метаданих у збірках.

### Охоплені ключові класи:
1. [Type.md](Type.md) — центральний клас метаданих у .NET (`System.Type`), через який відбувається вся подальша інспекція програми.
2. [Assembly.md](Assembly.md) — дескриптор фізичної збірки (`System.Reflection.Assembly`), що відповідає за завантаження коду в пам'ять та перелік його типів.
3. [MemberInfo.md](MemberInfo.md) — абстрактний базовий клас (`System.Reflection.MemberInfo`) для всіх елементів програми (полів, методів, конструкторів).

---

## 2. Зведена матриця методів та властивостей (Використані vs Загальні в BCL)

Нижче наведено порівняльний зріз API, що продемонстрований у практичних прикладах цього розділу, порівняно з повним набором методів у бібліотеці базових класів .NET (BCL).

| Клас | Метод / Властивість | Статус у курсі | Опис призначення | Приклад у коді | Документація MSDN |
| :--- | :--- | :---: | :--- | :--- | :--- |
| `Type` | `typeof(T)` | Використано | Отримання дескриптора типу на етапі компіляції через метатокен CLR | [01-TypeofDemo/Program.cs](../../01-TypeofDemo/Program.cs#L14-L24) | [typeof operator](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/type-testing-and-cast#typeof-operator) |
| `object` | `GetType()` | Використано | Отримання точного рантайм-типу екземпляра через покажчик MethodTable | [02-ObjectGetTypeDemo/Program.cs](../../02-ObjectGetTypeDemo/Program.cs#L15-L25) | [Object.GetType](https://learn.microsoft.com/en-us/dotnet/api/system.object.gettype) |
| `Type` | `Type.GetType(string)` | Використано | Динамічний пошук та завантаження типу за його текстовим ім'ям | [03-TypeGetTypeDemo/Program.cs](../../03-TypeGetTypeDemo/Program.cs#L14-L28) | [Type.GetType](https://learn.microsoft.com/en-us/dotnet/api/system.type.gettype) |
| `Type` | `TypeHandle` | Використано | Отримання низькорівневого нативного дескриптора типу (`RuntimeTypeHandle`) | [01-TypeofDemo/Program.cs](../../01-TypeofDemo/Program.cs#L26-L34) | [Type.TypeHandle](https://learn.microsoft.com/en-us/dotnet/api/system.type.typehandle) |
| `Type` | `AssemblyQualifiedName` | Використано | Повна однозначна назва типу включно з назвою збірки, версією та токеном ключа | [03-TypeGetTypeDemo/Program.cs](../../03-TypeGetTypeDemo/Program.cs#L30-L40) | [Type.AssemblyQualifiedName](https://learn.microsoft.com/en-us/dotnet/api/system.type.assemblyqualifiedname) |
| `Type` | `FullName` / `Name` | Використано | Повне ім'я типу з простором імен та коротке ім'я типу | [03-TypeGetTypeDemo/Program.cs](../../03-TypeGetTypeDemo/Program.cs#L42-L52) | [Type.FullName](https://learn.microsoft.com/en-us/dotnet/api/system.type.fullname) |
| `Type` | `IsClass` / `IsValueType` | Використано | Перевірка, чи є тип посилальним (`class`) або типом значення (`struct`) | [04-TypePropertiesDemo/Program.cs](../../04-TypePropertiesDemo/Program.cs#L16-L26) | [Type.IsClass](https://learn.microsoft.com/en-us/dotnet/api/system.type.isclass) |
| `Type` | `IsInterface` / `IsAbstract` | Використано | Перевірка на інтерфейс або абстрактний базовий клас | [04-TypePropertiesDemo/Program.cs](../../04-TypePropertiesDemo/Program.cs#L28-L38) | [Type.IsInterface](https://learn.microsoft.com/en-us/dotnet/api/system.type.isinterface) |
| `Type` | `IsSealed` / `IsEnum` | Використано | Перевірка на запечатаний тип (`sealed`) або перерахування (`enum`) | [04-TypePropertiesDemo/Program.cs](../../04-TypePropertiesDemo/Program.cs#L40-L50) | [Type.IsSealed](https://learn.microsoft.com/en-us/dotnet/api/system.type.issealed) |
| `Type` | `BaseType` | Використано | Отримання безпосереднього класу-предка в ієрархії успадкування | [04-TypePropertiesDemo/Program.cs](../../04-TypePropertiesDemo/Program.cs#L52-L62) | [Type.BaseType](https://learn.microsoft.com/en-us/dotnet/api/system.type.basetype) |
| `Type` | `GetInterfaces()` | Використано | Отримання масиву всіх інтерфейсів, які реалізує даний тип | [06-GetInterfacesDemo/Program.cs](../../06-GetInterfacesDemo/Program.cs#L14-L24) | [Type.GetInterfaces](https://learn.microsoft.com/en-us/dotnet/api/system.type.getinterfaces) |
| `Type` | `IsAssignableFrom(Type)` | Використано | Перевірка поліморфної сумісності (чи можна присвоїти екземпляр змінній) | [06-GetInterfacesDemo/Program.cs](../../06-GetInterfacesDemo/Program.cs#L26-L38) | [Type.IsAssignableFrom](https://learn.microsoft.com/en-us/dotnet/api/system.type.isassignablefrom) |
| `Type` | `GetInterfaceMap(Type)` | Використано | Отримання точної таблиці зіставлення методів інтерфейсу з методами класу | [06-GetInterfacesDemo/Program.cs](../../06-GetInterfacesDemo/Program.cs#L40-L56) | [Type.GetInterfaceMap](https://learn.microsoft.com/en-us/dotnet/api/system.type.getinterfacemap) |
| `Type` | `IsSubclassOf(Type)` | Загальний BCL | Перевірка, чи є тип прямим або непрямим нащадком вказаного класу | Довідник [Type.md](Type.md) | [Type.IsSubclassOf](https://learn.microsoft.com/en-us/dotnet/api/system.type.issubclassof) |
| `Type` | `IsInstanceOfType(object)` | Загальний BCL | Перевірка, чи є об'єкт екземпляром даного типу (еквівалент оператора `is`) | Довідник [Type.md](Type.md) | [Type.IsInstanceOfType](https://learn.microsoft.com/en-us/dotnet/api/system.type.isinstanceoftype) |
| `Type` | `GetNestedTypes()` | Загальний BCL | Отримання списку внутрішніх (вкладених) типів | Довідник [Type.md](Type.md) | [Type.GetNestedTypes](https://learn.microsoft.com/en-us/dotnet/api/system.type.getnestedtypes) |
| `Assembly` | `GetExecutingAssembly()` | Використано | Отримання збірки, в якій виконується поточний вихідний код | [05-AssemblyDemo/Program.cs](../../05-AssemblyDemo/Program.cs#L14-L22) | [Assembly.GetExecutingAssembly](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assembly.getexecutingassembly) |
| `Assembly` | `Assembly.Load(string)` | Використано | Завантаження збірки в домен за її коротким або повним іменем | [05-AssemblyDemo/Program.cs](../../05-AssemblyDemo/Program.cs#L24-L34) | [Assembly.Load](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assembly.load) |
| `Assembly` | `Assembly.LoadFrom(path)` | Використано | Завантаження збірки за абсолютним або відносним шляхом файлу на диску | [05-AssemblyDemo/Program.cs](../../05-AssemblyDemo/Program.cs#L36-L46) | [Assembly.LoadFrom](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assembly.loadfrom) |
| `Assembly` | `GetTypes()` | Використано | Отримання всіх типів, визначених у збірці (включно з внутрішніми `internal`) | [05-AssemblyDemo/Program.cs](../../05-AssemblyDemo/Program.cs#L48-L58) | [Assembly.GetTypes](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assembly.gettypes) |
| `Assembly` | `GetExportedTypes()` | Використано | Отримання лише публічних типів (`public`), видимих за межами збірки | [05-AssemblyDemo/Program.cs](../../05-AssemblyDemo/Program.cs#L60-L70) | [Assembly.GetExportedTypes](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assembly.getexportedtypes) |
| `Assembly` | `GetName()` | Використано | Отримання структурованого об'єкта `AssemblyName` з версією та культурою | [05-AssemblyDemo/Program.cs](../../05-AssemblyDemo/Program.cs#L72-L80) | [Assembly.GetName](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assembly.getname) |
| `Assembly` | `GetManifestResourceNames()` | Загальний BCL | Перелік вбудованих у файл збірки ресурсів (файлів, зображень, конфігів) | Довідник [Assembly.md](Assembly.md) | [Assembly.GetManifestResourceNames](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assembly.getmanifestresourcenames) |
| `Assembly` | `GetEntryAssembly()` | Загальний BCL | Отримання збірки процесу, що містить точку входу програми (`Main`) | Довідник [Assembly.md](Assembly.md) | [Assembly.GetEntryAssembly](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.assembly.getentryassembly) |
| `MemberInfo` | `Name` | Використано | Ім'я сутності метаданих | [04-TypePropertiesDemo/Program.cs](../../04-TypePropertiesDemo/Program.cs#L14) | [MemberInfo.Name](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.memberinfo.name) |
| `MemberInfo` | `DeclaringType` | Використано | Тип, у якому безпосередньо оголошено цей член програми | [06-GetInterfacesDemo/Program.cs](../../06-GetInterfacesDemo/Program.cs#L48) | [MemberInfo.DeclaringType](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.memberinfo.declaringtype) |
| `MemberInfo` | `ReflectedType` | Загальний BCL | Тип об'єкта, через який було отримано дескриптор члена | Довідник [MemberInfo.md](MemberInfo.md) | [MemberInfo.ReflectedType](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.memberinfo.reflectedtype) |
| `MemberInfo` | `MemberType` | Загальний BCL | Бітова категорія члена (`Field`, `Method`, `Property`, `TypeInfo` тощо) | Довідник [MemberInfo.md](MemberInfo.md) | [MemberInfo.MemberType](https://learn.microsoft.com/en-us/dotnet/api/system.reflection.memberinfo.membertype) |

---

## 3. Статистичний зріз охоплення API

- **Методів та властивостей досліджено в прикладах**: 19
- **Загальних методів API BCL задокументовано додатково**: 7
- **Сумарно в довіднику розділу**: 26 API елементів
- **Відсоток фокусного практичного охоплення**: ~73% (найбільш уживані операції повсякденної розробки)
