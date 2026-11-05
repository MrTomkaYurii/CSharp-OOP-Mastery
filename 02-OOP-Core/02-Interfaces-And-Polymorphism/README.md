# 02. Інтерфейси та Поведінкові Контракти (`02-Interfaces-And-Polymorphism`)

Підмодуль курсу **«C# & OOP Mastery»**, присвячений інтерфейсам: від оголошення контрактів та приведення посилань (`as`) — до множинної та явної реалізації (`Explicit Implementation`), методів за замовчуванням у C# 8+ (`DIM`), стандартних інтерфейсів платформи .NET BCL (`IComparable`, `IComparer`, `IEnumerable`, `ICloneable`, `IDisposable`) та моделювання зв'язків між об'єктами (Асоціація, Агрегація, Композиція).

---

## Структура Solution (`OOP - Interfaces.sln`) за Solution-теками

| Solution Folder | Гайд / Документація | Проєкти у секції |
|:---|:---|:---|
| **01. Контракти інтерфейсів (Оголошення, реалізація, інтерфейсні посилання)** | [`01_Interface_Contracts_And_References.md`](Docs/01_Interface_Contracts_And_References.md) | `p389[SOLIS]`, `p397[SOLIS]`, `p399[SOLIS]`, `p413[SOLIS]` |
| **02. Множинна реалізація, колізії імен та Явна реалізація (Explicit Implementation)** | [`02_Multiple_And_Explicit_Implementation.md`](Docs/02_Multiple_And_Explicit_Implementation.md) | `p401[SOLIS]`, `p402[SOLIS]`, `p404[SOLIS]`, `p408[SOLIS]` |
| **03. Еволюція інтерфейсів (Default Interface Methods у C# 8+)** | [`03_Modern_Interfaces_CSharp8_Plus.md`](Docs/03_Modern_Interfaces_CSharp8_Plus.md) | `InterfacesAfterCSharpVer8` |
| **04. Стандартні інтерфейси .NET BCL (IComparable, IComparer, IEnumerable, ICloneable, IDisposable)** | [`04_Standard_BCL_Interfaces.md`](Docs/04_Standard_BCL_Interfaces.md) | `p390[SOLIS]`, `p393[SOLIS]`, `ex-Icomparable01`, `ex-IComparable02`, `ex-IComparer01`, `ex-Icomparer02`, `ex-IEnumerable01`, `ex-IEnumerable02`, `ex-IClonable01`, `ex-IClonable02`, `ex-IDisposable01`, `ex-IDisposable02` |
| **05. Інтерфейси у зв'язках об'єктів (Асоціація, Агрегація, Композиція)** | [`05_Interfaces_In_UML_Relationships.md`](Docs/05_Interfaces_In_UML_Relationships.md) | `InterfacesAssociation`, `InterfacesAggregation`, `InterfacesComposition` |

---

## Збірка та запуск

```bash
dotnet build "OOP - Interfaces.sln"
dotnet run --project InterfacesComposition/InterfacesComposition.csproj
```
