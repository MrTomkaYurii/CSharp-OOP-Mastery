# 03. Зв'язки сутностей (`1:1`, `1:N`, `N:M`), Каскадне видалення та Завантаження даних

## 1. Типи зв'язків між сутностями

У термінології EF Core будь-який реляційний зв'язок має дві сторони:
- **Головна сутність (Principal Entity)** — сутність, яка містить первинний (або альтернативний) ключ, на який посилаються інші таблиці.
- **Залежна сутність (Dependent Entity)** — сутність, яка містить властивість **зовнішнього ключа (Foreign Key)**.

### 1.1. Зв'язок Один-до-Одного (`1:1`)
Класичний приклад: `Employee` (головна сутність) та `EmployeePassport` (залежна сутність). Залежна сторона обов'язково повинна мати унікальний зовнішній ключ `EmployeeId`:
```csharp
modelBuilder.Entity<Employee>()
    .HasOne(e => e.Passport)
    .WithOne(p => p.Employee)
    .HasForeignKey<EmployeePassport>(p => p.EmployeeId);
```

### 1.2. Зв'язок Один-до-Багатьох (`1:N`) та самопосилання (Self-Referencing)
Відділ `Department` має колекцію `List<Employee> Employees`, а кожен `Employee` має зовнішній ключ `DepartmentId` та навігаційне посилання `Department`.
Також сутність може посилатися сама на себе (наприклад, `Employee.MentorId` -> `Employee.Mentor`), де зовнішній ключ є опціональним (`int? MentorId`).

### 1.3. Зв'язок Багато-до-Багатьох (`N:M`) з корисним навантаженням (Payload)
Якщо у проміжній таблиці між `Employee` та `Project` потрібно зберігати додаткові бізнес-стовпці (`RoleOnProject`, `AllocatedHoursPerWeek`), використовується метод `.UsingEntity<ProjectAssignment>(...)` зі складеним ключем `{ EmployeeId, ProjectId }`.

---

## 2. Поведінка при видаленні (`DeleteBehavior`)

Що відбувається із залежними записами, коли видаляється головна сутність?
- **`DeleteBehavior.Cascade`** — залежні рядки автоматично видаляються самою СУБД (використовується за замовчуванням для обов'язкових зовнішніх ключів `int DepartmentId`).
- **`DeleteBehavior.SetNull`** — у залежних рядках зовнішній ключ встановлюється в `NULL` (можливо лише для nullable-ключів `int? MentorId`).
- **`DeleteBehavior.Restrict` / `NoAction`** — забороняє видалення головної сутності, доки на неї посилається хоча б один залежний запис.

---

## 3. Стратегії завантаження пов'язаних даних та проблема $N+1$

За замовчуванням EF Core **не завантажує** навігаційні властивості автоматично (вони дорівнюють `null` або порожній колекції). Існує три основні стратегії завантаження:

### 1. Eager Loading (Жадібне завантаження — `Include` / `ThenInclude`)
Завантажує головні сутності разом із пов'язаними таблицями **за один SQL-запит із `JOIN`** (або в режимі `AsSplitQuery()`). Починаючи з EF Core 5+, всередині `Include` підтримується фільтрація та сортування:
```csharp
var departments = await context.Departments
    .Include(d => d.Employees.OrderBy(e => e.FullName))
        .ThenInclude(e => e.Passport)
    .ToListAsync();
```

### 2. Explicit Loading (Явне завантаження — `context.Entry(entity)`)
Дозволяє пізніше довантажити колекцію або посилання для вже отриманого в пам'ять об'єкта, ба більше — відфільтрувати або порахувати кількість через `.Query()` без завантаження всіх об'єктів:
```csharp
int count = await context.Entry(rndDept).Collection(d => d.Employees).Query().CountAsync();
await context.Entry(rndDept).Collection(d => d.Employees).Query().Where(e => e.Position.Contains("Senior")).LoadAsync();
```

### 3. Lazy Loading (Ліниве завантаження) та загроза $N+1$
При Lazy Loading звернення до властивості `dept.Employees` у циклі `foreach` непомітно викликає окремий SQL-запит для кожного з $N$ відділів (1 запит на список відділів + $N$ запитів на співробітників кожного відділу = **$N+1$ запитів**). Тому у високопродуктивних застосунках перевагу віддають явному **Eager Loading (`Include`)** або проєкціям **`Select`**.
