# 01. Спадкування та Ієрархії Класів (`01-Inheritance`)

Підмодуль курсу **«C# & OOP Mastery»**, який послідовно розкриває механізми спадкування: від базового доступу до успадкованих членів та конструкторів `base(...)` — до віртуальних методів (`virtual`/`override` проти `new`), абстрактних каркасів (`abstract`), приведення типів в ієрархії (`Upcasting`/`Downcasting`/`is`/`as`) та фіналізації ієрархії через запечатані класи (`sealed`) і методи розширення.

---

## Структура Solution (`OOP-Inheritance.sln`) за Solution-теками

| Solution Folder | Гайд / Документація | Проєкти у секції |
|:---|:---|:---|
| **01. Базове спадкування, ключове слово base та конструктори** | [`01_Inheritance_Basics_And_Base_Keyword.md`](Docs/01_Inheritance_Basics_And_Base_Keyword.md) | `SOLIS-p177`, `SOLIS-p180`, `SOLIS-p181`, `SOLIS-p196`, `Ex-ThisInConstructor` |
| **02. Віртуальні члени (virtual), Перевизначення (override) vs Приховування (new)** | [`02_Virtual_Override_And_New_Hiding.md`](Docs/02_Virtual_Override_And_New_Hiding.md) | `SOLIS-p183`, `SOLIS-p185`, `SOLIS-p187`, `SOLIS-p188`, `SOLIS-p189` |
| **03. Абстрактні класи та абстрактні члени (abstract)** | [`03_Abstract_Classes_And_Members.md`](Docs/03_Abstract_Classes_And_Members.md) | `SOLIS-p207`, `SOLIS-p208` |
| **04. Приведення типів в ієрархії (Upcasting, Downcasting, is, as)** | [`04_Type_Casting_Upcasting_Downcasting.md`](Docs/04_Type_Casting_Upcasting_Downcasting.md) | `Ex-Upcasting`, `Ex-Downcasting`, `Ex-AsOperator` |
| **05. Запечатані класи (sealed), Статичні класи та Методи розширення** | [`05_Sealed_Static_And_Extension_Methods.md`](Docs/05_Sealed_Static_And_Extension_Methods.md) | `Ex-SealedClass`, `SOLIS-p210`, `SOLIS-p212`, `SOLIS-p214`, `Ex-ExtensionMethods` |

---

## Збірка та запуск

```bash
dotnet build OOP-Inheritance.sln
dotnet run --project Ex-Upcasting/Ex-Upcasting.csproj
```
