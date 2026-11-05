# 03. Поліморфізм та Архітектурне Проектування Ієрархій (`03-Polymorphism-And-Hierarchy-Design`)

Синтезуючий підмодуль розділу **`02-OOP-Core`**, який переводить синтаксис спадкування та інтерфейсів у площину реального ООП-дизайну:
* Віртуальні методи базового для всіх типів класу **`System.Object`** (`ToString`, `Equals`, `GetHashCode`, `MemberwiseClone`).
* Правильна **інкапсуляція в ієрархіях** через `protected`, `protected set`, `private protected` без розкриття приватних полів базового класу.
* Вибір між **`abstract class` (Template Method)** та **`interface` (Role Contract)**.
* Заміна розгалужених `switch/if` на **поліморфізм** та перевага **Композиції над Спадкуванням**.

---

## Структура Solution (`OOP-Polymorphism-And-Design.sln`) за Solution-теками

| Solution Folder | Гайд / Документація | Проєкти у секції |
|:---|:---|:---|
| **01. Корінь ієрархії System.Object та семантика рівності** | [`01-SystemObjectAndEquality.md`](01-SystemObjectAndEquality.md) | [`01-SystemObjectAndEquality`](01-SystemObjectAndEquality/Program.cs) |
| **02. Інкапсуляція в ієрархіях (protected, internal, private protected)** | [`02-HierarchyEncapsulationAndProtected.md`](02-HierarchyEncapsulationAndProtected.md) | [`02-HierarchyEncapsulationAndProtected`](02-HierarchyEncapsulationAndProtected/Program.cs) |
| **03. Абстрактний клас проти Інтерфейсу та патерн Template Method** | [`03-AbstractClassVsInterface.md`](03-AbstractClassVsInterface.md) | [`03-AbstractClassVsInterface`](03-AbstractClassVsInterface/Program.cs) |
| **04. Поліморфізм замість умовних операторів та Композиція проти Спадкування** | [`04-PolymorphismAndCompositionDesign.md`](04-PolymorphismAndCompositionDesign.md) | [`04-PolymorphismAndCompositionDesign`](04-PolymorphismAndCompositionDesign/Program.cs) |

---

## Збірка та запуск

```bash
dotnet build OOP-Polymorphism-And-Design.sln
dotnet run --project 04-PolymorphismAndCompositionDesign/04-PolymorphismAndCompositionDesign.csproj
```
