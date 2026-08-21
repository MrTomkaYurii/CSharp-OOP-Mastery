using System;
using System.Reflection;

// ═══════════════════════════════════════════════════════════════════════════
// CUSTOM ATTRIBUTE - Власний атрибут для документування та аудиту методів
// ═══════════════════════════════════════════════════════════════════════════

// AttributeUsage визначає, де можна використовувати атрибут
// AttributeTargets.Method - тільки на методах (більш специфічно, ніж All)
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
class MethodInfoAttribute : Attribute
{
    public string MethodType { get; private set; }  // "Modifier" або "Accessor"
    public string Description { get; private set; }  // Опис що робить метод
    public bool RequiresAudit { get; set; }         // Чи потрібен аудит викликів

    // Конструктор атрибута - обов'язкові параметри
    public MethodInfoAttribute(string methodType, string description)
    {
        this.MethodType = methodType;
        this.Description = description;
        this.RequiresAudit = false;  // За замовчуванням аудит вимкнено
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// EMPLOYEE CLASS - Клас працівника з атрибутами
// ═══════════════════════════════════════════════════════════════════════════

class Employee
{
    private int empID;
    private string empName;
    private double salary;

    // КРИТИЧНА ТОЧКА 1: Атрибут з RequiresAudit = true
    // Цей метод змінює дані, тому його виклики потрібно логувати
    [MethodInfo("Modifier", "Встановлює дані співробітника", RequiresAudit = true)]
    public void SetDetails(int id, string name, double sal)
    {
        empID = id;
        empName = name;
        salary = sal;
    }

    // КРИТИЧНА ТОЧКА 2: Accessor методи - тільки читання, аудит не потрібен
    [MethodInfo("Accessor", "Повертає ID співробітника")]
    public int GetEmpID()
    {
        return empID;
    }

    [MethodInfo("Accessor", "Повертає ім'я співробітника")]
    public string GetEmpName()
    {
        return empName;
    }

    // КРИТИЧНА ТОЧКА 3: Accessor для зарплати - чутливі дані, потрібен аудит
    [MethodInfo("Accessor", "Повертає зарплату співробітника (конфіденційно)", RequiresAudit = true)]
    public double GetSalary()
    {
        return salary;
    }

    // КРИТИЧНА ТОЧКА 4: Метод для підвищення зарплати - обов'язково аудитувати
    [MethodInfo("Modifier", "Підвищує зарплату на вказаний відсоток", RequiresAudit = true)]
    public void IncreaseSalary(double percentage)
    {
        salary += salary * (percentage / 100);
    }

    // Метод БЕЗ атрибута - для демонстрації фільтрації
    public void PrintInfo()
    {
        Console.WriteLine($"[{empID}] {empName} - {salary} грн");
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// AUDIT LOGGER - Система аудиту викликів методів
// ═══════════════════════════════════════════════════════════════════════════

static class AuditLogger
{
    // КРИТИЧНА ТОЧКА 5: Логування викликів методів через Reflection
    // Цей метод перевіряє атрибути та записує в лог, якщо RequiresAudit = true
    public static object InvokeWithAudit(object obj, string methodName, params object[] parameters)
    {
        Type type = obj.GetType();
        MethodInfo method = type.GetMethod(methodName);

        if (method == null)
        {
            Console.WriteLine($"❌ Метод '{methodName}' не знайдено!");
            return null;
        }

        // Отримуємо атрибут методу через Reflection
        var attributes = method.GetCustomAttributes(typeof(MethodInfoAttribute), false);

        if (attributes.Length > 0)
        {
            MethodInfoAttribute attr = (MethodInfoAttribute)attributes[0];

            // КРИТИЧНА ТОЧКА 6: Перевірка чи потрібен аудит
            if (attr.RequiresAudit)
            {
                Console.WriteLine("\n" + new string('═', 70));
                Console.WriteLine($"📋 АУДИТ ВИКЛИКУ МЕТОДУ");
                Console.WriteLine(new string('─', 70));
                Console.WriteLine($"Час: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                Console.WriteLine($"Метод: {methodName}");
                Console.WriteLine($"Тип: {attr.MethodType}");
                Console.WriteLine($"Опис: {attr.Description}");
                Console.WriteLine($"Параметри: {(parameters.Length > 0 ? string.Join(", ", parameters) : "немає")}");
                Console.WriteLine($"Об'єкт: {type.Name}");
            }
        }

        // Викликаємо метод
        object result = method.Invoke(obj, parameters);

        // Якщо є аудит - логуємо результат
        if (attributes.Length > 0)
        {
            MethodInfoAttribute attr = (MethodInfoAttribute)attributes[0];
            if (attr.RequiresAudit)
            {
                Console.WriteLine($"Результат: {(result != null ? result.ToString() : "void")}");
                Console.WriteLine(new string('═', 70) + "\n");
            }
        }

        return result;
    }

    // КРИТИЧНА ТОЧКА 7: Генерація документації з атрибутів
    public static void GenerateDocumentation(Type type)
    {
        Console.WriteLine("\n" + new string('═', 70));
        Console.WriteLine($"📚 АВТОМАТИЧНА ДОКУМЕНТАЦІЯ: {type.Name}");
        Console.WriteLine(new string('═', 70));

        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        foreach (var method in methods)
        {
            var attributes = method.GetCustomAttributes(typeof(MethodInfoAttribute), false);

            if (attributes.Length > 0)
            {
                MethodInfoAttribute attr = (MethodInfoAttribute)attributes[0];

                Console.WriteLine($"\n▶ {method.Name}()");
                Console.WriteLine($"  Тип: {attr.MethodType}");
                Console.WriteLine($"  Опис: {attr.Description}");
                Console.WriteLine($"  Параметри: {string.Join(", ", Array.ConvertAll(method.GetParameters(), p => $"{p.ParameterType.Name} {p.Name}"))}");
                Console.WriteLine($"  Повертає: {method.ReturnType.Name}");
                Console.WriteLine($"  Потребує аудит: {(attr.RequiresAudit ? "✓ Так" : "✗ Ні")}");
            }
        }

        Console.WriteLine("\n" + new string('═', 70));
    }

    // КРИТИЧНА ТОЧКА 8: Статистика по типах методів
    public static void ShowMethodStatistics(Type type)
    {
        Console.WriteLine("\n" + new string('═', 70));
        Console.WriteLine($"📊 СТАТИСТИКА МЕТОДІВ: {type.Name}");
        Console.WriteLine(new string('═', 70));

        int modifiers = 0;
        int accessors = 0;
        int auditRequired = 0;

        var methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);

        foreach (var method in methods)
        {
            var attributes = method.GetCustomAttributes(typeof(MethodInfoAttribute), false);

            if (attributes.Length > 0)
            {
                MethodInfoAttribute attr = (MethodInfoAttribute)attributes[0];

                if (attr.MethodType == "Modifier") modifiers++;
                if (attr.MethodType == "Accessor") accessors++;
                if (attr.RequiresAudit) auditRequired++;
            }
        }

        Console.WriteLine($"Методів-Modifier (змінюють дані): {modifiers}");
        Console.WriteLine($"Методів-Accessor (читають дані): {accessors}");
        Console.WriteLine($"Методів з обов'язковим аудитом: {auditRequired}");
        Console.WriteLine($"Всього методів з атрибутами: {modifiers + accessors}");
        Console.WriteLine(new string('═', 70));
    }
}

// ═══════════════════════════════════════════════════════════════════════════
// MAIN PROGRAM
// ═══════════════════════════════════════════════════════════════════════════

public class EmployeeManagementSystem
{
    public static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("╔════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  СИСТЕМА УПРАВЛІННЯ ПРАЦІВНИКАМИ З АУДИТОМ ТА ДОКУМЕНТАЦІЄЮ       ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");

        // КРИТИЧНА ТОЧКА 9: Генерація документації з атрибутів
        AuditLogger.GenerateDocumentation(typeof(Employee));

        // КРИТИЧНА ТОЧКА 10: Статистика методів
        AuditLogger.ShowMethodStatistics(typeof(Employee));

        // ═══════════════════════════════════════════════════════════════════
        // РОБОТА З ПРАЦІВНИКАМИ З АВТОМАТИЧНИМ АУДИТОМ
        // ═══════════════════════════════════════════════════════════════════

        Employee emp1 = new Employee();

        Console.WriteLine("\n\n🔹 СЦЕНАРІЙ 1: Створення нового працівника");
        Console.WriteLine(new string('─', 70));

        // КРИТИЧНА ТОЧКА 11: Виклик методу з аудитом (SetDetails має RequiresAudit = true)
        AuditLogger.InvokeWithAudit(emp1, "SetDetails", 2424, "Параска Іван", 45000.0);

        Console.WriteLine("\n🔹 СЦЕНАРІЙ 2: Читання базової інформації (без аудиту)");
        Console.WriteLine(new string('─', 70));

        // GetEmpID та GetEmpName НЕ мають RequiresAudit, тому логування не буде
        int id = (int)AuditLogger.InvokeWithAudit(emp1, "GetEmpID");
        string name = (string)AuditLogger.InvokeWithAudit(emp1, "GetEmpName");

        Console.WriteLine($"✓ ID: {id}");
        Console.WriteLine($"✓ Ім'я: {name}");

        Console.WriteLine("\n🔹 СЦЕНАРІЙ 3: Читання конфіденційних даних (З аудитом)");
        Console.WriteLine(new string('─', 70));

        // КРИТИЧНА ТОЧКА 12: GetSalary має RequiresAudit = true, бо це конфіденційні дані
        double salary = (double)AuditLogger.InvokeWithAudit(emp1, "GetSalary");
        Console.WriteLine($"✓ Зарплата отримана: {salary} грн");

        Console.WriteLine("\n🔹 СЦЕНАРІЙ 4: Підвищення зарплати (З аудитом)");
        Console.WriteLine(new string('─', 70));

        // КРИТИЧНА ТОЧКА 13: IncreaseSalary змінює дані - обов'язковий аудит
        AuditLogger.InvokeWithAudit(emp1, "IncreaseSalary", 15.0);

        Console.WriteLine("\n🔹 СЦЕНАРІЙ 5: Перевірка нової зарплати");
        Console.WriteLine(new string('─', 70));
        double newSalary = (double)AuditLogger.InvokeWithAudit(emp1, "GetSalary");
        Console.WriteLine($"✓ Нова зарплата: {newSalary} грн");

        // ═══════════════════════════════════════════════════════════════════
        // ДРУГИЙ ПРАЦІВНИК ДЛЯ ДЕМОНСТРАЦІЇ
        // ═══════════════════════════════════════════════════════════════════

        Console.WriteLine("\n\n🔹 СЦЕНАРІЙ 6: Робота з другим працівником");
        Console.WriteLine(new string('─', 70));

        Employee emp2 = new Employee();
        AuditLogger.InvokeWithAudit(emp2, "SetDetails", 2425, "Шевченко Марія", 52000.0);

        Console.WriteLine("\n🔹 СЦЕНАРІЙ 7: Порівняння зарплат (аудит обох викликів)");
        Console.WriteLine(new string('─', 70));

        double salary1 = (double)AuditLogger.InvokeWithAudit(emp1, "GetSalary");
        double salary2 = (double)AuditLogger.InvokeWithAudit(emp2, "GetSalary");

        Console.WriteLine($"\n💰 Різниця в зарплаті: {Math.Abs(salary1 - salary2)} грн");

        // ═══════════════════════════════════════════════════════════════════
        // ПІДСУМОК
        // ═══════════════════════════════════════════════════════════════════

        Console.WriteLine("\n\n╔════════════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║  ПІДСУМОК: ПЕРЕВАГИ ВИКОРИСТАННЯ CUSTOM ATTRIBUTES                 ║");
        Console.WriteLine("╚════════════════════════════════════════════════════════════════════╝");
        Console.WriteLine("\n✓ Автоматична генерація документації з коду");
        Console.WriteLine("✓ Вибіркове логування тільки критичних операцій");
        Console.WriteLine("✓ Статистика та аналіз структури класів");
        Console.WriteLine("✓ Аудит доступу до конфіденційних даних");
        Console.WriteLine("✓ Централізована система безпеки без зміни бізнес-логіки");
        Console.WriteLine("\n💡 Атрибути дозволяють додавати метадані до коду,");
        Console.WriteLine("   які потім можна читати через Reflection для");
        Console.WriteLine("   автоматизації рутинних завдань!\n");
    }
}