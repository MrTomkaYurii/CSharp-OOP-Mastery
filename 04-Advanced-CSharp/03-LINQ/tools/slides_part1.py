from pptx.util import Inches, Pt
from pptx.enum.text import PP_ALIGN
from pptx.dml.color import RGBColor
from pptx.enum.shapes import MSO_SHAPE
from tools.builder_core import (
    add_bg, add_header, add_bottom_banner, add_code_panel,
    add_theory_card, add_dual_comparison_card, add_table_card,
    ACCENT_INDIGO, ACCENT_SKY, ACCENT_EMERALD, ACCENT_VIOLET,
    ACCENT_AMBER, ACCENT_TEAL, ACCENT_CRIMSON, TEXT_MAIN, TEXT_MUTED
)

def build_part1(prs, blank_layout):
    print("Building Part 1 (Slides 1 - 36)...")

    # ==========================================
    # SLIDE 1: HERO TITLE SLIDE
    # ==========================================
    s1 = prs.slides.add_slide(blank_layout)
    bg1 = s1.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(0), Inches(0), Inches(13.333), Inches(7.500))
    bg1.fill.solid()
    bg1.fill.fore_color.rgb = RGBColor(15, 23, 42)
    bg1.line.fill.background()

    pill = s1.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(0.8), Inches(1.2), Inches(4.5), Inches(0.42))
    pill.fill.solid()
    pill.fill.fore_color.rgb = ACCENT_INDIGO
    pill.line.fill.background()
    p_p = pill.text_frame.paragraphs[0]
    p_p.alignment = PP_ALIGN.CENTER
    p_p.text = "ОБ'ЄКТНО-ОРІЄНТОВАНЕ ПРОГРАМУВАННЯ · ЛЕКЦІЯ №10"
    p_p.font.name = "Calibri"
    p_p.font.size = Pt(11)
    p_p.font.bold = True
    p_p.font.color.rgb = RGBColor(255, 255, 255)

    tb_t = s1.shapes.add_textbox(Inches(0.8), Inches(1.8), Inches(11.7), Inches(2.3))
    tf_t = tb_t.text_frame
    tf_t.word_wrap = True
    p_t1 = tf_t.paragraphs[0]
    p_t1.text = "Language Integrated Query (LINQ)\nу C# та платформі .NET 10"
    p_t1.font.name = "Calibri"
    p_t1.font.size = Pt(42)
    p_t1.font.bold = True
    p_t1.font.color.rgb = RGBColor(255, 255, 255)

    p_t2 = tf_t.add_paragraph()
    p_t2.text = "Повний академічний курс: Архітектура, синтаксис запитів, лямбда-вирази, стандартні оператори та моделі виконання"
    p_t2.font.name = "Calibri"
    p_t2.font.size = Pt(18)
    p_t2.font.color.rgb = RGBColor(148, 163, 184)

    card_data = [
        ("ФУНДАМЕНТ LINQ", "Єдиний декларативний синтаксис", "Запити до об'єктів у пам'яті, реляційних СКБД, XML та JSON"),
        ("15 КАТЕГОРІЙ ОПЕРАТОРІВ", "Повний спектр стандартних методів", "Фільтрація, сортування, групування, реляційні з'єднання та агрегація"),
        ("МОДЕЛІ ВИКОНАННЯ", "Оптимізація та продуктивність", "Відкладене (Deferred) та негайне (Immediate) виконання, конвеєризація")
    ]
    for i, (head, sub, desc) in enumerate(card_data):
        cx = 0.8 + i * 4.0
        c = s1.shapes.add_shape(MSO_SHAPE.RECTANGLE, Inches(cx), Inches(4.5), Inches(3.73), Inches(2.2))
        c.fill.solid()
        c.fill.fore_color.rgb = RGBColor(30, 41, 59)
        c.line.color.rgb = RGBColor(51, 65, 85)
        c.line.width = Pt(1)
        tf_c = c.text_frame
        tf_c.word_wrap = True
        p_c1 = tf_c.paragraphs[0]
        p_c1.text = head
        p_c1.font.name = "Calibri"
        p_c1.font.size = Pt(14)
        p_c1.font.bold = True
        p_c1.font.color.rgb = ACCENT_SKY

        p_c2 = tf_c.add_paragraph()
        p_c2.text = sub
        p_c2.font.name = "Calibri"
        p_c2.font.size = Pt(13)
        p_c2.font.bold = True
        p_c2.font.color.rgb = RGBColor(255, 255, 255)

        p_c3 = tf_c.add_paragraph()
        p_c3.text = desc
        p_c3.font.name = "Calibri"
        p_c3.font.size = Pt(11)
        p_c3.font.color.rgb = RGBColor(148, 163, 184)

    # ==========================================
    # SLIDE 2: ВСТУП ТА АРХІТЕКТУРА LINQ
    # ==========================================
    s2 = prs.slides.add_slide(blank_layout)
    add_bg(s2)
    add_header(s2, "01 · Вступ та Архітектура", "Що таке LINQ: Передумови виникнення та архітектура платформи", 2)
    add_theory_card(s2, 0.8, 1.55, 5.72, 4.25,
        'Революція декларативного підходу в .NET', ACCENT_INDIGO,
        [
            ('Походження:', "LINQ (Language Integrated Query) з'явився у .NET 3.5 / C# 3.0 як засіб уніфікованої роботи з даними."),
            ('Проблема парадигм (Impedance Mismatch):', "Раніше для об'єктів писали імперативні цикли, для SQL — вразливі текстові рядки, для XML — XPath."),
            ('Єдина мова запитів:', 'LINQ інтегрував синтаксис запитів як першокласний елемент C# із повною перевіркою типів під час збірки проєкту.'),
            ('Уніфікований конвеєр:', "Один і той самий стиль запиту застосовується до пам'яті (Objects), баз даних (Entities) та документів (XML/JSON).")
        ],
        '⚡ Архітектурна перевага:',
        'Помилки в назвах властивостей або типах даних виявляються компілятором під час збірки, а не під час аварії у продакшені.'
    )
    add_code_panel(s2, 6.81, 1.55, 5.72, 4.25, [
        '// 1. Чотири ключові провайдери платформи LINQ:',
        "//    • LINQ to Objects (IEnumerable<T> у пам'яті CLR)",
        '//    • LINQ to Entities / EF Core (IQueryable<T> -> SQL)',
        '//    • LINQ to XML (XDocument, XElement, XAttribute)',
        '//    • Parallel LINQ (PLINQ - паралельне виконання)',
        '',
        '// 2. Декларативний запит C# до будь-якого джерела:',
        'int[] numbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };',
        '',
        'var evens = from n in numbers',
        '            where n % 2 == 0',
        '            orderby n descending',
        '            select n;',
        '',
        '// 3. Ітерація по конвеєру результатів:',
        'foreach (var num in evens)',
        '    Console.Write($"{num} ");',
        '',
        '// -> Output: 10 8 6 4 2'
    ], 'Program.cs')
    add_bottom_banner(s2, "💡 Ключовий висновок:",
                      "LINQ інтегрував роботу з реляційними та об'єктними даними безпосередньо у мову C#, ліквідувавши розрив між кодом та СКБД.")
    # ==========================================
    # SLIDE 3: LINQ TO OBJECTS
    # ==========================================
    s3 = prs.slides.add_slide(blank_layout)
    add_bg(s3)
    add_header(s3, "01 · Вступ та Архітектура", "LINQ to Objects: Робота з колекціями у пам'яті процесу", 3)
    add_theory_card(s3, 0.8, 1.55, 5.72, 4.25,
        "Концепція обробки даних у пам'яті CLR", ACCENT_INDIGO,
        [
            ('Цільові джерела даних:', 'Будь-яка колекція, що реалізує інтерфейс IEnumerable<T> (масиви, List<T>, Dictionary<K,V>, HashSet<T>).'),
            ('Простір імен System.Linq:', 'Вся функціональність надається статичним класом Enumerable через ланцюжки методів розширення.'),
            ("Виконання в пам'яті процесу:", 'Запити виконуються процесором за допомогою скомпільованого IL-коду та делегатів Func<T>.'),
            ('Відсутність парсингу:', 'Код запиту не передається як текст (на відміну від SQL чи regex) — він одразу виконується як нативний код .NET.')
        ],
        '⚡ Pull-модель ітератора:',
        'LINQ to Objects не створює нових колекцій наперед. Обчислення виконуються ліниво на вимогу споживача (on-demand via yield return).'
    )
    add_code_panel(s3, 6.81, 1.55, 5.72, 4.25, [
        "// 1. Джерело даних у пам'яті процесу:",
        'string[] names = { "Bill", "Steve", "James", "Mohan", "Jane" };',
        '',
        '// 2. Декларативний запит (Query Syntax):',
        'var myLinqQuery = from name in names',
        "                  where name.Contains('a')",
        '                  select name;',
        '',
        '// 3. Еквівалентний методний запит (Method Syntax):',
        "var methodQuery = names.Where(n => n.Contains('a'));",
        '',
        '// 4. Потокове виконання під час ітерації:',
        'foreach (var name in myLinqQuery)',
        '    Console.WriteLine($"Знайдено: {name}");',
        '',
        '// -> Output:',
        '// -> Знайдено: James',
        '// -> Знайдено: Mohan',
        '// -> Знайдено: Jane'
    ], 'Program.cs')
    add_bottom_banner(s3, "💡 Архітектурна примітка:",
                      "Для роботи з LINQ to Objects достатньо підключити директиву using System.Linq; яка імпортує методи розширення Enumerable.")
    # ==========================================
    # SLIDE 4: ПРАКТИЧНИЙ ПРИКЛАД LINQ TO OBJECTS
    # ==========================================
    s4 = prs.slides.add_slide(blank_layout)
    add_bg(s4)
    add_header(s4, "01 · Вступ та Архітектура", "Практичний приклад: Перший запит LINQ to Objects", 4)
    add_theory_card(s4, 0.8, 1.55, 5.72, 4.25,
        'Анатомія простого запиту фільтрації', ACCENT_SKY,
        [
            ('1. Джерело (Data Source):', 'Масив names реалізує IEnumerable<string>, виступаючи вхідним потоком для конвеєра ітератора.'),
            ('2. Декларація запиту:', "Оголошення запиту створює об'єкт конвеєра, але ще НЕ виконує фактичний перебір даних."),
            ('3. Виконання (Execution):', 'Фактична фільтрація викликається лише під час кроків циклу foreach (через GetEnumerator/MoveNext).'),
            ("4. Стан пам'яті процесу:", 'До моменту ітерації жоден проміжний масив не створюється, мінімізуючи навантаження на збирач сміття (GC).')
        ],
        '⚡ Відкладене виконання:',
        'Якщо змінити масив names перед циклом foreach, запит автоматично поверне вже актуальні змінені дані!'
    )
    add_code_panel(s4, 6.81, 1.55, 5.72, 4.25, [
        "// 1. Джерело даних у пам'яті процесу:",
        'string[] names = { "Bill", "Steve", "James", "Mohan" };',
        '',
        "// 2. Створення об'єкта LINQ-запиту:",
        'var myLinqQuery = from name in names',
        "                  where name.Contains('a')",
        '                  select name;',
        '',
        '// 3. Модифікація джерела перед ітерацією:',
        'names[0] = "Dan"; // Bill -> Dan (Dan містить \'a\'!)',
        '',
        '// 4. Фактичне виконання під час ітерації:',
        'Console.WriteLine("Результати відкладеного запиту:");',
        'foreach (var name in myLinqQuery)',
        '    Console.WriteLine($"-> {name}");',
        '',
        '// -> Output:',
        '// -> Dan',
        '// -> James',
        '// -> Mohan'
    ], 'Program.cs')
    add_bottom_banner(s4, "💡 Важливий момент:",
                      "Запит myLinqQuery зберігає лише інструкцію отримання даних, а не самі дані. Дані зчитуються 'на льоту' під час ітерації.")
    # ==========================================
    # SLIDE 5: IQUERYABLE VS IENUMERABLE
    # ==========================================
    s5 = prs.slides.add_slide(blank_layout)
    add_bg(s5)
    add_header(s5, "01 · Вступ та Архітектура", "Дві гілки абстракції: IEnumerable проти IQueryable", 5)
    add_dual_comparison_card(s5, 0.8, 1.55, 5.72, 4.25,
        "IEnumerable<T> (LINQ to Objects)", ACCENT_INDIGO,
        [
            ("Простір імен:", "System.Collections.Generic, розширення в System.Linq.Enumerable."),
            ("Цільове середовище:", "Колекції в оперативній пам'яті (In-Memory)."),
            ("Параметри предикатів:", "Скомпільовані делегати Func<T, bool> (байт-код IL).")
        ],
        [
            "// IEnumerable: виконання кодом процесора C#",
            "IEnumerable<Student> list = studentList;",
            "var res = list.Where(s => s.Age > 18);",
            "// Дані вже завантажені в оперативну пам'ять!"
        ]
    )
    add_dual_comparison_card(s5, 6.81, 1.55, 5.72, 4.25,
        "IQueryable<T> (LINQ to SQL / Entities)", ACCENT_EMERALD,
        [
            ("Простір імен:", "System.Linq, розширення в System.Linq.Queryable."),
            ("Цільове середовище:", "Віддалені бази даних (SQL Server, PostgreSQL, EF Core)."),
            ("Параметри предикатів:", "Дерева виразів Expression<Func<T, bool>> (AST).")
        ],
        [
            "// IQueryable: трансляція в чистий SQL провайдером",
            "IQueryable<Student> query = dbContext.Students;",
            "var res = query.Where(s => s.Age > 18);",
            "// Генерує SQL: SELECT * FROM Students WHERE Age > 18"
        ]
    )
    add_bottom_banner(s5, "💡 Головна відмінність:",
                      "IEnumerable<T> виконує код у пам'яті C#, а IQueryable<T> передає дерево синтаксису базі даних для генерації SQL.")

    # ==========================================
    # SLIDE 6: КЛАС ENUMERABLE
    # ==========================================
    s6 = prs.slides.add_slide(blank_layout)
    add_bg(s6)
    add_header(s6, "01 · Вступ та Архітектура", "Приклад у стилі Query Syntax (SQL-подібний)", 6)
    add_theory_card(s6, 0.8, 1.55, 5.72, 4.25,
        'Особливості синтаксису запитів у C#', ACCENT_INDIGO,
        [
            ('Декларативність:', 'Описує, ЩО саме потрібно отримати з колекції, а не ЯК покроково перебирати масиви індексами.'),
            ('Читабельність для розробників:', 'Знайомий синтаксис для тих, хто має досвід роботи з реляційними базами даних SQL.'),
            ('Статична типізація:', 'Тип змінної s визначається компілятором автоматично з типу елементів колекції studentList.'),
            ('Лямбда-децукрування:', 'Компілятор Roslyn автоматично перетворює цей вираз на Enumerable.Where(studentList, s => s.Age > 12 && s.Age < 20).')
        ],
        '⚡ Порядок пропозицій:',
        'from стоїть ПЕРШИМ (на відміну від SQL SELECT), щоб IDE одразу знала тип елементів для автодоповнення полів (IntelliSense).'
    )
    add_code_panel(s6, 6.81, 1.55, 5.72, 4.25, [
        '// Колекція студентів:',
        'IList<Student> studentList = new List<Student>() {',
        '    new Student() { StudentID = 1, StudentName = "John", Age = 13 },',
        '    new Student() { StudentID = 2, StudentName = "Moin", Age = 21 },',
        '    new Student() { StudentID = 3, StudentName = "Bill", Age = 18 },',
        '    new Student() { StudentID = 4, StudentName = "Ram",  Age = 20 },',
        '    new Student() { StudentID = 5, StudentName = "Ron",  Age = 15 }',
        '};',
        '',
        '// Декларативний запит Query Syntax:',
        'var teenAgerStudent = from s in studentList',
        '                      where s.Age > 12 && s.Age < 20',
        '                      select s;',
        '',
        'foreach (var std in teenAgerStudent)',
        '    Console.WriteLine($"Студент: {std.StudentName}, Вік: {std.Age}");',
        '',
        '// -> Output: John (13), Bill (18), Ron (15)'
    ], 'Program.cs')
    add_bottom_banner(s6, "💡 Порада практики:",
                      "Query Syntax особливо зручний для запитів із кількома джерелами (join), групуванням (group by) та проміжними змінними (let).")
    # ==========================================
    # SLIDE 7: КЛАС QUERYABLE
    # ==========================================
    s7 = prs.slides.add_slide(blank_layout)
    add_bg(s7)
    add_header(s7, "01 · Вступ та Архітектура", "Приклад у стилі Method Syntax (Fluent API)", 7)
    add_theory_card(s7, 0.8, 1.55, 5.72, 4.25,
        'Функціональний підхід через ланцюжки методів', ACCENT_SKY,
        [
            ('Методи розширення (Extension Methods):', 'Методи Where, Select, OrderBy викликаються безпосередньо на екземплярі колекції через крапку.'),
            ('Лямбда-вирази:', 'Умови фільтрації та проекції передаються у формі анонімних функцій s => s.Age > 12.'),
            ('Композиція викликів (Fluent API):', "Методи легко з'єднуються у ланцюжки: .Where(...).OrderBy(...).Select(...)."),
            ('Повна потужність LINQ:', 'Усі 100% операторів LINQ доступні саме через Method Syntax (на відміну від Query Syntax).')
        ],
        '⚡ Еквівалентність байт-коду:',
        'Query Syntax компілюється у виклики Method Syntax. Швидкодія та результат виконання обох підходів АБСОЛЮТНО ІДЕНТИЧНІ.'
    )
    add_code_panel(s7, 6.81, 1.55, 5.72, 4.25, [
        '// Колекція студентів:',
        'IList<Student> studentList = new List<Student>() {',
        '    new Student() { StudentID = 1, StudentName = "John", Age = 13 },',
        '    new Student() { StudentID = 2, StudentName = "Moin", Age = 21 },',
        '    new Student() { StudentID = 3, StudentName = "Bill", Age = 18 },',
        '    new Student() { StudentID = 4, StudentName = "Ram",  Age = 20 },',
        '    new Student() { StudentID = 5, StudentName = "Ron",  Age = 15 }',
        '};',
        '',
        '// Method Syntax (Fluent API ланцюжок):',
        'var teenStudents = studentList.Where(s => s.Age > 12 && s.Age < 20);',
        '',
        'foreach (var std in teenStudents)',
        '    Console.WriteLine($"Студент: {std.StudentName}, Вік: {std.Age}");',
        '',
        '// -> Output: John (13), Bill (18), Ron (15)'
    ], 'Program.cs')
    add_bottom_banner(s7, "💡 Промисловий стандарт:",
                      "У сучасному коді C# розробники частіше використовують саме Method Syntax завдяки його компактності та лаконічності.")
    # ==========================================
    # SLIDE 8: LINQ QUERY SYNTAX ВСТУП
    # ==========================================
    s8 = prs.slides.add_slide(blank_layout)
    add_bg(s8)
    add_header(s8, "02 · Синтаксис Запитів та Методів", "Два стилі запитів: Query Syntax проти Method Syntax", 8)
    add_theory_card(s8, 0.8, 1.55, 5.72, 4.25,
        'Два синтаксичних підходи в мові C#', ACCENT_INDIGO,
        [
            ('1. Query Syntax (Синтаксис запитів):', 'Декларативний підхід, синтаксично схожий на SQL (from ... in ... where ... select).'),
            ('2. Method Syntax (Методний синтаксис):', 'Функціональний ланцюжок методів розширення з лямбда-виразами (.Where(...).Select(...)).'),
            ('Компіляторна еквівалентність:', 'Компілятор C# Roslyn завжди транслює Query Syntax у виклики Method Syntax.'),
            ('Універсальність бібліотеки BCL:', 'Будь-який запит можна записати у Method Syntax, але лише частину операторів підтримує Query Syntax.')
        ],
        '⚡ Що обрати:',
        'Query Syntax зручніший для Join, GroupJoin та let. Method Syntax є потужнішим і підтримує всі 100% операторів LINQ.'
    )
    add_code_panel(s8, 6.81, 1.55, 5.72, 4.25, [
        '// 1. Декларативний Query Syntax (SQL-подібний стиль):',
        'var q1 = from s in studentList',
        '         where s.Age > 12 && s.Age < 20',
        '         orderby s.StudentName',
        '         select s.StudentName;',
        '',
        '// 2. Функціональний Method Syntax (Fluent API ланцюжок):',
        'var q2 = studentList',
        '    .Where(s => s.Age > 12 && s.Age < 20)',
        '    .OrderBy(s => s.StudentName)',
        '    .Select(s => s.StudentName);',
        '',
        '// 3. Ітерація та перевірка результату:',
        'foreach (var name in q1)',
        '    Console.WriteLine($"Підліток: {name}");',
        '',
        '// -> Output:',
        '// -> Підліток: Bill',
        '// -> Підліток: John',
        '// -> Підліток: Ron'
    ], 'Program.cs')
    add_bottom_banner(s8, "💡 Компіляторна магія:",
                      "Roslyn виконує Desugaring (децукрування), перетворюючи ключові слова from, where, select на виклики Where і Select.")
    # ==========================================
    # SLIDE 9: АНАТОМІЯ QUERY SYNTAX
    # ==========================================
    s9 = prs.slides.add_slide(blank_layout)
    add_bg(s9)
    add_header(s9, "02 · Синтаксис Запитів та Методів", "Анатомія Query Syntax: Ключові слова та змінні діапазону", 9)
    add_theory_card(s9, 0.8, 1.55, 5.72, 4.25,
        'Складові частини виразу запиту', ACCENT_SKY,
        [
            ('from s in studentList:', 'Оголошує змінну діапазону (range variable s) та вказує вхідне джерело даних.'),
            ('where s.Age > 12 && s.Age < 20:', 'Логічний предикат фільтрації; пропускає лише елементи, для яких умова повертає true.'),
            ('select s:', 'Фінальна проекція; визначає форму та тип результату, що повертається з конвеєра.'),
            ('Типізація результату:', 'Запит повертає типізовану послідовність IEnumerable<Student> без необхідності ручного приведення.')
        ],
        '⚡ Range Variable (Змінна діапазону):',
        'Змінна s існує лише всередині запиту і позначає поточний ітерований елемент послідовності.'
    )
    add_code_panel(s9, 6.81, 1.55, 5.72, 4.25, [
        '// Модель студентів:',
        'IList<Student> studentList = new List<Student>() {',
        '    new Student() { StudentID = 1, StudentName = "John", Age = 13 },',
        '    new Student() { StudentID = 2, StudentName = "Moin", Age = 21 },',
        '    new Student() { StudentID = 3, StudentName = "Bill", Age = 18 },',
        '    new Student() { StudentID = 4, StudentName = "Ram",  Age = 20 },',
        '    new Student() { StudentID = 5, StudentName = "Ron",  Age = 15 }',
        '};',
        '',
        '// Пошук підлітків (teenagers):',
        'var teenAgerStudent = from s in studentList',
        '                      where s.Age > 12 && s.Age < 20',
        '                      select s;',
        '',
        'foreach (var s in teenAgerStudent)',
        '    Console.WriteLine($"{s.StudentName} ({s.Age} р.)");',
        '',
        '// -> Output: John (13 р.), Bill (18 р.), Ron (15 р.)'
    ], 'Program.cs')
    add_bottom_banner(s9, "💡 Синтаксичний нюанс:",
                      "На відміну від SQL, де SELECT стоїть першим, у LINQ запит завжди починається з from, щоб IDE надавала автодоповнення (IntelliSense).")
    # ==========================================
    # SLIDE 10: ПРАВИЛА QUERY SYNTAX
    # ==========================================
    s10 = prs.slides.add_slide(blank_layout)
    add_bg(s10)
    add_header(s10, "02 · Синтаксис Запитів та Методів", "Ключові правила написання Query Syntax (Points to Remember)", 10)
    add_theory_card(s10, 0.8, 1.55, 5.72, 4.25,
        'Чотири золоті правила синтаксису запитів', ACCENT_INDIGO,
        [
            ('1. Початок із from:', "Будь-який запит у Query Syntax ОБОВ'ЯЗКОВО розпочинається з пропозиції from."),
            ('2. Завершення select або group:', "Запит ОБОВ'ЯЗКОВО повинен закінчуватися або пропозицією select, або пропозицією group by."),
            ('3. Композиція операторів:', 'Між from та select можна довільно комбінувати where, orderby, join, let та додаткові from.'),
            ('4. Неявна типізація var:', 'Для збереження результату запиту рекомендовано використовувати ключове слово var.')
        ],
        '⚡ Завершальні конструкції:',
        'select трансформує дані в IEnumerable<TResult>. group by розбиває дані на підколекції IEnumerable<IGrouping<TKey, TElement>>.'
    )
    add_code_panel(s10, 6.81, 1.55, 5.72, 4.25, [
        '// Структурна схема Query Syntax:',
        '// var result = from <range_var> in <data_source>',
        '//              [where <condition>]',
        '//              [orderby <key> [ascending|descending]]',
        '//              [join ... on ... equals ...]',
        '//              [let <var> = <expr>]',
        '//              select <projection> | group <item> by <key>;',
        '',
        '// Практичний приклад повної форми запиту:',
        'var query = from s in studentList',
        '            where s.Age >= 18',
        '            orderby s.StudentName ascending',
        '            select new { Name = s.StudentName, Age = s.Age };',
        '',
        'foreach (var item in query)',
        '    Console.WriteLine($"Студент: {item.Name}, Вік: {item.Age}");',
        '',
        '// -> Output: Bill (18), Moin (21), Ram (20)'
    ], 'Program.cs')
    add_bottom_banner(s10, "💡 Чому var?",
                      "При проекції в анонімні типи ім'я типу генерується компілятором і невідоме заздалегідь, тому var є обов'язковим.")
    # ==========================================
    # SLIDE 11: LINQ METHOD SYNTAX
    # ==========================================
    s11 = prs.slides.add_slide(blank_layout)
    add_bg(s11)
    add_header(s11, "02 · Синтаксис Запитів та Методів", "LINQ Method Syntax: Ланцюжки методів та лямбда-вирази", 11)
    add_theory_card(s11, 0.8, 1.55, 5.72, 4.25,
        'Анатомія методного синтаксису (Fluent API)', ACCENT_INDIGO,
        [
            ('Виклики через крапку:', "Method Syntax базується на викликах методів розширення, з'єднаних у ланцюжок (Fluent Interface)."),
            ('Лямбда-делегати:', 'Кожен метод приймає функціональний делегат, який найчастіше описується лямбда-виразом s => ...'),
            ('Сигнатура Where:', 'Метод Where приймає предикат Func<TSource, bool>, що повертає true/false для кожного елемента.'),
            ('Автоматичне виведення типів:', 'Компілятор самостійно визначає generic-тип TSource, тому не потрібно писати .Where<Student>(...).')
        ],
        '⚡ Func<Student, bool> у Where:',
        'Лямбда s => s.Age > 12 автоматично приводиться компілятором до екземпляра делегата Func<Student, bool>.'
    )
    add_code_panel(s11, 6.81, 1.55, 5.72, 4.25, [
        '// 1. Анатомія виклику Method Syntax:',
        'var result = studentList.Where(s => s.Age > 12);',
        '//            ^ джерело  ^ метод  ^ лямбда-предикат',
        '',
        '// 2. Сигнатура методу розширення з простору Enumerable:',
        '// public static IEnumerable<TSource> Where<TSource>(',
        '//     this IEnumerable<TSource> source,',
        '//     Func<TSource, bool> predicate);',
        '',
        '// 3. Комбінування кількох операторів у ланцюжок:',
        'var adultNames = studentList',
        '    .Where(s => s.Age >= 18)',
        '    .Select(s => s.StudentName);',
        '',
        'foreach (var name in adultNames) Console.Write($"{name} ");',
        '',
        '// -> Output: Moin Bill Ram'
    ], 'Program.cs')
    add_bottom_banner(s11, "💡 Fluent Syntax:",
                      "Method Syntax дозволяє комбінувати десятки методів: .Where(...).OrderBy(...).Select(...).Take(5).")
    # ==========================================
    # SLIDE 12: ПРАКТИКА METHOD SYNTAX
    # ==========================================
    s12 = prs.slides.add_slide(blank_layout)
    add_bg(s12)
    add_header(s12, "02 · Синтаксис Запитів та Методів", "Практика Method Syntax: Пошук студентів-підлітків", 12)
    add_theory_card(s12, 0.8, 1.55, 5.72, 4.25,
        'Фільтрація списку студентів через Where', ACCENT_SKY,
        [
            ('Колекція студентів:', "Список об'єктів Student із полями StudentID, StudentName та Age у пам'яті."),
            ('Виклик Where:', 'Метод фільтрує список за складеною умовою s.Age > 12 && s.Age < 20.'),
            ('Результат:', 'Повертається лінивий перелічувач IEnumerable<Student>, що містить відповідні елементи.'),
            ('Композиція умов:', 'Логічні оператори && (AND) та || (OR) підтримують повну логіку булевих виразів C#.')
        ],
        '⚡ Points to Remember (Method Syntax):',
        '1. Базується на методах розширення. 2. Має назву Fluent API. 3. Підтримує 100% операторів (включно з тими, яких немає в Query Syntax).'
    )
    add_code_panel(s12, 6.81, 1.55, 5.72, 4.25, [
        '// Джерело даних:',
        'IList<Student> studentList = new List<Student>() {',
        '    new Student() { StudentID = 1, StudentName = "John", Age = 13 },',
        '    new Student() { StudentID = 2, StudentName = "Moin", Age = 21 },',
        '    new Student() { StudentID = 3, StudentName = "Bill", Age = 18 },',
        '    new Student() { StudentID = 4, StudentName = "Ram",  Age = 20 },',
        '    new Student() { StudentID = 5, StudentName = "Ron",  Age = 15 }',
        '};',
        '',
        '// Фільтрація підлітків (13-19 років) через Where:',
        'var teenStudents = studentList.Where(s => s.Age > 12 && s.Age < 20);',
        '',
        'foreach (Student std in teenStudents)',
        '    Console.WriteLine($"Знайдено: {std.StudentName}, {std.Age} р.");',
        '',
        '// -> Output:',
        '// -> Знайдено: John, 13 р.',
        '// -> Знайдено: Bill, 18 р.',
        '// -> Знайдено: Ron, 15 р.'
    ], 'Program.cs')
    add_bottom_banner(s12, "💡 Перевага перед циклами:",
                      "Замість 8 рядків коду з foreach, if та add() у новий список, запит записується в один зрозумілий декларативний рядок.")
    # ==========================================
    # SLIDE 13: ЕВОЛЮЦІЯ ЛЯМБДА-ВИРАЗІВ
    # ==========================================
    s13 = prs.slides.add_slide(blank_layout)
    add_bg(s13)
    add_header(s13, "03 · Лямбда-вирази та Делегати", "Лямбда-вирази: Синтаксис, параметри та стрілочний оператор =>", 13)
    add_theory_card(s13, 0.8, 1.55, 5.72, 4.25,
        'Анатомія лямбда-виразу в C#', ACCENT_INDIGO,
        [
            ('Вхідні параметри:', 'Зліва від оператора => записуються аргументи: s або (s, index). Типи виводяться автоматично.'),
            ('Лямбда-оператор =>:', "Читається як 'goes to' ('переходить у'). Відокремлює список аргументів від тіла виразу."),
            ('Тіло лямбди:', 'Справа від => розташовується вираз або блок коду: s.Age > 12 && s.Age < 20.'),
            ('Контекст замикання (Closure):', 'Лямбда-вираз може захоплювати локальні змінні із зовнішнього лексичного контексту методу.')
        ],
        '⚡ Еволюція синтаксису C#:',
        'C# 1.0: Іменовані методи -> C# 2.0: Анонімні методи delegate(s) { return ...; } -> C# 3.0+: Лямбда-вирази s => ...'
    )
    add_code_panel(s13, 6.81, 1.55, 5.72, 4.25, [
        '// Анатомія лямбда-виразу у фільтрації:',
        '// (s)      =>       (s.Age > 12 && s.Age < 20)',
        '//  ^       ^         ^',
        "// парам.  'goes to'  тіло / повернене значення",
        '',
        '// Приклад застосування з локальною змінною (Замикання):',
        'int minAge = 18;',
        'var adults = studentList.Where(s => s.Age >= minAge);',
        '',
        '// Перебір відфільтрованих записів:',
        'foreach (var s in adults)',
        '    Console.WriteLine($"Повнолітній: {s.StudentName} ({s.Age})");',
        '',
        '// -> Output:',
        '// -> Повнолітній: Moin (21)',
        '// -> Повнолітній: Bill (18)',
        '// -> Повнолітній: Ram (20)'
    ], 'Program.cs')
    add_bottom_banner(s13, "💡 Компіляторна магія:",
                      "Компілятор самостійно виводить тип аргументу s як Student, оскільки він знає сигнатуру вхідної колекції.")
    # ==========================================
    # SLIDE 14: ВАРІАЦІЇ ЛЯМБДА-ВИРАЗІВ
    # ==========================================
    s14 = prs.slides.add_slide(blank_layout)
    add_bg(s14)
    add_header(s14, "03 · Лямбда-вирази та Делегати", "Варіації синтаксису лямбда-виразів у C#", 14)
    add_dual_comparison_card(s14, 0.8, 1.55, 5.72, 4.25,
        "Параметри лямбда-виразів", ACCENT_INDIGO,
        [
            ("Кілька параметрів:", "Обов'язково беруться у круглі дужки: (s, youngAge) => ..."),
            ("Явна типізація параметрів:", "Якщо компілятор не може вивести тип: (Student s, int age) => ..."),
            ("Без параметрів:", "Порожні круглі дужки: () => Console.WriteLine(...)")
        ],
        [
            "// Кілька параметрів:",
            "(s, youngAge) => s.Age >= youngAge;",
            "",
            "// З явними типами:",
            "(Student s, int youngAge) => s.Age >= youngAge;",
            "",
            "// Без параметрів:",
            "() => Console.WriteLine(\"Hello from Lambda!\");"
        ]
    )
    add_dual_comparison_card(s14, 6.81, 1.55, 5.72, 4.25,
        "Тіло лямбда-виразу", ACCENT_EMERALD,
        [
            ("Expression Lambda:", "Один вираз, значення якого автоматично повертається (без return)."),
            ("Statement Lambda:", "Кілька інструкцій у фігурних дужках { ... }, вимагає явного return."),
            ("Побічні ефекти:", "У Statement Lambda можна оголошувати локальні змінні та виклики логування.")
        ],
        [
            "// Statement Lambda з кількома рядками:",
            "(s, youngAge) => {",
            "    Console.WriteLine($\"Перевірка {s.StudentName}\");",
            "    return s.Age >= youngAge;",
            "};",
            "",
            "// Expression Lambda (один рядок):",
            "s => s.Age >= 18;"
        ]
    )
    add_bottom_banner(s14, "💡 Рекомендація з чистоти коду:",
                      "У LINQ-запитах надавайте перевагу однорядковим Expression Lambdas — вони легко читаються і не мають побічних ефектів.")

    # ==========================================
    # SLIDE 15: ПРИЗНАЧЕННЯ У ДЕЛЕГАТ FUNC
    # ==========================================
    s15 = prs.slides.add_slide(blank_layout)
    add_bg(s15)
    add_header(s15, "03 · Лямбда-вирази та Делегати", "Призначення лямбда-виразів у делегат Func<T, bool>", 15)
    add_theory_card(s15, 0.8, 1.55, 5.72, 4.25,
        'Делегат Func як змінна першого класу', ACCENT_INDIGO,
        [
            ('Узагальнений делегат Func:', 'Останній тип у кутових дужках Func<T1, T2, ..., TResult> завжди вказує тип повернення.'),
            ('Func<Student, bool>:', "Приймає об'єкт Student і повертає логічне значення bool (предикат перевірки)."),
            ('Повторне використання:', 'Змінну-делегат можна зберегти, передавати як аргумент і викликати як звичайну функцію.'),
            ('Інкапсуляція бізнес-правил:', 'Дозволяє централізовано визначати складні бізнес-правила в одному місці системи.')
        ],
        '⚡ Сигнатура делегата:',
        'Func<Student, bool> isStudentTeenAger = s => s.Age > 12 && s.Age < 20;'
    )
    add_code_panel(s15, 6.81, 1.55, 5.72, 4.25, [
        '// 1. Оголошення делегата через лямбда-вираз:',
        'Func<Student, bool> isStudentTeenAger =',
        '    s => s.Age > 12 && s.Age < 20;',
        '',
        "// 2. Створення тестових об'єктів студентів:",
        'Student std1 = new Student() { StudentID = 1, StudentName = "Moin", Age = 21 };',
        'Student std2 = new Student() { StudentID = 2, StudentName = "Bill", Age = 18 };',
        '',
        '// 3. Прямий виклик збереженого делегата:',
        'bool isMoinTeen = isStudentTeenAger(std1); // False',
        'bool isBillTeen = isStudentTeenAger(std2); // True',
        '',
        'Console.WriteLine($"Moin is teen: {isMoinTeen}");',
        'Console.WriteLine($"Bill is teen: {isBillTeen}");',
        '',
        '// -> Output:',
        '// -> Moin is teen: False',
        '// -> Bill is teen: True'
    ], 'Program.cs')
    add_bottom_banner(s15, "💡 Головна перевага:",
                      "Збереження складних бізнес-предикатів у делегати дозволяє повторно використовувати їх у різних LINQ-запитах без дублювання коду.")
    # ==========================================
    # SLIDE 16: ГЛИБОКИЙ РОЗБІР FUNC ДЕЛЕГАТА
    # ==========================================
    s16 = prs.slides.add_slide(blank_layout)
    add_bg(s16)
    add_header(s16, "03 · Лямбда-вирази та Делегати", "Еквівалентність Func делегата та іменованої функції", 16)
    add_theory_card(s16, 0.8, 1.55, 5.72, 4.25,
        'Трансляція лямбди компілятором під капотом', ACCENT_INDIGO,
        [
            ('Вхідний тип:', 'Перший тип у списку узагальнень Func<Student, ...> задає тип вхідного параметра методу.'),
            ('Вихідний тип:', 'Останній тип Func<..., bool> задає тип, що повертається інструкцією return.'),
            ('Генерований метод:', 'Компілятор перетворює лямбду на звичайний приватний метод класу з атрибутом [CompilerGenerated].'),
            ('Швидкодія:', 'Виклик через делегат не створює накладних витрат рефлексії і оптимізується JIT-компілятором.')
        ],
        '⚡ Повна еквівалентність:',
        'Делегат Func<Student, bool> під капотом є строго типізованим вказівником на функцію bool isStudentTeenAger(Student s).'
    )
    add_code_panel(s16, 6.81, 1.55, 5.72, 4.25, [
        '// Форма 1. Лямбда-вираз через делегат Func:',
        'Func<Student, bool> isTeenLambda =',
        '    s => s.Age > 12 && s.Age < 20;',
        '',
        '// Форма 2. Класичний іменований метод:',
        'public bool IsTeenMethod(Student s)',
        '{',
        '    return s.Age > 12 && s.Age < 20;',
        '}',
        '',
        '// Форма 3. Передача іменованого методу в Where:',
        'var teenList = studentList.Where(IsTeenMethod);',
        '',
        'foreach (var s in teenList)',
        '    Console.WriteLine($"Знайдено: {s.StudentName}");',
        '',
        '// -> Output: John, Bill, Ron'
    ], 'Program.cs')
    add_bottom_banner(s16, "💡 Архітектурна перевага:",
                      "Лямбда-вирази дозволяють описувати локальну функціональну логіку прямо за місцем її використання без зайвого шаблонного коду.")
    # ==========================================
    # SLIDE 17: ДЕЛЕГАТ ACTION ПРОТИ FUNC
    # ==========================================
    s17 = prs.slides.add_slide(blank_layout)
    add_bg(s17)
    add_header(s17, "03 · Лямбда-вирази та Делегати", "Делегат Action<T> проти Func<T> та передача у Where", 17)
    add_dual_comparison_card(s17, 0.8, 1.55, 5.72, 4.25,
        "Делегат Action<T> (Повертає void)", ACCENT_AMBER,
        [
            ("Семантика:", "Виконує дію, але НІЧОГО не повертає (аналог процедури)."),
            ("Призначення:", "Виведення на консоль, запис у лог, виклик події."),
            ("У LINQ:", "Не використовується у Where/Select, бо не повертає результат.")
        ],
        [
            "// Action<Student>: дія без результату",
            "Action<Student> PrintStudentDetail = s =>",
            "    Console.WriteLine($\"Name: {s.StudentName}, Age: {s.Age}\");",
            "",
            "var std = new Student() { StudentName = \"Bill\", Age = 21 };",
            "PrintStudentDetail(std);",
            "// Output: Name: Bill, Age: 21"
        ]
    )
    add_dual_comparison_card(s17, 6.81, 1.55, 5.72, 4.25,
        "Делегат Func<T, bool> (Повертає результат)", ACCENT_INDIGO,
        [
            ("Семантика:", "Обчислює значення і ОБОВ'ЯЗКОВО повертає його (чиста функція)."),
            ("Призначення:", "Предикати фільтрації, проекції, компаратори."),
            ("У LINQ:", "Є основою методів Where, Select, OrderBy, Count тощо.")
        ],
        [
            "// Func<Student, bool>: предикат фільтра",
            "Func<Student, bool> isTeen = s => s.Age < 20;",
            "",
            "// Передача у метод розширення Where:",
            "var result = studentList.Where(isTeen);",
            "// Where приймає САМЕ Func<Student, bool>!"
        ]
    )
    add_bottom_banner(s17, "💡 Золоте правило:",
                      "Усі фільтри та проекції LINQ працюють з Func<...>. Делегати Action<...> застосовуються для побічних дій (наприклад, .ForEach()).")

    # ==========================================
    # SLIDE 18: ПІДСУМОК ЛЯМБДА-ВИРАЗІВ
    # ==========================================
    s18 = prs.slides.add_slide(blank_layout)
    add_bg(s18)
    add_header(s18, "03 · Лямбда-вирази та Делегати", "Практика передачі делегата Func<Student, bool> у Where()", 18)
    add_theory_card(s18, 0.8, 1.55, 5.72, 4.25,
        'Чистий код без громіздких лямбда-рядків', ACCENT_SKY,
        [
            ('Відокремлення логіки:', 'Складні бізнес-правила виносяться в окремі змінні делегатів або статичні методи.'),
            ('Читабельність запиту:', 'Виклик studentList.Where(isStudentTeenAger) читається як звичайна людська фраза.'),
            ('Легке тестування:', 'Делегат можна протестувати окремо модульними тестами Unit Tests без виконання запиту.'),
            ('Динамічна підміна:', 'Делегат можна передавати як параметр методу, динамічно змінюючи критерій пошуку.')
        ],
        '⚡ Еволюція коду:',
        '1. Where(s => ...) -> 2. Func<Student, bool> isTeen = ... -> 3. Доменний предикат Student.IsTeenager'
    )
    add_code_panel(s18, 6.81, 1.55, 5.72, 4.25, [
        '// 1. Оголошення предикатів фільтрації:',
        'Func<Student, bool> isStudentTeenAger =',
        '    s => s.Age > 12 && s.Age < 20;',
        '',
        'Func<Student, bool> isHonorStudent =',
        '    s => s.StudentID <= 3;',
        '',
        '// 2. Використання збереженого делегата у Where:',
        'var filteredStudents = studentList.Where(isStudentTeenAger);',
        '',
        '// 3. Ітерація по відфільтрованій вибірці:',
        'foreach (Student std in filteredStudents)',
        '    Console.WriteLine($"Студент: {std.StudentName}, Вік: {std.Age}");',
        '',
        '// -> Output:',
        '// -> Студент: John, Вік: 13',
        '// -> Студент: Bill, Вік: 18',
        '// -> Студент: Ron, Вік: 15'
    ], 'Program.cs')
    add_bottom_banner(s18, "💡 Рекомендація з архітектури:",
                      "У корпоративних системах тримайте бізнес-предикати як статичні властивості моделей (наприклад, Student.IsTeenagerPredicate).")
    # ==========================================
    # SLIDE 19: СТАНДАРТНІ ОПЕРАТОРИ ЗАПИТІВ
    # ==========================================
    s19 = prs.slides.add_slide(blank_layout)
    add_bg(s19)
    add_header(s19, "04 · Стандартні Оператори LINQ", "Стандартні оператори запитів: Query проти Method Syntax", 19)
    add_dual_comparison_card(s19, 0.8, 1.55, 5.72, 4.25,
        "Standard Query Operators у Query Syntax", ACCENT_INDIGO,
        [
            ("Ключові слова C#:", "from, where, select, group, into, orderby, join, let."),
            ("Трансляція:", "Roslyn перетворює на виклики Where, Select, OrderByBy, GroupBy."),
            ("Читабельність:", "Нагадує класичні SQL-запити до реляційних баз даних.")
        ],
        [
            "// Query Syntax:",
            "var students = from s in studentList",
            "               where s.Age > 20",
            "               select s;",
            "",
            "// Транслюється компілятором у Method Syntax!"
        ]
    )
    add_dual_comparison_card(s19, 6.81, 1.55, 5.72, 4.25,
        "Standard Query Operators у Method Syntax", ACCENT_SKY,
        [
            ("Методи розширення:", "Where, Select, OrderBy, GroupBy, Join, GroupJoin, Aggregate тощо."),
            ("Універсальність:", "Підтримує всі оператори без винятку (понад 50 методів)."),
            ("Функціональність:", "Легко поєднується з іншими бібліотеками та розширеннями.")
        ],
        [
            "// Method Syntax:",
            "var students = studentList",
            "    .Where(s => s.Age > 20);",
            "",
            "// Прямий виклик методу розширення Enumerable.Where"
        ]
    )
    add_bottom_banner(s19, "💡 Еквівалентність:",
                      "Обидва синтаксиси є повністю взаємозамінними за швидкодією та результатом виконання.")

    # ==========================================
    # SLIDE 20: ВЕЛИКА КЛАСИФІКАЦІЯ ОПЕРАТОРІВ
    # ==========================================
    s20 = prs.slides.add_slide(blank_layout)
    add_bg(s20)
    add_header(s20, "04 · Стандартні Оператори LINQ", "Велика класифікація операторів LINQ (15 категорій)", 20)
    
    headers_20 = ["Категорія операторів", "Методи LINQ", "Опис та призначення"]
    rows_20 = [
        ["Filtering (Фільтрація)", "Where, OfType", "Відбір елементів за умовою або типом"],
        ["Sorting (Сортування)", "OrderBy, OrderByDescending, ThenBy, ThenByDescending, Reverse", "Впорядкування за зростанням або спаданням"],
        ["Grouping (Групування)", "GroupBy, ToLookup", "Розбиття на групи за спільним ключем"],
        ["Joining (З'єднання)", "Join, GroupJoin", "Реляційне зіставлення двох послідовностей"],
        ["Projection (Проекція)", "Select, SelectMany", "Трансформація форми даних та сплющення 1:N"],
        ["Aggregation (Агрегація)", "Aggregate, Average, Count, LongCount, Max, Min, Sum", "Обчислення скалярних підсумків та згортка"],
        ["Quantifiers (Квантори)", "All, Any, Contains", "Логічна перевірка існування чи загальності"],
        ["Elements (Елементні)", "ElementAt, First, FirstOrDefault, Last, LastOrDefault, Single, SingleOrDefault", "Вилучення конкретного одиничного елемента"],
        ["Set (Множини)", "Distinct, Except, Intersect, Union", "Математичні операції над множинами без дублікатів"],
        ["Partitioning (Секціонування)", "Skip, SkipWhile, Take, TakeWhile, Chunk", "Посторінкова пагінація та зрізи потоку"],
        ["Concatenation & Misc", "Concat, SequenceEqual, Zip", "Послідовне об'єднання, перевірка рівності, зшивання"],
        ["Conversion & Execution", "AsEnumerable, AsQueryable, Cast, ToArray, ToList, ToDictionary", "Матеріалізація в пам'ять та керування виконанням"]
    ]
    add_table_card(s20, 0.8, 1.55, 11.733, 4.25, headers_20, rows_20, [3.0, 4.733, 4.0], ACCENT_INDIGO)
    add_bottom_banner(s20, "💡 Карта курсу:",
                      "Кожна з цих категорій детально розглядається на наступних слайдах із практичними прикладами C#.")

    # ==========================================
    # SLIDE 21: FILTERING OPERATORS ТАБЛИЦЯ
    # ==========================================
    s21 = prs.slides.add_slide(blank_layout)
    add_bg(s21)
    add_header(s21, "05 · Фільтрація (Filtering)", "Оператори фільтрації: Where та OfType", 21)
    headers_21 = ["Метод LINQ", "Опис", "Що робить", "Повертає", "Приклад"]
    rows_21 = [
        ["Where", "Фільтрує колекцію за заданою умовою (предикатом).", "Пропускає елементи, для яких предикат повертає true.", "IEnumerable<T>", "numbers.Where(x => x > 5)"],
        ["OfType", "Фільтрує елементи колекції за їхньою сумісністю з типом T.", "Відбирає лише ті елементи, які можна безпечно привести до типу T.", "IEnumerable<TResult>", "list.OfType<string>()"]
    ]
    add_table_card(s21, 0.8, 1.55, 11.733, 2.3, headers_21, rows_21, [1.8, 3.2, 3.2, 1.7, 1.833], ACCENT_INDIGO)
    add_code_panel(s21, 0.8, 4.0, 11.733, 1.80, [
        "// 1. Where: перевірка логічної умови",
        "var adults = studentList.Where(s => s.Age >= 18);",
        "",
        "// 2. OfType: безпечна фільтрація за типом даних (ігнорує інші типи без помилок)",
        "object[] mixed = { 1, \"Hello\", 3.14, \"World\", 42 };",
        "var stringsOnly = mixed.OfType<string>(); // [\"Hello\", \"World\"]"
    ])
    add_bottom_banner(s21, "💡 Фізика роботи:",
                      "Where та OfType є потоковими операторами (Streaming, O(1) пам'яті). Вони не виділяють проміжних списків.")

    # ==========================================
    # SLIDE 22: WHERE У QUERY SYNTAX
    # ==========================================
    s22 = prs.slides.add_slide(blank_layout)
    add_bg(s22)
    add_header(s22, "05 · Фільтрація (Filtering)", "Оператор Where: Робота та сигнатура в .NET", 22)
    add_theory_card(s22, 0.8, 1.55, 5.72, 4.25,
        'Ключовий оператор фільтрації Enumerable.Where', ACCENT_SKY,
        [
            ('Семантика Where:', 'Пропускає через конвеєр лише ті елементи, для яких вказаний предикат повертає значення true.'),
            ('Предикат Func<TSource, bool>:', 'Чиста функція, що приймає поточний елемент колекції і повертає булевий результат.'),
            ('Потокова обробка (Streaming):', 'Працює за принципом конвеєра: елементи не зберігаються в масиві, а віддаються один за одним.'),
            ('Імутабельність джерела:', 'Початкова колекція ніколи не змінюється; генерується новий перелічувач результатів.')
        ],
        '⚡ Відкладене виконання (Deferred Execution):',
        'Фільтрація виконується тільки під час проходження циклу foreach або під час матеріалізації (ToList, ToArray).'
    )
    add_code_panel(s22, 6.81, 1.55, 5.72, 4.25, [
        '// 1. Джерело даних:',
        'IList<Student> studentList = new List<Student>() {',
        '    new Student() { StudentID = 1, StudentName = "John", Age = 13 },',
        '    new Student() { StudentID = 2, StudentName = "Moin", Age = 21 },',
        '    new Student() { StudentID = 3, StudentName = "Bill", Age = 18 },',
        '    new Student() { StudentID = 4, StudentName = "Ram",  Age = 20 },',
        '    new Student() { StudentID = 5, StudentName = "Ron",  Age = 15 }',
        '};',
        '',
        '// 2. Фільтрація з кількома логічними умовами:',
        'var filtered = studentList.Where(s => s.Age > 12 && s.Age < 20);',
        '',
        '// 3. Ітерація по конвеєру:',
        'foreach (var std in filtered)',
        '    Console.WriteLine($"Знайдено: {std.StudentName}");',
        '',
        '// -> Output: John, Bill, Ron'
    ], 'Program.cs')
    add_bottom_banner(s22, "💡 Оптимізація продуктивності:",
                      "Where не копіює дані. Якщо з мільйона записів підійдуть 5, у пам'яті будуть створені посилання тільки на ці 5 об'єктів.")
    # ==========================================
    # SLIDE 23: WHERE У METHOD SYNTAX
    # ==========================================
    s23 = prs.slides.add_slide(blank_layout)
    add_bg(s23)
    add_header(s23, "05 · Фільтрація (Filtering)", "Перевантаження Where: Фільтрація з використанням індексу", 23)
    add_theory_card(s23, 0.8, 1.55, 5.72, 4.25,
        'Сигнатура з порядковим номером елемента', ACCENT_INDIGO,
        [
            ('Друге перевантаження Where:', 'Приймає делегат Func<TSource, int, bool>, де другий параметр — 0-базовий індекс елемента.'),
            ('Автоматичний інкремент:', 'Індекс збільшується автоматично для кожного обробленого вхідного елемента під час перебору.'),
            ('Тільки у Method Syntax:', 'Це перевантаження не має прямого синтаксичного аналога в Query Syntax (from ... where).'),
            ('Практичні сценарії:', 'Відбір парних/непарних рядків, зеброве фарбування таблиць, обмеження перших N записів за позицією.')
        ],
        '⚡ Сигнатура методу розширення:',
        'public static IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, int, bool> predicate);'
    )
    add_code_panel(s23, 6.81, 1.55, 5.72, 4.25, [
        '// Вибірка елементів на парних індексах (0, 2, 4, ...):',
        'var evenIndexStudents = studentList',
        '    .Where((s, index) => index % 2 == 0);',
        '',
        'foreach (var std in evenIndexStudents)',
        '    Console.WriteLine($"Індексний студент: {std.StudentName}");',
        '// Output: John (idx 0), Bill (idx 2), Ron (idx 4)',
        '',
        '// Комбінування індексу та бізнес-властивості:',
        'var complexFilter = studentList.Where((s, index) =>',
        '    s.Age > 15 && index < 3);',
        '',
        'foreach (var std in complexFilter)',
        '    Console.WriteLine($"Фінал: {std.StudentName}");',
        '// Output: Bill (Age 18, idx 2)'
    ], 'Program.cs')
    add_bottom_banner(s23, "💡 Важлива деталь:",
                      "Індекс передає позицію у вхідній послідовності, а не значення первинного ключа (StudentID) в базі даних!")
    # ==========================================
    # SLIDE 24: МНОЖИННІ WHERE
    # ==========================================
    s24 = prs.slides.add_slide(blank_layout)
    add_bg(s24)
    add_header(s24, "05 · Фільтрація (Filtering)", "Множинні пропозиції where у запитах C#", 24)
    add_dual_comparison_card(s24, 0.8, 1.55, 5.72, 4.25,
        "Множинні where у Query Syntax", ACCENT_INDIGO,
        [
            ("Послідовний запис:", "Можна вказувати кілька пропозицій where одну за одною."),
            ("Еквівалентність AND:", "Елемент пройде далі лише тоді, коли задовольнить УСІ блоки where."),
            ("Читабельність:", "Допомагає розбивати довгі громіздкі умови на окремі логічні рядки.")
        ],
        [
            "// Множинні where в одному запиті:",
            "var filteredResult = from s in studentList",
            "                     where s.Age > 12",
            "                     where s.Age < 20",
            "                     select s.StudentName;",
            "",
            "// Еквівалентно: where s.Age > 12 && s.Age < 20"
        ]
    )
    add_dual_comparison_card(s24, 6.81, 1.55, 5.72, 4.25,
        "Ланцюжок .Where().Where() у Method Syntax", ACCENT_SKY,
        [
            ("Композиція фільтрів:", "Кожен наступний .Where() фільтрує результати попереднього."),
            ("Оптимізація BCL (Iterator Folding):", "Сучасний .NET BCL об'єднує послідовні Where в один комбінований ітератор."),
            ("Динамічні фільтри:", "Зручно додавати умови за прапорцями: if (filterAge) q = q.Where(...);")
        ],
        [
            "// Ланцюжок методів:",
            "var result = studentList",
            "    .Where(s => s.Age > 12)",
            "    .Where(s => s.Age < 20);",
            "",
            "// .NET оптимізує це в єдиний прохід!"
        ]
    )
    add_bottom_banner(s24, "💡 Практична порада:",
                      "Використовуйте окремі where, коли умови належать до різних бізнес-правил (наприклад, перевірка статусу та діапазону дат).")

    # ==========================================
    # SLIDE 25: ОПЕРАТОР OFTYPE ВСТУП
    # ==========================================
    s25 = prs.slides.add_slide(blank_layout)
    add_bg(s25)
    add_header(s25, "05 · Фільтрація (Filtering)", "Оператор OfType<T>: Безпечна фільтрація за типами", 25)
    add_theory_card(s25, 0.8, 1.55, 5.72, 4.25,
        'Робота з різнотипними колекціями та поліморфізмом', ACCENT_INDIGO,
        [
            ('Суть OfType<T>:', 'Фільтрує колекцію, залишаючи лише ті елементи, які можна безпечно привести до типу T.'),
            ('Безпека без помилок:', 'Елементи, що не відповідають цільовому типу, тихо пропускаються (без InvalidCastException).'),
            ('Інструкція IL isinst:', 'На рівні байт-коду використовує інструкцію isinst, яка працює як оператор is у C#.'),
            ('Підтримка наслідування:', 'Якщо вказати базовий тип OfType<Person>(), поверне як екземпляри Person, так і Student/Teacher.')
        ],
        '⚡ OfType проти Cast:',
        'Cast<T> намагається жорстко привести кожен елемент і кине виняток при невідповідності. OfType<T> просто відкидає зайві типи.'
    )
    add_code_panel(s25, 6.81, 1.55, 5.72, 4.25, [
        '// Нетипізована колекція (наприклад, ArrayList або object[]):',
        'IList mixedList = new ArrayList() {',
        '    0,',
        '    "One",',
        '    "Two",',
        '    3,',
        '    new Student() { StudentID = 1, StudentName = "Bill" }',
        '};',
        '',
        '// 1. Отримати тільки рядки (ігноруючи числа та студентів):',
        'var stringResult = mixedList.OfType<string>();',
        'foreach (var str in stringResult) Console.Write($"{str} ");',
        '// -> Output: One Two',
        '',
        '// 2. Отримати тільки студентів:',
        'var studentResult = mixedList.OfType<Student>();',
        'foreach (var std in studentResult) Console.WriteLine(std.StudentName);',
        '// -> Output: Bill'
    ], 'Program.cs')
    add_bottom_banner(s25, "💡 Сфера застосування:",
                      "OfType незамінний при міграції старого коду .NET 1.1/2.0 з ArrayList або при обробці ієрархій UI контролів (Controls.OfType<Button>()).")
    # ==========================================
    # SLIDE 26: ПРАКТИКА OFTYPE
    # ==========================================
    s26 = prs.slides.add_slide(blank_layout)
    add_bg(s26)
    add_header(s26, "05 · Фільтрація (Filtering)", "Практичний приклад застосування OfType у C#", 26)
    add_dual_comparison_card(s26, 0.8, 1.55, 5.72, 4.25,
        "Вибірка цілих чисел OfType<int>()", ACCENT_SKY,
        [
            ("Джерело:", "Змішаний список mixedList з числами, рядками та об'єктами."),
            ("Вибірка:", "mixedList.OfType<int>() відбирає лише цілочисельні елементи."),
            ("Результат:", "Повертає значення 0 та 3. Усі рядки та студенти проігноровані.")
        ],
        [
            "// Вибірка чисел:",
            "var intResult = mixedList.OfType<int>();",
            "",
            "foreach (int num in intResult)",
            "    Console.Write(num + \" \");",
            "",
            "// Output: 0 3"
        ]
    )
    add_dual_comparison_card(s26, 6.81, 1.55, 5.72, 4.25,
        "Вибірка рядків OfType<string>()", ACCENT_EMERALD,
        [
            ("Джерело:", "Той самий змішаний список mixedList."),
            ("Вибірка:", "mixedList.OfType<string>() відбирає текстові елементи."),
            ("Результат:", "Повертає 'One' та 'Two'. Числа 0, 3 та об'єкти проігноровані.")
        ],
        [
            "// Вибірка рядків:",
            "var strResult = mixedList.OfType<string>();",
            "",
            "foreach (string s in strResult)",
            "    Console.Write(s + \" \");",
            "",
            "// Output: One Two"
        ]
    )
    add_bottom_banner(s26, "💡 Поліморфний бонус:",
                      "Якщо клас GraduateStudent успадковує Student, то list.OfType<Student>() поверне як звичайних студентів, так і випускників!")

    # ==========================================
    # SLIDE 27: SORTING OPERATORS ВСТУП
    # ==========================================
    s27 = prs.slides.add_slide(blank_layout)
    add_bg(s27)
    add_header(s27, "06 · Сортування (Sorting)", "Оператори сортування: Впорядкування даних у LINQ", 27)
    add_theory_card(s27, 0.8, 1.55, 5.72, 4.25,
        'Концепція сортування послідовностей', ACCENT_INDIGO,
        [
            ('Призначення:', 'Впорядковує елементи колекції у порядку зростання або спадання за одним або кількома ключами.'),
            ('Стабільність сортування:', 'Усі оператори сортування у LINQ є стабільними (зберігають порядок однакових елементів).'),
            ('Ключовий інтерфейс:', 'Методи сортування повертають спеціальний інтерфейс IOrderedEnumerable<T>.'),
            ('Модель виконання:', 'Сортування є жадібним оператором з відкладеним стартом (буферизує всі елементи під час першого MoveNext).')
        ],
        '⚡ Особливість виконання:',
        'Для сортування неможливо віддати перший елемент без перегляду всієї колекції! Тому алгоритм QuickSort/TimSort зчитує всі дані в буфер.'
    )
    add_code_panel(s27, 6.81, 1.55, 5.72, 4.25, [
        '// 1. Первинне сортування за віком (зростання):',
        'var s1 = studentList.OrderBy(s => s.Age);',
        '',
        '// 2. Первинне сортування за віком (спадання):',
        'var s2 = studentList.OrderByDescending(s => s.Age);',
        '',
        '// 3. Вторинне сортування (ThenBy / ThenByDescending):',
        'var s3 = studentList',
        '    .OrderBy(s => s.Age)',
        '    .ThenBy(s => s.StudentName);',
        '',
        '// 4. Виведення результату комплексного сортування:',
        'foreach (var s in s3)',
        '    Console.WriteLine($"{s.Age} р. - {s.StudentName}");',
        '',
        '// -> Output:',
        '// -> 13 р. - John',
        '// -> 15 р. - Ron',
        '// -> 18 р. - Bill'
    ], 'Program.cs')
    add_bottom_banner(s27, "💡 Продуктивність:",
                      "Уникайте сортування великих нескінченних послідовностей — операція вимагає читання всієї колекції у пам'ять!")
    # ==========================================
    # SLIDE 28: ТАБЛИЦЯ ОПЕРАТОРІВ СОРТУВАННЯ
    # ==========================================
    s28 = prs.slides.add_slide(blank_layout)
    add_bg(s28)
    add_header(s28, "06 · Сортування (Sorting)", "Зведена таблиця операторів сортування в LINQ", 28)
    headers_28 = ["Оператор", "Query Syntax", "Method Syntax", "Опис дії", "Тип повернення"]
    rows_28 = [
        ["OrderBy", "orderby", "OrderBy()", "Сортування за зростанням (Ascending)", "IOrderedEnumerable<T>"],
        ["OrderByDescending", "orderby ... desc", "OrderByDescending()", "Сортування за спаданням (Descending)", "IOrderedEnumerable<T>"],
        ["ThenBy", "кома ',' у списку", "ThenBy()", "Вторинне сортування за зростанням", "IOrderedEnumerable<T>"],
        ["ThenByDescending", "кома + desc", "ThenByDescending()", "Вторинне сортування за спаданням", "IOrderedEnumerable<T>"],
        ["Reverse", "Не підтримується", "Reverse()", "Розгортає порядок елементів навпаки", "IEnumerable<T>"]
    ]
    add_table_card(s28, 0.8, 1.55, 11.733, 2.35, headers_28, rows_28, [2.0, 2.5, 2.8, 2.7, 1.733], ACCENT_INDIGO)
    add_code_panel(s28, 0.8, 4.05, 11.733, 1.75, [
        "// Приклад комбінованого сортування за віком, а для однакового віку - за ім'ям:",
        'var sorted = studentList.OrderBy(s => s.Age).ThenBy(s => s.StudentName);',
        '// Інверсія порядку відсортованих даних:',
        'var reversed = sorted.Reverse();',
        'foreach (var s in sorted)',
        '    Console.WriteLine($"{s.Age} р. - {s.StudentName}");',
        '// Output: 13 р. - John, 15 р. - Ron, 18 р. - Bill'
    ], 'SortingOperators.cs')
    add_bottom_banner(s28, "💡 Важливе правило:",
                      "ThenBy та ThenByDescending можуть викликатися ТІЛЬКИ після OrderBy/OrderByDescending, бо вони розширюють IOrderedEnumerable.")
    # ==========================================
    # SLIDE 29: ORDERBY У QUERY SYNTAX
    # ==========================================
    s29 = prs.slides.add_slide(blank_layout)
    add_bg(s29)
    add_header(s29, "06 · Сортування (Sorting)", "OrderBy та OrderByDescending у Query та Method Syntax", 29)
    add_theory_card(s29, 0.8, 1.55, 5.72, 4.25,
        'Первинне сортування колекцій', ACCENT_INDIGO,
        [
            ('За замовчуванням (Ascending):', 'Сортування за зростанням: числа від меншого до більшого, текст від А до Я.'),
            ('За спаданням (Descending):', 'OrderByDescending / descending: від більшого до меншого, текст від Я до А.'),
            ('Кастомні компаратори:', 'Обидва методи приймають перевантаження з IComparer<TKey> для власної логіки порівняння.'),
            ('Імутабельність джерела:', 'Початкова колекція залишається незмінною; генерується впорядкований ітератор.')
        ],
        '⚡ Синтаксис у Query:',
        'orderby s.StudentName (за замовчуванням ascending) або orderby s.StudentName descending (явне спадання)'
    )
    add_code_panel(s29, 6.81, 1.55, 5.72, 4.25, [
        '// 1. Query Syntax (за спаданням):',
        'var qAsc = from s in studentList',
        '           orderby s.StudentName descending',
        '           select s;',
        '',
        '// 2. Method Syntax (за зростанням):',
        'var mAsc = studentList.OrderBy(s => s.StudentName);',
        '',
        '// 3. Ітерація по відсортованих даних:',
        'foreach (var std in mAsc)',
        '    Console.WriteLine($"Студент: {std.StudentName}");',
        '',
        '// -> Output:',
        '// -> Студент: Bill',
        '// -> Студент: John',
        '// -> Студент: Moin',
        '// -> Студент: Ram',
        '// -> Студент: Ron'
    ], 'Program.cs')
    add_bottom_banner(s29, "💡 Регістрозалежність:",
                      "За замовчуванням рядки сортуються з урахуванням поточної культури (CultureInfo.CurrentCulture).")
    # ==========================================
    # SLIDE 30: ORDERBY У METHOD SYNTAX
    # ==========================================
    s30 = prs.slides.add_slide(blank_layout)
    add_bg(s30)
    add_header(s30, "06 · Сортування (Sorting)", "Вторинне сортування: Оператори ThenBy та ThenByDescending", 30)
    add_theory_card(s30, 0.8, 1.55, 5.72, 4.25,
        'Багаторівневе сортування колекцій', ACCENT_SKY,
        [
            ('Проблема повторного OrderBy:', 'Якщо викликати OrderBy двічі поспіль, друге сортування ПОВНІСТЮ ЗЛАМАЄ перше!'),
            ('Призначення ThenBy:', 'Виконує сортування ТІЛЬКИ між елементами, у яких ключі попереднього сортування однакові.'),
            ('Ланцюжки ThenBy:', 'Можна викликати необмежену кількість разів: .OrderBy().ThenBy().ThenByDescending().'),
            ('Синтаксис запитів:', 'У Query Syntax вторинні ключі перераховуються через кому: orderby s.Age, s.StudentName.')
        ],
        '⚡ Чому ламається OrderBy:',
        "studentList.OrderBy(s => s.Age).OrderBy(s => s.StudentName) зламає сортування, пересортувавши все за ім'ям!"
    )
    add_code_panel(s30, 6.81, 1.55, 5.72, 4.25, [
        '// 1. Правильне вторинне сортування через ThenBy:',
        'var result = studentList',
        '    .OrderBy(s => s.Age)',
        '    .ThenBy(s => s.StudentName);',
        '',
        '// 2. Аналог у Query Syntax через кому:',
        'var query = from s in studentList',
        '            orderby s.Age, s.StudentName',
        '            select s;',
        '',
        '// 3. Виведення результату багаторівневого сортування:',
        'foreach (var s in result)',
        '    Console.WriteLine($"{s.Age} р. - {s.StudentName}");',
        '',
        '// -> Output:',
        '// -> 13 р. - John',
        '// -> 15 р. - Ron',
        '// -> 18 р. - Bill',
        '// -> 20 р. - Ram',
        '// -> 21 р. - Moin'
    ], 'Program.cs')
    add_bottom_banner(s30, "💡 Типізація C#:",
                      "Метод ThenBy доступний лише для інтерфейсу IOrderedEnumerable<T>, тому компілятор не дозволить викликати його першим.")
    # ==========================================
    # SLIDE 31: МНОЖИННЕ СОРТУВАННЯ У QUERY SYNTAX
    # ==========================================
    s31 = prs.slides.add_slide(blank_layout)
    add_bg(s31)
    add_header(s31, "06 · Сортування (Sorting)", "Оператор Reverse: Розгортання послідовності у зворотному напрямку", 31)
    add_theory_card(s31, 0.8, 1.55, 5.72, 4.25,
        'Інверсія порядку елементів', ACCENT_INDIGO,
        [
            ('Суть оператора:', 'Повертає елементи вхідної колекції у прямо протилежному порядку без аналізу їх значення.'),
            ('Тільки у Method Syntax:', 'У Query Syntax ключового слова reverse немає; використовується orderby descending.'),
            ('LINQ проти List<T>.Reverse():', 'List<T>.Reverse() змінює список на місці (in-place void). LINQ Reverse() повертає новий IEnumerable<T>.'),
            ('Буферизація:', 'Як і сортування, потребує зчитування елементів у внутрішній буфер для розгортання порядку.')
        ],
        '⚡ Дві різні операції Reverse у C#:',
        'list.Reverse() мутує список List<T>, тоді як LINQ list.AsEnumerable().Reverse() повертає новий незмінний ітератор.'
    )
    add_code_panel(s31, 6.81, 1.55, 5.72, 4.25, [
        '// Джерело чисел:',
        'int[] numbers = { 1, 2, 3, 4, 5 };',
        '',
        '// Розгортання послідовності через LINQ:',
        'var reversedNums = numbers.Reverse();',
        '',
        'foreach (var n in reversedNums)',
        '    Console.Write($"{n} ");',
        '// -> Output: 5 4 3 2 1',
        '',
        '// Розгортання після сортування:',
        'var sortedDesc = studentList',
        '    .OrderBy(s => s.Age)',
        '    .Reverse();',
        '',
        'foreach (var s in sortedDesc)',
        '    Console.WriteLine($"{s.Age}: {s.StudentName}");'
    ], 'Program.cs')
    add_bottom_banner(s31, "💡 Чистота коду:",
                      "Використовуйте LINQ Reverse() для збереження незмінності (immutability) колекцій у функціональному стилі.")
    # ==========================================
    # SLIDE 32: THENBY ТА THENBYDESCENDING
    # ==========================================
    s32 = prs.slides.add_slide(blank_layout)
    add_bg(s32)
    add_header(s32, "06 · Сортування (Sorting)", "Вторинне сортування: ThenBy проти помилки повторного OrderBy", 32)
    add_dual_comparison_card(s32, 0.8, 1.55, 5.72, 4.25,
        "❌ Груба помилка: подвійний OrderBy", ACCENT_CRIMSON,
        [
            ("Суть проблеми:", "Повторний виклик .OrderBy() створює НОВИЙ ітератор і ПОВНІСТЮ ЗНИЩУЄ попереднє сортування!"),
            ("Наслідок:", "Колекція буде відсортована ЛИШЕ за останнім полем. Попередній порядок буде втрачено."),
            ("Порушення контракту:", "Зайві обчислення сортування QuickSort двічі.")
        ],
        [
            "// ❌ НЕПРАВИЛЬНО: OrderBy двічі ламає порядок!",
            "var broken = studentList",
            "    .OrderBy(s => s.Age)",
            "    .OrderBy(s => s.StudentName);",
            "// Попереднє сортування за Age СТЕРТО!"
        ]
    )
    add_dual_comparison_card(s32, 6.81, 1.55, 5.72, 4.25,
        "✅ Правильно: OrderBy + ThenBy", ACCENT_EMERALD,
        [
            ("Суть рішення:", "ThenBy створює складений компаратор у межах існуючого IOrderedEnumerable."),
            ("Збереження структури:", "Вторинний ключ застосовується лише тоді, коли первинні ключі рівні."),
            ("Гнучкість напрямків:", "ThenByDescending сортує вторинний ключ за спаданням.")
        ],
        [
            "//  ПРАВИЛЬНО: композитне сортування",
            "var correct = studentList",
            "    .OrderBy(s => s.Age)",
            "    .ThenBy(s => s.StudentName);",
            "// Ідеальне багаторівневе сортування!"
        ]
    )
    add_bottom_banner(s32, "💡 Золоте правило архітектора:",
                      "У ланцюжку викликів OrderBy може бути лише ОДИН (на початку). Усі наступні рівні сортування — тільки ThenBy/ThenByDescending.")

    # ==========================================
    # SLIDE 33: GROUPING OPERATORS ТАБЛИЦЯ
    # ==========================================
    s33 = prs.slides.add_slide(blank_layout)
    add_bg(s33)
    add_header(s33, "07 · Групування (Grouping)", "Порівняння: Оператор GroupBy проти ToLookup", 33)
    headers_33 = ["Характеристика", "GroupBy", "ToLookup", "Перевага", "Сценарій використання"]
    rows_33 = [
        ["Тип виконання", "Відкладене (Deferred Execution)", "Негайне (Immediate Execution)", "GroupBy не вантажить пам'ять заздалегідь", "GroupBy для конвеєрів; ToLookup для кешу"],
        ["Тип повернення", "IEnumerable<IGrouping<TKey, TElement>>", "ILookup<TKey, TElement>", "ToLookup має індексатор lookup[key]", "Швидкий багаторазовий пошук за ключем O(1)"]
    ]
    add_table_card(s33, 0.8, 1.55, 11.733, 1.85, headers_33, rows_33, [1.8, 2.3, 2.3, 2.6, 2.733], ACCENT_INDIGO)
    add_code_panel(s33, 0.8, 3.55, 11.733, 2.25, [
        '// 1. GroupBy: Відкладене виконання (Deferred) - обчислюється при кожному foreach:',
        'IEnumerable<IGrouping<int, Student>> grouped = studentList.GroupBy(s => s.Age);',
        'foreach (var group in grouped)',
        '    Console.WriteLine($"Ключ віку: {group.Key}, Кількість студентів: {group.Count()}");',
        '',
        "// 2. ToLookup: Негайне виконання (Immediate) - створює постійний незмінний хеш-індекс 1:N у пам'яті:",
        'ILookup<int, Student> lookup = studentList.ToLookup(s => s.Age);',
        'var age18Students = lookup[18]; // Швидкий доступ O(1), lookup[99] безпечно поверне Empty без винятків!'
    ], 'GroupByVsToLookup.cs')
    add_bottom_banner(s33, "💡 Архітектурна різниця:",
                      "GroupBy повертає відкладений ітератор, а ToLookup негайно матеріалізує незмінний швидкий індекс у пам'яті.")
    # ==========================================
    # SLIDE 34: GROUPBY У QUERY SYNTAX
    # ==========================================
    s34 = prs.slides.add_slide(blank_layout)
    add_bg(s34)
    add_header(s34, "07 · Групування (Grouping)", "Оператор GroupBy у Query Syntax (group ... by ...)", 34)
    add_theory_card(s34, 0.8, 1.55, 5.72, 4.25,
        'Синтаксис групування group s by s.Age', ACCENT_INDIGO,
        [
            ('Завершальна пропозиція:', 'Запит у Query Syntax може завершуватися або select, або group by.'),
            ('Структура IGrouping<TKey, TElement>:', 'Кожна група має властивість .Key та сама є колекцією IEnumerable<TElement>.'),
            ('Проміжна змінна into:', 'Дозволяє продовжити запит після групування: group s by s.Age into g where g.Count() > 1...'),
            ('Відкладене виконання:', 'Групування виконується ліниво під час перебору зовнішнього циклу foreach.')
        ],
        '⚡ Дворівневий перебір:',
        'Зовнішній цикл перебирає групи (foreach group in groupedResult). Внутрішній цикл перебирає елементи групи (foreach item in group).'
    )
    add_code_panel(s34, 6.81, 1.55, 5.72, 4.25, [
        '// Query Syntax групування за віком:',
        'var groupedResult = from s in studentList',
        '                    group s by s.Age;',
        '',
        '// Дворівневий перебір сформованих груп:',
        'foreach (var ageGroup in groupedResult)',
        '{',
        '    Console.WriteLine($"Вікова група: {ageGroup.Key}");',
        '    foreach (Student s in ageGroup)',
        '    {',
        '        Console.WriteLine($"  * {s.StudentName}");',
        '    }',
        '}',
        '',
        '// -> Output:',
        '// -> Вікова група: 13 -> John',
        '// -> Вікова група: 21 -> Moin',
        '// -> Вікова група: 18 -> Bill'
    ], 'Program.cs')
    add_bottom_banner(s34, "💡 Ключ групи (Key):",
                      "Ключем може бути як просте поле (Age), так і складний складений ключ: group s by new { s.Age, s.Department }.")
    # ==========================================
    # SLIDE 35: GROUPBY У METHOD SYNTAX
    # ==========================================
    s35 = prs.slides.add_slide(blank_layout)
    add_bg(s35)
    add_header(s35, "07 · Групування (Grouping)", "Групування GroupBy у Method Syntax", 35)
    add_theory_card(s35, 0.8, 1.55, 5.72, 4.25,
        'Метод studentList.GroupBy(s => s.Age)', ACCENT_SKY,
        [
            ('KeySelector лямбда:', 's => s.Age вказує властивість, за якою розбиваються групи.'),
            ('Збереження порядку:', 'GroupBy у LINQ гарантує збереження початкового відносного порядку появи груп та елементів.'),
            ('Відкладена оцінка:', 'Групи формуються лише під час перебору в циклі foreach.'),
            ('Проекція елементів групи:', "GroupBy(s => s.Age, s => s.StudentName) дозволяє формувати групи лише з імен, уникаючи збереження всього об'єкта.")
        ],
        '⚡ Можливості проекції груп:',
        'GroupBy має перевантаження з вибором значення: GroupBy(s => s.Age, s => s.StudentName) для створення груп лише з імен.'
    )
    add_code_panel(s35, 6.81, 1.55, 5.72, 4.25, [
        '// Method Syntax групування за віком:',
        'var groupedResult = studentList.GroupBy(s => s.Age);',
        '',
        'foreach (var ageGroup in groupedResult)',
        '{',
        '    Console.WriteLine($"AgeGroup: {ageGroup.Key} (К-сть: {ageGroup.Count()})");',
        '    foreach (Student s in ageGroup)',
        '    {',
        '        Console.WriteLine($"  - {s.StudentName}");',
        '    }',
        '}',
        '',
        '// Проекція елементів групи лише в рядки імен:',
        'var namesByAge = studentList.GroupBy(s => s.Age, s => s.StudentName);',
        '',
        '// -> Output:',
        '// -> AgeGroup: 13 (К-сть: 1) -> John',
        '// -> AgeGroup: 21 (К-сть: 1) -> Moin',
        '// -> AgeGroup: 18 (К-сть: 1) -> Bill'
    ], 'Program.cs')
    add_bottom_banner(s35, "💡 Хешування ключів:",
                      "Під капотом GroupBy будує хеш-таблицю відер (buckets) за значенням ключа, тому операція виконується за лінійний час O(N).")
    # ==========================================
    # SLIDE 36: ОПЕРАТОР TOLOOKUP
    # ==========================================
    s36 = prs.slides.add_slide(blank_layout)
    add_bg(s36)
    add_header(s36, "07 · Групування (Grouping)", "Оператор ToLookup: Негайний індекс 1:N у пам'яті", 36)
    add_theory_card(s36, 0.8, 1.55, 5.72, 4.25,
        'Незмінний словник-довідник ILookup<TKey, TElement>', ACCENT_INDIGO,
        [
            ('Негайне виконання (Immediate):', 'На відміну від GroupBy, ToLookup матеріалізує структуру даних негайно при виклику.'),
            ('Індексатор за ключем O(1):', 'lookup[18] миттєво повертає всіх студентів віком 18 років за константний час O(1).'),
            ('Безпека відсутності ключа:', 'Якщо ключ не знайдено, повертає порожню послідовність Enumerable.Empty<T> (без винятків!).'),
            ('Виключно в Method Syntax:', 'ToLookup підтримується лише у Method Syntax, у Query Syntax його аналога немає.')
        ],
        '⚡ Сценарій використання:',
        'Ідеально підходить для створення довгоживучого незмінного кешу для швидкого багаторазового пошуку списків за ключем.'
    )
    add_code_panel(s36, 6.81, 1.55, 5.72, 4.25, [
        '// Негайне створення індексу Lookup:',
        'var lookupResult = studentList.ToLookup(s => s.Age);',
        '',
        '// 1. Швидкий прямий доступ до студентів віком 18 років:',
        'foreach (var s in lookupResult[18])',
        '    Console.WriteLine(s.StudentName); // John, Bill',
        '',
        '// 2. Безпечний доступ до відсутнього ключа 99 (не кидає помилки!):',
        'var emptyGroup = lookupResult[99];',
        'Console.WriteLine(emptyGroup.Count()); // Output: 0',
        '',
        '// 3. Повний перебір груп, як у GroupBy:',
        'foreach (var group in lookupResult)',
        '    Console.WriteLine($"Group: {group.Key}, Count: {group.Count()}");',
        '',
        '// -> Output: Group 13: 1, Group 21: 1, Group 18: 1'
    ], 'Program.cs')
    add_bottom_banner(s36, "💡 Головна перевага:",
                      "ToLookup є безпечною версією Dictionary<TKey, List<TElement>>, оптимізованою для зв'язків один-до-багатьох.")

    print("Part 1 built successfully (36 slides).")
